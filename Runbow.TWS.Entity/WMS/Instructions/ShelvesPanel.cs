using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Instructions
{
    public class ShelvesPanel
    {
        [EntityPropertyExtension("Levels", "Levels")]
        public int Levels { get; set; }
        [EntityPropertyExtension("RowNumber", "RowNumber")]
        public int RowNumber { get; set; }
        [EntityPropertyExtension("CellNumber", "CellNumber")]
        public int CellNumber { get; set; }
        //[EntityPropertyExtension("Levels", "Levels")]
        //public int Levels { get; set; }
        //[EntityPropertyExtension("Grids", "Grids")]
        //public int Grids { get; set; }
        //[EntityPropertyExtension("LocationID", "LocationID")]
        //public string LocationID { get; set; }
        //[EntityPropertyExtension("LevelsNumber", "LevelsNumber")]
        //public int LevelsNumber { get; set; }
        //[EntityPropertyExtension("GridsNumber", "GridsNumber")]
        //public int GridsNumber { get; set; }
        //[EntityPropertyExtension("SKU", "SKU")]
        //public string SKU { get; set; }
        //[EntityPropertyExtension("GoodsName", "GoodsName")]
        //public string GoodsName { get; set; }    
        //[EntityPropertyExtension("TotalQty", "TotalQty")]
        //public decimal TotalQty { get; set; }   
        //[EntityPropertyExtension("Qty", "Qty")]
        //public decimal Qty { get; set; }    
        //[EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        //public string ExternOrderNumber { get; set; }
    }
}
