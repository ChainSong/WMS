using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Gadget.Model
{
    public class E1BP2017_GM_HEAD_01
    {
        [XmlAttribute("SEGMENT")]
        public string SEGMENT
        {
            get;
            set;
        }
        [XmlElement("PSTNG_DATE")]
        public string PSTNG_DATE { get; set; }

        [XmlElement("DOC_DATE")]
        public string DOC_DATE { get; set; }

        [XmlElement("REF_DOC_NO")]
        public string REF_DOC_NO { get; set; }

        [XmlElement("EXT_WMS")]
        public string EXT_WMS { get; set; }

        [XmlElement("HEADER_TXT")]
        public string HEADER_TXT { get; set; }
        

        
    }
}
