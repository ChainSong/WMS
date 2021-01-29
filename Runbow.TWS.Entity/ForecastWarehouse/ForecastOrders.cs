//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Runbow.TWS.Common;
namespace Runbow.TWS.Entity
{
    public class ForecastOrders
    {

        [EntityPropertyExtension("MailID", "MailID")]
        public string MailID { get; set; }
        [EntityPropertyExtension("ThreePL", "ThreePL")]
        public string ThreePL { get; set; }

        [EntityPropertyExtension("Piecesi", "Piecesi")]
        public string Piecesi { get; set; }

        [EntityPropertyExtension("zhi2", "zhi2")]
        public string zhi2 { get; set; }

        [EntityPropertyExtension("shipments", "shipments")]
        public string shipments { get; set; }
        [EntityPropertyExtension("WID", "WID")]
        public long WID { get; set; }
        [EntityPropertyExtension("WaveType", "WaveType")]
        public string WaveType { get; set; }

        [EntityPropertyExtension("waveId", "waveId")]
        public string waveId { get; set; }
        [EntityPropertyExtension("Shiptocity", "Shiptocity")]
        public string Shiptocity { get; set; }
        [EntityPropertyExtension("ShipToSity", "ShipToSity")]
        public string ShipToSity { get; set; }

        [EntityPropertyExtension("Shiptocode", "Shiptocode")]
        public string Shiptocode { get; set; }
        [EntityPropertyExtension("Shiptoname", "Shiptoname")]
        public string Shiptoname { get; set; }
        [EntityPropertyExtension("Pieces", "Pieces")]
        public string Pieces { get; set; }
        [EntityPropertyExtension("Cartons", "Cartons")]
        public string Cartons { get; set; }
        [EntityPropertyExtension("Sorterlane", "Sorterlane")]
        public string Sorterlane { get; set; }


        [EntityPropertyExtension("pl", "pl")]
        public string pl { get; set; }

        [EntityPropertyExtension("Shipment", "Shipment")]
        public string Shipment { get; set; }

        [EntityPropertyExtension("WaveReleaseTime", "WaveReleaseTime")]
        public string WaveReleaseTime { get; set; }
        [EntityPropertyExtension("WaveReleaseTime2", "WaveReleaseTime2")]
        public string WaveReleaseTime2 { get; set; }

        [EntityPropertyExtension("DeliverTime", "DeliverTime")]
        public string DeliverTime { get; set; }
        [EntityPropertyExtension("DeliverTime2", "DeliverTime2")]
        public string DeliverTime2 { get; set; }
        [EntityPropertyExtension("PickTime", "PickTime")]
        public string PickTime { get; set; }
        [EntityPropertyExtension("PickTime2", "PickTime2")]
        public string PickTime2 { get; set; }
        [EntityPropertyExtension("State", "State")]
        public string State { get; set; }


    }
}
