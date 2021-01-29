using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class CustomerOrderTransaction
    {
        [XmlElement("SpecialOrderNumber")]
        public string SpecialOrderNumber { get; set; }
        [XmlElement("LineItem")]
        public List<LineItem> LineItem { get; set; }
        [XmlElement("Delivery")]
        public Delivery2 Delivery { get; set; }
        [XmlElement("Customer")]
        public Customer Customer { get; set; }
        [XmlElement("ReceiptDateTime")]
        public string ReceiptDateTime { get; set; }
    }
}
