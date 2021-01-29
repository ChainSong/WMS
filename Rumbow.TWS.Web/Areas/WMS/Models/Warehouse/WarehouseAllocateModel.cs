using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models
{
    public class WarehouseAllocateModel
    {
        public long CustomerID { get; set; }

        public long UserID { get; set; }

        public long WarehouseID { get; set; }

        public IEnumerable<Runbow.TWS.Entity.WMS.Warehouse.WarehouseAllocate> WarehouseAllocate { get; set; }

        public IEnumerable<Customer> Customers { get; set; }

        public IEnumerable<WarehouseInfo> Warehouse  { get; set; }

        public IEnumerable<User> User { get; set; }

        public int ViewType { get; set; }
        //盘点 hzf
        //public WarehouseSearchCondition SearchCondition { get; set; }
        //public IEnumerable<WarehouseInfo> WarehouseCollection { get; set; }
        //public int PageIndex { get; set; }

        //public int PageCount { get; set; }

        //public IEnumerable<SelectListItem> WarehouseName
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
        //            new SelectListItem() { Value = "5", Text = "变动库位盘点" }, 
        //            new SelectListItem() { Value = "6", Text = "手工录入" }
        //        };
        //    }
        //}
    }
}