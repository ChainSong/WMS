using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.InventoryAdjustment
{
    public class ItemLocation
    {
        [XmlElement("ItemID")]
        public ItemID ItemID { get; set; }

        [XmlElement("InventoryLocation")]
        public InventoryLocation InventoryLocation { get; set; }

        [XmlElement("Quantity")]
        public Quantity Quantity { get; set; }

        [XmlElement("DateTime")]
        public DateTime1 DateTime1 { get; set; }
    }
}
