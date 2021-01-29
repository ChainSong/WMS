using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class LineItem
    {
        [XmlAttribute("Action")]
        public string Action { get; set; }
        [XmlElement("SaleForDelivery")]
        public SaleForDelivery SaleForDelivery { get; set; }
        [XmlElement("SequenceNumber")]
        public string SequenceNumber { get; set; }
    }
}
