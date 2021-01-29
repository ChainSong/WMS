using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WebApi;
using Runbow.TWS.Entity.WMS.OnlineOrder;

namespace Runbow.TWS.Biz.WMS
{
    public class DeliveryOrderService : BaseService
    {
        DeliveryOrderAccessor accessor = new DeliveryOrderAccessor();
        public Response<DeliveryOrderManagementResponse> GetDeliveryOrderBindingBLL(DeliveryOrderManagementRequest request)
        {
            Response<DeliveryOrderManagementResponse> response = new Response<DeliveryOrderManagementResponse>() { Result = new DeliveryOrderManagementResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("pro_wms_SampleInventory request");
                response.Exception = ex;
                return response;
            }
            try
            {
                response.Result = accessor.GetDeliveryOrderBindingDAL(request);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
            }

            return response;
        }
        public string SaveOrderBLL(DeliveryOrderManagementRequest request)
        {
            string str = string.Empty;
            try
            {
                str= accessor.SaveOrderDAL(request);
            
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return str;
        }
        public string CreateOrderSubmitBLL(ConsigneeSearchCondition consignee)
        {
            return accessor.CreateOrderSubmitDAL(consignee);
        }
        public Response<DeliveryOrderManagementResponse> GetApplicationRecordBLL(DeliveryOrderManagementRequest request)
        {
            Response<DeliveryOrderManagementResponse> response = new Response<DeliveryOrderManagementResponse>() { Result = new DeliveryOrderManagementResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("pro_wms_UserInventory request");
                response.Exception = ex;
                return response;
            }
            try
            {
                response.Result = accessor.GetApplicationRecordDAL(request);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
            }

            return response;
        }
        
        public Response<DeliveryOrderManagementResponse> GetReturnOrderBLL(DeliveryOrderManagementRequest request)
        {
            Response<DeliveryOrderManagementResponse> response = new Response<DeliveryOrderManagementResponse>() { Result = new DeliveryOrderManagementResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("pro_wms_SampleReturnOrder request");
                response.Exception = ex;
                return response;
            }
            try
            {
                response.Result = accessor.GetReturnOrderDAL(request);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
            }

            return response;
        }

        public Response<DeliveryOrderManagementResponse> GetOrderDetailBLL(string OrderKey)
        {
            Response<DeliveryOrderManagementResponse> response = new Response<DeliveryOrderManagementResponse>() { Result = new DeliveryOrderManagementResponse() };
            if (OrderKey == null || OrderKey == "")
            {
                ArgumentNullException ex = new ArgumentNullException("pro_wms_SampleReturnOrderDetail request");
                response.Exception = ex;
                return response;
            }
            try
            {
                response.Result = accessor.GetOrderDetailDAL(OrderKey);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
            }

            return response;
        }

        public string GetReturnOrderBLL(string UserName)
        {
            return  accessor.GetReturnOrderDAL(UserName);
            
        }
    }
}
