using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.DeliveryConfirm;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderECManagement
{
    public class DeliveryConfirmModel
    {

        public DeliverHeaderSearchCondition SearchCondition { get; set; }

        public IEnumerable<DeliverHeader> DeliverHeaderConnection { get; set; }
        public IEnumerable<DeliverDetail> DeliverDetailConnection { get; set; }//交接单明细
        public IEnumerable<DeliverDetail> DeliverExpressNoConnection { get; set; }//快递单列表
        public Table Config1 { get; set; }
        public Table Config2 { get; set; }
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int ViewType { get; set; }
        public IEnumerable<SelectListItem> Customerids { get; set; }
        public string Express { get; set; }
        public IEnumerable<SelectListItem> Customers
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> ExpressCompany//快递公司
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ExpressCompany");
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