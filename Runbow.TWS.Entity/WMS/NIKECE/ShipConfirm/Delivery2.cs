using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipConfirm
{
    public class Delivery2
    {
        [XmlElement("Name")]
        public Name Name { get; set; }

        [XmlElement("ActualShipDateTime")]
        public string ActualShipDateTime { get; set; }
        [XmlElement("Courier")]
        public string Courier { get; set; }

        [XmlElement("PreferredLocation")]
        public PreferredLocation PreferredLocation { get; set; }
    }
}
