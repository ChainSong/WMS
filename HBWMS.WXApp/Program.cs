using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.WMS.WXApp;
using Runbow.TWS.Biz;
using Newtonsoft.Json;

namespace HBWMS.WXApp
{
    /// <summary>
    /// 所有参数key都要传，可空参数key的值可为空
    /// </summary>
    class Program
    {
        private static readonly string url = WXAppConstants.WXUrl;
        private static readonly string appKey = WXAppConstants.WXAppKey;
        private static readonly string appSecret = WXAppConstants.WXAppSecret;

        private static readonly int PageSize = 40;

        //示例
        //"http://newshop.tiger8.com.cn/OpenAPI/TigerShop.ITrade.GetSoldTrades/?start_created=&end_created=&status=&buyer_uname=&page_no=&page_size=&app_key=202202261123425020633&timestamp=2020-05-26%2016:42:57&sign=550060E27788C935B3CEA38E73DE26FC
        //"http://newshop.tiger8.com.cn/OpenAPI/TigerShop.ITrade.GetSoldTrades/?app_key=202202261123425020633&buyer_uname=&end_created=&page_no=&page_size=&start_created=&status=&timestamp=2020-05-26%2016:42:57&sign=550060E27788C935B3CEA38E73DE26FC";
        //"http://newshop.tiger8.com.cn/OpenAPI/TigerShop.IProduct.GetSoldProducts/?app_key=202202261123425020633&approve_status=&end_modified=&order_by=&page_no=&page_size=&q=&start_modified=&timestamp=2020-05-26%2016:41:21&sign=DF763B095606A2CC5038762643186256");

        private static WXAppService service = new WXAppService();

        static void Main(string[] args)
        {
            //GetTrade();
            //var dt = service.GetDataTable();
            //GetSoldTrades();
            //GetSoldProducts();
            //GetSoldRefunds();
            //GetRefundDetail("10324");//("202005143100789");
        }

        /// <summary>
        /// 获取当前商家的订单列表（根据创建时间）
        /// </summary>
        public static void GetSoldTrades(int Page_No = 0)
        {
            Page_No++;
            LogHelper.SetPreFilePath = "SoldTrades";
            //总1335条数据
            SoldTradesRequest request = new SoldTradesRequest();
            request.page_no = Page_No;
            //request.start_created = DateTime.Now;
            //request.end_created = DateTime.Now;

            string param = string.Empty;//参数

            Dictionary<string, string> keyValuePairs = SignHelper.GetDictionary<SoldTradesRequest>(request);
            keyValuePairs.Add("app_key", appKey);
            keyValuePairs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            foreach (var kv in keyValuePairs)
            {
                param += kv.Key + "=" + kv.Value + "&";
            }
            param = param.Substring(0, param.Length - 1);
            LogHelper.Info("param参数:" + param);

            ///签名
            string sign = SignHelper.SignTopRequest(keyValuePairs, "md5", appSecret);
            LogHelper.Info("sign签名:" + sign);

            param += "&sign=" + sign;

            string postUrl = url + WXAppConstants.soldtradesUrl;
            LogHelper.Info("url地址:" + postUrl);

            var result = string.Empty;
            try
            {
                result = HttpHelper.HttpGet(postUrl, param);
                LogHelper.Info("result反馈:" + result);
            }
            catch (Exception ex)
            {
                LogHelper.Info("请求错误，error:" + ex.Message);
            }
            var response = new SoldTradesResponse();
            try
            {
                response = JsonConvert.DeserializeObject<SoldTradesResponse>(result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception ex)
            {
                LogHelper.Info("转换错误，error:" + ex.Message);
            }
            //插入数据到数据库
            List<SoldTradeOrder> orderss = new List<SoldTradeOrder>();
            foreach (var item in response.trades_sold_get_response.trades)
            {
                item.orders.ForEach(e => e.tid = item.tid);
                orderss.AddRange(item.orders);
            }
            var res = new WXAppService().GetSoldTrades(response.trades_sold_get_response.trades, orderss);
            if (!string.IsNullOrEmpty(res))
            {
                LogHelper.Info("错误：" + res + ";PageNo:" + Page_No);
            }

            if (response.trades_sold_get_response.has_next)
            {
                GetSoldTrades(Page_No);
            }
        }

        /// <summary>
        /// 查询订单的增量交易数据（根据修改时间）
        /// </summary>
        public static void GetIncrementSoldTrades()
        {
            IncrementSoldTradesRequest request = new IncrementSoldTradesRequest()
            {
                start_modified = DateTime.Now.AddDays(-1),
                end_modified = DateTime.Now
            };

            Dictionary<string, string> keyValuePairs = SignHelper.GetDictionary<IncrementSoldTradesRequest>(request);
            keyValuePairs.Add("app_key", appKey);
            keyValuePairs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            string sign = SignHelper.SignTopRequest(keyValuePairs, "md5", appSecret);

            string postUrl = url + WXAppConstants.incrementsoldtradesUrl;
            string param = string.Empty;
            foreach (var kv in keyValuePairs)
            {
                param += kv.Key + "=" + kv.Value + "&";
            }
            param += "sign=" + sign;

            var result = HttpHelper.HttpGet(postUrl, param);

            try
            {
                var response = JsonConvert.DeserializeObject<IncrementSoldTradesResponse>(result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取单笔交易的详细信息
        /// </summary>
        public static void GetTrade()
        {
            //202005213524461
            TradeRequest request = new TradeRequest()
            {
                tid = "202005213524461"
            };
            Dictionary<string, string> keyValuePairs = SignHelper.GetDictionary<TradeRequest>(request);
            keyValuePairs.Add("app_key", appKey);
            keyValuePairs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            string sign = SignHelper.SignTopRequest(keyValuePairs, "md5", appSecret);

            string postUrl = url + WXAppConstants.tradeUrl;
            string param = string.Empty;
            foreach (var kv in keyValuePairs)
            {
                param += kv.Key + "=" + kv.Value + "&";
            }
            param += "sign=" + sign;

            var result = HttpHelper.HttpGet(postUrl, param);

            try
            {
                var response = JsonConvert.DeserializeObject<TradeResponse>(result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获取当前商家的商品列表
        /// </summary>
        public static void GetSoldProducts()
        {
            LogHelper.SetPreFilePath = "SoldProducts";//日志专属文件夹

            SoldProductsRequest request = new SoldProductsRequest();//请求传参

            int TotalCount = GetSoldProductsCount(request);
            LogHelper.Info("总条数:" + TotalCount);
            int PageCount = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
            LogHelper.Info("总页数:" + PageCount);

            #region 过程
            for (int i = 1; i <= PageCount; i++)
            {
                request.page_no = i;//当前页数

                string param = string.Empty;//参数
                //请求类转参字符串
                Dictionary<string, string> keyValuePairs = SignHelper.GetDictionary<SoldProductsRequest>(request);
                keyValuePairs.Add("app_key", appKey);
                keyValuePairs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                foreach (var kv in keyValuePairs)
                {
                    param += kv.Key + "=" + kv.Value + "&";
                }
                param = param.Substring(0, param.Length - 1);
                LogHelper.Info("param参数:" + param);

                //签名
                string sign = SignHelper.SignTopRequest(keyValuePairs, "md5", appSecret);
                LogHelper.Info("sign签名:" + sign);

                param += "&sign=" + sign;

                string postUrl = url + WXAppConstants.soldproductsUrl;//请求地址

                var result = string.Empty;//请求结果
                try
                {
                    result = HttpHelper.HttpGet(postUrl, param);
                    LogHelper.Info("请求结果:" + result);
                }
                catch (Exception ex)
                {
                    LogHelper.Info("请求失败:" + ex.Message);
                }

                SoldProductsResponse response = null;//反馈

                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        response = JsonConvert.DeserializeObject<SoldProductsResponse>(result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Info("转换失败:" + ex.Message);
                    }
                }

                if (response != null)
                {
                    List<SoldProduct> soldProducts = response.products_get_response.items;//商品集合
                    if (soldProducts != null && soldProducts.Any())
                    {
                        foreach (var item in soldProducts)
                        {
                            //获取商品的详细信息
                            GetProduct(item.num_iid);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取商品总条数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int GetSoldProductsCount(SoldProductsRequest request)
        {
            int totalCount = 0;

            #region MyRegion

            string param = string.Empty;//参数

            //请求类转参字符串
            Dictionary<string, string> keyValuePairs = SignHelper.GetDictionary<SoldProductsRequest>(request);
            keyValuePairs.Add("app_key", appKey);
            keyValuePairs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            foreach (var kv in keyValuePairs)
            {
                param += kv.Key + "=" + kv.Value + "&";
            }
            param = param.Substring(0, param.Length - 1);
            LogHelper.Info("param参数:" + param);

            //签名
            string sign = SignHelper.SignTopRequest(keyValuePairs, "md5", appSecret);
            LogHelper.Info("sign签名:" + sign);

            param += "&sign=" + sign;

            string postUrl = url + WXAppConstants.soldproductsUrl;//请求地址

            var result = string.Empty;//请求结果
            try
            {
                result = HttpHelper.HttpGet(postUrl, param);
                LogHelper.Info("请求结果:" + result);
            }
            catch (Exception ex)
            {
                LogHelper.Info("请求失败:" + ex.Message);
            }

            SoldProductsResponse response = null;//反馈

            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    response = JsonConvert.DeserializeObject<SoldProductsResponse>(result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                }
                catch (Exception ex)
                {
                    LogHelper.Info("转换失败:" + ex.Message);
                }
            }

            if (response != null)
            {
                totalCount = response.products_get_response.total_results;
            }
            #endregion

            return totalCount;
        }

        /// <summary>
        /// 获取指定商品的详细信息
        /// </summary>
        public static void GetProduct(int num_iid)
        {
            LogHelper.SetPreFilePath = "Product";
            ProductRequest request = new ProductRequest() //传参
            {
                num_iid = num_iid
            };
            LogHelper.Info("商品编号:" + num_iid);

            string param = string.Empty;
            Dictionary<string, string> keyValuePairs = SignHelper.GetDictionary<ProductRequest>(request);
            keyValuePairs.Add("app_key", appKey);
            keyValuePairs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            foreach (var kv in keyValuePairs)
            {
                param += kv.Key + "=" + kv.Value + "&";
            }
            param = param.Substring(0, param.Length - 1);
            LogHelper.Info("param参数:" + param);

            string sign = SignHelper.SignTopRequest(keyValuePairs, "md5", appSecret);
            LogHelper.Info("sign签名:" + sign);

            param += "&sign=" + sign;

            string postUrl = url + WXAppConstants.productUrl;

            var result = string.Empty;//请求结果
            try
            {
                result = HttpHelper.HttpGet(postUrl, param);
                LogHelper.Info("请求结果:" + result);
            }
            catch (Exception ex)
            {
                LogHelper.Info("请求失败:" + ex.Message);
            }

            ProductResponse response = null;

            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    response = JsonConvert.DeserializeObject<ProductResponse>(result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                }
                catch (Exception ex)
                {
                    LogHelper.Info("转换失败:" + ex.Message);
                }
            }

            if (response != null)
            {
                List<Product> ps = new List<Product>();
                Product p = response.product_get_response.item;
                ps.Add(p);
                response.product_get_response.item.skus.ForEach(i => i.num_iid = p.num_iid);
                //List<ProductSku> pskus = response.product_get_response.item.skus.ForEach(i => i.num_iid =i.num_iid);
                List<ProductSku> pds = response.product_get_response.item.skus;

                var ds = service.AddSoldProductOrDetail(ps, pds);
            }

        }

        /// <summary>
        /// 获取售后列表（根据申请时间）
        /// </summary>
        public static void GetSoldRefunds()
        {
            LogHelper.SetPreFilePath = "SoldRefund";
            SoldRefundsRequest request = new SoldRefundsRequest();

            int TotalCount = GetSoldRefundsCount(request);
            LogHelper.Info("总条数:" + TotalCount);
            int PageCount = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
            LogHelper.Info("总页数:" + PageCount);

            #region 过程
            for (int i = 1; i <= PageCount; i++)
            {
                request.page_no = i;

                string param = string.Empty;
                Dictionary<string, string> keyValuePairs = SignHelper.GetDictionary<SoldRefundsRequest>(request);
                keyValuePairs.Add("app_key", appKey);
                keyValuePairs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                foreach (var kv in keyValuePairs)
                {
                    param += kv.Key + "=" + kv.Value + "&";
                }
                param = param.Substring(0, param.Length - 1);
                LogHelper.Info("param参数:" + param);

                string sign = SignHelper.SignTopRequest(keyValuePairs, "md5", appSecret);
                LogHelper.Info("sign签名:" + sign);

                param += "&sign=" + sign;

                string postUrl = url + WXAppConstants.soldrefundsUrl;

                var result = string.Empty;//请求结果
                try
                {
                    result = HttpHelper.HttpGet(postUrl, param);
                    LogHelper.Info("请求结果:" + result);
                }
                catch (Exception ex)
                {
                    LogHelper.Info("请求失败:" + ex.Message);
                }

                SoldRefundsResponse response = null;

                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        response = JsonConvert.DeserializeObject<SoldRefundsResponse>(result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Info("转换失败:" + ex.Message);
                    }
                }

                if (response != null)
                {
                    List<RefundDetail> rds = new List<RefundDetail>();
                    List<SoldRefund> soldRefunds = response.refunds_get_response.refunds;
                    foreach (var item in soldRefunds)
                    {
                        RefundDetail rd = GetRefundDetail(item.returnId.ToString());
                        rd.userSendGoodsTime = item.userSendGoodsTime;

                        rds.Add(rd);
                    }
                    try
                    {
                        var ms = service.AddSoldRefundDetail(rds);
                        LogHelper.Info("返回信息" + ms);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            #endregion
        }

        public static int GetSoldRefundsCount(SoldRefundsRequest request)
        {
            int totalCount = 0;

            #region MyRegion
            string param = string.Empty;
            Dictionary<string, string> keyValuePairs = SignHelper.GetDictionary<SoldRefundsRequest>(request);
            keyValuePairs.Add("app_key", appKey);
            keyValuePairs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            foreach (var kv in keyValuePairs)
            {
                param += kv.Key + "=" + kv.Value + "&";
            }
            param = param.Substring(0, param.Length - 1);
            LogHelper.Info("param参数:" + param);

            string sign = SignHelper.SignTopRequest(keyValuePairs, "md5", appSecret);
            LogHelper.Info("sign签名:" + sign);

            param += "&sign=" + sign;

            string postUrl = url + WXAppConstants.soldrefundsUrl;

            var result = string.Empty;//请求结果
            try
            {
                result = HttpHelper.HttpGet(postUrl, param);
                LogHelper.Info("请求结果:" + result);
            }
            catch (Exception ex)
            {
                LogHelper.Info("请求失败:" + ex.Message);
            }

            SoldRefundsResponse response = null;

            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    response = JsonConvert.DeserializeObject<SoldRefundsResponse>(result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                }
                catch (Exception ex)
                {
                    LogHelper.Info("转换失败:" + ex.Message);
                }
            }

            if (response != null)
            {
                totalCount = response.refunds_get_response.total_results;
            }
            #endregion

            return totalCount;
        }

        /// <summary>
        /// 获取单笔退货的详细信息
        /// </summary>
        /// <param name=""></param>
        public static RefundDetail GetRefundDetail(string orderId)
        {
            RefundDetail refundDetail = null;
            LogHelper.SetPreFilePath = "RefundDetail";
            RefundDetailRequest request = new RefundDetailRequest()
            {
                tid = orderId
            };

            string param = string.Empty;
            Dictionary<string, string> keyValuePairs = SignHelper.GetDictionary<RefundDetailRequest>(request);
            keyValuePairs.Add("app_key", appKey);
            keyValuePairs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            foreach (var kv in keyValuePairs)
            {
                param += kv.Key + "=" + kv.Value + "&";
            }
            param = param.Substring(0, param.Length - 1);
            LogHelper.Info("param参数:" + param);

            string sign = SignHelper.SignTopRequest(keyValuePairs, "md5", appSecret);
            LogHelper.Info("sign签名:" + sign);

            param += "&sign=" + sign;

            string postUrl = url + WXAppConstants.refundretailUrl;

            var result = string.Empty;//请求结果
            try
            {
                result = HttpHelper.HttpGet(postUrl, param);
                LogHelper.Info("请求结果:" + result);
            }
            catch (Exception ex)
            {
                LogHelper.Info("请求失败:" + ex.Message);
            }

            RefundDetailResponse response = null;

            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    response = JsonConvert.DeserializeObject<RefundDetailResponse>(result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                }
                catch (Exception ex)
                {
                    LogHelper.Info("转换失败:" + ex.Message);
                }
            }

            if (response != null)
            {
                refundDetail = response.refund_get_response.refund;
            }
            return refundDetail;
        }


    }
}
