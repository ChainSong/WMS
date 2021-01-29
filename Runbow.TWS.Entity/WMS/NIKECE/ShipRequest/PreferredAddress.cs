using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class PreferredAddress
    {
        [XmlAttribute("PrimaryFlag")]
        public string PrimaryFlag { get; set; }
        [XmlAttribute("AddressType")]
        public string AddressType { get; set; }

        [XmlElement("City")]
        public string City { get; set; }

        [XmlElement("AddressLine")]
        public AddressLine AddressLine { get; set; }

        [XmlElement("Territory")]
        public List<Territory> Territory { get; set; }

        [XmlElement("PostalCode")]
        public string PostalCode { get; set; }

        [XmlElement("Country")]
        public string Country { get; set; }
    }
}
