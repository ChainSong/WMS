using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class VehicleLocation
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("Times", "Times")]
        public DateTime? Times { get; set; }

        [EntityPropertyExtension("Phone", "Phone")]
        public string Phone { get; set; }

        [EntityPropertyExtension("VehicleNumber", "VehicleNumber")]
        public string VehicleNumber { get; set; }

        [EntityPropertyExtension("Longitude", "Longitude")]
        public string Longitude { get; set; }

        [EntityPropertyExtension("Latitude", "Latitude")]
        public string Latitude { get; set; }

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

        [EntityPropertyExtension("Int1", "Int1")]
        public long Int1 { get; set; }

        [EntityPropertyExtension("Int2", "Int2")]
        public long Int2 { get; set; }

        [EntityPropertyExtension("Int3", "Int3")]
        public long Int3 { get; set; }

        [EntityPropertyExtension("Datetime1", "Datetime1")]
        public DateTime? Datetime1 { get; set; }

        [EntityPropertyExtension("Datetime2", "Datetime2")]
        public DateTime? Datetime2 { get; set; }

        [EntityPropertyExtension("Datetime3", "Datetime3")]
        public DateTime? Datetime3 { get; set; }
    }
}
