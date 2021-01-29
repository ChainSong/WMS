using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodReplyDocumentAuditViewModel
    {
        public PodReplyDocumentSearchCondition SearchCondition { get; set; }

        public IEnumerable<PodReplyDocumentWithAttachment> ReplyDocumentWithAttachments { get; set; }

        public string SelectedIDs { get; set; }

        public long ShipperID { get; set; }

        public bool IsInnerUser { get; set; }

        public IEnumerable<SelectListItem> TrueOrFalse
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "true", Text = "Y" }, 
                    new SelectListItem() { Value = "false", Text = "N" } 
                };
            }
        }

        public IEnumerable<SelectListItem> AuditTypes
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "1", Text = "审核通过" },
                    new SelectListItem() { Value = "2", Text = "审核未通过" }
                };
            }
        }

        public IEnumerable<SelectListItem> ShipperTypes { get; set; }

        public IEnumerable<SelectListItem> PODTypes { get; set; }

        public string StartTime { get; set; }

        public string Tip { get; set; }

        public bool IsForExport { get; set; }

        //Hilti专用
        public bool IsShowForHilti { get; set; }

        public IEnumerable<SelectListItem> SalesOrderOrNoneSalesOrders
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "销售单", Text = "销售单" }, 
                    new SelectListItem() { Value = "非销售单", Text = "非销售单" } 
                };
            }
        }
    }
}