using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Receipt;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Models.ASNManagement
{
    public class ASNAbnormalTrackingModel
    {
        public ASNAbnormalSearchCondition SearchCondition { get; set; }
        public IEnumerable<ASNAbnormalTracking> ASNAbnormalList { get; set; }

        public IEnumerable<SelectListItem> CustomerList;

        /// <summary>
        /// 仓库下拉
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseList
        {
            get;
            set;
        }

        public long ProjectRoleID { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        public IEnumerable<SelectListItem> ASNAbnormalTypeList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ASNAbnormalType");
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