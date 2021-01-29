using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetAsnOrReceiptOrDetailByConditionResponse
    {
        public IEnumerable<ASNDetailByProc> ASNDetailCollections { get; set; }
        public IEnumerable<ASNDetail> ASNDetailCollection { get; set; }
        public ASN ASN { get; set; }
        public DataTable dtAsn { get; set; }
        public DataTable dtAsnDetail { get; set; }  
        public IEnumerable<ReceiptDetail> ReceiptDetailCollection { get; set; }
        public Receipt Receipt { get; set; }
        

        public int PageCount { get; set; } 

        public int PageIndex { get; set; }
    }
}
