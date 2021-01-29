using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPackageAndDetailRequest
    {
        public IEnumerable<PackageInfo> packages { get; set; }
        public IEnumerable<PackageDetailInfo> packageDetails { get; set; }
       
    }
}
