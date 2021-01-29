using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.ShipmentOut.Print
{
    public class IDOC
    {
        /// <summary>
        /// IDOC下的属性
        /// </summary>
        [XmlAttribute("BEGIN")]
        public string BEGIN
        {
            get;
            set;
        }

        /// <summary>
        /// 报文头
        /// </summary>
        [XmlElement("EDI_DC40")]
        public EDI_DC40 EDI_DC40
        {
            get;
            set;
        }

        /// <summary>
        /// 运单头
        /// </summary>
        [XmlElement("E1BPSHIPMENTHEADER")]
        public E1BPSHIPMENTHEADER E1BPHEADER
        {
            get; set;
        }

        /// <summary>
        /// 运单明细  可以包含多个DN
        /// </summary>
        [XmlElement("E1BPSHIPMENTITEM")]
        public List<E1BPSHIPMENTITEM> E1BPITEMList
        {
            get; set;
        }
    }
}
