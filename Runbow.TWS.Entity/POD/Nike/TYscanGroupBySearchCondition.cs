using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 天翼同步记录汇总结果Model
    /// </summary>
    public class TYscanGroupBySearchCondition
    {
        /// <summary>
        /// 扫描数量
        /// </summary>
        public int? ScanCount { get; set; }

        /// <summary>
        /// 扫描状态
        /// </summary>
        public string ScanStatus { get; set; }


        //发货日期
        public DateTime? StatCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
    }
}
