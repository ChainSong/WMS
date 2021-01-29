using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class CreditUpdate
    {
        public bool ISORSUCCESS { get; set; }
        public string ERRORSOURCEVALUE { get; set; }
        public string Message { get; set; }
    }
}