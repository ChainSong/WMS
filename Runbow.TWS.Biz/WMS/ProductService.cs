using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Product;

namespace Runbow.TWS.Biz
{
    public class ProductService : BaseService
    {
        /// <summary>
        /// 查询SKU
        /// </summary>
        /// <returns></returns>
        public Response<GetProductByConditionResponse> QuerySKUProduct(GetProductByConditionRequest request)
        {
            Response<GetProductByConditionResponse> response = new Response<GetProductByConditionResponse>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result = new ProductAccessor().QuerySKUProductInfo(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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


        /// <summary>
        /// 查询SKU
        /// </summary>
        /// <returns></returns>
        public Response<GetProductByConditionResponse> QuerySKUProductFG(GetProductByConditionRequest request)
        {
            Response<GetProductByConditionResponse> response = new Response<GetProductByConditionResponse>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result = new ProductAccessor().QuerySKUProductInfoFG(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        /// <summary>
        /// 查询详细SKU
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<QuerySKUProductResponse> GetSKUProduct(string ID)
        {
            Response<QuerySKUProductResponse> response = new Response<QuerySKUProductResponse>() { Result = new QuerySKUProductResponse() };
            if (ID == null)
            {
                ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                response.Result = new ProductAccessor().QuerySKUProduct(ID);
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
        /// 查询详细SKU
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<QuerySKUProductResponse> GetSKUProductFG(string ID)
        {
            Response<QuerySKUProductResponse> response = new Response<QuerySKUProductResponse>() { Result = new QuerySKUProductResponse() };
            if (ID == null)
            {
                ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                response.Result = new ProductAccessor().QuerySKUProductFG(ID);
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
        /// 编辑添加SKU
        /// </summary>
        /// <returns></returns>
        public Response<EditSKUProductRequest> EditSKUProduct(EditSKUProductRequest request)
        {
            Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>() { Result = new EditSKUProductRequest() };
            // Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>();
            // response.Result = new EditSKUProductRequest();        //结果集初始化

            try
            {
                ProductStorerInfo Product = new ProductStorerInfo();
                IList<ProductStorerInfo> ProductStorer = new List<ProductStorerInfo>();
                ProductStorer.Add(request.Info);
                response.Result.Info = new ProductAccessor().GetSKUProductInfo(ProductStorer);
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
        /// 编辑添加SKU
        /// </summary>
        /// <returns></returns>
        public Response<EditSKUProductRequest> EditSKUProductFG(EditSKUProductRequest request)
        {
            Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>() { Result = new EditSKUProductRequest() };
            // Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>();
            // response.Result = new EditSKUProductRequest();        //结果集初始化

            try
            {
                ProductStorerInfo Product = new ProductStorerInfo();
                IList<ProductStorerInfo> ProductStorer = new List<ProductStorerInfo>();
                ProductStorer.Add(request.Info);
                response.Result.Info = new ProductAccessor().GetSKUProductInfoFG(ProductStorer);
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
        /// 批量添加SKU
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<AddProductExeclResponse> EditSKUProductExecl(AddProductExeclRequest request)
        {
            Response<AddProductExeclResponse> response = new Response<AddProductExeclResponse>() { Result = new AddProductExeclResponse() };
            // Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>();
            // response.Result = new EditSKUProductRequest();        //结果集初始化

            try
            {

                response.Result.Info = new ProductAccessor().GetSKUProductInfoExecl(request.Info,request.InfoDetail);
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



        public Response<IEnumerable<ProductStorer>> GetProductStorerList(string CustomerID)
        {
            Response<IEnumerable<ProductStorer>> response = new Response<IEnumerable<ProductStorer>>();
            // Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>();
            // response.Result = new EditSKUProductRequest();        //结果集初始化

            try
            {

                response.Result = new ProductAccessor().GetProductStorerList(CustomerID);
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
        public Response<IEnumerable<ProductStorer>> GetSKUListBySKU(long CustomerID,string SKU)
        {
            Response<IEnumerable<ProductStorer>> response = new Response<IEnumerable<ProductStorer>>();
            // Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>();
            // response.Result = new EditSKUProductRequest();        //结果集初始化

            try
            {

                response.Result = new ProductAccessor().GetSKUListBySKU(CustomerID, SKU);
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
        public Response<IEnumerable<ProductStorer>> GetProductByCustomerIDList(long CustomerID)
        {
            Response<IEnumerable<ProductStorer>> response = new Response<IEnumerable<ProductStorer>>();
            // Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>();
            // response.Result = new EditSKUProductRequest();        //结果集初始化

            try
            {

                response.Result = new ProductAccessor().GetProductStorerList(CustomerID);
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

        public IEnumerable<ProductSearch> GetSearchProduct(long CustomerID, List<ProductSearch>  ProductSearch,string type)
        {
            IEnumerable<ProductSearch> response;
            // Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>();
            // response.Result = new EditSKUProductRequest();        //结果集初始化

            try
            {

                response = new ProductAccessor().GetSearchProduct(CustomerID, ProductSearch, type);
                
            }
            catch (Exception ex)
            {
                response = null;
            }

            return response;
        }
        public IEnumerable<ProductSearch> GetSearchProductYXDR(long CustomerID, List<ProductSearch> ProductSearch, string type)
        {
            IEnumerable<ProductSearch> response;
            // Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>();
            // response.Result = new EditSKUProductRequest();        //结果集初始化

            try
            {

                response = new ProductAccessor().GetSearchProductYXDR(CustomerID, ProductSearch, type);

            }
            catch (Exception ex)
            {
                response = null;
            }

            return response;
        }
        public IEnumerable<ArticleSearch> GetSearchArticle(long CustomerID, List<ArticleSearch> ArticleSearch, string type)
        {
            IEnumerable<ArticleSearch> response;
            // Response<EditSKUProductRequest> response = new Response<EditSKUProductRequest>();
            // response.Result = new EditSKUProductRequest();        //结果集初始化

            try
            {

                response = new ProductAccessor().GetSearchArticle(CustomerID, ArticleSearch, type);

            }
            catch (Exception ex)
            {
                response = null;
            }

            return response;
        }
        /// <summary>
        /// 删除SKU
        /// </summary>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public bool DelSKUProduct(string ID)
        {
            bool bo = false;
            try
            {
                bo = new ProductAccessor().DelSKUProduct(ID);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return bo;
        }
        /// <summary>
        /// 删除SKU
        /// </summary>
        /// <param name="SKU"></param>
        /// <returns></returns>
        //public bool DelSKUProduct(string ID)
        //{
        //    bool bo = true;
        //    try
        //    {
        //        bo = new ProductAccessor().DelSKUProduct(ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //    }

        //    return bo;
        //}
        /// <summary>
        /// 添加SKU明细
        /// </summary>
        /// <param name="ProductDetail"></param>
        /// <returns></returns>
        public bool AddProductDetail(AddProductExeclRequest ProductDetail)
        {
            bool Result = false;
            try
            {
                Result = new ProductAccessor().AddProductDetail(ProductDetail.InfoDetail, ProductDetail.UserName);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Result;
        }
        /// <summary>
        /// 添加SKU明细
        /// </summary>
        /// <param name="ProductDetail"></param>
        /// <returns></returns>
        public bool DelProductDetail(string ID)
        {
            bool Result = false;
            try
            {
                Result = new ProductAccessor().DelProductDetail(ID);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Result;
        }

        public QuerySKUProductResponse QueryProductAndArticleDetail(int StorerID, IEnumerable<WMS_SKUAndArticleNoTable> list)
        {
            return new ProductAccessor().QueryProductAndArticleDetail(StorerID, list);
        }

        /// <summary>
        /// 根据sku 得到产品名称和产品类型
        /// </summary>add  by hujiaoqiang201511111
        /// <param name="sku"></param>
        /// <returns></returns>
        //public IEnumerable<ProductStorer> GetGoodsNameAndGoodsTypeBySKU(string sku) { 

        // }

        public string UpdateProductAndArticleDetail(IEnumerable<ProductStorerInfo> list1, IEnumerable<WMS_ArticleDetail> list2)
        {
            string Result = "";
            try
            {
                Result = new ProductAccessor().UpdateProductAndArticleDetail(list1,list2);
            }
            catch (Exception ex)
            {
                Result = ex.ToString();
                LogError(ex);
            }

            return Result;
        }

        public IEnumerable<WMS_Customer> Get_WMS_CustomerByNumbers(IEnumerable<string> nums)
        {
            IEnumerable<WMS_Customer> stores = new List<WMS_Customer>();
            try
            {
                stores = new ProductAccessor().Get_WMS_CustomerByNumbers(nums);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return stores;
        }
    }
}
