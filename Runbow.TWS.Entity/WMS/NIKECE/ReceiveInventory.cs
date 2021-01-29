using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class ReceiveInventory
    {
        [XmlAttribute("DocumentType")]
        public string DocumentType;

        [XmlElement("DocumentID")]
        public string DocumentID;

        [XmlElement("LineItem")]
        public List<LineItemGR> LineItem;
    }
}
