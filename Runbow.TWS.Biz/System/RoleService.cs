using System;
using System.Collections.Generic;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class RoleService : BaseService
    {
        /// <summary>
        /// 角色新增
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static int roleinsert(RoleRequest r)
        {
            return RoleAccessor.RolePod(r);
        }

        /// <summary>
        /// 检查角色是否已存在
        /// </summary>
        /// <param name="r"></param>
        /// <returns> -1 表示已存在 否则返回即将产生的ID</returns>
        public static string CheckInput(RoleRequest r)
        {
            return RoleAccessor.CheckIput(r);
        }

        public static Role UpdateSelct(int id)
        {
            try
            {
                RoleAccessor r = new RoleAccessor();
                return r.UpdateSelect(id);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static int RoleUpdate(RoleRequest r)
        {

            return RoleAccessor.RoleUpdate(r);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<Role>> GetRole()
        {
            Response<IEnumerable<Role>> response = new Response<IEnumerable<Role>>();
            try
            {
                response.Result = new RoleAccessor().GetRoles();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        /// <summary>
        /// 验证公司编号 唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string CheckNameIsExist(string Name, int? Id, string ProjectID, bool IsEdit)
        {
            RoleAccessor accessor = new RoleAccessor();
            string ReturnValue = string.Empty;
            try
            {
                if (!accessor.CheckNameIsExist(Name, Id, ProjectID, IsEdit))
                {
                    ReturnValue = "该角色名称已存在！";
                }
            }
            catch (Exception ex)
            {
                ReturnValue = ex.Message;
                LogError(ex);
            }
            return ReturnValue;
        }

        /// <summary>
        /// 获取role信息
        /// </summary>
        /// <param name="Sqlwhere"></param>
        /// <returns></returns>
        public Response<RoleRequest> GetRoleInfo(RoleRequest request)
        {
            Response<RoleRequest> response = new Response<RoleRequest>() { Result = new RoleRequest() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetRoleInfoByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                RoleAccessor Roleccessor = new RoleAccessor();
                int Rowcount;


                response.Result.IEnumerableRole = Roleccessor.GetRole(request.Role, request.PageIndex, request.PageSize, request.ProjectId,request.Satate.ToString(), out Rowcount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        /// <summary>
        ///获取某个角色下的菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<ProjectRoleMenu>> GetRoleMenu(RoleRequest request)
        {
            Response<IEnumerable<ProjectRoleMenu>> response = new Response<IEnumerable<ProjectRoleMenu>>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetRoleMenu request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                RoleAccessor accessor = new RoleAccessor();
                response.Result = accessor.GetRoleMenuByProjectRoleID(request.ProjectRoleID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<ProjectRole>> GetRoleInfo(GetRoleByProjectIDRequest request)
        {
            Response<IEnumerable<ProjectRole>> response = new Response<IEnumerable<ProjectRole>>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetRoleInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                RoleAccessor accessor = new RoleAccessor();
                response.Result = accessor.GetRoleByProjectID(request.ProjectID, request.GetAll);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<ProjectRole> GetUserProjectRole(GetUserProjectRoleRequest request)
        {
            Response<ProjectRole> response = new Response<ProjectRole>();

            if (request == null || request.UserID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetUserProjectRole request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                RoleAccessor accessor = new RoleAccessor();
                response.Result = accessor.GetUserProjectRole(request.UserID, request.ProjectID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }



        public Response<IEnumerable<ProjectRole>> GetProjectRoles(GetRoleByProjectIDRequest request)
        {
            Response<IEnumerable<ProjectRole>> response = new Response<IEnumerable<ProjectRole>>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetProjectRoles request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                RoleAccessor accessor = new RoleAccessor();
                response.Result = accessor.GetRoleByProjectID(request.ProjectID, request.GetAll);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<ProjectRole>> GetAllProjectRoles()
        {
            Response<IEnumerable<ProjectRole>> response = new Response<IEnumerable<ProjectRole>>();
            try
            {
                RoleAccessor accessor = new RoleAccessor();
                response.Result = accessor.GetAllProjectRoles();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<bool> SetProjectRole(SetProjectRoleRequest request)
        {
            Response<bool> response = new Response<bool>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SetProjectRole request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                RoleAccessor accessor = new RoleAccessor();
                response.Result = accessor.SetProjectRole(request.ProjectID, request.RoleIDs);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="paremIds"></param>
        /// <returns></returns>
        public Response<bool> DeleteRole(RoleRequest request)
        {
            RoleAccessor ra = new RoleAccessor();
            Response<bool> response = new Response<bool>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SetProjectRole request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                string paremIds = request.Name.ToString();
                if (paremIds.IndexOf(",") > -1)
                    ra.DeleteRole(paremIds.Substring(1), "1",request.ProjectId);  //批量删除
                else
                    ra.DeleteRole(paremIds, "0", request.ProjectId); //单个删除
                response.IsSuccess = true;
                response.SuccessMessage = "删除成功！";
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
                response.SuccessMessage = "删除失败！" + ex;
            }
            return response;
        }
    }
}