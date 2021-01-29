using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.POD.Hilti
{
    public  class AddOrUpdateServicePeriodRequest
    {
        public string  EndCity { get; set; }
        public int EndCityID { get; set; }
        public int Period { get; set; }
        public string ErrorValue { get; set; }


        public string SellName { get; set; }
        public string SellPhone { get; set; }
    }
}
