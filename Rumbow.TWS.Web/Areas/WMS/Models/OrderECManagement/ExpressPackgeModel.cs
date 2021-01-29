using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Print;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderECManagement
{
    public class ExpressPackgeModel
    {
        public string CustomerIDs { get; set; }
        public string Warehouses { get; set; }
        public PackageInfo PackageCollection { get; set; }
        public IEnumerable<OrderDetailInfo> OrderDetailCollection { get; set; }
        /// <summary>
        /// 耗材类型
        /// </summary>
        public IEnumerable<SelectListItem> SupplieTypeList
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> Customers
        {
            get;
            set;
        }
    }
}