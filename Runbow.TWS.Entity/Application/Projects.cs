using System.Collections.Generic;
using System.Xml.Serialization;
using Runbow.TWS.Entity.Application;

namespace Runbow.TWS.Entity
{
    [XmlRoot("Projects")]
    public class Projects
    {
        [XmlElement("Project")]
        public List<Project> ProjectCollection { get; set; }
    }
}