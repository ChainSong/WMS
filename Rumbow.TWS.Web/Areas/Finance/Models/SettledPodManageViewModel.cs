using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.Finance.Models
{
    public class SettledPodManageViewModel
    {
        public SettledPodSearchCondition SearchCondition { get; set; }

        public IEnumerable<SettledPod> SettledPods { get; set; }

        public IEnumerable<SettledPodAuditHistory> SettledPodAuditHistoryCollection { get; set; }

        public bool IsForInvoice { get; set; }

        public bool IsInnerUser { get; set; }

        public bool ShowSelectCheckBox { get; set; }

        public bool ShowActionButton { get; set; }

        public long ProjectRoleID { get; set; }

        public int IsInvoiced { get; set; }

        public IEnumerable<SelectListItem> ShipperTypes { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Shippers { get; set; }

        public IEnumerable<SelectListItem> PODTypes { get; set; }

        public IEnumerable<SelectListItem> TtlOrTpls { get; set; }

        public IEnumerable<SelectListItem> IsInvoiceds = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "N" }, new SelectListItem() { Value = "1", Text = "Y" } };

        public string SelectedIDs { get; set; }

        public string Message { get; set; }

        public bool IsForAudit { get; set; }

        public int AuditType { get; set; }

        public bool FinalAudit { get; set; }

        public string Name { get; set; }

        public bool IsBatchAdjust { get; set; }

        public bool IsExport { get; set; }
    }
}