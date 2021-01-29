using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.NikeOSRBJPrint
{
    public class PrintBoxInfo
    {

        [EntityPropertyExtension("Status", "Status")]
        public int Status { get; set; }

        [EntityPropertyExtension("PackageNumber", "PackageNumber")]
        public string PackageNumber { get; set; }

        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }

        [EntityPropertyExtension("Atrcle", "Atrcle")]
        public string Atrcle { get; set; }

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("Qty", "Qty")]
        public int Qty { get; set; }

        [EntityPropertyExtension("str4", "str4")]
        public string str4 { get; set; }

        [EntityPropertyExtension("str5", "str5")]
        public string str5 { get; set; }

        [EntityPropertyExtension("CompanyCode", "CompanyCode")]
        public string CompanyCode { get; set; }

        [EntityPropertyExtension("Company", "Company")]
        public string Company { get; set; }

        [EntityPropertyExtension("AddressLine1", "AddressLine1")]
        public string AddressLine1 { get; set; }

        [EntityPropertyExtension("StorerKey", "StorerKey")]
        public string StorerKey { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public int CustomerID { get; set; }

        [EntityPropertyExtension("BU", "BU")]
        public string BU { get; set; }

        [EntityPropertyExtension("GenderAge", "GenderAge")]
        public string GenderAge { get; set; }

        [EntityPropertyExtension("ShipmentNo", "ShipmentNo")]
        public string ShipmentNo { get; set; }

        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }

        [EntityPropertyExtension("MaterialDesc", "MaterialDesc")]
        public string MaterialDesc { get; set; }

        [EntityPropertyExtension("Gender", "Gender")]
        public string Gender { get; set; }

        [EntityPropertyExtension("Str17", "Str17")]
        public string Str17 { get; set; }

        [EntityPropertyExtension("Str18", "Str18")]
        public string Str18 { get; set; }

        [EntityPropertyExtension("Str19", "Str19")]
        public string Str19 { get; set; }

        [EntityPropertyExtension("Str20", "Str20")]
        public string Str20 { get; set; }

        //楼层
        [EntityPropertyExtension("Str9", "Str9")]
        public string Str9 { get; set; }

        [EntityPropertyExtension("City", "City")]
        public string City { get; set; }

        [EntityPropertyExtension("CRD", "CRD")]
        public string CRD { get; set; }

        [EntityPropertyExtension("Category", "Category")]
        public string Category { get; set; }

        [EntityPropertyExtension("SafeLock", "SafeLock")]
        public string SafeLock { get; set; }

        [EntityPropertyExtension("Hanger", "Hanger")]
        public string Hanger { get; set; }

        [EntityPropertyExtension("ToCompanyCode", "ToCompanyCode")]
        public string ToCompanyCode { get; set; }

        /// <summary>
        /// 退货仓
        /// </summary>
        [EntityPropertyExtension("PLNO", "PLNO")]
        public string PLNO { get; set; }
        /// <summary>
        /// 退货仓 是否单仓
        /// </summary>
        [EntityPropertyExtension("Str12", "Str12")]
        public string Str12 { get; set; }
        /// <summary>
        /// 退货仓 预计发货日期
        /// </summary>
        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7 { get; set; }
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
        /// 退货仓专用 毛重
        /// </summary>
        [EntityPropertyExtension("GrossWeight", "GrossWeight")]
        public string GrossWeight { get; set; }
        /// <summary>
        /// 退货仓专用 体积(volume)
        /// </summary>
        [EntityPropertyExtension("NetWeight", "NetWeight")]
        public string NetWeight { get; set; }
        /// <summary>
        /// 退货仓专用 LF出库单号
        /// </summary>
        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8 { get; set; }


        /// <summary>
        /// PACK SLIP NO
        /// </summary>
        [EntityPropertyExtension("Str11", "Str11")]
        public string Str11 { get; set; }
        /// <summary>
        /// CRD
        /// </summary>
        [EntityPropertyExtension("Str15", "Str15")]
        public string Str15 { get; set; }
        /// <summary>
        /// 就不能多整点备用字段吗 没意思
        /// </summary>
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
        [EntityPropertyExtension("Str16", "Str16")]
        public string Str16 { get; set; }
        [EntityPropertyExtension("Str10", "Str10")]
        public string Str10 { get; set; }
        [EntityPropertyExtension("Str13", "Str13")]
        public string Str13 { get; set; }

        [EntityPropertyExtension("Str14", "Str14")]
        public string Str14 { get; set; }


        /// <summary>
        /// 退货仓专用 
        /// </summary>
        [EntityPropertyExtension("Str6", "Str6")]
        public string Str6 { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [EntityPropertyExtension("Quantity", "Quantity")]
        public int Quantity { get; set; }
    }
}
