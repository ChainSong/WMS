using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipConfirm
{
    public class Delivery
    {
        [XmlElement("TrackingNumber")]
        public string TrackingNumber { get; set; }
        [XmlElement("Quantity")]
        public string Quantity { get; set; }
    }
}
