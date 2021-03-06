﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class QueryAMSUploadResponses
    {
        public IEnumerable<AMSUpload> AMSUploadCollection { get; set; }

        public IEnumerable<WMS_Package> WMS_PackageCollection { get; set; }

        public IEnumerable<WMS_SFDetail> WMS_SFDetaileCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    } 
}
