using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class SettledPodSearchCondition : SettledPod
    {
        public DateTime? EndActualDeliveryDate { get; set; }

        public DateTime? EndCreateTime { get; set; }

        public DateTime? EndDateTime1 { get; set; }

        public DateTime? EndDateTime2 { get; set; }

        public IEnumerable<long> CustomerIDs { get; set; }

        public int UserType { get; set; }

        public string SystemNumberSufixx { get; set; }

        public bool IsManualSettled { get; set; }

        public bool IsForAudit { get; set; }

        public string StartCities { get; set; }

        public string EndCities { get; set; }
    }
}
