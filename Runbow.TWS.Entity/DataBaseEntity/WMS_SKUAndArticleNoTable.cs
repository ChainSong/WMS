using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WMS_SKUAndArticleNoTable
    {
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        [EntityPropertyExtension("ArticleNo", "ArticleNo")]
        public string ArticleNo { get; set; }
    }
}
