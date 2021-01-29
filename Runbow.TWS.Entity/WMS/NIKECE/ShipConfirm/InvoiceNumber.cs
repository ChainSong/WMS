using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipConfirm
{
    public class InvoiceNumber
    {
        [XmlAttribute("DateTime")]
        public DateTime DateTime;
        [XmlText]
        public string Value { get; set; } 
    }
}
