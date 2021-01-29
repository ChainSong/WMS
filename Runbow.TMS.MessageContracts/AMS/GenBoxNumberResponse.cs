using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.AMS
{
    public class GenBoxNumberResponse
    {
        public IEnumerable<AMSUpload> AMSUploadCollection { get; set; }

        public int PageIndex { get; set; } 

        public int PageCount { get; set; }
    }
}
