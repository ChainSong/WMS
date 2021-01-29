using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.POD
{
    public class PODAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "POD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "POD_default",
                "POD/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}