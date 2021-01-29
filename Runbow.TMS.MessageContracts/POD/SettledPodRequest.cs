using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class SettledPodRequest
    {
        public IEnumerable<long> IDs { get; set; }

        public int SettledType { get; set; }
        //是否同天同客户结算,数字1代表是
        public int? IsID { get; set; }
        
    }
}