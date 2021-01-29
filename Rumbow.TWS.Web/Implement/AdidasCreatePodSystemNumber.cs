using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Web.Interface;

namespace Runbow.TWS.Web.Implement
{
    public class AdidasCreatePodSystemNumber : ICreatePodSystemNumber
    {
        public string CreatePodSystemNumber()
        {
            return new DefaultCreatePodSystemNumber().CreatePodSystemNumber();
        }

        public int GetTodaysPodNumber(out string systemNumberPrefix)
        {
            return new DefaultCreatePodSystemNumber().GetTodaysPodNumber(out systemNumberPrefix);
        }
    }
}