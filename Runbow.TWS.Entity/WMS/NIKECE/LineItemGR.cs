using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class LineItemGR
    {
        [XmlElement("ItemID")]
        public ItemID ItemID;

        [XmlElement("QuantityOrdered")]
        public string QuantityOrdered;

        [XmlElement("QuantityReceived")]
        public string QuantityReceived;

        [XmlElement("LineItemNumber")]
        public string LineItemNumber;

        [XmlElement("DiscrepancyReason")]
        public string DiscrepancyReason;
    }
}
