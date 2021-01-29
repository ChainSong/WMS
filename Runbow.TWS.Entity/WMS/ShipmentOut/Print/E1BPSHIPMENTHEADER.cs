using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.ShipmentOut.Print
{
    /// <summary>
    /// shipment print的头部
    /// </summary>
    public class E1BPSHIPMENTHEADER
    {
        [XmlAttribute("SEGMENT")]
        public string SEGMENT
        {
            get;
            set;
        }

        public string SJIPMENT_TYPE
        {
            get;
            set;
        }
        public string TRANS_PLAN_PT
        {
            get;
            set;
        }
        public string STATUS_PLAN
        {
            get;
            set;
        }
        public string STATUS_LOAD_END
        {
            get;
            set;
        }
        public string STATUS_COMPL
        {
            get;
            set;
        }


    }
}
