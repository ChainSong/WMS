using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Receipt
{
    public class ReceiptPrint : ReceiptDetail
    {
        [EntityPropertyExtension("Manufacturer", "Manufacturer")]
        public string Manufacturer { get; set; }

        [EntityPropertyExtension("PageIndex", "PageIndex")]
        public string PageIndex { get; set; }

        [EntityPropertyExtension("PictureStr", "PictureStr")]
        public string PictureStr { get; set; }
    }
}
