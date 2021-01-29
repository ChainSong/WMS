using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipConfirm
{
    public class Name
    {
        [XmlElement("Name")]
        public List<Name2> Name2 { get; set; }


    }
}
