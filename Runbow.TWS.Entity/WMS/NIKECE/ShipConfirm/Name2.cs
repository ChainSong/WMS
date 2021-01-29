using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipConfirm
{
    public class Name2
    {
        [XmlAttribute("TypeCode")]
        public string TypeCode { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
