using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.POD.Nike
{
    public class NikePodForBSCondition
    {
        public string SystemNumber { get; set; }

        public string CustomerOrderNumber { get; set; }

        public string Str1 { get; set; }

        public string PodStateName { get; set; }

        public string ShipperName { get; set; }

        public string TtlOrTplName { get; set; }

        public string StartCityName { get; set; }

        public string StartCities { get; set; }

        public string StartCityID { get; set; }

        public long EndCityID { get; set; }

        public string EndCityName { get; set; }

        public string EndCities { get; set; }

        public string StartDeliveryTime { get; set; }

        public string EndDeliveryTime { get; set; }

        public string ShipperID { get; set; }

        public int? IsConversion { get; set; }

        public string Carriers { get; set; }

        public string PODCarState { get; set; }
        
    }
}
