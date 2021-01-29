using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.Warehouse;
using Runbow.TWS.RFScan.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Runbow.TWS.RFScan.Models
{
    [Serializable]
    public class ReceiptViewModel
    {
        public IEnumerable<ASN> AsnCollection { get; set; }
        public IEnumerable<Receipt> ReceiptCollection { get; set; }
        public IEnumerable<ReceiptDetail> ReceiptDetailCollection { get; set; }
        public Receipt SearchCondition { get; set; }
        public IEnumerable<Adjustment> AdjustmentCollection { get; set; }
        public IEnumerable<AdjustmentDetail> AdjustmentDetailCollection { get; set; }
        public Adjustment AdjustMentSearchCondition { get; set; }
        public IEnumerable<WarehouseCheck> WarehouseCheckCollection { get; set; }
        public IEnumerable<WarehouseCheckDetail> WarehouseCheckDetailCollection { get; set; }
        public WarehouseCheck WarehouseCheckSearchCondition { get; set; }
        public OrderSearchCondition orderSearchCondition { get; set; }
        public IEnumerable<SelectListItem> CompanyCodeList
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> OrderTypeList
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> ProductLevel
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> NoSelected
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("NoSelected");
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