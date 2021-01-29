using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Receipt
{
    public class ReceiptShelves
    {
        public string LineNumber { get; set; }

        public string CustomerName { get; set; }

        public string OfTheGoods { get; set; }

        public string ShelvesState { get; set; }

        public string CreateTime { get; set; }

        public string ExpectedNumber { get; set; }

        public string AdjustedNumber { get; set; }

        public string GoodsQuantity { get; set; }

        public string PlaceGoods { get; set; }

        public string SKUListNum { get; set; }

        public string SKU { get; set; }

        public string Describe { get; set; }

        public string GoodsType { get; set; }

        public string ActualNumber { get; set; }

        public string Note { get; set; }

        //      收货单行号: 'LineNumber', 供货商: 'CustomerName', 品名: 'SKU', 状态: 'ShelvesState', 收货日期: 'CreateTime',
        //期望数量:'',已调整数量:'',已收货数量:'',放置货位:'',SKU行号:'',SKU:'',描述:'',货物类型:'',实收数量:'',备注:''};
    }
}
