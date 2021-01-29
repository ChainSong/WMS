using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class SettledPodCompare : SettledPod
    {
        public decimal? CompareShipAmt { get; set; }

        public decimal? CompareBAFAmt { get; set; }

        public decimal? ComparePointAmt { get; set; }

        public decimal? CompareOtherAmt { get; set; }

        public decimal? CompareTotalAmt { get; set; }
    }
}
