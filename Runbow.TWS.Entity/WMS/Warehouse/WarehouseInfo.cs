using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class WarehouseInfo
    {
        /*仓库基本信息
        */

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        /// <summary>
        /// 仓库状态
        /// </summary>
        [EntityPropertyExtension("WarehouseStatus", "WarehouseStatus")]
        public string WarehouseStatus { get; set; }
        /// <summary>
        /// 仓库类型
        /// </summary>
        [EntityPropertyExtension("WarehouseType", "WarehousevType")]
        public long WarehouseType { get; set; }

        /// <summary>
        /// 仓库描述
        /// </summary>
        [EntityPropertyExtension("Description", "Description")]
        public string Description { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        [EntityPropertyExtension("Company", "Company")]
        public string Company { get; set; }

        /// <summary>
        /// 信用额度
        /// </summary>
        [EntityPropertyExtension("CreditLine", "CreditLine")]
        public string CreditLine { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [EntityPropertyExtension("Address", "Address")]
        public string Address { get; set; }

        /// <summary>
        /// 省份城市
        /// </summary>
        [EntityPropertyExtension("ProvinceCity", "ProvinceCity")]
        public string ProvinceCity { get; set; }
        /// <summary>
        /// 邮政编号
        /// </summary>
        [EntityPropertyExtension("ZipCode", "ZipCode")]
        public string ZipCode { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [EntityPropertyExtension("Contractor", "Contractor")]
        public string Contractor { get; set; }
        /// <summary>
        /// 联系人地址
        /// </summary>
        [EntityPropertyExtension("ContractorAddress", "ContractorAddress")]
        public string ContractorAddress { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [EntityPropertyExtension("Mobile", "Mobile")]
        public string Mobile { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [EntityPropertyExtension("Phone", "Phone")]
        public string Phone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        [EntityPropertyExtension("Fax", "Fax")]
        public string Fax { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [EntityPropertyExtension("Email", "Email")]
        public string Email { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 所属客户ID
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public int  CustomerID { get; set; }

        /// <summary>
        /// 所属客户ID
        /// </summary>
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        /// <summary>
        /// 所属客户ID
        /// </summary>
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        /// <summary>
        /// 库区List
        /// </summary>
        public IEnumerable<AreaInfo> Areas { get; set; }

    }
}
