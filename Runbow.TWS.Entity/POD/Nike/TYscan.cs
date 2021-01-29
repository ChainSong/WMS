using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 天翼扫描记录实体Model
    /// </summary>
    public class TYscan
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("PODID", "PODID")]
        public long PODID { get; set; }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }

        [EntityPropertyExtension("PODStateName", "PODStateName")]
        public string PODStateName { get; set; }

        [EntityPropertyExtension("StartCityName", "StartCityName")]
        public string StartCityName { get; set; }

        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("PODTypeName", "PODTypeName")]
        public string PODTypeName { get; set; }

        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public string ActualDeliveryDate { get; set; }

        [EntityPropertyExtension("type", "type")]
        public int? type { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }


        [EntityPropertyExtension("Str42", "Str42")]
        public string Str42 { get; set; }


        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7 { get; set; }

        #region MyRegion
        //[EntityPropertyExtension("ID", "ID")]
        //public long ID { get; set; }

        //[EntityPropertyExtension("ID", "ID")]
        //public long PODID { get; set; }

        //[EntityPropertyExtension("OrderNumber", "OrderNumber")]
        //public string OrderNumber { get; set; }

        //[EntityPropertyExtension("Str5", "Str5")]
        //public string Str5 { get; set; }

        //[EntityPropertyExtension("Str14", "Str14")]
        //public string Str14 { get; set; }

        //[EntityPropertyExtension("Str10", "Str10")]
        //public string Str10 { get; set; }

        //[EntityPropertyExtension("Str12", "Str12")]
        //public string Str12 { get; set; }

        //[EntityPropertyExtension("Str32", "Str32")]
        //public string Str32 { get; set; }

        //[EntityPropertyExtension("Str8", "Str8")]
        //public string Str8 { get; set; }

        //[EntityPropertyExtension("type", "type")]
        //public int? type { get; set; }

        //[EntityPropertyExtension("CreateTime", "CreateTime")]
        //public DateTime CreateTime { get; set; }      
        #endregion
    }
}
