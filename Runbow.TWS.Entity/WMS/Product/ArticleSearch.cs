using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Product
{
    public class ArticleSearch
    {

        /// <summary>
        /// ArticleNo
        /// </summary>
        [EntityPropertyExtension("ArticleNo", "ArticleNo")]
        public string ArticleNo { get; set; }


        /// <summary>
        /// Division
        /// </summary>
        [EntityPropertyExtension("Division", "Division")]
        public string Division { get; set; }


        [EntityPropertyExtension("GPC", "GPC")]
        public string GPC { get; set; }
        [EntityPropertyExtension("UOM", "UOM")]
        public string UOM { get; set; }

      

    }
}
