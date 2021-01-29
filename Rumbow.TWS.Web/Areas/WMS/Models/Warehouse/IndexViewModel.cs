using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.Warehouse
{
    public class IndexViewModel
    {
        public WarehouseSearchCondition SearchCondition { get; set; }
        public IEnumerable<WarehouseInfo> WarehouseCollection { get; set; }
        public int PageIndex { get; set; }

        public int PageCount { get; set; }
        public IEnumerable<SelectListItem> WarehouseType
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("WarehouseType");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }
        public IEnumerable<SelectListItem> SearchType
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    //new SelectListItem() { Value = "", Text = "" },
                    new SelectListItem() { Value = "1", Text = "仓库查询" }, 
                    new SelectListItem() { Value = "2", Text = "库位查询" }
                };
            }
        }
        public IEnumerable<SelectListItem> WarehouseStatus
        {
         get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("WarehouseStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    if (w.Code == "1")
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name, Selected = true });
                    }
                    else
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    }
                }

                return st;
            }
        }

        public IEnumerable<SelectListItem> WarehouseList
        {
            get
            {               
                IEnumerable<WarehouseInfo> list = ApplicationConfigHelper.GetWarehouseList();
                List<SelectListItem> st = new List<SelectListItem>();
                
                foreach (WarehouseInfo warehouse in list)
                {
                    st.Add(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.WarehouseName });
                }
                
                return st;
            }
        }

        public IEnumerable<SelectListItem> AreaList
        {
            get
            {
                long WarehouseID = 0;
                //ApplicationConfigHelper.RefreshGetWarehouseAreaList(01);
                IEnumerable<AreaInfo> list = ApplicationConfigHelper.GetWarehouseAreaList(WarehouseID);
                List<SelectListItem> st = new List<SelectListItem>();
                
                foreach (AreaInfo warehouse in list)
                {
                    st.Add(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.AreaName });
                }
               
                return st;
            }

        }
     
    }
}