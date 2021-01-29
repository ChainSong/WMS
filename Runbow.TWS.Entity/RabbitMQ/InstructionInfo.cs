using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.RabbitMQ
{
    public class InstructionInfo
    {
        [EntityPropertyExtension("id", "id")]
        public long id { get; set; }
        [EntityPropertyExtension("goodsShelve", "goodsShelve")]
        public string goodsShelve { get; set; }
        [EntityPropertyExtension("x", "x")]
        public string x { get; set; }
        [EntityPropertyExtension("y", "y")]
        public string y { get; set; }
        [EntityPropertyExtension("releatedDetailID", "releatedDetailID")]
        public long releatedDetailID { get; set; }
        [EntityPropertyExtension("workStation", "workStation")]
        public string workStation { get; set; }
    }
}
