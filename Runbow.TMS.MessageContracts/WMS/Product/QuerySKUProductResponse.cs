using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.MessageContracts.WMS.Product
{
    public class QuerySKUProductResponse
    {
        public ProductStorerInfo ProductStorerInfo { get; set; }
        public IEnumerable<ProductStorerInfo> Info { get; set; }
        public IEnumerable<ProductDetail> InfoDetail { get; set; }
        public IEnumerable<WMS_ArticleDetail> ArticleDetail { get; set; }
    }
}
