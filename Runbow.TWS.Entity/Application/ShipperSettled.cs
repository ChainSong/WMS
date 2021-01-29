using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.Application
{
    public class ShipperSettled
    {
        [XmlAttribute("ShipperID")]
        public long ShipperID { get; set; }

        [XmlAttribute("SettledInstance")]
        public string SettledInstance { get; set; }

        [XmlAttribute("IsGroupedPods")]
        public bool IsGroupedPods { get; set; }

        [XmlAttribute("UseOldMethod")]
        public bool UseOldMethod { get; set; }
    }
}
