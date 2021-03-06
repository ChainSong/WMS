﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class QueryTYscanRequests
    {
        public TYscanSearchCondition SearchCondition { get; set; }
        public TYscanDetailSearchCondition SearchConditionDetail { get; set; }

        public long ID { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Customers { get; set; }

        //public IEnumerable<long> CustomerIDs { get; set; }
    }
}
