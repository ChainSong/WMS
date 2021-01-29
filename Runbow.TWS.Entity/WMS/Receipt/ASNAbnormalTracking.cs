using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Receipt
{
    public class ASNAbnormalTracking
    {

        #region Model
        /// <summary>
        /// 自增ID
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long? ID { get; set; }
         
        /// <summary>
        /// 预入库主订单ID
        /// </summary>
        [EntityPropertyExtension("ASNID", "ASNID")]
        public long? ASNID { get; set; }

        /// <summary>
        /// 预入库主订单ID
        /// </summary>
        [EntityPropertyExtension("ADID", "ADID")]
        public long? ADID { get; set; }


        /// <summary>
        /// 预入库主订单单号
        /// </summary>
        [EntityPropertyExtension("ASNNumber", "ASNNumber")]
        public string ASNNumber { get; set; }

        /// <summary>
        /// 外部单号，由外部指定 
        /// </summary>
        [EntityPropertyExtension("ExternReceiptNumber", "ExternReceiptNumber")]
        public string ExternReceiptNumber { get; set; }

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
        /// 索赔箱号（序号）
        /// </summary>
        [EntityPropertyExtension("BoxNo", "BoxNo")]
        public int BoxNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        [EntityPropertyExtension("Article", "Article")]
        public string Article { get; set; }

        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }

        /// <summary>
        /// 期望收货数量（计划数量）
        /// </summary>
        [EntityPropertyExtension("QtyExpected", "QtyExpected")]
        public decimal? QtyExpected { get; set; }

        /// <summary>
        /// 实际收货数量
        /// </summary>
        [EntityPropertyExtension("QtyReceived", "QtyReceived")]
        public decimal? QtyReceived { get; set; }

        /// <summary>
        /// 差异数量
        /// </summary>
        [EntityPropertyExtension("QtyDiff", "QtyDiff")]
        public decimal? QtyDiff { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        [EntityPropertyExtension("ReasonCode", "ReasonCode")]
        public string ReasonCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }


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
        /// 登记人 自己填
        /// </summary>
        [EntityPropertyExtension("Registrant", "Registrant")]
        public string Registrant { get; set; }

        /// <summary>
        /// 收货日期 自己选
        /// </summary>
        [EntityPropertyExtension("ReceiptTime", "ReceiptTime")]
        public DateTime? ReceiptTime { get; set; }

        /// <summary>
        /// 门店
        /// </summary>
        [EntityPropertyExtension("StorerKey", "StorerKey")]
        public string StorerKey { get; set; }
        /// <summary>
        /// 冻结状态
        /// </summary>
        [EntityPropertyExtension("FreeStatus", "FreeStatus")]
        public string FreeStatus { get; set; }
        /// <summary>
        /// 上架库位
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }
        /// <summary>
        /// 索赔编号
        /// </summary>
        [EntityPropertyExtension("ClaimNumber", "ClaimNumber")]
        public string ClaimNumber { get; set; }
        /// <summary>
        /// 冻结单号
        /// </summary>
        [EntityPropertyExtension("FreeNumber", "FreeNumber")]
        public string FreeNumber { get; set; }
        /// <summary>
        /// nike调查结果
        /// </summary>
        [EntityPropertyExtension("SurveyResult", "SurveyResult")]
        public string SurveyResult { get; set; }
        /// <summary>
        /// 调整数量
        /// </summary>
        [EntityPropertyExtension("QtyAdj", "QtyAdj")]
        public decimal? QtyAdj { get; set; }
        /// <summary>
        /// 系统调整结果
        /// </summary>
        [EntityPropertyExtension("QtyAdjResult", "QtyAdjResult")]
        public decimal? QtyAdjResult { get; set; }
        /// <summary>
        /// 系统调整日期
        /// </summary>
        [EntityPropertyExtension("AdjTime", "AdjTime")]
        public DateTime? AdjTime { get; set; }
        /// <summary>
        /// 发送IT调整邮件日期
        /// </summary>
        [EntityPropertyExtension("SendITTime", "SendITTime")]
        public DateTime? SendITTime { get; set; }
        /// <summary>
        /// IT回复上传文件时间
        /// </summary>
        [EntityPropertyExtension("ITReplyTime", "ITReplyTime")]
        public DateTime? ITReplyTime { get; set; }

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

        /// <summary>
        /// 索赔日期
        /// </summary>
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
        #endregion Model

    }
}
