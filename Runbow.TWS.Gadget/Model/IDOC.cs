using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Gadget.Model
{
   public class IDOC
    {
       [XmlAttribute("BEGIN")]
       public string BEGIN
       {
           get;
           set;
       }
       [XmlElement("EDI_DC40")]
       public EDI_DC40 EDI_DC40 { get; set; }

       [XmlElement("E1BP2017_GM_HEAD_01")]
       public E1BP2017_GM_HEAD_01 E1BP2017_GM_HEAD_01 { get; set; }

       [XmlElement("E1BP2017_GM_CODE")]
       public E1BP2017_GM_CODE E1BP2017_GM_CODE { get; set; }

       [XmlElement("E1BP2017_GM_ITEM_CREATE")]
       public List<E1BP2017_GM_ITEM_CREATE> E1BP2017_GM_ITEM_CREATE { get; set; } 
 
    }
}
