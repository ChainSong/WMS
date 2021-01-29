using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Gadget.Model
{
   public class MBGMCR02
    {
       [XmlElement("IDOC")]
       public IDOC IDOC { get; set; }
    }
}
