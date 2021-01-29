using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.POD.MaryKay
{
    public  class MaryKay_InterfaceLog
    {
        [EntityPropertyExtension("LogUID", "LogUID")]
        public string LogUID { get; set; }

        [EntityPropertyExtension("CreateDate", "CreateDate")]
        public string CreateDate { get; set; }

        [EntityPropertyExtension("LogDetails", "LogDetails")]
        public string LogDetails { get; set; }

        [EntityPropertyExtension("EventType", "EventType")]
        public string EventType { get; set; }

        [EntityPropertyExtension("UserDef1", "UserDef1")]
        public string UserDef1 { get; set; }

        [EntityPropertyExtension("UserDef2", "UserDef2")]
        public string UserDef2 { get; set; }

        [EntityPropertyExtension("UserDef3", "UserDef3")]
        public string UserDef3 { get; set; }

        [EntityPropertyExtension("UserDef4", "UserDef4")]
        public string UserDef4 { get; set; }

        [EntityPropertyExtension("UserDef5", "UserDef5")]
        public string UserDef5 { get; set; }
    }
}
