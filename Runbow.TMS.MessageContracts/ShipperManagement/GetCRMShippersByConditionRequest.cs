using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.ShipperManagement;

namespace Runbow.TWS.MessageContracts
{
    public class GetCRMShippersByConditionRequest
    {
        public CRMShipperSearchCondition SearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }


        public IEnumerable<CRMShipper> CRMShipper { get; set; }

        public IEnumerable<InsertShipperExcel> InsertShipper {get;set;}

        public string User { get; set; }
    }
}
