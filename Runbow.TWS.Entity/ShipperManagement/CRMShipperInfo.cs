using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public  class CRMShipperInfo
    {
        public CRMShipper CRMShipper { get; set; }

        public IEnumerable<CRMShipperTransportationLine> CRMShipperTransportationLineCollection { get; set; }

        public IEnumerable<CRMShipperCooperation> CRMShipperCooperationCollection { get; set; }

        public IEnumerable<CRMShipperTerminalInfo> CRMShipperTerminalInfoCollection { get; set; }
    }
}
