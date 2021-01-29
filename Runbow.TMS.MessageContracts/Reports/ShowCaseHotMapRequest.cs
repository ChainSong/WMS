using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.Reports
{
    public class ShowCaseHotMapRequest
    {
        public int Customer { get; set; }
        public string City { get; set; }
        public string EndActualDeliveryDate { get; set; }
        public string StartActualDeliveryDate { get; set; }
        public int HotMapType { get; set; }
    }
}
