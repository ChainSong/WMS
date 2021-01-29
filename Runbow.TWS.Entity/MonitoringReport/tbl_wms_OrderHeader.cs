using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.MonitoringReport
{
    public class tbl_wms_OrderHeader
    {
        [EntityPropertyExtension("yi", "yi")]
        public int yi { get; set; }

        [EntityPropertyExtension("wu", "wu")]
        public int wu { get; set; }
        [EntityPropertyExtension("dayin", "dayin")]
        public int dayin { get; set; }
        [EntityPropertyExtension("qi", "qi")]
        public int qi { get; set; }
        [EntityPropertyExtension("ba", "ba")]
        public int ba { get; set; }

        [EntityPropertyExtension("jiu", "jiu")]
        public int jiu { get; set; }
        [EntityPropertyExtension("quxiao", "quxiao")]
        public int quxiao { get; set; }
        [EntityPropertyExtension("zuotian", "zuotian")]
        public int zuotian { get; set; }

        [EntityPropertyExtension("num12", "num12")]
        public int num12 { get; set; }
        [EntityPropertyExtension("num24", "num24")]
        public int num24 { get; set; }
        [EntityPropertyExtension("num36", "num36")]
        public int num36 { get; set; }
        [EntityPropertyExtension("num48", "num48")]
        public int num48 { get; set; }
        [EntityPropertyExtension("num", "num")]
        public int num { get; set; }
        [EntityPropertyExtension("dates", "dates")]
        public int dates { get; set; }

        [EntityPropertyExtension("Complete", "Complete")]
        public int Complete { get; set; }

        [EntityPropertyExtension("Unfinished", "Unfinished")]
        public int Unfinished { get; set; }
        [EntityPropertyExtension("DataTime", "DataTime")]
        public DateTime DataTime { get; set; }


        [EntityPropertyExtension("Data", "Data")]
        public string Data { get; set; }
        [EntityPropertyExtension("OfficeElectric", "OfficeElectric")]
        public float OfficeElectric { get; set; }
        [EntityPropertyExtension("NfsElectric", "NfsElectric")]
        public float NfsElectric { get; set; }
        [EntityPropertyExtension("DigitalElectric", "DigitalElectric")]
        public float DigitalElectric { get; set; }
        [EntityPropertyExtension("InlineElectric", "InlineElectric")]
        public float InlineElectric { get; set; }
        [EntityPropertyExtension("Numerical", "Numerical")]
        public float Numerical { get; set; }
        [EntityPropertyExtension("TotalElectric", "TotalElectric")]
        public float TotalElectric { get; set; }
     
    }
}
