using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class Delivery2
    {
        [XmlElement("CustomerID")]
        public string CustomerID { get; set; }

        [XmlElement("Name")]
        public Name Name { get; set; }

        [XmlElement("TelephoneNumber")]
        public TelephoneNumber TelephoneNumber { get; set; }

        [XmlElement("EMail")]
        public EMail EMail { get; set; }

        [XmlElement("Notes")]
        public string Notes { get; set; }

        [XmlElement("Courier")]
        public string Courier { get; set; }

        [XmlElement("PreferredLocation")]
        public PreferredLocation PreferredLocation { get; set; }
    }
}
