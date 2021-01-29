using Runbow.TWS.Entity.WMS.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.IntelligentWarehouse
{
    public class PickUpGoodsManagementRequest
    {
        public string WorkStationId { get; set; }
        public string WorkStationName { get; set; }
        public int Ststus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerID { get; set; }
        public string Warehouse { get; set; }
        public string WarehouseId { get; set; }
        public string OrderNumber { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
