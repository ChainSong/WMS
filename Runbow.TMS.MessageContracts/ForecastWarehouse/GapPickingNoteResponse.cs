using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.ForecastWarehouse;

namespace Runbow.TWS.MessageContracts.ForecastWarehouse
{
    public class GapPickingNoteResponse
    {
        public UserCode Code { get; set; }

        public GapPickingNote GapPickingNote { get; set; }
    }
}
