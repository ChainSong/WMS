using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.NikeOSRBJPrint
{
    public class PrintExpressJite
    {
        /// <summary>
        /// 系统订单号
        /// </summary>
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }
        /// <summary>
        /// 外部订单号
        /// </summary>
        [EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        public string ExternOrderNumber { get; set; }
        /// <summary>
        /// 大头笔
        /// </summary>
        [EntityPropertyExtension("PackageNumber", "PackageNumber")]
        public string PackageNumber { get; set; }
        /// <summary>
        /// 大头笔
        /// </summary>
        [EntityPropertyExtension("arrivedOrgSimpleName", "arrivedOrgSimpleName")]
        public string arrivedOrgSimpleName { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        [EntityPropertyExtension("Consignee", "Consignee")]
        public string Consignee { get; set; }
        /// <summary>
        /// 收货人联系方式
        /// </summary>
        [EntityPropertyExtension("Contact", "Contact")]
        public string Contact { get; set; }
        /// <summary>
        /// 收货人 省份
        /// </summary>
        [EntityPropertyExtension("Province", "Province")]
        public string Province { get; set; }
        /// <summary>
        /// 收货人 城市
        /// </summary>
        [EntityPropertyExtension("City", "City")]
        public string City { get; set; }
        /// <summary>
        /// 收货人 区
        /// </summary>
        [EntityPropertyExtension("District", "District")]
        public string District { get; set; }
        /// <summary>
        /// 收货人 详细地址
        /// </summary>
        [EntityPropertyExtension("Address", "Address")]
        public string Address { get; set; }
        /// <summary>
        /// 发货人
        /// </summary>
        [EntityPropertyExtension("senderContractor", "senderContractor")]
        public string senderContractor { get; set; }
        /// <summary>
        /// 发货人 联系方式
        /// </summary>
        [EntityPropertyExtension("senderMobile", "senderMobile")]
        public string senderMobile { get; set; }
        /// <summary>
        /// 发货人 省份城市区
        /// </summary>
        [EntityPropertyExtension("senderProvinceCity", "senderProvinceCity")]
        public string senderProvinceCity { get; set; }
        /// <summary>
        /// 发货人 详细地址
        /// </summary>
        [EntityPropertyExtension("senderAddress", "senderAddress")]
        public string senderAddress { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        [EntityPropertyExtension("ExpressNumber", "ExpressNumber")]
        public string ExpressNumber { get; set; }
        /// <summary>
        /// 母单号
        /// </summary>
        [EntityPropertyExtension("parentMailNo", "parentMailNo")]
        public string parentMailNo { get; set; }

    }
}
