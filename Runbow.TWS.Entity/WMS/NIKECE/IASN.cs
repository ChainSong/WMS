using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    [XmlRoot("POSLog",Namespace = "http://www.nrf-arts.org/IXRetail/namespace/")]
    public class IASN :POSLog
    {
        [XmlElement("Transaction")]
        public Transaction Transaction;
    }
}