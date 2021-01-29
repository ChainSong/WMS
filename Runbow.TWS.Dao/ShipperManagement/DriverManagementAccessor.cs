using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;
using System.Data.SqlClient;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;
using Runbow.TWS.Entity.ShipperManagement;

namespace Runbow.TWS.Dao.ShipperManagement
{
    public class DriverManagementAccessor : BaseAccessor
    {
        //查询
        public IEnumerable<CRMDriver> GetCRMDriverByConditionNoPaging(CRMDriverSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetCRMDriverWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
           new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
           };
            return this.ExecuteDataTable("Proc_GetCRMDriverByConditionNoPaging", dbParams).ConvertToEntityCollection<CRMDriver>();
        }


        //查询及页数。。
        public IEnumerable<CRMDriver> GetCRMDriverByCondition(CRMDriverSearchCondition Driver, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetCRMDriverWhere(Driver);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_GetCRMDriverByCondition", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMDriver>();
        }
        //手机端的查询    条件是或者
        public IEnumerable<CRMDriver> GetCRMDriverBykeyWord(string keyword, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetCRMDriverWheres(keyword);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_GetCRMDriverByCondition", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMDriver>();
        }

        /// <summary>
        /// 通过车辆ID查询该车辆由那几位司机驾驶
        /// </summary>
        /// <param name="shipperid"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<CRMDriver> GetVehicleMappingDriverVID(string vid, int PageIndex, int PageSize, out int RowCount)
        {
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@VID", DbType.String, vid, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetVehicleMappingDriverVID", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMDriver>();
        }

        /// <summary>
        /// 通过车辆id查询该车由哪几位司机驾驶待条件分页
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keyword"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<CRMDriver> GetVehicleMappingDriverInfoByVIDandkeyWord(string id, string keyword, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetCRMDriverWheres(keyword);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@VID", DbType.String, id, ParameterDirection.Input),
                new DbParam("@where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetVehicleMappingDriverInfoByVIDandkeyWord", dbParams);
            RowCount = (int)dbParams[4].Value;
            return dt.ConvertToEntityCollection<CRMDriver>();
        }


        /// <summary>
        /// 通过shipperID查询当前承运商有哪些司机服务
        /// </summary>
        /// <param name="shipperid"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<CRMDriver> GetShippingMappingDriverSID(string Sid, int PageIndex, int PageSize, out int RowCount)
        {
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SID", DbType.String, Sid, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetShippingMappingDriverSID", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMDriver>();
        }

        /// <summary>
        /// 通过shipperid查询该承运商由哪几位司机驾驶待条件分页
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keyword"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<CRMDriver> GetShipperMappingDriverInfoBySIDandkeyWord(string id, string keyword, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetCRMDriverWheres(keyword);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SID", DbType.String, id, ParameterDirection.Input),
                new DbParam("@where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetShipperMappingDriverInfoBySIDandkeyWord", dbParams);
            RowCount = (int)dbParams[4].Value;
            return dt.ConvertToEntityCollection<CRMDriver>();
        }


        //新增或更新
        public CRMDriver AddOrUpdateCRMDriver(IEnumerable<CRMDriver> CRMDriver)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_AddOrUpdateCRMDriver", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Driver", CRMDriver.Select(p => new CRMDriverToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Size = 10;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return ds.Tables[0].ConvertToEntity<CRMDriver>();
            }
        }

        //根据ID查询
        public CRMDriver GetCRMDriverSearchConditionByID(string id)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {


                DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.String, id, ParameterDirection.Input),
               
            };
                DataTable dt = this.ExecuteDataTable("Proc_GetCRMDriverSearchByID", dbParams);

                return dt.ConvertToEntity<CRMDriver>();
                //SqlCommand cmd = new SqlCommand("Proc_GetCRMVehicleSearchID", conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@VehicleID", id);
                //cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                //conn.Open();
                //DataTable ds = new DataTable();
                //SqlDataAdapter VSC = new SqlDataAdapter(cmd);
                //VSC.Fill(ds);  
                //return ds.ConvertToEntity<CRMVehicle>();
            }
        }
        //根据ID查询
        public IEnumerable<VehicleToDriver> GetCarCacheInfo(string Sql)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, Sql, ParameterDirection.Input)
                   };
                DataTable dt = this.ExecuteDataTable("Proc_GetCarCacheInfo", dbParams);
                //VehicleToDriver
                return dt.ConvertToEntityCollection<VehicleToDriver>();
            }
        }
        private string GenGetCRMDriverWheres(string keyword)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" and (");
            sb.Append("  CRM_Driver.DriverName like").Append("'%" + keyword + "%'");
            sb.Append(" or CRM_Driver.DriverLogisticsCompany like").Append("'%" + keyword + "%'");
            sb.Append(" or CRM_Driver.DriverPhone like").Append("'%" + keyword + "%'");
            sb.Append(" or CRM_Driver.DriverCardNo like").Append("'%" + keyword + "%'");
            sb.Append(" or CRM_Driver.DriverStartServeForRunbowDate like").Append("'%" + keyword + "%'");
            sb.Append(" or CRM_Driver.DriverServiceArea like").Append("'%" + keyword + "%'");
            sb.Append(" or CRM_Driver.DriverMainRoute like").Append("'%" + keyword + "%'").Append(")");
            return sb.ToString();
        }

        //删除
        public bool DeleteCRMDriver(string ID)
        {
            DbParam[] dbParams = new DbParam[] {
                           new DbParam("@ID",DbType.String,ID,ParameterDirection.Input)
                        };
            string str = this.ExecuteScalar("Proc_DeleteCRMDriver", dbParams).ToString();
            if (!string.IsNullOrEmpty(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GenGetCRMDriverWhere(CRMDriverSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            //司机姓名
            if (!string.IsNullOrEmpty(SearchCondition.DriverName))
            {
                sb.Append(" AND CRM_Driver.DriverName like '%").Append(SearchCondition.DriverName).Append("%' ");
            }
            //物流公司
            if (!string.IsNullOrEmpty(SearchCondition.DriverLogisticsCompany))
            {
                sb.Append(" AND CRM_Driver.DriverLogisticsCompany like '%").Append(SearchCondition.DriverLogisticsCompany).Append("%' ");
            }
            //联系电话
            if (!string.IsNullOrEmpty(SearchCondition.DriverPhone))
            {
                sb.Append(" AND CRM_Driver.DriverPhone like '%").Append(SearchCondition.DriverPhone).Append("%' ");
            }
            //开始为服务日期
            if (!string.IsNullOrEmpty(SearchCondition.StartServeForRunbowTime))
            {
                sb.Append(" and CRM_Driver.DriverStartServeForRunbowDate>='").Append(SearchCondition.StartServeForRunbowTime).Append("'");
                //sb.Append(" AND CRM_Driver.DriverStartServeForRunbowDate='").Append(SearchCondition.DriverStartServeForRunbowDate).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.EndServeForRunbowTime))
            {
                sb.Append(" and CRM_Driver.DriverStartServeForRunbowDate<='").Append(SearchCondition.EndServeForRunbowTime).Append("'");
                //sb.Append(" AND CRM_Driver.DriverStartServeForRunbowDate='").Append(SearchCondition.DriverStartServeForRunbowDate).Append("' ");
            }
            //身份证号码
            if (!string.IsNullOrEmpty(SearchCondition.DriverIDCard))
            {
                sb.Append(" AND CRM_Driver.DriverIDCard like '%").Append(SearchCondition.DriverIDCard).Append("%' ");
            }
            //驾驶证档案号
            if (!string.IsNullOrEmpty(SearchCondition.DriverCardNo))
            {
                sb.Append(" AND CRM_Driver.DriverCardNo like '%").Append(SearchCondition.DriverCardNo).Append("%' ");
            }
            //是否在服务中
            if (!string.IsNullOrEmpty(SearchCondition.DriverIsServing))
            {
                sb.Append(" AND CRM_Driver.DriverIsServing='").Append(SearchCondition.DriverIsServing).Append("' ");
            }
            //驾照类型
            if (!string.IsNullOrEmpty(SearchCondition.DriverCardType))
            {
                sb.Append(" AND CRM_Driver.DriverCardType='").Append(SearchCondition.DriverCardType).Append("' ");
            }
            //登记证签发地
            if (!string.IsNullOrEmpty(SearchCondition.DriverRegistrationCardSignedAddress))
            {
                sb.Append(" AND CRM_Driver.DriverRegistrationCardSignedAddress like '%").Append(SearchCondition.DriverRegistrationCardSignedAddress).Append("%' ");
            }
            //服务区域
            if (!string.IsNullOrEmpty(SearchCondition.DriverServiceArea))
            {
                sb.Append(" AND CRM_Driver.DriverServiceArea like '%").Append(SearchCondition.DriverServiceArea).Append("%' ");
            }
            //驾驶车辆牌号
            if (!string.IsNullOrEmpty(SearchCondition.DriverCarNo))
            {
                sb.Append(" AND CRM_Driver.DriverCarNo like '%").Append(SearchCondition.DriverCarNo).Append("%' ");
            }
            //主要行驶路线
            if (!string.IsNullOrEmpty(SearchCondition.DriverMainRoute))
            {
                sb.Append(" AND CRM_Driver.DriverMainRoute like '%").Append(SearchCondition.DriverMainRoute).Append("%' ");
            }

            return sb.ToString();
        }

        public IEnumerable<CRMDriver> GetDriverList()
        {
            return this.ExecuteDataTable("Proc_GetDriverList").ConvertToEntityCollection<CRMDriver>();
        }



        //车辆司机管理 分页
        public IEnumerable<CRMDriver> GetAllDriver(CRMDriver Driver, string driver, int PageIndex, int PageSize, out int RowCount)
        {
            string Where = SqlWhere(driver);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, Where, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_GetAllDriver", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMDriver>();
        }
        private string SqlWhere(string driver)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(driver))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (driver.IndexOf("\n") > 0)
                {
                    numbers = driver.Split('\n').Select(s => { return s.Trim(); });
                }
                if (driver.IndexOf(',') > 0)
                {
                    numbers = driver.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and DriverName in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and DriverName  like '%" + driver.Trim() + "%' ");
                }
            }
            return sb.ToString();
        }

        public string AddVehicleToDriver(IEnumerable<CRMDriverName> crmdrivername, string VehicleNo, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_AddVehicleToDriver", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Driver", crmdrivername.Select(p => new CRMVehicleToDriverToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Size = 50;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Size = 50;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[3].Size = 50;
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return message;
            }

        }

        public IEnumerable<CRMDriver> GetCRM_VehicleMappingDriver(string vehicleno)
        {

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@VehicleNo", DbType.String, vehicleno, ParameterDirection.Input)
                 
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetCRM_VehicleMappingDriver", dbParams);

            return dt.ConvertToEntityCollection<CRMDriver>();
        }



        public IEnumerable<CRMDriver> GetCRMVehicleMappingDriver(string vehicleno, string drivername)
        {
            string Where = SqlWhere(vehicleno, drivername);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, Where, ParameterDirection.Input)
            
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetCRMVehicleMappingDriver", dbParams);
            return dt.ConvertToEntityCollection<CRMDriver>();
        }

        private string SqlWhere(string vehicleno, string drivername)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(drivername))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (drivername.IndexOf("\n") > 0)
                {
                    numbers = drivername.Split('\n').Select(s => { return s.Trim(); });
                }
                if (drivername.IndexOf(',') > 0)
                {
                    numbers = drivername.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and d.DriverName in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and d.DriverName  like '%" + drivername.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(vehicleno))
            {
                sb.Append("and VehicleNo ='" + vehicleno + "'");

            }
            return sb.ToString();
        }

    }

}
