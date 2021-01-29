using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public  class AMSSearchCondition
    {
        public string Numbers { get; set; }

        public DateTime? StatUpLoadTime { get; set; }

        public DateTime? EndUpLoadTime { get; set; }

        public long? CustomerID { get; set; }

        public string BoxNumber { get; set; }

        public int? StateID { get; set; }

        public int? StateNumID { get; set; }
    }
}
