using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Order
{
    public class WMS_Wave
    {
        [EntityPropertyExtension("ID", "ID")]
        public int ID { get; set; }

        [EntityPropertyExtension("WaveNumber", "WaveNumber")]
        public string WaveNumber{get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("WaveCount", "WaveCount")]
        public int WaveCount { get; set; }

        [EntityPropertyExtension("ActualCount", "ActualCount")]
        public int ActualCount { get; set; }

        [EntityPropertyExtension("ExpressCompany", "ExpressCompany")]
        public string ExpressCompany { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public int Status { get; set; }

        [EntityPropertyExtension("PrintStatus", "PrintStatus")]
        public int? PrintStatus { get; set; }

        [EntityPropertyExtension("PrintCount", "PrintCount")]
        public int? PrintCount { get; set; }

        [EntityPropertyExtension("IsSinglePriece", "IsSinglePriece")]
        public int? IsSinglePriece { get; set; }

        [EntityPropertyExtension("IsExpressCompany", "IsExpressCompany")]
        public int? IsExpressCompany { get; set; }

        [EntityPropertyExtension("IsProvinceCity", "IsProvinceCity")]
        public int? IsProvinceCity { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }

        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }

        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }

        [EntityPropertyExtension("Str6", "Str6")]
        public string Str6 { get; set; }

        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7 { get; set; }

        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8 { get; set; }

        [EntityPropertyExtension("Str9", "Str9")]
        public string Str9 { get; set; }

        [EntityPropertyExtension("Str10", "Str10")]
        public string Str10 { get; set; }

        [EntityPropertyExtension("Str11", "Str11")]
        public string Str11 { get; set; }

        [EntityPropertyExtension("Str12", "Str12")]
        public string Str12 { get; set; }

        [EntityPropertyExtension("Str13", "Str13")]
        public string Str13 { get; set; }

        [EntityPropertyExtension("Str14", "Str14")]
        public string Str14 { get; set; }

        [EntityPropertyExtension("Str15", "Str15")]
        public string Str15 { get; set; }

        [EntityPropertyExtension("Str16", "Str16")]
        public string Str16 { get; set; }

        [EntityPropertyExtension("Str17", "Str17")]
        public string Str17 { get; set; }

        [EntityPropertyExtension("Str18", "Str18")]
        public string Str18 { get; set; }

        [EntityPropertyExtension("Str19", "Str19")]
        public string Str19 { get; set; }

        [EntityPropertyExtension("Str20", "Str20")]
        public string Str20 { get; set; }

        [EntityPropertyExtension("CompleteTime", "CompleteTime")]
        public DateTime? CompleteTime { get; set; }
    }
}
