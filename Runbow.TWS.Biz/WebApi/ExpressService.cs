using Runbow.TWS.Common;
using Runbow.TWS.Dao.WebApi;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Runbow.TWS.MessageContracts.WebApi.Express;
using Runbow.TWS.Entity.WebApi.Express;

namespace Runbow.TWS.Biz.WebApi
{
    public class ExpressService : BaseService
    {



        public Response<YtoRequest> GetExpressNumYto(string ids)
        {


            Response<YtoRequest> response = new Response<YtoRequest>() { Result = new YtoRequest() };
            try
            {
                response.Result = new ExpressAccessor().GetExpressNumYto(ids);
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


        public Response<int> InsExpressNumYto(YtoRequest ytoRequest)
        {


            Response<int> response = new Response<int>() { Result = 0 };
            try
            {
                response.Result = new ExpressAccessor().InsExpressNumYto(ytoRequest);

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

        public Response<ExpressResponse> GetOrderByYD(string PackageNumber)
        {
            Response<ExpressResponse> response = new Response<ExpressResponse>() { Result = new ExpressResponse() };
            try
            {
                ExpressAccessor accessor = new ExpressAccessor();
                response.Result = accessor.GetOrderByYD(PackageNumber);
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

        public Response<string> AddExpressAndUpdatePackageYD(YdResponseParam responseParam, Entity.PackageInfo package, PdfInfoObj obj)
        {
            Response<string> response = new Response<string>();
            try
            {
                ExpressAccessor accessor = new ExpressAccessor();
                response.Result = accessor.AddExpressAndUpdatePackageYD(responseParam, package, obj);
                if (response.Result == "")
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.Result = ex.Message;
            }
            return response;
        }


        public Response<ExpressResponse> GetOrder(string PackageNumber, string OrderType)
        {
            Response<ExpressResponse> response = new Response<ExpressResponse>() { Result = new ExpressResponse() };

            try
            {
                ExpressAccessor accessor = new ExpressAccessor();
                response.Result = accessor.GetOrder(PackageNumber, OrderType);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = Common.ErrorCode.Technical;
            }

            return response;
        }

        public Response<ExpressResponse> GetOrderByExpress(string PackageNumber, string OrderType)
        {
            Response<ExpressResponse> response = new Response<ExpressResponse>() { Result = new ExpressResponse() };

            try
            {
                ExpressAccessor accessor = new ExpressAccessor();
                response.Result = accessor.GetOrderByExpress(PackageNumber, OrderType);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = Common.ErrorCode.Technical;
            }

            return response;
        }

        public void AddExpressAndUpdatePackage(DPCreateOrderResponse response, IEnumerable<Entity.PackageInfo> packages)
        {
            try
            {
                ExpressAccessor accessor = new ExpressAccessor();
                packages.ForEach(item => accessor.AddExpressAndUpdatePackage(response, item));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
