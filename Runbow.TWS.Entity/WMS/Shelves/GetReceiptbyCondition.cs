using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R= Runbow.TWS.Entity;
using Runbow.TWS.Entity;
namespace Runbow.TWS.Entity.WMS.Shelves
{
    [Serializable]
    public class GetReceiptbyCondition : R.Receipt
    {

        //public string ExternReceiptNumber { get; set; }
        //public string ReceiptNumber { get; set; }
        public int? ShelvesState { get; set; }
        public long? StorerID { get; set; }
        public string CustomerIDs { get; set; }
        //public string WarehouseID { get; set; }
        public DateTime? StartStorageTime{ get; set; }
        public DateTime? EndStorageTime { get; set; }
        public DateTime? StartShelvesTime { get; set; }
        public DateTime? EndShelvesTime { get; set; }
        public string WarehouseIDs { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
    }
}
