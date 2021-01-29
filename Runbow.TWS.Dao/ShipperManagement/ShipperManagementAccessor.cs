using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.POD.Hilti;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.ShipperManagement;

namespace Runbow.TWS.Dao
{
    public class ShipperManagementAccessor : BaseAccessor
    {
        public CRMShipperInfo GetCRMShipperInfoByID(long ID)
        {
            CRMShipperInfo shipperInfo = new CRMShipperInfo();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
            };

            DataSet ds = base.ExecuteDataSet("Proc_GetCRMShipperByID", dbParams);
            if (ds != null)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    shipperInfo.CRMShipper = ds.Tables[0].ConvertToEntity<CRMShipper>();
                }
                else
                {
                    return shipperInfo;
                }

                if (ds.Tables[1] != null)
                {
                    shipperInfo.CRMShipperCooperationCollection = ds.Tables[1].ConvertToEntityCollection<CRMShipperCooperation>();
                }

                if (ds.Tables[2] != null)
                {
                    shipperInfo.CRMShipperTransportationLineCollection = ds.Tables[2].ConvertToEntityCollection<CRMShipperTransportationLine>();
                }

                if (ds.Tables[3] != null)
                {
                    shipperInfo.CRMShipperTerminalInfoCollection = ds.Tables[3].ConvertToEntityCollection<CRMShipperTerminalInfo>();
                }
            }

            return shipperInfo;
        }

        public IEnumerable<CRMShipper> GetCRMShippersByConditionNoPaging(CRMShipperSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetCRMShipperWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetCRMShipperByConditionNoPaging", dbParams).ConvertToEntityCollection<CRMShipper>();
        }

        public IEnumerable<CRMShipper> GetCRMShippersByCondition(CRMShipperSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenGetCRMShipperWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_GetCRMShipperByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMShipper>();
        }

        public IEnumerable<CRMShipperCooperation> GetCRMShipperCooperationsByCRMShipperID(long CRMShipperID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CRMShipperID", DbType.Int64, CRMShipperID, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_GetCRMShipperCooperationByCRMShipperID", dbParams).ConvertToEntityCollection<CRMShipperCooperation>();
        }

        public IEnumerable<CRMShipperTransportationLine> GetCRMShipperTransportationLinesByCRMShipperID(long CRMShipperID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CRMShipperID", DbType.Int64, CRMShipperID, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_GetCRMShipperTransportationLineByCRMShipperID", dbParams).ConvertToEntityCollection<CRMShipperTransportationLine>();
        }

        public IEnumerable<CRMShipperTerminalInfo> GetCRMShipperTerminalInfosByCRMShipperID(long CRMShipperID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CRMShipperID", DbType.Int64, CRMShipperID, ParameterDirection.Input)
            };

            return base.ExecuteDataTable("Proc_GetCRMShipperTerminalInfosByCRMShipperID", dbParams).ConvertToEntityCollection<CRMShipperTerminalInfo>();
        }

        public void DeleteCRMShipper(long ID)
        {
            DbParam[] Db = {
                           new DbParam("@ID",DbType.Int64,ID,ParameterDirection.Input)
                        };

            base.ExecuteNoQuery("Proc_DeleteCRMShipper", Db);
        }

        public void DeleteCRMShipperTransportationLine(long ID)
        {
            DbParam[] Db = {
                           new DbParam("@ID",DbType.Int64,ID,ParameterDirection.Input)
                        };

            base.ExecuteNoQuery("Proc_DeleteCRMShipperTransportationLine", Db);
        }

        public void DeleteCRMShipperTerminalInfo(long ID)
        {
            DbParam[] Db = {
                           new DbParam("@ID",DbType.Int64,ID,ParameterDirection.Input)
                        };

            base.ExecuteNoQuery("Proc_DeleteCRMShipperTerminalInfo", Db);
        }

        public void DeleteCRMShipperCooperation(long ID)
        {
            DbParam[] Db = {
                           new DbParam("@ID",DbType.Int64,ID,ParameterDirection.Input)
                        };

            base.ExecuteNoQuery("Proc_DeleteCRMShipperCooperation", Db);
        }

        public IEnumerable<long> AddOrUpdateCRMShippers(IEnumerable<CRMShipper> CRMShippers)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddOrUpdateCRMShipper", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Shipper", CRMShippers.Select(c => new CRMShipperToDb(c)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                IList<long> IDs = new List<long>();
                while (reader.Read())
                {
                    IDs.Add(reader.IsDBNull(0) ? 0 : reader.GetInt64(0));
                }

                return IDs;
            }
        }

        public IEnumerable<long> AddOrUpdateCRMShipperCooperations(IEnumerable<CRMShipperCooperation> CRMShipperCooperations)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddOrUpdateCRMShipperCooperation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShipperCopperation", CRMShipperCooperations.Select(c => new CRMShipperCoopperationToDb(c)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                IList<long> IDs = new List<long>();
                while (reader.Read())
                {
                    IDs.Add(reader.IsDBNull(0) ? 0 : reader.GetInt64(0));
                }

                return IDs;
            }
        }

        public IEnumerable<CRMShipperTransportationLine> AddOrUpdateCRMShipperTransportationLines(IEnumerable<CRMShipperTransportationLine> CRMShipperTransportationLines)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_AddOrUpdateCRMShipperTransportationLine", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShipperTransportationLine", CRMShipperTransportationLines.Select(c => new CRMShipperTransportationLineToDb(c)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                return dtable.ConvertToEntityCollection<CRMShipperTransportationLine>();

            }
        }

        public IEnumerable<CRMShipperTerminalInfo> AddOrUpdateCRMShipperTerminalInfos(IEnumerable<CRMShipperTerminalInfo> CRMShipperTerminalInfos)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataTable dtable = new DataTable();
                SqlCommand cmd = new SqlCommand("Proc_AddOrUpdateCRMShipperTerminalInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TerminalInfo", CRMShipperTerminalInfos.Select(c => new CRMShipperTerminalInfoToDb(c)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();

                SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                Adp.Fill(dtable);
                return dtable.ConvertToEntityCollection<CRMShipperTerminalInfo>();

            }
        }

        private string GenGetCRMShipperWhere(CRMShipperSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.Name))
            {
                sb.Append(" AND a.Name like '%").Append(SearchCondition.Name).Append("%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.Attribution))
            {
                sb.Append(" AND a.Attribution='").Append(SearchCondition.Attribution).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.RegisteredCapitalRange))
            {
                sb.Append(" AND a.RegisteredCapitalRange='").Append(SearchCondition.RegisteredCapitalRange).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.AnnualTurnoverRange))
            {
                sb.Append(" AND a.AnnualTurnoverRange='").Append(SearchCondition.AnnualTurnoverRange).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.TrunkOfVehicleType))
            {
                sb.Append(" AND a.TrunkOfVehicleType like '%").Append(SearchCondition.TrunkOfVehicleType).Append("%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.FrequencyOfDeparture))
            {
                sb.Append(" AND a.FrequencyOfDeparture='").Append(SearchCondition.FrequencyOfDeparture).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.TrunkOfVehicleRange))
            {
                sb.Append(" AND a.TrunkOfVehicleRange='").Append(SearchCondition.TrunkOfVehicleRange).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.DeliveryOfVehicleRange))
            {
                sb.Append(" AND a.DeliveryOfVehicleRange='").Append(SearchCondition.DeliveryOfVehicleRange).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.WarehouseAreaRange))
            {
                sb.Append(" AND a.WarehouseAreaRange='").Append(SearchCondition.WarehouseAreaRange).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.Recommended))
            {
                sb.Append(" AND a.Recommended='").Append(SearchCondition.Recommended).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.StartPlaceIDs))
            {
                StringBuilder startPlaceSB = new StringBuilder();
                using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
                {
                    DataTable dtable = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_GetReginAndSunRegionsByRegionIDs", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EndCityIDs", SearchCondition.StartPlaceIDs.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                    Adp.Fill(dtable);
                    for (int i = 0; i < dtable.Rows.Count; i++)
                    {
                        startPlaceSB.Append(dtable.Rows[i][0].ToString()).Append(",");
                    }

                    startPlaceSB.Remove(startPlaceSB.Length - 1, 1);

                    sb.Append(" AND b.StartCityID IN (").Append(startPlaceSB.ToString()).Append(") ");
                }  
            }

            if (!string.IsNullOrEmpty(SearchCondition.EndPlaceIDs))
            {
                StringBuilder endPlaceSB = new StringBuilder();
                using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
                {
                    DataTable dtable = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_GetReginAndSunRegionsByRegionIDs", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EndCityIDs", SearchCondition.EndPlaceIDs.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    SqlDataAdapter Adp = new SqlDataAdapter(cmd);
                    Adp.Fill(dtable);
                    for (int i = 0; i < dtable.Rows.Count; i++)
                    {
                        endPlaceSB.Append(dtable.Rows[i][0].ToString()).Append(",");
                    }

                    endPlaceSB.Remove(endPlaceSB.Length - 1, 1);

                    sb.Append(" AND b.EndCityID IN (").Append(endPlaceSB.ToString()).Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(SearchCondition.CoverRegionIDs))
            {
                var CoverRegionIDs = SearchCondition.CoverRegionIDs.Split(',');
                sb.Append(" AND b.CoverRegionID IN (");
                CoverRegionIDs.Each((i, s) => { sb.Append(s).Append(","); });
                sb.Remove(sb.Length - 1, 1);
                sb.Append(") ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.TransportMode))
            {
                var transportModes = SearchCondition.TransportMode.Split('|');
                sb.Append(" AND (");
                transportModes.Each((i, t) => {
                    sb.Append("a.TransportMode like '%").Append(t).Append("%' ").Append(" OR ");
                });
                sb.Remove(sb.Length - 3, 3);
                sb.Append(") ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.ProductType))
            {
                var productTypes = SearchCondition.ProductType.Split('|');
                sb.Append(" AND (");
                productTypes.Each((i, t) =>
                {
                    sb.Append("c.Str7 like '%").Append(t).Append("%' ").Append(" OR ");
                });
                sb.Remove(sb.Length - 3, 3);
                sb.Append(") ");
            }
            
            if (!string.IsNullOrEmpty(SearchCondition.KeyWord))
            {
                sb.Append(" AND (").Append("c.Name like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR a.Name like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR c.Remark like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR c.Str1 like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR c.Str2 like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR c.Str3 like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR c.Str4 like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR c.Str5 like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR c.Str6 like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR c.Str7 like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR a.AnnualTurnover like '%").Append(SearchCondition.KeyWord.Trim())
                  .Append("%' OR a.Remark like '%").Append(SearchCondition.KeyWord.Trim()).Append("%')");
            }

            if (!string.IsNullOrEmpty(SearchCondition.PartnerShipType))
            {
                sb.Append(" AND a.PartnershipTypes = '").Append(SearchCondition.PartnerShipType).Append("' ");
            }

            return sb.ToString();
        }

        //承运商车辆管理 根据SID查询当前承运商已有车辆
        public IEnumerable<ShipperToVehicle> GetShipperToVehicle(long SID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SID", DbType.Int64, SID, ParameterDirection.Input),
                              };
            return this.ExecuteDataTable("Proc_GetShipperToVehicle", dbParams).ConvertToEntityCollection<ShipperToVehicle>();
        }
        //public IEnumerable<ShipperToVehicle> GetShipperToVehicle()
        //{
        //    return this.ExecuteDataTable("Proc_GetShipperToVehicle").ConvertToEntityCollection<ShipperToVehicle>();
        //}

        //所有的承运商
        public IEnumerable<CRMShipper> GetAllShippers()
        {
            return this.ExecuteDataTable("Proc_GetAllShippers").ConvertToEntityCollection<CRMShipper>();
        }

        //批量导入承运商
        public bool InsertCRMShipperExecl(IEnumerable<InsertShipperExcel> crmshipper )
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_InsertCRMShipperExecl", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Shipper", crmshipper.Select(p => new InsertShipperToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                //cmd.Parameters.AddWithValue("@UserName", UserName);
                //cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters[1].Size = 50;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                conn.Close();
                if (ds.Tables.Count>0)
                {
                    return true;
                }
                return false;

            }
        }
    }
}
