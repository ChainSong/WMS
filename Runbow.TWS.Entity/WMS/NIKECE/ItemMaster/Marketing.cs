using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ItemMaster
{
    public class Marketing
    {
        [XmlElement("AgeGroup")]
        public string AgeGroup { get; set; }
        [XmlElement("Gender")]
        public string Gender { get; set; }
        [XmlElement("LifestyleID")]
        public string LifestyleID { get; set; }
    }
}
