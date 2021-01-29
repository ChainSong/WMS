﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetReceiptAndReceriptDetailsResponse
    {
         public IEnumerable<Receipt> ReceiptCollection { get; set; }

         public IEnumerable<ReceiptDetail> ReceiptDetailCollection { get; set; }


    }
}
