using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Runbow.TWS.Gadget.Model
{
    public class EDI_DC40
    {
        [XmlAttribute("SEGMENT")]
        public string SEGMENT
        {
            get;
            set;
        }
        public string TABNAM
        {
            get;
            set;
        }

        public string MANDT
        {
            get;
            set;
        }
        public string DOCNUM
        {
            get;
            set;
        }
        public string DOCREL
        {
            get;
            set;
        }
        public string STATUS
        {
            get;
            set;
        }
        public string DIRECT
        {
            get;
            set;
        }
        public string OUTMOD
        {
            get;
            set;
        }
        public string EXPRSS
        {
            get;
            set;
        }
        public string TEST
        {
            get;
            set;
        }
        public string IDOCTYP
        {
            get;
            set;
        }
        public string CIMTYP
        {
            get;
            set;
        }
        public string MESTYP
        {
            get;
            set;
        }
        public string MESCOD
        {
            get;
            set;
        }
        public string MESFCT
        {
            get;
            set;
        }
        public string STD
        {
            get;
            set;
        }
        public string STDVRS
        {
            get;
            set;
        }
        public string STDMES
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
        public string SNDPFC
        {
            get;
            set;
        }
        public string SNDPRN
        {
            get;
            set;
        }
        public string SNDSAD
        {
            get;
            set;
        }
        public string SNDLAD
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
        public string RCVPFC
        {
            get;
            set;
        }
        public string RCVPRN
        {
            get;
            set;
        }
        public string RCVSAD
        {
            get;
            set;
        }

        public string CREDAT
        {
            get;
            set;
        }
        public string CRETIM
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
        public string ARCKEY
        {
            get;
            set;
        }
        public string SERIAL
        {
            get;
            set;
        }
        public string RCVLAD
        {
            get;
            set;
        }
    }
}
