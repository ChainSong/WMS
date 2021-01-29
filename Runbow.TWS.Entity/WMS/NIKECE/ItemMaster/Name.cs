using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    public class Name
    {
        [XmlAttribute("Language")]
        public string Language { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
