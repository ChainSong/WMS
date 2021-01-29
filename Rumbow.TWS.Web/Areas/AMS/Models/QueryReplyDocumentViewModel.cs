using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.AMS.Models
{
    public class QueryReplyDocumentViewModel
    {
        public int Type { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> States
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "否" }, 
                    new SelectListItem() { Value = "1", Text = "是" } 
                };

            }
        }

        public AMSSearchCondition SearchCondition { get; set; }
         
        public IEnumerable<AMSUpload> AMSUploadCollection { get; set; } 
    }
}