using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.Reports;

namespace Runbow.TWS.MessageContracts.Reports
{
    public class ShowCaseHotMapResponse
    {
        public IEnumerable<ShowCaseHotMap> showCaseHotMap { get; set; }
    }
}
