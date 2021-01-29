using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{

    public class Transaction 
    {
        [XmlElement("TransactionID")]
        public string TransactionID;

        [XmlElement("BusinessUnit")]
        public List<BusinessUnit> BusinessUnit;

        [XmlElement("TrailerText")]
        public TrailerText TrailerText;

        [XmlElement("InvoiceNumber")]
        public InvoiceNumber InvoiceNumber;

        [XmlElement("POSLogDateTime")]
        public POSLogDateTime POSLogDateTime;

        public CustomerOrderTransaction CustomerOrderTransaction;
    }
}