using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    /// <summary>
    /// 
    /// </summary>
    public class AddScanDatasRequest
    {
        public IEnumerable<ScanInfo> scanInfos { get; set; }
    }
}
