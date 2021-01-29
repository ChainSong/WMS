using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.LogisticsReport
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}