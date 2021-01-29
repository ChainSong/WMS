using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class AddOrUpdateWarehouseRequest
    {
        /// <summary>
        /// 仓库信息
        /// </summary>
        public WarehouseInfo warehouse { get; set; }
    }
}
