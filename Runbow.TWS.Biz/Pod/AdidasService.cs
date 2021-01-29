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
using Runbow.TWS.Entity.POD;

namespace Runbow.TWS.Biz
{
    public class AdidasService : BaseService
    {
        /// <summary>
        /// 增加一条扫描数据  未 使用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<ScanInfo> AddScanData(AddScanDataRequest request)
        {
            Response<ScanInfo> response = new Response<ScanInfo>();

            if (request == null || request.scanInfo == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddScanInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (string.IsNullOrEmpty(request.scanInfo.CustomerOrderNumber) || string.IsNullOrEmpty(request.scanInfo.Shipper)
                || request.scanInfo.BoxNumber == 0)
            {
                ArgumentException ex = new ArgumentException("运单号或承运商或箱数不能为空值");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
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
        /// 批量增加扫描数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<ScanInfo>> AddScanDatas(AddScanDatasRequest request) 
        {
            Response<IEnumerable<ScanInfo>> response = new Response<IEnumerable<ScanInfo>>();
            if (request == null || request.scanInfos == null || !request.scanInfos.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddScanDatas request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                AdidasAccessor accessor = new AdidasAccessor();
                string repeatPOD = "";
                string shipperError = "";
                response.Result = accessor.AddScanDatas(request.scanInfos, out repeatPOD, out shipperError);  //导入数据
                if (response.Result.Count() == 0 && string.IsNullOrEmpty(repeatPOD))
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                }
                else
                {
                    response.IsSuccess = true;
                }


                response.SuccessMessage = (string.IsNullOrEmpty(repeatPOD) ? "" : repeatPOD.Split(',')[0])
                    + "," + (string.IsNullOrEmpty(shipperError) ? "" : shipperError.Split(',')[0]);  //将重复的运单号,和错误的运单通过SuccessMessage返回，用于显示
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
        /// 根据扫码查询条件，返回查询结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<QueryScanDataResponses> GetQueryScanDatas(QuerySearchConditionRequest request)
        {

            Response<QueryScanDataResponses> response = new Response<QueryScanDataResponses>();
            response.Result = new QueryScanDataResponses();
            try
            {
                int PageCount = 0;
                response.Result.ScanDataCollection = new AdidasAccessor().GetQueryScanData(request.SearchCondition, request.PageIndex, request.PageSize,out PageCount);
                response.Result.PageCount = PageCount;  //总行数
                response.Result.PageIndex = request.PageIndex;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Result.PageCount = request.PageSize;
                response.Result.PageIndex = 0;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }

        /// <summary>
        /// 关闭运单号
        /// </summary>
        /// <param name="POD"></param>
        /// <returns></returns>
        public bool ClosePOD(string POD) 
        {
            return new AdidasAccessor().ClosePOD(POD);
        }
        /// <summary>
        /// 删除油价报价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool del(int id)
        {
            return new AdidasAccessor().delBAF(id);
          
        }

        /// <summary>
        /// 获取油价表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<ABFPriceResponses> GetABFPrice(ABFPriceRequest request)
        {

            Response<ABFPriceResponses> response = new Response<ABFPriceResponses>();
            response.Result = new ABFPriceResponses();
            try
            {
                //if (request.PageIndex == 0)
                //{///若页码为0，则置为1
                //    request.PageIndex++;
                //}
                int PageCount = 0;
                response.Result.bafPriceInfo = new AdidasAccessor().GetABFPrice(request.BAFStartTime, request.BAFEndTime, request.PageIndex, request.PageSize, out PageCount);
                response.Result.PageCount = PageCount / request.PageSize + PageCount % request.PageSize > 0 ? 1 : 0;  //获取总页数
                response.Result.PageIndex = request.PageIndex;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Result.PageCount = request.PageSize;
                response.Result.PageIndex = 0;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }



        /// <summary>
        /// 添加油价
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool addABFPrice(ABFPriceRequest request)
        {
            Response<ABFPriceResponses> response = new Response<ABFPriceResponses>();
            response.Result = new ABFPriceResponses();
            bool boo = false;
            try
            {
                BAFPriceInfo BAF = new BAFPriceInfo();
                IList<BAFPriceInfo> Price = new List<BAFPriceInfo>();
                Price.Add(request.Info);
                  boo = new AdidasAccessor().AddABFPrice(Price);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Result.PageCount = request.PageSize;
                response.Result.PageIndex = 0;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return boo;
        }
    }
}
