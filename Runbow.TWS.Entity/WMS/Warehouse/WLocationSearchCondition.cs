using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class WLocationSearchCondition
    {
        public string WarehouseID { get; set; }
        public string AreaID { get; set; }
        public string Location { get; set; }
        public string LocationType { get; set; }
        public string LocationStatus { get; set; }
        public string Classification { get; set; }
        public string Category { get; set; }
        public string Handling { get; set; }
        public string ABCClassification { get; set; }
        public string IsMultiLot { get; set; }
        public string IsMultiSKU { get; set; }
        public string LocationLevel { get; set; }
        public string SearchType { get; set; }
    }
}
