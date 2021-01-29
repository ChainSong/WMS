using System;
using System.Collections.Generic;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class MenuService : BaseService
    {
        public Response<IEnumerable<CheckedMenu>> GetMenuInfo(GetMenuByProjectRoleIDRequest request)
        {
            Response<IEnumerable<CheckedMenu>> response = new Response<IEnumerable<CheckedMenu>>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMenuInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (projectRoleMenus.ContainsKey(request.ProjectRoleID))
            {
                return projectRoleMenus[request.ProjectRoleID];
            }

            response = this.GetAllCheckedMenuInfo(request);

            if (response.IsSuccess)
            {
                projectRoleMenus[request.ProjectRoleID] = response;
            }

            return response;
        }
        /// <summary>
        /// 直接取数据库菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<CheckedMenu>> GetMenuInfo2(GetMenuByProjectRoleIDRequest request)
        {
            Response<IEnumerable<CheckedMenu>> response = new Response<IEnumerable<CheckedMenu>>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMenuInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            //if (projectRoleMenus.ContainsKey(request.ProjectRoleID))
            //{
            //    return projectRoleMenus[request.ProjectRoleID];
            //}

            response = this.GetAllCheckedMenuInfo(request);

            //if (response.IsSuccess)
            //{
            //    projectRoleMenus[request.ProjectRoleID] = response;
            //}

            return response;
        }

        public Response<IEnumerable<CheckedMenu>> GetAllCheckedMenuInfo(GetMenuByProjectRoleIDRequest request)
        {
            Response<IEnumerable<CheckedMenu>> response = new Response<IEnumerable<CheckedMenu>>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMenuInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MenuAccessor accessor = new MenuAccessor();
                response.Result = accessor.GetMenuByProjectRoleID(request.ProjectRoleID, request.GetAll);
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

        public Response<bool> SetProjectRoleMenu(SetProjectRoleMenuRequest request)
        {
            Response<bool> response = new Response<bool>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SetProjectRoleMenu request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MenuAccessor accessor = new MenuAccessor();
                response.Result = accessor.SetProjectRoleMenu(request.ProjectRoleID, request.MenuIDs);
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

        public Response<bool> RefreshProjectRoleMenus(GetMenuByProjectRoleIDRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("RefreshProjectRoleMenus request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            var menuInfoResponse = GetAllCheckedMenuInfo(request);
            if (menuInfoResponse.IsSuccess)
            {
                projectRoleMenus[request.ProjectRoleID] = menuInfoResponse;
                response.Result = true;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.DataEffective;
            }

            return response;
        }

        private static IDictionary<long, Response<IEnumerable<CheckedMenu>>> projectRoleMenus = new Dictionary<long, Response<IEnumerable<CheckedMenu>>>();
    }
}