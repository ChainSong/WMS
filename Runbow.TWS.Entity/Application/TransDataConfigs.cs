using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.Application
{
    public class TransDataConfigs
    {
        [XmlElement("TransDataConfig")]
        public List<TransDataConfig> TransDataConfigCollection { get; set; }
    }
}
