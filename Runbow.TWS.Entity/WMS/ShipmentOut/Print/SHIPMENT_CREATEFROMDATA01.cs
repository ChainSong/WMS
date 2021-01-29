using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.ShipmentOut.Print
{
    //生成需要反馈的shipment(Print)根节点
    [XmlRoot("SHIPMENT_CREATEFROMDATA01")]
    public class SHIPMENT_CREATEFROMDATA01
    {
        [XmlElement("IDOC")]
        public IDOC IDOC { get; set; }
    }
}
