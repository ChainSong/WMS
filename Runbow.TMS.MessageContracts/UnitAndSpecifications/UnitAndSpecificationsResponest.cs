using Runbow.TWS.Entity.WMS.UnitAndSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.UnitAndSpecifications
{
    public class UnitAndSpecificationsResponest
    {
        public IEnumerable<UnitAndSpecificationsInfo> unitAndSpecificationsInfos { get; set; }
    }
}
