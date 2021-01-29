using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderManagement
{
    public class OrderViewModel
    {
        public OrderSearchCondition SearchCondition { get; set; }
        public Table Config1 { get; set; }
        public Table Config2 { get; set; }
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public OrderInfo order { get; set; }
        public string box { get; set; }
        public IEnumerable<OrderInfo> OrderCollection { get; set; }
        public IEnumerable<OrderDetailInfo> OrderDetailCollection { get; set; }
        public IEnumerable<PackageInfo> PackageCollection { get; set; }
        public IEnumerable<PackageDetailInfo> PackageDetailCollection { get; set; }
        public long UPCSum { get; set; }
        public long OrderSum { get; set; }
        public long PrintID { get; set; }//打印关联主键ID
        //public IEnumerable<SelectListItem> WorkStation { get; set; }
        public IEnumerable<SelectListItem> Customers
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> OrderType
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> IsMerged
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("IsMerged");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }

        public IEnumerable<SelectListItem> IsHaveWave
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("IsHaveWave");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> OrderStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("OrderStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> ExpressStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ExpressStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> ExpressCompany
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
        public IEnumerable<SelectListItem> OrderYesOrNoFlag
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("OrderYesOrNoFlag");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> IsGetExpressNumber
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("IsGetExpressNumber");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> Skucounts
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("Skucounts");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> SKUTypeAndCount
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("SKUTypeAndCount");
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
        public IEnumerable<SelectListItem> Str19
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "0", Text = "未打印" },
                    new SelectListItem() { Value = "1", Text = "已打印" }
                };
            }
        }
        public IEnumerable<BarCodeInfo> BarCodeCollection { get; set; }

        /// <summary>
        /// 承运商类型（快递还是物流）
        /// </summary>
        public IEnumerable<SelectListItem> ShipmentType
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem(){ Value="快递公司",Text="快递公司"},
                    new SelectListItem(){ Value="物流公司",Text="物流公司"}
                };
            }
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