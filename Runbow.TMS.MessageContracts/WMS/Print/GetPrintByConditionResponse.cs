using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Print;

namespace Runbow.TWS.MessageContracts.WMS.Print
{
    public class GetPrintByConditionResponse
    {
        public IEnumerable<PrintHeader> PrintHeaderCollection { get; set; }

        public IEnumerable<PrintDetail> PrintDetailCollection { get; set; }

        public string Prompt { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
