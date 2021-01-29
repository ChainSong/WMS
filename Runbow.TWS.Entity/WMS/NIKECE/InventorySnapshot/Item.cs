using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.InventorySnapshot
{
    public class Item
    {
        [XmlAttribute("State")]
        public string State { get; set; }

        [XmlElement("ItemID")]
        public ItemID ItemID { get; set; }
        [XmlElement("Quantity")]
        public Quantity Quantity { get; set; }
    }
}
