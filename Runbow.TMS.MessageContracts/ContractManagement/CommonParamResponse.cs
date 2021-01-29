using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class CommonParamResponse
    {
        public IEnumerable<ContractConfig> configList { get; set; }
    }
}
