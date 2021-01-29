using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class POSLog
    {
        [XmlAttribute("FixVersion")]
        public string FixVersion { get; set; }
        [XmlAttribute("MajorVersion")]
        public string MajorVersion { get; set; }
        [XmlAttribute("MinorVersion")]
        public string MinorVersion { get; set; }
    }
}
