﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.AMS
{
   public  class QueryAMSUploadRequests
   {
        public AMSSearchCondition SearchCondition { get; set; }

        public WMS_Package WMS_PackageSearch { get; set; }

        public WMS_SFDetail WMS_SFDetailSearch { get; set; }

        public IEnumerable<WMS_SFDetail> WMS_SFDetailList { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        
        public string Customers { get; set; }

        public IEnumerable<long> CustomerIDs { get; set; }
   }
}
