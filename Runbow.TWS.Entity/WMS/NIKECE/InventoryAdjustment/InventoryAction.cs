using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.InventoryAdjustment
{
    public class InventoryAction
    {
        [XmlElement("DateTime")]
        public DateTime1 DateTime1 { get; set; }

        [XmlElement("InventoryItem")]
        public InventoryItem InventoryItem { get; set; }
    }
}
