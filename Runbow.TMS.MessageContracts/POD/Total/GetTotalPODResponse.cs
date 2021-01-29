using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.POD.Total
{
    public class TotalPODResponse
    {
        public IEnumerable<Pod> TotalPODCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public int RowCount { get; set; }
    }
}
