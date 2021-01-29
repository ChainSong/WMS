using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class ASNDetailLocation:ASNDetail
    {
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }
    }
}
