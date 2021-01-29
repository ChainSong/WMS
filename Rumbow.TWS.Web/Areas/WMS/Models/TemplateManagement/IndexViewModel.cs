using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
using System.Web.Script.Serialization;
namespace Runbow.TWS.Web.Areas.WMS.Models.TemplateManagement
{
    [Serializable]
    public class IndexViewModel
    {
        
        public IEnumerable<TableColumn> TemplateCollection { get; set; }
        public SearchCondition searchCondition { get; set; }
        public IEnumerable<SelectListItem> CompanyList;
        public IEnumerable<SelectListItem> CustomerList;
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<SelectListItem> TemplateType
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("TemplateType");
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