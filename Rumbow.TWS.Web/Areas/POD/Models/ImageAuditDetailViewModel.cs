using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class ImageAuditDetailViewModel
    {
        public string PictureUrl { get; set; }

        public string URLPrev { get; set; }

        public string Message { get; set; }

        public long PodID { get; set; }

        public string UserName { get; set; }

        public string CurrentFolder { get; set; }

        public string CurrentImageName { get; set; }

        public string CurrentImageExtension { get; set; }

        public long PrevPodID { get; set; }

        public long NextPodID { get; set; }

        // public long customerID { get; set; }

        public string ImageNames { get; set; }

        public bool IsOK { get; set; }

        public string Remark { get; set; }

        public long AttachmentID { get; set; }

        public IEnumerable<SelectListItem> IsOKs
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "true", Text="合格"},
                    new SelectListItem(){Value = "false", Text="不合格"}
                };
            }
        }

        public Pod Pod { get; set; }
    }
}