using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.ShipperManagement;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.ShipperManagement;

namespace Runbow.TWS.Biz
{
    public class ShipperManagementService : BaseService
    {
        public Response<CRMShipperInfo> GetCRMShipperInfo(CRMShipperOperationRequest request)
        {
            Response<CRMShipperInfo> response = new Response<CRMShipperInfo>();

            if (request == null || !request.CRMShipperID.HasValue || request.CRMShipperID.Value == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMShipperInfo request CRMShipperID");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                response.Result = accessor.GetCRMShipperInfoByID(request.CRMShipperID.Value);
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

        public Response<GetCRMShippersByConditionResponse> GetCRMShippersByCondition(GetCRMShippersByConditionRequest request)
        {
            Response<GetCRMShippersByConditionResponse> response = new Response<GetCRMShippersByConditionResponse>() { Result = new GetCRMShippersByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMShippersByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                int RowCount ;
                if (request.PageSize > 0)
                {
                    response.Result.CRMShipperCollection = accessor.GetCRMShippersByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMShipperCollection = accessor.GetCRMShippersByConditionNoPaging(request.SearchCondition);
                }
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

        public Response<IEnumerable<CRMShipperCooperation>> GetCRMShipperCooperations(CRMShipperOperationRequest request)
        {
            Response<IEnumerable<CRMShipperCooperation>> response = new Response<IEnumerable<CRMShipperCooperation>>();

            if (request == null || !request.CRMShipperID.HasValue || request.CRMShipperID.Value == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMShipper request CRMShipperID");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                response.Result = accessor.GetCRMShipperCooperationsByCRMShipperID(request.CRMShipperID.Value);
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

        public Response<IEnumerable<CRMShipperTerminalInfo>> GetCRMShipperTerminalInfos(CRMShipperOperationRequest request)
        {
            Response<IEnumerable<CRMShipperTerminalInfo>> response = new Response<IEnumerable<CRMShipperTerminalInfo>>();

            if (request == null || !request.CRMShipperID.HasValue || request.CRMShipperID.Value == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMShipperTerminalInfos request CRMShipperID");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                response.Result = accessor.GetCRMShipperTerminalInfosByCRMShipperID(request.CRMShipperID.Value);
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

        public Response<IEnumerable<CRMShipperTransportationLine>> GetCRMShipperTransportationLines(CRMShipperOperationRequest request)
        {
            Response<IEnumerable<CRMShipperTransportationLine>> response = new Response<IEnumerable<CRMShipperTransportationLine>>();

            if (request == null || !request.CRMShipperID.HasValue || request.CRMShipperID.Value == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMShipper request CRMShipperID");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                response.Result = accessor.GetCRMShipperTransportationLinesByCRMShipperID(request.CRMShipperID.Value);
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

        public Response<bool> DeleteCRMShipper(CRMShipperOperationRequest request)
        {
            Response<bool> response = new Response<bool> { Result = false };

            if (request == null || !request.CRMShipperID.HasValue || request.CRMShipperID.Value == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMShipper request CRMShipperID");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                accessor.DeleteCRMShipper(request.CRMShipperID.Value);
                response.Result = true;
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

        public Response<bool> DeleteCRMShipperTransportationLine(CRMShipperOperationRequest request)
        {
            Response<bool> response = new Response<bool> { Result = false };

            if (request == null || !request.CRMShipperTransportationLineID.HasValue || request.CRMShipperTransportationLineID.Value == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteCRMShipperTransportationLine request CRMShipperTransportationLineID");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                accessor.DeleteCRMShipperTransportationLine(request.CRMShipperTransportationLineID.Value);
                response.Result = true;
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

        public Response<bool> DeleteCRMShipperTerminalInfo(CRMShipperOperationRequest request)
        {
            Response<bool> response = new Response<bool> { Result = false };

            if (request == null || !request.CRMShipperTerminalInfoID.HasValue || request.CRMShipperTerminalInfoID.Value == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteCRMShipperTerminalInfo request CRMShipperTerminalInfoID");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                accessor.DeleteCRMShipperTerminalInfo(request.CRMShipperTerminalInfoID.Value);
                response.Result = true;
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

        public Response<bool> DeleteCRMShipperCooperation(CRMShipperOperationRequest request)
        {
            Response<bool> response = new Response<bool> { Result = false };

            if (request == null || !request.CRMShipperCooperationID.HasValue || request.CRMShipperCooperationID.Value == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMShipper request CRMShipperCooperationID");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                accessor.DeleteCRMShipperCooperation(request.CRMShipperCooperationID.Value);
                response.Result = true;
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

        public Response<IEnumerable<long>> AddOrUpdateCRMShippers(AddOrUpdateCRMShippersRequest request)
        {
            Response<IEnumerable<long>> response = new Response<IEnumerable<long>>();

            if (request == null || request.CRMShipperCollection == null || !request.CRMShipperCollection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdateCRMShippers request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                response.Result = accessor.AddOrUpdateCRMShippers(request.CRMShipperCollection);
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

        public Response<IEnumerable<long>> AddOrUpdateCRMShipperCooperations(AddOrUpdateCRMShipperCooperationsRequest request)
        {
            Response<IEnumerable<long>> response = new Response<IEnumerable<long>>();

            if (request == null || request.CRMShipperCooperationCollection == null || !request.CRMShipperCooperationCollection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdateCRMShipperCooperations request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                response.Result = accessor.AddOrUpdateCRMShipperCooperations(request.CRMShipperCooperationCollection);
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

        public Response<IEnumerable<CRMShipperTransportationLine>> AddOrUpdateCRMShipperTransportationLines(AddOrUpdateCRMShipperTransportationLineRequest request)
        {
            Response<IEnumerable<CRMShipperTransportationLine>> response = new Response<IEnumerable<CRMShipperTransportationLine>>();

            if (request == null || request.CRMShipperTransportationLineCollection == null || !request.CRMShipperTransportationLineCollection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdateCRMShipperTransportationLines request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                response.Result = accessor.AddOrUpdateCRMShipperTransportationLines(request.CRMShipperTransportationLineCollection);
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

        public Response<IEnumerable<CRMShipperTerminalInfo>> AddOrUpdateCRMShipperTerminalInfos(AddOrUpdateCRMShipperTerminalInfoRequest request)
        {
            Response<IEnumerable<CRMShipperTerminalInfo>> response = new Response<IEnumerable<CRMShipperTerminalInfo>>();

            if (request == null || request.CRMShipperTerminalInfoCollection == null || !request.CRMShipperTerminalInfoCollection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdateCRMShipperTerminalInfos request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                response.Result = accessor.AddOrUpdateCRMShipperTerminalInfos(request.CRMShipperTerminalInfoCollection);
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

        /// <summary>
        /// 查询当前承运商已选择的车辆
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<ShipperToVehicle>> GetShipperToVehicle(ShipperToVehicleRequest request)
        {

            Response<IEnumerable<ShipperToVehicle>> response = new Response<IEnumerable<ShipperToVehicle>>();

            if (request == null || request.SID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetShipperToVehicle request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ShipperManagementAccessor stv = new ShipperManagementAccessor();
                response.Result = stv.GetShipperToVehicle(request.SID);
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


        //所有承运商
        public Response<IEnumerable<CRMShipper>> GetShipperList()
        {
            Response<IEnumerable<CRMShipper>> response = new Response<IEnumerable<CRMShipper>>();

            try
            {
                response.Result = new ShipperManagementAccessor().GetAllShippers();
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

        //批量导入承运商
        public bool InsertCRMShipperExecl(GetCRMShippersByConditionRequest request)
        {
            bool IsSuccess = false;

            try
            {
                ShipperManagementAccessor accessor = new ShipperManagementAccessor();
                IsSuccess = accessor.InsertCRMShipperExecl(request.InsertShipper );

            }
            catch (Exception ex)
            {
                LogError(ex);
                IsSuccess = false;
            }

            return IsSuccess;
        }

    }
}
