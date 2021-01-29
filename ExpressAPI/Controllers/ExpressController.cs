using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using Runbow.TWS.Common;
using Runbow.TWS.Biz.WebApi;
using Runbow.TWS.MessageContracts.WebApi.Express;
using Runbow.TWS.Entity.WebApi.Express;
using Runbow.TWS.Entity;
using ExpressAPI.Common;
using Runbow.TWS.MessageContracts.WebApi;
using Newtonsoft.Json;

namespace ExpressAPI.Controllers
{
    /// <summary>
    /// 快递接口
    /// </summary>
    public class ExpressController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Test(string IDs)
        {
            return GetJsonData(IDs);
        }

        [HttpPost]
        public HttpResponseMessage GetExpressNum([FromBody]ApiRequest request)
        {
            ApiResponse response = new ApiResponse();
            if (request.ExpressCompany == "DP")
            {
                response = GetExpressNumByDeppon(request);
            }
            else if (request.ExpressCompany == "YTO")
            {
                response = GetExpressNumByYto(request);
            }
            else if (request.ExpressCompany == "YD")
            {
                response = GetExpressNumByYd(request);
            }
            else
            {

            }

            return GetJsonData(response);
        }


        /// <summary>
        /// 德邦快递
        /// 下单服务接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse GetExpressNumByDeppon(ApiRequest apiRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            ///月结 有子母件 大件快递360
            ExpressService service = new ExpressService();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string url = ExpressConstants.dpCreateOrderUrl;
            string appKey = ExpressConstants.dpAppKey;
            string companyCode = ExpressConstants.dbCompanyCode;
            string sign = ExpressConstants.dpSign;
            string customerCode = ExpressConstants.jtCustomerCode;//测试用219401,219402

            LogHelper.SetPreFilePath = "Deppon";
            DPCreateOrderRequest request = new DPCreateOrderRequest();//订单信息

            ExpressResponse res = service.GetOrderByExpress(apiRequest.PackageNumber, apiRequest.OrderType).Result;

            if (res.orderInfo != null)
            {
                if (res.packageInfos != null && res.packageInfos.Any())
                {
                    LogHelper.Info("获取订单以及包装信息");
                    request.logisticID = sign + res.orderInfo.OrderNumber.Substring(2);
                    request.custOrderNo = res.orderInfo.OrderNumber;
                    request.companyCode = companyCode;
                    request.orderType = "2";
                    request.transportType = "RCP";//大件快递360
                    request.customerCode = customerCode;
                    request.sender = new Sender()
                    {
                        name = res.warehouseInfo.Contractor,
                        mobile = res.warehouseInfo.Mobile,
                        province = "陕西省",
                        city = "西安市",
                        county = res.warehouseInfo.ProvinceCity,
                        address = res.warehouseInfo.Address
                    };
                    request.receiver = new Receiver()
                    {
                        name = res.orderInfo.Consignee,
                        mobile = res.orderInfo.Contact,
                        province = res.orderInfo.Province,
                        city = res.orderInfo.City,
                        county = res.orderInfo.District,
                        address = res.orderInfo.Address
                    };
                    request.packageInfo = new Runbow.TWS.MessageContracts.WebApi.Express.PackageInfo()
                    {
                        cargoName = "货物名",
                        totalNumber = apiRequest.OrderType == "A" ? res.packageInfos.Count() : 1,
                        totalWeight = Convert.ToDouble(res.packageInfos.Sum(i => i.NetWeight.AsDecimal())),
                        deliveryType = "9"
                    };
                    request.gmtCommit = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    request.payType = "2";

                    string timeStamp = getTimeStamp();
                    LogHelper.Info("timeStamp时间戳：" + timeStamp);
                    string _params = js.Serialize(request);
                    LogHelper.Info("params请求参数：" + _params);
                    string digest = getDigest(_params + appKey + timeStamp);
                    LogHelper.Info("digest密文摘要：" + digest);

                    StringBuilder sb = new StringBuilder();
                    sb.Append("params=" + _params);
                    sb.Append("&digest=" + digest);
                    sb.Append("&timestamp=" + timeStamp);
                    sb.Append("&companyCode=" + companyCode);
                    string result = string.Empty;
                    try
                    {
                        result = this.Post(url, sb.ToString());
                        LogHelper.Info("返回结果：" + result);
                    }
                    catch (Exception ex)
                    {
                        apiResponse.result = "false";
                        apiResponse.resultCode = "203";
                        apiResponse.reason = "接口请求失败：" + ex.Message;
                        LogHelper.Info("返回失败，错误：" + ex.Message);
                        return apiResponse;
                    }

                    DPCreateOrderResponse response = null;
                    try
                    {
                        response = js.Deserialize<DPCreateOrderResponse>(result);
                    }
                    catch (Exception ex)
                    {
                        apiResponse.result = "false";
                        apiResponse.resultCode = "204";
                        apiResponse.reason = "接口反馈反序列化失败：" + ex.Message;
                        LogHelper.Info("反序列化失败，错误：" + ex.Message);
                        return apiResponse;
                    }

                    if (response != null && response.result == "true")
                    {
                        if (apiRequest.OrderType == "A")
                        {
                            var mails = response.mailNo.Split(',');
                            res.packageInfos.Each((i, item) =>
                            {
                                item.ExpressNumber = mails[i];
                            });
                        }
                        else
                        {
                            res.packageInfos.ForEach(a => a.ExpressNumber = response.mailNo);
                        }
                        ///插入快递表WMS_ExpressDelivery，更新包装主表WMS_Package
                        service.AddExpressAndUpdatePackage(response, res.packageInfos);
                    }
                    else
                    {
                        LogHelper.Info("获取快递失败，错误：" + result);
                    }
                    apiResponse.result = response.result;
                    apiResponse.resultCode = response.resultCode;
                    apiResponse.reason = response.reason;
                    //return GetJsonData(response);
                }
                else
                {
                    apiResponse.result = "true";
                    apiResponse.resultCode = "202";
                    apiResponse.reason = "订单包装已全部绑定快递单号";
                }
            }
            else
            {
                apiResponse.result = "false";
                apiResponse.resultCode = "201";
                apiResponse.reason = "获取订单及包装信息失败";
            }
            return apiResponse;
        }



        /// <summary>
        /// 韵达快递
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse GetExpressNumByYd([FromBody]ApiRequest apiRequest)
        {
            ApiResponse apiResponse = new ApiResponse();

            StringBuilder msg = new StringBuilder();

            string partnerID = ExpressConstants.ydPartnerID;//客户ID
            string password = ExpressConstants.ydPassword;//接口联调密码
            string ydUrl = ExpressConstants.ydUrl;//接口地址

            LogHelper.SetPreFilePath = "YD";
            ExpressService service = new ExpressService();
            var response = service.GetOrderByExpress(apiRequest.PackageNumber, apiRequest.OrderType).Result;
            if (response.orderInfo != null)
            {
                if (response.packageInfos != null && response.packageInfos.Any())
                {
                    LogHelper.Info("获取订单信息及包装明细");
                    OrderInfo orderInfo = response.orderInfo;
                    List<PackageDetailInfo> packageDetailInfos = null;
                    WarehouseInfo warehouseInfo = response.warehouseInfo;

                    if (apiRequest.OrderType == "A")
                    {
                        foreach (var pack in response.packageInfos)
                        {
                            packageDetailInfos = response.packageDetailInfos.Where(p => p.PID == pack.ID).ToList();

                            YDRequest request = new YDRequest();
                            request.orders = new List<YdRequestParam>();
                            YdRequestParam order = new YdRequestParam()
                            {
                                order_serial_no = orderInfo.OrderNumber,
                                khddh = orderInfo.OrderNumber,
                                order_type = "common",
                                //weight = 11,
                                //size = "20,20,20",
                                //value = 20,
                                //node_id = "",
                                //remark = "",
                                //cus_area1 = "订单号：123123  \n 批次号：152201 "
                            };
                            order.sender = new Runbow.TWS.Entity.WebApi.Express.User()
                            {
                                name = warehouseInfo.WarehouseName,
                                company = "虹迪",
                                city = "上海市,上海市,金山区",
                                address = "上海市,上海市,金山区" + warehouseInfo.Address,
                                mobile = warehouseInfo.Mobile
                            };
                            order.receiver = new Runbow.TWS.Entity.WebApi.Express.User()
                            {
                                name = orderInfo.Consignee,
                                company = "",
                                city = orderInfo.Province + "," + orderInfo.City + "," + orderInfo.District,
                                address = orderInfo.Address,
                                mobile = orderInfo.Contact
                            };
                            order.items = new SkuItem();
                            order.items.item = new List<Sku>();
                            foreach (var pd in packageDetailInfos)
                            {
                                order.items.item.Add(new Sku()
                                {
                                    name = pd.GoodsName,
                                    number = Convert.ToInt32(pd.Qty.Value),
                                    remark = ""
                                });
                            }
                            request.orders.Add(order);
                            var xmlData = CommonHelper.XmlSerialize(request, Encoding.UTF8);//请求订单数据
                            var xmlDataToBase64 = CommonHelper.ToBASE64(xmlData);//BASE64 编码

                            //签名 md5(xmldata + partnerid + 密码),xmldata需进行BASE64编码
                            var validation = CommonHelper.ToMD5(xmlDataToBase64 + partnerID + password);

                            StringBuilder sb = new StringBuilder();
                            sb.Append("partnerid=" + ExpressConstants.ydPartnerID);
                            sb.Append("&version=" + ExpressConstants.ydVersion);
                            sb.Append("&request=generalOrderApi");
                            sb.Append("&xmldata=" + CommonHelper.UrlEncode(xmlDataToBase64));
                            sb.Append("&validation=" + validation);

                            string result = string.Empty;
                            try
                            {
                                result = CommonHelper.HttpPost(ydUrl, sb.ToString());
                                LogHelper.Info("韵达接口反馈：" + result);
                            }
                            catch (Exception ex)
                            {
                                msg.Append("韵达接口反馈失败，错误：" + ex.Message);
                                LogHelper.Info("韵达接口反馈失败，错误：" + ex.Message);
                            }

                            var data = CommonHelper.XmlDeserialize<YDResponse>(result);

                            YdResponseParam responseParam = new YdResponseParam();
                            if (data != null)
                            {
                                responseParam = data.responses.FirstOrDefault();
                                var jsonData = CommonHelper.UnicodeToString(responseParam.pdf_info);//Unicode转中文
                                LogHelper.Info("jsonData：" + jsonData);
                                var dataList = new List<PdfInfoObj>();
                                if (!string.IsNullOrEmpty(jsonData))
                                {
                                    //json字符串切分
                                    var jsonResult = jsonData.Substring(1, jsonData.IndexOf(",["));
                                    jsonResult = jsonResult.Substring(0, jsonResult.Length - 1) + "]";
                                    LogHelper.Info("反序列化字符串：" + jsonResult);

                                    dataList = JsonConvert.DeserializeObject<List<PdfInfoObj>>(jsonResult);
                                }
                                PdfInfoObj pdfInfoObj = dataList.Count() == 0 ? new PdfInfoObj() : dataList.First();

                                try
                                {
                                    var responseResult = service.AddExpressAndUpdatePackageYD(responseParam, response.packageInfos.FirstOrDefault(), pdfInfoObj);
                                    if (responseResult.IsSuccess)
                                    {

                                    }
                                    else
                                    {
                                        msg.Append(responseResult.Result);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    msg.Append("新增失败：" + ex.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        packageDetailInfos = response.packageDetailInfos;

                        YDRequest request = new YDRequest();
                        request.orders = new List<YdRequestParam>();
                        YdRequestParam order = new YdRequestParam()
                        {
                            order_serial_no = orderInfo.OrderNumber,
                            khddh = orderInfo.OrderNumber,
                            order_type = "common",
                            //weight = 11,
                            //size = "20,20,20",
                            //value = 20,
                            //node_id = "",
                            //remark = "",
                            //cus_area1 = "订单号：123123  \n 批次号：152201 "
                        };
                        order.sender = new Runbow.TWS.Entity.WebApi.Express.User()
                        {
                            name = warehouseInfo.WarehouseName,
                            company = "虹迪",
                            city = "上海市,上海市,金山区",
                            address = "上海市,上海市,金山区" + warehouseInfo.Address,
                            mobile = warehouseInfo.Mobile
                        };
                        order.receiver = new Runbow.TWS.Entity.WebApi.Express.User()
                        {
                            name = orderInfo.Consignee,
                            company = "",
                            city = orderInfo.Province + "," + orderInfo.City + "," + orderInfo.District,
                            address = orderInfo.Address,
                            mobile = orderInfo.Contact
                        };
                        order.items = new SkuItem();
                        order.items.item = new List<Sku>();
                        foreach (var pd in packageDetailInfos)
                        {
                            order.items.item.Add(new Sku()
                            {
                                name = pd.GoodsName,
                                number = Convert.ToInt32(pd.Qty.Value),
                                remark = ""
                            });
                        }
                        request.orders.Add(order);
                        var xmlData = CommonHelper.XmlSerialize(request, Encoding.UTF8);//请求订单数据
                        var xmlDataToBase64 = CommonHelper.ToBASE64(xmlData);//BASE64 编码

                        //签名 md5(xmldata + partnerid + 密码),xmldata需进行BASE64编码
                        var validation = CommonHelper.ToMD5(xmlDataToBase64 + partnerID + password);

                        StringBuilder sb = new StringBuilder();
                        sb.Append("partnerid=" + ExpressConstants.ydPartnerID);
                        sb.Append("&version=" + ExpressConstants.ydVersion);
                        sb.Append("&request=generalOrderApi");
                        sb.Append("&xmldata=" + CommonHelper.UrlEncode(xmlDataToBase64));
                        sb.Append("&validation=" + validation);

                        string result = string.Empty;
                        try
                        {
                            result = CommonHelper.HttpPost(ydUrl, sb.ToString());
                            LogHelper.Info("韵达接口反馈：" + result);
                        }
                        catch (Exception ex)
                        {
                            msg.Append("韵达接口反馈失败，错误：" + ex.Message);
                            LogHelper.Info("韵达接口反馈失败，错误：" + ex.Message);
                        }

                        var data = CommonHelper.XmlDeserialize<YDResponse>(result);

                        YdResponseParam responseParam = new YdResponseParam();
                        if (data != null)
                        {
                            responseParam = data.responses.FirstOrDefault();
                            var jsonData = CommonHelper.UnicodeToString(responseParam.pdf_info);//Unicode转中文
                            LogHelper.Info("jsonData：" + jsonData);
                            var dataList = new List<PdfInfoObj>();
                            if (!string.IsNullOrEmpty(jsonData))
                            {
                                //json字符串切分
                                var jsonResult = jsonData.Substring(1, jsonData.IndexOf(",["));
                                jsonResult = jsonResult.Substring(0, jsonResult.Length - 1) + "]";
                                LogHelper.Info("反序列化字符串：" + jsonResult);

                                dataList = JsonConvert.DeserializeObject<List<PdfInfoObj>>(jsonResult);
                            }
                            PdfInfoObj pdfInfoObj = dataList.Count() == 0 ? new PdfInfoObj() : dataList.First();

                            try
                            {
                                var responseResult = service.AddExpressAndUpdatePackageYD(responseParam, response.packageInfos.FirstOrDefault(), pdfInfoObj);
                                if (responseResult.IsSuccess)
                                {

                                }
                                else
                                {
                                    msg.Append(responseResult.Result);
                                }
                            }
                            catch (Exception ex)
                            {
                                msg.Append("新增失败：" + ex.Message);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(msg.ToString()))
                    {
                        apiResponse.result = "false";
                        apiResponse.resultCode = "203";
                        apiResponse.reason = msg.ToString();
                    }
                    else
                    {
                        apiResponse.result = "true";
                        apiResponse.resultCode = "1000";
                        apiResponse.reason = "";
                    }
                }
                else
                {
                    apiResponse.result = "true";
                    apiResponse.resultCode = "202";
                    apiResponse.reason = "订单包装已全部绑定快递单号";
                }
            }
            else
            {
                apiResponse.result = "false";
                apiResponse.resultCode = "201";
                apiResponse.reason = "获取订单及包装信息失败";
            }
            return apiResponse;
        }



        /// <summary>
        /// 圆通快递
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        [HttpPost]
        public ApiResponse GetExpressNumByYto([FromBody]ApiRequest apiRequest)
        {
            ApiResponse apiResponse = new ApiResponse();

            //IDs = "TRB20091810010360040";
            ExpressService service = new ExpressService();

            //客户编码（电商标识）
            string clientId = ConfigHelper.GetConfigValue("YtoClientID");
            //签名key
            string partnerId = ConfigHelper.GetConfigValue("YtoPartnerID");

            StringBuilder msg = new StringBuilder();

            try
            {
                YtoExpressrNumberInfo numberInfo = new YtoExpressrNumberInfo();

                var response = service.GetOrderByExpress(apiRequest.PackageNumber, apiRequest.OrderType);
                if (response.IsSuccess && response.Result.orderInfo != null)
                {
                    if (response.Result.packageInfos != null && response.Result.packageInfos.Any())
                    {
                        OrderInfo orderInfo = response.Result.orderInfo;
                        List<PackageDetailInfo> packageDetailInfos = null;
                        WarehouseInfo warehouseInfo = response.Result.warehouseInfo;

                        if (apiRequest.OrderType == "A")
                        {
                            foreach (var pack in response.Result.packageInfos)
                            {
                                packageDetailInfos = response.Result.packageDetailInfos.Where(p => p.PID == pack.ID).ToList();

                                //发件人信息
                                YtoSender ytoSender = new YtoSender();
                                ytoSender.phone = warehouseInfo.Mobile;
                                ytoSender.name = warehouseInfo.Contractor;
                                ytoSender.prov = warehouseInfo.ProvinceCity;
                                ytoSender.city = warehouseInfo.Str1 + "," + warehouseInfo.Str2;
                                ytoSender.address = warehouseInfo.Address;
                                numberInfo.sender = ytoSender;


                                //收件人信息
                                YtoReceiver ytoReceiver = new YtoReceiver();
                                ytoReceiver.phone = orderInfo.Contact;
                                ytoReceiver.name = orderInfo.Consignee;
                                ytoReceiver.prov = orderInfo.Province;
                                ytoReceiver.city = orderInfo.City + "," + orderInfo.District;
                                ytoReceiver.address = orderInfo.Address;
                                numberInfo.receiver = ytoReceiver;


                                numberInfo.clientID = clientId;
                                numberInfo.logisticProviderID = "YTO";
                                numberInfo.customerId = clientId;
                                numberInfo.txLogisticID = "YTO" + pack.PackageNumber;
                                numberInfo.tradeNo = clientId;
                                numberInfo.orderType = orderTypeint("普通订单");
                                numberInfo.serviceType = serviceTypeint("上门揽收");
                                numberInfo.itemName = string.Join(",", packageDetailInfos.Select(a => a.GoodsName));
                                numberInfo.number = (int)packageDetailInfos.Sum(a => a.Qty).Value;

                                //消息内容
                                string logistics_interface = XmlSerializeHelper.XmlSerialize(numberInfo, Encoding.UTF8);
                                //消息签名
                                string data_digest = System.Web.HttpUtility.UrlEncode(base64(orderMD5(logistics_interface + partnerId)));

                                //订单类型（online:在线下单，offline:线下下单）
                                string type = "offline";

                                string strUrl = ConfigHelper.GetConfigValue("YtoURL") + "logistics_interface=" + System.Web.HttpUtility.UrlEncode(logistics_interface) + "&data_digest=" + data_digest + "&type=" + type + "&clientId=" + clientId;

                                string dataStr = string.Empty;
                                try
                                {
                                    dataStr = PostPage(strUrl, "");
                                }
                                catch (Exception ex)
                                {
                                    msg.Append("箱号：" + pack.PackageNumber + "获取快递失败，" + ex.Message);
                                    continue;
                                }

                                var data = XmlSerializeHelper.DESerializer<Runbow.TWS.Entity.WebApi.Express.Ytofeedback>(dataStr);
                                if (data.code == "200")
                                {
                                    YtoRequest ytoRequest = new YtoRequest();
                                    var qrCode = JSONString<qrCode>(data.qrCode);
                                    ytoRequest.expressDelivery = new ExpressDelivery();
                                    ytoRequest.expressDelivery.CustomerID = response.Result.orderInfo.CustomerID.Value;
                                    ytoRequest.expressDelivery.CustomerName = response.Result.orderInfo.CustomerName;
                                    ytoRequest.expressDelivery.WarehouseID = response.Result.orderInfo.WarehouseID;
                                    ytoRequest.expressDelivery.WarehouseName = response.Result.orderInfo.Warehouse;
                                    ytoRequest.expressDelivery.OID = response.Result.orderInfo.ID;
                                    ytoRequest.expressDelivery.OrderNumber = response.Result.orderInfo.OrderNumber;
                                    ytoRequest.expressDelivery.ExternOrderNumber = response.Result.orderInfo.ExternOrderNumber;
                                    ytoRequest.expressDelivery.ExpressNumber = data.mailNo; //data.txLogisticID;
                                    ytoRequest.expressDelivery.ExpressCompany = "圆通";
                                    ytoRequest.expressDelivery.PackageNumber = apiRequest.PackageNumber;
                                    ytoRequest.expressDelivery.Status = "0";
                                    ytoRequest.expressDelivery.success = data.success;
                                    ytoRequest.expressDelivery.code = data.code;
                                    ytoRequest.expressDelivery.logisticProviderID = data.logisticProviderID;
                                    ytoRequest.expressDelivery.txLogisticID = data.txLogisticID;
                                    ytoRequest.expressDelivery.mailNo = data.mailNo;
                                    ytoRequest.expressDelivery.originateOrgCode = data.originateOrgCode;
                                    //ytoRequest.expressDelivery.reason = data.reason;
                                    ytoRequest.expressDelivery.consigneeBranchCode = data.distributeInfo.consigneeBranchCode;
                                    ytoRequest.expressDelivery.packageCenterCode = data.distributeInfo.packageCenterCode;
                                    ytoRequest.expressDelivery.packageCenterName = data.distributeInfo.packageCenterName;
                                    ytoRequest.expressDelivery.printKeyWord = data.distributeInfo.printKeyWord;
                                    ytoRequest.expressDelivery.shortAddress = data.distributeInfo.shortAddress;
                                    ytoRequest.expressDelivery.mn = qrCode.mn;
                                    ytoRequest.expressDelivery.pcn = qrCode.pcn;
                                    ytoRequest.expressDelivery.rbc = qrCode.rbc;
                                    ytoRequest.expressDelivery.sbc = qrCode.sbc;
                                    ytoRequest.expressDelivery.ssc = qrCode.ssc;
                                    ytoRequest.expressDelivery.tsc = qrCode.tsc;
                                    ytoRequest.expressDelivery.Creator = "圆通";
                                    ytoRequest.expressDelivery.CreateTime = DateTime.Now.ToString();
                                    //YtoResponse ytoResponse = new YtoResponse();
                                    //ytoResponse.Response = new Runbow.TWS.Entity.WebApi.Express.Ytofeedback();
                                    var InsExpressNumdata = service.InsExpressNumYto(ytoRequest);
                                }
                                else
                                {
                                    msg.Append("箱号：" + pack.PackageNumber + "快递反馈失败");
                                }
                            }
                        }
                        else
                        {
                            packageDetailInfos = response.Result.packageDetailInfos;

                            //发件人信息
                            YtoSender ytoSender = new YtoSender();
                            ytoSender.phone = warehouseInfo.Mobile;
                            ytoSender.name = warehouseInfo.Contractor;
                            ytoSender.prov = warehouseInfo.ProvinceCity;
                            ytoSender.city = warehouseInfo.Str1 + "," + warehouseInfo.Str2;
                            ytoSender.address = warehouseInfo.Address;
                            numberInfo.sender = ytoSender;


                            //收件人信息
                            YtoReceiver ytoReceiver = new YtoReceiver();
                            ytoReceiver.phone = orderInfo.Contact;
                            ytoReceiver.name = orderInfo.Consignee;
                            ytoReceiver.prov = orderInfo.Province;
                            ytoReceiver.city = orderInfo.City + "," + orderInfo.District;
                            ytoReceiver.address = orderInfo.Address;
                            numberInfo.receiver = ytoReceiver;


                            numberInfo.clientID = clientId;
                            numberInfo.logisticProviderID = "YTO";
                            numberInfo.customerId = clientId;
                            numberInfo.txLogisticID = "YTO" + orderInfo.PackageNumber;
                            numberInfo.tradeNo = clientId;
                            numberInfo.orderType = orderTypeint("普通订单");
                            numberInfo.serviceType = serviceTypeint("上门揽收");
                            numberInfo.itemName = string.Join(",", packageDetailInfos.Select(a => a.GoodsName));
                            numberInfo.number = (int)packageDetailInfos.Sum(a => a.Qty).Value;

                            //消息内容
                            string logistics_interface = XmlSerializeHelper.XmlSerialize(numberInfo, Encoding.UTF8);
                            //消息签名
                            string data_digest = System.Web.HttpUtility.UrlEncode(base64(orderMD5(logistics_interface + partnerId)));

                            //订单类型（online:在线下单，offline:线下下单）
                            string type = "offline";

                            string strUrl = ConfigHelper.GetConfigValue("YtoURL") + "logistics_interface=" + System.Web.HttpUtility.UrlEncode(logistics_interface) + "&data_digest=" + data_digest + "&type=" + type + "&clientId=" + clientId;

                            //string dataStr = PostPage(strUrl, "");
                            string dataStr = string.Empty;
                            try
                            {
                                dataStr = PostPage(strUrl, "");
                            }
                            catch (Exception ex)
                            {
                                msg.Append("箱号：" + apiRequest.PackageNumber + "获取快递失败，" + ex.Message);
                            }
                            var data = XmlSerializeHelper.DESerializer<Runbow.TWS.Entity.WebApi.Express.Ytofeedback>(dataStr);
                            if (data.code == "200")
                            {
                                YtoRequest ytoRequest = new YtoRequest();
                                var qrCode = JSONString<qrCode>(data.qrCode);
                                ytoRequest.expressDelivery = new ExpressDelivery();
                                ytoRequest.expressDelivery.CustomerID = response.Result.orderInfo.CustomerID.Value;
                                ytoRequest.expressDelivery.CustomerName = response.Result.orderInfo.CustomerName;
                                ytoRequest.expressDelivery.WarehouseID = response.Result.orderInfo.WarehouseID;
                                ytoRequest.expressDelivery.WarehouseName = response.Result.orderInfo.Warehouse;
                                ytoRequest.expressDelivery.OID = response.Result.orderInfo.ID;
                                ytoRequest.expressDelivery.OrderNumber = response.Result.orderInfo.OrderNumber;
                                ytoRequest.expressDelivery.ExternOrderNumber = response.Result.orderInfo.ExternOrderNumber;
                                ytoRequest.expressDelivery.ExpressNumber = data.txLogisticID;
                                ytoRequest.expressDelivery.ExpressCompany = "圆通";
                                ytoRequest.expressDelivery.PackageNumber = apiRequest.PackageNumber;
                                ytoRequest.expressDelivery.Status = "0";
                                ytoRequest.expressDelivery.success = data.success;
                                ytoRequest.expressDelivery.code = data.code;
                                ytoRequest.expressDelivery.logisticProviderID = data.logisticProviderID;
                                ytoRequest.expressDelivery.txLogisticID = data.txLogisticID;
                                ytoRequest.expressDelivery.mailNo = data.mailNo;
                                ytoRequest.expressDelivery.originateOrgCode = data.originateOrgCode;
                                //ytoRequest.expressDelivery.reason = data.reason;
                                ytoRequest.expressDelivery.consigneeBranchCode = data.distributeInfo.consigneeBranchCode;
                                ytoRequest.expressDelivery.packageCenterCode = data.distributeInfo.packageCenterCode;
                                ytoRequest.expressDelivery.packageCenterName = data.distributeInfo.packageCenterName;
                                ytoRequest.expressDelivery.printKeyWord = data.distributeInfo.printKeyWord;
                                ytoRequest.expressDelivery.shortAddress = data.distributeInfo.shortAddress;
                                ytoRequest.expressDelivery.mn = qrCode.mn;
                                ytoRequest.expressDelivery.pcn = qrCode.pcn;
                                ytoRequest.expressDelivery.rbc = qrCode.rbc;
                                ytoRequest.expressDelivery.sbc = qrCode.sbc;
                                ytoRequest.expressDelivery.ssc = qrCode.ssc;
                                ytoRequest.expressDelivery.tsc = qrCode.tsc;
                                ytoRequest.expressDelivery.Creator = "圆通";
                                ytoRequest.expressDelivery.CreateTime = DateTime.Now.ToString();
                                //YtoResponse ytoResponse = new YtoResponse();
                                //ytoResponse.Response = new Runbow.TWS.Entity.WebApi.Express.Ytofeedback();
                                var InsExpressNumdata = service.InsExpressNumYto(ytoRequest);
                            }
                            else
                            {
                                msg.Append("箱号：" + apiRequest.PackageNumber + "快递信息获取失败");
                            }
                        }

                        if (!string.IsNullOrEmpty(msg.ToString()))
                        {
                            apiResponse.result = "false";
                            apiResponse.resultCode = "203";
                            apiResponse.reason = msg.ToString();
                        }
                        else
                        {
                            apiResponse.result = "true";
                            apiResponse.resultCode = "1000";
                            apiResponse.reason = "";
                        }
                    }
                    else
                    {
                        apiResponse.result = "true";
                        apiResponse.resultCode = "202";
                        apiResponse.reason = "订单包装已全部绑定快递单号";
                    }
                }
                else
                {
                    apiResponse.result = "false";
                    apiResponse.resultCode = "201";
                    apiResponse.reason = "未获取到订单和包装信息";
                }
            }
            catch (Exception ex)
            {
                apiResponse.result = "false";
                apiResponse.resultCode = "205";
                apiResponse.reason = "获取快递信息失败：" + ex.Message;

            }
            return apiResponse;
        }

        #region MyRegion
        /// <summary>
        /// 德邦快递
        /// 下单服务接口
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public HttpResponseMessage GetExpressNumByDeppon([FromBody]ApiRequest apiRequest)
        //{
        //    ///月结 有子母件 大件快递360
        //    ExpressService service = new ExpressService();
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    string url = ExpressConstants.dpCreateOrderUrl;
        //    string appKey = ExpressConstants.dpAppKey;
        //    string companyCode = ExpressConstants.dbCompanyCode;
        //    string sign = ExpressConstants.dpSign;
        //    string customerCode = ExpressConstants.jtCustomerCode;//测试用219401,219402

        //    LogHelper.SetPreFilePath = "Deppon";
        //    DPCreateOrderRequest request = new DPCreateOrderRequest();//订单信息
        //    ExpressResponse res = service.GetOrder(apiRequest.PackageNumber, apiRequest.OrderType).Result;
        //    if (res.order != null)
        //    {
        //        if (res.packages != null && res.packages.Any())
        //        {
        //            LogHelper.Info("获取订单以及包装信息");
        //            request.logisticID = sign + res.order.OrderNumber.Substring(2);
        //            request.custOrderNo = res.order.OrderNumber;
        //            request.companyCode = companyCode;
        //            request.orderType = "2";
        //            request.transportType = "RCP";//大件快递360
        //            request.customerCode = customerCode;
        //            request.sender = new Sender()
        //            {
        //                name = res.warehouse.Contractor,
        //                mobile = res.warehouse.Mobile,
        //                province = "陕西省",
        //                city = "西安市",
        //                county = res.warehouse.ProvinceCity,
        //                address = res.warehouse.Address
        //            };
        //            request.receiver = new Receiver()
        //            {
        //                name = res.order.Consignee,
        //                mobile = res.order.Contact,
        //                province = res.order.Province,
        //                city = res.order.City,
        //                county = res.order.District,
        //                address = res.order.Address
        //            };
        //            request.packageInfo = new Runbow.TWS.MessageContracts.WebApi.Express.PackageInfo()
        //            {
        //                cargoName = "货物名",
        //                totalNumber = apiRequest.OrderType == "A" ? res.packages.Count() : 1,
        //                totalWeight = Convert.ToDouble(res.packages.Sum(i => i.NetWeight.AsDecimal())),
        //                deliveryType = "9"
        //            };
        //            request.gmtCommit = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //            request.payType = "2";

        //            string timeStamp = getTimeStamp();
        //            LogHelper.Info("timeStamp时间戳：" + timeStamp);
        //            string _params = js.Serialize(request);
        //            LogHelper.Info("params请求参数：" + _params);
        //            string digest = getDigest(_params + appKey + timeStamp);
        //            LogHelper.Info("digest密文摘要：" + digest);

        //            StringBuilder sb = new StringBuilder();
        //            sb.Append("params=" + _params);
        //            sb.Append("&digest=" + digest);
        //            sb.Append("&timestamp=" + timeStamp);
        //            sb.Append("&companyCode=" + companyCode);
        //            string result = string.Empty;
        //            try
        //            {
        //                result = this.Post(url, sb.ToString());
        //                LogHelper.Info("返回结果：" + result);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Info("返回失败，错误：" + ex.Message);
        //            }

        //            DPCreateOrderResponse response = null;
        //            try
        //            {
        //                response = js.Deserialize<DPCreateOrderResponse>(result);

        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Info("反序列化失败，错误：" + ex.Message);
        //            }

        //            if (response != null && response.result == "true")
        //            {
        //                if (apiRequest.OrderType == "A")
        //                {
        //                    var mails = response.mailNo.Split(',');
        //                    res.packages.Each((i, item) =>
        //                    {
        //                        item.ExpressNumber = mails[i];
        //                    });
        //                }
        //                else
        //                {
        //                    res.packages.ForEach(a => a.ExpressNumber = response.mailNo);
        //                }
        //                ///插入快递表WMS_ExpressDelivery，更新包装主表WMS_Package
        //                service.AddExpressAndUpdatePackage(response, res.packages);
        //            }
        //            else
        //            {
        //                LogHelper.Info("获取快递失败，错误：" + result);
        //            }

        //            return GetJsonData(response);
        //        }
        //        else
        //        {
        //            return GetJsonData(new { result = "false", resultCode = "202", reason = "订单包装已全部绑定快递单号" });
        //        }
        //    }
        //    return GetJsonData(new { result = "false", resultCode = "201", reason = "获取订单及包装信息失败" });
        //}


        ///// <summary>
        ///// 韵达快递
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public HttpResponseMessage GetExpressNumByYD([FromBody]ApiRequest apiRequest)
        //{
        //    string partnerID = ExpressConstants.ydPartnerID;//客户ID
        //    string password = ExpressConstants.ydPassword;//接口联调密码
        //    string ydUrl = ExpressConstants.ydUrl;//接口地址

        //    LogHelper.SetPreFilePath = "YD";
        //    ExpressService service = new ExpressService();
        //    var response = service.GetOrderByYD(apiRequest.PackageNumber);
        //    if (response.IsSuccess)
        //    {
        //        LogHelper.Info("获取订单信息及包装明细");
        //        OrderInfo orderInfo = response.Result.order;
        //        IEnumerable<PackageDetailInfo> packageDetailInfos = response.Result.packageDetailInfos;
        //        WarehouseInfo warehouseInfo = response.Result.warehouse;

        //        YDRequest request = new YDRequest();
        //        request.orders = new List<YdRequestParam>();
        //        YdRequestParam order = new YdRequestParam()
        //        {
        //            order_serial_no = orderInfo.OrderNumber,
        //            khddh = orderInfo.OrderNumber,
        //            order_type = "common",
        //            //weight = 11,
        //            //size = "20,20,20",
        //            //value = 20,
        //            //node_id = "",
        //            //remark = "",
        //            //cus_area1 = "订单号：123123  \n 批次号：152201 "
        //        };
        //        order.sender = new Runbow.TWS.Entity.WebApi.Express.User()
        //        {
        //            name = warehouseInfo.WarehouseName,
        //            company = "虹迪",
        //            city = "上海市,上海市,金山区",
        //            address = "上海市,上海市,金山区" + warehouseInfo.Address,
        //            mobile = warehouseInfo.Mobile
        //        };
        //        order.receiver = new Runbow.TWS.Entity.WebApi.Express.User()
        //        {
        //            name = orderInfo.Consignee,
        //            company = "",
        //            city = orderInfo.Province + "," + orderInfo.City + "," + orderInfo.District,
        //            address = orderInfo.Address,
        //            mobile = orderInfo.Contact
        //        };
        //        order.items = new SkuItem();
        //        order.items.item = new List<Sku>();
        //        foreach (var pd in packageDetailInfos)
        //        {
        //            order.items.item.Add(new Sku()
        //            {
        //                name = pd.GoodsName,
        //                number = Convert.ToInt32(pd.Qty.Value),
        //                remark = ""
        //            });
        //        }
        //        request.orders.Add(order);
        //        var xmlData = CommonHelper.XmlSerialize(request, Encoding.UTF8);//请求订单数据
        //        var xmlDataToBase64 = CommonHelper.ToBASE64(xmlData);//BASE64 编码

        //        //签名 md5(xmldata + partnerid + 密码),xmldata需进行BASE64编码
        //        var validation = CommonHelper.ToMD5(xmlDataToBase64 + partnerID + password);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("partnerid=" + ExpressConstants.ydPartnerID);
        //        sb.Append("&version=" + ExpressConstants.ydVersion);
        //        sb.Append("&request=generalOrderApi");
        //        sb.Append("&xmldata=" + CommonHelper.UrlEncode(xmlDataToBase64));
        //        sb.Append("&validation=" + validation);

        //        string result = string.Empty;
        //        try
        //        {
        //            result = CommonHelper.HttpPost(ydUrl, sb.ToString());
        //            LogHelper.Info("韵达接口反馈：" + result);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.Info("韵达接口反馈失败，错误：" + ex.Message);
        //        }

        //        var data = CommonHelper.XmlDeserialize<YDResponse>(result);

        //        YdResponseParam responseParam = new YdResponseParam();
        //        if (data != null)
        //        {
        //            responseParam = data.responses.FirstOrDefault();
        //            var jsonData = CommonHelper.UnicodeToString(responseParam.pdf_info);//Unicode转中文
        //            LogHelper.Info("jsonData：" + jsonData);
        //            var dataList = new List<PdfInfoObj>();
        //            if (!string.IsNullOrEmpty(jsonData))
        //            {
        //                //json字符串切分
        //                var jsonResult = jsonData.Substring(1, jsonData.IndexOf(",["));
        //                jsonResult = jsonResult.Substring(0, jsonResult.Length - 1) + "]";
        //                LogHelper.Info("反序列化字符串：" + jsonResult);

        //                dataList = JsonConvert.DeserializeObject<List<PdfInfoObj>>(jsonResult);
        //            }
        //            PdfInfoObj pdfInfoObj = dataList.Count() == 0 ? new PdfInfoObj() : dataList.First();

        //            try
        //            {
        //                var responseResult = service.AddExpressAndUpdatePackageYD(responseParam, response.Result.packages.FirstOrDefault(), pdfInfoObj);
        //                if (responseResult.IsSuccess)
        //                {
        //                    return GetJsonData(new { code = 200, msg = "" });
        //                }
        //                else
        //                {
        //                    return GetJsonData(new { code = 201, msg = responseResult.Result });
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return GetJsonData(new { code = 205, msg = "错误：" + ex.Message });
        //            }
        //        }
        //    }
        //    return GetJsonData("");
        //}



        ///// <summary>
        ///// 圆通快递
        ///// </summary>
        ///// <returns></returns>
        ////[HttpPost]
        //[HttpPost]
        //public HttpResponseMessage GetExpressNumYto([FromBody]ApiRequest apiRequest)
        //{
        //    try
        //    {

        //        //IDs = "TRB20091810010360040";
        //        ExpressService service = new ExpressService();

        //        //客户编码（电商标识）
        //        string clientId = ConfigHelper.GetConfigValue("YtoClientID");
        //        //签名key
        //        string partnerId = ConfigHelper.GetConfigValue("YtoPartnerID");

        //        //string Ytourl = ConfigHelper.GetConfigValue("YtoURL");


        //        YtoExpressrNumberInfo numberInfo = new YtoExpressrNumberInfo();
        //        var response = service.GetExpressNumYto(apiRequest.PackageNumber);
        //        if (response.IsSuccess)
        //        {
        //            OrderInfo orderInfo = response.Result.orderInfo;
        //            List<OrderDetailInfo> orderDetailInfos = response.Result.orderDetailInfos;
        //            WarehouseInfo warehouseInfo = response.Result.warehouseInfo;
        //            //发件人信息
        //            YtoSender ytoSender = new YtoSender();
        //            ytoSender.phone = warehouseInfo.Mobile;
        //            ytoSender.name = warehouseInfo.Contractor;
        //            ytoSender.prov = warehouseInfo.ProvinceCity;
        //            ytoSender.city = warehouseInfo.Str1 + "," + warehouseInfo.Str2;
        //            ytoSender.address = warehouseInfo.Address;
        //            numberInfo.sender = ytoSender;


        //            //收件人信息
        //            YtoReceiver ytoReceiver = new YtoReceiver();
        //            ytoReceiver.phone = orderInfo.Contact;
        //            ytoReceiver.name = orderInfo.Consignee;
        //            ytoReceiver.prov = orderInfo.Province;
        //            ytoReceiver.city = orderInfo.City + "," + orderInfo.District;
        //            ytoReceiver.address = orderInfo.Address;
        //            numberInfo.receiver = ytoReceiver;


        //            numberInfo.clientID = clientId;
        //            numberInfo.logisticProviderID = "YTO";
        //            numberInfo.customerId = clientId;
        //            numberInfo.txLogisticID = "YTO" + orderInfo.PackageNumber;
        //            numberInfo.tradeNo = clientId;
        //            numberInfo.orderType = orderTypeint("普通订单");
        //            numberInfo.serviceType = serviceTypeint("上门揽收");
        //            numberInfo.itemName = string.Join(",", orderDetailInfos.Select(a => a.GoodsName));
        //            numberInfo.number = (int)orderDetailInfos.Sum(a => a.Qty).Value;

        //            //消息内容
        //            string logistics_interface = XmlSerializeHelper.XmlSerialize(numberInfo, Encoding.UTF8);
        //            //消息签名
        //            string data_digest = System.Web.HttpUtility.UrlEncode(base64(orderMD5(logistics_interface + partnerId)));

        //            //订单类型（online:在线下单，offline:线下下单）
        //            string type = "offline";

        //            string strUrl = ConfigHelper.GetConfigValue("YtoURL") + "logistics_interface=" + System.Web.HttpUtility.UrlEncode(logistics_interface) + "&data_digest=" + data_digest + "&type=" + type + "&clientId=" + clientId;

        //            string dataStr = PostPage(strUrl, "");
        //            var data = XmlSerializeHelper.DESerializer<Runbow.TWS.Entity.WebApi.Express.Ytofeedback>(dataStr);
        //            if (data.code == "200")
        //            {
        //                YtoRequest ytoRequest = new YtoRequest();
        //                var qrCode = JSONString<qrCode>(data.qrCode);
        //                ytoRequest.expressDelivery = new ExpressDelivery();
        //                ytoRequest.expressDelivery.CustomerID = response.Result.orderInfo.CustomerID.Value;
        //                ytoRequest.expressDelivery.CustomerName = response.Result.orderInfo.CustomerName;
        //                ytoRequest.expressDelivery.WarehouseID = response.Result.orderInfo.WarehouseID;
        //                ytoRequest.expressDelivery.WarehouseName = response.Result.orderInfo.Warehouse;
        //                ytoRequest.expressDelivery.OID = response.Result.orderInfo.ID;
        //                ytoRequest.expressDelivery.OrderNumber = response.Result.orderInfo.OrderNumber;
        //                ytoRequest.expressDelivery.ExternOrderNumber = response.Result.orderInfo.ExternOrderNumber;
        //                ytoRequest.expressDelivery.ExpressNumber = data.txLogisticID;
        //                ytoRequest.expressDelivery.ExpressCompany = "圆通";
        //                ytoRequest.expressDelivery.PackageNumber = apiRequest.PackageNumber;
        //                ytoRequest.expressDelivery.Status = "0";
        //                ytoRequest.expressDelivery.success = data.success;
        //                ytoRequest.expressDelivery.code = data.code;
        //                ytoRequest.expressDelivery.logisticProviderID = data.logisticProviderID;
        //                ytoRequest.expressDelivery.txLogisticID = data.txLogisticID;
        //                ytoRequest.expressDelivery.mailNo = data.mailNo;
        //                ytoRequest.expressDelivery.originateOrgCode = data.originateOrgCode;
        //                //ytoRequest.expressDelivery.reason = data.reason;
        //                ytoRequest.expressDelivery.consigneeBranchCode = data.distributeInfo.consigneeBranchCode;
        //                ytoRequest.expressDelivery.packageCenterCode = data.distributeInfo.packageCenterCode;
        //                ytoRequest.expressDelivery.packageCenterName = data.distributeInfo.packageCenterName;
        //                ytoRequest.expressDelivery.printKeyWord = data.distributeInfo.printKeyWord;
        //                ytoRequest.expressDelivery.shortAddress = data.distributeInfo.shortAddress;
        //                ytoRequest.expressDelivery.mn = qrCode.mn;
        //                ytoRequest.expressDelivery.pcn = qrCode.pcn;
        //                ytoRequest.expressDelivery.rbc = qrCode.rbc;
        //                ytoRequest.expressDelivery.sbc = qrCode.sbc;
        //                ytoRequest.expressDelivery.ssc = qrCode.ssc;
        //                ytoRequest.expressDelivery.tsc = qrCode.tsc;
        //                ytoRequest.expressDelivery.Creator = "圆通";
        //                ytoRequest.expressDelivery.CreateTime = DateTime.Now.ToString();
        //                //YtoResponse ytoResponse = new YtoResponse();
        //                //ytoResponse.Response = new Runbow.TWS.Entity.WebApi.Express.Ytofeedback();
        //                var InsExpressNumdata = service.InsExpressNumYto(ytoRequest);
        //                return GetJsonData(new { code = 200, msg = "" });
        //            }
        //            else
        //            {
        //                return GetJsonData(new { code = 202, msg = "快递信息获取失败" });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return GetJsonData(new { code = 205, msg = ex.Message });
        //    }
        //    return GetJsonData(new { code = 201, msg = "未获取到订单和包装信息" });
        //}
        #endregion





        /// <summary>
        /// 订单类型
        /// </summary>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        private int orderTypeint(string OrderType)
        {
            switch (OrderType)
            {
                case "COD":
                    return 0;
                case "普通订单":
                    return 1;
                case "便携式订单":
                    return 2;
                case "退货单":
                    return 3;
                case "到付":
                    return 4;
                default:
                    return 1;
            }

        }
        /// <summary>
        /// 服务类型
        /// </summary>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        private int serviceTypeint(string serviceType)
        {
            switch (serviceType)
            {
                case "自己联系":
                    return 0;
                case "上门揽收":
                    return 1;
                //case "便携式订单":
                //    return 2;
                //case "退货单":
                //    return 3;
                //case "到付":
                //    return 4;
                default:
                    return 0;
            }

        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encode(string str, string key)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(str);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            stream.Close();
            return builder.ToString();
        }
        #region URL编码
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str">要进行编码的字符串</param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
        #endregion

        /// <summary>
        /// 德邦密文digest
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string getDigest(string plainText)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] dataHash = md5.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in dataHash)
            {
                sb.Append(b.ToString("x2").ToLower());//小写的16进制
            }
            return Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes(sb.ToString()));
        }

        /// <summary>
        /// 圆通密文digest
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string getDigestYto(string plainText)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] dataHash = md5.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in dataHash)
            {
                sb.Append(b.ToString("x2"));
            }
            return Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes(sb.ToString().Substring(8, 16)));
        }
        //订单接口MD5加密
        public byte[] orderMD5(String str)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] date = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                //Console.WriteLine(BitConverter.ToString(date));
                //String s = "{";
                //for (int i = 0; i < date.Length; i++)
                //{
                //    s += (date[i]+",");
                //}
                //s += "}";
                //this.txLogisticID.Text = s;
                //return BitConverter.ToString(date);
                return date;
            }
        }
        public static T JSONString<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            return Serializer.Deserialize<T>(JsonStr);
        }
        public String base64(byte[] date)
        {
            String result = null;
            result = Convert.ToBase64String(date);
            return result;
        }
        /// <summary>
        /// 获取时间戳，当前时间毫秒数
        /// </summary>
        /// <returns></returns>
        public string getTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string times = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return times;
        }

        /// <summary>
        /// 返回json数据格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private HttpResponseMessage GetJsonData(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string str = serializer.Serialize(obj);
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, System.Text.Encoding.GetEncoding("UTF-8"), "application/json") };

            return result;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string Post(string url, string postDataStr = "")
        {
            string resStr = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            if (!string.IsNullOrEmpty(postDataStr))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postDataStr);
                req.ContentLength = bytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse res = null;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            if (res != null)
            {
                Stream responseStream = res.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream, Encoding.UTF8))
                {
                    resStr = sr.ReadToEnd();
                    sr.Close();
                }
                responseStream.Close();
            }
            res.Close();
            req.Abort();

            return resStr;
        }

        /// <summary>
        /// 请求API
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private static string PostPage(string posturl, string postData)
        {
            try
            {
                string _url = posturl;
                //json参数
                string jsonParam = postData;
                var request = (HttpWebRequest)WebRequest.Create(_url);
                request.Timeout = 5000;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                byte[] byteData = Encoding.UTF8.GetBytes(jsonParam);
                int length = byteData.Length;
                request.ContentLength = length;
                Stream writer = request.GetRequestStream();
                writer.Write(byteData, 0, length);
                writer.Close();
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

    }
}
