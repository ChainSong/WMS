using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class Log56Track
    {
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("PodID", "PodID")]
        public long PodID { get; set; }

        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("Phone", "Phone")]
        public string Phone { get; set; }

        [EntityPropertyExtension("CurrentLocation", "CurrentLocation")]
        public string CurrentLocation { get; set; }

        [EntityPropertyExtension("TrackTime", "TrackTime")]
        public DateTime TrackTime { get; set; }

        [EntityPropertyExtension("Trackor", "Trackor")]
        public string Trackor { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        public string ServiceStatus { get; set; }
    }
}