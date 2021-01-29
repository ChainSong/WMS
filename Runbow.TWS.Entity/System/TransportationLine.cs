using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class TransportationLine
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }

        [EntityPropertyExtension("StartCityID", "StartCityID")]
        public long StartCityID { get; set; }

        [EntityPropertyExtension("StartCityName", "StartCityName")]
        public string StartCityName { get; set; }

        [EntityPropertyExtension("EndCityID", "EndCityID")]
        public long EndCityID { get; set; }

        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("Distance", "Distance")]
        public string Distance { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

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
    }
}