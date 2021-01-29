using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class AddOrUpdateWarehouseLocationRequest
    {
        /// <summary>
        /// 库位信息
        /// </summary>
        public  LocationInfo Location { get; set; }
    }
}
