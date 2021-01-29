using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.POD.AKZO
{
    public  class GetAbnormalPODSearchRequest
    {
        public string  SqlWhere { get; set; }
        public DataTable AbnormalTable { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<long> PodIDList { get; set; }
        public IEnumerable<Pod> PodCollection { get; set; }
    }
}
