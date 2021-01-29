using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD.Nike
{
    public class NikeforBSPOD
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }
        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }
        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        [EntityPropertyExtension("ShipperID", "ShipperID")]
        public long ShipperID { get; set; }
        [EntityPropertyExtension("ShipperName", "ShipperName")]
        public string ShipperName { get; set; }
        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public DateTime? ActualDeliveryDate { get; set; }
        [EntityPropertyExtension("StartCityID", "StartCityID")]
        public long StartCityID { get; set; }
        [EntityPropertyExtension("StartCityName", "StartCityName")]
        public string StartCityName { get; set; }
        [EntityPropertyExtension("EndCityID", "EndCityID")]
        public long EndCityID { get; set; }
        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }
        [EntityPropertyExtension("PODStateID", "PODStateID")]
        public long PODStateID { get; set; }
        [EntityPropertyExtension("PODStateName", "PODStateName")]
        public string PODStateName { get; set; }
        [EntityPropertyExtension("ShipperTypeID", "ShipperTypeID")]
        public long ShipperTypeID { get; set; }
        [EntityPropertyExtension("ShipperTypeName", "ShipperTypeName")]
        public string ShipperTypeName { get; set; }
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public float BoxNumber { get; set; }
        [EntityPropertyExtension("Weight", "Weight")]
        public float Weight { get; set; }
        [EntityPropertyExtension("GoodsNumber", "GoodsNumber")]
        public float GoodsNumber { get; set; }
        [EntityPropertyExtension("Volume", "Volume")]
        public float Volume { get; set; }
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
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
        [EntityPropertyExtension("Str50", "Str50")]
        public string Str50 { get; set; }
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
        [EntityPropertyExtension("DateTime6", "DateTime6")]
        public DateTime? DateTime6 { get; set; }
        [EntityPropertyExtension("PODTypeID", "PODTypeID")]
        public long PODTypeID { get; set; }
        [EntityPropertyExtension("PODTypeName", "PODTypeName")]
        public string PODTypeName { get; set; }
        [EntityPropertyExtension("TtlOrTplID", "TtlOrTplID")]
        public long TtlOrTplID { get; set; }
        [EntityPropertyExtension("TtlOrTplName", "TtlOrTplName")]
        public string TtlOrTplName { get; set; }
        [EntityPropertyExtension("IsSettledForCustomer", "IsSettledForCustomer")]
        public string IsSettledForCustomer { get; set; }
        [EntityPropertyExtension("IsSettledForShipper", "IsSettledForShipper")]
        public bool IsSettledForShipper { get; set; }
  
        [EntityPropertyExtension("Type", "Type")]
        public int Type { get; set; }
        [EntityPropertyExtension("HasShortDial", "HasShortDial")]
        public bool HasShortDial { get; set; }
        [EntityPropertyExtension("HasDistribution", "HasDistribution")]
        public bool HasDistribution { get; set; }
        [EntityPropertyExtension("HasExpress", "HasExpress")]
        public bool HasExpress { get; set; }

    }
}
