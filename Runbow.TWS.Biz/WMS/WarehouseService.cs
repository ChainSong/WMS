using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Data;
using Runbow.TWS.MessageContracts.WMS.Warehouse;
using Runbow.TWS.Entity.WMS.Warehouse;

namespace Runbow.TWS.Biz
{
    public class WarehouseService : BaseService
    {
        public Response<GetWarehouseByConditionResponse> GetWarehouseByCondition(GetWarehouseByConditonRequest request)
        {
            Response<GetWarehouseByConditionResponse> response = new Response<GetWarehouseByConditionResponse>() { Result = new GetWarehouseByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWarehouseByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.WarehouseCollection = accessor.GetWarehouseByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.WarehouseCollection = accessor.GetWarehouseByConditionNoPaging(request.SearchCondition);
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
        public string SaveQRCode(GetQRCodeByConditonRequest request, long? ProjectID, long? WareHouseID, long? Length, long? Width, string Creator, int Flag)
        {

            string responses="";
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SaveQRCode request");
                LogError(ex);
              
                return "";
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();

                responses = accessor.SaveQRCode(request, ProjectID, WareHouseID,  Length,  Width,  Creator,Flag);
             
            }
            catch (Exception ex)
            {
                LogError(ex);
              
              
            }

            return responses;
        }
        public string EditQRCode(GetQRCodeByConditonRequest request, long? ProjectID, long? WareHouseID, int Flag)
        {

            string responses = "";
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("EditQRCode request");
                LogError(ex);

                return "";
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();

                responses = accessor.EditQRCode(request, ProjectID, WareHouseID, Flag);

            }
            catch (Exception ex)
            {
                LogError(ex);


            }

            return responses;
        }
        public string ImportGoodsShelf(GetGoodsShelfByConditonRequest request, int ViewType)
        {

            string responses = "";
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SaveGoodsShelf request");
                LogError(ex);

                return "";
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();

                responses = accessor.ImportGoodsShelf(request, ViewType);

            }
            catch (Exception ex)
            {
                LogError(ex);


            }

            return responses;
        }
        public string ImportGoodsShelfLocation(GetGoodsShelfByConditonRequest request, int ViewType)
        {

            string responses = "";
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SaveGoodsShelfLocation request");
                LogError(ex);

                return "";
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();

                responses = accessor.ImportGoodsShelfLocation(request, ViewType);

            }
            catch (Exception ex)
            {
                LogError(ex);


            }

            return responses;
        }
        public string SaveGoodsShelf(GetGoodsShelfByConditonRequest request, int ViewType)
        {

            string responses = "";
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SaveGoodsShelf request");
                LogError(ex);

                return "";
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();

                responses = accessor.SaveGoodsShelf(request,ViewType);

            }
            catch (Exception ex)
            {
                LogError(ex);


            }

            return responses;
        }
        public string SaveGoodsShelfWithLocation(GetGoodsShelfByConditonRequest request, int ViewType)
        {

            string responses = "";
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SaveGoodsShelfWithLocation request");
                LogError(ex);

                return "";
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();

                responses = accessor.SaveGoodsShelfWithLocation(request, ViewType);

            }
            catch (Exception ex)
            {
                LogError(ex);


            }

            return responses;
        }
        public string DeleteGoodsShelf(long ID)
        {

            string responses = "";
         
            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();

                responses = accessor.DeleteGoodsShelf(ID);

            }
            catch (Exception ex)
            {
                LogError(ex);


            }

            return responses;
        }
        public Response<GetQRCodeByConditionResponse> GetQRCodeByCondition(GetQRCodeByConditonRequest request)
        {
            Response<GetQRCodeByConditionResponse> response = new Response<GetQRCodeByConditionResponse>() { Result = new GetQRCodeByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetQRCodeByConditonRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
               
                response.Result= accessor.GetQRCodeByCondition(request.SearchCondition);
               

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
        public Response<GetGoodsShelfByConditionResponse> GetGoodsShelfByCondition(GetGoodsShelfByConditonRequest request)
        {
            Response<GetGoodsShelfByConditionResponse> response = new Response<GetGoodsShelfByConditionResponse>() { Result = new GetGoodsShelfByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetGoodsShelfByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                int RowCount;
                    response.Result.GoodsShelfCollection = accessor.GetGoodsShelfByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        public Response<GetProductWarningByConditionResponse> GetProductWarningByCondition(GetProductWarningByConditonRequest request)
        {
            Response<GetProductWarningByConditionResponse> response = new Response<GetProductWarningByConditionResponse>() { Result = new GetProductWarningByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetProductWarningByConditionResponse request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                int RowCount;
                response.Result.ProductWarningCollection = accessor.GetProductWarningByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        public Response<GetProductWarningByConditionResponse> GetNoWarningSKUByCondition(GetProductWarningByConditonRequest request)
        {
            Response<GetProductWarningByConditionResponse> response = new Response<GetProductWarningByConditionResponse>() { Result = new GetProductWarningByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetNoWarningSKUByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                int RowCount;
                response.Result.ProductCollection = accessor.GetNoWarningSKUByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        public Response<GetGoodsShelfByConditionResponse> GetGoodsShelfByID(long? ID)
        {
            Response<GetGoodsShelfByConditionResponse> response = new Response<GetGoodsShelfByConditionResponse>() { Result = new GetGoodsShelfByConditionResponse() };

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                response.Result = accessor.GetGoodsShelfByID(ID);
                if (response.Result != null)
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
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<ProductWarningSearchCondition> GetProductWarningByID(long? ID)
        {
            Response<ProductWarningSearchCondition> response = new Response<ProductWarningSearchCondition>() { Result = new ProductWarningSearchCondition() };

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                response.Result = accessor.GetProductWarningByID(ID);
                if (response.Result != null)
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
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public string ProductWarningAdd(string IDS, string WarehouseID, string CustomerID, string WarehouseName, string MinNumber, string MaxNumber)
        {
            string message = "";
            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                message = accessor.ProductWarningAdd( IDS,  WarehouseID,  CustomerID,  WarehouseName,  MinNumber,  MaxNumber);
               
            }
            catch (Exception ex)
            {
                LogError(ex);
               
            }

            return message;
        }
        public string ProductWarningDelete(string IDS)
        {
            string message = "";
            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                message = accessor.ProductWarningDelete(IDS);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return message;
        }
        public string ProductWarningEdit(string ID, string MinNumber, string MaxNumber)
        {
            string message = "";
            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                message = accessor.ProductWarningEdit(ID, MinNumber, MaxNumber);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return message;
        }
        public Response<GetWLocationByConditionResponse> GetWLocationByCondition(GetWLocationByConditonRequest request)
        {
            Response<GetWLocationByConditionResponse> response = new Response<GetWLocationByConditionResponse>() { Result = new GetWLocationByConditionResponse() };

            if (request == null || request.WLSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWLocationByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.WLocationCollection = accessor.GetWLocationByCondition(request.WLSearchCondition, request.PageIndex, request.PageSize, out RowCount);
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.WLocationCollection = accessor.GetWLocationByConditionNoPaging(request.WLSearchCondition);
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

        /// <summary>
        /// 根据ID获取仓库信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseByIDResponse> GetWarehouseByID(GetWarehouseByIDRequest request)
        {
            Response<GetWarehouseByIDResponse> response = new Response<GetWarehouseByIDResponse>();
            response.Result = new GetWarehouseByIDResponse();
            long? ID = request.ID;
            WarehouseInfo warehouse = new WarehouseInfo();
            IEnumerable<AreaInfo> areas = new List<AreaInfo>();
            IEnumerable<LocationInfo> locations = new List<LocationInfo>();
            response.SuccessMessage = new WarehouseAccessor().GetWarehouseByID(ID, out warehouse, out areas, out locations);
            response.Result.Warehouse = warehouse;
            response.Result.Areas = areas;
            response.Result.Locations = locations;
            return response;
        }

        /// <summary>
        /// 根据ID获取仓库信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseByIDResponse> GetWarehouseAndAreaByID(GetWarehouseByIDRequest request)
        {
            Response<GetWarehouseByIDResponse> response = new Response<GetWarehouseByIDResponse>();
            response.Result = new GetWarehouseByIDResponse();
            long? ID = request.ID;
            WarehouseInfo warehouse = new WarehouseInfo();
            IEnumerable<AreaInfo> areas = new List<AreaInfo>();
            response.SuccessMessage = new WarehouseAccessor().GetWarehouseAndAreaByID(ID, out warehouse, out areas);
            response.Result.Warehouse = warehouse;
            response.Result.Areas = areas;
            return response;
        }

        /// <summary>
        /// 提交仓库信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseByIDResponse> AddOrUpdateWarehouse(AddOrUpdateWarehouseRequest request)
        {
            Response<GetWarehouseByIDResponse> response = new Response<GetWarehouseByIDResponse>();
            response.Result = new GetWarehouseByIDResponse();
            WarehouseInfo warehouse = new WarehouseInfo();
            IEnumerable<AreaInfo> areas = new List<AreaInfo>();
            IEnumerable<LocationInfo> locations = new List<LocationInfo>();

            ///定义表数据，作为参数传给存储过程
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("WarehouseName");
            dt.Columns.Add("Status");
            dt.Columns.Add("Type");
            dt.Columns.Add("Description");
            dt.Columns.Add("Company");
            dt.Columns.Add("CreditLine");
            dt.Columns.Add("Address");
            dt.Columns.Add("ProvinceCity");
            dt.Columns.Add("ZipCode");
            dt.Columns.Add("Contractor");
            dt.Columns.Add("ContractorAddress");
            dt.Columns.Add("Mobile");
            dt.Columns.Add("Phone");
            dt.Columns.Add("Fax");
            dt.Columns.Add("Email");
            dt.Columns.Add("Remark");
            dt.Columns.Add("Creator");
            dt.Columns.Add("CreateTime");
            dt.Columns.Add("ProjectID");
            dt.Rows.Add(request.warehouse.ID, request.warehouse.WarehouseName, request.warehouse.WarehouseStatus, request.warehouse.WarehouseType, request.warehouse.Description,
                request.warehouse.Company, request.warehouse.CreditLine, request.warehouse.Address, request.warehouse.ProvinceCity, request.warehouse.ZipCode,
                request.warehouse.Contractor, request.warehouse.ContractorAddress, request.warehouse.Mobile, request.warehouse.Phone, request.warehouse.Fax,
                request.warehouse.Email, request.warehouse.Remark, request.warehouse.Creator, DateTime.Now, request.warehouse.ProjectID);


            response.SuccessMessage = new WarehouseAccessor().AddOrUpdateWarehouse(dt, out warehouse, out areas, out locations);
            response.Result.Warehouse = warehouse;
            response.Result.Areas = areas;
            response.Result.Locations = locations;
            return response;
        }

        /// <summary>
        /// 获取仓库下拉列表数据
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<WarehouseInfo>> GetWarehouseList()
        {
            Response<IEnumerable<WarehouseInfo>> response = new Response<IEnumerable<WarehouseInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetWarehouseList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
        

        /// <summary>
        /// 根据客户ID获取仓库信息
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<WarehouseInfo>> GetWarehouseListByCustomerID(long CustomerID)
        {
            Response<IEnumerable<WarehouseInfo>> response = new Response<IEnumerable<WarehouseInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetWarehouseListByCustomerID(CustomerID);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        /// <summary>
        /// 根据仓库ID获取库区信息
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<AreaInfo>> GetWarehouseAreaList(long WarehouseID)
        {
            Response<IEnumerable<AreaInfo>> response = new Response<IEnumerable<AreaInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetWarehouseAreaList(WarehouseID);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
        /// <summary>
        /// 根据仓库名称获取库区信息
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<AreaInfo>> GetWarehouseAreaListByWarehouseName(string WarehouseName)
        {
            Response<IEnumerable<AreaInfo>> response = new Response<IEnumerable<AreaInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetWarehouseAreaListByWarehouseName(WarehouseName);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
        public Response<IEnumerable<GoodsShelfInfo>> GetGoodsShelfList(long project, long CustomerID, long WarehouseID)
        {
            Response<IEnumerable<GoodsShelfInfo>> response = new Response<IEnumerable<GoodsShelfInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetGoodsShelfList(project, CustomerID, WarehouseID);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
        /// <summary>
        /// 根据仓库ID获取库位信息
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<LocationInfo>> GetWarehouseLocationList(long WarehouseID)
        {
            Response<IEnumerable<LocationInfo>> response = new Response<IEnumerable<LocationInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetWarehouseLocationList(WarehouseID);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
        /// <summary>
        /// 根据仓库ID获取库位信息
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<LocationInfo>> GetWarehouseLocationListByWCID(long WarehouseID,long CustomerID)
        {
            Response<IEnumerable<LocationInfo>> response = new Response<IEnumerable<LocationInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetWarehouseLocationListByWCID(WarehouseID, CustomerID);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
        /// <summary>
        /// 根据仓库ID和库位名称获取库位信息
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<LocationInfo>> GetWarehouseLocationListByLocation(string WarehouseID,string Location)
        {
            Response<IEnumerable<LocationInfo>> response = new Response<IEnumerable<LocationInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetWarehouseLocationListByLocation(WarehouseID,Location);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }



        /// <summary>
        /// 根据仓库ID获取库位信息
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<LocationInfo>> GetWarehouseLocationListByWarehouseName(string WarehouseName)
        {
            Response<IEnumerable<LocationInfo>> response = new Response<IEnumerable<LocationInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetWarehouseLocationListByWarehouseName(WarehouseName);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public Response<IEnumerable<LocationInfo>> GetLocationGoodShelfList(long WarehouseID)
        {
            Response<IEnumerable<LocationInfo>> response = new Response<IEnumerable<LocationInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().GetLocationGoodShelfList(WarehouseID);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
        /// <summary>
        /// 更新库区信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseAreaByIDResponse> AddOrUpdateWarehouseArea(AddOrUpdateWarehouseAreaRequest request)
        {
            Response<GetWarehouseAreaByIDResponse> response = new Response<GetWarehouseAreaByIDResponse>();
            response.Result = new GetWarehouseAreaByIDResponse();
            WarehouseInfo warehouse = new WarehouseInfo();
            AreaInfo area = new AreaInfo();
            IEnumerable<LocationInfo> locations = new List<LocationInfo>();

            ///定义表数据，作为参数传给存储过程
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("WarehouseID");
            dt.Columns.Add("AreaName");
            dt.Columns.Add("Status");
            dt.Columns.Add("Type");
            dt.Columns.Add("Remark");
            dt.Columns.Add("Creator");
            dt.Columns.Add("CreateTime");
            dt.Columns.Add("Str1");

            dt.Rows.Add(request.Area.ID, request.Area.WarehouseID, request.Area.AreaName, request.Area.Status,
                request.Area.Type, request.Area.Remark, request.Area.Creator, DateTime.Now,request.Area.Str1);

            response.SuccessMessage = new WarehouseAccessor().AddOrUpdateWarehouseArea(dt, out warehouse, out area, out locations);

            if (response.SuccessMessage == "操作成功")
            {
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }

            response.Result.Warehouse = warehouse;
            response.Result.Area = area;
            response.Result.Locations = locations;
            return response;
        }


        /// <summary>
        /// 根据ID获取库区，以及库区下的所有库位
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Response<GetWarehouseAreaByIDResponse> GetWarehouseAreaByID(GetWarehouseAreaByIDRequest request)
        {
            Response<GetWarehouseAreaByIDResponse> response = new Response<GetWarehouseAreaByIDResponse>();
            response.Result = new GetWarehouseAreaByIDResponse();
            long ID = request.ID;

            WarehouseInfo warehouse = new WarehouseInfo();
            AreaInfo area = new AreaInfo();
            IEnumerable<LocationInfo> locations = new List<LocationInfo>();
            response.SuccessMessage = new WarehouseAccessor().GetWarehouseAreaByID(ID, out warehouse, out area, out locations);
            response.Result.Warehouse = warehouse;
            response.Result.Area = area;
            response.Result.Locations = locations;
            return response;
        }


        public IEnumerable<WarehouseInfo> GetWarehouse()
        {

            WarehouseAccessor accessor = new WarehouseAccessor();

            return accessor.GetWarehouse();

        }


        /// <summary>
        /// 更新库位信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseLocationByIDResponse> AddOrUpdateWarehouseLocation(AddOrUpdateWarehouseLocationRequest request)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            response.Result = new GetWarehouseLocationByIDResponse();        //结果集初始化
            WarehouseInfo warehouse = new WarehouseInfo();                  //仓库
            AreaInfo area = new AreaInfo();                                 //库区
            LocationInfo location = new LocationInfo();                      //库位
            GoodsShelfInfo goodsShelf = new GoodsShelfInfo();
            IList<LocationInfo> locations = new List<LocationInfo>();        //输入参数
            locations.Add(request.Location);
            response.SuccessMessage = new WarehouseAccessor().AddOrUpdateWarehouseLocation(locations, out warehouse, out area, out location,out goodsShelf);  //获取执行结果

            if (response.SuccessMessage == "操作成功")
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            response.Result.Warehouse = warehouse;
            response.Result.Area = area;
            response.Result.Location = location;
            response.Result.GoodsShelf = goodsShelf;
            return response;
        }

        public Response<GetWarehouseLocationByIDResponse> AddLocation(AddLocationRequest request)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            response.Result = new GetWarehouseLocationByIDResponse();        //结果集初始化
            LocationInfo location = new LocationInfo();                      //库位类
            //IList<LocationInfo> locations = new List<LocationInfo>();
            //locations = request.Location;
            response.SuccessMessage = new WarehouseAccessor().AddLocation(request.Location, out location);  //获取执行结果

            if (response.SuccessMessage == "操作成功")
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            response.Result.Location = location;
            return response;
        }
        public Response<GetWarehouseLocationByIDResponse> ImportLocation(AddLocationRequest request)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            response.Result = new GetWarehouseLocationByIDResponse();        //结果集初始化
           
            response.SuccessMessage = new WarehouseAccessor().ImportLocation(request.Location);  //获取执行结果

            if (response.SuccessMessage == "操作成功")
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;
        }
        public Response<GetWarehouseLocationByIDResponse> ImportLocationAndGoodShelf(AddLocationRequest request)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            response.Result = new GetWarehouseLocationByIDResponse();        //结果集初始化

            response.SuccessMessage = new WarehouseAccessor().ImportLocationAndGoodShelf(request.Location);  //获取执行结果

            if (response.SuccessMessage == "操作成功")
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;
        }
        public Response<GetWarehouseLocationByIDResponse> DeleteLocation(string ID)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            response.Result = new GetWarehouseLocationByIDResponse();
            response.SuccessMessage = new WarehouseAccessor().DeleteLocation(ID);  //获取执行结果

            if (response.SuccessMessage == "")
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;

        }

        public Response<GetWarehouseLocationByIDResponse> WarehouseDelete(string ID)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            response.Result = new GetWarehouseLocationByIDResponse();
            response.SuccessMessage = new WarehouseAccessor().WarehouseDelete(ID);  //获取执行结果

            if (response.SuccessMessage == "")
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;

        }
        public Response<GetWarehouseLocationByIDResponse> DeleteArea(string ID)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            response.Result = new GetWarehouseLocationByIDResponse();
            response.SuccessMessage = new WarehouseAccessor().DeleteArea(ID);  //获取执行结果

            if (response.SuccessMessage == "")
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;

        }
        public string DeleteMap(long WarehouseID)
        {
            string message = "";
            message = new WarehouseAccessor().DeleteMap(WarehouseID);  //获取执行结果

            return message;

        }

        /// <summary>
        /// 根据库位ID获取库位信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseLocationByIDResponse> GetWarehouseLocationByID(GetWarehouseLocationByIDRequest request)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            response.Result = new GetWarehouseLocationByIDResponse();
            WarehouseInfo warehouse = new WarehouseInfo();
            AreaInfo area = new AreaInfo();
            LocationInfo location = new LocationInfo();
            GoodsShelfInfo goodsShelf = new GoodsShelfInfo();
            response.SuccessMessage = new WarehouseAccessor().GetWarehouseLocationByID(request.ID ?? 0, out warehouse, out area, out location,out goodsShelf);
            response.Result.Warehouse = warehouse;
            response.Result.Area = area;
            response.Result.Location = location;
            response.Result.GoodsShelf = goodsShelf;
            return response;
        }
        /// <summary>
        /// 插入仓库用户权限表   add by hujiaoqiang
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<bool> SetUserWarehouseAllocate(WarehouseAllocateRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.CustomerID == 0 || request.WarehouseID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("SetCustomersWarehouseAllocate request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                response.Result = accessor.SetUserWarehouseAllocate(request.CustomerID, request.WarehouseID, request.Creator);
                response.IsSuccess = response.Result;
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
        /// 添加用户仓库权限表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<bool> SetUserWarehouseAllocates(WarehouseAllocateRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.UserID == 0 || request.WarehouseID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("SetCustomersWarehouseAllocates request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                response.Result = accessor.SetUserWarehouseAllocates(request.UserID, request.WarehouseID, request.Creator);
                response.IsSuccess = response.Result;
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
        /// 查出当前用户已有的仓库权限
        /// </summary>add by hujiaoqiang  20151027
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<WarehouseAllocate>> GetWarehouseAllocate(WarehouseAllocateRequest request)
        {
            Response<IEnumerable<WarehouseAllocate>> response = new Response<IEnumerable<WarehouseAllocate>>();

            if (request == null || request.CustomerID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWarehouseAllocate request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                response.Result = accessor.GetWarehouseAllocate(request.CustomerID);
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
        /// 查询当前用户已有仓库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<WarehouseAllocate>> GetWarehouseAllocates(WarehouseAllocateRequest request)
        {
            Response<IEnumerable<WarehouseAllocate>> response = new Response<IEnumerable<WarehouseAllocate>>();

            if (request == null || request.UserID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWarehouseAllocates request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                response.Result = accessor.GetWarehouseAllocates(request.UserID);
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

        //add by hujiaoqiang  20151027
        public Response<IEnumerable<WarehouseAllocate>> GetAllWarehouseAllocate()
        {
            Response<IEnumerable<WarehouseAllocate>> response = new Response<IEnumerable<WarehouseAllocate>>();
            try
            {
                response.Result = new WarehouseAccessor().GetALLWarehouseAllocate();
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

        //2016-3-9 13:58:47   hzf  查询盘点数据
        /// <summary>
        /// 查看当前盘点信息数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseCheckByConditionResponse> GetWarehouseCheck(GetWarehouseCheckByConditonRequest request)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>() { Result = new GetWarehouseCheckByConditionResponse() };

            if (request.WLSearchCondition == null || request.WLSearchCondition.CustomerID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWarehouseCheck request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {

                WarehouseAccessor accessor = new WarehouseAccessor();
                int RowCount;
                response.Result.WarehouseCheckCollection = accessor.GetWarehouseCheck(request.WLSearchCondition, out RowCount);
                response.Result.PageCount = RowCount;
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
        public Response<GetWarehouseCheckByConditionResponse> GetWarehouseCheckNew(GetWarehouseCheckByConditonRequest request)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>() { Result = new GetWarehouseCheckByConditionResponse() };

            if (request.WLSearchCondition == null || request.WLSearchCondition.CustomerID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWarehouseCheck request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {

                WarehouseAccessor accessor = new WarehouseAccessor();
                int RowCount;
                response.Result.WarehouseCheckDetailCollection = accessor.GetWarehouseCheckNew(request.WLSearchCondition, out RowCount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = RowCount % request.WLSearchCondition.PageSize == 0 ? (RowCount / request.WLSearchCondition.PageSize) : (RowCount / request.WLSearchCondition.PageSize) + 1;
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
        /// 保存当前盘点信息数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseCheckByConditionResponse> GetWarehouseSave(GetWarehouseCheckByConditonRequest request)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>() { Result = new GetWarehouseCheckByConditionResponse() };
            string News = string.Empty;
            try
            {
                News = new WarehouseAccessor().WarehouseCheckSave(request.WLSearchCondition);
                response.IsSuccess = true;
                response.Result.Message = News;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result.Message = "提交失败";
            }

            return response;
        }

        /// <summary>
        /// 查询主数据信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseCheckByConditionResponse> GetWarehouseCheckList(GetWarehouseCheckByConditonRequest request)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();

            try
            {
                int RowCount;
                response.Result = new WarehouseAccessor().GetWarehouseCheckList(request.WLSearchCondition, out RowCount);
                response.Result.PageCount = RowCount % request.WLSearchCondition.PageSize == 0 ? RowCount / request.WLSearchCondition.PageSize : RowCount / request.WLSearchCondition.PageSize + 1;
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
        /// 根据盘点单号获取盘点信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWarehouseCheckByConditionResponse> GetWarehouseCheckByCheckNumber(GetWarehouseCheckByConditonRequest request)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();

            try
            {
                response.Result = new WarehouseAccessor().GetWarehouseCheckByCheckNumber(request.WLSearchCondition);
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
        public Response<GetWarehouseCheckByConditionResponse> ExportWarehouseCheckByCheckNumber(GetWarehouseCheckByConditonRequest request)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();

            try
            {
                response.Result = new WarehouseAccessor().ExportWarehouseCheckByCheckNumber(request.WLSearchCondition);
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
        /// 保存单条盘点信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string SaveWarehouseCheckByCheckNumber(GetWarehouseCheckByConditonRequest request)
        {
            string Message = string.Empty;
            try
            {
                Message = new WarehouseAccessor().SaveWarehouseCheckByCheckNumber(request.WLSearchCondition);
            }
            catch (Exception ex)
            {
                return "更新错误";
            }
            return Message;

        }

        /// <summary>
        /// 删除单条盘点信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetWarehouseCheckDelete(GetWarehouseCheckByConditonRequest request)
        {
            string Message = string.Empty;
            try
            {
                Message = new WarehouseAccessor().GetWarehouseCheckDelete(request.WLSearchCondition);
            }
            catch (Exception ex)
            {
                return "更新错误";
            }
            return Message;

        }

        /// <summary>
        /// 删除单条盘点信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetWarehouseCheckDone(GetWarehouseCheckByConditonRequest request)
        {
            string Message = string.Empty;
            try
            {
                Message = new WarehouseAccessor().GetWarehouseCheckDone(request.WLSearchCondition,request.Warehousecheckdetails);
            }
            catch (Exception ex)
            {
                return "更新错误";
            }
            return Message;

        }

        //public Response<GetWarehouseCheckByConditionResponse> PrintWareHouseCheckNumber(string checknumber)
        //{
        //    Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>() { Result = new GetWarehouseCheckByConditionResponse() };

        //    try
        //    {
        //        WarehouseAccessor accessor = new WarehouseAccessor();
        //        response.Result = accessor.PrintWareHouseCheckNumber(checknumber);
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        response.IsSuccess = false;
        //        response.ErrorCode = ErrorCode.Technical;
        //    }
        //    return response;
        //}

        public Response<GetWarehouseCheckByConditionResponse> GetPrintWareHouseCheckByCheckNumber(string checknumber)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>() { Result = new GetWarehouseCheckByConditionResponse() };

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                response.Result = accessor.GetPrintWareHouseCheckByCheckNumber(checknumber);
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
        public Response<GetWarehouseCheckByConditionResponse> GetPrintWareHouseCheckByCheckNumberNike(string checknumber)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>() { Result = new GetWarehouseCheckByConditionResponse() };

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                response.Result = accessor.GetPrintWareHouseCheckByCheckNumberNike(checknumber);
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
        public Response<GetWarehouseCheckByConditionResponse> GetPrintWareHouseCheckByCheckNumberNikeRF(string checknumber)
        {
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>() { Result = new GetWarehouseCheckByConditionResponse() };

            try
            {
                WarehouseAccessor accessor = new WarehouseAccessor();
                response.Result = accessor.GetPrintWareHouseCheckByCheckNumberNikeRF(checknumber);
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
        /// 根据仓库ID获取库位信息（库区变更）
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<LocationInfo>> ALGetWarehouseLocationListByWarehouseName(string WarehouseName)
        {
            Response<IEnumerable<LocationInfo>> response = new Response<IEnumerable<LocationInfo>>();
            try
            {

                response.Result = new WarehouseAccessor().ALGetWarehouseLocationListByWarehouseName(WarehouseName);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        /// <summary>
        /// 库区变更
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="AreaID"></param>
        /// <param name="ToAreID"></param>
        /// <param name="sqlString"></param>
        /// <param name="LocationCount"></param>
        /// <returns></returns>
        public Response<GetWarehouseLocationByIDResponse> AlImportLocationAndGoodShelf(string ID, string AreaID, string ToAreID, string sqlString, int LocationCount)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            response.Result = new GetWarehouseLocationByIDResponse();        //结果集初始化

            response.SuccessMessage = new WarehouseAccessor().AlImportLocationAndGoodShelf(ID, AreaID, ToAreID, sqlString, LocationCount);  //获取执行结果

            if (response.SuccessMessage.Contains("更新成功"))
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else if (response.SuccessMessage == "库存数量不为0")
            {
                response.IsSuccess = false;
            }
            else if (response.SuccessMessage == "导入库位数与数据库数量不符")
            {
                response.IsSuccess = false;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;


        }


        /// <summary>
        /// 导出库位
        /// </summary>
        /// <returns></returns>
        public Response<GetWLocationByConditionResponse> GetWLocationInfo(GetWLocationByConditonRequest request)
        {
            Response<GetWLocationByConditionResponse> response = new Response<GetWLocationByConditionResponse>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWLocationByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result = new WarehouseAccessor().GetWLocationInfo(request.WLSearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                response.Result.RowCount = RowCount;
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
