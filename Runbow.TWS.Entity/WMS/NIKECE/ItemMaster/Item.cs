using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    public class Item
    {
        [XmlAttribute("Action")]
        public string Action { get; set; }
        [XmlAttribute("ItemCategory")]
        public string ItemCategory { get; set; }
        [XmlAttribute("StatusCode")]
        public string StatusCode { get; set; }

        [XmlElement("ItemID")]
        public ItemID ItemID { get; set; }

        [XmlElement("Name")]
        public Name Name { get; set; }

        [XmlElement("MerchandiseHierarchy")]
        public List<MerchandiseHierarchy> MerchandiseHierarchy { get; set; }

        [XmlElement("Dates")]
        public Dates Dates { get; set; }

        [XmlElement("Color")]
        public Color Color { get; set; }

        [XmlElement("ItemPrice")]
        public ItemPrice ItemPrice { get; set; }

        [XmlElement("Marketing")]
        public Marketing Marketing { get; set; }
        [XmlElement("Pack")]
        public Pack Pack { get; set; }
        [XmlElement("Size")]
        public Size Size { get; set; }
        [XmlElement("StyleID")]
        public Style Style { get; set; }

        [XmlElement("SupplierInformation")]
        public SupplierInformation SupplierInformation { get; set; }
    }
}
