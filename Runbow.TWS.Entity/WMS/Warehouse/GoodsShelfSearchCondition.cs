﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class GoodsShelfSearchCondition:GoodsShelfInfo
    {
       
        public long UserID { get; set; }
        public long LocationID { get; set; }
    }
}
