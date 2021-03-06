﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class MerchandiseHierarchy
    {
        [XmlAttribute("Level")]
        public string Level { get; set; }
        [XmlAttribute("ID")]
        public string ID { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
