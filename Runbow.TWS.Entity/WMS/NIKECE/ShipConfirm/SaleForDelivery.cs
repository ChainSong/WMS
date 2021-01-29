using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipConfirm
{
    public class SaleForDelivery
    {
        [XmlElement("ItemID")]
        public List<ItemID> ItemID { get; set; }

        [XmlElement("SpecialOrderNumber")]
        public string SpecialOrderNumber { get; set; }

        [XmlElement("LotNumber")]
        public string LotNumber { get; set; }
        [XmlElement("UnitListPrice")]
        public string UnitListPrice { get; set; }

        [XmlElement("Quantity")]
        public string Quantity { get; set; }

        [XmlElement("Delivery")]
        public Delivery Delivery { get; set; }
    }
}
