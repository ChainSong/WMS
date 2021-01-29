using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.NikeNFSPrint;
using Runbow.TWS.MessageContracts.NikeOSRBJPrint;
using Runbow.TWS.MessageContracts.WMS.Order;
using Runbow.TWS.MessageContracts.YXDBJRPrint;
using System.Web.Mvc;

namespace Runbow.TWS.Biz
{
    public class OrderECManagementService : BaseService
    {
        public ExpressPackageResponse CheckExpress(string ExpressNumber, long CustomerID, string WarehouseName)
        {
            ExpressPackageResponse EPR = new ExpressPackageResponse();
            try
            {
                OrderECManagementAccessor accessor = new OrderECManagementAccessor();
                EPR = accessor.CheckExpress(ExpressNumber, CustomerID, WarehouseName);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return EPR;
        }
        public DataSet CheckExpress(string ExpressNumber, long CustomerID, string WarehouseName, string Type)
        {
            DataSet EPR = new DataSet();
            try
            {
                OrderECManagementAccessor accessor = new OrderECManagementAccessor();
                EPR = accessor.CheckExpress(ExpressNumber, CustomerID, WarehouseName, Type);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return EPR;
        }

        public DataSet SaveExpressPackage(string ExpressNumber, string PackageType, long CustomerID, string WarehouseName, string PackageCode)
        {
            DataSet EPR = new DataSet();
            try
            {
                OrderECManagementAccessor accessor = new OrderECManagementAccessor();
                EPR = accessor.SaveExpressPackage(ExpressNumber, PackageType, CustomerID, WarehouseName, PackageCode);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return EPR;
        }

        /// <summary>
        /// 获取中间表里面的订单
        /// </summary>
        /// <returns></returns>
        public OrderECModel GetNikeOrderB2C(int type)
        {
            try
            {
                return new OrderECManagementAccessor().GetNikeOrderB2C(type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <returns></returns>
        public bool UpdateNikeOrderStatus(List<string> Numbers)
        {
            try
            {
                return new OrderECManagementAccessor().UpdateNikeOrderStatus(Numbers);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Response<GetOrderByConditionResponse> ExternOrderNumberCheck(List<OrderNumbers> list, long CustomerID)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };
            try
            {
                OrderECManagementAccessor accessor = new OrderECManagementAccessor();
                response.Result = accessor.ExternOrderNumberCheck(list, CustomerID);
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

        public Response<string> UpdateOrderExpressInfo(IEnumerable<OrderInfo> orders, long CustomerID)
        {
            Response<string> response = new Response<string>();
            try
            {
                OrderECManagementAccessor accessor = new OrderECManagementAccessor();
                response.Result = accessor.UpdateOrderExpressInfo(orders, CustomerID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;

            }

            return response;
        }

    }
}
