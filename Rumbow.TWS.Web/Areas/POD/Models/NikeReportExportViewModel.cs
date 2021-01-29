using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class NikeReportExportViewModel
    {
        
        [Display(Name = "客户运单号")]
        public string CustomerOrderNumber { get; set; }

        [MaxLength(50)]
        [Display(Name = "运输类型")]
        public string ShipperTypeName { get; set; }

        [MaxLength(50)]
        [Display(Name = "客户代码")]
        public string CustomerCode { get; set; }

        [MaxLength(50)]
        [Display(Name = "承运商")]
        public string ShipperName { get; set; }

        [MaxLength(50)]
        [Display(Name = "运单状态")]
        public string PodStateName { get; set; }

        [MaxLength(50)]
        [Display(Name = "实际发货时间")]
        public string ActualShipTime { get; set; }

       
        [Display(Name = "开始实际发货时间")]
        public DateTime? BeginActualShipTime { get; set; }

       
        [Display(Name = "结束实际发货时间")]
        public DateTime? EndActualShipTime { get; set; }

        [MaxLength(50)]
        [Display(Name = "预计发货时间")]
        public string ExpectedDeliveryTime { get; set; }

        [Display(Name = "开始预计发货时间")]
        public DateTime? BeginExpectedDeliveryTime { get; set; }


        [Display(Name = "结束预计发货时间")]
        public DateTime? EndExpectedDeliveryTime { get; set; }

        [MaxLength(50)]
        [Display(Name = "实际到货时间")]
        public string ActualCompleteTime { get; set; }

       
        [Display(Name = "开始实际到货时间")]
        public DateTime? BeginActualCompleteTime { get; set; }

       
        [Display(Name = "结束实际到货时间")]
        public DateTime? EndActualCompleteTime { get; set; }

        
        [MaxLength(50)]
        [Display(Name = "报表名称")]
        public string ReportName { get; set; }


        public long? SelectedPodStatesID { get; set; }

        public IEnumerable<SelectListItem> PodStates { get; set; }


        public long? SelectedShipperTypeID { get; set; }

        public IEnumerable<SelectListItem> ShipperTypes { get; set; }


        public long SelectedShipperID { get; set; }

        public IEnumerable<SelectListItem> Shippers { get; set; }

        
        public string SelectedReportID { get; set; }

        
        public List<SelectListItem> ReportNames { get; set; }

        public string  HtmlStr { get; set; }

        public long ShipperID { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }

        public bool IsExport { get; set; }


        public DataTable NikeReport { get; set; }
       
    }
}