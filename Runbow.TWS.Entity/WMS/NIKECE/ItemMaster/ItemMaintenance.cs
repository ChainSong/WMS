using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    [XmlRoot("ItemMaintenance", Namespace = "http://www.nrf-arts.org/IXRetail/namespace/")]
    public class ItemMaintenance
    {
        [XmlElement("BusinessUnit")]
        public string BusinessUnit { get; set; }

        [XmlElement("Batch")]
        public Batch Batch { get; set; }
    }
}
