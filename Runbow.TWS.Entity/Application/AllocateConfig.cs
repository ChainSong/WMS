using System.Collections.Generic;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity
{
    public class AllocateConfig
    {
        [XmlAttribute("CustomerID")]
        public long CustomerID { get; set; }

        [XmlAttribute("DefaultAllocateInstance")]
        public string DefaultAllocateInstance { get; set; }

        [XmlElement("WarehouseConfig")]
        public List<WarehouseConfig> WarehouseConfigCollection { get; set; }
    }
}
