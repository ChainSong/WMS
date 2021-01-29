using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;


namespace Runbow.TWS.Entity.WMS.Report
{
    /// <summary>
    /// 快递单报表
    /// </summary>
    public class ReportExpressInfo
    {

        /// <summary>
        /// 自增ID多列
        /// </summary>
        [EntityPropertyExtension("IDS", "IDS")]
        public string IDS { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        [EntityPropertyExtension("OrderType", "OrderType")]
        public string OrderType { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }
        /// <summary>
        /// 客户(货主)编号
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        //系统订单号
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }
        //外部单号
        [EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        public string ExternOrderNumber { get; set; }
        //订单创建时间
        [EntityPropertyExtension("OrderTime", "OrderTime")]
        public string OrderTime { get; set; }
        //出库时间
        [EntityPropertyExtension("CompleteDate", "CompleteDate")]
        public string CompleteDate { get; set; }
        //货主
        [EntityPropertyExtension("StorerKey", "StorerKey")]
        public string StorerKey { get; set; }
        //订单状态
        [EntityPropertyExtension("Status", "Status")]
        public string Status { get; set; }
        //联系人
        [EntityPropertyExtension("Contact1", "Contact1")]
        public string Contact1 { get; set; }
        //联系电话
        [EntityPropertyExtension("PhoneNum1", "PhoneNum1")]
        public string PhoneNum1 { get; set; }
        //公司名称
        [EntityPropertyExtension("Company", "Company")]
        public string Company { get; set; }
        //地址
        [EntityPropertyExtension("AddressLine1", "AddressLine1")]
        public string AddressLine1 { get; set; }
        //快递单号
        [EntityPropertyExtension("ExpressNumber", "ExpressNumber")]
        public string ExpressNumber { get; set; }
        //快递公司
        [EntityPropertyExtension("ExpressCompany", "ExpressCompany")]
        public string ExpressCompany { get; set; }
        //箱号
        [EntityPropertyExtension("PackageNumber", "PackageNumber")]
        public string PackageNumber { get; set; }
        //净重
        [EntityPropertyExtension("NetWeight", "NetWeight")]
        public string NetWeight { get; set; }
        //箱型
        [EntityPropertyExtension("BoxSize", "BoxSize")]
        public string BoxSize { get; set; }
        //体积
        [EntityPropertyExtension("Volume", "Volume")]
        public string Volume { get; set; }
        //箱件数
        [EntityPropertyExtension("Qty", "Qty")]
        public string Qty { get; set; }
        //箱数(箱号)
        [EntityPropertyExtension("Box", "Box")]
        public string Box { get; set; }
        //商品名称   
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }
        #region 备用字段
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

        [EntityPropertyExtension("str6", "str6")]
        public string str6 { get; set; }

        [EntityPropertyExtension("str7", "str7")]
        public string str7 { get; set; }

        [EntityPropertyExtension("str8", "str8")]
        public string str8 { get; set; }

        [EntityPropertyExtension("str9", "str9")]
        public string str9 { get; set; }

        [EntityPropertyExtension("str10", "str10")]
        public string str10 { get; set; }
        #endregion

    }
}
