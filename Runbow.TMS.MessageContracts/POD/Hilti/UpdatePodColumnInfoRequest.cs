using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Runbow.TWS.MessageContracts.POD.Hilti
{
    public   class UpdatePodColumnInfoRequest
    {
        public DataTable PodColumnInfo { get; set; }
        public bool result { get; set; }
    }
}
