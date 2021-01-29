using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Runbow.TWS.Web.Interface;

namespace Runbow.TWS.Web.Implement
{
    public class SettledDefault: ISettledForPod
    {
        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            
        }

        public void SettledPodForPay(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            
        }
    }
}