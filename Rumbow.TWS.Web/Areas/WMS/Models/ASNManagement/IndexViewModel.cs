using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.ASNs;
using Runbow.TWS.Web.Common;
namespace Runbow.TWS.Web.Areas.WMS.Models.ASNManagement
{
    [Serializable]
    public class IndexViewModel
    {
        public IEnumerable<ASNScan> ASNScanBoxSKUCollection { get; set; }
        public IEnumerable<ASNScan> ASNScanBoxDetailSKUCollection { get; set; }
        public ASNScan ExpectTotalBox { get; set; }
        public ASNScan ReceiveTotalBox { get; set; }
        public ASNScan ExpectTotalSKU { get; set; }
        public ASNScan ReceiveTotalSKU { get; set; }

        public Table Config { get; set; }
        public Table Config1 { get; set; }
        public Table Config2 { get; set; }
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public IEnumerable<ASNDetailViewModel> ASNDetailViewModel { get; set; }
        public ASNSearchCondition ASNCondition { get; set; }
        public ASNAndASNDetail AsnandDetails { get; set; }
        public IEnumerable<ASN> ASNCollection { get; set; }
        public IEnumerable<ASNDetail> ASNDetailCollection { get; set; }
        public ASN asninfos { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        /// <summary>
        /// 0 View;1 单条转入库单操作;2 Edit;3 查看入库单
        /// </summary>
        public int ViewType { get; set; }
        public string ReturnViewType { get; set; }

        public IEnumerable<SelectListItem> CustomerNames
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem> WarehouseNames
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem> ASNTypes
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> Statuss
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ASNStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }
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

        public IEnumerable<SelectListItem> CustomerList;

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
        public IEnumerable<SelectListItem> Units
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> Specificationss
        {
            get
            {

                List<SelectListItem> st = new List<SelectListItem>();

                st.Add(new SelectListItem() { Value = "", Text = "" });


                return st;
            }
        }
        public IEnumerable<SelectListItem> UnitAndSpecificationss
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> Specificationss2
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem> str8
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