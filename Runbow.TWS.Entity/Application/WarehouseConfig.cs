using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity
{
    public class WarehouseConfig
    {
        [XmlAttribute("WarehouseID")]
        public long WarehouseID { get; set; }

        [XmlAttribute("AllocateInstance")]
        public string AllocateInstance { get; set; }

        [XmlAttribute("TransDataInstances")]
        public string TransDataInstances { get; set; }
    }
}
