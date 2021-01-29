using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.YXDRBJPrint
{
    public class YXDRBJPrintPodInfo
    {
        /// <summary>
        /// 系统单号
        /// </summary>
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }
        /// <summary>
        /// 外部单号
        /// </summary>
        [EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        public string ExternOrderNumber { get; set; }
        /// <summary>
        /// 订单创建日期
        /// </summary>
        [EntityPropertyExtension("datetime1", "datetime1")]
        public string datetime1 { get; set; }
        /// <summary>
        /// 预计卸货时间
        /// </summary>
        [EntityPropertyExtension("Estimatdischargetime", "Estimatdischargetime")]
        public string Estimatdischargetime { get; set; }

        /// <summary>
        /// 仓库联系人
        /// </summary>
        [EntityPropertyExtension("Warehousecontact", "Warehousecontact")]
        public string Warehousecontact { get; set; }
        /// <summary>
        /// 仓库联系电话
        /// </summary>
        [EntityPropertyExtension("Warehousephone", "Warehousephone")]
        public string Warehousephone { get; set; }

        /// <summary>
        /// 发货方公司
        /// </summary>
        [EntityPropertyExtension("Warehousecompany", "Warehousecompany")]
        public string Warehousecompany { get; set; }
        /// <summary>
        /// 发货方地址
        /// </summary>
        [EntityPropertyExtension("Warehouseaddress", "Warehouseaddress")]
        public string Warehouseaddress { get; set; }
        /// <summary>
        /// 门店代码
        /// </summary>
        [EntityPropertyExtension("ShipToKey", "ShipToKey")]
        public string ShipToKey { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        [EntityPropertyExtension("Company", "Company")]
        public string Company { get; set; }

        /// <summary>
        /// 收货方联系人
        /// </summary>
        [EntityPropertyExtension("Contact1", "Contact1")]
        public string Contact1 { get; set; }

        /// <summary>
        /// 收货方联系电话
        /// </summary>
        [EntityPropertyExtension("Receivingcontact", "Receivingcontact")]
        public string Receivingcontact { get; set; }
        /// <summary>
        /// 收货方地址
        /// </summary>
        [EntityPropertyExtension("AddressLine1", "AddressLine1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// 货物名称
        /// </summary>
        [EntityPropertyExtension("goodsName", "goodsName")]
        public string goodsName { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        [EntityPropertyExtension("sumNum", "sumNum")]
        public int sumNum { get; set; }

        /// <summary>
        /// 总箱数
        /// </summary>
        [EntityPropertyExtension("sumBox", "sumBox")]
        public int sumBox { get; set; }

        /// <summary>
        ///净重
        /// </summary>
        [EntityPropertyExtension("NetWeight", "NetWeight")]
        public string NetWeight { get; set; }

        /// <summary>
        ///体积
        /// </summary>
        [EntityPropertyExtension("volume", "volume")]
        public double volume { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }
    }
}
