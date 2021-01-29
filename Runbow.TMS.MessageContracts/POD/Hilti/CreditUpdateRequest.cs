using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.POD.Hilti
{
    public class CreditUpdateRequest
    {
        public bool ISORSUCCESS { get; set; }
        public string ERRORSOURCEVALUE { get; set; }
        public DataTable OrderNoinfo { get; set; }
    }
}
