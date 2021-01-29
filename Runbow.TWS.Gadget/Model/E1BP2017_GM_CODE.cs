using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Gadget.Model
{
    public class E1BP2017_GM_CODE
    {

        [XmlAttribute("SEGMENT")]
        public string SEGMENT
        {
            get;
            set;
        }
        [XmlElement("GM_CODE")]
        public string GM_CODE { get; set; }
    }
}
