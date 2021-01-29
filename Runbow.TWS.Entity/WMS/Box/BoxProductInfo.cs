using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Box
{
    public class BoxProductInfo
    {
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        [EntityPropertyExtension("ManufacturerSKU", "ManufacturerSKU")]
        public string ManufacturerSKU { get; set; }
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }
        //sku相关信息
        [EntityPropertyExtension("SKUlength", "SKUlength")]
        public double SKUlength { get; set; } = 40;//SKU长度
        [EntityPropertyExtension("SKUwidth", "SKUwidth")]
        public double SKUwidth { get; set; } = 20;//SKU宽度
        [EntityPropertyExtension("SKUhigh", "SKUhigh")]
        public double SKUhigh { get; set; } = 10;//SKU高度
        [EntityPropertyExtension("SKUvolume", "SKUvolume")]
        public double SKUvolume { get; set; } = 8000;//SKU体积
    }
}
