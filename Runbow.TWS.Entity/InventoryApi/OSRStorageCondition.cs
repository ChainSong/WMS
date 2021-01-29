using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRStorageCondition
    {
        //入库
        public string PE { get; set; }

        public string TransOrderNO { get; set; }

        public string Status { get; set; }

        public string Category { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
       
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        //入库明细
        public string ReceiptNumber { get; set; }
        //门店调拨
        public string ShiptoCode { get; set; }
        //Export
        public string Identification { get; set; }
        //类型
        public string Type { get; set; }
       
        
    }
}
