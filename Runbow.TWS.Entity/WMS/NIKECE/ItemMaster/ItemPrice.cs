using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    public class ItemPrice
    {
        [XmlAttribute("ValueTypeCode")]
        public string ValueTypeCode { get; set; }
        [XmlAttribute("Currency")]
        public string Currency { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
