using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Instructions
{
    public class PickUpGoodsWall
    {
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long? WarehouseID { get; set; }
        [EntityPropertyExtension("OperationID", "OperationID")]
        public long? OperationID { get; set; }
        [EntityPropertyExtension("Rows", "Rows")]
        public int Rows { get; set; }
        [EntityPropertyExtension("Cells", "Cells")]
        public int Cells { get; set; }

        [EntityPropertyExtension("RowNumber", "RowNumber")]
        public int RowNumber { get; set; }
        [EntityPropertyExtension("CellNumber", "CellNumber")]
        public int CellNumber { get; set; }

        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }

        [EntityPropertyExtension("InstructionsID", "InstructionsID")]
        public string InstructionsID { get; set; }

        [EntityPropertyExtension("RePickWallDetailID", "RePickWallDetailID")]
        public string RePickWallDetailID { get; set; }
    }
}
