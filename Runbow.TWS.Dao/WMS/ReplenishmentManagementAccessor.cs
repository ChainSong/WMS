using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Inventory;
using Runbow.TWS.Entity.WMS.Replenishment;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.Entity.DataBaseEntity;

namespace Runbow.TWS.Dao.WMS
{
    public class ReplenishmentManagementAccessor:BaseAccessor
    {
        public GetReplenishmentDetailByConditionResponse GetReplenishmentByCondition(ReplenishmentSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = 0;
            GetReplenishmentDetailByConditionResponse response = new GetReplenishmentDetailByConditionResponse();
            string sqlWhere = this.GenGetReplenishmentWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_ReplenishmentByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.ReplenishmentCollection = ds.Tables[0].ConvertToEntityCollection<Replenishment>();
            response.ReplenishmentDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReplenishmentDetail>();
            return response;
        }

        private string GenGetReplenishmentWhere(ReplenishmentSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.ReplenishmentNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ReplenishmentNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ReplenishmentNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ReplenishmentNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ReplenishmentNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and a.ReplenishmentNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and a.ReplenishmentNumber  like '%" + SearchCondition.ReplenishmentNumber.Trim() + "%' ");
                }
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND a.CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.WarehouseID.ToString()))
            {
                sb.Append(" AND a.WarehouseID="+SearchCondition.WarehouseID);
            }
            //if (!string.IsNullOrEmpty(SearchCondition.AdjustmentType))
            //{
            //    sb.Append(" AND a.AdjustmentType='").Append(SearchCondition.AdjustmentType).Append("' ");
            //}
            if (SearchCondition.Status != 0)
            {
                sb.Append(" AND a.Status='").Append(SearchCondition.Status).Append("' ");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND a.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND a.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            //if (SearchCondition.IsHold != 0)
            //{
            //    sb.Append(" AND a.IsHold ='").Append(SearchCondition.IsHold).Append("' ");
            //}
            if (SearchCondition.Remark != null)
            {
                sb.Append(" and a.Remark  like '%" + SearchCondition.Remark.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str1))
            {
                sb.Append(" AND a.str1='").Append(SearchCondition.str1).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str2))
            {
                sb.Append(" AND a.str2='").Append(SearchCondition.str2).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {
                sb.Append(" AND a.str3='").Append(SearchCondition.str3).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str4))
            {
                sb.Append(" AND a.str4='").Append(SearchCondition.str4).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str5))
            {
                sb.Append(" AND a.str5='").Append(SearchCondition.str5).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str6))
            {
                sb.Append(" AND a.str6='").Append(SearchCondition.str6).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str7))
            {
                sb.Append(" AND a.str7='").Append(SearchCondition.str7).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str8))
            {
                sb.Append(" AND a.str8=").Append(SearchCondition.str8).Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str9))
            {
                sb.Append(" AND a.str9='").Append(SearchCondition.str9).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str10))
            {
                sb.Append(" AND a.str10='").Append(SearchCondition.str10).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str11))
            {
                sb.Append(" AND a.str11='").Append(SearchCondition.str11).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str12))
            {
                sb.Append(" AND a.str12='").Append(SearchCondition.str12).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str13))
            {
                sb.Append(" AND a.str13='").Append(SearchCondition.str13).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str14))
            {
                sb.Append(" AND a.str14='").Append(SearchCondition.str14).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str15))
            {
                sb.Append(" AND a.str15='").Append(SearchCondition.str15).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str16))
            {
                sb.Append(" AND a.str16='").Append(SearchCondition.str16).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str17))
            {
                sb.Append(" AND a.str17='").Append(SearchCondition.str17).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str18))
            {
                sb.Append(" AND b.18='").Append(SearchCondition.str18).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str19))
            {
                sb.Append(" AND b.19='").Append(SearchCondition.str19).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str20))
            {
                sb.Append(" AND b.20='").Append(SearchCondition.str20).Append("' ");
            }
            if (SearchCondition.Int1 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int1.ToString())))
            {
                sb.Append(" AND a.Int1=").Append(SearchCondition.Int1).Append(" ");
            }
            if (SearchCondition.Int2 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int2.ToString())))
            {
                sb.Append(" AND a.Int2=").Append(SearchCondition.Int2).Append(" ");
            }
            if (SearchCondition.Int3 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int3.ToString())))
            {
                sb.Append(" AND a.Int3=").Append(SearchCondition.Int3).Append(" ");
            }
            if (SearchCondition.Int4 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int4.ToString())))
            {
                sb.Append(" AND a.Int4=").Append(SearchCondition.Int4).Append(" ");
            }
            if (SearchCondition.Int5 != 0 && !(string.IsNullOrEmpty(SearchCondition.Int5.ToString())))
            {
                sb.Append(" AND a.Int5=").Append(SearchCondition.Int5).Append(" ");
            }
            return sb.ToString();
        }

        //点击新增  查看  编辑
        public ReplenishmentAndReplenishmentDetail GetReplenishmentInfos(int ID)
        {
            ReplenishmentAndReplenishmentDetail request = new ReplenishmentAndReplenishmentDetail();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int32,ID, ParameterDirection.Input),
            };
            DataSet ds = base.ExecuteDataSet("Proc_WMS_GetReplenishmentInfos", dbParams);
            request.replenishment = ds.Tables[0].ConvertToEntity<Replenishment>();
            request.replenishmentDetails = ds.Tables[1].ConvertToEntityCollection<ReplenishmentDetail>();
            return request;
        }

        public ReplenishmentAndReplenishmentDetail GenerateReplenishment(IEnumerable<ReplenishmentDetailSKUs> list, string ProjectID, string CustomerID, string CustomerName, string WarehouseID, string WarehouseName, string Remark ,string Creator)
        {
            ReplenishmentAndReplenishmentDetail request = new ReplenishmentAndReplenishmentDetail();

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ReplenishmentBySKU", conn);//Proc_WMS_AutomatedOutbound     Proc_WMS_AutomatedOutbound_Total
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SKUTable", list.Select(a => new WMSReplenishmentDetailSKUToDb(a)));//这是声明一个参数 并赋值
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;  //声明第一个参数的类型
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);//声明第二个参数  并赋值
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt; // 声明第二个参数的类型
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@WarehouseID", WarehouseID);
                    cmd.Parameters[4].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Remark", Remark);
                    cmd.Parameters[6].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[7].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Message", message);//声明第四个参数
                    cmd.Parameters[8].SqlDbType = SqlDbType.NVarChar;//声明参数的类型
                    cmd.Parameters[8].Direction = ParameterDirection.Output;//声明参数是输出类型
                    cmd.Parameters[8].Size = 8000;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);//将得到的数据 填充到DataTable中
                    message = sda.SelectCommand.Parameters["@Message"].Value.ToString();//获得数据库  out出来的参数的值 （并不是由return 而来）
                    conn.Close();
                    request.replenishment = ds.Tables[1].ConvertToEntity<Replenishment>();
                    request.replenishmentDetails = ds.Tables[2].ConvertToEntityCollection<ReplenishmentDetail>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //request.replenishment = ds.Tables[0].ConvertToEntity<Replenishment>();
            //request.replenishmentDetails = ds.Tables[1].ConvertToEntityCollection<ReplenishmentDetail>();
            return request;
        }

        public ReplenishmentAndReplenishmentDetail GenerateReplenishmentByNumber(IEnumerable<ReplenishmentDetailSKUs> list, string ProjectID, string CustomerID, string CustomerName, string WarehouseID, string WarehouseName, string Remark, string Creator,int Number)
        {
            ReplenishmentAndReplenishmentDetail request = new ReplenishmentAndReplenishmentDetail();

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ReplenishmentBySKUAndNumber", conn);//Proc_WMS_AutomatedOutbound     Proc_WMS_AutomatedOutbound_Total
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SKUTable", list.Select(a => new WMSReplenishmentDetailSKUToDb(a)));//这是声明一个参数 并赋值
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;  //声明第一个参数的类型
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);//声明第二个参数  并赋值
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt; // 声明第二个参数的类型
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@WarehouseID", WarehouseID);
                    cmd.Parameters[4].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Remark", Remark);
                    cmd.Parameters[6].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[7].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Number", Number);
                    cmd.Parameters[8].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@Message", message);//声明第四个参数
                    cmd.Parameters[9].SqlDbType = SqlDbType.NVarChar;//声明参数的类型
                    cmd.Parameters[9].Direction = ParameterDirection.Output;//声明参数是输出类型
                    cmd.Parameters[9].Size = 8000;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);//将得到的数据 填充到DataTable中
                    message = sda.SelectCommand.Parameters["@Message"].Value.ToString();//获得数据库  out出来的参数的值 （并不是由return 而来）
                    conn.Close();
                    request.replenishment = ds.Tables[1].ConvertToEntity<Replenishment>();
                    request.replenishmentDetails = ds.Tables[2].ConvertToEntityCollection<ReplenishmentDetail>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //request.replenishment = ds.Tables[0].ConvertToEntity<Replenishment>();
            //request.replenishmentDetails = ds.Tables[1].ConvertToEntityCollection<ReplenishmentDetail>();
            return request;
        }
        //取消
        public bool Cancel(int ID)
        {
            try
            {
                ReplenishmentAndReplenishmentDetail request = new ReplenishmentAndReplenishmentDetail();
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int32,ID, ParameterDirection.Input),
            };
                base.ExecuteNoQuery("Proc_CancelReplenishment", dbParams);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        //完成
        public bool Complate(int ID)
        {
            try
            {
                ReplenishmentAndReplenishmentDetail request = new ReplenishmentAndReplenishmentDetail();
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int32,ID, ParameterDirection.Input),
            };
                base.ExecuteNoQuery("Proc_ComplateReplnishment", dbParams);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #region 打印功能
        public GetReplenishmentAndReplenishmentDetailsResponse PrintReplishmentYFBLD(string rsid)
        {
            GetReplenishmentAndReplenishmentDetailsResponse response = new GetReplenishmentAndReplenishmentDetailsResponse();
            string where = this.GetId(rsid);
            string sqlWhere = this.GetRSId(rsid);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String,where,ParameterDirection.Input),
                new DbParam("@sqlWhere", DbType.String,sqlWhere,ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintReplishmentYFBLD", dbParams);

            response.ReplenishmentCollection = ds.Tables[0].ConvertToEntityCollection<Replenishment>();
            response.ReplenishmentDetailCollection = ds.Tables[1].ConvertToEntityCollection<ReplenishmentDetail>();

            return response;

        }
        public string GetId(string rsid)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(rsid))
            {
                sb.Append(" AND ID IN (").Append(rsid).Append(")");
            }

            return sb.ToString();
        }
        public string GetRSId(string rsid)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(rsid))
            {
                sb.Append(" AND RSID IN (").Append(rsid).Append(")");
            }

            return sb.ToString();
        }
        #endregion
    }
}
