using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRThelibraryCondition
    {
        //OSR出库
       public string Category { get; set; }

       public string TransOrderNO { get; set; }

       public string PE { get; set; }

       public string DeliveryStore { get; set; }

       public string ShiptoCode { get; set; }

       //public DateTime? ATABegin { get; set; }

       //public DateTime? ATAEnd { get; set; }

       public DateTime? BeginTime { get; set; }

       public DateTime? EndTime { get; set; }

       public int PageIndex { get; set; }

       public int PageSize { get; set; }

        //OSR退货
       public string SKU { get; set; }

       public string UPC { get; set; }

       public string Season { get; set; }

       public string Article { get; set; }

       //Detailed
       public string OrderKey { get; set; }

       //Export
       public string Identification { get; set; }

       public string Type { get; set; }
    }
}
