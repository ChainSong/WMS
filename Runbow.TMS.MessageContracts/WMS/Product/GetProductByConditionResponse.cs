using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.MessageContracts.WMS.Product
{
    public class GetProductByConditionResponse
    {
        public IEnumerable<ProductStorer> SearchCondition { get; set; }

        public IEnumerable<Storer> Storer { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public int RowCount { get; set; }

    }
}
