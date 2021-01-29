using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class DMS_POD
    {
        #region Model   

        [EntityPropertyExtension("MKWebOrderNo", "MKWebOrderNo")]
        public string MKWebOrderNo { get; set; }

        [EntityPropertyExtension("MKConsultID", "MKConsultID")]
        public string MKConsultID { get; set; }

        [EntityPropertyExtension("MKConsultName", "MKConsultName")]
        public string MKConsultName { get; set; }

        [EntityPropertyExtension("IDCardType", "IDCardType")]
        public string IDCardType { get; set; }

        [EntityPropertyExtension("IDCardNo", "IDCardNo")]
        public string IDCardNo { get; set; }

        [EntityPropertyExtension("OrderAmount", "OrderAmount")]
        public decimal? OrderAmount { get; set; }

        [EntityPropertyExtension("MKBigCartonNo", "MKBigCartonNo")]
        public string MKBigCartonNo { get; set; }

        [EntityPropertyExtension("lot", "lot")]
        public string lot { get; set; }

        [EntityPropertyExtension("DeliveryNo", "DeliveryNo")]
        public string DeliveryNo { get; set; }

        [EntityPropertyExtension("DeliveryCompany", "DeliveryCompany")]
        public string DeliveryCompany { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        /// <summary>
        /// 系统单号
        /// </summary>
        [EntityPropertyExtension("BillNo", "BillNo")]
        public string BillNo { get; set; }
 
        /// <summary>
        /// 实际发货时间
        /// </summary>
        [EntityPropertyExtension("ActualShipTime", "ActualShipTime")]
        public DateTime? ActualShipTime { get; set; }

        /// <summary>
        /// 预计到达时间
        /// </summary>
        [EntityPropertyExtension("DeliveryTime", "DeliveryTime")]
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// 实际到达时间
        /// </summary>
        [EntityPropertyExtension("ActualCompleteTime", "ActualCompleteTime")]
        public DateTime? ActualCompleteTime { get; set; }

        /// <summary>
        /// 预计回单时间
        /// </summary>
        [EntityPropertyExtension("RequireBillReturnTime", "RequireBillReturnTime")]
        public DateTime? RequireBillReturnTime { get; set; }
 
        /// <summary>
        /// 实际回单时间
        /// </summary>
        [EntityPropertyExtension("BillReturnTime", "BillReturnTime")]
        public DateTime? BillReturnTime { get; set; }

        /// <summary>
        /// 计划发货时间
        /// </summary>
        [EntityPropertyExtension("BeginAddressTime", "BeginAddressTime")]
        public DateTime? BeginAddressTime { get; set; }
 
        /// <summary>
        /// 退货类型
        /// </summary>
        [EntityPropertyExtension("ReturnType", "ReturnType")]
        public string ReturnType { get; set; }

        /// <summary>
        /// 通知提货时间
        /// </summary>
        [EntityPropertyExtension("ReturnInformTime", "ReturnInformTime")]
        public DateTime? ReturnInformTime { get; set; }

        /// <summary>
        /// 要求退回时间
        /// </summary>
        [EntityPropertyExtension("ReturnNeedBackTime", "ReturnNeedBackTime")]
        public DateTime? ReturnNeedBackTime { get; set; }

        /// <summary>
        /// 预估货物总箱数
        /// </summary>
        [EntityPropertyExtension("IntendBoxNumber", "IntendBoxNumber")]
        public decimal? IntendBoxNumber { get; set; }

        /// <summary>
        /// 实际货物总箱数
        /// </summary>
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public decimal? BoxNumber { get; set; }
  
        /// <summary>
        /// 预估货物总件数
        /// </summary>
        [EntityPropertyExtension("BookingNumber", "BookingNumber")]
        public decimal? BookingNumber { get; set; }

        /// <summary>
        /// 实际货物总件数
        /// </summary>
        [EntityPropertyExtension("TotalQty", "TotalQty")]
        public decimal? TotalQty { get; set; }

        /// <summary>
        /// 货物总净重
        /// </summary>
        [EntityPropertyExtension("TotalWeight", "TotalWeight")]
        public decimal? TotalWeight { get; set; }

        /// <summary>
        /// 发货单号
        /// </summary>
        [EntityPropertyExtension("CustomerOrderNo", "CustomerOrderNo")]
        public string CustomerOrderNo { get; set; }

        /// <summary>
        /// 货物价值
        /// </summary>
        [EntityPropertyExtension("BalanceAmount", "BalanceAmount")]
        public decimal? BalanceAmount { get; set; }

        /// <summary>
        /// 空运结算重量
        /// </summary>
        [EntityPropertyExtension("TotalGrossWeight", "TotalGrossWeight")]
        public decimal? TotalGrossWeight { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        [EntityPropertyExtension("Length", "Length")]
        public decimal? Length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        [EntityPropertyExtension("Width", "Width")]
        public decimal? Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        [EntityPropertyExtension("Height", "Height")]
        public decimal? Height { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        [EntityPropertyExtension("TotalCubage", "TotalCubage")]
        public decimal? TotalCubage { get; set; }

        /// <summary>
        /// 起运单位代码
        /// </summary>
        [EntityPropertyExtension("BalancerCode", "BalancerCode")]
        public string BalancerCode { get; set; }

        /// <summary>
        /// 起运单位名称
        /// </summary>
        [EntityPropertyExtension("BalancerName", "BalancerName")]
        public string BalancerName { get; set; }

        /// <summary>
        /// 起运单位联系人
        /// </summary>
        [EntityPropertyExtension("BalancerContact", "BalancerContact")]
        public string BalancerContact { set; get; }

        /// <summary>
        /// 起运单位联系电话
        /// </summary>
        [EntityPropertyExtension("BalancerTEL", "BalancerTEL")]
        public string BalancerTEL { get; set; }

        /// <summary>
        /// 起运省份
        /// </summary>
        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        /// <summary>
        /// 起运城市
        /// </summary>
        [EntityPropertyExtension("BeginCityName", "BeginCityName")]
        public string BeginCityName { get; set; }

        /// <summary>
        /// 起运单位地址
        /// </summary>
        [EntityPropertyExtension("BeginAddress", "BeginAddress")]
        public string BeginAddress { get; set; }

        /// <summary>
        /// 目的单位代码
        /// </summary>
        [EntityPropertyExtension("ReceiverCode", "ReceiverCode")]
        public string ReceiverCode { get; set; }

        /// <summary>
        /// 目的单位名称
        /// </summary>
        [EntityPropertyExtension("ReceiverName", "ReceiverName")]
        public string ReceiverName { get; set; }

        /// <summary>
        /// 目的单位联系人
        /// </summary>
        [EntityPropertyExtension("ReceiverContact", "ReceiverContact")]
        public string ReceiverContact { get; set; }

        /// <summary>
        /// 目的单位联系电话
        /// </summary>
        [EntityPropertyExtension("ReceiverTEL", "ReceiverTEL")]
        public string ReceiverTEL { get; set; }

        /// <summary>
        /// 目的单位手机号码
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public string EndAddressTEL { get; set; }

        /// <summary>
        /// 目的单位传真
        /// </summary>
        [EntityPropertyExtension("CustomerFax", "CustomerFax")]
        public string CustomerFax { get; set; } 

        /// <summary>
        /// 目的省份
        /// </summary>
        [EntityPropertyExtension("Province", "Province")]
        public string Province { get; set; }
  
        /// <summary>
        /// 订单类型
        /// </summary>
        [EntityPropertyExtension("OrderTypes", "OrderTypes")]
        public string OrderTypes { get; set; }
  
        /// <summary>
        /// 目的城市
        /// </summary>
        [EntityPropertyExtension("City", "City")]
        public string City { get; set; }

        /// <summary>
        /// 目的单位地址
        /// </summary>
        [EntityPropertyExtension("EndAddress", "EndAddress")]
        public string EndAddress { get; set; }

        /// <summary>
        /// 起止里程
        /// </summary>
        [EntityPropertyExtension("AcceptOrderPrice", "AcceptOrderPrice")]
        public string AcceptOrderPrice { get; set; }

        /// <summary>
        /// 注意事项
        /// </summary>
        [EntityPropertyExtension("RecieveFeeRemark", "RecieveFeeRemark")]
        public string RecieveFeeRemark { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EntityPropertyExtension("Remarks", "Remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        [EntityPropertyExtension("IsUrgency", "IsUrgency")]
        public string IsUrgency { get; set; }
 
        /// <summary>
        /// 运输方式
        /// </summary>
        [EntityPropertyExtension("BizMode", "BizMode")]
        public string BizMode { get; set; }


        /// <summary>
        /// 项目编码
        /// </summary>
        [EntityPropertyExtension("CustomerCode", "CustomerCode")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 承运商编号
        /// </summary>
        [EntityPropertyExtension("ShipperCode", "ShipperCode")]
        public string ShipperCode { get; set; }

        /// <summary>
        /// 托运单状态
        /// </summary>
        [EntityPropertyExtension("Status", "Status")]
        public string Status { get; set; }


        /// <summary>
        /// 短信预报情况
        /// </summary>
        [EntityPropertyExtension("SMS_Status", "SMS_Status")]
        public string SMS_Status { get; set; }


        /// <summary>
        /// 短信预报次数
        /// </summary>
        [EntityPropertyExtension("SMS_Frequency", "SMS_Frequency")]
        public decimal? SMS_Frequency { get; set; }


        /// <summary>
        /// 最后一次短信预报时间
        /// </summary>
        [EntityPropertyExtension("SMS_LastSendTime", "SMS_LastSendTime")]
        public DateTime? SMS_LastSendTime { get; set; }


        /// <summary>
        /// 电话预报状态
        /// </summary>
        [EntityPropertyExtension("TEL_Status", "TEL_Status")]
        public string TEL_Status { get; set; }
 

        /// <summary>
        /// 电话预报次数
        /// </summary>
        [EntityPropertyExtension("TEL_Frequency", "TEL_Frequency")]
        public decimal? TEL_Frequency { get; set; }


        /// <summary>
        /// 电话预报最新时间
        /// </summary>
        [EntityPropertyExtension("TEL_LastSendTime", "TEL_LastSendTime")]
        public DateTime? TEL_LastSendTime { get; set; }

        /// <summary>
        /// 签单人
        /// </summary>
        [EntityPropertyExtension("Receipt_SignOne", "Receipt_SignOne")]
        public string Receipt_SignOne { get; set; }

        /// <summary>
        /// 回单备注
        /// </summary>
        [EntityPropertyExtension("Receipt_Remarks", "Receipt_Remarks")]
        public string Receipt_Remarks { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [EntityPropertyExtension("colCreater", "colCreater")]
        public string colCreater { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("colCreateTime", "colCreateTime")]
        public DateTime? colCreateTime { get; set; }

        /// <summary>
        /// 更新人员
        /// </summary>
        [EntityPropertyExtension("colUpdater", "colUpdater")]
        public string colUpdater { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [EntityPropertyExtension("colUpdateTime", "colUpdateTime")]
        public DateTime? colUpdateTime { get; set; }

        /// <summary>
        /// 承运商名称
        /// </summary>
        [EntityPropertyExtension("ShipperName", "ShipperName")]
        public string ShipperName { get; set; }

        /// <summary>
        /// 运输方式名称
        /// </summary>
        [EntityPropertyExtension("BizModeName", "BizModeName")]
        public string BizModeName { get; set; }

        /// <summary>
        /// 订单类型名称
        /// </summary>
        [EntityPropertyExtension("OrderTypeName", "OrderTypeName")]
        public string OrderTypeName { get; set; }

        #endregion
    }
}
