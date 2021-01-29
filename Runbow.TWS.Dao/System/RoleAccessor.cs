using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Text;
using System;
using Runbow.TWS.Entity.System;

namespace Runbow.TWS.Dao
{
    public class RoleAccessor : BaseAccessor
    {
        public static int RolePod(RoleRequest r)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("porc_AddRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name",r.Name);
                cmd.Parameters.AddWithValue("@Description", r.Description);
                cmd.Parameters.AddWithValue("@Satate", r.Satate);
                cmd.Parameters.AddWithValue("@PId", (int)r.ProjectId);
                cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["RETURN_VALUE"].Value;
            }
        }

        /// <summary>
        /// 验证角色名称的唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool CheckNameIsExist(string Name, int? Id, string ProjectID, bool IsEdit)
        {
            string sql = " SELECT COUNT(1) FROM dbo.Role A INNER JOIN ProjectRole_Mapping  B ON A.ID=B.RoleID  WHERE B.ProjectID=@ProjectID AND A.Name=@Name ";
            sql += Id > 0 ? " AND A.ID!=" + Id + " " : string.Empty;

            DbParam[] parms = {
                      new DbParam("@Name", DbType.String, Name, ParameterDirection.Input),
                       new DbParam("@ProjectID", DbType.String, ProjectID, ParameterDirection.Input)
                             };
            return (int)base.ExecuteScalarBySqlString(sql, parms) > 0 ? false : true;
        }

        /// <summary>
        /// 插入数据库前先检查 并
        /// </summary>
        /// <param name="name"></param>
        /// <returns>-1 则数据库已存在该角色 否则返回的是将要插入角色产生的角色ID</returns>
        public static string CheckIput(RoleRequest r)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("proc_CheckRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", r.Name);
                SqlParameter parOutput = cmd.Parameters.Add("@rId",0);　　//定义输出参数
                parOutput.Direction = ParameterDirection.Output;　　//参数类型为Output
                conn.Open();
                cmd.ExecuteNonQuery();
               
                return cmd.Parameters["@rId"].Value.ToString();
            }
        }

        public static int RoleUpdate(RoleRequest r)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql = string.Format("update  [dbo].[Role] set Name='{0}',Description='{1}',Satate={2} where id={3};", r.Name, r.Description, r.Satate, r.ID);
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@PodData", podList);
                //cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                //cmd.Parameters.AddWithValue("@OutputID", 0);
                //cmd.Parameters[1].Direction = ParameterDirection.Output;
                //cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Role> GetRole(Role role, int PageIndex, int PageSize,long Pid,string State, out int RowCount)
        {
            string strSQL = this.GetSqlWhere(role);
            DbParam[] dbParams = {
                           new DbParam("@where",DbType.String,strSQL,ParameterDirection.Input),
                           new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                           new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                           new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output),
                           new DbParam("@ProjectId",DbType.String,Pid,ParameterDirection.Input),
                           new DbParam("@State",DbType.String,State,ParameterDirection.Input)
                          };

            IEnumerable<Role> rollist = base.ExecuteDataTable("porc_GetRole", dbParams).ConvertToEntityCollection<Role>();
            RowCount = (int)dbParams[3].Value;
            return rollist;
        }

        public Role UpdateSelect(int id)
        {
            Role r = new Role();
            string sql = "select * from [dbo].[Role] where ID=" + id;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);

                //comm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sdr = new SqlDataAdapter(comm);
                DataTable dt = new DataTable();
                sdr.Fill(dt);


                foreach (DataRow dr in dt.Rows)
                {
                    r.Name = dr["Name"].ToString();
                    r.Description = dr["Description"].ToString();
                    r.State = (bool)dr["Satate"];
                    r.ID = dr["ID"].ObjectToInt32();

                }

            }
            return r;

        }

        public string GetSqlWhere(Role info)
        {
            StringBuilder sb = new StringBuilder();
           
            sb.Append(" 1=1 ");

            if (info != null)
            {
                if (!string.IsNullOrEmpty(info.Description))
                {
                    sb.Append(" AND A.Description like '" + info.Description + "%'");
                }
                if (!string.IsNullOrEmpty(info.Name))
                {
                    sb.Append(" AND A.Name like '" + info.Name + "%' ");
                }
            }
         

            return sb.ToString();
        }

        public IEnumerable<ProjectRoleMenu> GetRoleMenuByProjectRoleID(long projectRoleID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ProjectRoleID", DbType.Int64, projectRoleID, ParameterDirection.Input)
                
            };

            return this.ExecuteDataTable("Proc_GetProjectRoleMenu", dbParams).ConvertToEntityCollection<ProjectRoleMenu>();
        }

        public IEnumerable<ProjectRole> GetRoleByProjectID(long projectID, bool getAll = false)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
                new DbParam("@GetAll", DbType.Boolean, getAll, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetRoleByProjectID", dbParams).ConvertToEntityCollection<ProjectRole>();
        }

        public IEnumerable<ProjectRole> GetAllProjectRoles()
        {
            return this.ExecuteDataTable("Proc_GetAllProjectRole").ConvertToEntityCollection<ProjectRole>();
        }

          public IEnumerable<Role> GetRoles()
        {
            return this.ExecuteDataTable("Proc_GetRoles").ConvertToEntityCollection<Role>();
        }
                           
        public bool SetProjectRole(long projectID, IEnumerable<long> roleIDs)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SetProjectRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectID", projectID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@RoleIDs", roleIDs.Any() ? roleIDs.Select(id => new IdsForInt64(id)) : null);
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ReturnValue", -1);
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                conn.Open();

                cmd.ExecuteNonQuery();

                if (!cmd.Parameters[2].Value.ObjectToBoolean())
                {
                    return false;
                }
            }

            return true;
        }

        public ProjectRole GetUserProjectRole(long userID, long projectID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@UserID", DbType.Int64, userID, ParameterDirection.Input),
                new DbParam("@ProjectID", DbType.Int64, projectID, ParameterDirection.Input),
            };

            return this.ExecuteDataTable("Proc_GetUserProjectRole", dbParams).ConvertToEntity<ProjectRole>();
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="paramsIds"></param>
        /// <returns></returns>
        public string DeleteRole(string paramsIds,string Isp,long ProjectId)
        {
            string ReturnValue = "0";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("proc_DeleteRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ids", paramsIds);
                cmd.Parameters.AddWithValue("@Isp",int.Parse(Isp));
                cmd.Parameters.AddWithValue("@ProjectId", ProjectId);
                conn.Open();
                try
                {
                    ReturnValue = cmd.ExecuteNonQuery().ToString();
                }
                catch (Exception err)
                {
                    
                }
            }
            return ReturnValue; //返回0 失败  否则成功
        }
    }
}