using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.RFScan.Common;

namespace Runbow.TWS.RFScan.Models
{
    public class ExpressPackageModel
    {
        public long? CustomerIDs { get; set; }
        public long? WarehouseIDs { get; set; }
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