using System.Collections.Generic;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.Application
{
    public class CustomerSettled
    {
        [XmlAttribute("RelatedCustomerID")]
        public long RelatedCustomerID { get; set; }

        [XmlAttribute("DefaultSettledForReceiveInstance")]
        public string DefaultSettledForReceiveInstance { get; set; }

        [XmlAttribute("UseOldReceiveMethod")]
        public bool UseOldReceiveMethod { get; set; }

        [XmlAttribute("IsGroupedPodsForReceive")]
        public bool IsGroupedPodsForReceive { get; set; }

        [XmlAttribute("DefaultSettledForPayInstance")]
        public string DefaultSettledForPayInstance { get; set; }

        [XmlAttribute("UseOldPayMethod")]
        public bool UseOldPayMethod { get; set; }

        [XmlAttribute("IsGroupedPodsForPay")]
        public bool IsGroupedPodsForPay { get; set; }

        [XmlElement("ShipperSettled")]
        public List<ShipperSettled> ShipperSettledCollection { get; set; }
    }
}
