using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.IntelligentWarehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Models.IntelligentOperation
{
    public class IntelligentOperation
    {
        public PickUpGoodsManagementRequest request { get; set; }
        public PickUpGoodsManagementResponse response { get; set; }
        public IEnumerable<OrderInfo> OrderCollection { get; set; }
        public IEnumerable<OrderDetailInfo> OrderDetailCollection { get; set; }

        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<SelectListItem> StstusList
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "1", Text = "正常" },   
                    new SelectListItem() { Value = "2", Text = "异常" }  ,          
                };
            }
        }
        public IEnumerable<SelectListItem> CustomerList { get; set; }
        public IEnumerable<SelectListItem> WarehouseList { get; set; }
        public IEnumerable<SelectListItem> WorkStationList { get; set; }


    }
}