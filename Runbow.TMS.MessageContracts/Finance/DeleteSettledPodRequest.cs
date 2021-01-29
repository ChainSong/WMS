using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class DeleteSettledPodRequest
    {
        public long ID { get; set; }

        public int SettledType { get; set; }
    }
}
