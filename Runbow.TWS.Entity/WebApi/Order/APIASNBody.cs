using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class APIASNBody
    { 
        /// <summary>
        ///  Request Header
        /// </summary>
        [EntityPropertyExtension("ASN", "ASN")]
        public List<ASN> ASN { get; set; }

        [EntityPropertyExtension("ASNDetail", "ASNDetail")]
        public List<ASNDetail> ASNDetail { get; set; }
       
    }
}
