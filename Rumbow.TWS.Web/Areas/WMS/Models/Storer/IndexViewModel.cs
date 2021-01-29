using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Storer
{
    public class IndexViewModel
    {
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int Type { get; set; }
        public IEnumerable<SelectListItem> GetTypes
        {
            
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("storeType");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }
        public int Status { get; set; }
        public IEnumerable<SelectListItem> GetStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("storeStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }

        public StorerSearchCondition SearchCondition { get; set; }

        public IEnumerable<Runbow.TWS.Entity.Storer> WMS_StorerCollection { get; set; } 
    }
}