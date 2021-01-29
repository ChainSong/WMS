using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.DeliveryConfirm
{
    public class DeliverDetail
    {

        #region 交接单明细Model
        /// <summary>
        /// 交接明细ID
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long? ID { get; set; }

        /// <summary>
        /// 交接单号
        /// </summary>
        [EntityPropertyExtension("DeliverKey", "DeliverKey")]
        public string DeliverKey { get; set; }


        /// <summary>
        /// 出库单ID
        /// </summary>
        [EntityPropertyExtension("OID", "OID")]
        public long? OID { get; set; }


        /// <summary>
        /// 交接单ID
        /// </summary>
        [EntityPropertyExtension("DeliverID", "DeliverID")]
        public long? DeliverID { get; set; }

        /// <summary>
        /// 交接明细行号
        /// </summary>
        [EntityPropertyExtension("DeliverDetailKey", "DeliverDetailKey")]
        public string DeliverDetailKey { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }

        [EntityPropertyExtension("PackBoxKey", "PackBoxKey")]
        public string PackBoxKey { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        [EntityPropertyExtension("ExpressNumber", "ExpressNumber")]
        public string ExpressNumber { get; set; }

        /// <summary>
        /// 箱重
        /// </summary>
        [EntityPropertyExtension("BoxWeight", "BoxWeight")]
        public string BoxWeight { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public string Status { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long? WarehouseID { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }

        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        #endregion

        //备用字段
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

        [EntityPropertyExtension("str18", "str18")]
        public string str18 { get; set; }

        [EntityPropertyExtension("Int1", "Int1")]
        public int? Int1 { get; set; }

        [EntityPropertyExtension("Int2", "Int2")]
        public int? Int2 { get; set; }

        [EntityPropertyExtension("Int3", "Int3")]
        public int? Int3 { get; set; }

        [EntityPropertyExtension("Int4", "Int4")]
        public int? Int4 { get; set; }

        [EntityPropertyExtension("Int5", "Int5")]
        public int? Int5 { get; set; }


    }
}
