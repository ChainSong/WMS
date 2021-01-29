using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.InventorySnapshot
{
    [XmlRoot("Inventory", Namespace = "http://www.nrf-arts.org/IXRetail/namespace/")]
    public class InventorySnapshot
    {        
        [XmlAttribute("MajorVersion")]
        public string MajorVersion { get { return "2"; } set { } }
        [XmlAttribute("MinorVersion")]
        public string MinorVersion { get { return "0"; } set { } }
        [XmlAttribute("FixVersion")]
        public string FixVersion { get { return "0"; } set { } }

        [XmlAttribute("ActionCode")]
        public string ActionCode { get { return "Replace"; } set { } }
        [XmlAttribute("MessageType")]
        public string MessageType { get { return "Publish"; } set { } }

        [XmlElement("MessageID")]
        public MessageID MessageID { get; set; }

        [XmlElement("InventoryPosition")]
        public InventoryPosition InventoryPosition { get; set; }
    }
}
