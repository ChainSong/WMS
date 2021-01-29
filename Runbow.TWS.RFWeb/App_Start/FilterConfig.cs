using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.RFWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}