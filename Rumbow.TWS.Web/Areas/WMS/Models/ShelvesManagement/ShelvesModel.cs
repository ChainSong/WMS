using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Shelves;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
namespace Runbow.TWS.Web.Areas.WMS.Models.ShelvesManagement
{
    [Serializable]
    public class ShelvesModel
    {
        public IEnumerable<Shelves> Shelves { get; set; }

        public ReceiptReceivingSearchCondition SearchCondition { get; set; }

        public GetReceiptbyCondition Condition { get; set; }

        public bool IsInnerUser { get; set; }

        public bool ShowCustomerOrShipperDrop { get; set; }

        public bool HideActionButton { get; set; }

        public bool ShowEditRelated { get; set; }

        public long ProjectRoleID { get; set; }

        public Table Config { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        /// <summary>
        /// 仓库列表
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseList{ get; set; }
        //{
        //    get
        //    {
        //        IEnumerable<WarehouseInfo> list = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID.ToString() && c.UserID == base.UserInfo.ID))
        //                           .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
        //        List<SelectListItem> st = new List<SelectListItem>();
        //        foreach (WarehouseInfo warehouse in list)
        //        {
        //            st.Add(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.WarehouseName });
        //        }
        //        return st;
        //    }
        //}
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
        public IEnumerable<SelectListItem> Customers { get; set; }

        public StoresByGetReceipt storesByGetReceipt { get; set; }
        public IEnumerable<StoresByGetReceipt> receipt { get; set; }
        public IEnumerable<SelectListItem> ReceiptStatusBack
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "-1", Text = "取消" }, 
                    new SelectListItem() { Value = "1", Text = "待上架" } 
                };
                //IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ReceiptStatus");
                //List<SelectListItem> st = new List<SelectListItem>();
                //foreach (WMSConfig w in wms)
                //{
                //    if (w.Code == "-1" || w.Code == "1")
                //    {
                //        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                //    }
                //}

                //return st;
            }

        }

        //上架管理页面的上架状态（读取状态）
        public IEnumerable<SelectListItem> ReceiptStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ReceiptStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    //if (w.Code == "-1" || w.Code == "1")
                    //{
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                    //}
                }

                return st;
            }

        }
        public IEnumerable<SelectListItem> selectList //{ get; set; }
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
        public IEnumerable<SelectListItem> StorerIDs
        {
            get;
            set;
            //get
            //{
            //    IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_SKUClass");
            //    List<SelectListItem> st = new List<SelectListItem>();
            //    foreach (WMSConfig w in wms)
            //    {
            //        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //    }

            //    return st;
            //}
        }
    }
}