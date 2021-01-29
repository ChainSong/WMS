using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// RF扫描
    /// </summary>
    public class ReceiptReceivingRFScan
    {
        
        [EntityPropertyExtension("SKUTotal", "SKUTotal")]
        public string SKUTotal { get; set; }
        [EntityPropertyExtension("SKUScan", "SKUScan")]
        public string SKUScan { get; set; }
        [EntityPropertyExtension("BoxScan", "BoxScan")]
        public string BoxScan { get; set; }

    }
}
