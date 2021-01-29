using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class BaiXingViewModel
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }

        public string Str1 { get; set; }    //快递单号
        public string CustomerOrderNumber { get; set; }//客户运单号
        public string Str2 { get; set; }    //商品品名
        public string Str7 { get; set; }    //发件城市
        public string Str13 { get; set; }   //收件城市
        public string Str17 { get; set; }   //快递公司
        public string SystemNumber { get; set; }        

        public PodSearchCondition SearchCondition { get; set; }
        public string StartCreateTime { get; set; }
        public string EndCreateTime { get; set; }
        public bool IsForExport { get; set; }

        public IEnumerable<PodWithAttachment> PodCollection { get; set; }
    }
}