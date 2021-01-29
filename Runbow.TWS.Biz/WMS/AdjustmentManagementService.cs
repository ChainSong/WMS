using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Biz.WMS
{
    public class AdjustmentManagementService : BaseService
    {
        public Response<GetAdjustmentDetailByConditionResponse> GetAdjustmentByCondition(GetAdjustmentByConditionRequest request)
        {
            Response<GetAdjustmentDetailByConditionResponse> response = new Response<GetAdjustmentDetailByConditionResponse>() { Result = new GetAdjustmentDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiptByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AdjustmentManagementAccessor accessor = new AdjustmentManagementAccessor();
                int RowCount;

                response.Result = accessor.GetadjustByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        /// <summary>
        ///导出库存变更数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetAdjustmentDetailByConditionResponse> ExportAdjustmentByCondition(GetAdjustmentByConditionRequest request)
        {
            Response<GetAdjustmentDetailByConditionResponse> response = new Response<GetAdjustmentDetailByConditionResponse>() { Result = new GetAdjustmentDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetExportByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                AdjustmentManagementAccessor accessor = new AdjustmentManagementAccessor();               
                response.Result = accessor.ExportadjustByCondition(request.SearchCondition);               
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
        public Response<AdjustmentAndAdjustmentDetail> GetAdjustmentInfos(GetAdjustmentByConditionRequest request)
        {
            Response<AdjustmentAndAdjustmentDetail> response = new Response<AdjustmentAndAdjustmentDetail>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetAdjustmentByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AdjustmentManagementAccessor accessor = new AdjustmentManagementAccessor();
                response.Result = accessor.GetAdjustmentInfos(request.ID);
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
        //取消操作
        public Response<bool> Cancel(int id)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                if (new AdjustmentManagementAccessor().Cancel(id))
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
        //解冻操作
        public Response<bool> Unfreeze(int id)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                if (new AdjustmentManagementAccessor().Unfreeze(id))
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
        //批量取消操作
        public bool Cancels(string asnid)
        {
            bool ve = true;
            try
            {
                ve = new AdjustmentManagementAccessor().Cancels(asnid);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return ve;
        }

        //完成单条
        public Response<bool> Complets(int id, string type)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string result = new AdjustmentManagementAccessor().Complets(id, type);
                if (result.Contains("提交成功"))
                {
                    response.IsSuccess = true;
                    response.SuccessMessage = "操作成功";
                }
                else
                {
                    response.IsSuccess = false;
                    response.SuccessMessage = result;
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
        //批量完成
        public Response<bool> PLComplet(string ID, string type)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string result = new AdjustmentManagementAccessor().PLComplet(ID, type);
                if (result.Contains("提交成功"))
                {
                    response.IsSuccess = true;
                    response.SuccessMessage = "操作成功";
                }
                else
                {
                    response.IsSuccess = false;
                    response.SuccessMessage = result;
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

        public Response<IEnumerable<LocationInfo>> GetLocationList()
        {
            Response<IEnumerable<LocationInfo>> response = new Response<IEnumerable<LocationInfo>>();
            try
            {
                response.Result = new AdjustmentManagementAccessor().GetLocationList();
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

        public Response<IEnumerable<Inventorys>> GetInventoryLocationList(string location, string warehouse, string Customer,string StoreCode)
        {
            Response<IEnumerable<Inventorys>> response = new Response<IEnumerable<Inventorys>>();
            try
            {
                response.Result = new AdjustmentManagementAccessor().GetInventoryLocationList(location, warehouse, Customer, StoreCode);
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
        /// 暂存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<string> AddAdjustmentANDAdjustmentDetail(AddAdjustmentandAdjustmentDetailRequest request)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (request == null || request.adjustment == null || !request.adjustment.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddAdjustmentANDAdjustmentDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new AdjustmentManagementAccessor().AddAdjustmentANDAdjustmentDetail(request);
                if (message.Contains("添加成功"))
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;
        }
        public string CheckAdjustData(AddAdjustmentandAdjustmentDetailRequest request)
        {
            if (request == null || request.adjustmentDetails == null || !request.adjustmentDetails.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddAdjustmentANDAdjustmentDetail request");
                return "数据不能为空";
            }
            try
            {
                return new AdjustmentManagementAccessor().CheckAdjustData(request);


            }
            catch (Exception ex)
            {
                return "验证数据失败，请重试";
            }
        }
        /// <summary>
        /// 更新暂存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<string> UpdateAdjustmentANDAdjustmentDetail(AddAdjustmentandAdjustmentDetailRequest request)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (request == null || request.adjustment == null || !request.adjustment.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateAdjustmentANDAdjustmentDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new AdjustmentManagementAccessor().UpdateAdjustmentANDAdjustmentDetail(request);
                if (message == "更新成功")
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<string> UpdateAndInsertInventory(AddAdjustmentandAdjustmentDetailRequest request, string Inventorytype)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (request == null || request.adjustmentDetails == null || !request.adjustmentDetails.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddInventroyRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new AdjustmentManagementAccessor().UpdateAndInsertInventory(request, Inventorytype);
                if (message.Contains("提交成功"))
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;
        }
    }
}
