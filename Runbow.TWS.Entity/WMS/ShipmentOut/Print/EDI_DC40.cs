using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Entity.WMS.ShipmentOut.Print
{
    /// <summary>
    /// 报文头
    /// </summary>
    public class EDI_DC40
    {
        [XmlAttribute("SEGMENT")]
        public string SEGMENT
        {
            get;
            set;
        }

        public string DOCNUM
        {
            get; set;
        }
        public string IDOCTYP
        {
            get;
            set;
        }
        public string MESTYP
        {
            get;
            set;
        }
        public string SNDPOR
        {
            get;
            set;
        }
        public string SNDPRT
        {
            get;
            set;
        }
        public string SNDPRN
        {
            get;
            set;
        }
        public string RCVPOR
        {
            get;
            set;
        }
        public string RCVPRT
        {
            get;
            set;
        }
        public string RCVPRN
        {
            get;
            set;
        }
        public string REFINT
        {
            get;
            set;
        }
        public string REFGRP
        {
            get;
            set;
        }
        public string REFMES
        {
            get;
            set;
        }


    }
}
