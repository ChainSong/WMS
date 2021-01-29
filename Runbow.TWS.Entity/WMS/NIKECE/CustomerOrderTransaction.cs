
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class CustomerOrderTransaction
    {
        [XmlElement("ItemCount")]
        public string ItemCount;

        [XmlElement("LineItem")]
        public List<LineItem> LineItem;

        [XmlElement("Customer")]
        public Customer Customer;
    }
}