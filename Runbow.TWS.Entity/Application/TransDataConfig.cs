using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.Application
{
    public class TransDataConfig
    {
        [XmlAttribute("CustomerID")]
        public long CustomerID { get; set; }

        [XmlAttribute("DefaultTransDataInstance")]
        public string DefaultTransDataInstance { get; set; }

        [XmlElement("WarehouseConfig")]
        public List<WarehouseConfig> WarehouseConfigCollection { get; set; }
    }
}
