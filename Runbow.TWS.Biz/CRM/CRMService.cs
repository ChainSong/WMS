using System;
using System.Collections.Generic;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Linq;
using System.Data;

namespace Runbow.TWS.Biz
{
    public class CRMService : BaseService
    {
        /// <summary>
        /// 获取crm信息
        /// </summary>
        /// <param name="Sqlwhere"></param>
        /// <returns></returns>
        public Response<GetCRMInfoRequest> GetCRMInfo(GetCRMInfoRequest request)
        {
            Response<GetCRMInfoRequest> response = new Response<GetCRMInfoRequest>() { Result = new GetCRMInfoRequest()};
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMInfoByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                CRMInfoAccessor CRMAccessor = new CRMInfoAccessor();
                int Rowcount;       
                response.Result.IEnumerableCRMInfo = CRMAccessor.GetCRMInfo(request.CRMInfo, request.PageIndex, request.PageSize, out Rowcount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
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
        /// <summary>
        /// 获取crm信息
        /// </summary>
        /// <param name="Sqlwhere"></param>
        /// <returns></returns>
        public DataTable GetCRMInfodao(string  type)
        {
            CRMInfoAccessor CRMAccessor = new CRMInfoAccessor();
            DataTable dt = CRMAccessor.selectCRMInfo(type);
            return dt;
        }
        public static int Import(string sql) {
            try
            {
                 
            
                return CRMInfoAccessor.Import(sql);
            }
            catch (Exception)
            {
                
                throw;
            }
        
        }

    /// <summary>
        /// 添加crm信息和更改crm信息方法
        /// </summary>
        /// <param name="crminfo"></param>
        /// <returns></returns>

        public Response<CRMInfo> OperateCRMInfo(AddOrUpdateCRMInfoRequest request)
        {
            Response<CRMInfo> response = new Response<CRMInfo>();

            if (request == null || request.CRMInfo == null)
            {
                ArgumentNullException ex = new ArgumentNullException("OperateCRMInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                CRMInfoAccessor CRMAccessor = new CRMInfoAccessor();
                response.Result = CRMAccessor.OperateCRMInfo(request.CRMInfo);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                }
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

        /// <summary>
        /// 删除crm信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Response<long> DeleteCRMInfo(DeleteCRMInfoRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteCRMInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                CRMInfoAccessor CRMAccessor = new CRMInfoAccessor();
                response.Result = CRMAccessor.DeleteCRMInfo(request.ID);
                if (response.Result > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                }
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

        /// <summary>
        /// 获取crmtrack信息
        /// </summary>
        /// <param name="Sqlwhere"></param>
        /// <returns></returns>
        public Response<IEnumerable<CRMTrackInfo>> GetCRMTrackInfo(GetCRMTrackInfoRequest request)
        {
            Response<IEnumerable<CRMTrackInfo>> response = new Response<IEnumerable<CRMTrackInfo>>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMTrackInfoByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                CRMTrackInfoAccessor CRMTrackAccessor = new CRMTrackInfoAccessor();
                response.Result = CRMTrackAccessor.GetCRMTrackInfo(request.CRMTrackInfo);
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

        /// <summary>
        /// 添加crmTrack信息和更改crmTrack信息方法
        /// </summary>
        /// <param name="crminfo"></param>
        /// <returns></returns>

        public Response<CRMTrackInfo> OperateCRMTrackInfo(AddOrUpdateCRMTrackInfoRequest request)
        {
            Response<CRMTrackInfo> response = new Response<CRMTrackInfo>();

            if (request == null || request.CRMTrackInfo == null)
            {
                ArgumentNullException ex = new ArgumentNullException("OperateCRMTrackInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                CRMTrackInfoAccessor CRMTrackAccessor = new CRMTrackInfoAccessor();
                response.Result = CRMTrackAccessor.OperateCRMTrackInfo(request.CRMTrackInfo);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                }
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
        /// 删除crmTrack信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Response<long> DeleteCRMTrackInfo(DeleteCRMTrackInfoRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteCRMTrackInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                CRMTrackInfoAccessor CRMTrackAccessor = new CRMTrackInfoAccessor();
                response.Result = CRMTrackAccessor.DeleteCRMTrackInfo(request.ID);
                if (response.Result > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                }
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






        public Response<CRMInfo> GetCRMInfoByID(GetCRMInfoRequest request)
        {
            Response<CRMInfo> response = new Response<CRMInfo>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMInfoByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                CRMInfoAccessor CRMAccessor = new CRMInfoAccessor();
                response.Result = CRMAccessor.GetCRMInfoByID(request.ID);
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



        public Response<CRMTrackInfo> GetCRMTrackInfoByID(GetCRMTrackInfoRequest request) 
        {
            Response<CRMTrackInfo> response = new Response<CRMTrackInfo>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMTrackInfoByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                CRMTrackInfoAccessor CRMTrackAccessor = new CRMTrackInfoAccessor(); 
                response.Result = CRMTrackAccessor.GetCRMTrackInfoByID(request.ID);
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


        public Response<CRMInfo> AddPod(AddOrUpdateCRMInfoRequest request)
        {
            Response<CRMInfo> response = new Response<CRMInfo>();

            if (request == null || request.CRMInfo == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (string.IsNullOrEmpty(request.CRMInfo.CustomerName) || string.IsNullOrEmpty(request.CRMInfo.Age)
                || string.IsNullOrEmpty(request.CRMInfo.City) || string.IsNullOrEmpty(request.CRMInfo.CreateTime) || string.IsNullOrEmpty(request.CRMInfo.Phone) || string.IsNullOrEmpty(request.CRMInfo.ProjectName))
            {
                ArgumentException ex = new ArgumentException("Add CRMInfo, CustomerName or Age or City or CreateTime or Phone or ProjectName can't be null");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                CRMInfoAccessor accessor = new CRMInfoAccessor();
                response.Result = accessor.AddCrm(request.CRMInfo);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<IEnumerable<CRMInfo>> AddPods(AddOrUpdateCRMInfoRequest request)
        {
            throw new NotImplementedException();
            //Response<IEnumerable<CRMInfo>> response = new Response<IEnumerable<CRMInfo>>();

            //if (request == null || request.CRMInfos == null || !request.CRMInfos.Any())
            //{
            //    ArgumentNullException ex = new ArgumentNullException("AddCrms request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}

            //try
            //{
            //    CRMInfoAccessor accessor = new CRMInfoAccessor();
            //    response.Result = accessor.AddCrms(request.CRMInfos);
            //    if (response.Result.Count() == 0)
            //    {
            //        response.IsSuccess = false;
            //        response.ErrorCode = ErrorCode.Technical;
            //    }
            //    else
            //    {
            //        response.IsSuccess = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogError(ex);
            //    response.IsSuccess = false;
            //    response.ErrorCode = ErrorCode.Technical;
            //}

            //return response;
        }


       
    }
}
