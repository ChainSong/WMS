﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.POD;

namespace Runbow.TWS.MessageContracts.POD.Adidas
{
    public class ABFPriceResponses
    {
        public IEnumerable<BAFPriceInfo> bafPriceInfo { get; set; }

        public int   PageCount { get; set; }
                   
        public int   PageIndex { get; set; }
    }
}
