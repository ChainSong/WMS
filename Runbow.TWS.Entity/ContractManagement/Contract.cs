using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 合同表字段
    /// </summary>
    public class Contract
    {
        //Contract 属性

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("CompanyCode", "CompanyCode")]
        public string CompanyCode { get; set; }

        [EntityPropertyExtension("CompanyName", "CompanyName")]
        public string CompanyName { get; set; }

        [EntityPropertyExtension("BusinessCode", "BusinessCode")]
        public string BusinessCode { get; set; }

        [EntityPropertyExtension("BusinessName", "BusinessName")]
        public string BusinessName { get; set; }

        [EntityPropertyExtension("DepartmentCode", "DepartmentCode")]
        public string DepartmentCode { get; set; }

        [EntityPropertyExtension("DepartmentName", "DepartmentName")]
        public string DepartmentName { get; set; }

        [EntityPropertyExtension("ContractTypeCode", "ContractTypeCode")]
        public string ContractTypeCode { get; set; }

        [EntityPropertyExtension("ContractTypeName", "ContractTypeName")]
        public string ContractTypeName { get; set; }

        [EntityPropertyExtension("ContractStartDate", "ContractStartDate")]
        public DateTime ContractStartDate { get; set; }

        [EntityPropertyExtension("ContractNumber", "ContractNumber")]
        public string ContractNumber { get; set; }

        [EntityPropertyExtension("ContractContent", "ContractContent")]
        public string ContractContent { get; set; }

        [EntityPropertyExtension("BusinessPartnerName", "BusinessPartnerName")]
        public string BusinessPartnerName { get; set; }

        [EntityPropertyExtension("IsContractExtension", "IsContractExtension")]
        public string IsContractExtension { get; set; }

        [EntityPropertyExtension("ContractExpireDate", "ContractExpireDate")]
        public DateTime ContractExpireDate { get; set; }

        [EntityPropertyExtension("IsContractExpired", "IsContractExpired")]
        public string IsContractExpired { get; set; }

        [EntityPropertyExtension("StampTax", "StampTax")]
        public string StampTax { get; set; }

        [EntityPropertyExtension("OldContractNumber", "OldContractNumber")]
        public string OldContractNumber { get; set; }

        [EntityPropertyExtension("QualificationCertificate", "QualificationCertificate")]
        public string QualificationCertificate { get; set; }

        [EntityPropertyExtension("PolStartDate", "PolStartDate")]
        public DateTime PolStartDate { get; set; }

        [EntityPropertyExtension("PolEndDate", "PolEndDate")]
        public DateTime PolEndDate { get; set; }

        [EntityPropertyExtension("IsPolExpired", "IsPolExpired")]
        public string IsPolExpired { get; set; }

        [EntityPropertyExtension("AttachmentGroupID", "AttachmentGroupID")]
        public string AttachmentGroupID { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }


        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }


        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }


        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime UpdateTime { get; set; }


    }
}
