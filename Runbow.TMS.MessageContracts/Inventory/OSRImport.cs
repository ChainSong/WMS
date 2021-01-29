using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.Inventory
{
   public class OSRImport
    {
        public string OrderKey { get; set; }
        //导入退货TransOrderNO(数据库中的BillToAddress1字段)
        public string TransOrderNO { get; set; }
        //导入ATA(数据库中的Date1字段)
        //public string ATA { get; set; }
        //导入SKU（ArticleNo+Color）
        //public string SKUCategory10 { get; set; }
        //public string HSCode { get; set; }
    }
}
