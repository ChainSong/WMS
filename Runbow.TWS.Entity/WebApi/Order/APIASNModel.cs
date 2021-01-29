using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class APIASNModel
    { 
        /// <summary>
        ///  Request Header
        /// </summary>
        [EntityPropertyExtension("MsgHeader", "MsgHeader")]
        public APIAndBackSetting MsgHeader { get; set; }

        [EntityPropertyExtension("MsgBody", "MsgBody")]
        public APIASNBody MsgBody { get; set; }
       
    }
}
