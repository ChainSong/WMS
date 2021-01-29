using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class TYscanDetailSearchCondition
    {
        public long PODID { get; set; }
        /// <summary>
        /// 客户运单号
        /// </summary>
        public string CustomerOrderNumber { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 箱号     
        /// </summary>
        public string LabelNo { get; set; }


        /// <summary>
        /// 收货扫描人 
        /// </summary>
        public string SHTor { get; set; }

        /// <summary>
        /// 收货扫描时间 
        /// </summary>
        public string SHTime { get; set; }

        /// <summary>
        /// 发货扫描人 
        /// </summary>
        public string FHTor { get; set; }

        /// <summary>
        /// 发货扫描时间 
        /// </summary>
        public string FHTime { get; set; }
    }
}
