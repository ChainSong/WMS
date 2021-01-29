using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.ShipmentOut.Print
{
    /// <summary>
    /// 发送的shipment print 的DO单明细
    /// </summary>
    public class E1BPSHIPMENTITEM
    {
        [XmlAttribute("SEGMENT")]
        public string SEGMENT
        {
            get;
            set;
        }

        public string DELIVERY
        {
            get;
            set;
        }

    }
}
