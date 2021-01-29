using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Common;

namespace Runbow.TWS.Web.Implement
{
    public class SettledForSHKXInRunbowProject : ISettledForPod
    {
        #region ISettledForPod Members

        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
           
        }

        public void SettledPodForPay(IEnumerable<long> PodIDs, string creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            
        }

        #endregion
    }
}