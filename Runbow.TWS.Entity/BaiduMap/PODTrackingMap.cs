using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.BaiduMap
{
    public class PODTrackingMap
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("DriverPhone", "DriverPhone")]
        public string DriverPhone { get; set; }

        [EntityPropertyExtension("CarNo", "CarNo")]
        public string CarNo { get; set; }

        [EntityPropertyExtension("DriverName", "DriverName")]
        public string DriverName { get; set; }

        [EntityPropertyExtension("PODType", "PODType")]
        public string PODType { get; set; }

        [EntityPropertyExtension("ActualDepartureTime", "ActualDepartureTime")]
        public string ActualDepartureTime { get; set; }

        [EntityPropertyExtension("Num", "Num")]
        public int Num { get; set; }

        [EntityPropertyExtension("Longitude", "Longitude")]
        public string Longitude { get; set; }

        [EntityPropertyExtension("Times", "Times")]
        public DateTime Times { get; set; }

        [EntityPropertyExtension("Latitude", "Latitude")]
        public string Latitude { get; set; }

        [EntityPropertyExtension("PodID", "PodID")]
        public string PodID { get; set; }

        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }

        [EntityPropertyExtension("GeographicalPosition", "GeographicalPosition")]
        public string GeographicalPosition { get; set; }

        [EntityPropertyExtension("info", "info")]
        public string info { get; set; }

        [EntityPropertyExtension("Hub", "Hub")]
        public string Hub { get; set; }


        [EntityPropertyExtension("Code", "Code")]
        public string Code { get; set; }
    }
}
