using System.Collections.Generic;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity
{
    public class Tables
    {
        [XmlElement("Table")]
        public List<Table> TableCollection { get; set; }
    }
}