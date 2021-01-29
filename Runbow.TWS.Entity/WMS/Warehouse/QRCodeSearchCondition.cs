using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class QRCodeSearchCondition
    {
        public long ID { get; set; }
        public long? CustomerID { get; set; }
        public long WarehouseID { get; set; }
        public long ProjectID { get; set; }
        public long UserID { get; set; }
        public string QRCode { get; set; }
    }
}
