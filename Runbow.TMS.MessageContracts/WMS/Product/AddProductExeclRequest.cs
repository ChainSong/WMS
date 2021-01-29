using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.MessageContracts.WMS.Product
{
    public class AddProductExeclRequest
    {
        public IEnumerable<ProductStorerInfo> Info { get; set; }

        public IEnumerable<ProductDetail> InfoDetail { get; set; }

        public string UserName { get; set; }
    }
}
