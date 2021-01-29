using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;
using Runbow.TWS.Entity.ShipperManagement;


namespace Runbow.TWS.Dao.ShipperManagement
{
   
    public class VehicleManagementAccessor : BaseAccessor
    {
        //查询
        public IEnumerable<CRMVehicle> GetCRMVehicleByConditionNoPaging(CRMVehicleSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetCRMVehicleWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
            new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetCRMVehicleByConditionNoPaging", dbParams).ConvertToEntityCollection<CRMVehicle>();
        }
        
        
        //两个
        public IEnumerable<CRMVehicle> GetCRMVehicleByCondition(CRMVehicleSearchCondition Vehicle, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetCRMVehicleWhere(Vehicle);
              int tempRowCount = 0;
              DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
              DataTable dt = this.ExecuteDataTable("Proc_GetCRMVehicleByCondition", dbParams);
             RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMVehicle>();

        }


        /// <summary>
        /// 通过承运商ID查询该承运商下面有哪些车辆 
        /// </summary>
        /// <param name="shipperid"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<CRMVehicle> GetShipperMappingVehicleBySID(string shipperid, int PageIndex, int PageSize, out int RowCount)
        {
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ShipperID", DbType.String, shipperid, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetShipperMappingVehicleBySID", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMVehicle>();
        }
        //手机端查询
        public IEnumerable<CRMVehicle> GetCRMVehicleBykeyword(string keyword, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetCRMVehicleWheres(keyword);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_GetCRMVehicleByConditionurl", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMVehicle>();

        }
       
        //通过shipperid查车辆再在结果集里面根据关键字查询  分页
        public IEnumerable<CRMVehicle> GetShipperMappingVehicleInfoByShipperIDandkeyWord(string id, string keyword, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenGetCRMVehicleWheres(keyword);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ShipperID", DbType.String, id, ParameterDirection.Input),
                new DbParam("@where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetShipperMappingVehicleInfoByShipperIDandkeyWord", dbParams);
            RowCount = (int)dbParams[4].Value;
            return dt.ConvertToEntityCollection<CRMVehicle>();
        }

        //未明
        //private string QuerySql(CRMVehicleSearchCondition Vehicle)
        //{
        //    StringBuilder sb = new StringBuilder();



        //    return sb.ToString();
        //}


        public CRMVehicle AddOrUpdateCRMVehicle(IEnumerable<CRMVehicle> CRMVehicle)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_AddOrUpdateCRMVehicle", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Vehicle", CRMVehicle.Select(p => new CRMVehicleToDb(p)));
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
                return ds.Tables[0].ConvertToEntity<CRMVehicle>();
            }
        }

        //删除
        public bool DeleteCRMVehicle(string ID)
        {
            DbParam[] dbParams = new DbParam[] {
                           new DbParam("@ID",DbType.String,ID,ParameterDirection.Input)
                        };
            string str = this.ExecuteScalar("Proc_DeleteCRMVehicle", dbParams).ToString();
            if (!string.IsNullOrEmpty(str))
            {
                return true;
            }
            else
                return false;
           //base.ExecuteNoQuery("Proc_DeleteCRMVehicle", Db);
        }


         

        //根据ID查询
        public CRMVehicle GetCRMVehicleSearchConditionID(string id)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
 
               
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@VehicleID", DbType.String, id, ParameterDirection.Input),
               
            };
                DataTable dt = this.ExecuteDataTable("Proc_GetCRMVehicleSearchID", dbParams);
                
                return dt.ConvertToEntity<CRMVehicle>();
                 
            }
        }
        public CRMVehicle GetCRMVehiclebyID(string id)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@VehicleID", DbType.String, id, ParameterDirection.Input),
               
            };
                DataTable dt = this.ExecuteDataTable("Proc_GetCRMVehiclebyID", dbParams);

                return dt.ConvertToEntity<CRMVehicle>();

            }
        }
        private string GenGetCRMVehicleWhere(CRMVehicleSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            //车牌号
            if (!string.IsNullOrEmpty(SearchCondition.CarNo))
            {
                sb.Append(" AND CRM_Vehicle.CarNo like '%").Append(SearchCondition.CarNo).Append("%' ");
            }
            //车型编码
            if (!string.IsNullOrEmpty(SearchCondition.CarTypeNo))
            {
                sb.Append(" AND CRM_Vehicle.CarTypeNo like '%").Append(SearchCondition.CarTypeNo).Append("%' ");
            }
            //物流公司
            if (!string.IsNullOrEmpty(SearchCondition.LogisticCompany))
            {
                sb.Append(" AND CRM_Vehicle.LogisticCompany like '%").Append(SearchCondition.LogisticCompany).Append("%' ");
            }
            //已行驶公里数
            if (!string.IsNullOrEmpty(SearchCondition.DrivedJourney))
            {
                sb.Append(" AND CRM_Vehicle.DrivedJourney='").Append(SearchCondition.DrivedJourney).Append("' ");
            }
            //上牌日期
            if (!string.IsNullOrEmpty(SearchCondition.StartBoardlotTime))
            {
                sb.Append("and  CRM_Vehicle.BoardlotDate>='").Append(SearchCondition.StartBoardlotTime).Append("'");
                //sb.Append(" AND CRM_Vehicle.BoardlotDate='").Append(SearchCondition.BoardlotDate).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.EndBoardlotTime))
            {
                sb.Append("and  CRM_Vehicle.BoardlotDate<='").Append(SearchCondition.EndBoardlotTime).Append("'");
                //sb.Append(" AND CRM_Vehicle.BoardlotDate='").Append(SearchCondition.BoardlotDate).Append("' ");
            }
            //车龄
            if (!string.IsNullOrEmpty(SearchCondition.CarAge))
            {
                sb.Append(" AND CRM_Vehicle.CarAge like '%").Append(SearchCondition.CarAge).Append("%' ");
            }
            //号牌种类
            if (!string.IsNullOrEmpty(SearchCondition.CarNumType))
            {
                sb.Append(" AND CRM_Vehicle.CarNumType='").Append(SearchCondition.CarNumType).Append("' ");
            }
            //车身颜色
            if (!string.IsNullOrEmpty(SearchCondition.CarBodyColor))
            {
                sb.Append(" AND CRM_Vehicle.CarBodyColor like '%").Append(SearchCondition.CarBodyColor).Append("%' ");
            }
            //生产厂家
            if (!string.IsNullOrEmpty(SearchCondition.Manufacturer))
            {
                sb.Append(" AND CRM_Vehicle.Manufacturer like '%").Append(SearchCondition.Manufacturer).Append("%' ");
            }
            //整备质量
            if (!string.IsNullOrEmpty(SearchCondition.EntireCarWeight))
            {
                sb.Append(" AND CRM_Vehicle.EntireCarWeight='").Append(SearchCondition.EntireCarWeight).Append("' ");
            }
            //燃料种类
            if (!string.IsNullOrEmpty(SearchCondition.FuelType))
            {
                sb.Append(" AND CRM_Vehicle.FuelType='").Append(SearchCondition.FuelType).Append("' ");
            }
            //开始服务日期
            if (!string.IsNullOrEmpty(SearchCondition.StartServiceTime))
            {
                sb.Append("and  CRM_Vehicle.StartServiceDate>='").Append(SearchCondition.StartServiceTime).Append("'");
                //sb.Append(" AND CRM_Vehicle.StartServiceDate='").Append(SearchCondition.StartServiceDate).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.EndServiceTime))
            {
                sb.Append("and  CRM_Vehicle.StartServiceDate<='").Append(SearchCondition.EndServiceTime).Append("'");
                //sb.Append(" AND CRM_Vehicle.StartServiceDate='").Append(SearchCondition.StartServiceDate).Append("' ");
            }



            return sb.ToString();

        }

        private string GenGetCRMVehicleWheres(string keyword)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" and (");
            //车牌号
            sb.Append("  CRM_Vehicle.CarNo like ").Append("'%" + keyword + "%'");

            //车型编码
            sb.Append(" or CRM_Vehicle.CarTypeNo like ").Append("'%" + keyword + "%'");

            //物流公司
            sb.Append(" or CRM_Vehicle.LogisticCompany like ").Append("'%" + keyword + "%'");

            //已行驶公里数
            sb.Append(" or CRM_Vehicle.DrivedJourney like ").Append("'%" + keyword + "%'");

            //车龄
            sb.Append(" or CRM_Vehicle.CarAge like ").Append("'%" + keyword + "%'");

            //号牌种类
            sb.Append(" or  CRM_Vehicle.CarNumType like ").Append("'%" + keyword + "%'");

            //车身颜色
            sb.Append(" or CRM_Vehicle.CarBodyColor like ").Append("'%" + keyword + "%'");

            //整备质量
            sb.Append(" or CRM_Vehicle.EntireCarWeight like ").Append("'%" + keyword + "%'");

            //燃料种类
            sb.Append(" or CRM_Vehicle.FuelType like ").Append("'%" + keyword + "%'").Append(")");

            return sb.ToString();

        }

        // 获取所有的车辆
        public IEnumerable<CRMVehicle> GetVehicleList()
        {
            return this.ExecuteDataTable("Proc_GetVehicleList").ConvertToEntityCollection<CRMVehicle>();
        }

        /// <summary>
        ///获取所有车辆 分页
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<CRMVehicle> GetAllVehicle(CRMVehicle Vehicle, string vehicle, int PageIndex, int PageSize, out int RowCount)
        {

            string Where = SqlWhere(vehicle);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String,Where , ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_GetAllVehicle", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<CRMVehicle>();

        }
        private string SqlWhere(string vehicle)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(vehicle))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (vehicle.IndexOf("\n") > 0)
                {
                    numbers = vehicle.Split('\n').Select(s => { return s.Trim(); });
                }
                if (vehicle.IndexOf(',') > 0)
                {
                    numbers = vehicle.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and CarNo in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and CarNo  like '%" + vehicle.Trim() + "%' ");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 分配车辆信息
        /// </summary>
        /// <param name="crmCar"></param>
        /// <returns></returns>
        public string AddShipperToVehicle(IEnumerable<CRMCar> crmcar, string ShipperName, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_AddShipperToVehicle", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Vehicle", crmcar.Select(p => new CRMShipperToVehicleToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ShipperName", ShipperName);
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


        public IEnumerable<CRMVehicle> GetCRM_ShipperMappingVehicle(string shippername)
        { 
            
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ShipperName", DbType.String, shippername, ParameterDirection.Input)
                 
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetCRM_ShipperMappingVehicle", dbParams);
       
            return dt.ConvertToEntityCollection<CRMVehicle>();
        }


        public IEnumerable<CRMVehicle> GetCRMShipperMappingVehicle(string shippername, string vehicleno)
        {
            string Where = SqlWhere(vehicleno,shippername);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, Where, ParameterDirection.Input)
            
            };
            DataTable dt = this.ExecuteDataTable("Pro_GetCRMShipperMappingVehicle", dbParams);
            return dt.ConvertToEntityCollection<CRMVehicle>();
        }

        private string SqlWhere(string vehicle,string shippername)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(vehicle))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (vehicle.IndexOf("\n") > 0)
                {
                    numbers = vehicle.Split('\n').Select(s => { return s.Trim(); });
                }
                if (vehicle.IndexOf(',') > 0)
                {
                    numbers = vehicle.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and CarNo in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and CarNo  like '%" + vehicle.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(shippername))
            {
                sb.Append("and ShipperName ='" + shippername+"'");
            
            }
            return sb.ToString();
        }

        




        //public IEnumerable<CRMVehicle> SearchVehicle(string vehicleNo, int PageIndex, int PageSize, out int RowCount)
        //{
        //    string sqlWhere = this.SearchVehicle(vehicleNo);
        //    int tempRowCount = 0;
        //    DbParam[] dbParams = new DbParam[]{
        //        new DbParam("@Where", DbType.String,sqlWhere , ParameterDirection.Input),
        //        new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
        //        new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
        //        new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
        //    };
        //    DataTable dt = this.ExecuteDataTable("Proc_SearchVehicle", dbParams);
        //    RowCount = (int)dbParams[3].Value;
        //    return dt.ConvertToEntityCollection<CRMVehicle>();
             
        //}

        //    private string SearchVehicle(string vehicle)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        if (!string.IsNullOrEmpty(vehicle))
        //        {
        //            IEnumerable<string> numbers = Enumerable.Empty<string>();
        //            if (vehicle.IndexOf("\n") > 0)
        //            {
        //                numbers = vehicle.Split('\n').Select(s => { return s.Trim(); });
        //            }
        //            if (vehicle.IndexOf(',') > 0)
        //            {
        //                numbers = vehicle.Split(',').Select(s => { return s.Trim(); });
        //            }

        //            if (numbers != null && numbers.Any())
        //            {
        //                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
        //            }

        //            if (numbers != null && numbers.Any())
        //            {
        //                sb.Append(" and VehicleNo in ( ");
        //                foreach (string s in numbers)
        //                {
        //                    sb.Append("'").Append(s).Append("',");
        //                }
        //                sb.Remove(sb.Length - 1, 1);
        //                sb.Append(" ) ");
        //            }
        //            else
        //            {
        //                sb.Append(" and VehicleNo  like '%" + vehicle.Trim() + "%' ");
        //            }
        //        }
        //        return sb.ToString();
        //    }
    
    
    
    
    }
}
