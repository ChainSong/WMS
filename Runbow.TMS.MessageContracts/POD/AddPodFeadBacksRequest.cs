﻿using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPodFeadBacksRequest
    {
        public IEnumerable<PodFeadBack> PodFeadBacks { get; set; }

        public long CustomerID { get; set; }
    }
}