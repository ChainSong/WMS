using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.ReceiptManagement
{
    [Serializable]
    public class IndexViewModel
    {
        public Table Config { get; set; }
        public Table Config1 { get; set; }
        public Table Config2 { get; set; }
        public Table Config3 { get; set; }
        public Table Config4 { get; set; }
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public ReceiptSearchCondition SearchCondition { get; set; }
        public IEnumerable<Receipt> ReceiptCollection { get; set; }
        public Receipt receipt { get; set; }
        public IEnumerable<ReceiptDetail> ReceiptDetailCollection { get; set; }
        public IEnumerable<ReportReceiptReport> ReceiptDetailCollection2 { get; set; }
        public IEnumerable<BarCodeInfo> BarCodeCollection { get; set; }
        public ASN asn { get; set; }
        public IEnumerable<ASNDetail> asndetail { get; set; }
        public DataTable dtAsn { get; set; }
        public DataTable dtAsnDetail { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int ViewType { get; set; }
        public int Flag { get; set; }
        public IEnumerable<SelectListItem> ReceiptTypes
        {
            get; set;

        }
        public IEnumerable<SelectListItem> Statuss
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ReceiptStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }

        }

        public IEnumerable<SelectListItem> CustomerList;
        //{
        //    get
        //    {
        //        IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("CustomerList");
        //        List<SelectListItem> st = new List<SelectListItem>();

        //        foreach (WMSConfig w in wms)
        //        {
        //            st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
        //        }

        //        return st;
        //    }

        //}



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

        public IEnumerable<SelectListItem> TrueOrFalse
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "0", Text = "N" },
                    new SelectListItem() { Value = "1", Text = "Y" }
                };
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


        /// <summary>
        /// 阿克苏的订单5种场景类型
        /// </summary>
        public IEnumerable<SelectListItem> OrderShipmentTypeList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("OrderShipmentType_Akzo");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }

        }
        /// <summary>
        ///阿克苏 场景 的订单是否已经发送打印了
        /// </summary>
        public IEnumerable<SelectListItem> OrderSendPrintTypeList
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem(){ Value="0",Text="未发送"},
                    new SelectListItem(){ Value="1",Text="已发送"}
                };
            }
        }
    }
}