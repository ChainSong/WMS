using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class MaryKayLoginsticsTrackInfoModel
    {
        public string  colID { get; set; }
        public string colOrderNo { get; set; }
        public string colCustOrderNo { get; set; }
        public string colTrackTime { get; set; }
        public string colIsNormal { get; set; }
        public string colTransStatus { get; set; }
        public string colTrackInfo { get; set; }
        public string colTrackComment { get; set; }
        public string colResponsibilityOwner { get; set; }
        public string colGoodsStatus { get; set; }
        public string colActualNoReceive { get; set; }
        public string colProvince { get; set; }
        public string colCity { get; set; }
        public string colCreater { get; set; }
        public string colCreateTime { get; set; }
        public string colUpdater { get; set; }
        public string colUpdateTime { get; set; }
        public string colSignName { get; set; }
        public string colSignTime { get; set; }
        public string colDelivery { get; set; }

        public DataTable TrackInfoTable { get; set; }
        public IEnumerable<SelectListItem> TrackInfoType
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "请选择",Selected=true }, 
                    new SelectListItem() { Value = "GOT", Text = "GOT" }, 
                    new SelectListItem() { Value = "DISPATCH", Text = "DISPATCH" }, 
                    new SelectListItem() { Value = "SIGNED", Text = "SIGNED" },
                    new SelectListItem() { Value = "LOST", Text = "LOST" },
                    new SelectListItem() { Value = "FAILED", Text = "FAILED" }
                    
                };

            }
        }



    }
}