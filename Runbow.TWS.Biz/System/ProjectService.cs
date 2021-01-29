using System;
using System.Collections.Generic;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using Runbow.TWS.Entity.WMS.Warehouse;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class ProjectService : BaseService
    {
        public Response<Project> GetProjectInfo(GetProjectByProjectIdRequest request)
        {
            Response<Project> response = new Response<Project>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetProjectInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex; 
                return response;
            }

            ProjectAccessor accessor = new ProjectAccessor();

            try
            {
                response.Result = accessor.GetProjectById(request.ProjectID,request.State);
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

        public Response<IEnumerable<Project>> GetAllProjects()
        {
            Response<IEnumerable<Project>> response = new Response<IEnumerable<Project>>();

            ProjectAccessor accessor = new ProjectAccessor();

            try
            {
                response.Result = accessor.GetAllProjects();
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
        /// 查询项目
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Des"></param>
        /// <returns></returns>
        public Response<IEnumerable<Project>> GetAllProjects(string name,string Des,string state)
        {
            Response<IEnumerable<Project>> response = new Response<IEnumerable<Project>>();

            ProjectAccessor accessor = new ProjectAccessor();

            try
            {
                response.Result = accessor.GetAllProjects(name, Des,state);
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


        public Response<GetProjectByProjectIdRequest> SaveProject(GetProjectByProjectIdRequest request)
        {
            Response<GetProjectByProjectIdRequest> response = new Response<GetProjectByProjectIdRequest>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetProject request");
                LogError(ex);
                response.IsSuccess = false;
            }
            ProjectAccessor accessor = new ProjectAccessor();
            Project project = new Project();
            project.Name = request.Name;
            project.Description = request.Description;
            project.Code = request.Code;
            try
            {
                int returnValue = accessor.SaveProject(project);
                if (returnValue == -1)
                {
                    response.SuccessMessage = "公司名称已存在!";
                }
                else
                {
                    response.SuccessMessage = "保存成功！";
                    response.IsSuccess = true;
                }
               
            }
            catch (Exception ex)
            {
                response.SuccessMessage = ex.Message;
                LogError(ex);
            }
            return response;
        }
       
        public bool DeleteProject(long ID)
        {
            ProjectAccessor accessor = new ProjectAccessor();
            try
            {
                return accessor.DeleteP(ID, 0);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// 验证公司编号 唯一性
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string CheckCodeIsExist(string code,int?Id, bool IsEdit)
        {
            ProjectAccessor accessor = new ProjectAccessor();
            string ReturnValue = string.Empty;
            try
            {
                if (!accessor.CheckCodeIsExist(code,Id, IsEdit))
                {
                    ReturnValue = "该公司编号已存在！";
                }
            }
            catch (Exception ex)
            {
                ReturnValue = ex.Message;
                LogError(ex);
            }
            return ReturnValue;
        }

        public Response<Project> UpdateProject(Project project)
        {
            Response<Project> response = new Response<Project>();
            ProjectAccessor accessor = new ProjectAccessor();
            try
            {
                if (accessor.UpdateProject(project) == -1)
                {
                    response.IsSuccess = false;
                    response.SuccessMessage = "公司名称已存在!";
                }
                else
                {
                    response.SuccessMessage = "更新成功";
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
            }
            return response;
        }

        public Response<IEnumerable<ProjectUserCustomer>> GetAllProjectUserCustomers()
        {
            Response<IEnumerable<ProjectUserCustomer>> response = new Response<IEnumerable<ProjectUserCustomer>>();
            try
            {
                response.Result = new ProjectAccessor().GetAllProjectUserCustomers();
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
        public Response<IEnumerable<ProjectCustomerWarehouse>> GetAllProjectCustomersWarehouse()
        {
            Response<IEnumerable<ProjectCustomerWarehouse>> response = new Response<IEnumerable<ProjectCustomerWarehouse>>();
            try
            {
                response.Result = new ProjectAccessor().GetAllProjectCustomersWarehouse();
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
        //库区
        public Response<IEnumerable<AreaInfo>> GetAllProjectCustomersWarehouse_Area()
        {
            Response<IEnumerable<AreaInfo>> response = new Response<IEnumerable<AreaInfo>>();
            try
            {
                response.Result = new ProjectAccessor().GetAllProjectCustomersWarehouse_Area();
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

        //获取用户库区关联
        public Response<IEnumerable<WMS_User_Area_Mapping>> GetAllUser_Area()
        {
            Response<IEnumerable<WMS_User_Area_Mapping>> response = new Response<IEnumerable<WMS_User_Area_Mapping>>();
            try
            {
                response.Result = new ProjectAccessor().GetAllUser_Area();
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

        //库位
        public Response<IEnumerable<LocationInfo>> GetAllProjectCustomersWarehouse_Location()
        {
            Response<IEnumerable<LocationInfo>> response = new Response<IEnumerable<LocationInfo>>();
            try
            {
                response.Result = new ProjectAccessor().GetAllProjectCustomersWarehouse_Location();
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

        //库位
        public Response<IEnumerable<LocationInfo>> GetAllProjectCustomersWarehouse_Location(long Customer)
        {
            Response<IEnumerable<LocationInfo>> response = new Response<IEnumerable<LocationInfo>>();
            try
            {
                response.Result = new ProjectAccessor().GetAllProjectCustomersWarehouse_Location(Customer);
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
        public Response<IEnumerable<ProjectCustomersOrShippers>> GetProjectCustomersOrShippers(GetProjectCustomersOrShippersRequest request)
        {
            Response<IEnumerable<ProjectCustomersOrShippers>> response = new Response<IEnumerable<ProjectCustomersOrShippers>>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetProjectCustomersOrShippers request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                response.Result = new ProjectAccessor().GetProjectCustomersOrShippers(request.Target, request.ProjectID);
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

        public Response<bool> SetProjectUserCustomers(SetProjectUserCustomersRequest request)
        {
            Response<bool> response = new Response<bool>();

            if (request == null || request.UserID == 0 || request.ProjectID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("SetProjectUserCustomers request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ProjectAccessor accessor = new ProjectAccessor();
                response.Result = accessor.SetProjectUserCustomers(request.UserID, request.ProjectID, request.CustomerIDs, request.Creator, request.CreateTime);
                response.IsSuccess = response.Result;
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

        public Response<bool> SetUserProjectRole(SetUserProjectRoleRequest request)
        {
            Response<bool> response = new Response<bool>();

            if (request == null || request.UserID == 0 || request.ProjectID == 0 || request.ProjectRoleID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("SetUserProjectRole request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ProjectAccessor accessor = new ProjectAccessor();
                response.Result = accessor.SetUserProjectRole(request.UserID, request.ProjectRoleID, request.ProjectID);
                response.IsSuccess = response.Result;
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

        public Response<bool> SetProjectCustomerOrShippers(SetProjectCustomerOrShippersRequest request)
        {
            Response<bool> response = new Response<bool>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SetProjectCustomerOrShippers request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ProjectAccessor accessor = new ProjectAccessor();
                response.Result = accessor.SetProjectCustomerOrShippers(request.ProjectID, request.Target, request.ProjectCustomersOrShippers);
                response.IsSuccess = response.Result;
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

        public Response<IEnumerable<ProjectCustomerOrShipperSegment>> GetAllProjectCustomerOrShipperSegments()
        {
            Response<IEnumerable<ProjectCustomerOrShipperSegment>> response = new Response<IEnumerable<ProjectCustomerOrShipperSegment>>();
            try
            {
                ProjectAccessor accessor = new ProjectAccessor();
                response.Result = accessor.GetAllProjectCustomerOrShipperSegments();
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
        public Response<bool> SetProjectCustomerOrShipperSegment(SetProjectCustomerOrShipperSegmentRequest request)
        {
            Response<bool> response = new Response<bool>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SetProjectCustomerOrShipperSegment request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ProjectAccessor accessor = new ProjectAccessor();
                var result = accessor.SetProjectCustomerOrShipperSegment(request.ProjectID, request.Target, request.CustomerOrShipperID, request.SegmentIDs, request.RelatedCustomerIDs);
                if (result == 1)
                {
                    response.Result = true;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = false;
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Argument;
                }
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
        //public Response<IEnumerable<ProjectCustomerOrShipperSegment>> UserWarehouse()
        //{
 
        //}
    }
}