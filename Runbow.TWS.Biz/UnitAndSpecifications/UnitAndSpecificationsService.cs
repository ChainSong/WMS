using Runbow.TWS.Common;
using Runbow.TWS.Dao.UnitAndSpecifications;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.UnitAndSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Biz.UnitAndSpecifications
{
    public class UnitAndSpecificationsService : BaseService
    {
        public Response<UnitAndSpecificationsResponest> GetUnitAndSpecifications(UnitAndSpecificationsRequest request)
        {
            Response<UnitAndSpecificationsResponest> response = new Response<UnitAndSpecificationsResponest>() { Result = new UnitAndSpecificationsResponest() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCustomerInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            UnitAndSpecificationsAccessor accessor = new UnitAndSpecificationsAccessor();

            try
            {
                response.Result.unitAndSpecificationsInfos = accessor.GetUnitAndSpecifications(request.unitAndSpecificationsInfo);
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
        public Response<UnitAndSpecificationsResponest> AddUnitAndSpecifications(UnitAndSpecificationsRequest request)
        {
            Response<UnitAndSpecificationsResponest> response = new Response<UnitAndSpecificationsResponest>() { Result = new UnitAndSpecificationsResponest() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCustomerInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            UnitAndSpecificationsAccessor accessor = new UnitAndSpecificationsAccessor();

            try
            {
                response.IsSuccess = accessor.AddUnitAndSpecifications(request.unitAndSpecificationsInfo);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public bool DeleteUnitAndSpecifications(int id)
        { 
            UnitAndSpecificationsAccessor accessor = new UnitAndSpecificationsAccessor();
            try
            {
                return accessor.DeleteUnitAndSpecifications(id);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return false;
        }


    }
}
