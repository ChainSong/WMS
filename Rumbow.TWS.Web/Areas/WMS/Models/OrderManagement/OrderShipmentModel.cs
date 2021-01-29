using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderManagement
{
    public class OrderShipmentModel
    {
        public OrderShipmentSearchCondition SearchCondition { get; set; }//查询条件


        /// <summary>
        /// 客户下拉
        /// </summary>
        public IEnumerable<SelectListItem> CustomerList
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库下拉
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseList
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public IEnumerable<SelectListItem> StatusList
        {
            //get
            //{

            //    return new List<SelectListItem>()
            //    {
            //        new SelectListItem() { Value = "1", Text = "新增" },
            //        new SelectListItem() { Value = "2", Text = "已发送打印" },
            //        new SelectListItem() { Value = "3", Text = "已发送货物离场" }
            //    };
            //}
            get;set;
        }

        /// <summary>
        /// 场景类型
        /// </summary>
        public IEnumerable<SelectListItem> TypeList
        {
            get;set;
            //get
            //{
            //    return new List<SelectListItem>()
            //    {
            //        new SelectListItem() { Value = "1", Text = "场景1-4" },
            //        new SelectListItem() { Value = "2", Text = "场景5" }
            //    };
            //}
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public IEnumerable<SelectListItem> OrderStatusList
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

        /// <summary>
        /// 预出库单状态
        /// </summary>
        public IEnumerable<SelectListItem> PreOrderStatusList
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

        /// <summary>
        /// 预入库单状态
        /// </summary>
        public IEnumerable<SelectListItem> AsnStatus
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

        /// <summary>
        /// 入库单状态
        /// </summary>
        public IEnumerable<SelectListItem> ReceiptStatus
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

        

    }
}