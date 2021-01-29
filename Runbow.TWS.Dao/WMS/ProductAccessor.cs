using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.MessageContracts.WMS.Product;

namespace Runbow.TWS.Dao
{
    public class ProductAccessor : BaseAccessor
    {
        public GetProductByConditionResponse QuerySKUProductInfo(ProductSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = "";
            if (SearchCondition != null)
            {
                sqlWhere = SKUSearchCondition(SearchCondition);
            }
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetSKU]", dbParams);
            rowCount = (int)dbParams[3].Value;
            GetProductByConditionResponse response = new GetProductByConditionResponse();
            response.Storer = dt.ConvertToEntityCollection<Storer>();
            response.SearchCondition = dt.ConvertToEntityCollection<ProductStorer>();
            return response;
        }
        /// <summary>
        /// 查询详细SKU
        /// </summary>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public QuerySKUProductResponse QuerySKUProduct(string ID)
        {
            QuerySKUProductResponse response = new QuerySKUProductResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.String, ID, ParameterDirection.Input),
            };

            DataSet ds = this.ExecuteDataSet("[Proc_WMS_QuerySKUProduct]", dbParams);
            response.ProductStorerInfo = ds.Tables[0].ConvertToEntity<ProductStorerInfo>();
            response.InfoDetail = ds.Tables[1].ConvertToEntityCollection<ProductDetail>();
            return response;
            //return dt.ConvertToEntity<ProductStorerInfo>();
            // return response;
        }

        private string SKUSearchCondition(ProductSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(SearchCondition.StorerID))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.StorerID.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.StorerID.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.StorerID.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.StorerID.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and WMS_Product.StorerID in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and WMS_Product.StorerID ='" + SearchCondition.StorerID.Trim() + "' ");
                }
            }
            if (string.IsNullOrWhiteSpace(SearchCondition.StorerID))
            {
                sb.Append(" and WMS_Product.StorerID=null");
            }
            if (!string.IsNullOrEmpty(SearchCondition.SKUClassification))
            {
                sb.Append(" and WMS_Product.SKUClassification='" + SearchCondition.SKUClassification + "'");
            }
            if (!string.IsNullOrEmpty(SearchCondition.SKU))
            {
                IEnumerable<string> SKUList = Enumerable.Empty<string>();
                if (SearchCondition.SKU.IndexOf("\n") > 0)
                {
                    SKUList = SearchCondition.SKU.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.SKU.IndexOf(',') > 0)
                {
                    SKUList = SearchCondition.SKU.Split(',').Select(s => { return s.Trim(); });
                }

                if (SKUList != null && SKUList.Any())
                {
                    SKUList = SKUList.Where(c => !string.IsNullOrEmpty(c));
                }

                if (SKUList != null && SKUList.Any())
                {
                    sb.Append(" AND  WMS_Product.SKU in ( ");
                    foreach (string s in SKUList)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND  WMS_Product.SKU  like '%" + SearchCondition.SKU.Trim() + "%' ");
                }
            }
            //if (!string.IsNullOrWhiteSpace(SearchCondition.SKU))
            //{
            //    sb.Append(" and WMS_Product.SKU='" + SearchCondition.SKU.Trim() + "'");
            //}
            if (!string.IsNullOrWhiteSpace(SearchCondition.UPC))
            {
                sb.Append(" and WMS_ProductDetail.UPC='" + SearchCondition.UPC.Trim() + "'");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Article))
            {
                IEnumerable<string> ArticleList = Enumerable.Empty<string>();
                if (SearchCondition.Article.IndexOf("\n") > 0)
                {
                    ArticleList = SearchCondition.Article.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.Article.IndexOf(',') > 0)
                {
                    ArticleList = SearchCondition.Article.Split(',').Select(s => { return s.Trim(); });
                }

                if (ArticleList != null && ArticleList.Any())
                {
                    ArticleList = ArticleList.Where(c => !string.IsNullOrEmpty(c));
                }

                if (ArticleList != null && ArticleList.Any())
                {
                    sb.Append(" AND  WMS_Product.Str10 in ( ");
                    foreach (string s in ArticleList)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND  WMS_Product.Str10  like '%" + SearchCondition.Article.Trim() + "%' ");
                }
            }
            //if (!string.IsNullOrWhiteSpace(SearchCondition.Article))
            //{
            //    sb.Append(" and WMS_Product.Str10='" + SearchCondition.Article.Trim() + "'");
            //}
            if (!string.IsNullOrWhiteSpace(SearchCondition.Size))
            {
                sb.Append(" and WMS_Product.Str9='" + SearchCondition.Size.Trim() + "'");
            }
            if (!string.IsNullOrWhiteSpace(SearchCondition.Remark))
            {
                sb.Append(" and (WMS_Product.Remark like '%" + SearchCondition.Remark + "%'");
                sb.Append(" or WMS_Product.GoodsName like '%" + SearchCondition.Remark + "%')");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 添加和编辑SKU
        /// </summary>
        /// <param name="productStorer"></param>
        public ProductStorerInfo GetSKUProductInfo(IEnumerable<ProductStorerInfo> productStorer)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_UpdateProduct]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Productdata", productStorer.Select(p => new ProductStorerInfoToDB(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[1].Size = 10;
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (message != "有重复")
                {
                    return ds.Tables[0].ConvertToEntity<ProductStorerInfo>();
                }
                return new ProductStorerInfo();
            }
        }
        /// <summary>
        /// 批量添加SKU
        /// </summary>
        /// <param name="productStorer"></param>
        public IEnumerable<ProductStorerInfo> GetSKUProductInfoExecl(IEnumerable<ProductStorerInfo> productStorer, IEnumerable<ProductDetail> pd)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_UpdateProduct]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Productdata", productStorer.Select(p => new ProductStorerInfoToDB(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ProductDateil",pd.Count()==0?null: pd.Select(p => new ProductDetailToDb(p)));
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Size = 10;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (message != "有重复")
                {
                    return ds.Tables[0].ConvertToEntityCollection<ProductStorerInfo>();
                }
                return null;
            }
        }
        public bool DelSKUProduct(string ID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.String, ID, ParameterDirection.Input),
                new DbParam("@messing", DbType.Int32, 0, ParameterDirection.Output) 
             };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_DelProduct]", dbParams);
            int rowCount = (int)dbParams[1].Value;
            if (rowCount > 0)
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 将ProductStorer表放在缓存里面
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductStorer> GetProductStorerList(string CustomerID)
        {
            CustomerID = " and p.StorerID=" + CustomerID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, CustomerID, ParameterDirection.Input),
            };

            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetProductStorerList]", dbParams);
            return dt.ConvertToEntityCollection<ProductStorer>();
        }
        /// <summary>
        /// 将ProductStorer表放在缓存里面
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductStorer> GetProductStorerList(long CustomerID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.Int64, CustomerID, ParameterDirection.Input),
            };

            DataTable dt = this.ExecuteDataTable("[Proc_WMS_ProductByCustomerIDList]", dbParams);
            return dt.ConvertToEntityCollection<ProductStorer>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductStorer> GetSKUListBySKU(long CustomerID,string SKU)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.Int64, CustomerID, ParameterDirection.Input),
                  new DbParam("@SKU", DbType.String, SKU, ParameterDirection.Input)
            };

            DataTable dt = this.ExecuteDataTable("[Proc_WMS_ProductlistByCustomerSKU]", dbParams);
            return dt.ConvertToEntityCollection<ProductStorer>();
        }

        public IEnumerable<ProductSearch> GetSearchProduct(long CustomerID, List<ProductSearch>  ProductSearch,string type)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetProductByProductSearch", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@ProductSearch", ProductSearch.Select(productsearh => new WMSProductSearch(productsearh)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    conn.Close();
                    return ds.Tables[0].ConvertToEntityCollection<ProductSearch>();
                }
                catch
                {
                    return null;
                }

            }
        }

        public IEnumerable<ProductSearch> GetSearchProductYXDR(long CustomerID, List<ProductSearch> ProductSearch, string type)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetProductByProductSearchYXDR", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@ProductSearch", ProductSearch.Select(productsearh => new WMSProductSearch(productsearh)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    conn.Close();
                    return ds.Tables[0].ConvertToEntityCollection<ProductSearch>();
                }
                catch
                {
                    return null;
                }
            }
        }

        public IEnumerable<ArticleSearch> GetSearchArticle(long CustomerID, List<ArticleSearch> ArticleSearch, string type)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetArticleByArticleSearch", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@ArticleSearch", ArticleSearch.Select(articlesearch => new WMSArticleSearch(articlesearch)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    conn.Close();
                    return ds.Tables[0].ConvertToEntityCollection<ArticleSearch>();
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 将ProductStorer表放在缓存里面
        /// </summary>
        /// <returns></returns>
        public bool DelProductDetail(string ID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.String, ID, ParameterDirection.Input),
                new DbParam("@message", DbType.Int32, 0, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_WMS_DelProductDetail", dbParams);
            int rowCount = (int)dbParams[1].Value;
            if (rowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddProductDetail(IEnumerable<ProductDetail> ProductDetail, string UserName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("Proc_WMS_AddProductDetail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductDetail", ProductDetail.Select(p => new ProductDetailToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@UserName", message);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Size = 500;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;

            }
        }

        public QuerySKUProductResponse QueryProductAndArticleDetail(int StorerID,IEnumerable<WMS_SKUAndArticleNoTable> list)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                QuerySKUProductResponse response = new QuerySKUProductResponse();
                SqlCommand cmd = new SqlCommand("[Proc_WMS_QueryProductAndArticleDetail]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StorerID", StorerID);
                cmd.Parameters[0].SqlDbType = SqlDbType.Int;

                cmd.Parameters.AddWithValue("@SKUAndArticleNo", list.Select(p => new WMS_SKUAndArticleNoToDb(p)));
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                conn.Close();

                response.Info = ds.Tables[0].ConvertToEntityCollection<ProductStorerInfo>();
                response.ArticleDetail = ds.Tables[1].ConvertToEntityCollection<WMS_ArticleDetail>();
                return response;
            }
        }

        public string UpdateProductAndArticleDetail(IEnumerable<ProductStorerInfo> list1, IEnumerable<WMS_ArticleDetail> list2)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                SqlCommand cmd = new SqlCommand("[Proc_WMS_UpdateProductAndArticleDetail]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Productdata", list1.Select(p => new ProductStorerInfoToDB(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ArticleDateil", list2.Select(p => new ArticleDetailToDb(p)));
                cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Size = 10;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                return message;
            }
        }

        public IEnumerable<WMS_Customer> Get_WMS_CustomerByNumbers(IEnumerable<string> nums)
        { 
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_Get_WMS_CustomerByNumbers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReceiptNumbers", nums.Select(p => new Numbers(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.CommandTimeout = 300;
                conn.Open();

                IList<WMS_Customer> stores = new List<WMS_Customer>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    WMS_Customer store = new WMS_Customer();
                    store.StorerKey = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                    store.UserDef2 = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    store.UserDef3 = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                    store.UserDef4 = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                    stores.Add(store);
                }

                return stores;
            }
        }
    }
}