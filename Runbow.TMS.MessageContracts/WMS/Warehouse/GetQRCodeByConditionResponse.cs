using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetQRCodeByConditionResponse
    {
        public IEnumerable<QRCodeInfo> QRCodeCollection { get; set; }

        public IEnumerable<QRCodeInfo> OperationCollection { get; set; }

        public IEnumerable<QRCodeInfo> ChargeCollection { get; set; }
        

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
