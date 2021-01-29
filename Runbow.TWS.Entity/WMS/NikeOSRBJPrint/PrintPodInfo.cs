using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.NikeOSRBJPrint
{
  public  class PrintPodInfo
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
        [EntityPropertyExtension("Receivingcontact", "Receivingcontact")]
        public string Receivingcontact { get; set; }
        /// <summary>
        /// 地址
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
        /// 仓库地址
        /// </summary>
        [EntityPropertyExtension("Warehouseaddress", "Warehouseaddress")]
        public string Warehouseaddress { get; set; }
        /// <summary>
        /// 总箱数
        /// </summary>
        [EntityPropertyExtension("sumBox", "sumBox")]
        public int sumBox { get; set; }
        /// <summary>
        /// 楼层   退货仓 LOAD KEY
        /// </summary>
        [EntityPropertyExtension("str9", "str9")]
        public string str9 { get; set; }
        /// <summary>
        /// 退货仓  NFS店铺编码
        /// </summary>
        [EntityPropertyExtension("str4", "str4")]
        public string str4 { get; set; }
        /// <summary>
        /// 退货仓 公司名
        /// </summary>
        [EntityPropertyExtension("str5", "str5")]
        public string str5 { get; set; }
        /// <summary>
        /// 退货仓
        /// </summary>
        [EntityPropertyExtension("str6", "str6")]
        public string str6 { get; set; }
        /// <summary>
        /// 退货仓 计划发货时间
        /// </summary>
        [EntityPropertyExtension("str7", "str7")]
        public string str7 { get; set; }
        /// <summary>
        /// 退货仓 VAS Code
        /// </summary>
        [EntityPropertyExtension("str8", "str8")]
        public string str8 { get; set; }
        /// <summary>
        /// 退货仓专用 Nike PO
        /// </summary>
        [EntityPropertyExtension("str10", "str10")]
        public string str10 { get; set; }
        /// <summary>
        /// 退货仓专用 PACK SLIP NO
        /// </summary>
        [EntityPropertyExtension("str11", "str11")]
        public string str11 { get; set; }
        /// <summary>
        /// 退货仓专用 LF出库单号行号 (是否单仓)
        /// </summary>
        [EntityPropertyExtension("str12", "str12")]
        public string str12 { get; set; }
        /// <summary>
        /// 退货仓专用 BU
        /// </summary>
        [EntityPropertyExtension("str13", "str13")]
        public string str13 { get; set; }
        /// <summary>
        /// 退货仓专用 RP LI
        /// </summary>
        [EntityPropertyExtension("str14", "str14")]
        public string str14 { get; set; }
        /// <summary>
        /// 退货仓专用 CRD
        /// </summary>
        [EntityPropertyExtension("str15", "str15")]
        public string str15 { get; set; }
        /// <summary>
        /// 退货仓专用 PLNO
        /// </summary>
        [EntityPropertyExtension("PLNO", "PLNO")]
        public string PLNO { get; set; }
        /// <summary>
        /// 退货仓专用 address1
        /// </summary>
        [EntityPropertyExtension("Address1", "Address1")]
        public string Address1 { get; set; }
        /// <summary>
        /// 退货仓专用 address2
        /// </summary>
        [EntityPropertyExtension("Address2", "Address2")]
        public string Address2 { get; set; }
        /// <summary>
        /// 退货仓专用 address3
        /// </summary>
        [EntityPropertyExtension("Address3", "Address3")]
        public string Address3 { get; set; }
        /// <summary>
        /// 退货仓专用 address4
        /// </summary>
        [EntityPropertyExtension("Address4", "Address4")]
        public string Address4 { get; set; }
        /// <summary>
        /// 退货仓专用 City
        /// </summary>
        [EntityPropertyExtension("City", "City")]
        public string City { get; set; }
        

    }
}
