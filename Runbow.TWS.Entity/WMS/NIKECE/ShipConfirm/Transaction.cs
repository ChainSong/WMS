using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipConfirm
{
    public class Transaction
    {
        [XmlElement("BusinessUnit")]
        public List<BusinessUnit> BusinessUnit { get; set; }
        [XmlElement("TransactionID")]
        public string TransactionID { get; set; }
        [XmlElement("POSLogDateTime")]
        public POSLogDateTime POSLogDateTime { get; set; }
        [XmlElement("CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlElement("TrailerText")]
        public TrailerText TrailerText { get; set; }

        [XmlElement("ReceiptNumber")]
        public string ReceiptNumber { get; set; }

        [XmlElement("InvoiceNumber")]
        public InvoiceNumber InvoiceNumber { get; set; }

        [XmlElement("CustomerOrderTransaction")]
        public CustomerOrderTransaction CustomerOrderTransaction { get; set; }
    }
}
