using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class AdidasScanDataViewModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }


        /// <summary>
        /// 登录用户
        /// </summary>
        public IEnumerable<SelectListItem> Customers { get; set; }


        /// <summary>
        /// 关闭标记
        /// </summary>
        public IEnumerable<SelectListItem> CloseFlag
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    
                    new SelectListItem() { Value = "-1", Text = "==请选择==" }, 
                    new SelectListItem() { Value = "0", Text = "否" }, 
                    new SelectListItem() { Value = "1", Text = "是" } 
                };

            }
        }

        /// <summary>
        /// 完成标记
        /// </summary>
        public IEnumerable<SelectListItem> CompleteFlag
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "-1", Text = "==请选择==" }, 
                    new SelectListItem() { Value = "0", Text = "否" }, 
                    new SelectListItem() { Value = "1", Text = "是" } 
                };

            }
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public AdidasScanDataSearchCondition SearchCondition { get; set; }


        /// <summary>
        /// 表数据
        /// </summary>
        public IEnumerable<ScanInfo> ScanInfoCollection { get; set; } 
    }
}