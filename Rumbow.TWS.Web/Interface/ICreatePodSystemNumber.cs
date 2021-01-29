using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Web.Interface
{
    public interface ICreatePodSystemNumber
    {
        string CreatePodSystemNumber();

        int GetTodaysPodNumber(out string systemNumberPrefix);
    }
}
