﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class TelephoneNumber
    {
        [XmlAttribute("TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement("LocalNumber")]
        public string LocalNumber { get; set; }
    }
}
