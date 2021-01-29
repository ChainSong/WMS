using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD
{
    public class PODDistributionVehicle
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("CarNo", "CarNo")]
        public string CarNo { get; set; }

        [EntityPropertyExtension("DriverName", "DriverName")]
        public string DriverName { get; set; }

        [EntityPropertyExtension("DriverPhone", "DriverPhone")]
        public string DriverPhone { get; set; }

        [EntityPropertyExtension("PODType", "PODType")]
        public string PODType{ get; set; }

        [EntityPropertyExtension("StartTime", "StartTime")]
        public string StartTime { get; set; }

        [EntityPropertyExtension("EndTime", "EndTime")]
        public string EndTime { get; set; }

        [EntityPropertyExtension("Hub", "Hub")]
        public string Hub { get; set; }

        [EntityPropertyExtension("Ids", "Ids")]
        public IEnumerable<long> Ids { get; set; }

        [EntityPropertyExtension("UserName", "UserName")]
        public string UserName { get; set; }

        [EntityPropertyExtension("GPSCode", "GPSCode")]
        public string GPSCode { get; set; }
    }
}
