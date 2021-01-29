using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Biz.WebApi
{
    public class Ytofeedback
    {
        public string logisticProviderID { get; set; }
        public string txLogisticID { get; set; }
        public string clientID { get; set; }
        public string mailNo { get; set; }
        public string code { get; set; }
        public string success { get; set; }
        public distributeInfoYto distributeInfo { get; set; }
    }

    public class distributeInfoYto
    {
        public string shortAddress { get; set; }
        public string consigneeBranchCode { get; set; }
        public string packageCenterCode { get; set; }
        public string packageCenterName { get; set; }
    }

}
