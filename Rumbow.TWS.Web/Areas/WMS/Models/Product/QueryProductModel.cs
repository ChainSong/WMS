using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.Product
{
    public class QueryProductModel
    {
        public int type { get; set; }

        public ProductStorer SearchCondition { get; set; }

        public ProductStorerInfo productStorerInfo { get; set; }

        public IEnumerable<ProductDetail> productDetail { get; set; }

        public string StorerName { get; set; }

        public bool IsQcEligible { get; set; }

        public IEnumerable<ProductStorer> IEnumerableSearchCondition { get; set; }

        public IEnumerable<SelectListItem> GetStorerID { get; set; }

        public IEnumerable<SelectListItem> GetSKUClass { get; set; }

        public IEnumerable<SelectListItem> GetStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_GetStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }

                return st;
            }
        }

        public IEnumerable<SelectListItem> GetStatusCode
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_GetStatus");
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

        public IEnumerable<SelectListItem> Get_Type
        {
            get;
            //{
                //IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");
                //List<SelectListItem> st = new List<SelectListItem>();
                //foreach (WMSConfig w in wms)
                //{
                //    st.Add(new SelectListItem() { Value = w.Code.ToString(), Text = w.Name });
                //}
                //return st;
            //}
            set;
        }

        public IEnumerable<SelectListItem> GetSKUClassification
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_GetSKUClassification");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name.ToString(), Text = w.Name });
                }

                return st;
            }
        }

        public IEnumerable<SelectListItem> GetSKUGroup
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_GetSKUGroup");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name.ToString(), Text = w.Name });
                }
                return st;
            }
        }


    }
}