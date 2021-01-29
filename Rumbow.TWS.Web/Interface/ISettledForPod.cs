using System.Collections.Generic;
using System.Text;

namespace Runbow.TWS.Web.Interface
{
    internal interface ISettledForPod
    {
        void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID,int Target,long CustomerOrShipperID,long RelatedCustomerID, StringBuilder Message);

        void SettledPodForPay(IEnumerable<long> PodIDs, string creator, long ProjectID,int Target,long CustomerOrShipperID,long RelatedCustomerID, StringBuilder Message);
    }

    internal interface IsettledForPodNew
    {
        StringBuilder SettledForPod();
    }
}