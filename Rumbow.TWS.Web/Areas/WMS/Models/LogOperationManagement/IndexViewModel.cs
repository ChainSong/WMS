using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.MessageContracts.WMS.ASNs;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.LogOperationManagement
{
    [Serializable]
    public class IndexViewModel
    {
        public Table Config { get; set; }
        public Table Config1 { get; set; }
        public Table Config2 { get; set; }
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public LogOperationSearchCondition LogOperationCondition { get; set; }
        public IEnumerable<WMS_Log_Operation> LogOperationCollection { get; set; }
        public LogOperationRFSearchCondition LogOperationRFCondition { get; set; }
        public IEnumerable<WMS_Log_OperationRF> LogOperationRFCollection { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int ViewType { get; set; }
        public IEnumerable<SelectListItem> WarehouseList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("WarehouseList");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }

        }
        public IEnumerable<SelectListItem> RFLogTypeList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("RFLogTypeList");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }

                return st;
            }

        }
        public IEnumerable<SelectListItem> CustomerList;
    }
}