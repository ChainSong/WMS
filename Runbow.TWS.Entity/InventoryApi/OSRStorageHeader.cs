using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRStorageHeader
    {
        //OSR入库
       [EntityPropertyExtension("ID", "ID")]
       public int ID { get; set; }

       [EntityPropertyExtension("TransOrderNO", "TransOrderNO")]
       public string TransOrderNO { get; set; }

        [EntityPropertyExtension("ReceiptKey", "ReceiptKey")]
        public string ReceiptKey { get; set; }

        [EntityPropertyExtension("ETA", "ETA")]
        public DateTime? ETA { get; set; }

        [EntityPropertyExtension("Total", "Total")]
        public int Total { get; set; }

        [EntityPropertyExtension("ReceiveDate", "ReceiveDate")]
        public DateTime? ReceiveDate { get; set; }

        [EntityPropertyExtension("UpdateDate", "UpdateDate")]
        public DateTime? UpdateDate { get; set; }

        [EntityPropertyExtension("ProcessingTime", "ProcessingTime")]
        public int ProcessingTime { get; set; }

        [EntityPropertyExtension("STATUS", "STATUS")]
        public int STATUS { get; set; }

        [EntityPropertyExtension("Company", "Company")]
        public string Company { get; set; }

        [EntityPropertyExtension("PE", "PE")]
        public string PE { get; set; }

       [EntityPropertyExtension("ShiptoCode", "ShiptoCode")]
        public string ShiptoCode { get; set; }
       


        
        //APP、FWL
    }
}
