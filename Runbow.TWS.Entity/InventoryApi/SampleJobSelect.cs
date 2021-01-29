using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class SampleJobSelect
    {

        [EntityPropertyExtension("Date", "Date")]
        public string Date { get; set; }

        [EntityPropertyExtension("FWL", "FWL")]
        public int FWL { get; set; }

        [EntityPropertyExtension("EQP", "EQP")]
        public int EQP { get; set; }

        [EntityPropertyExtension("APP", "APP")]
        public int APP { get; set; }

        [EntityPropertyExtension("FWR", "FWR")]
        public int FWR { get; set; }

        [EntityPropertyExtension("Total", "Total")]
        public int Total { get; set; }
    }
}
