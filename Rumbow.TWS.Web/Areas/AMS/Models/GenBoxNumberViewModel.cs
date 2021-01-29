using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.AMS.Models
{
    public class GenBoxNumberViewModel
    {
        public int PageIndex { get; set; }

        public int PageCount { get; set; }
        public int RowCount { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> States
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "未生成装箱单" }, 
                    new SelectListItem() { Value = "1", Text = "已生成装箱单" }  
                };

            }
        }
        public List<string> Check { get; set; }

        public AMSSearchCondition SearchCondition { get; set; }

        public IEnumerable<AMSUpload> AMSUploadCollection { get; set; }
    }
}