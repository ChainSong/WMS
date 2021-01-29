using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class POSLogGR
    {
        [XmlAttribute("xmlns")]
        public string xmlns
        {
            get
            {
                return "http://www.nrf-arts.org/IXRetail/namespace/";
            }
            set { }
        }
        [XmlAttribute("FixVersion")]
        public string FixVersion
        {
            get
            {
                return "0";
            }
            set { }
        }
        [XmlAttribute("MajorVersion")]
        public string MajorVersion
        {
            get
            {
                return "6";
            }
            set { }
        }
        [XmlAttribute("MinorVersion")]
        public string MinorVersion
        {
            get
            {
                return "0";
            }
            set { }
        }

    }
}
