using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class CountryOfOrigin
    {
        [XmlAttribute("TypeCode")]
        public string TypeCode;

        [XmlText]
        public string Vlaue { get; set; }
    }
}
