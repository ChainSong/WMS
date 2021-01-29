using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class ControlTransaction
    {
        [XmlElement("BusinessSOD")]
        public BusinessSOD BusinessSOD { get; set; }
    }
}
