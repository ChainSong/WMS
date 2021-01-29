using System.Collections.Generic;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.POD.Distribution;

namespace Runbow.TWS.MessageContracts
{
    public class DbToExcelsResponse
    {
        public IEnumerable<DbToExcel> PodExcel { get; set; }
    }
}