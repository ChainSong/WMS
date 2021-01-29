using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.SettlementManagement;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.SettlementManagement
{
    public class SettlementListViewModel
    {
        public SettlementSearchCondition SearchCondition { get; set; }
        public Settlement Settlement { get; set; }
        public IEnumerable<Settlement> SettlementCollection { get; set; }
        public IEnumerable<SettlementDetail> SettlementDetailCollection { get; set; }
        public string Roles {get;set; }
        public string ActualQty { get; set; }
        public string Oprer { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int RowIndex { get; set; }
        public int RowCount { get; set; }
        public int ViewType { get; set; }
        public IEnumerable<SelectListItem> Customerlist
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> Warehouselist
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> Monthlist
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "1", Text = "1月" },
                    new SelectListItem() { Value = "2", Text = "2月" },
                    new SelectListItem() { Value = "3", Text = "3月" },
                    new SelectListItem() { Value = "4", Text = "4月" },
                    new SelectListItem() { Value = "5", Text = "5月" },
                    new SelectListItem() { Value = "6", Text = "6月" },
                    new SelectListItem() { Value = "7", Text = "7月" },
                    new SelectListItem() { Value = "8", Text = "8月" },
                    new SelectListItem() { Value = "9", Text = "9月" },
                    new SelectListItem() { Value = "10", Text = "10月" },
                    new SelectListItem() { Value = "11", Text = "11月" },
                    new SelectListItem() { Value = "12", Text = "12月" }
                };
            }
        }
        IEnumerable<WMSConfig> wms;
        List<SelectListItem> st = new List<SelectListItem>();
        public IEnumerable<SelectListItem> SettlementType
        {
            get
            {
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("SettlementType");
                    foreach (WMSConfig w in wms)
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    }
                }
                catch (Exception)
                {
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> CostCategorylist
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "1", Text = "固定费用" },
                    new SelectListItem() { Value = "2", Text = "额外费用" }
                };
            }
        }


    }
}