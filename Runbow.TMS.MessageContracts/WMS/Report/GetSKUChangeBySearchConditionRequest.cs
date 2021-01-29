using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Report;

namespace Runbow.TWS.MessageContracts.WMS.Report
{
    public class GetSKUChangeBySearchConditionRequest
    {
        public ReportSKUChangeSearchCondition SKUChangeSearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

    }
}
