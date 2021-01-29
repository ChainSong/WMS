using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.InventorySnapshot
{
    public class InventoryAction
    {
        [XmlAttribute("ActionCode")]
        public string ActionCode { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlElement("DateTime")]
        public DateTime1 DateTime1 { get; set; }

    }
}
