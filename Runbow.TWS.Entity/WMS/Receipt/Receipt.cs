using System;
using Runbow.TWS.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 入库主订单
    /// </summary>
  [Serializable]
    public class Receipt
    {
        #region Model
        /// <summary>
        /// 仓库
        /// </summary>
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }

        /// <summary>
        /// 库区
        /// </summary>
        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }

         [EntityPropertyExtension("PageIndex", "PageIndex")]
         public string PageIndex { get; set; }

          [EntityPropertyExtension("PictureStr", "PictureStr")]
          public string PictureStr { get; set; }

         [EntityPropertyExtension("ReceiptTypeName", "ReceiptTypeName")]
        public string ReceiptTypeName { get; set; }

         [EntityPropertyExtension("ReceiptStatusName", "ReceiptStatusName")]
         public string ReceiptStatusName { get; set; }
        
        /// <summary>
        /// 自增ID
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        /// <summary>
        /// 收货单号，系统自动生成
        /// </summary>
        [EntityPropertyExtension("ReceiptNumber", "ReceiptNumber")]
        public string ReceiptNumber { get; set; }
        /// <summary>
        /// 外部单号，由外部导入
        /// </summary>
        [EntityPropertyExtension("ExternReceiptNumber", "ExternReceiptNumber")] 
        public string ExternReceiptNumber { get; set; }
        /// <summary>
        /// 预入库单ID
        /// </summary>
        [EntityPropertyExtension("ASNID", "ASNID")]
        public long ASNID { get; set; }
        /// <summary>
        /// 预入库单单号
        /// </summary>
        [EntityPropertyExtension("ASNNumber", "ASNNumber")]
        public string ASNNumber { get; set; }
        /// <summary>
        /// 客户(货主)编号
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }
        /// <summary>
        /// 客户(货主)名称
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        /// <summary>
        /// 预计收货日期
        /// </summary>
        [EntityPropertyExtension("ReceiptDate", "ReceiptDate")]
        public DateTime? ReceiptDate { get; set; }
        /// <summary>
        /// 1待上架 5已上架 9入库  -1取消
        /// </summary>
        [EntityPropertyExtension("Status", "Status")]
        public int Status { get; set; }
        /// <summary>
        /// 收货类型
        /// </summary>
        [EntityPropertyExtension("ReceiptType", "ReceiptType")]
        public string ReceiptType { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改操作人
        /// </summary>
        [EntityPropertyExtension("Updator", "Updator")] 
        public string Updator { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        [EntityPropertyExtension("UpdateTime", "UpdateTime")] 
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 收货完成日期
        /// </summary>
        [EntityPropertyExtension("CompleteDate", "CompleteDate")] 
        public DateTime? CompleteDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")] 
        public string Remark { get; set; }
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

        [EntityPropertyExtension("str11", "str11")]
        public string str11 { get; set; }

        [EntityPropertyExtension("str12", "str12")]
        public string str12 { get; set; }

        [EntityPropertyExtension("str13", "str13")]
        public string str13 { get; set; }

        [EntityPropertyExtension("str14", "str14")]
        public string str14 { get; set; }

        [EntityPropertyExtension("str15", "str15")]
        public string str15 { get; set; }

        [EntityPropertyExtension("str16", "str16")]
        public string str16 { get; set; }

        [EntityPropertyExtension("str17", "str17")]
        public string str17 { get; set; }
        
        [EntityPropertyExtension("str18", "str18")]
        public string str18 { get; set; }

        [EntityPropertyExtension("str19", "str19")]
        public string str19 { get; set; }

        [EntityPropertyExtension("str20", "str20")]
        public string str20 { get; set; }

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }

        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }

        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime? DateTime3 { get; set; }

        [EntityPropertyExtension("DateTime4", "DateTime4")]
        public DateTime? DateTime4 { get; set; }

        [EntityPropertyExtension("DateTime5", "DateTime5")]
        public DateTime? DateTime5 { get; set; }

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

        /// <summary>
        /// 产品编码
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        /// <summary>
        /// Article
        /// </summary>
        [EntityPropertyExtension("Article", "Article")]
        public string Article { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        /// <summary>
        /// Model
        /// </summary>
        [EntityPropertyExtension("Model", "Model")]
        public string Model { get; set; }
        
        #endregion Model
    }
}
