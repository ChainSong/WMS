using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Report;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Entity.WMS.Product;
namespace Runbow.TWS.Web.Areas.WMS.Models.ReportManagement
{
    [Serializable]
    public class InventoryViewModel
    {
        public Table Config { get; set; }
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public ReportInventorySearchCondition SearchCondition { get; set; }
        public IEnumerable<ReportInventory> ReportInventoryCollection { get; set; }
        public IEnumerable<WMS_Customer> WMS_CustomerCollection { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<SelectListItem> InventoryTypes
        {
            //get
            //{
            //    IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("InventoryTypes");
            //    List<SelectListItem> st = new List<SelectListItem>();
            //    foreach (WMSConfig w in wms)
            //    {
            //        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //    }
            //    return st;
            //}
            get;
            set;
        }
        public IEnumerable<SelectListItem> GoodsTypes
        {
            get;
            set;
        }

        /// <summary>
        /// 库存汇总查询条件
        /// </summary>
        public ReportInventorySummarySearchCondition InventorySummarySearchCondition { get; set; }

        public IEnumerable<ReportInventorySummary> ReportInventorySummaryCollection { get; set; }

        /// <summary>
        /// 是否根据库位汇总分组
        /// </summary>
        public IEnumerable<SelectListItem> LocationGroupBy
        {
            get
            {
                List<SelectListItem> st = new List<SelectListItem>();
                st.Add(new SelectListItem { Value = 1.ToString(), Text = "是" });
                st.Add(new SelectListItem { Value = 0.ToString(), Text = "否" });
                return st;
            }

        }
    }
}