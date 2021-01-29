using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.AMS
{
    public class GenBoxNumberRequest
    {
        public AMSSearchCondition SearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Customers { get; set; } 

        public string Check { get; set; }
    }
}
