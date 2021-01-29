using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class AddOrUpdateServicePeriodModel
    {
        public string EndCity { get; set; }
        public int  EndCityID { get; set; }
        
        public int Period { get; set; }
        public string ErrorValue { get; set; }


        public string SellName { get; set; }
        public int SellPhone { get; set; }
    }
}