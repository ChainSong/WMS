using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    public class Batch
    {
        [XmlElement("BatchID")]
        public string BatchID { get; set; }

        [XmlElement("Item")]
        public List<Item> Item { get; set; }
    }
}
