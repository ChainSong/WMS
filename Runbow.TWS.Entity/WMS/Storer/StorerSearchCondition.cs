using System;
using Runbow.TWS.Common;


namespace Runbow.TWS.Entity
{
    /// <summary>
    /// WMS货主
    /// </summary>
    public class StorerSearchCondition
    {
        #region Model
        /// <summary>
        /// 自增ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 基本信息-货主名称
        /// </summary>
        public string StorerName { set; get; }
        /// <summary>
        /// 基本信息-使用状态
        /// </summary>
        public int Status { set; get; }
        /// <summary>
        /// 基本信息-货主类型
        /// </summary>
        public int Type { set; get; }
        /// <summary>
        /// 基本信息-全称
        /// </summary>
        public string FullName { set; get; }
        /// <summary>
        /// 基本信息-公司
        /// </summary>
        public string Company { set; get; }
        /// <summary>
        /// 基本信息-信用额度
        /// </summary>
        public string CreditLine { set; get; }
        /// <summary>
        /// 基本信息-地址
        /// </summary>
        public string Address { set; get; }
        /// <summary>
        /// 省份城市
        /// </summary>
        public string ProvinceCity { set; get; }
        /// <summary>
        /// 基本信息-邮政编码
        /// </summary>
        public string ZipCode { set; get; }
        /// <summary>
        /// 基本信息-联系人
        /// </summary>
        public string Contractor { set; get; }
        /// <summary>
        /// 基本信息-联系地址
        /// </summary>
        public string ContractorAddress { set; get; }
        /// <summary>
        /// 基本信息-手机
        /// </summary>
        public string Mobile { set; get; }
        /// <summary>
        /// 基本信息-电话
        /// </summary>
        public string Phone { set; get; }
        /// <summary>
        /// 基本信息-传真
        /// </summary>
        public string Fax { set; get; }
        /// <summary>
        /// 基本信息-电子邮箱
        /// </summary>
        public string Email { set; get; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string BankAccount { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { set; get; }

        //订单日期
        public DateTime? StatCreateTime { get; set; }

        public DateTime? EndCreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Str1 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str2 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str3 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str4 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str5 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str6 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str7 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str8 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str9 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str10 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str11 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str12 { set; get; }
        /// <summary>
        /// 
        public string Str13 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str14 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str15 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str16 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str17 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str18 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str19 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Str20 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateTime1 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateTime2 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateTime3 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateTime4 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateTime5 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long? Bigint1 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long? Bingint2 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long? Bigint3 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int? Int1 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int? Int2 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int? Int3 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public bool? Bit1 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public bool? Bit2 { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public bool? Bit3 { set; get; }
        #endregion Model
    }
}
