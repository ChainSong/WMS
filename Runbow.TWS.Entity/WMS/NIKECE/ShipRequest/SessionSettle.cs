using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class SessionSettle
    {
        [XmlElement("TenderSummary")]
        public TenderSummary TenderSummary { get; set; }
    }
}
