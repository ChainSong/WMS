using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.ASNManagement
{
    [Serializable]
    public class ASNDetailViewModel
    {
        public ASNDetail ASNDetail { get; set; }
        public IEnumerable<ASNDetailByProc> ASNDetailCollection { get; set; }

        public IEnumerable<SelectListItem> ASNType
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }

        }
        public IEnumerable<SelectListItem> ASNStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("Status");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }

        }

        public IEnumerable<SelectListItem> UnitList
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> SpecificationsList
        {
            get;
            set;
        }
    }
}