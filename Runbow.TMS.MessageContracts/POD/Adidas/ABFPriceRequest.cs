using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.POD;

namespace Runbow.TWS.MessageContracts.POD.Adidas
{
    public class ABFPriceRequest
    {
        public BAFPriceInfo Info { get; set; }

        public decimal BAFPrice { get; set; }

        public string BAFStartTime { get; set; }

        public string BAFEndTime { get; set; }

        public int ProjectID { get; set; }

        public string ProjectName { get; set; }

        public int TragetID { get; set; }

        public string TragetName { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }


    }
}
