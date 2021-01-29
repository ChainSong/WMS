using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.Warehouse
{
    public class GoodsShelfsViewModel
    {
        public GoodsShelfSearchCondition SearchCondition { get; set; }
        public IEnumerable<GoodsShelfInfo> GoodsShelfCollection { get; set; }
        public IEnumerable<GoodsShelfInfo> GoodsShelfRowAndCellCollection { get; set; }
        public GoodsShelfInfo GoodsShelf { get; set; }
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int ViewType { get; set; }
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

     
    }
}