using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    public class Pack
    {
        [XmlElement("UnitNumberCount")]
        public string UnitNumberCount { get; set; }
        [XmlElement("PackVolume")]
        public string PackVolume { get; set; }
        [XmlElement("PackWeight")]
        public string PackWeight { get; set; }
    }
}
