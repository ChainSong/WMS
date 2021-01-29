using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.SettlementManagement;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.SettlementManagement
{
    public class HiltiModel
    {
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int SumCount1 { get; set; }
        public int SumCount2 { get; set; }

        public decimal? inpTotal { get; set; }
        public decimal? inpAvg { get; set; }
        public int? number3 { get; set; }
        public decimal? opCost3 { get; set; }
        public int? number4 { get; set; }
        public decimal? opCost4 { get; set; }
        public decimal? t1Price { get; set; }
        public int? inNumber1 { get; set; }
        public decimal? inCost1 { get; set; }
        public int? inNumber2 { get; set; }
        public decimal? inCost2 { get; set; }
        public int? inNumber3 { get; set; }
        public decimal? inCost3 { get; set; }
        public int? inNumber4 { get; set; }
        public decimal? inCost4 { get; set; }
        public int? inNumber5 { get; set; }
        public decimal? inCost5 { get; set; }
        public int? inNumber6 { get; set; }
        public decimal? inCost6 { get; set; }
        public int? inNumber7 { get; set; }
        public decimal? inCost7 { get; set; }
        public decimal? t2Price { get; set; }

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
       new SelectListItem(){Value="50",Text="喜利得北京仓"}
       };

        public int PageIndex { get; set; }
        public int PageCount { get; set; }

    }
}