using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class MaryKayTrackInfoModel
    {
        public string SystemOrderNo { get; set; }
        public string MkOrderNo { get; set; }

        public DateTime? BeginTrackDateTime { get; set; }
        public DateTime? EndTrackDateTime { get; set; }
        public string ExpressOrderNo { get; set; }
        public string TrackInfo { get; set; }

        public string UpLoadStatusID { get; set; }
        public IEnumerable<SelectListItem> UpLoadStatus
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "请选择",Selected=true }, 
                    new SelectListItem() { Value = "未上传", Text = "未上传" }, 
                    new SelectListItem() { Value = "上传成功", Text = "上传成功" }, 
                    new SelectListItem() { Value = "上传失败", Text = "上传失败" }
                    
                };

            }
        }



        public string TrackInfoTypeID { get; set; }
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



         
                    


        public string EndCity { get; set; }
        public string EndCityID { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public DataTable TrackInfoTable { get; set; }

        public bool IsExport { get; set; }
        public bool IsImport { get; set; }
    }
}