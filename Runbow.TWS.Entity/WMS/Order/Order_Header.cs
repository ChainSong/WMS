using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    /// <summary>
    /// nike接口订单头
    /// </summary>
    public class Order_Header
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

        [EntityPropertyExtension("batchId", "batchId")]
        public string batchId { get; set; }

        [EntityPropertyExtension("appointStcode", "appointStcode")]
        public string appointStcode { get; set; }

        [EntityPropertyExtension("appointStName", "appointStName")]
        public string appointStName { get; set; }

        [EntityPropertyExtension("platType", "platType")]
        public string platType { get; set; }

        [EntityPropertyExtension("orderCode", "orderCode")]
        public string orderCode { get; set; }

        [EntityPropertyExtension("orderStore", "orderStore")]
        public string orderStore { get; set; }

        [EntityPropertyExtension("orderStatus", "orderStatus")]
        public int orderStatus { get; set; }

        [EntityPropertyExtension("pushTime", "pushTime")]
        public string pushTime { get; set; }

        [EntityPropertyExtension("expireTime", "expireTime")]
        public string expireTime { get; set; }

        [EntityPropertyExtension("platCreateTime", "platCreateTime")]
        public string platCreateTime { get; set; }

        [EntityPropertyExtension("paymentTime", "paymentTime")]
        public string paymentTime { get; set; }

        [EntityPropertyExtension("country", "country")]
        public string country { get; set; }

        [EntityPropertyExtension("province", "province")]
        public string province { get; set; }

        [EntityPropertyExtension("city", "city")]
        public string city { get; set; }

        [EntityPropertyExtension("district", "district")]
        public string district { get; set; }

        [EntityPropertyExtension("address", "address")]
        public string address { get; set; }

        [EntityPropertyExtension("postcode", "postcode")]
        public string postcode { get; set; }

        [EntityPropertyExtension("receiver", "receiver")]
        public string receiver { get; set; }

        [EntityPropertyExtension("receiverPhone", "receiverPhone")]
        public string receiverPhone { get; set; }

        [EntityPropertyExtension("receiverEmail", "receiverEmail")]
        public string receiverEmail { get; set; }

        [EntityPropertyExtension("extCode1", "extCode1")]
        public string extCode1 { get; set; }

        [EntityPropertyExtension("totalPrice", "totalPrice")]
        public string totalPrice { get; set; }

        [EntityPropertyExtension("totalQuantity", "totalQuantity")]
        public int totalQuantity { get; set; }

        [EntityPropertyExtension("type", "type")]
        public string type { get; set; }

        [EntityPropertyExtension("vasIndicator", "vasIndicator")]
        public int vasIndicator { get; set; }

        [EntityPropertyExtension("remark", "remark")]
        public string remark { get; set; }

        [EntityPropertyExtension("extProps", "extProps")]
        public string extProps { get; set; }
    }
}
