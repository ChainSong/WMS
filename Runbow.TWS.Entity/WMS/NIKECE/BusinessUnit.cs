using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class BusinessUnit
    {
        [XmlElement("UnitID")]
        public UnitID UnitID;
    }
}