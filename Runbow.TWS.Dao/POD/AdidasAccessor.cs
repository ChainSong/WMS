using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.MessageContracts.POD.AKZO;
using System.Data;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts;
using System.Data.SqlClient;
using Runbow.TWS.MessageContracts.POD.Adidas;
using Runbow.TWS.Entity.POD;


namespace Runbow.TWS.Dao.POD
{
    public class AdidasAccessor : BaseAccessor
    {
        /// <summary>
        /// 增加扫描 数据
        /// </summary>
        /// <param name="scanInfoList"></param>
        /// <returns></returns>
        public IEnumerable<ScanInfo> AddScanDatas(IEnumerable<ScanInfo> scanInfoList,out string repeatPOD,out string shipperError) 
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_Adidas_ImportScanData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsertData", scanInfoList.Select(p => new ScanToDb(p)));
                cmd.Parameters.Add("@repeatPOD",SqlDbType.NVarChar,1000);
                cmd.Parameters.Add("@shipperError", SqlDbType.NVarChar, 1000);

                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                conn.Open();
                IList<ScanInfo> returnList = new List<ScanInfo>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    returnList.Add(
                        new ScanInfo()
                        {
                            ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                            CustomerOrderNumber = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            BoxNumber = reader.IsDBNull(2) ? 0 : reader.GetInt64(2),
                            ScanBoxNumber = reader.IsDBNull(3) ? 0 : reader.GetInt64(3),
                            TrailerNo = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            PlateNumber = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            ShipperID = reader.IsDBNull(6) ? 0 : reader.GetInt64(6),
                            Shipper=reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            CloseFlag = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                            CompleteFlag = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                            Operator = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                            OperateTime = reader.IsDBNull(11) ? DateTime.Now : reader.GetDateTime(11),
                            Creater = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                            CreateTime = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13),
                            Modifier = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                            ModifyTime = reader.IsDBNull(15) ? DateTime.Now : reader.GetDateTime(15),
                            Remark = reader.IsDBNull(16) ? string.Empty : reader.GetString(16)
                        });
                }
                reader.NextResult();
                repeatPOD = cmd.Parameters[1].Value.ToString();    //重复的运单
                shipperError = cmd.Parameters[2].Value.ToString(); //错误的物流公司
                conn.Close();
                return returnList;
            }
        }


        /// <summary>
        /// 查询扫描数据
        /// </summary>
        public IEnumerable<ScanInfo> GetQueryScanData(AdidasScanDataSearchCondition SearchCondition, int PageIndex, int PageSize,out int PageCount)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                ///新建一个datatable
                DataTable dt = new DataTable();
                dt.Columns.Add("CustomerOrderNumber");
                dt.Columns.Add("TrailerNo");
                dt.Columns.Add("PlateNumber");
                dt.Columns.Add("Shipper");
                dt.Columns.Add("CloseFlag");
                dt.Columns.Add("CompleteFlag");
                dt.Columns.Add("CreateTime_Start");
                dt.Columns.Add("CreateTime_End");

                dt.Rows.Add(SearchCondition.CustomerOrderNumber, SearchCondition.TrailerNo,
                    SearchCondition.PlateNumber, SearchCondition.Shipper, SearchCondition.CloseFlag, SearchCondition.CompleteFlag,
                    SearchCondition.CreateTime_Start == new DateTime(0001, 1, 1) ? new DateTime(1753, 1, 1) : SearchCondition.CreateTime_Start
                  , SearchCondition.CreateTime_End == new DateTime(0001, 1, 1) ? new DateTime(1753, 1, 1) : SearchCondition.CreateTime_End);

                

                SqlCommand cmd = new SqlCommand("Proc_Adidas_QueryScanData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsertData", dt);
                cmd.Parameters.AddWithValue("@pageIndex", PageIndex);
                cmd.Parameters.AddWithValue("@pageSize", PageSize);
                cmd.Parameters.AddWithValue("@pageCount", PageCount=0);

                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                cmd.Parameters[1].Direction = ParameterDirection.Input;
                cmd.Parameters[2].SqlDbType = SqlDbType.Int;
                cmd.Parameters[2].Direction = ParameterDirection.Input;
                cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                conn.Open();
                IList<ScanInfo> returnList = new List<ScanInfo>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    returnList.Add(
                        new ScanInfo()
                        {
                            ID = reader.GetInt64(1),
                            CustomerOrderNumber = reader.GetString(2),
                            BoxNumber = reader.GetInt64(3),
                            ScanBoxNumber = reader.GetInt64(4),
                            TrailerNo=reader.IsDBNull(5) ? "" : reader.GetString(5),
                            PlateNumber = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            ShipperID=reader.GetInt64(7),
                            CloseFlag=reader.GetInt32(8),
                            CompleteFlag=reader.GetInt32(9),
                            Operator=reader.IsDBNull(10) ? "" : reader.GetString(10),
                            OperateTime=reader.IsDBNull(11) ? DateTime.Today: reader.GetDateTime(11),
                            Creater=reader.IsDBNull(12) ? "" :reader.GetString(12),
                            CreateTime=reader.IsDBNull(13)? DateTime.Today: reader.GetDateTime(13),
                         
                            
                            Shipper=reader.GetString(17)
                        });
                }
                reader.NextResult();
                PageCount =Convert.ToInt32(cmd.Parameters[3].Value);    //重复的运单

                return returnList;
            }
        }

        /// <summary>
        /// 关闭标记置为1
        /// </summary>
        /// <param name="POD"></param>
        /// <returns></returns>
        public bool ClosePOD(string POD) 
        {
            try
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@POD", DbType.String,POD, ParameterDirection.Input)
                };
                base.ExecuteNoQuery("Proc_Adidas_ClosePOD", dbParams);
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }


        public bool delBAF(int id)
        {
            try
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@id", DbType.Int32,id, ParameterDirection.Input)
                };
                base.ExecuteNoQuery("Proc_DelBAF", dbParams);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///获取油价表
        /// </summary>
        public IEnumerable<BAFPriceInfo> GetABFPrice(string StartTime, string EndTime, int? PageIndex,int? PageSize, out int PageCount)
        {
            string sqlWhere = "";
            if (!string.IsNullOrEmpty(StartTime))
            {
                sqlWhere += " and BAFStartTime>='" + StartTime+"'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sqlWhere += " and BAFEndTime<='" + EndTime+"'";
            }
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]
             {
                 new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                 new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                 new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                 new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
             };
            DataTable dt = this.ExecuteDataTable("Proc_UpdateBAFPrice", dbParams);
            PageCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<BAFPriceInfo>();
         
        }



        /// <summary>
        ///添加油价
        /// </summary>
        public bool AddABFPrice(IEnumerable<BAFPriceInfo> BAF)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_AddBAFPrict", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BAF", BAF.Select(p => new BAFPriceInfoToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                //cmd.Parameters.AddWithValue("@message", message);
                //cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                //cmd.Parameters[1].Size = 10;
                //cmd.Parameters[1].Direction = ParameterDirection.Output;
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                
                //message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (Convert.ToInt32(dt.Rows[0][0])> 0)
                {
                    return true;
                }

                //if (ds.Tables[0].Rows.Count>)
                //return ds.Tables[0].ConvertToEntityCollection<BAFPriceInfo>();

                return false;
            }
        }
    }
}
