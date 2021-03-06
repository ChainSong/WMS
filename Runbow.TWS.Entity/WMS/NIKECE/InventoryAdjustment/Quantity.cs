﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.InventoryAdjustment
{
    public class Quantity
    {
        [XmlAttribute("UnitOfMeasureCode")]
        public string UnitOfMeasureCode { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
