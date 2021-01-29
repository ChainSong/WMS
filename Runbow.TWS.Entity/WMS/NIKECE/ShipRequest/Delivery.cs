using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class Delivery
    {
        [XmlElement("PreferredDateTime")]
        public string PreferredDateTime { get; set; }
        [XmlElement("DueDate")]
        public string DueDate { get; set; }
        [XmlElement("ExpectedShipmentDate")]
        public string ExpectedShipmentDate { get; set; }
    }
}
