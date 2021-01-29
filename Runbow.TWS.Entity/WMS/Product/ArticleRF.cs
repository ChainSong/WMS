using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Product
{
    public class ArticleRF
    {
        /// <summary>
        /// AsnNumber
        /// </summary>
        [EntityPropertyExtension("AsnNumber", "AsnNumber")]
        public string AsnNumber { get; set; }

        /// <summary>
        /// ArticleNo
        /// </summary>
        [EntityPropertyExtension("ArticleNo", "ArticleNo")]
        public string ArticleNo { get; set; }

        /// <summary>
        /// GoodsType
        /// </summary>
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }

        /// <summary>
        /// Creator
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }


        /// <summary>
        /// No
        /// </summary>
        [EntityPropertyExtension("No", "No")]
        public int No { get; set; }


       

      

    }
}
