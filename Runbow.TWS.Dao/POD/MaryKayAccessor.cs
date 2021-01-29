using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.POD.MaryKay;
using System.Data.SqlClient;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Dao.POD
{
    public class MaryKayAccessor : BaseAccessor
    {
        public DataTable GetMaryKayGetOrderIssued(string SqlWhere, int? PageIndex, int PageSize, out int RowCount)
        {

            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input),
                            new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                            new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                            new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output)
                          };

            DataTable table = base.ExecuteDataTable("Proc_MaryKay_GetOrderIssued", dbParams);
            RowCount = (int)dbParams[3].Value;
            return table;

        }

        public DataTable GetMaryKayGetTrack(string SqlWhere, int? PageIndex, int PageSize, out int RowCount)
        {

            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input),
                            new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                            new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                            new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output)
                          };

            DataTable table = base.ExecuteDataTable("Proc_MaryKay_TrackInfo", dbParams);
            RowCount = (int)dbParams[3].Value;
            return table;

        }

        public DataTable GetMaryKayTrackListInfoByIDS(string IDS)
        {

            DbParam[] dbParams = {
                            new DbParam("@IDS",DbType.String,IDS,ParameterDirection.Input),
                           
                          };

            DataTable table = base.ExecuteDataTable("Proc_MaryKay_UploadMKTrackInfo", dbParams);
           
            return table;

        }

        public DataTable GetMaryKayTrackExport(string SqlWhere)
        {

            DbParam[] dbParams = {
                            new DbParam("@SqlWhere",DbType.String,SqlWhere,ParameterDirection.Input)
                           
                          };

            DataTable table = base.ExecuteDataTable("Proc_MaryKay_TrackInfoExport", dbParams);
           
            return table;

        }

        public Pod GetOrderNoIssuedInfoByID(int ID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input),
            };

            return base.ExecuteDataTable("Proc_MaryKay_GetOrderNoIssuedInfoByID", dbParams).ConvertToEntity<Pod>();
        }

        public void UpdateOrderNoIsSuedStatus(string OrderNoIsSuedStatus,int ID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input),
                new DbParam("@OrderNoIsSuedStatus", DbType.String, OrderNoIsSuedStatus, ParameterDirection.Input)
            };

            base.ExecuteNoQuery("Proc_MaryKay_UpdateOrderNoIsSuedStatus", dbParams);
        }

        public DataTable GetHttpYD(int ID)
        {
            DbParam[] dbParams = {
                            new DbParam("@ID",DbType.Int64,ID,ParameterDirection.Input)
                          };

            DataTable table = base.ExecuteDataTable("Proc_MaryKay_GetHttpYD", dbParams);
            return table;
        }

        public DataTable GetHttpYZ(int ID)
        {
            DbParam[] dbParams = {
                            new DbParam("@ID",DbType.Int64,ID,ParameterDirection.Input)
                          };

            DataTable table = base.ExecuteDataTable("Proc_MaryKay_GetHttpYZ", dbParams);
            return table;
        }

        public void AddMaryKayInterfaceLog(MaryKay_InterfaceLog InterfaceLog)
        {

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@LogUID", DbType.String,Guid.NewGuid().ToString(), ParameterDirection.Input),
                new DbParam("@LogDetails", DbType.String, InterfaceLog.LogDetails, ParameterDirection.Input),
                new DbParam("@EventType", DbType.String, InterfaceLog.EventType, ParameterDirection.Input),
                new DbParam("@UserDef1", DbType.String, InterfaceLog.UserDef1, ParameterDirection.Input),
                new DbParam("@UserDef2", DbType.String, InterfaceLog.UserDef2, ParameterDirection.Input),
                new DbParam("@UserDef3", DbType.String, InterfaceLog.UserDef3, ParameterDirection.Input),
                new DbParam("@UserDef4", DbType.String, InterfaceLog.UserDef4, ParameterDirection.Input),
                new DbParam("@UserDef5", DbType.String, InterfaceLog.UserDef5, ParameterDirection.Input)
            };

            base.ExecuteNoQuery("Proc_Add_MaryKay_InterfaceLog", dbParams);
        }

        public bool DeleteTrackInfoByID(string ID)
        {
            try
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.String, ID, ParameterDirection.Input),
                
            };

                base.ExecuteNoQuery("Proc_MaryKay_DeleteTrackByID", dbParams);
                return true;
            }
            catch(Exception EX)
            {
                return false;
            }
        }

        public void UpdateIsNormalByID(string UpLoadMKStatus, int ID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input),
                new DbParam("@UpLoadMKStatus", DbType.String, UpLoadMKStatus, ParameterDirection.Input)
               
            };

            base.ExecuteNoQuery("Proc_MaryKay_UpdateIsNormalByID", dbParams);
        }

        public void UpdatePODStatusByCustomerOrderNumber(string CustomerOrderNumber, int PodStatusID, string PODStatusName)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerOrderNumber", DbType.String, CustomerOrderNumber, ParameterDirection.Input),
                new DbParam("@PODStatusID", DbType.Int64, PodStatusID, ParameterDirection.Input),
                new DbParam("@PODStatusName", DbType.String, PODStatusName, ParameterDirection.Input)
               
            };

            base.ExecuteNoQuery("Proc_MaryKay_UpdatePODStatusByCustomerOrderNumber", dbParams);
        }

        public DataTable GetYundaOrderNoInfo()
        {
            DataTable table = base.ExecuteDataTable("Proc_MaryKay_GetYUNDAOrderNoInfo", null);
            return table;
        }

        public void AddYUNDATrackInfo(string CustomerOrderNumber, string Creator, DateTime? CreateTime, string TrackInfo, string TrackComment, string ResponsibiltyOwner, DateTime? TrackTime, string SignName)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerOrderNumber", DbType.String, CustomerOrderNumber, ParameterDirection.Input),
                new DbParam("@Creator", DbType.String, Creator, ParameterDirection.Input),
                new DbParam("@CreateTime", DbType.DateTime, CreateTime, ParameterDirection.Input),
                new DbParam("@TrackInfo", DbType.String, TrackInfo, ParameterDirection.Input),
                new DbParam("@TrackComment", DbType.String, TrackComment, ParameterDirection.Input),
                new DbParam("@ResponsibiltyOwner", DbType.String, ResponsibiltyOwner, ParameterDirection.Input),
                new DbParam("@TrackTime", DbType.DateTime, TrackTime, ParameterDirection.Input),
                new DbParam("@SignName", DbType.String, SignName, ParameterDirection.Input),
            };

            base.ExecuteNoQuery("Proc_MaryKay_AddYUNDATrackInfo", dbParams);
        }

        /// <summary>
        /// 获取玫琳凯承运商
        /// </summary>
        public IEnumerable<DMS_Shipper> GetMaryKayShipper()
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor.DMSSqlConnection))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID,colCode,colName,colEnglishName FROM dbo.tbl_Shipper", conn);
                cmd.CommandType = CommandType.Text;
                //cmd.Parameters.AddWithValue("@ids", names);
                //cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                conn.Open();
                IList<DMS_Shipper> returnShippe = new List<DMS_Shipper>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnShippe.Add(
                        new DMS_Shipper()
                        {
                            ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            colCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            colName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            colEnglishName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        });
                }

                return returnShippe;
            }
        }


        /// <summary>
        /// 获取玫琳凯省份信息
        /// </summary>
        public IEnumerable<DMS_Province> GetMaryKayProvince()
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor.DMSSqlConnection))
            {
                SqlCommand cmd = new SqlCommand("SELECT colCode,colName,colDesc FROM tbl_Province WHERE colEnable=1", conn);
                cmd.CommandType = CommandType.Text;
                //cmd.Parameters.AddWithValue("@ids", names);
                //cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                conn.Open();
                IList<DMS_Province> returnShippe = new List<DMS_Province>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnShippe.Add(
                        new DMS_Province()
                        {
                            colCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            colName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            colDesc = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                        });
                }

                return returnShippe;
            }
        }


        /// <summary>
        /// 查询DMS玫琳凯运单
        /// </summary>
        public IEnumerable<DMS_POD> QueryDMSPOD(QueryDMSPODRequest SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = " WHERE Status='已发运' ";//this.GenQueryAttachmentSql(SearchCondition, Customers);
            int tempRowCount = 0;

            using (SqlConnection conn = new SqlConnection(BaseAccessor.DMSSqlConnection))
            {
                SqlCommand cmd = new SqlCommand("Proc_QueryPOD", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Where", sqlWhere);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@PageSize", PageSize);
                cmd.Parameters[2].SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@RowCount", tempRowCount);
                cmd.Parameters[3].SqlDbType = SqlDbType.Int;    
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    RowCount = Convert.ToInt32(cmd.Parameters[3].Value);
                    cmd.Parameters.Clear();
                    return ds.Tables[0].ConvertToEntityCollection<DMS_POD>();
                }
            }
        }

    }
}
