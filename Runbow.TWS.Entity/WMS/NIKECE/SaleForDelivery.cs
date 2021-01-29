using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class SaleForDelivery
    {
        [XmlElement("ItemID")]
        public ItemID ItemID;

        [XmlElement("CountryOfOrigin")]
        public CountryOfOrigin CountryOfOrigin;

        [XmlElement("Quantity")]
        public Quantity Quantity;

        [XmlElement("Parent")]
        public List<Parent> Parent;

        [XmlElement("Delivery")]
        public Delivery Delivery;
    }
}
