using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.Product
{
    public class AddProductExeclResponse
    {
        public ProductStorerInfo ProductStorerInfo { get; set; }
        public IEnumerable<ProductStorerInfo> Info { get; set; }
        public IEnumerable<ProductDetail> InfoDetail { get; set; }
    }
}
