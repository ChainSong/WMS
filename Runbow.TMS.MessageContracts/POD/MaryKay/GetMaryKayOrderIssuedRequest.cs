using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.POD.MaryKay;

namespace Runbow.TWS.MessageContracts.POD.MaryKay
{
    public  class GetMaryKayOrderIssuedRequest
    {
        public string SqlWhere { get; set; }
        public DataTable OrderNoIssuedTable { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int ID { get; set; }
        public Pod PODEntity {get; set; }



        public string OrderNoIssuedStatus {get; set; }


        public DataTable YDTable { get; set; }
        public DataTable YZTable { get; set; }

        public MaryKay_InterfaceLog InterfaceLog { get; set; }

        public string UpLoadMKStatus { get; set; }
    }
}
