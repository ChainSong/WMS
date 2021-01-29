using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class ShipperService : BaseService
    {
        public Response<IEnumerable<Shipper>> GetShippers()
        {
            Response<IEnumerable<Shipper>> response = new Response<IEnumerable<Shipper>>();
            try
            {
                response.Result = new ShipperAccessor().GetShippers();
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

        public Response<GetShippersByConditionResponse> GetShippersByConditon(GetShippersByConditionRequest request)
        {
            Response<GetShippersByConditionResponse> response = new Response<GetShippersByConditionResponse>() { Result = new GetShippersByConditionResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetShippersByConditon request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                int rowCount;
                response.Result.Shippers = accessor.GetShippersByConditon(request.Code, request.Name, request.EnglishName, request.State, request.PageIndex, request.PageSize,request.ProjectId, out rowCount).ToList();
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

        public Response<Vehicle> AddOrUpdateVehicle(AddOrUpdateVehicleRequest request)
        {
            Response<Vehicle> response = new Response<Vehicle>();
            if (request == null || request.Vehicle == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdateVehicle request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                response.Result = accessor.AddOrUpdateVehicle(request.Vehicle);
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

        public Response<long> AddOrUpdateShipper(AddOrUpdateShipperRequest request)
        {
            Response<long> response = new Response<long>();
            response.Result = 0;
            if (request == null || request.Shipper == null || string.IsNullOrEmpty(request.Shipper.Name) || string.IsNullOrEmpty(request.Shipper.Code))
            {
                ArgumentNullException ex = new ArgumentNullException("InserShipper request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                response.Result = accessor.AddOrUpdateShipper(request.Shipper,request.ProjectId);
                if (response.Result == 0)
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.DataEffective;
                    response.SuccessMessage = "操作失败!";
                }
                else
                {
                    response.IsSuccess = true;
                    response.SuccessMessage = "操作成功!";
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.SuccessMessage = ex.Message;
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
            ShipperAccessor accessor = new ShipperAccessor();
            string ReturnValue = string.Empty;
            try
            {
                if (!accessor.CheckNameIsExist(Name, Id, ProjectID, IsEdit))
                {
                    ReturnValue = "该承运商名称已存在！";
                }
            }
            catch (Exception ex)
            {
                ReturnValue = ex.Message;
                LogError(ex);
            }
            return ReturnValue;
        }

        public Response<IEnumerable<Shipper>> GetShipperList()
        {
            Response<IEnumerable<Shipper>> response = new Response<IEnumerable<Shipper>>();
            ShipperAccessor accessor = new ShipperAccessor();

            try
            {
                response.Result = accessor.GetShipperList("");
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

        public Response<bool> DeleteVehicle(DeleteVehicleRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteVehicle request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                response.Result = accessor.DeleteVehicle(request.ID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<IEnumerable<Vehicle>> GetShipperVehicle(ShipperByIDRequest request)
        {
            Response<IEnumerable<Vehicle>> response = new Response<IEnumerable<Vehicle>>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetShipperVehicle request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                response.Result = new ShipperAccessor().GetShipperVehicle(request.ID);
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

        public Response<Shipper> GetShipperByID(ShipperByIDRequest request)
        {
            Response<Shipper> response = new Response<Shipper>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetShipperByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                response.Result = accessor.GetShipperById(request.ID);
                if (response.Result != null)
                {
                    response.IsSuccess = true;
                    response.SuccessMessage = "操作成功！";
                }
                else
                {
                    response.SuccessMessage = "操作失败！";
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.DataEffective;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.SuccessMessage = ex.Message;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<ProjectShipper>> GetProjectShippers()
        {
            Response<IEnumerable<ProjectShipper>> response = new Response<IEnumerable<ProjectShipper>>();

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                response.Result = accessor.GetProjectShippers();
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

        public Response<ShipperAllInfo> GetShipperAllInfo(GetShipperAllInfoRequest request)
        {
            Response<ShipperAllInfo> response = new Response<ShipperAllInfo>();
            if (request == null || request.ShipperID == 0 || request.ProjectID == 0 || request.RelatedCustomerID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetShipperAllInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                response.Result = accessor.GetShipperAllInfo(request.ShipperID, request.ProjectID, request.RelatedCustomerID);
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

        public Response<bool> ManageShipperEmailInfo(ManageShipperEmailInfoRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ShipperID == 0 || request.ProjectID == 0 || request.RelatedCustomerID == 0 || request.ShipperID == 0
                || request.Type == 0 )
            {
                ArgumentNullException ex = new ArgumentNullException("ManageShipperEmailInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                accessor.ManageShipperEmailInfo(request.ProjectID, request.RelatedCustomerID, request.ShipperID, request.ShipperName, request.EmailAddress, request.EmailContent, request.Type);
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

        public Response<bool> ManageShipperRegionCovered(ManageShipperRegionCoveredRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ShipperRegionCovered.ShipperID == 0 || request.ShipperRegionCovered.ProjectID == 0 || request.ShipperRegionCovered.RelatedCustomerID == 0
                || request.ShipperRegionCovered.ShipperID == 0 || request.ShipperRegionCovered.StartCityID == 0 || request.ShipperRegionCovered.EndCityID == 0 || string.IsNullOrEmpty(request.ShipperRegionCovered.StartCityName) || string.IsNullOrEmpty(request.ShipperRegionCovered.EndCityName))
            {
                ArgumentNullException ex = new ArgumentNullException("ManageShipperRegion request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                accessor.ManageShipperRegionCovered(request.ShipperRegionCovered.ProjectID, request.ShipperRegionCovered.RelatedCustomerID, request.ShipperRegionCovered.ShipperID, request.ShipperRegionCovered.ShipperName,request.ShipperRegionCovered.StartCityID,request.ShipperRegionCovered.StartCityName,request.ShipperRegionCovered.EndCityID,request.ShipperRegionCovered.EndCityName);
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

        public Response<bool> DeleteShipperRegionCovered(DeleteShipperRegionCoveredRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ProjectID == 0 || request.RelatedCustomerID == 0
                || request.ShipperID == 0 || request.StartCityID == 0 || request.EndCityID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteShipperRegionCovered request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                accessor.DeleteShipperRegionCovered(request.ProjectID, request.RelatedCustomerID, request.ShipperID, request.StartCityID, request.EndCityID);
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
    }
}