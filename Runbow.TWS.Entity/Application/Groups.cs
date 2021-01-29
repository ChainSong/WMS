using System.Collections.Generic;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity
{
    public class Groups
    {
        [XmlElement("Group")]
        public List<Group> GroupCollection { get; set; }
    }
}