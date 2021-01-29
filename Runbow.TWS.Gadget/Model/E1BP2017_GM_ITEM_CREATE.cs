using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Gadget.Model
{
    public class E1BP2017_GM_ITEM_CREATE
    {
         
        [XmlAttribute("SEGMENT")]
        public string SEGMENT
        {
            get;
            set;
        }
        [XmlElement("MATERIAL")]
        public string MATERIAL { get; set; }

        [XmlElement("PLANT")]
        public string PLANT { get; set; }

        [XmlElement("STGE_LOC")]
        public string STGE_LOC { get; set; }
        [XmlElement("MOVE_TYPE")]
        public string MOVE_TYPE { get; set; }
        [XmlElement("SPEC_STOCK")]
        public string SPEC_STOCK { get; set; }
        [XmlElement("ENTRY_QNT")]
        public string ENTRY_QNT { get; set; }

        [XmlElement("MOVE_STLOC")]
        public string MOVE_STLOC { get; set; }

        [XmlElement("VAL_SALES_ORD")]
        public string VAL_SALES_ORD { get; set; }

        [XmlElement("VAL_S_ORD_ITEM")]
        public string VAL_S_ORD_ITEM { get; set; }

        

        

        


        [XmlElement("COSTCENTER")]
        public string COSTCENTER { get; set; }

        [XmlElement("MOVE_REAS")]
        public string MOVE_REAS { get; set; }

        [XmlElement("NO_PST_CHGNT")]
        public string NO_PST_CHGNT { get; set; }

        [XmlElement("NO_TRANSFER_REQ")]
        public string NO_TRANSFER_REQ { get; set; }
    }
}
