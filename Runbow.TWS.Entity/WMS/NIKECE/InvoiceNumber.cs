using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
    public class InvoiceNumber
    {
        [XmlAttribute("DateTime")]
        public DateTime DateTime;
         [XmlText]  
         public string Value { get; set; } 

    }
}