using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 天翼同步记录汇总实体Model
    /// </summary>
    public class TYscanGroupBy
    {
        [EntityPropertyExtension("ScanCount", "ScanCount")]
        public int? ScanCount { get; set; }

        [EntityPropertyExtension("ScanStatus", "ScanStatus")]
        public string ScanStatus { get; set; }
    }
}
