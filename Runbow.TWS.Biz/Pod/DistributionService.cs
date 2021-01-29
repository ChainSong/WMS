using Runbow.TWS.Common;
using Runbow.TWS.Dao.POD;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.POD.Adidas;
using Runbow.TWS.MessageContracts.POD.AKZO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Dao;
using Runbow.TWS.MessageContracts.POD.Distribution;

namespace Runbow.TWS.Biz.POD
{
    public class DistributionService : BaseService
    {
        public Response<DistributionPodResponse> QueryOrOperatePod(DistributionPodRequest request)
        {
            Response<DistributionPodResponse> response = new Response<DistributionPodResponse>() { Result = new DistributionPodResponse() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryOrOperatePod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                DistributionAccessor accessor = new DistributionAccessor();
                int rowCount;
                //response.Result.PodCollections = accessor.QueryOrOperatePod(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize, out rowCount).ToList().Distinct(new ComparePod());
                response.Result.PodCollections = accessor.QueryOrOperatePod(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize, out rowCount).ToList();
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = rowCount % request.PageSize == 0 ? rowCount / request.PageSize : rowCount / request.PageSize + 1;
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
        //public class ComparePod : IEqualityComparer<PodToDistribution>
        //{
        //    public bool Equals(PodToDistribution x, PodToDistribution y)
        //    {
        //        return x.ID == y.ID;
        //    }

        //    public int GetHashCode(PodToDistribution obj)
        //    {
        //        return obj.ToString().GetHashCode();
        //    }
        //}
        public Response<bool> SettlePodsDistribution(SettlePodsDistributionRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.SettledPodsDistribution == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SettlePodsDistribution request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                DistributionAccessor accessor = new DistributionAccessor();
                accessor.SettlePodsDistribution(request.SettledPodsDistribution);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<bool> selectPodFee(string podID)
        {
            Response<bool> response = new Response<bool>();
            if (podID == null)
            {
                ArgumentNullException ex = new ArgumentNullException("selectPodFee request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                DistributionAccessor accessor = new DistributionAccessor();
                accessor.selectPodFee(podID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public string selectPodFee()
        {
            DistributionAccessor accessor = new DistributionAccessor();
            return  accessor.selectPodFee();
        }
        public Response<DbToExcelsResponse> DbToExcels(DistributionPodRequest request)
        {
            Response<DbToExcelsResponse> response = new Response<DbToExcelsResponse>() { Result = new DbToExcelsResponse() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DbToExcels request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                DistributionAccessor accessor = new DistributionAccessor();
                response.Result.PodExcel = accessor.DbToExcels(request.SearchCondition);
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
