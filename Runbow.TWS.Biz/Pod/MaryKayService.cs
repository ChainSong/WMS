using Runbow.TWS.Common;
using Runbow.TWS.Dao.POD;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.POD.MaryKay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using System.Data;

namespace Runbow.TWS.Biz.POD
{
    public class MaryKayService : BaseService
    {
        public Response<GetMaryKayOrderIssuedRequest> GetMaryKayGetOrderIssued(GetMaryKayOrderIssuedRequest request)
        {
            Response<GetMaryKayOrderIssuedRequest> response = new Response<GetMaryKayOrderIssuedRequest>() { Result = new GetMaryKayOrderIssuedRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetAbnormalPODSearch request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                int Rowcount;
                response.Result.OrderNoIssuedTable = accessor.GetMaryKayGetOrderIssued(request.SqlWhere, request.PageIndex, request.PageSize, out Rowcount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        public Response<GetMaryKayOrderIssuedRequest> GetOrderNoIssuedInfoByID(GetMaryKayOrderIssuedRequest request)
        {
            Response<GetMaryKayOrderIssuedRequest> response = new Response<GetMaryKayOrderIssuedRequest>() { Result = new GetMaryKayOrderIssuedRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetOrderNoIssuedInfoByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                response.Result.PODEntity = accessor.GetOrderNoIssuedInfoByID(request.ID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        public void UpdateOrderNoIsSuedStatus(GetMaryKayOrderIssuedRequest request)
        {
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateOrderNoIsSuedStatus request");
                LogError(ex);
                
            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                accessor.UpdateOrderNoIsSuedStatus(request.OrderNoIssuedStatus,request.ID);
               
            }
            catch (Exception ex)
            {
                LogError(ex);
                
            }
           
        }

        public void AddMaryKayInterfaceLog(GetMaryKayOrderIssuedRequest request)
        {
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddMaryKayInterfaceLog request");
                LogError(ex);

            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                accessor.AddMaryKayInterfaceLog(request.InterfaceLog);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

        }

        public Response<GetMaryKayOrderIssuedRequest> GetHttpYD(GetMaryKayOrderIssuedRequest request)
        {
            Response<GetMaryKayOrderIssuedRequest> response = new Response<GetMaryKayOrderIssuedRequest>() { Result = new GetMaryKayOrderIssuedRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetHttpYD request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                response.Result.YDTable = accessor.GetHttpYD(request.ID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        public Response<GetMaryKayOrderIssuedRequest> GetHttpYZ(GetMaryKayOrderIssuedRequest request)
        {
            Response<GetMaryKayOrderIssuedRequest> response = new Response<GetMaryKayOrderIssuedRequest>() { Result = new GetMaryKayOrderIssuedRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetHttpYZ request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                response.Result.YZTable = accessor.GetHttpYZ(request.ID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        public Response<GetMaryKayTrackInfoRequest> GetMaryKayGetTrack(GetMaryKayTrackInfoRequest request)
        {
            Response<GetMaryKayTrackInfoRequest> response = new Response<GetMaryKayTrackInfoRequest>() { Result = new GetMaryKayTrackInfoRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMaryKayGetTrack request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                int Rowcount;
                response.Result.TrackInfoTable = accessor.GetMaryKayGetTrack(request.SqlWhere, request.PageIndex, request.PageSize, out Rowcount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        public Response<GetMaryKayTrackInfoRequest> GetMaryKayTrackExport(GetMaryKayTrackInfoRequest request)
        {
            Response<GetMaryKayTrackInfoRequest> response = new Response<GetMaryKayTrackInfoRequest>() { Result = new GetMaryKayTrackInfoRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMaryKayTrackExport request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                
                response.Result.TrackInfoTable = accessor.GetMaryKayTrackExport(request.SqlWhere);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        public bool DeleteTrackInfoByID(GetMaryKayTrackInfoRequest request)
        {
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteTrackInfoByID request");
                LogError(ex);

            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                accessor.DeleteTrackInfoByID(request.ID);
                
            }
            catch (Exception ex)
            {
                LogError(ex);
                return false;
            }

            return true;

        }

        /// <summary>
        /// 同步运单号信息
        /// </summary>
        /// <param name="strSql"></param>
        public int SynchroLogisticInfo(string strSql)
        {
            int ReturnValue = 0;
            try
            {
                MaryKayBakAccessor accessor = new MaryKayBakAccessor();
               ReturnValue= accessor.ExcuteSQL(strSql,null,CommandType.Text);

            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return ReturnValue;
        }

        /// <summary>
        /// 查询物流跟踪信息详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetMaryKayTrackInfoRequest> GetGetMaryKayGetTrackDetail(GetMaryKayTrackInfoRequest request)
        {
            Response<GetMaryKayTrackInfoRequest> response = new Response<GetMaryKayTrackInfoRequest>() { Result = new GetMaryKayTrackInfoRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetGetMaryKayGetTrackDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayBakAccessor accessor = new MaryKayBakAccessor();

                response.Result.TrackInfoTable = accessor.GetDataSet(request.SqlWhere,null,CommandType.Text).Tables[0];
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// 更新物流跟踪信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetMaryKayTrackInfoRequest> UpdateMaryKayGetTrackInfo(GetMaryKayTrackInfoRequest request)
        {
            Response<GetMaryKayTrackInfoRequest> response = new Response<GetMaryKayTrackInfoRequest>() { Result = new GetMaryKayTrackInfoRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateMaryKayGetTrackInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayBakAccessor accessor = new MaryKayBakAccessor();
               int i= accessor.ExcuteSQL(request.SqlWhere, null, CommandType.Text);
               response.IsSuccess = i > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// 删除物流跟踪信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool DeleteOrderNo(GetMaryKayTrackInfoRequest request)
        {
            bool value;
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteOrderNo request");
                LogError(ex);
            }
            try
            {
                MaryKayBakAccessor accessor = new MaryKayBakAccessor();

                if (request.ID.IndexOf(",") > -1)
                    value=accessor.DeleteTrackInfoByID(request.ID.Substring(1));  //批量删除
                else
                    value=accessor.DeleteTrackInfoByID(request.ID); //单个删除
            }
            catch (Exception ex)
            {
                LogError(ex);
                value= false;
            }

            return value;
        }

        /// <summary>
        /// 导出玫琳凯物流跟踪信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetMaryKayTrackInfoRequest> MaryKayExportTrackInfo(GetMaryKayTrackInfoRequest request)
        {
            Response<GetMaryKayTrackInfoRequest> response = new Response<GetMaryKayTrackInfoRequest>() { Result = new GetMaryKayTrackInfoRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("MaryKayExportTrackInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayBakAccessor accessor = new MaryKayBakAccessor();
               
                response.Result.TrackInfoTable = accessor.MaryKayExportTrack(request.SqlWhere);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// 获取玫琳凯物流跟踪信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetMaryKayTrackInfoRequest> GetMaryKayGetTrackInfo(GetMaryKayTrackInfoRequest request)
        {
            Response<GetMaryKayTrackInfoRequest> response = new Response<GetMaryKayTrackInfoRequest>() { Result = new GetMaryKayTrackInfoRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMaryKayGetTrackInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayBakAccessor accessor = new MaryKayBakAccessor();
                int Rowcount;
                response.Result.TrackInfoTable = accessor.GetMaryKayGetTrack(request.SqlWhere, request.PageIndex, request.PageSize, out Rowcount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }


        public Response<GetMaryKayTrackInfoRequest> GetMaryKayTrackListInfoByIDS(GetMaryKayTrackInfoRequest request)
        {
            Response<GetMaryKayTrackInfoRequest> response = new Response<GetMaryKayTrackInfoRequest>() { Result = new GetMaryKayTrackInfoRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMaryKayTrackListInfoByIDS request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();

                response.Result.TrackInfoTable = accessor.GetMaryKayTrackListInfoByIDS(request.ID);
               
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        public void UpdateIsNormalByID(GetMaryKayTrackInfoRequest request)
        {
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateIsNormalByID request");
                LogError(ex);

            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                accessor.UpdateIsNormalByID(request.UpLoadMKStatus, request.PodTrackID);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

        }

        public void UpdatePODStatusByCustomerOrderNumber(GetMaryKayTrackInfoRequest request)
        {
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("UpdatePODStatusByCustomerOrderNumber request");
                LogError(ex);

            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                accessor.UpdatePODStatusByCustomerOrderNumber(request.CustomerOrderNumber, request.PODStatusID,request.PODStatusName);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

        }

        public Response<GetMaryKayTrackInfoRequest> GetYundaOrderNoInfo()
        {
            Response<GetMaryKayTrackInfoRequest> response = new Response<GetMaryKayTrackInfoRequest>() { Result = new GetMaryKayTrackInfoRequest() };

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();

                response.Result.TrackInfoTable = accessor.GetYundaOrderNoInfo();

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }

        public void AddYUNDATrackInfo(GetMaryKayTrackInfoRequest request)
        {
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("UpdatePODStatusByCustomerOrderNumber request");
                LogError(ex);

            }

            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                accessor.AddYUNDATrackInfo(request.CustomerOrderNumber,request.Creator,request.CreateTime,request.TrackInfo,request.TrackComment,request.ResponsibilityOwner,request.TrackTime,request.SignName);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }
        }

        /// <summary>
        /// 获取玫琳凯承运商
        /// </summary>
        public Response<IEnumerable<DMS_Shipper>> GetMaryKayShipper()
        {
            MaryKayAccessor accessor = new MaryKayAccessor();
            Response<IEnumerable<DMS_Shipper>> response = new Response<IEnumerable<DMS_Shipper>>();
            try
            {
                response.Result = accessor.GetMaryKayShipper();
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
        /// 获取玫琳凯省份信息
        /// </summary>
        public Response<IEnumerable<DMS_Province>> GetMaryKayProvince()
        {
            MaryKayAccessor accessor = new MaryKayAccessor();
            Response<IEnumerable<DMS_Province>> response = new Response<IEnumerable<DMS_Province>>();
            try
            {
                response.Result = accessor.GetMaryKayProvince();
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
        /// 查询DMS玫琳凯运单
        /// </summary>
        public Response<QueryDMSPODResponse> QueryDMSPOD(QueryDMSPODRequest request)
        {
            Response<QueryDMSPODResponse> response = new Response<QueryDMSPODResponse>() { Result = new QueryDMSPODResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryDMSPOD request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                MaryKayAccessor accessor = new MaryKayAccessor();
                int RowCount;
                response.Result.DMS_PODCollection = accessor.QueryDMSPOD(request, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
