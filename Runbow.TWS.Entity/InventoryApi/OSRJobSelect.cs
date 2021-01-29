using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRJobSelect
    {

        [EntityPropertyExtension("Date", "Date")]
        public string Date { get; set; }

        [EntityPropertyExtension("APPQTY", "APPQTY")]
        public int APPQTY { get; set; }

        [EntityPropertyExtension("FTWQTY", "FTWQTY")]
        public int FTWQTY { get; set; }

        [EntityPropertyExtension("EQPQTY", "EQPQTY")]
        public int EQPQTY { get; set; }

        [EntityPropertyExtension("Total", "Total")]
        public int Total { get; set; }

        

    }
}
