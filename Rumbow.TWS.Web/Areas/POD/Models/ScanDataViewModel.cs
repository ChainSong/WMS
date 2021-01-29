using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class ScanDataViewModel
    {
        public Table Config { get; set; }   //表配置
        public int PageIndex { get; set; }  //页号
        public int PageCount { get; set; }  //页总数
    }
}