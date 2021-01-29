using Runbow.TWS.Entity.WMS.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.System
{
    public class AddWMS_CustomerRequest
    {
        public WMS_Customer Customer { get; set; }

        public IEnumerable<WMS_Customer> customers { get; set; }


        public bool IsCoverOld { get; set; }

    }
}
