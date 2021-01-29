using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.ShipperManagement
{
    public class InsertShipperExcel
    {
        [EntityPropertyExtension("Name","Name")]
        public string Name {get; set;}

        [EntityPropertyExtension("TransportMode", "TransportMode")]
        public string TransportMode { get; set; }

        [EntityPropertyExtension("Code", "Code")]
        public string Code { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("Creater", "Creater")]
        public string Creater { get; set; }
    }
}
