using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using Runbow.TWS.Entity.WMS.Warehouse;

namespace Runbow.TWS.Dao
{
    public class ProjectAccessor : BaseAccessor
    {
        public bool DeleteProject(long ID) 
        {
            string strSQL = "update dbo.[Project] set state=0 where [ID] =@ID";
            DbParam[] para = {
                      new DbParam("@ID", global::System.Data.DbType.Int64, ID, global::System.Data.ParameterDirection.Input)
                             };
            try
            {
                if (base.ExecuteScalarBySqlString(strSQL, para).ObjectToBoolean())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id">项目Id</param>
        /// <param name="IsEnable">0禁用 1 启用</param>
        /// <returns></returns>
        public bool DeleteP(long id,int IsEnable)
        {
            DbParam[] para = {
                      new DbParam("@ProjectId", global::System.Data.DbType.Int64, id, global::System.Data.ParameterDirection.Input),
                      new DbParam("@IsEnable", global::System.Data.DbType.Int64, IsEnable, global::System.Data.ParameterDirection.Input)
                             };

            try
            {
                if (base.ExecuteScalar("Proc_DeleteByProjectID", para).ObjectToBoolean())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }

        }

        public IEnumerable<Project> GetAllProjects()
        {
            string strSQL = "select [ID] ,[Name] ,dbo.fun_getPY([Name]) AS UserName,[Description],[State],[CreateDate] from dbo.[Project] where [state]=1";
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL).ConvertToEntityCollection<Project>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 查询项目（模糊查询）
        /// </summary>
        /// <param name="Name">项目名称</param>
        /// <param name="Description">项目描述</param>
        /// <returns></returns>
        public IEnumerable<Project> GetAllProjects(string Name,string Description,string state)
        {
            string strSQL = "select * from dbo.[Project] where [state]=" + state + " ";

            if (Name.Trim().Length > 0)
                strSQL += " and Name like '" + Name + "%' ";

            if (Description.Trim().Length > 0)
                strSQL += " and Description like '" + Description + "%' ";

            try
            {
                return base.ExecuteDataTableBySqlString(strSQL).ConvertToEntityCollection<Project>();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<ProjectUserCustomer> GetAllProjectUserCustomers()
        {
            return base.ExecuteDataTable("Proc_GetAllProjectUserCustomers").ConvertToEntityCollection<ProjectUserCustomer>();
        }

        public IEnumerable<ProjectCustomerWarehouse> GetAllProjectCustomersWarehouse()
        {
            return base.ExecuteDataTable("Proc_WMS_GetAllWarehouse").ConvertToEntityCollection<ProjectCustomerWarehouse>();
        }
        //库区
        public IEnumerable<AreaInfo> GetAllProjectCustomersWarehouse_Area()
        {
            return base.ExecuteDataTable("Proc_WMS_GetAllWMS_Warehouse_Area").ConvertToEntityCollection<AreaInfo>();
        }

        //用户关联库区
        public IEnumerable<WMS_User_Area_Mapping> GetAllUser_Area()
        {
            return base.ExecuteDataTable("Proc_WMS_GetAllUser_Area").ConvertToEntityCollection<WMS_User_Area_Mapping>();
        }
        //库位
        public IEnumerable<LocationInfo> GetAllProjectCustomersWarehouse_Location()
        {
            return base.ExecuteDataTable("Proc_WMS_GetAllWMS_Warehouse_Location").ConvertToEntityCollection<LocationInfo>();
        }
        //库位
        public IEnumerable<LocationInfo> GetAllProjectCustomersWarehouse_Location(long Customer)
        {
            DbParam[] para = { new DbParam("@Customer", DbType.Int64, Customer, ParameterDirection.Input) };
            return base.ExecuteDataTable("[Proc_WMS_GetAllWMS_Warehouse_LocationByCustomer]", para).ConvertToEntityCollection<LocationInfo>();
        }

        public Project GetProjectById(long projectId, string State)
        {
            string strSQL = "select top 1 * from dbo.[Project] where [state]=" + State + " and  [ID]=@ID";
            DbParam[] para = {
                      new DbParam("@ID", global::System.Data.DbType.Int64, projectId, global::System.Data.ParameterDirection.Input)
                             };
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL, para).ConvertToEntity<Project>();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<ProjectCustomersOrShippers> GetProjectCustomersOrShippers(int target, long projectID)
        {
            DbParam[] parms = {
                      new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
                      new DbParam("@Target", DbType.Int32, target, ParameterDirection.Input)
                             };
            return base.ExecuteDataTable("Proc_GetCustomerOrShipperByProjectID", parms).ConvertToEntityCollection<ProjectCustomersOrShippers>();
        }

        public int SaveProject(Project project)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_InsertProject", conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", project.Name);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Code", project.Code);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Description", project.Description);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    return (int)cmd.Parameters["RETURN_VALUE"].Value; //保存成功返回Id
                }
                catch
                {
                    throw;
                }
            }
        }

        public bool SetProjectCustomerOrShippers(long projectID, int target, IEnumerable<ProjectCustomersOrShippers> customerOrShippers)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SetProjectCustomerOrShipper", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectID", projectID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@Target", target);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@CustomerOrShippers", customerOrShippers.Any() ? customerOrShippers.Select(cs => new ProjectCustomerOrShipperToDb(cs)) : null);
                cmd.Parameters[2].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ReturnValue", -1);
                cmd.Parameters[3].Direction = ParameterDirection.Output;
                cmd.Parameters[3].SqlDbType = SqlDbType.Int;

                conn.Open();

                cmd.ExecuteNonQuery();

                if (!cmd.Parameters[3].Value.ObjectToBoolean())
                {
                    return false;
                }
            }

            return true;
        }

        public int SetProjectCustomerOrShipperSegment(long projectID, int target, long projectCustomerOrShipperID, long customerOrShipperID, long oldSegmentID, long newSegmentID)
        {
            DbParam[] parms = {
                      new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
                      new DbParam("@Target", DbType.Int32, target, ParameterDirection.Input),
                      new DbParam("@ProjectCustomerOrShipperID", DbType.Int64, projectCustomerOrShipperID, ParameterDirection.Input),
                      new DbParam("@CustomerOrShipperID", DbType.Int64, customerOrShipperID, ParameterDirection.Input),
                      new DbParam("@OldSegmentID", DbType.Int32, oldSegmentID, ParameterDirection.Input),
                      new DbParam("@NewSegmentID", DbType.Int64, newSegmentID, ParameterDirection.Input),
                      new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };
            base.ExecuteNoQuery("Proc_SetProjectCustomerOrShipperSegment", parms);

            return parms[6].Value.ObjectToInt32();
        }

        public int SetProjectCustomerOrShipperSegment(long projectID, int target, long customerOrShipperID, string segmentIDs, string relatedCustomerIDs)
        {
            DbParam[] parms = {
                      new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
                      new DbParam("@Target", DbType.Int32, target, ParameterDirection.Input),
                      new DbParam("@CustomerOrShipperID", DbType.Int64, customerOrShipperID, ParameterDirection.Input),
                      new DbParam("@SegmentIDs", DbType.String, segmentIDs, ParameterDirection.Input),
                      new DbParam("@RelatedCustomerIDs", DbType.String, relatedCustomerIDs, ParameterDirection.Input),
                      new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };
            base.ExecuteNoQuery("Proc_SetProjectCustomerOrShipperSegmentNew", parms);

            return parms[5].Value.ObjectToInt32();
        }

        public int SetProjectCustomerOrShipperSegment(long projectID, int target, long customerOrShipperID, long segmentID, long relatedCustomerID)
        {
            DbParam[] parms = {
                      new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
                      new DbParam("@Target", DbType.Int32, target, ParameterDirection.Input),
                      new DbParam("@CustomerOrShipperID", DbType.Int64, customerOrShipperID, ParameterDirection.Input),
                      new DbParam("@SegmentID", DbType.Int64, segmentID, ParameterDirection.Input),
                      new DbParam("@RelatedCustomerID", DbType.Int64, relatedCustomerID, ParameterDirection.Input),
                      new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };
            base.ExecuteNoQuery("Proc_SetProjectCustomerOrShipperSegmentNew", parms);

            return parms[5].Value.ObjectToInt32();
        }

        public bool SetProjectUserCustomers(long userID, long projectID, IEnumerable<long> customerIDs, string creator, DateTime createTime)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SetProjectUserCustomers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectID", projectID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@CustomerIDs", customerIDs != null && customerIDs.Any() ? customerIDs.Select(cs => new IdsForInt64(cs)) : null);
                cmd.Parameters[2].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@Creator", creator);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@CreateTime", createTime);
                cmd.Parameters[4].SqlDbType = SqlDbType.DateTime;
                cmd.Parameters.AddWithValue("@ReturnValue", -1);
                cmd.Parameters[5].Direction = ParameterDirection.Output;
                cmd.Parameters[5].SqlDbType = SqlDbType.Int;

                conn.Open();

                cmd.ExecuteNonQuery();

                if (!cmd.Parameters[5].Value.ObjectToBoolean())
                {
                    return false;
                }
            }

            return true;
        }

        public bool SetUserProjectRole(long userID, long projectRoleID, long projectID)
        {
            DbParam[] parms = {
                      new DbParam("@UserID", DbType.Int64, userID, ParameterDirection.Input),
                      new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
                      new DbParam("@ProjectRoleID", DbType.Int64, projectRoleID, ParameterDirection.Input)
                             };
            base.ExecuteNoQuery("Proc_SetUserProjectRole", parms);
            return true;
        }

        /// <summary>
        /// 验证公司编号的唯一性
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public bool CheckCodeIsExist(string Code, int? Id, bool IsEdit)
        {
            string sql = " SELECT COUNT(1) FROM Project WHERE Code=@Code ";
            sql += IsEdit ? " AND ID!=" + Id + " " : string.Empty;

            DbParam[] parms = {
                      new DbParam("@Code", DbType.String, Code, ParameterDirection.Input)
                             };
            return (int)base.ExecuteScalarBySqlString(sql, parms) > 0 ? false : true;
        }

        public int UpdateProject(Project project)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_UpdateProject", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", project.Name);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Code", project.Code);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Description", project.Description);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@ID", project.ID);
                cmd.Parameters[3].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@State", project.State);
                cmd.Parameters[4].SqlDbType = SqlDbType.Bit;

                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        public IEnumerable<ProjectCustomerOrShipperSegment> GetAllProjectCustomerOrShipperSegments()
        {
            return base.ExecuteDataTable("Proc_GetAllProjectCustomerOrShipperSegments").ConvertToEntityCollection<ProjectCustomerOrShipperSegment>();
        }
    }
}