using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class TransactionGR
    {
        [XmlElement("BusinessUnit")]
        public List<BusinessUnit> BusinessUnit;

        [XmlElement("TransactionID")]
        public string TransactionID;

        [XmlElement("TrailerText")]
        public TrailerText TrailerText;

        [XmlElement("ReceiptNumber")]
        public string ReceiptNumber;

        [XmlElement("InvoiceNumber")]
        public InvoiceNumber InvoiceNumber;

        public InventoryControlTransaction InventoryControlTransaction;
    }
}
