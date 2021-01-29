using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Common
{
    public class ResponseModel
    {
        public int code { get; set; }

        public string msg { get; set; }

        public int count { get; set; }

        public IEnumerable<object> data { get; set; }

        public object singleData { get; set; }
    }
}