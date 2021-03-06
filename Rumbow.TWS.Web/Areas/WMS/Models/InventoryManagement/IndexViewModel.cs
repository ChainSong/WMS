﻿using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Inventory;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Runbow.TWS.Web.Areas.WMS.Models.Inventory
{
    [Serializable]
    public class IndexViewModel
    {
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public IEnumerable<AdjustmentDetailsViewModel> AdjustmentDetailViewModel { get; set; }
        public AdjustmentSearchCondition AdjustmentCondition { get; set; }
        public AdjustmentAndAdjustmentDetail AdjustmentAndAdjustmentDetails { get; set; }
        public IEnumerable<Adjustment> AdjustmentCollection { get; set; }
        public Adjustment adjustment { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int ViewType { get; set; }

        public IEnumerable<Inventorys> InventoryCollection { get; set; }
        public IEnumerable<Inventorys> InventoryCollection2 { get; set; }
        public IEnumerable<InventorySnapshoot> InventorySnapCollection { get; set; }
        public InventorySearchCondition InventorySearchCondition { get; set; }
        public IEnumerable<Receipt> ReceiptCollection { get; set; }
        public IEnumerable<OrderInfo> OrderCollection { get; set; }
        public IEnumerable<DirectAddInventory> directAddInventory { get; set; }

        public IEnumerable<SelectListItem> CaiwuType
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "1", Text = "出库" },
                    new SelectListItem() { Value = "2", Text = "入库" }
                };
            }
        }

        public IEnumerable<SelectListItem> InventoryType
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("InventoryType");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }
                return st;
            }
        }

        public IEnumerable<SelectListItem> GoodsTypes
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
        public IEnumerable<SelectListItem> InventoryStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("InventoryStatus");
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
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name, Selected = true });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> FreezeList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("Freeze");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }

        }

        public IEnumerable<SelectListItem> CustomerList;
        public IEnumerable<SelectListItem> AreaList;
        public IEnumerable<SelectListItem> LocationList;


        public IEnumerable<SelectListItem> InventoryTypes
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "1", Text = "可用库存",Selected=true },
                    new SelectListItem() { Value = "2", Text = "操作中库存" },
                    new SelectListItem() { Value = "3", Text = "冻结库存" },
                    new SelectListItem() { Value = "9", Text = "已出库库存" }

                };
            }
        }

        public IEnumerable<SelectListItem> Model
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "产品", Text = "产品",Selected=true },
                    new SelectListItem() { Value = "物料", Text = "物料" },

                };
            }
        }

        public IEnumerable<SelectListItem> OrderByType
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("OrderByType");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }

        }
        public IEnumerable<SelectListItem> SKUListEndTime
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("SKUListEndTime");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }

        }

        /// <summary>
        /// 调整表的reasonCode
        /// </summary>
        public IEnumerable<SelectListItem> ADJReasonType
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ADJReasonType");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }
                return st;
            }
        }
        /// <summary>
        /// 门店Code
        /// </summary>
        public IEnumerable<SelectListItem> CompanyCodeList
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem> StorerList
        {
            get; set;
        }
    }
}