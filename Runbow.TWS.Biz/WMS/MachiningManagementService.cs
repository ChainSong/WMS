using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Data;
using Runbow.TWS.MessageContracts.WMS.Machining;

namespace Runbow.TWS.Biz.WMS
{
    public class MachiningManagementService : BaseService
    {

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetMachiningByConditionResponse> GetMachiningByCondition(GetMachiningByConditionRequest request)
        {

            Response<GetMachiningByConditionResponse> response = new Response<GetMachiningByConditionResponse>() { Result = new GetMachiningByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMachiningByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.MachiningCollection = accessor.GetMachiningByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
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


        public Response<GetMachiningByConditionResponse> GetInventoryBySearchCondition(GetMachiningByConditionRequest request)
        {
            Response<GetMachiningByConditionResponse> response = new Response<GetMachiningByConditionResponse>() { Result = new GetMachiningByConditionResponse() };

            if (request == null && request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                int RowCount;
                response.Result.InventoryCollection = accessor.GetInventoryBySearchCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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

        public Response<GetMachiningByConditionResponse> GetLittleInventoryBySearchCondition(GetMachiningByConditionRequest request)
        {
            Response<GetMachiningByConditionResponse> response = new Response<GetMachiningByConditionResponse>() { Result = new GetMachiningByConditionResponse() };

            if (request == null && request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                int RowCount;
                response.Result.InventoryCollection = accessor.GetLittleInventoryBySearchCondition(request.SearchCondition);
                //response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                //response.Result.PageIndex = request.PageIndex;
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

        public string SaveMachining(GetMachiningByConditionRequest request, string Creator)
        {
            string str = "";
            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                str = accessor.SaveMachining(request.MachiningCollection, Creator);             
            }
            catch (Exception ex)
            {
                LogError(ex);
              
            }

            return str;
        }
        public string AddMachining(GetMachiningByConditionRequest request, string Creator, string IDS, string IDDS)
        {
            string str = "";
            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                str = accessor.AddMachining(request.MachiningCollection, Creator, IDS,IDDS);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return str;
        }

        public string BucketOutMachining(GetMachiningByConditionRequest request, string Creator, string IDS)
        {
            string str = "";
            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                str = accessor.BucketOutMachining(request.MachiningCollection, Creator, IDS);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return str;
        }

        public string BucketInMachining(GetMachiningByConditionRequest request, string Creator, string IDS)
        {
            string str = "";
            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                str = accessor.BucketInMachining(request.MachiningCollection, Creator, IDS);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return str;
        }

        public Response<GetMachiningByConditionResponse> GetMachiningByID(long ID)
        {

            Response<GetMachiningByConditionResponse> response = new Response<GetMachiningByConditionResponse>() { Result = new GetMachiningByConditionResponse() };

            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                response.Result.MachiningCollection = accessor.GetMachiningByID(ID);
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
        public string MachiningDelete(long ID)
        {
            string s = ""; 
            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                s = accessor.MachiningDelete(ID);
            }
            catch (Exception ex)
            {
                LogError(ex);
               
            }

            return s;
        }
        public int CheckMachiningNumber(string MachiningNumber)
        {
            int i=0;
            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                i = accessor.CheckMachiningNumber(MachiningNumber);
               
            }
            catch (Exception ex)
            {
                LogError(ex);
               
            }

            return i;
        }
        public Response<GetMachiningByConditionResponse> GetPrintMachining(string id)
        {
            Response<GetMachiningByConditionResponse> response = new Response<GetMachiningByConditionResponse>() { Result = new GetMachiningByConditionResponse() };

            try
            {
                MachiningManagementAccessor accessor = new MachiningManagementAccessor();
                response.Result.MachiningCollection  = accessor.GetPrintMachining(id);
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
