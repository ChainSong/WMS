using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity.POD;
using Runbow.TWS.MessageContracts.POD.Adidas;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class GetBAFMobile
    {
      
        public IEnumerable<BAFPriceInfo> abfRiceInfos { get; set; }

        public BAFPriceInfo abfRiceInfo { get; set; }

        public ABFPriceRequest Request { get; set; }

        public ABFPriceResponses Responses { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public string BAFStartTime { get; set; }

        public string BAFEndTime { get; set; }

        public decimal BAFPrice { get; set; }
    }
}