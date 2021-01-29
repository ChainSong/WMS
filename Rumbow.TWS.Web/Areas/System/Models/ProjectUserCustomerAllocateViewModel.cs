using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class ProjectUserCustomerAllocateViewModel
    {
        public long UserID { get; set; }

        public long ProjectID { get; set; }

        public IEnumerable<SelectListItem> ProjectUsers { get; set; }

        public IEnumerable<ProjectUserCustomer> ProjectCustomer { get; set; }

        public IEnumerable<ProjectCustomer> ProjectCustomers { get; set; }

        public string SelectedCustomers { get; set; }
    }
}