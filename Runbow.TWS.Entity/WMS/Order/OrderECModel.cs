using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
  public class OrderECModel
    {
        public IEnumerable<Order_Header> Order_Headers { get; set; }
        public IEnumerable<Order_Detail> Order_Details { get; set; }
        public IEnumerable<Order_Express> Order_ExpresList { get; set; }
    }
}
