using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.DeliverConfirm;

namespace Runbow.TWS.Biz.WMS
{
    public class DeliverConfirmService : BaseService
    {
        /// <summary>
        /// 交接单列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetDeliverByConditionResponse> GetDeliverHeaderByCondition(GetDeliverByConditionRequest request)
        {
            Response<GetDeliverByConditionResponse> response = new Response<GetDeliverByConditionResponse>() { Result = new GetDeliverByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetDeliverByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
                int RowCount;
                response.Result = accessor.GetDeliverHeaderByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        /// 交接单明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Response<GetDeliverByConditionResponse> GetDeliverHeaderAndDetailByID(int ID)
        {
            Response<GetDeliverByConditionResponse> response = new Response<GetDeliverByConditionResponse>() { Result = new GetDeliverByConditionResponse() };
            try
            {
                DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
                response.Result = accessor.GetDeliverHeaderAndDetailByID(ID);
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
        /// 交接称重快递单验证
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public string VilidateDeliverExpress(string express, long customerID, string warehouse)
        {
            string resualt = "";
            try
            {
                DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
                resualt = accessor.VilidateDeliverExpress(express, customerID, warehouse);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return resualt;
        }

        /// <summary>
        /// 获取待上传信息
        /// </summary>
        /// <param name="express"></param>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public Response<GetDeliverByConditionResponse> GetDeliverUploadData(string express, long customerid, string warehouse)
        {
            Response<GetDeliverByConditionResponse> response = new Response<GetDeliverByConditionResponse>() { Result = new GetDeliverByConditionResponse() };
            try
            {
                DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
                response.Result = accessor.GetDeliverUploadData(express, customerid, warehouse);
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
        /// 验证订单是否是取消单
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public string ValidOrderCancel(string express, long customerID, string Proc, string warehouse, int type)
        {
            string resualt = "";
            try
            {
                DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
                resualt = accessor.ValidOrderCancel(express, customerID, Proc, warehouse, type);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return resualt;
        }

        /// <summary>
        /// 验证订单是否是取消单
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public IEnumerable<PreOrder> ValidOrderCancel(IEnumerable<PreOrderSearchCondition> preoders, string Proc, int type)
        {

            try
            {
                DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
                return accessor.ValidOrderCancel(preoders, Proc, type);
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliverID"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfo> ValidDeliverOrderCancel(long deliverID)
        {
            try
            {
                return new DeliverConfirmAccessor().ValidDeliverOrderCancel(deliverID);
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        /// <summary>
        /// 新增交接清单及明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<string> AddDeliverAndDetail(AddDeliverAndDetailRequest request, int flag)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new DeliverConfirmAccessor().AddDeliverAndDetail(request, flag);
                //if (message == "")
                //{
                //    response.Result = message;
                //    response.IsSuccess = true;
                //}
                //else
                //{
                //    response.Result = message;
                //    response.IsSuccess = false;

                //}
                //return response;
                response.Result = message;
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                //response.Result = message + ex.Message;
                response.Result = "";
            }
            return response;
        }


        /// <summary>
        /// 根据快递单号删除交接明细
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="DeliverKey"></param>
        /// <param name="ExpressNumber"></param>
        /// <returns></returns>
        public string DeliverDetailDelete(long customerID, string DeliverKey, string ExpressNumber, string warehouse)
        {
            string result = "";
            try
            {
                DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
                if (accessor.DeliverDetailDelete(customerID, DeliverKey, ExpressNumber, warehouse))
                    result = "";
            }
            catch (Exception ex)
            {
                result = "-1";

            }

            return result;
        }

        /// <summary>
        /// 打印交接清单
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="deliverID"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public Response<GetDeliverByConditionResponse> GetPrintDelivery(long deliverID, string Proc)
        {
            Response<GetDeliverByConditionResponse> response = new Response<GetDeliverByConditionResponse>() { Result = new GetDeliverByConditionResponse() };
            try
            {
                DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
                response.Result = accessor.GetPrintDelivery(deliverID, Proc);
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
        /// 交接单在提交出库时验证交接明细
        /// </summary>
        /// <param name="DeliverID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public string DeliverCompleteInfoValidate(long DeliverID, long customerID)
        {
            string resualt = "";
            try
            {
                DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
                resualt = accessor.DeliverCompleteInfoValidate(DeliverID, customerID);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return resualt;
        }


        /// <summary>
        /// 交接单提交出库
        /// </summary>
        /// <param name="DeliverID"></param>
        /// <returns></returns>    
        public Response<string> DeliverOut(long DeliverID)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new DeliverConfirmAccessor().DeliverOut(DeliverID);
                if (message == "")
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
        /// 验证对应快递单是否为可出库状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WMS_CheckExpress> CheckExpressStatus(long DeliverID, long customerID)
        {
            List<WMS_CheckExpress> response = new List<WMS_CheckExpress>();

            DeliverConfirmAccessor accessor = new DeliverConfirmAccessor();
            try
            {
                response = accessor.CheckExpressStatus(DeliverID, customerID);

            }
            catch
            {
                response = null;
            }
            return response;
        }


    }
}
