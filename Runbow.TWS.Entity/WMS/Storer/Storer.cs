using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// WMS货主实体Model
    /// </summary>
    public class Storer
    {
        #region Model
        /// <summary>
        /// 自增ID
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        /// <summary>
        /// 基本信息-货主名称
        /// </summary>
        [EntityPropertyExtension("StorerName", "StorerName")]
        public string StorerName { set; get; }
        /// <summary>
        /// 基本信息-使用状态
        /// </summary>
        [EntityPropertyExtension("Status", "Status")]
        public int Status
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-货主类型
        /// </summary>
        [EntityPropertyExtension("Type", "Type")]
        public int Type
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-全称
        /// </summary>
        [EntityPropertyExtension("FullName", "FullName")]
        public string FullName
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-公司
        /// </summary>
        [EntityPropertyExtension("Company", "Company")]
        public string Company
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-信用额度
        /// </summary>
        [EntityPropertyExtension("CreditLine", "CreditLine")]
        public string CreditLine
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-地址
        /// </summary>
        [EntityPropertyExtension("Address", "Address")]
        public string Address
        {
            set;
            get;
        }
        /// <summary>
        /// 省份城市
        /// </summary>
        [EntityPropertyExtension("ProvinceCity", "ProvinceCity")]
        public string ProvinceCity
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-邮政编码
        /// </summary>
        [EntityPropertyExtension("ZipCode", "ZipCode")]
        public string ZipCode
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-联系人
        /// </summary>
        [EntityPropertyExtension("Contractor", "Contractor")]
        public string Contractor
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-联系地址
        /// </summary>
        [EntityPropertyExtension("ContractorAddress", "ContractorAddress")]
        public string ContractorAddress
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-手机
        /// </summary>
        [EntityPropertyExtension("Mobile", "Mobile")]
        public string Mobile
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-电话
        /// </summary>
        [EntityPropertyExtension("Phone", "Phone")]
        public string Phone
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-传真
        /// </summary>
        [EntityPropertyExtension("Fax", "Fax")]
        public string Fax
        {
            set;
            get;
        }
        /// <summary>
        /// 基本信息-电子邮箱
        /// </summary>
        [EntityPropertyExtension("Email", "Email")]
        public string Email
        {
            set;
            get;
        }
        /// <summary>
        /// 开户行
        /// </summary>
        [EntityPropertyExtension("Bank", "Bank")]
        public string Bank
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("BankAccount", "BankAccount")]
        public string BankAccount
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark
        {
            set;
            get;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator
        {
            set;
            get;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str6", "Str6")]
        public string Str6
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str9", "Str9")]
        public string Str9
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str10", "Str10")]
        public string Str10
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str11", "Str11")]
        public string Str11
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str12", "Str12")]
        public string Str12
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str13", "Str13")]
        public string Str13
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str14", "Str14")]
        public string Str14
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str15", "Str15")]
        public string Str15
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str16", "Str16")]
        public string Str16
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str17", "Str17")]
        public string Str17
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str18", "Str18")]
        public string Str18
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str19", "Str19")]
        public string Str19
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Str20", "Str20")]
        public string Str20
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime? DateTime3
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("DateTime4", "DateTime4")]
        public DateTime? DateTime4
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("DateTime5", "DateTime5")]
        public DateTime? DateTime5
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Bigint1", "Bigint1")]
        public long? Bigint1
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Bingint2", "Bingint2")]
        public long? Bingint2
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Bigint3", "Bigint3")]
        public long? Bigint3
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Int1", "Int1")]
        public int? Int1
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Int2", "Int2")]
        public int? Int2
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Int3", "Int3")]
        public int? Int3
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Bit1", "Bit1")]
        public bool? Bit1
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Bit2", "Bit2")]
        public bool? Bit2
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Bit3", "Bit3")]
        public bool? Bit3
        {
            set;
            get;
        }
        #endregion Model
    }
}
