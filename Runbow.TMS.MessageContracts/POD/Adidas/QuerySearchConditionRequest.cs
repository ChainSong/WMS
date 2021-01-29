using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    /// <summary>
    /// 查询条件请求类
    /// </summary>
    public class QuerySearchConditionRequest
    {
        /// <summary>
        /// 查询条 件
        /// </summary>
        public AdidasScanDataSearchCondition SearchCondition { get; set; }

        public int PageIndex { get; set; }  //页标

        public int PageSize { get; set; }  //每页数据大小

        public string Customers { get; set; }

    }
}
