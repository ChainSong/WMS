using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class LocalRequirements
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Value")]
        public string Value { get; set; }
    }
}
