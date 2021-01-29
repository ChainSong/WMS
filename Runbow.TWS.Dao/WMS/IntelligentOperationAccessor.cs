using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Instructions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using System.Threading.Tasks;
using Runbow.TWS.MessageContracts.WMS.IntelligentWarehouse;

namespace Runbow.TWS.Dao.WMS
{
    public class IntelligentOperationAccessor : BaseAccessor
    {
        public ShelvesPanelResponse ShelvesPanel(string id, string WorkStationId)
        {
            ShelvesPanelResponse response = new ShelvesPanelResponse();
            //long IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Id", DbType.String, id, ParameterDirection.Input),
                new DbParam("@WorkStationId", DbType.String, WorkStationId, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetShelvesPanel", dbParams);
            response.shelvesPanel = ds.Tables[0].ConvertToEntityCollection<ShelvesPanel>();
            response.instructions = ds.Tables[1].ConvertToEntityCollection<Instructions>();
            return response;
        }

        public ShelvesPanelResponse ShelvesPanel_Receipt(string id, string WorkStationId)
        {
            ShelvesPanelResponse response = new ShelvesPanelResponse();
            //long IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Id", DbType.String, id, ParameterDirection.Input),
                new DbParam("@WorkStationId", DbType.String, WorkStationId, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetShelvesPanel_Receipt", dbParams);
            response.shelvesPanel = ds.Tables[0].ConvertToEntityCollection<ShelvesPanel>();
            response.instructions = ds.Tables[1].ConvertToEntityCollection<Instructions>();
            return response;
        }

        public ShelvesPanelResponse SubmitData(long ID, long RePickWallDetailId, decimal ActualQty)
        {
            ShelvesPanelResponse response = new ShelvesPanelResponse();
            //long IDs = ReleatedDetailID;    @
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input),
                new DbParam("@RePickWallDetailId", DbType.Int64, RePickWallDetailId, ParameterDirection.Input),
                new DbParam("@ActualQty", DbType.Decimal, ActualQty, ParameterDirection.Input),
                new DbParam("@message", DbType.String, "", ParameterDirection.Input)
            };
            string s = (string)dbParams[3].Value;
            DataTable dt = this.ExecuteDataTable("Proc_WMS_InstructionsAddActualQty", dbParams);
            response.instructions = dt.ConvertToEntityCollection<Instructions>();
            return response;
        }

        public ShelvesPanelResponse SubmitData_Receipt(long ID, decimal ActualQty)
        {
            ShelvesPanelResponse response = new ShelvesPanelResponse();
            //long IDs = ReleatedDetailID;    @
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input),
                new DbParam("@ActualQty", DbType.Decimal, ActualQty, ParameterDirection.Input),
                new DbParam("@message", DbType.String, "", ParameterDirection.Input)
            };
            string s = (string)dbParams[2].Value;
            DataTable dt = this.ExecuteDataTable("Proc_WMS_InstructionsAddActualQty_Receipt", dbParams);
            response.instructions = dt.ConvertToEntityCollection<Instructions>();
            return response;
        }

        public ShelvesPanelResponse GetPickUpGoodsWall(long WorkStationId)
        {
            ShelvesPanelResponse response = new ShelvesPanelResponse();
            //long IDs = ReleatedDetailID;    @
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@WorkStationId", DbType.Int64, WorkStationId, ParameterDirection.Input),                   
                new DbParam("@message", DbType.String, "", ParameterDirection.Input)
            };
            string s = (string)dbParams[1].Value;
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPickUpGoodsWall", dbParams);
            response.pickUpGoodsWall = ds.Tables[0].ConvertToEntityCollection<PickUpGoodsWall>();
            //response.mapping = ds.Tables[1].ConvertToEntityCollection<Instruction_Order_Mapping>();
            return response;
        }

        public PickUpGoodsManagementResponse GetGoodsManagement(PickUpGoodsManagementRequest request)             //, out int RowCount
        {
            PickUpGoodsManagementResponse response = new PickUpGoodsManagementResponse();
            string SqlWhere = "";
            if (request != null)
            {
                SqlWhere = GetGoodsManagementSqlWhere(request);
            }
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, SqlWhere, ParameterDirection.Input)   //,
                  //new DbParam("@RowCount", DbType.Int32, 0, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_PickUpGoodsManagement", dbParams);
            //RowCount = (int)dbParams[1].Value;

            response.instructions = dt.ConvertToEntityCollection<Instructions>();
            //response.mapping = ds.Tables[1].ConvertToEntityCollection<Instruction_Order_Mapping>();
            return response;
        }

        private string GetGoodsManagementSqlWhere(PickUpGoodsManagementRequest request)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(request.WorkStationId))
            {
                sb.Append(" and OperatingArea = '" + request.WorkStationId + "'");
            }
            if (request.Ststus == 1)
            {
                sb.Append(" and QtyExcepted= QtyActual ");
            }
            if (request.Ststus == 2)
            {
                sb.Append(" and QtyExcepted <> QtyActual ");
            }
            if (!string.IsNullOrEmpty(request.CustomerName))
            {
                IEnumerable<string> CustomerName = Enumerable.Empty<string>();
                if (request.CustomerName.IndexOf("\n") > 0)
                {
                    CustomerName = request.CustomerName.Split('\n').Select(s => { return s.Trim(); });
                }
                if (request.CustomerName.IndexOf(',') > 0)
                {
                    CustomerName = request.CustomerName.Split(',').Select(s => { return s.Trim(); });
                }

                if (CustomerName != null && CustomerName.Any())
                {
                    CustomerName = CustomerName.Where(c => !string.IsNullOrEmpty(c));
                }

                if (CustomerName != null && CustomerName.Any())
                {
                    sb.Append(" and CustomerName in ( ");
                    foreach (string s in CustomerName)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and CustomerName  like '%" + request.CustomerName.Trim() + "%' ");
                }

                //sb.Append("and CustomerName = '" + request.CustomerName + "'");
            }
            if (!string.IsNullOrEmpty(request.Warehouse))
            {
                IEnumerable<string> Warehouse = Enumerable.Empty<string>();
                if (request.Warehouse.IndexOf("\n") > 0)
                {
                    Warehouse = request.Warehouse.Split('\n').Select(s => { return s.Trim(); });
                }
                if (request.Warehouse.IndexOf(',') > 0)
                {
                    Warehouse = request.Warehouse.Split(',').Select(s => { return s.Trim(); });
                }

                if (Warehouse != null && Warehouse.Any())
                {
                    Warehouse = Warehouse.Where(c => !string.IsNullOrEmpty(c));
                }

                if (Warehouse != null && Warehouse.Any())
                {
                    sb.Append(" and Warehouse in ( ");
                    foreach (string s in Warehouse)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and Warehouse  like '%" + request.Warehouse.Trim() + "%' ");
                }
                //sb.Append("and Warehouse = '" + request.Warehouse + "'");
            }
            if (!string.IsNullOrEmpty(request.OrderNumber))
            {
                sb.Append("and OrderNumber = '" + request.OrderNumber + "'");
            }
            return sb.ToString();
        }
    }
}
