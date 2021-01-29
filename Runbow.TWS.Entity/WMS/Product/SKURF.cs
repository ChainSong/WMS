using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Product
{
    public class SKURF
    {
        /// <summary>
        /// AsnNumber
        /// </summary>
        [EntityPropertyExtension("AsnNumber", "AsnNumber")]
        public string AsnNumber { get; set; }

        /// <summary>
        /// ArticleNo
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

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


        /// <summary>
        /// Order
        /// </summary>
        [EntityPropertyExtension("Order", "Order")]
        public int Order { get; set; }

        /// <summary>
        /// BoxNumber
        /// </summary>
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        /// <summary>
        /// Qty
        /// </summary>
        [EntityPropertyExtension("Qty", "Qty")]
        public int Qty { get; set; }





    }
}
