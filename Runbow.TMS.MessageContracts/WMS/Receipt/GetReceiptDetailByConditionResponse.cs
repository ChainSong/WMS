using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
namespace Runbow.TWS.MessageContracts
{
    public class GetReceiptDetailByConditionResponse
    {
        public IEnumerable<ReceiptDetail> ReceiptDetailCollection { get; set; }
        public IEnumerable<ReceiptDetail> ReceiptDetailCollection2 { get; set; }
        public IEnumerable<ReportReceiptReport> ReceiptDetailCollection3 { get; set; }
        public Receipt Receipt { get; set; }
        public IEnumerable<Receipt> ReceiptCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
