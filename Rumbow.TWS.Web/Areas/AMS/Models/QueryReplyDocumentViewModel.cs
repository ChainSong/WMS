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

        /// <summary>
        /// 跟踪状态
        /// </summary>
        public IEnumerable<SelectListItem> CurrentStateList
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "-1", Text = "==请选择==" },
                    new SelectListItem() { Value = "1", Text = "运输中" },
                    new SelectListItem() { Value = "2", Text = "已签收" }
                };

            }
        }

        public AMSSearchCondition SearchCondition { get; set; }
         
        public IEnumerable<AMSUpload> AMSUploadCollection { get; set; } 

        public WMS_Package packageSearch { get; set; }
        public IEnumerable<WMS_Package> packageList { get; set; }
        public IEnumerable<WMS_PackageTrack> packageTrackList { get; set; }
        public IEnumerable<WMS_SFDetail> sfDetailList { get; set; }
    }
}