using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity.WMS.Inventory;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Inventory;

namespace Runbow.TWS.Biz.WMS
{
    public class ReplenishmentManagementService:BaseService
    {
        public Response<GetReplenishmentDetailByConditionResponse> GetReplenishmentByCondition(GetReplenishmentByConditionRequest request)
        {
            Response<GetReplenishmentDetailByConditionResponse> response = new Response<GetReplenishmentDetailByConditionResponse>() { Result = new GetReplenishmentDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReplenishmentByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReplenishmentManagementAccessor accessor = new ReplenishmentManagementAccessor();
                int RowCount;

                response.Result = accessor.GetReplenishmentByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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

        //明细单条的查看或者编辑
        public Response<ReplenishmentAndReplenishmentDetail> GetReplenishmentInfos(GetReplenishmentByConditionRequest request)
        {
            Response<ReplenishmentAndReplenishmentDetail> response = new Response<ReplenishmentAndReplenishmentDetail>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReplenishmentByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReplenishmentManagementAccessor accessor = new ReplenishmentManagementAccessor();
                response.Result = accessor.GetReplenishmentInfos(request.ID);
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

        public Response<ReplenishmentAndReplenishmentDetail> GenerateReplenishment(IEnumerable<ReplenishmentDetailSKUs> list, string ProjectID, string CustomerID, string CustomerName, string WarehouseID, string WarehouseName, string Remark, string Creator)
        {
            Response<ReplenishmentAndReplenishmentDetail> response = new Response<ReplenishmentAndReplenishmentDetail>();


            try
            {
                ReplenishmentManagementAccessor accessor = new ReplenishmentManagementAccessor();
                response.Result = accessor.GenerateReplenishment(list,ProjectID,CustomerID,CustomerName,WarehouseID,WarehouseName,Remark,Creator);
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

        public Response<ReplenishmentAndReplenishmentDetail> GenerateReplenishmentByNumber(IEnumerable<ReplenishmentDetailSKUs> list, string ProjectID, string CustomerID, string CustomerName, string WarehouseID, string WarehouseName, string Remark, string Creator, int Number)
        {
            Response<ReplenishmentAndReplenishmentDetail> response = new Response<ReplenishmentAndReplenishmentDetail>();


            try
            {
                ReplenishmentManagementAccessor accessor = new ReplenishmentManagementAccessor();
                response.Result = accessor.GenerateReplenishmentByNumber(list, ProjectID, CustomerID, CustomerName, WarehouseID, WarehouseName, Remark, Creator,Number);
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

        //public string Cancel(int ID)
        //{
        //    ReplenishmentManagementAccessor accessor = new ReplenishmentManagementAccessor();
        //    return accessor.Cancel(ID);
        //}
        //取消操作
        public Response<bool> Cancel(int id)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                if (new ReplenishmentManagementAccessor().Cancel(id))
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
        public Response<bool> Complate(int id)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                if (new ReplenishmentManagementAccessor().Complate(id))
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

        //打印补货单
        public Response<GetReplenishmentAndReplenishmentDetailsResponse> PrintReplishmentYFBLD(string rsid)
        {
            Response<GetReplenishmentAndReplenishmentDetailsResponse> response = new Response<GetReplenishmentAndReplenishmentDetailsResponse>()
            {
                Result = new GetReplenishmentAndReplenishmentDetailsResponse()
            };
            //if (request == null)
            //{
            //    ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            try
            {
                ReplenishmentManagementAccessor accessor = new ReplenishmentManagementAccessor();
                response.Result = accessor.PrintReplishmentYFBLD(rsid);
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
    }
}
