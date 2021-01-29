using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class UserWarehouse
    {
        public long UserID { get; set; }

        public long ProjectID { get; set; }

        public IEnumerable<SelectListItem> ProjectUsers { get; set; }

        public IEnumerable<UserWarehouseInfo> ProjectCustomers { get; set; }

        public string SelectedCustomers { get; set; }
    }
}