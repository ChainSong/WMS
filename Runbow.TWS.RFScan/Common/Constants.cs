using Runbow.TWS.Common;
using System.Text;
using System.Web;

namespace Runbow.TWS.RFScan.Common
{
    public class Constants
    {
        public const string PASSWORD = "123456";
        public const string PODSTATE = "PodState";
        public const string PODTYPE = "PODType";
        public const string SHIPPERTYPE = "ShipperType";
        public const string SHIPPERIDENTIFY = "ShipperIdentify";
        public const string TTLORTPL = "TtlOrTpl";
        public const string USER_INFO_KEY = "UserInfo";
        public const string INVOICETYPE = "InvoiceType";
        public const string QUOTEDPRICEPRE = "QuotedPrice";
        //public static readonly string RedisPath = ConfigHelper.GetConfigValue("RedisPath");
        //public static readonly string RedisPrefix = ConfigHelper.GetConfigValue("RedisPrefix");
        public static readonly string APPLICATIONCONFIGPATH = HttpContext.Current.Server.MapPath("~/App_Data/ApplicationConfig.xml");
        public static readonly string APPLICATIONFILEPATH = HttpContext.Current.Server.MapPath("~/App_Data/ApplicationFile.xml");

        public static string GenApplicationQuotedPriceKey(long projectID, int target, long targetID, long relatedCustomerID)
        {
            StringBuilder cacheQuotedPricesSB = new StringBuilder();
            cacheQuotedPricesSB.Append(QUOTEDPRICEPRE).Append("_")
                             .Append(projectID.ToString()).Append("_")
                             .Append(target.ToString()).Append("_")
                             .Append(targetID.ToString()).Append("_")
                             .Append(relatedCustomerID.ToString());
            return cacheQuotedPricesSB.ToString();
        }
    }
}