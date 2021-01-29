using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Common.Layui
{
    public class ResponseData
    {
        public int code { get; set; }

        public string msg { get; set; }

        public int count { get; set; }

        public IEnumerable<object> data
        {
            get; set;
        }
    }
}
