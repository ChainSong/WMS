using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.Application
{
    public class SettledConfig
    {
        [XmlAttribute("DefaultSelltedForReceiveInstance")]
        public string DefaultSelltedForReceiveInstance { get; set; }

        [XmlAttribute("DefaultSettledToPayInstance")]
        public string DefaultSettledToPayInstance { get; set; }

        [XmlElement("CustomerSettled")]
        public List<CustomerSettled> CustomerSettledCollection { get; set; }
    }
}
