using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD
{
    public class SuperPOD : Pod
    {
        [EntityPropertyExtension("ExistPod", "ExistPod")]
        public string ExistPod { get; set; }
    }
}
