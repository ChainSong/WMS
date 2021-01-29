using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class AdidasScanDataSearchCondition
    {
        /// <summary>
        /// 运单号 查询
        /// </summary>
        public string CustomerOrderNumber { get; set; }

        /// <summary>
        /// 车牌号查询
        /// </summary>
        public string PlateNumber { get; set; }

        /// <summary>
        /// 拖号查询
        /// </summary>
        public string TrailerNo { get; set; }

        /// <summary>
        /// 完成状态
        /// </summary>
        public int CompleteFlag { get; set; }

        /// <summary>
        /// 关闭状态
        /// </summary>
        public int CloseFlag { get; set; }


        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 操作时间上限
        /// </summary>
        public DateTime OperateTime_Start { get; set; }


        /// <summary>
        /// 操作时间下限
        /// </summary>
        public DateTime OperateTime_End { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater { get; set; }

        /// <summary>
        /// 创建时间上限
        /// </summary>
        public DateTime CreateTime_Start { get; set; }


        /// <summary>
        /// 创建时间下限
        /// </summary>
        public DateTime CreateTime_End { get; set; }


        /// <summary>
        /// 承运商
        /// </summary>
        public string Shipper { get; set; }



    }
}
