using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class UserService : BaseService
    {
        public Response<User> GetUserInfo(GetUserByUserIdRequest request)
        {
            Response<User> response = new Response<User>();
            if (request == null || request.UserID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetUserInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                UserAccessor accessor = new UserAccessor();
                response.Result = accessor.GetUserById(request.UserID);
                if (response.Result == null)
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.DataEffective;
                }
                else
                {
                    response.IsSuccess = true;
                }
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
        /// 验证登录名称 唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string CheckNameIsExist(string Name, int? Id, bool IsEdit)
        {
            UserAccessor accessor = new UserAccessor();
            string ReturnValue = string.Empty;
            try
            {
                if (!accessor.CheckNameIsExist(Name, Id, IsEdit))
                {
                    ReturnValue = "该登录名称已存在！";
                }
            }
            catch (Exception ex)
            {
                ReturnValue = ex.Message;
                LogError(ex);
            }
            return ReturnValue;
        }

        public Response<GetUsersByConditionResponse> GetUsersByConditon(GetUsersByConditionRequest request)
        {
            Response<GetUsersByConditionResponse> response = new Response<GetUsersByConditionResponse>() { Result = new GetUsersByConditionResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetUsersByConditon request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                UserAccessor accessor = new UserAccessor();
                int rowCount;
                response.Result.Users = accessor.GetUsersByConditon(request.Name, request.DisplyName, request.State, request.UserType, request.PageIndex, request.PageSize,request.ProjectID, out rowCount).ToList();
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = rowCount % request.PageSize == 0 ? rowCount / request.PageSize : rowCount / request.PageSize + 1;
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

        public Response<User> CheckLogin(GetUserByCheckLoginRequest request)
        {
            Response<User> response = new Response<User>();
            if (request == null || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Password) || request.ProjectID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("CheckLogin request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            UserAccessor accessor = new UserAccessor();
            try
            {
                response.Result = accessor.CheckLoginUser(request.Name, request.Password, request.ProjectID);
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

        public Response<long> AddUser(UserRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Password))
            {
                ArgumentNullException ex = new ArgumentNullException("AddUser request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            UserAccessor accessor = new UserAccessor();
            try
            {
                int returnVal = 0;
                long userID = accessor.AddUser(request.Name, request.DisplayName, request.Password, request.State, request.Sex, request.Tel, request.Mobile, request.Email, request.UserType, request.CustomerOrShipperID,request.RuleArea, out returnVal);
                if (returnVal == 1)
                {
                    response.IsSuccess = true;
                    response.Result = userID;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = returnVal;
                }
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
        /// 新增用户时 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<long> AddUser2(UserRequest request,string UserName,string RoleId,string ProjectId)
        {
            Response<long> response = new Response<long>();
            if (request == null || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Password))
            {
                ArgumentNullException ex = new ArgumentNullException("AddUser request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            UserAccessor accessor = new UserAccessor();
            try
            {
                int returnVal = 0;
                long userID = accessor.AddUser2(request.Name, request.DisplayName, request.Password, request.State, request.Sex, request.Tel, request.Mobile, request.Email, request.UserType, request.CustomerOrShipperID, request.RuleArea, UserName, RoleId, int.Parse(ProjectId), out returnVal);
                if (returnVal > 0)
                {
                    response.IsSuccess = true;
                    response.Result = userID;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = returnVal;
                }
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
        /// 获取某个项目下的用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<UserRequest>> GetUserByProjetId(UserRequest request)
        {
            Response<IEnumerable<UserRequest>> response = new Response<IEnumerable<UserRequest>>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetUserByProjetId request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                UserAccessor accessor = new UserAccessor();
                accessor.GetUserByProjectId(request.ProjectId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<bool> EditUser(UserRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0 || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.DisplayName))
            {
                ArgumentNullException ex = new ArgumentNullException("EditUser request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                UserAccessor accessor = new UserAccessor();
                accessor.UpdateUser(request.ID, request.Name,request.UserName,request.ProjectId, request.ProjectRoleId,request.DisplayName, request.State, request.Sex, request.Tel, request.Mobile, request.Email, request.UserType, request.CustomerOrShipperID, request.RuleArea);
                response.IsSuccess = true;
                response.Result = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<Project>> GetUserProjects(GetUserProjectsRequest request)
        {
            Response<IEnumerable<Project>> response = new Response<IEnumerable<Project>>();
            if (request == null || string.IsNullOrEmpty(request.UserName))
            {
                ArgumentNullException ex = new ArgumentNullException("GetUserProjects request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            UserAccessor accessor = new UserAccessor();

            try
            {
                response.Result = accessor.GetUserProjects(request.UserName);
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
        /// 根据用户登录名获取密码相关信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<User>> GetUserPassword(GetUserProjectsRequest request)
        {
            Response<IEnumerable<User>> response = new Response<IEnumerable<User>>();
            if (request == null || string.IsNullOrEmpty(request.UserName))
            {
                ArgumentNullException ex = new ArgumentNullException("GetUserPassword request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            UserAccessor accessor = new UserAccessor();

            try
            {
                response.Result = accessor.GetUserPassword(request.UserName);
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


        public Response<IEnumerable<User>> GetAllUsers()
        {
            Response<IEnumerable<User>> response = new Response<IEnumerable<User>>();

            try
            {
                response.Result = new UserAccessor().GetAllUsers();
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

        public Response<IEnumerable<ProjectUserRole>> GetAllProjectUserRoles()
        {
            Response<IEnumerable<ProjectUserRole>> response = new Response<IEnumerable<ProjectUserRole>>();

            try
            {
                response.Result = new UserAccessor().GetAllProjectUserRoles();
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





        public Response<bool> UpdateUserPassword(UserRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateUserPassword request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            UserAccessor accessor = new UserAccessor();
            try
            {
               
                bool result = accessor.UpdateUserPassword(request.ID, request.Password);
                if (result)
                {
                    response.Result = true;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = false;
                    response.IsSuccess = false;
                }
                
               
                
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }


        public Response<bool> ValidationUser(UserRequest request)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                UserAccessor accessor = new UserAccessor();
                bool result = accessor.ValidationUser(request.Name);
                if (result)
                {
                    response.Result = true;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = false;
                    response.IsSuccess = false;
                }

            }
            catch(Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }


        public Response<long> RegisterUser(UserRequest request)
        {
            Response<long> response = new Response<long>();

            UserAccessor accessor = new UserAccessor();
            try
            {
                int returnVal = 0;
                long userID = accessor.RegisterUser(request.Name, request.DisplayName, request.Password, request.State, request.Sex, request.Tel, request.Mobile, request.Email, request.UserType, request.CustomerOrShipperID, out returnVal);
                if (returnVal == 1)
                {
                    response.IsSuccess = true;
                    response.Result = userID;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = returnVal;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<User>> GetAllUser()
        {
            Response<IEnumerable<User>> response = new Response<IEnumerable<User>>();

            UserAccessor accessor = new UserAccessor();

            try
            {
                response.Result = accessor.GetAllUser();
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
        /// 修改用户Token值及有效时间
        /// </summary>
        public Response<bool> UpdateToken(long ID, string Str1, string Str2)
        {
            Response<bool> response = new Response<bool>();

            UserAccessor accessor = new UserAccessor();
            try
            {

                bool result = accessor.UpdateToken(ID, Str1, Str2);
                if (result)
                {
                    response.Result = true;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = false;
                    response.IsSuccess = false;
                }



            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
    }
}