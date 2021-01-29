using System.Collections.Generic;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity
{
    public class AllocateConfigs
    {
        [XmlElement("AllocateConfig")]
        public List<AllocateConfig> AllocateConfigCollection { get; set; }
    }
}
