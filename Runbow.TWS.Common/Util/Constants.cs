using System;

namespace Runbow.TWS.Common
{
    public class Constants
    {
        public static readonly string Excel03ConString = ConfigHelper.GetConnectionString("Excel03ConString");
        public static readonly string Excel07ConString = ConfigHelper.GetConnectionString("Excel07ConString");
        public static readonly Lazy<int> PageSize = new Lazy<int>(() => ConfigHelper.GetConfigValueToInt("PageSize", 20).Value);

        public static readonly int PAGESIZE = ConfigHelper.GetConfigValue("PageSize").ObjectToInt32();
        public static readonly int ASNQueryPageSize = ConfigHelper.GetConfigValue("ASNQueryPageSize").ObjectToInt32();
        public static readonly int SplitOrderSize = ConfigHelper.GetConfigValue("SplitOrderSize").ObjectToInt32();
        public static readonly string TEMPFOLDER = "TEMP";
        public static readonly string UPLOAD_FOLDER_PATH = ConfigHelper.GetConfigValue("UploadFolderPath");
        public static readonly string DownLoadFiles = ConfigHelper.GetConfigValue("DownLoadFiles");
        public static readonly string DownLoadFilesTEMP = ConfigHelper.GetConfigValue("DownLoadFilesTEMP");
        public static readonly string UPLOAD_AMS_PATH = ConfigHelper.GetConfigValue("UploadAMSPath");
        public static readonly string Audit_ReplyDocument_FOLDER_PATH = ConfigHelper.GetConfigValue("AuditReplyDocumentFolderPath");
        public static readonly string Audit_ReplyDocument_Picture_Url_Pre = ConfigHelper.GetConfigValue("AuditReplyDocumentPictureUrlPre");
        public static readonly string FtpServerIP = ConfigHelper.GetConfigValue("FtpServerIP");
        public static readonly string FtpRemotePath = ConfigHelper.GetConfigValue("FtpRemotePath");
        public static readonly string FtpUserID = ConfigHelper.GetConfigValue("FtpUserID");
        public static readonly string FtpPassword = ConfigHelper.GetConfigValue("FtpPassword");
        public static readonly string JCSendAPIAddress = ConfigHelper.GetConfigValue("JCSendAPIAddress");
        public static readonly string JiteAPIAddress = ConfigHelper.GetConfigValue("JiteAPIAddress");
        public static readonly string YtoAPIAddress = ConfigHelper.GetConfigValue("YtoAPIAddress");
        public static readonly string YdAPIAddress = ConfigHelper.GetConfigValue("YdAPIAddress");
        public static readonly string ExpressApiAddress = ConfigHelper.GetConfigValue("expressApiAddress");
    }

    public class RedisConstants
    {
        public static readonly string RedisPath = ConfigHelper.GetConfigValue("RedisPath");
        public static readonly string RedisPrefix = ConfigHelper.GetConfigValue("RedisPrefix");
    }

    public class WXAppConstants
    {
        public static readonly string WXUrl = ConfigHelper.GetConfigValue("wxurl");
        public static readonly string WXAppKey = ConfigHelper.GetConfigValue("wxappkey");
        public static readonly string WXAppSecret = ConfigHelper.GetConfigValue("wxappsecret");

        /// <summary>
        /// 当前商家的订单列表 请求地址
        /// </summary>
        public static readonly string soldtradesUrl = "/TigerShop.ITrade.GetSoldTrades/";
        /// <summary>
        /// 订单的增量交易数据 请求地址
        /// </summary>
        public static readonly string incrementsoldtradesUrl = "/TigerShop.ITrade.GetIncrementSoldTrades/";
        /// <summary>
        /// 单笔交易的详细信息 请求地址
        /// </summary>
        public static readonly string tradeUrl = "/TigerShop.ITrade.GetTrade/";
        /// <summary>
        /// 当前商家的商品列表 请求地址
        /// </summary>
        public static readonly string soldproductsUrl = "/TigerShop.IProduct.GetSoldProducts/";
        /// <summary>
        /// 指定商品的详细信息 请求地址
        /// </summary>
        public static readonly string productUrl = "/TigerShop.IProduct.GetProduct/";
        /// <summary>
        /// 售后列表 请求地址
        /// </summary>
        public static readonly string soldrefundsUrl = "/TigerShop.IRefund.GetSoldRefunds/";
        /// <summary>
        /// 单笔退货的详细信息 请求地址
        /// </summary>
        public static readonly string refundretailUrl = "/TigerShop.IRefund.GetRefundDetail/";
    }

    public class ExpressConstants
    {
        /// <summary>
        /// 德邦Url
        /// </summary>
        public static readonly string dpCreateOrderUrl = ConfigHelper.GetConfigValue("dpCreateOrderUrl");
        /// <summary>
        /// 德邦Appkey
        /// </summary>
        public static readonly string dpAppKey = ConfigHelper.GetConfigValue("dpAppkey");
        /// <summary>
        /// 德邦CompanyCode
        /// </summary>
        public static readonly string dbCompanyCode = ConfigHelper.GetConfigValue("dpCompanyCode");
        /// <summary>
        /// 德邦Sign
        /// </summary>
        public static readonly string dpSign = ConfigHelper.GetConfigValue("dpSign");
        /// <summary>
        /// 吉特专用CustomerCode
        /// </summary>
        public static readonly string jtCustomerCode = ConfigHelper.GetConfigValue("jtCustomerCode");

        /// <summary>
        /// 韵达 合作商 ID，由韵达提供给大客户
        /// </summary>
        public static readonly string ydPartnerID = ConfigHelper.GetConfigValue("ydPartnerID");
        /// <summary>
        /// 韵达 密码
        /// </summary>
        public static readonly string ydPassword = ConfigHelper.GetConfigValue("ydPassword");
        /// <summary>
        /// 韵达 请求的版本，当前为 1.0
        /// </summary>
        public static readonly string ydVersion = ConfigHelper.GetConfigValue("ydVersion");
        /// <summary>
        /// 韵达 订单创建Url
        /// </summary>
        public static readonly string ydUrl = ConfigHelper.GetConfigValue("ydUrl");


    }
}