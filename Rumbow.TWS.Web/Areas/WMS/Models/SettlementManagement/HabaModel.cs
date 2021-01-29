using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.SettlementManagement;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.SettlementManagement
{
    public class HabaModel
    {


        public decimal? vo1 { get; set; }
        public decimal? vo1Cost { get; set; }
        public decimal? sum3 { get; set; }
        public int? sum2 { get; set; }
        public int? sum1 { get; set; }
        public decimal? sumCost1 { get; set; }
        public decimal? sumCost2 { get; set; }
        public decimal? sumCost3 { get; set; }
        public decimal? sumCost4 { get; set; }

        public decimal? CzMj { get; set; }
        public decimal? CzSr { get; set; }


        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? Price3 { get; set; }
        public decimal? Price4 { get; set; }
        public decimal? Price5 { get; set; }
        public decimal? Price6 { get; set; }


        public decimal Mergercost { get; set; }

        public int MyProperty { get; set; }
        public DateTime CreateTime { get; set; }
        public IEnumerable<WMS_HiltibjSettled> SettlementCollection { get; set; }
        public WMS_HiltibjSettled SearchCondition { get; set; }

        public DateTime? StartSettlementdate { get; set; }
        public DateTime? EndSettlementdate { get; set; }
        public string DateTime1 { get; set; }
        public long? WarehouseID { get; set; }
        public IEnumerable<SelectListItem> SettlementCustomerName = new List<SelectListItem>(){
       new SelectListItem(){Value="98",Text="HABA"}
       };

        public int PageIndex { get; set; }
        public int PageCount { get; set; }

    }
}