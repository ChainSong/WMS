using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.ShipperManagement;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement;
using Runbow.TWS.Entity.ShipperManagement;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.ShipperManagement;

namespace Runbow.TWS.Biz.ShipperManagement
{
    public class VehicleManagementService : BaseService
    {
        /// <summary>
        /// 查询
        /// </summary> 
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetCRMVehicleByConditionResponse> GetCRMVehicleByCondition(GetCRMVehicleByConditionRequest request)
        {
            Response<GetCRMVehicleByConditionResponse> response = new Response<GetCRMVehicleByConditionResponse>() { Result = new GetCRMVehicleByConditionResponse() };
            
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMVehicleByCondition request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            
            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMVehicleCollection = accessor.GetCRMVehicleByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                    if (response.Result.PageCount <= response.Result.PageIndex)
                    {
                        response.Result.PageIndex = 0;
                }
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMVehicleCollection = accessor.GetCRMVehicleByConditionNoPaging(request.SearchCondition);
                }
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
        /// 手机端查询 通过关键字
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetCRMVehicleByConditionResponse> GetCRMVehicleBykeyword(GetCRMVehicleByConditionRequest request)
        {
            Response<GetCRMVehicleByConditionResponse> response = new Response<GetCRMVehicleByConditionResponse>() { Result = new GetCRMVehicleByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMVehicleByCondition request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMVehicleCollection = accessor.GetCRMVehicleBykeyword(request.keyword, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMVehicleCollection = accessor.GetCRMVehicleByConditionNoPaging(request.SearchCondition);
                }
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

      

        public Response<GetCRMVehicleByConditionResponse> GetShipperMappingVehicleInfoByShipperIDandkeyWord(string id, GetCRMVehicleByConditionRequest request)
        {
            Response<GetCRMVehicleByConditionResponse> response = new Response<GetCRMVehicleByConditionResponse>() { Result = new GetCRMVehicleByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMVehicleByCondition request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMVehicleCollection = accessor.GetShipperMappingVehicleInfoByShipperIDandkeyWord(id,request.keyword, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMVehicleCollection = accessor.GetCRMVehicleByConditionNoPaging(request.SearchCondition);
                }
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
        /// 通过承运商ID查询该承运商下面有哪些车辆
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetCRMVehicleByConditionResponse> GetShipperMappingVehicleBySID(GetCRMVehicleByConditionRequest request)
        {
            Response<GetCRMVehicleByConditionResponse> response = new Response<GetCRMVehicleByConditionResponse>() { Result = new GetCRMVehicleByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMVehicleByCondition request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMVehicleCollection = accessor.GetShipperMappingVehicleBySID(request.keyword, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMVehicleCollection = accessor.GetCRMVehicleByConditionNoPaging(request.SearchCondition);
                }
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


        public Response<IEnumerable<long>> AddOrUpdateCRMVehicle(AddOrUpdateCRMVehicleRequest request)
        {
            Response<IEnumerable<long>> response = new Response<IEnumerable<long>>();

            if (request == null || request.CRMVehicleCollection == null || !request.CRMVehicleCollection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdateCRMVehicle request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
             //  response.Result = accessor.AddOrUpdateCRMVehicle(request.CRMVehicleCollection);
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


        //新增和更新
        public CRMVehicle addCreateVehicle(GetCRMVehicleByConditionRequest request)
        {

            CRMVehicle CreateFiles = new CRMVehicle();
            try
            {
                CRMVehicle Vehicle = new CRMVehicle();
                IList<CRMVehicle> CRMVehicle = new List<CRMVehicle>();
                CRMVehicle.Add(request.CreateFiles);
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                CreateFiles = accessor.AddOrUpdateCRMVehicle(CRMVehicle);// request.PageIndex, request.PageSize, out RowCount
            }
            catch (Exception)
            {

                throw;
            }
            return CreateFiles;

        }


        //通过id查询
        public CRMVehicle GetSearchVehicle(string id)
        {
            CRMVehicle SearcheFiles = new CRMVehicle();
            try
            {
               
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                SearcheFiles = accessor.GetCRMVehicleSearchConditionID(id);
            }
            catch (Exception)
            {
                throw;
            }
            return SearcheFiles;
        }

        public CRMVehicle GetCRMVehiclebyID(string id)
        {
            CRMVehicle SearcheFiles = new CRMVehicle();
            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                SearcheFiles = accessor.GetCRMVehiclebyID(id);
            }
            catch (Exception)
            {
                throw;
            }
            return SearcheFiles;
        }

        /// <summary>
        /// 删除crm信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //public Response<long> DeleteCRMVehicle(DeleteCRMVehicleRequest request)
        //{
        //    Response<bool> response = new Response<bool> { Result = false };
        //    if (request == null)
        //    {
        //        ArgumentNullException ex = new ArgumentNullException("DeleteCRMVehicle request CRMVehicleID");
        //        LogError(ex);
        //        response.ErrorCode = ErrorCode.Argument;
        //        response.Exception = ex;
        //        return response;
        //    }



        //删除
        public bool DeleteCRMVehicle(string id)
        {
            bool ve = true;
            try
            {
                ve = new VehicleManagementAccessor().DeleteCRMVehicle(id);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return ve;
        }

        /// <summary>
        /// 获取车辆列表
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<CRMVehicle>> GetVehicleList()
        {
            Response<IEnumerable<CRMVehicle>> response = new Response<IEnumerable<CRMVehicle>>();

            try
            {
                response.Result = new VehicleManagementAccessor().GetVehicleList();
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
        /// 车辆分页显示
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetCRMVehicleByConditionResponse> GetAllVehicle(GetCRMVehicleByConditionRequest request)
        {
            Response<GetCRMVehicleByConditionResponse> response = new Response<GetCRMVehicleByConditionResponse>() { Result = new GetCRMVehicleByConditionResponse() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetAllVehicle request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                int RowCount;
              
                    response.Result.CRMVehicleCollection = accessor.GetAllVehicle(request.Vehicle, request.vehicleNo, request.PageIndex, request.PageSize, out RowCount);
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


        /// <summary>
        /// 已选车辆信息
        /// </summary>
        /// <param name="crmCar"></param>
        /// <returns></returns>
        public string AddShipperToVehicle(ShipperMappingVehicleRequest request)
        {
            string IsSuccess = "";
            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                IsSuccess = accessor.AddShipperToVehicle(request.car, request.ShipperName,request.UserName);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return IsSuccess;
        }



        //根据承运商名称查询已选择车辆
        public Response<GetCRMVehicleByConditionResponse> GetCRM_ShipperMappingVehicle(ShipperMappingVehicleRequest request)
        {
            Response<GetCRMVehicleByConditionResponse> response = new Response<GetCRMVehicleByConditionResponse>() { Result = new GetCRMVehicleByConditionResponse() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetAllVehicle request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                

                    response.Result.CRMVehicleCollection = accessor.GetCRM_ShipperMappingVehicle(request.ShipperName); //request.PageIndex, request.PageSize, out RowCount
                  //  response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    //response.Result.PageIndex = request.PageIndex;
                
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

        //右边表格查询
        public Response<GetCRMVehicleByConditionResponse> GetCRMShipperMappingVehicle(ShipperMappingVehicleRequest request)
        {
            Response<GetCRMVehicleByConditionResponse> response = new Response<GetCRMVehicleByConditionResponse>() { Result = new GetCRMVehicleByConditionResponse() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMShipperMappingVehicle request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                VehicleManagementAccessor accessor = new VehicleManagementAccessor();
                response.Result.CRMVehicleCollection = accessor.GetCRMShipperMappingVehicle(request.ShipperName, request.VehicleNo);  

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

