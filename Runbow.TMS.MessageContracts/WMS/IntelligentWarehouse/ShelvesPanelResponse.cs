using Runbow.TWS.Entity.WMS.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.IntelligentWarehouse
{
    public class ShelvesPanelResponse
    {
        public IEnumerable<ShelvesPanel> shelvesPanel { get; set; }

        public IEnumerable<Instructions> instructions { get; set; }

        public IEnumerable<PickUpGoodsWall> pickUpGoodsWall { get; set; }

        public IEnumerable<Instruction_Order_Mapping> mapping { get; set; }

    }
}
