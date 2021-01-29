using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.InventorySnapshot
{
    public class InventoryPosition
    {
        [XmlElement("BusinessUnit")]
        public BusinessUnit BusinessUnit { get; set; }

        [XmlElement("Item")]
        public List<Item> Item { get; set; }
    }
}
