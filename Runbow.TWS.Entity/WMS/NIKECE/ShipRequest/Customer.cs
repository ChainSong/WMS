using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class Customer
    {
        [XmlElement("CustomerID")]
        public string CustomerID { get; set; }
        [XmlElement("Name")]
        public Name Name { get; set; }

        [XmlElement("Address")]
        public Address Address { get; set; }

        [XmlElement("TelephoneNumber")]
        public TelephoneNumber TelephoneNumber { get; set; }

        [XmlElement("EMail")]
        public EMail EMail { get; set; }

        [XmlElement("LocalRequirements")]
        public List<LocalRequirements> LocalRequirements { get; set; }
    }
}
