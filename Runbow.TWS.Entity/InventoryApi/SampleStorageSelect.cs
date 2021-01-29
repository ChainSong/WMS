using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class SampleStorageSelect
    {

       [EntityPropertyExtension("Ponumber", "Ponumber")]
       public string Ponumber { get; set; }

       [EntityPropertyExtension("SKU", "SKU")]
       public string SKU { get; set; }

       [EntityPropertyExtension("Size", "Size")]
       public string Size { get; set; }

       [EntityPropertyExtension("PE", "PE")]
       public string PE { get; set; }

       [EntityPropertyExtension("Gender", "Gender")]
       public string Gender { get; set; }

       [EntityPropertyExtension("Category", "Category")]
       public string Category { get; set; }

       [EntityPropertyExtension("SKUDesc", "SKUDesc")]
       public string SKUDesc { get; set; }

       [EntityPropertyExtension("SilhouetteDesc", "SilhouetteDesc")]
       public string SilhouetteDesc { get; set; }

       [EntityPropertyExtension("Season", "Season")]
       public string Season { get; set; }

       [EntityPropertyExtension("ExpectedQty", "ExpectedQty")]
       public int ExpectedQty { get; set; }

       [EntityPropertyExtension("RcvQty", "RcvQty")]
       public int RcvQty { get; set; }

       [EntityPropertyExtension("Variance", "Variance")]
       public int Variance { get; set; }

       [EntityPropertyExtension("RcvDate", "RcvDate")]
       public string RcvDate { get; set; }

       //FOB、零售价钱导入

       
      
    }
}
