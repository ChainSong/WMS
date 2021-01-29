using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    [XmlRoot("xml", Namespace = "http://www.nrf-arts.org/IXRetail/namespace/")]
    public class ItemMaster
    {
        [XmlAttribute("version")]
        public string version { get { return "1.0"; } set { } }
        [XmlAttribute("encoding")]
        public string encoding { get { return "UTF-8"; } set { } }

        [XmlElement("ItemMaintenance")]
        public ItemMaintenance ItemMaintenance { get; set; }
    }
}
