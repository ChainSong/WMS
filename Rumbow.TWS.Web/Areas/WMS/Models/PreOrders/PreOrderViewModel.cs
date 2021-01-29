using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.PreOrders;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.PreOrders
{
    public class PreOrderViewModel
    {

        public bool IsInnerUser { get; set; }

        public long ProjectRoleID { get; set; }

        public bool ShowCustomerOrShipperDrop { get; set; }

        public bool HideActionButton { get; set; }

        public bool ShowEditRelated { get; set; }

        public string ShowSubmit { get; set; }

        public string ReturnViewType { get; set; }

        public int Flag { get; set; }
        public IEnumerable<SelectListItem> CustomerNames
        {
            get;
            set;
        }
        public PreOrderAndPreOrderDetail PreAndDetail { get; set; }
        /// <summary>
        /// 仓库列表
        /// </summary>
        public IEnumerable<SelectListItem> Warehouses { get; set; }
        //{
        //    get
        //    {
        //        IEnumerable<WarehouseInfo> list = ApplicationConfigHelper.GetWarehouseList();
        //        List<SelectListItem> st = new List<SelectListItem>();
        //        foreach (WarehouseInfo warehouse in list)
        //        {
        //            st.Add(new SelectListItem() { Value = warehouse.WarehouseName.ToString(), Text = warehouse.WarehouseName });
        //        }
        //        return st;
        //    }
        //}
        /// <summary>
        /// 出库单类型
        /// </summary>
        public IEnumerable<SelectListItem> OrderTypes
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> ExpressCompanys
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
        /// <summary>
        /// 分配方式
        /// </summary>
        public IEnumerable<SelectListItem> ManualAllocation
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "全部分配", Text = "全部分配" }, 
                    new SelectListItem() { Value = "部分分配", Text = "部分分配" } 
                };
            }
        }
        public IEnumerable<SelectListItem> Statuss
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("PreOrderStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }
        public IEnumerable<SelectListItem> PreOrderStatusRead
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("PreOrderStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }
        public IEnumerable<SelectListItem> PreOrderSKUModelList
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "单SKU" }, 
                    new SelectListItem() { Value = "1", Text = "双SKU" } 
                };
            }
        }
        public IEnumerable<SelectListItem> selectList { get; set; }
        //{
        //    get
        //    {
        //        IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel");
        //        List<SelectListItem> st = new List<SelectListItem>();
        //        foreach (WMSConfig w in wms)
        //        {
        //            st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
        //        }

        //        return st;
        //    }

        //}

        public IEnumerable<SelectListItem> batchList
        {
            get
            {

                List<SelectListItem> st = new List<SelectListItem>();

                st.Add(new SelectListItem() { Value = "", Text = "" });


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

        public Table Config1 { get; set; }

        public Table Config2 { get; set; }

        public PreOrderSearchCondition SearchCondition { get; set; }

        public IEnumerable<PreOrderSearchCondition> SearchConditionResponse { get; set; }

        public PreOrderAndPreOrderDetail PoAndPod { get; set; }

        public PreOrderRequest Request { get; set; }

        public PreOrderResponse Response { get; set; }

        public IEnumerable<PreOrder> PreO { get; set; }

        public PreOrder preOrder { get; set; }

        public PreOrderDetail preOrderDetail { get; set; }

        public IEnumerable<PreOrderDetail> PreOd { get; set; }

        public int ViewType { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }
        public IEnumerable<SelectListItem> UnitList
        {
            //get
            //{

            //    List<SelectListItem> st = new List<SelectListItem>();

            //    st.Add(new SelectListItem() { Value = "", Text = "" });


            //    return st;
            //}
            get;set;
        }
        public IEnumerable<SelectListItem> SpecificationsList
        {
            //get
            //{

            //    List<SelectListItem> st = new List<SelectListItem>();

            //    st.Add(new SelectListItem() { Value = "", Text = "" });


            //    return st;
            //}
            get;set;
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
        /// <summary>
        /// 出库单取消原因
        /// </summary>
        public IEnumerable<SelectListItem> OrderCancelReasonList { get; set; }
    }
}