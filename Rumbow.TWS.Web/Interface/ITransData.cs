using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Interface
{
    internal interface ITransData
    {
        DataSet TransData(ref string message);

    }
}