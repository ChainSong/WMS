using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.ReceiptManagement
{
    public class ReceiptDetailViewModel
    {
        public int? ViewType { get; set; }

        public string rid { get; set; }
        public ReceiptDetailSearchCondition SearchCondition { get; set; }
        public ReceiptDetail ReceiptDetail{ get; set; }
        public Receipt Receipt { get; set; }
        public IEnumerable<Receipt> ReceiptCollection { get; set; }
        public IEnumerable<ReceiptDetail> ReceiptDetailCollection { get; set; }
        public IEnumerable<SelectListItem> ReceiptType
        {
            get;
            set;

        }
        public IEnumerable<SelectListItem> ReceiptStatus
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
    }
}