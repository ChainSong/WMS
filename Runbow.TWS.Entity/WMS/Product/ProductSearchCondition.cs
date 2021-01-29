using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.Entity
{
    public class ProductSearchCondition
    {
        //货主
        public string StorerID { get; set; }
        //品名
        public string SKU { get; set; }
        //分类
        public string SKUClassification { get; set; }
        //描述
        public string Remark { get; set; }
        //UPC
        public string UPC { get; set; }
        //品名类别
        public string Article { get; set; }
        //货品尺寸
        public string Size { get; set; }
    }
}
