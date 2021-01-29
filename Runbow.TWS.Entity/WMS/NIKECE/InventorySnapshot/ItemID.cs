using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.InventorySnapshot
{
    public class ItemID
    {
        [XmlAttribute("Type")]
        public string Type { get; set; }
        [XmlAttribute("Qualifier")]
        public string Qualifier { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
