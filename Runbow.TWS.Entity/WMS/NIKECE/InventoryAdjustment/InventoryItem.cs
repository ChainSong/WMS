using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.InventoryAdjustment
{
    public class InventoryItem
    {
        [XmlAttribute("State")]
        public string State { get; set; }

        [XmlElement("ItemLocation")]
        public ItemLocation ItemLocation { get; set; }
    }
}
