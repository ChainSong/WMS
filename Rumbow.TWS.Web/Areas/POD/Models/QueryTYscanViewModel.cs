using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.POD
{
    /// <summary>
    /// 天翼扫描记录视图MODEL
    /// </summary>
    public class QueryTYscanViewModel
    {
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public bool IsExport { get; set; }

        //public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> type
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "否" }, 
                    new SelectListItem() { Value = "1", Text = "是" } 
                };

            }
        }

        //订单列表
        public TYscanSearchCondition SearchCondition { get; set; }
        public IEnumerable<TYscan> TYscanCollection { get; set; }

        //订单汇总
        public TYscanGroupBySearchCondition SearchConditionGroupBy { get; set; }
        public IEnumerable<TYscanGroupBy> TYscanCollectionGroupBy { get; set; }

        //订单明细
        public TYscanDetailSearchCondition SearchConditionDetail { get; set; }
        public IEnumerable<TYscanDetail> TYscanCollectionDetail { get; set; } 
    }
}