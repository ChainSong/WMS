using System;
using System.Collections.Generic;
using Runbow.TWS.Entity.POD;

namespace Runbow.TWS.Entity
{
    public class PodSearchCondition : Pod
    {
        public DateTime? EndActualDeliveryDate { get; set; }

        public DateTime? StartCreateTime { get; set; }

        public DateTime? EndCreateTime { get; set; }

        public DateTime? EndDateTime1 { get; set; }

        public DateTime? EndDateTime2 { get; set; }

        public DateTime? EndDateTime3 { get; set; }

        public DateTime? EndDateTime4 { get; set; }

        public DateTime? EndDateTime5 { get; set; }

        public DateTime? EndDateTime6 { get; set; }

        public DateTime? EndDateTime7 { get; set; }

        public DateTime? EndDateTime8 { get; set; }

        public DateTime? EndDateTime9 { get; set; }

        public DateTime? EndDateTime10 { get; set; }

        public DateTime? EndDateTime11 { get; set; }

        public DateTime? EndDateTime12 { get; set; }

        public DateTime? EndDateTime13 { get; set; }

        public DateTime? EndDateTime14 { get; set; }

        public DateTime? EndDateTime15 { get; set; }

        public bool ShipperIDIsNull { get; set; }

        public int UserType { get; set; }

        public IEnumerable<long> CustomerIDs { get; set; }

        public IList<int> Types { get; set; }

        public bool IsUsedForOriginalPOD { get; set; }
        public bool IsPaging { get; set; }
        public string StartCities { get; set; }

        public string EndCities { get; set; }

        public int? PodMinStateID { get; set; }

        public bool? HasAllocateShipper { get; set; }

        public string RuleArea { get; set; }

        public string UserID { get; set; }

        public PODDistributionVehicle podDistributionVehicle { get; set; }
    }
}