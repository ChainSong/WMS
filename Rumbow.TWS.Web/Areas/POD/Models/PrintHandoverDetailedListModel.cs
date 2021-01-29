using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PrintHandoverDetailedListModel
    {
        [Required(ErrorMessage = "必填")]
        [Display(Name = "承运商")]
        public string Shipper { get; set; }
        [Display(Name = "发货日期")]
        public string ActualDeliveryDate { get; set; }
        public DateTime? BeginActualDeliveryDate { get; set; }
        public DateTime? EndActualDeliveryDate { get; set; }

        [Display(Name = "分配日期")]
        public string DistributionDate { get; set; }
        public DateTime? BeginDistributionDate { get; set; }
        public DateTime? EndDistributionDate { get; set; }
        public int ShipperID { get; set; }
        public DataTable DateList { get; set; }
        public int UserType { get; set; }

       


    }
}