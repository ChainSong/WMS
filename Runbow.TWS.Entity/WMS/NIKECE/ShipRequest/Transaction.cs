using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class Transaction
    {
        [XmlAttribute("CODFlag")]
        public string CODFlag { get; set; }
        [XmlAttribute("TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement("BusinessUnit")]
        public List<BusinessUnit> BusinessUnit { get; set; }

        [XmlElement("SequenceNumber")]
        public string SequenceNumber { get; set; }
        [XmlElement("TransactionID")]
        public string TransactionID { get; set; }
        [XmlElement("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [XmlElement("CustomerOrderTransaction")]
        public CustomerOrderTransaction CustomerOrderTransaction { get; set; }

        [XmlElement("ControlTransaction")]
        public ControlTransaction ControlTransaction { get; set; }
    }
}
