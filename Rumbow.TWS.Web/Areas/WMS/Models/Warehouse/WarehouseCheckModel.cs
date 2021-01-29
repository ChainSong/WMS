using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Warehouse;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.Warehouse
{
    public class WarehouseCheckModel
    {
        //盘点 hzf
        public WarehouseCheckSearchCondition SearchCondition { get; set; }
        public WarehouseCheck WarehouseCheck { get; set; }
        public IEnumerable<WarehouseCheck> WarehouseCheckCollection { get; set; }
        public IEnumerable<WarehouseCheckDetail> WarehouseCheckDetailCollection { get; set; }

        public string Roles {get;set; }

        public string ActualQty { get; set; }

        public string Oprer { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }
        
        public int ViewType { get; set; }

        //public IEnumerable<SelectListItem> Type
        //{
        //    get
        //    {
        //        return new List<SelectListItem>() 
        //        { 
        //            //new SelectListItem() { Value = "", Text = "" },
        //            new SelectListItem() { Value = "1", Text = "全部盘点" }, 
        //            new SelectListItem() { Value = "2", Text = "库位盘点" }, 
        //            new SelectListItem() { Value = "3", Text = "品名盘点" }, 
        //            new SelectListItem() { Value = "4", Text = "小货量盘点" }, 
        //            new SelectListItem() { Value = "5", Text = "变动库位盘点" }
        //            //, 
        //            //new SelectListItem() { Value = "6", Text = "手工录入" }
        //        };
        //    }
        //}
        public IEnumerable<SelectListItem> CheckType
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("CheckType");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }
        public IEnumerable<SelectListItem> Remark
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    //new SelectListItem() { Value = "", Text = "" },
                    new SelectListItem() { Value = "1", Text = "收货"  , Selected = false }, 
                    new SelectListItem() { Value = "2", Text = "发货"  , Selected = false }, 
                    new SelectListItem() { Value = "3", Text = "移动"  , Selected = false }, 
                    new SelectListItem() { Value = "4", Text = "调整"  , Selected = false },
                    new SelectListItem() { Value = "5", Text = "冻结"  , Selected = false }
                };
            }
        }

        /// <summary>
        /// 空库位盘点类型
        /// </summary>
        public IEnumerable<SelectListItem> EmptyLocationType
        {
            get
            {
                return new List<SelectListItem>()
                {                 
                    new SelectListItem() { Value = "2", Text = "发货"  , Selected = false }                   
                };
            }
        }


        public IEnumerable<SelectListItem> WarehouseList
        {
            get
            {
                IEnumerable<WarehouseInfo> list = ApplicationConfigHelper.GetWarehouseList();
                List<SelectListItem> st = new List<SelectListItem>();

                foreach (WarehouseInfo warehouse in list)
                {
                    st.Add(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.WarehouseName });
                }

                return st;
            }
        }

        public IEnumerable<SelectListItem> AreaList
        {
            get
            {
                long WarehouseID = 0;
                //ApplicationConfigHelper.RefreshGetWarehouseAreaList(01);
                IEnumerable<AreaInfo> list = ApplicationConfigHelper.GetWarehouseAreaList(WarehouseID);
                List<SelectListItem> st = new List<SelectListItem>();

                foreach (AreaInfo warehouse in list)
                {
                    st.Add(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.AreaName });
                }

                return st;
            }

        }
    }
}