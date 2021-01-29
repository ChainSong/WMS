using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.Web.Areas.WMS.Models.Product
{
    public class ProductModel
    {
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public ProductStorer SearchCondition { get; set; }

        public IEnumerable<ProductStorer> IEnumerableSearchCondition { get; set; }

        public ProductSearchCondition ProductSKU { get; set; }

        //public Storer storer { get; set; }
        
        public IEnumerable<SelectListItem> SKUClass
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_GetSKUClassification");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> StorerID
        {
            get;
            set;
            //get
            //{
            //    IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_SKUClass");
            //    List<SelectListItem> st = new List<SelectListItem>();
            //    foreach (WMSConfig w in wms)
            //    {
            //        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //    }

            //    return st;
            //}
        }
        public IEnumerable<SelectListItem> GetSKUGroup
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_GetSKUGroup");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code.ToString(), Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> GetSKUClassification
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_GetSKUClassification");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code.ToString(), Text = w.Name });
                }

                return st;
            }
        }
    }
}