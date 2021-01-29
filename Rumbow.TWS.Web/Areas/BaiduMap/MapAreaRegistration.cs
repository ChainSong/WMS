using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.BaiduMap
{
    public class MapAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BaiduMap";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BaiduMap_default",
                "BaiduMap/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
