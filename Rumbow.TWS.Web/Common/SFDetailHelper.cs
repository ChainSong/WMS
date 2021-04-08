using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;
using Runbow.TWS.Biz;
using System.Xml;
using System.Text;

namespace Runbow.TWS.Web.Common
{
    public class SFDetailHelper
    {
        public static IEnumerable<WMS_SFDetail> CreateSFOrder(RequestSFModel request)
        {
            //接口文档说明:http://qiao.sf-express.com/pages/developDoc/index.html?level2=460077
            #region 测试
            //is_gen_bill_no是否要求返回顺丰运单号：1为要求
            //orderid,玫琳凯订单号|j_company,寄件方公司名称|j_province,寄件方省份|j_city,寄件方城市|j_county,寄件人县/区
            //|j_contact,寄件方联系人|j_tel,寄件方电话|j_address,寄件方地址
            //parcel_quantity,包裹数(多个子母单返回多个快递单号)
            //custid顺丰月结卡号|customs_batchs报关批次|cargo货物描述(化妆品/保健品)
            //pay_method,付款方式（1:寄方付 2:收方付 3:第三方付）
            //cargo_total_weight,订单货物总重量(单位千克，精确到小数点后3位,必须>0)
            StringBuilder rq = new StringBuilder();
            rq.Clear();
            rq.Append("<Request service='OrderService' lang='zh-CN'>");
            rq.Append("<Head>SHHDWLKJ,g8SiYFm8SFg5d4OjqEe2SPu3cCkiOnJo</Head>");
            rq.Append("<Body>");
            rq.Append("<Order ");
            rq.Append("  orderid='" + request.orderid + "' ");
            rq.Append("  is_gen_bill_no='1' ");
            rq.Append("  j_company='" + request.j_company + "' ");
            rq.Append("  j_province='" + request.j_province + "'");
            rq.Append("  j_city='" + request.j_city + "'      ");
            rq.Append("  j_county='" + request.j_county + "' ");
            rq.Append("  j_contact='" + request.j_contact + "' ");
            rq.Append("  j_tel='" + request.j_tel + "' ");
            rq.Append("  j_address='" + request.j_address + "' ");
            rq.Append("  d_company='" + request.d_company + "' ");
            rq.Append("  d_province='" + request.d_province + "' ");
            rq.Append("  d_city='" + request.d_city + "' ");
            rq.Append("  d_county='" + request.d_county + "' ");            
            rq.Append("  d_contact='" + request.d_contact + "' ");
            rq.Append("  d_tel='" + request.d_tel + "' ");
            rq.Append("  d_address='" + request.d_address + "'  ");
            rq.Append("  parcel_quantity='" + request.parcel_quantity.Trim() + "' ");
            rq.Append("  pay_method='" + request.pay_method + "'  ");
            rq.Append("  custid ='" + request.custid + "'  ");
            rq.Append("  customs_batchs='" + request.customs_batchs + "' ");
            rq.Append("  cargo_total_weight='" + request.cargo_total_weight + "' ");
            //rq.Append("  express_type='2' ");
            rq.Append("  cargo='" + request.cargo + "' > ");
            rq.Append(" </Order> ");
            rq.Append("</Body> ");
            rq.Append("</Request> ");

            sfservice.ServiceClient sf11 = new sfservice.ServiceClient();

            try
            {
                string resxml = sf11.sfexpressService(rq.ToString());
                return AddSFDetail(request.OID, request.Creator, resxml);
            }
            catch
            {
                return null;
            }   
            
            #endregion
        }

        public static IEnumerable<WMS_SFDetail> AddSFDetail(long OID,string creator, string resxml)
        {
            string result = string.Empty;
            //string resxml = "<?xml version='1.0' encoding='UTF-8'?><Response service=\"OrderService\"><Head>OK</Head><Body><OrderResponse mailno=\"279124615041\" filter_result=\"2\" orderid=\"OR1082021030000002\" destcode=\"023\" origincode=\"028\"><rls_info invoke_result=\"OK\" rls_code=\"1000\" rls_errormsg=\"279124615041:\"><rls_detail waybillNo=\"279124615041\" sourceCityCode=\"028\" sourceDeptCode=\"028\" destCityCode=\"023\" destDeptCode=\"023SA\" destTeamCode=\"044\" destTransferCode=\"023W\" destRouteLabel=\"023SA-044\" proName=\"顺丰标快\" cargoTypeCode=\"T6\" limitTypeCode=\"T6\" expressTypeCode=\"B1\" codingMapping=\"F11\" xbFlag=\"0\" printFlag=\"000000000\" twoDimensionCode=\"MMM ={ 'k1':'023W','k2':'023SA','k3':'044','k4':'T6','k5':'279124615041','k6':'','k7':'f7985412'}\" proCode=\"T6\" printIcon=\"00000000\" checkCode=\"f7985412\" destGisDeptCode=\"023SA\" /></rls_info></OrderResponse></Body></Response>";

            //解析接口返回的xml
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resxml);
            //获取responses 下的所有response 节点
            XmlNodeList mylist = doc.SelectNodes("/Response/Head");
            string head = mylist[0].InnerText.Trim();
            if (head.ToUpper() == "OK")
            {
                List<WMS_SFDetail> sfDetailList = new List<WMS_SFDetail>();
                XmlNodeList mylist1 = doc.SelectNodes("/Response/Body/OrderResponse");
                if(mylist1 != null)
                {
                    string OrderNumber = mylist1[0].Attributes["orderid"].Value.Trim();//订单号
                    string waybillNo = mylist1[0].Attributes["mailno"].Value.Trim();//快递单号
                    XmlNodeList mydetail = doc.SelectNodes("/Response/Body/OrderResponse/rls_info/rls_detail");
                    if (mydetail != null)//attributeValue
                    {
                        WMS_SFDetail sFDetail = new WMS_SFDetail() {
                            ID = 0,
                            OID = OID, //订单ID
                            OrderNumber = OrderNumber,//订单号
                            waybillNo = waybillNo, //快递单号
                            sourceTransferCode = mydetail[0].Attributes["sourceTransferCode"] != null ? mydetail[0].Attributes["sourceTransferCode"].Value.Trim() : "", //原寄地中转场
                            sourceCityCode = mydetail[0].Attributes["sourceCityCode"] != null ? mydetail[0].Attributes["sourceCityCode"].Value.Trim() : "", //原寄地城市代码
                            sourceDeptCode = mydetail[0].Attributes["sourceDeptCode"] != null ? mydetail[0].Attributes["sourceDeptCode"].Value.Trim() : "", //
                            sourceTeamCode = mydetail[0].Attributes["sourceTeamCode"] != null ? mydetail[0].Attributes["sourceTeamCode"].Value.Trim() : "", //
                            destCityCode = mydetail[0].Attributes["destCityCode"] != null ? mydetail[0].Attributes["destCityCode"].Value.Trim() : "", //
                            destDeptCode = mydetail[0].Attributes["destDeptCode"] != null ? mydetail[0].Attributes["destDeptCode"].Value.Trim() : "", //
                            destDeptCodeMapping = mydetail[0].Attributes["destDeptCodeMapping"] != null ? mydetail[0].Attributes["destDeptCodeMapping"].Value.Trim() : "", //
                            destTeamCode = mydetail[0].Attributes["destTeamCode"] != null ? mydetail[0].Attributes["destTeamCode"].Value.Trim() : "", //
                            destTeamCodeMapping = mydetail[0].Attributes["destTeamCodeMapping"] != null ? mydetail[0].Attributes["destTeamCodeMapping"].Value.Trim() : "", //
                            destTransferCode = mydetail[0].Attributes["destTransferCode"] != null ? mydetail[0].Attributes["destTransferCode"].Value.Trim() : "", //
                            destRouteLabel = mydetail[0].Attributes["destRouteLabel"] != null ? mydetail[0].Attributes["destRouteLabel"].Value.Trim() : "", //
                            proName = mydetail[0].Attributes["proName"] != null ? mydetail[0].Attributes["proName"].Value.Trim() : "", //
                            cargoTypeCode = mydetail[0].Attributes["cargoTypeCode"] != null ? mydetail[0].Attributes["cargoTypeCode"].Value.Trim() : "", //
                            limitTypeCode = mydetail[0].Attributes["limitTypeCode"] != null ? mydetail[0].Attributes["limitTypeCode"].Value.Trim() : "", //
                            expressTypeCode = mydetail[0].Attributes["expressTypeCode"] != null ? mydetail[0].Attributes["expressTypeCode"].Value.Trim() : "", //
                            codingMapping = mydetail[0].Attributes["codingMapping"] != null ? mydetail[0].Attributes["codingMapping"].Value.Trim() : "", //
                            codingMappingOut = mydetail[0].Attributes["codingMappingOut"] != null ? mydetail[0].Attributes["codingMappingOut"].Value.Trim() : "", //
                            xbFlag = mydetail[0].Attributes["xbFlag"] != null ? mydetail[0].Attributes["xbFlag"].Value.Trim() : "", //
                            printFlag = mydetail[0].Attributes["printFlag"] != null ? mydetail[0].Attributes["printFlag"].Value.Trim() : "", //
                            twoDimensionCode = mydetail[0].Attributes["twoDimensionCode"] != null ? mydetail[0].Attributes["twoDimensionCode"].Value.Trim() : "", //
                            proCode = mydetail[0].Attributes["proCode"] != null ? mydetail[0].Attributes["proCode"].Value.Trim() : "", //
                            printIcon = mydetail[0].Attributes["printIcon"] != null ? mydetail[0].Attributes["printIcon"].Value.Trim() : "", //
                            abFlag = mydetail[0].Attributes["abFlag"] != null ? mydetail[0].Attributes["abFlag"].Value.Trim() : "", //
                            destPortCode = mydetail[0].Attributes["destPortCode"] != null ? mydetail[0].Attributes["destPortCode"].Value.Trim() : "", //
                            destCountry = mydetail[0].Attributes["destCountry"] != null ? mydetail[0].Attributes["destCountry"].Value.Trim() : "", //
                            destPostCode = mydetail[0].Attributes["destPostCode"] != null ? mydetail[0].Attributes["destPostCode"].Value.Trim() : "", //
                            goodsValueTotal = mydetail[0].Attributes["goodsValueTotal"] != null ? mydetail[0].Attributes["goodsValueTotal"].Value.Trim() : "", //
                            currencySymbol = mydetail[0].Attributes["currencySymbol"] != null ? mydetail[0].Attributes["currencySymbol"].Value.Trim() : "", //
                            goodsNumber = mydetail[0].Attributes["goodsNumber"] != null ? mydetail[0].Attributes["goodsNumber"].Value.Trim() : "", //
                            checkCode = mydetail[0].Attributes["checkCode"] != null ? mydetail[0].Attributes["checkCode"].Value.Trim() : "", //
                            str1 = "",
                            str2 = "",
                            str3 = "",
                            str4 = "",
                            str5 = "",
                            str6 = "",
                            str7 = "",
                            str8 = "",
                            str9 = "",
                            str10 = "",
                            Creator = creator,
                            CreateTime = DateTime.Now,
                        };
                        sfDetailList.Add(sFDetail);
                    }
                }

                if(sfDetailList != null && sfDetailList.Any())
                {
                   return  new AMSUploadService().AddWMS_SFDetail(sfDetailList);
                }
               
            }

            return null;
        }

    }


    /// <summary>
    /// 顺丰下单实体类
    /// </summary>
    public class RequestSFModel
    {
        //orderid,客户订单号|j_company,寄件方公司名称|j_province,寄件方省份|j_city,寄件方城市|j_county,寄件人县/区
        //|j_contact,寄件方联系人|j_tel,寄件方电话|j_address,寄件方地址
        //parcel_quantity,包裹数(多个子母单返回多个快递单号)
        //custid顺丰月结卡号|customs_batchs报关批次|cargo货物描述(化妆品/保健品)
        //pay_method,付款方式（1:寄方付 2:收方付 3:第三方付）
        //cargo_total_weight,订单货物总重量(单位千克，精确到小数点后3位,必须>0)

        /// <summary>
        /// 订单ID
        /// </summary>
        public long OID { get; set; }

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string orderid { get; set; }

        /// <summary>
        /// 寄件方公司名称
        /// </summary>
        public string j_company { get; set; }

        /// <summary>
        /// 寄件方省份
        /// </summary>
        public string j_province { get; set; }

        /// <summary>
        /// 寄件方城市
        /// </summary>
        public string j_city { get; set; }

        /// <summary>
        /// 寄件人县/区
        /// </summary>
        public string j_county { get; set; }

        /// <summary>
        /// 寄件方联系人
        /// </summary>
        public string j_contact { get; set; }

        /// <summary>
        /// 寄件方电话
        /// </summary>
        public string j_tel { get; set; }

        /// <summary>
        /// 寄件方地址
        /// </summary>
        public string j_address { get; set; }

        /////////////////

        /// <summary>
        /// 收件方公司名称
        /// </summary>
        public string d_company { get; set; }

        /// <summary>
        /// 收件方省份
        /// </summary>
        public string d_province { get; set; }

        /// <summary>
        /// 收件方城市
        /// </summary>
        public string d_city { get; set; }

        /// <summary>
        /// 收件人县/区
        /// </summary>
        public string d_county { get; set; }

        /// <summary>
        /// 收件方联系人
        /// </summary>
        public string d_contact { get; set; }

        /// <summary>
        /// 收件方电话
        /// </summary>
        public string d_tel { get; set; }

        /// <summary>
        /// 收件方地址
        /// </summary>
        public string d_address { get; set; }

        /// <summary>
        /// 订单货物总重量(单位千克，精确到小数点后3位,必须>0)
        /// </summary>
        public string cargo_total_weight { get; set; }

        /// <summary>
        /// 包裹数,默认1
        /// </summary>
        public string parcel_quantity { get; set; }

        /// <summary>
        /// 付款方式,默认1（1:寄方付 2:收方付 3:第三方付）
        /// </summary>
        public string pay_method { get; set; }

        /// <summary>
        /// 顺丰月结卡号，0286784041
        /// </summary>
        public string custid { get; set; }

        /// <summary>
        /// 货物描述(化妆品/保健品)
        /// </summary>
        public string cargo { get; set; }

        /// <summary>
        /// 报关批次，默认空
        /// </summary>
        public string customs_batchs { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        public string Creator { get; set; }

    }
}