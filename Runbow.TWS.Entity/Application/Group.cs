using System.Xml.Serialization;

namespace Runbow.TWS.Entity
{
    public class Group
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("IsHide")]
        public bool IsHide { get; set; }
    }
}