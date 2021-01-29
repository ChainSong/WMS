using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.Warehouse
{
    public class QRCodeViewModel
    {
        public QRCodeSearchCondition SearchCondition { get; set; }
        public IEnumerable<QRCodeInfo> QRCodeCollection { get; set; }
        public IEnumerable<QRCodeInfo> OperationCollection { get; set; }
        public IEnumerable<QRCodeInfo> ChargeCollection { get; set; }
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<SelectListItem> QRCodeType
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("QRCodeType");
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