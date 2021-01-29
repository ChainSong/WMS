using System.Collections.Generic;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.Application
{
    public class Project
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("Module")]
        public List<Module> ModuleCollection { get; set; }

        [XmlElement("CalculateMethod")]
        public string CalculateMethod { get; set; }

        [XmlElement("PODNumberCreator")]
        public string PODNumberCreator { get; set; }

        [XmlElement("SettledPodConfigs")]
        public SettledPodConfigs SettledPodConfigs { get; set; }

        [XmlElement("AllocateConfigs")]
        public AllocateConfigs AllocateConfigs { get; set; }

        [XmlElement("SettledConfig")]
        public SettledConfig SettledConfig { get; set; }

        [XmlElement("TransDataConfigs")]
        public TransDataConfigs TransDataConfigs { get; set; }
    }
}