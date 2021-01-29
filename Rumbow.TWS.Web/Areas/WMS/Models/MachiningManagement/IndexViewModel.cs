using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;
namespace Runbow.TWS.Web.Areas.WMS.Models.MachiningManagement
{
    [Serializable]
    public class IndexViewModel
    {
        public IEnumerable<WMS_MachiningHeaderAndDetail> MachiningCollection { get; set; }
        public IEnumerable<Inventorys> InventoryCollection { get; set; }
        public MachiningSearchCondition searchCondition { get; set; }     
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int ViewType { get; set; }
        public string ShowSubmit { get; set; }
        public IEnumerable<SelectListItem> SpecificationsList
        {
            get
            {

                List<SelectListItem> st = new List<SelectListItem>();

                st.Add(new SelectListItem() { Value = "", Text = "" });


                return st;
            }
           
        }
        public IEnumerable<SelectListItem> WashSpecificationsList
        {
            get
            {

                List<SelectListItem> st = new List<SelectListItem>();

                st.Add(new SelectListItem() { Value = "", Text = "" });


                return st;
            }
           
        }
        public IEnumerable<SelectListItem> MoreThanSpecificationsList
        {
            get
            {

                List<SelectListItem> st = new List<SelectListItem>();
                st.Add(new SelectListItem() { Value = "", Text = "" });
                return st;
            }
           
        }

        public IEnumerable<SelectListItem> SKUTypeMachiningList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("SKUTypeMachiningList");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }

                return st;
            }
        }
    }
}