using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    public class Order_Express
    {
        [EntityPropertyExtension("Id", "Id")]
        public string Id { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }
        [EntityPropertyExtension("CreatorId", "CreatorId")]
        public string CreatorId { get; set; }
        [EntityPropertyExtension("CreatorRealName", "CreatorRealName")]
        public string CreatorRealName { get; set; }
        [EntityPropertyExtension("Deleted", "Deleted")]
        public int Deleted { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [EntityPropertyExtension("orderCode", "orderCode")]
        public string orderCode { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        [EntityPropertyExtension("trackingNo", "trackingNo")]
        public string trackingNo { get; set; }

        /// <summary>
        /// 大头笔
        /// </summary>
        [EntityPropertyExtension("transBigWord", "transBigWord")]
        public string transBigWord { get; set; }
        [EntityPropertyExtension("logisticsCode", "logisticsCode")]
        public string logisticsCode { get; set; }
        /// <summary>
        /// 集包地
        /// </summary>
        [EntityPropertyExtension("packageCenterCode", "packageCenterCode")]
        public string packageCenterCode { get; set; }
        [EntityPropertyExtension("IsProcess", "IsProcess")]
        public int IsProcess { get; set; }


        /// <summary>
        /// 备用
        /// </summary>
        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }

        [EntityPropertyExtension("str2", "str2")]
        public string str2 { get; set; }

        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }

        [EntityPropertyExtension("str4", "str4")]
        public string str4 { get; set; }

        [EntityPropertyExtension("str5", "str5")]
        public string str5 { get; set; }
    }
}
