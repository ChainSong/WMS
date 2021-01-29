using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.ForecastWarehouse
{
    public class ForecastWarehouseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ForecastWarehouse";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ForecastWarehouse_default",
                "ForecastWarehouse/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
