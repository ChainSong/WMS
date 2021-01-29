using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.CRM.Models
{
    public class CrmViewInfoViewModel
    {
        public IEnumerable<CRMTrackInfo> CRMTrackInfoList { get; set; }
        public CRMInfo CRMInfo { get; set; }
        public int TypeID { get; set; }
    }
}