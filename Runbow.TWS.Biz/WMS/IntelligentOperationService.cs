using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.IntelligentWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Biz.WMS
{
    public class IntelligentOperationService : BaseService
    {

        public Response<ShelvesPanelResponse> ShelvesPanel(string Id, string WorkStationId)
        {
            Response<ShelvesPanelResponse> response = new Response<ShelvesPanelResponse>() { Result = new ShelvesPanelResponse() };

            try
            {
                IntelligentOperationAccessor accessor = new IntelligentOperationAccessor();
                response.Result = accessor.ShelvesPanel(Id, WorkStationId);
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
        public Response<ShelvesPanelResponse> ShelvesPanel_Receipt(string Id, string WorkStationId)
        {
            Response<ShelvesPanelResponse> response = new Response<ShelvesPanelResponse>() { Result = new ShelvesPanelResponse() };

            try
            {
                IntelligentOperationAccessor accessor = new IntelligentOperationAccessor();
                response.Result = accessor.ShelvesPanel(Id, WorkStationId);
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
        public Response<ShelvesPanelResponse> SubmitData(long ID, long RePickWallDetailId, decimal ActualQty)
        {
            Response<ShelvesPanelResponse> response = new Response<ShelvesPanelResponse>() { Result = new ShelvesPanelResponse() };

            try
            {
                IntelligentOperationAccessor accessor = new IntelligentOperationAccessor();
                response.Result = accessor.SubmitData(ID,RePickWallDetailId, ActualQty);
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
        public Response<ShelvesPanelResponse> SubmitData_Receipt(long ID, decimal ActualQty)
        {
            Response<ShelvesPanelResponse> response = new Response<ShelvesPanelResponse>() { Result = new ShelvesPanelResponse() };

            try
            {
                IntelligentOperationAccessor accessor = new IntelligentOperationAccessor();
                response.Result = accessor.SubmitData_Receipt(ID, ActualQty);
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
        public Response<ShelvesPanelResponse> GetPickUpGoodsWall(long WorkStationId)
        {
            Response<ShelvesPanelResponse> response = new Response<ShelvesPanelResponse>() { Result = new ShelvesPanelResponse() };

            try
            {
                IntelligentOperationAccessor accessor = new IntelligentOperationAccessor();
                response.Result = accessor.GetPickUpGoodsWall(WorkStationId);
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
        public Response<PickUpGoodsManagementResponse> GetGoodsManagement(PickUpGoodsManagementRequest request)
        {
            Response<PickUpGoodsManagementResponse> response = new Response<PickUpGoodsManagementResponse>() { Result = new PickUpGoodsManagementResponse() };
            try
            {
                //int RowCount;
                IntelligentOperationAccessor accessor = new IntelligentOperationAccessor();
                response.Result = accessor.GetGoodsManagement(request);         //, out RowCount
                //response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                //response.Result.PageIndex = request.PageIndex;
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
    }
}
