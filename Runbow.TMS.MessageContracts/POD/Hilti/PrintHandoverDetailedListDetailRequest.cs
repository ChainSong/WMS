using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Runbow.TWS.MessageContracts.POD.Hilti
{
    public class PrintHandoverDetailedListDetailRequest
    {
        public DataTable DataList { get; set; }
        public string SqlWhere { get; set; }
        public string  ShipperName { get; set; }
        public DateTime?  BeginDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
