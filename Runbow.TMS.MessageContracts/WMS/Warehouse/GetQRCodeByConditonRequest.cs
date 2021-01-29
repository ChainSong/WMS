using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetQRCodeByConditonRequest
    {
        public QRCodeSearchCondition SearchCondition { get; set; }
        public IEnumerable<QRCodeInfo> QRCode { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
