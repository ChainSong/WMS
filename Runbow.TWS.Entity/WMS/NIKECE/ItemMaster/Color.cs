using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    public class Color
    {
        [XmlElement("Description")]
        public Description Description { get; set; }
        [XmlElement("Code")]
        public string Code { get; set; }
    }
}
