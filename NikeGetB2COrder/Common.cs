using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeGetB2COrder
{
    public class Common
    {
        internal static readonly string SHWMSSqlConnection = ConfigurationManager.ConnectionStrings["SHWMS"].ConnectionString.ToString();
        internal static readonly string CDWMSSSqlConnection = ConfigurationManager.ConnectionStrings["CDWMS"].ConnectionString.ToString();


    }
}
