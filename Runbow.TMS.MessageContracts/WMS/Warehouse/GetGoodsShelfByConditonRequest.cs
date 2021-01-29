using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetGoodsShelfByConditonRequest
    {
        public GoodsShelfSearchCondition SearchCondition { get; set; }

        public IEnumerable<GoodsShelfInfo> GoodsShelf { get; set; }

        public IEnumerable<GoodsShelfInfo> GoodsShelfForLocation { get; set; }

        public IEnumerable<GoodsShelfInfo> GoodsShelfRowAndCell { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
