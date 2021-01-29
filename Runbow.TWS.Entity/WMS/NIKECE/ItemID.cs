using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class ItemID
    {
        [XmlAttribute("Type")]
        public string Type;        

        [XmlAttribute("Qualifier")]
        public string Qualifier;

        [XmlAttribute("Name")]
        public string Name;

        [XmlText]
        public string Value { get; set; } 
    }
}
