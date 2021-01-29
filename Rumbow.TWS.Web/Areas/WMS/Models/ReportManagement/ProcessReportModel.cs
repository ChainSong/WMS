using Runbow.TWS.Entity.WMS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Models.ReportManagement
{
    public class ProcessReportModel
    {
        public ProcessTrackingSearchCondition SearchCondition { get; set; }
        public IEnumerable<WMS_ProcessTracking> processCollection { get; set; }


        /// <summary>
        /// 客户下拉
        /// </summary>
        public IEnumerable<SelectListItem> CustomerList
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库下拉
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseList
        {
            get;
            set;
        }

        /// <summary>
        /// 门店下拉
        /// </summary>
        public IEnumerable<SelectListItem> StoresList
        {
            get;
            set;
        }
        /// <summary>
        /// 出库/入库
        /// </summary>
        public IEnumerable<SelectListItem> TypeList
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "1", Text = "出库" },
                    new SelectListItem() { Value = "2", Text = "入库" }
                 };
            }
        }
    }
}





