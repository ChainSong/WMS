using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using System.Data;

namespace Runbow.TWS.MessageContracts
{
    public class QueryDMSPODRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public string SystemOrderNo { get; set; }
        public string MkOrderNo { get; set; }

        public DateTime? BeginOrderNoDateTime { get; set; }
        public DateTime? EndOrderNoDateTime { get; set; }
        public string EndCity { get; set; }
        public string EndCityID { get; set; }
        public string ShipperName { get; set; }
        public string ShipperID { get; set; }
        public string IssuedStatusID { get; set; }
        public DataTable OrderNoIssuedTable { get; set; }
     
    }
}
