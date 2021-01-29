using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    [XmlRoot("POSLog", Namespace = "http://www.nrf-arts.org/IXRetail/namespace/")]
    public class ShipRequest
    {
        [XmlElement("Transaction")]
        public Transaction Transaction;
    }
}
