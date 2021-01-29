using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE
{
      

   //[XmlAttribute(AttributeName="aa")]
    public class POSLog
    {


        [XmlAttribute("xmlns")]
        public string xmlns
        {
            get
            {
                return "http://www.w3.org/2001/XMLSchema";
            }
            set { }
        }

       [XmlAttribute("FixVersion")]
       public string FixVersion
       {
           get
           {
               return "0";
           }
           set { }
       }
       [XmlAttribute("MajorVersion")]
       public string MajorVersion
       {
           get
           {
               return "6";
           }
           set { }
       }
       [XmlAttribute("MinorVersion")]
       public string MinorVersion
       {
           get
           {
               return "0";
           }
           set{}
       }

       //[XmlElement("Transaction")]
       //public Transaction Transaction;
    }
}