using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace Runbow.TWS.MessageContracts.POD.Hilti
{
    public class UpdatePodInfoUpdateRequest
    {
        public bool ISORSUCCESS { get; set; }
        public string ERRORSOURCEVALUE { get; set; }
         
        public DataTable DeliverGoods { get; set; }
        public DataTable NotDeliverGoods { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
    }   
}
