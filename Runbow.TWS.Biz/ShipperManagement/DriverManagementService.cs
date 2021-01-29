using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.ShipperManagement;
using Runbow.TWS.Entity.ShipperManagement;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.ShipperManagement.DriverManagement;
using Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement;

namespace Runbow.TWS.Biz.ShipperManagement
{
    public class DriverManagementService : BaseService
    {
        /// <summary>
        /// 查询
        /// </summary> 
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetCRMDriverByConditionResponse> GetCRMDriverByCondition(GetCRMDriverByConditionRequest request)
        {
            Response<GetCRMDriverByConditionResponse> response = new Response<GetCRMDriverByConditionResponse>() { Result = new GetCRMDriverByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMDriverByCondition request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMDriverCollection = accessor.GetCRMDriverByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMDriverCollection = accessor.GetCRMDriverByConditionNoPaging(request.SearchCondition);
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
        /// 通过车辆id查询该车由哪几位司机驾驶
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetCRMDriverByConditionResponse> GetVehicleMappingDriverVID(GetCRMDriverByConditionRequest request)
        {
            Response<GetCRMDriverByConditionResponse> response = new Response<GetCRMDriverByConditionResponse>() { Result = new GetCRMDriverByConditionResponse() };

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
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMDriverCollection = accessor.GetVehicleMappingDriverVID(request.keyword, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMDriverCollection = accessor.GetCRMDriverByConditionNoPaging(request.SearchCondition);
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
        /// 通过车辆id查询该车由哪几位司机驾驶待条件分页
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetCRMDriverByConditionResponse> GetVehicleMappingDriverInfoByVIDandkeyWord(string id, GetCRMDriverByConditionRequest request)
        {
            Response<GetCRMDriverByConditionResponse> response = new Response<GetCRMDriverByConditionResponse>() { Result = new GetCRMDriverByConditionResponse() };

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
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMDriverCollection = accessor.GetVehicleMappingDriverInfoByVIDandkeyWord(id, request.keyword, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMDriverCollection = accessor.GetCRMDriverByConditionNoPaging(request.SearchCondition);
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
        /// 通过shipperID查询当前承运商有哪些司机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetCRMDriverByConditionResponse> GetShippingMappingDriverSID(GetCRMDriverByConditionRequest request)
        {
            Response<GetCRMDriverByConditionResponse> response = new Response<GetCRMDriverByConditionResponse>() { Result = new GetCRMDriverByConditionResponse() };

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
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMDriverCollection = accessor.GetShippingMappingDriverSID(request.keyword, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMDriverCollection = accessor.GetCRMDriverByConditionNoPaging(request.SearchCondition);
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
        ///通过shipperID查询当前承运商有哪些司机服务  条件分页
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetCRMDriverByConditionResponse> GetShipperMappingDriverInfoBySIDandkeyWord(string id, GetCRMDriverByConditionRequest request)
        {
            Response<GetCRMDriverByConditionResponse> response = new Response<GetCRMDriverByConditionResponse>() { Result = new GetCRMDriverByConditionResponse() };

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
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMDriverCollection = accessor.GetShipperMappingDriverInfoBySIDandkeyWord(id, request.keyword, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMDriverCollection = accessor.GetCRMDriverByConditionNoPaging(request.SearchCondition);
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








        public Response<GetCRMDriverByConditionResponse> GetCRMDriverBykeyword(GetCRMDriverByConditionRequest request)
        {
            Response<GetCRMDriverByConditionResponse> response = new Response<GetCRMDriverByConditionResponse>() { Result = new GetCRMDriverByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMDriverByCondition request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.CRMDriverCollection = accessor.GetCRMDriverBykeyWord(request.keyword, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.CRMDriverCollection = accessor.GetCRMDriverByConditionNoPaging(request.SearchCondition);
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

        public Response<IEnumerable<long>> AddOrUpdateCRMDriver(AddOrUpdateCRMDriverRequest request)
        {
            Response<IEnumerable<long>> response = new Response<IEnumerable<long>>();

            if (request == null || request.CRMDriverCollection == null || !request.CRMDriverCollection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdateCRMDriver request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                //response.Result = accessor.AddOrUpdateCRMDriver(request.CRMDriverCollection);
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






        public CRMDriver addCreateDriver(GetCRMDriverByConditionRequest request)
        {

            CRMDriver CreateDriver = new CRMDriver();
            try
            {
                CRMDriver Driver = new CRMDriver();
                IList<CRMDriver> CRMDriver = new List<CRMDriver>();
                CRMDriver.Add(request.AddDriver);
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                CreateDriver = accessor.AddOrUpdateCRMDriver(CRMDriver);// request.PageIndex, request.PageSize, out RowCount
            }
            catch (Exception)
            {

                throw;
            }
            return CreateDriver;

        }

        public IEnumerable<VehicleToDriver> GetCarCacheInfo(string Sql)
        {

            IEnumerable<VehicleToDriver> CreateCar = null;
            try
            {  
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                CreateCar = accessor.GetCarCacheInfo(Sql);// request.PageIndex, request.PageSize, out RowCount
            }
            catch (Exception)
            { 
                return CreateCar;
            }
            return CreateCar;

        }

        //删除
        public bool DeleteCRMDriver(string id)
        {
            bool dr = true;
            try
            {
                dr = new DriverManagementAccessor().DeleteCRMDriver(id);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return dr;
        }



        public CRMDriver GetSearcheDriver(string id)
        {
            CRMDriver SearcheFiles = new CRMDriver();
            try
            {

                DriverManagementAccessor accessor = new DriverManagementAccessor();
                SearcheFiles = accessor.GetCRMDriverSearchConditionByID(id);
            }
            catch (Exception)
            {
                throw;
            }
            return SearcheFiles;
        }

        public Response<IEnumerable<CRMDriver>> GetDriverList()
        {
            Response<IEnumerable<CRMDriver>> response = new Response<IEnumerable<CRMDriver>>();

            try
            {
                response.Result = new DriverManagementAccessor().GetDriverList();
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


        //车辆司机管理
        public Response<GetCRMDriverByConditionResponse> GetAllDriver(GetCRMDriverByConditionRequest request)
        {
            Response<GetCRMDriverByConditionResponse> response = new Response<GetCRMDriverByConditionResponse>() { Result = new GetCRMDriverByConditionResponse() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetVehicleToDriver request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                int RowCount;
                //if (request.PageSize > 0)
                //{
                    response.Result.CRMDriverCollection = accessor.GetAllDriver(request.Driver,request.driverName, request.PageIndex, request.PageSize, out RowCount); //request.PageIndex, request.PageSize, out RowCount
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                //}
                //else
                //{
                //    response.Result.PageIndex = 0;
                //    response.Result.PageCount = 0;
                //    response.Result.CRMDriverCollection = accessor.GetCRMDriverByConditionNoPaging(request.SearchCondition);
                //}
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

        public string AddVehicleToDriver(VehicleMappingDriverRequest request)
        {
            string IsSuccess = "";
            try
            {
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                IsSuccess = accessor.AddVehicleToDriver(request.driver, request.VehicleNo, request.UserName);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return IsSuccess;
        }


        //根据车牌号码查询已选择司机
        public Response<GetCRMDriverByConditionResponse> GetCRM_VehicleMappingDriver(VehicleMappingDriverRequest request)
        {
            Response<GetCRMDriverByConditionResponse> response = new Response<GetCRMDriverByConditionResponse>() { Result = new GetCRMDriverByConditionResponse() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRM_VehicleMappingDriver request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                DriverManagementAccessor accessor = new DriverManagementAccessor();


                response.Result.CRMDriverCollection = accessor.GetCRM_VehicleMappingDriver(request.VehicleNo); //request.PageIndex, request.PageSize, out RowCount
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
        public Response<GetCRMDriverByConditionResponse> GetCRMVehicleMappingDriver(VehicleMappingDriverRequest request)
        {
            Response<GetCRMDriverByConditionResponse> response = new Response<GetCRMDriverByConditionResponse>() { Result = new GetCRMDriverByConditionResponse() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCRMVehicleMappingDriver request ");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                DriverManagementAccessor accessor = new DriverManagementAccessor();
                response.Result.CRMDriverCollection = accessor.GetCRMVehicleMappingDriver(request.VehicleNo, request.DriverName);

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
