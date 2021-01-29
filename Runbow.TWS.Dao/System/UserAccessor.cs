using System.Collections.Generic;
using System.Data;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;

namespace Runbow.TWS.Dao
{
    public class UserAccessor : BaseAccessor
    {
        public User GetUserById(long userId)
        {
            DbParam[] paras = {
                      new DbParam("@ID", DbType.Int64, userId, ParameterDirection.Input)
                             };
            return base.ExecuteDataTable("Proc_GetUserById", paras).ConvertToEntity<User>();
        }

        /// <summary>
        /// 验证登录名称的唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool CheckNameIsExist(string Name, int? Id, bool IsEdit)
        {
            string sql = " SELECT COUNT(1) FROM dbo.[User] WHERE Name=@Name ";
            sql += Id > 0 ? " AND ID!=" + Id + " " : string.Empty;

            DbParam[] parms = {
                      new DbParam("@Name", DbType.String, Name, ParameterDirection.Input)
                             };
            return (int)base.ExecuteScalarBySqlString(sql, parms) > 0 ? false : true;
        }

        /// <summary>
        /// 查询某个项目下的用户
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<User> GetUserByProjectId(long projectId)
        {
            DbParam[] paras = {
                      new DbParam("@ProjectId", DbType.Int64, projectId, ParameterDirection.Input)
                             };
            return base.ExecuteDataTable("Proc_GetUserByProjectId", paras).ConvertToEntityCollection<User>();
        }

        public User CheckLoginUser(string UserName, string Password, long ProjectID)
        {
            DbParam[] paras = {
                      new DbParam("@Name", DbType.String,UserName, ParameterDirection.Input, 50),
                      new DbParam("@Password", DbType.String, Password, ParameterDirection.Input,50),
                      new DbParam("@ProjectID", DbType.Int64, ProjectID, ParameterDirection.Input,50)
                                                  };
            return base.ExecuteDataTable("Proc_CheckLoginUser", paras).ConvertToEntity<User>();
        }

        public IEnumerable<User> GetUsersByConditon(string name, string displyName, bool state, int userType, int pageIndex, int pageSize, long projectId,out int rowCount)
        {
            int tmpRowCount = 0;
            DbParam[] dbParams = {
                             new DbParam("@Name", DbType.String, name, ParameterDirection.Input),
                             new DbParam("@DisplyName", DbType.String, displyName, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input),
                             new DbParam("@UserType", DbType.Int32, userType, ParameterDirection.Input),
                             new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                             new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                             new DbParam("@RowCount", DbType.Int32, tmpRowCount, ParameterDirection.Output),
                             new DbParam("@ProjectId", DbType.Int32, projectId, ParameterDirection.Input)
                                  };

            IEnumerable<User> users = this.ExecuteDataTable("Proc_GetUsersByConditon", dbParams).ConvertToEntityCollection<User>();
            rowCount = dbParams[6].Value.ObjectToInt32();
            return users;
        }

        public long AddUser(string name, string displyName, string password, bool state, char sex, string tel, string mobile, string email, int userType, long customerOrShipperID, string RuleArea, out int returnVal)
        {
            DbParam[] dbParams = {
                             new DbParam("@Name", DbType.String, name, ParameterDirection.Input),
                             new DbParam("@DisplyName", DbType.String, displyName, ParameterDirection.Input),
                             new DbParam("@Password", DbType.String, password, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input),
                             new DbParam("@Sex", DbType.String, sex, ParameterDirection.Input),
                             new DbParam("@Tel", DbType.String, tel, ParameterDirection.Input),
                             new DbParam("@Mobile", DbType.String, mobile, ParameterDirection.Input),
                             new DbParam("@Email", DbType.String, email, ParameterDirection.Input),
                             new DbParam("@UserType", DbType.Int32, userType, ParameterDirection.Input),
                             new DbParam("@CustomerOrShipperID", DbType.Int64, customerOrShipperID, ParameterDirection.Input),
                             new DbParam("@RuleArea", DbType.String, RuleArea, ParameterDirection.Input),
                             new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };

            long userID = base.ExecuteScalar("Proc_AddUser", dbParams).ObjectToInt64();
            returnVal = dbParams[11].Value.ObjectToInt32();
            return userID;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displyName"></param>
        /// <param name="password"></param>
        /// <param name="state"></param>
        /// <param name="sex"></param>
        /// <param name="tel"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="userType"></param>
        /// <param name="customerOrShipperID"></param>
        /// <param name="RuleArea"></param>
        /// <param name="UserName">操作人用户名</param>
        /// <param name="Uid">操作人id</param>
        /// <param name="WarehourseId">仓库id</param>
        /// <param name="RoelId">角色id</param>
        /// <param name="returnVal"></param>
        /// <returns></returns>
        public long AddUser2(string name, string displyName, string password, bool state, char sex, string tel, string mobile, string email, int userType, long customerOrShipperID, string RuleArea, string UserName, string RoelId,int ProjectId, out int returnVal)
        {
            DbParam[] dbParams = {
                             new DbParam("@Name", DbType.String, name, ParameterDirection.Input),
                             new DbParam("@DisplyName", DbType.String, displyName, ParameterDirection.Input),
                             new DbParam("@Password", DbType.String, password, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input),
                             new DbParam("@Sex", DbType.String, sex, ParameterDirection.Input),
                             new DbParam("@Tel", DbType.String, tel, ParameterDirection.Input),
                             new DbParam("@Mobile", DbType.String, mobile, ParameterDirection.Input),
                             new DbParam("@Email", DbType.String, email, ParameterDirection.Input),
                             new DbParam("@UserType", DbType.Int32, userType, ParameterDirection.Input),
                             new DbParam("@CustomerOrShipperID", DbType.Int64, customerOrShipperID, ParameterDirection.Input),
                             new DbParam("@RuleArea", DbType.String, RuleArea, ParameterDirection.Input),
                             //新增参数(2016-5-16) start 
                             new DbParam("@ProjectId", DbType.Int32, ProjectId, ParameterDirection.Input),
                             new DbParam("@UserName", DbType.String, UserName, ParameterDirection.Input),
                             //new DbParam("@WareHouseId", DbType.String, WarehourseId, ParameterDirection.Input),
                             new DbParam("@RoleId", DbType.String, RoelId, ParameterDirection.Input),
                             //新增参数 end

                             new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };

            long userId = base.ExecuteScalar("Proc_AddUser2", dbParams).ObjectToInt64();
            returnVal = dbParams[14].Value.ObjectToInt32();
            return userId;
        }

        public void UpdateUser(long ID, string name,string UserName,long ProjectId,long projectRoleId, string displyName, bool state, char sex, string tel, string mobile, string email, int userType, long customerOrShipperID, string RuleArea)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input),
                             new DbParam("@Name", DbType.String, name, ParameterDirection.Input),

                             new DbParam("@UserName", DbType.String, UserName, ParameterDirection.Input),
                             new DbParam("@ProjectRoleId", DbType.Int32, projectRoleId, ParameterDirection.Input),
                             new DbParam("@ProjectId", DbType.Int32, ProjectId, ParameterDirection.Input),

                             new DbParam("@DisplyName", DbType.String, displyName, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input),
                             new DbParam("@Sex", DbType.String, sex, ParameterDirection.Input),
                             new DbParam("@Tel", DbType.String, tel, ParameterDirection.Input),
                             new DbParam("@Mobile", DbType.String, mobile, ParameterDirection.Input),
                             new DbParam("@Email", DbType.String, email, ParameterDirection.Input),
                             new DbParam("@UserType", DbType.Int32, userType, ParameterDirection.Input),
                             new DbParam("@CustomerOrShipperID", DbType.Int64, customerOrShipperID, ParameterDirection.Input),
                             new DbParam("@RuleArea", DbType.String, RuleArea, ParameterDirection.Input)
                             };
            base.ExecuteNoQuery("Proc_UpdateUser", dbParams);
        }

        public IEnumerable<Project> GetUserProjects(string userName)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@UserName", DbType.String, userName, ParameterDirection.Input),
            };
            return this.ExecuteDataTable("Proc_GetUserProjects", dbParams).ConvertToEntityCollection<Project>();
        }

        /// <summary>
        /// 获取用户信息及密码
        /// </summary>
        public IEnumerable<User> GetUserPassword(string userName)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@UserName", DbType.String, userName, ParameterDirection.Input),
            };
            return this.ExecuteDataTable("Proc_GetUserPassword", dbParams).ConvertToEntityCollection<User>();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return this.ExecuteDataTable("Proc_GetAllUsers").ConvertToEntityCollection<User>();
        }

        public IEnumerable<ProjectUserRole> GetAllProjectUserRoles()
        {
            return this.ExecuteDataTable("Proc_GetAllProjectUserRoles").ConvertToEntityCollection<ProjectUserRole>();
        }

        public bool UpdateUserPassword(long ID, string Password)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.String, ID, ParameterDirection.Input),
                             new DbParam("@Password", DbType.String, Password, ParameterDirection.Input)
                             
                             };
            return base.ExecuteScalar("Proc_UserPassword_Alter", dbParams).ObjectToInt64() >= 0 ? true : false;
        }

        public bool ValidationUser(string username)
        {
            DbParam[] dbParama = {
                                 new DbParam("@User",DbType.String, username, ParameterDirection.Input)
                                 };
            return base.ExecuteScalar("Proc_ValidationUser", dbParama).ObjectToInt64() >= 0 ? true : false;
        }

        public long RegisterUser(string name, string displyName, string password, bool state, char sex, string tel, string mobile, string email, int userType, long customerOrShipperID, out int returnVal)
        {
            int returnvals = 0;
            DbParam[] dbParams = {
                             new DbParam("@Name", DbType.String, name, ParameterDirection.Input),
                             new DbParam("@DisplyName", DbType.String, displyName, ParameterDirection.Input),
                             new DbParam("@Password", DbType.String, password, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input),
                             new DbParam("@Sex", DbType.String, sex, ParameterDirection.Input),
                             new DbParam("@Tel", DbType.String, tel, ParameterDirection.Input),
                             new DbParam("@Mobile", DbType.String, mobile, ParameterDirection.Input),
                             new DbParam("@Email", DbType.String, email, ParameterDirection.Input),
                             new DbParam("@UserType", DbType.Int32, userType, ParameterDirection.Input),
                             new DbParam("@CustomerOrShipperID", DbType.Int64, customerOrShipperID, ParameterDirection.Input),
                             new DbParam("@ReturnVal", DbType.Int32, returnvals, ParameterDirection.Output)
                             };
            long userID = base.ExecuteScalar("Proc_RegisterUser", dbParams).ObjectToInt64();
            returnVal = dbParams[10].Value.ObjectToInt32();
            return userID;

        }


        public IEnumerable<User> GetAllUser()
        {
            string strSQL = " SELECT * FROM dbo.[User]";
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL).ConvertToEntityCollection<User>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 修改用户Token值及有效时间
        /// </summary>
        public bool UpdateToken(long ID, string Str1, string Str2)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.String, ID, ParameterDirection.Input),
                             new DbParam("@Str1", DbType.String, Str1, ParameterDirection.Input),
                             new DbParam("@Str2", DbType.String, Str2, ParameterDirection.Input)
                             
                             };
            return base.ExecuteScalarBySqlString("UPDATE dbo.[User] SET Str1=@Str1,Str2=@Str2 WHERE ID=@ID", dbParams).ObjectToInt64() >= 0 ? true : false;
        }
    }
}