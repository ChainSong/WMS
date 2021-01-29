using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.Warehouse
{
    public class IndexWLocationViewModel
    {
        WarehouseService ws = new WarehouseService();
        public WLocationSearchCondition WLSearchCondition { get; set; }
        public IEnumerable<LocationInfo> WLocationCollection { get; set; }
        public int flag { get; set; }
        public int PageIndex { get; set; }

        public int PageCount { get; set; }
        public IEnumerable<SelectListItem> LocationType1
        {
            get;
            set;
            //{
            //    IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType");
            //    List<SelectListItem> st = new List<SelectListItem>();
            //    foreach (WMSConfig w in wms)
            //    {
            //        if (w.Code == "1")
            //        {
            //            st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //        }
            //        else
            //        {
            //            st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //        }
            //    }            
            //    return st;
            //}
        }

        public IEnumerable<SelectListItem> LocationLevel
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("LocationLevel");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    if (w.Code == "1")
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    }
                    else
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    }
                }

                return st;
            }
        }

        public IEnumerable<SelectListItem> Classification
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("Classification");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    if (w.Code == "1")
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    }
                    else
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    }
                }

                return st;
            }
        }

        public IEnumerable<SelectListItem> Handling
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("Handling");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    if (w.Code == "1")
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    }
                    else
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name});
                    }
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
                    new SelectListItem() { Value = "2", Text = "库位查询",Selected=true }
                };
            }
        }
        public IEnumerable<SelectListItem> LocationStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("LocationStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }
        public IEnumerable<SelectListItem> IsMultiLot
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("IsMultiLot");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }
        
        public IEnumerable<SelectListItem> ABCClassification
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ABCClassification");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    if (w.Code == "A")
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    }
                    else
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    }
                }

                return st;
            }
        }
        public IEnumerable<SelectListItem> IsMultiSKU
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("IsMultiSKU");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                   
                }

                return st;
            }
        }

        public IEnumerable<SelectListItem> WarehouseIDD
        {
            get

                //IEnumerable<WarehouseInfo> wms = ws.GetWarehouse();
                //List<SelectListItem> st = new List<SelectListItem>();
                //foreach (WarehouseInfo w in wms)
                //{
                //    st.Add(new SelectListItem() { Value = w.ID.ToString(), Text = w.WarehouseName });
                //}

                //return st;
             ;
            set;

        }

        /// <summary>
        /// 库区列表
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseAreaList
        {
            //get
            //{
            //    long WarehouseID = 0;
            //    //ApplicationConfigHelper.RefreshGetWarehouseAreaList(01);
            //    IEnumerable<AreaInfo> list = ApplicationConfigHelper.GetWarehouseAreaList(WarehouseID);
            //    List<SelectListItem> st = new List<SelectListItem>();
            //    foreach (AreaInfo warehouse in list)
            //    {
            //        st.Add(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.AreaName });
            //    }
            //    return st;
            //}
            get;
            set;
        }
    }
}