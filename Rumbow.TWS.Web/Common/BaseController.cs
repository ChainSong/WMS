using System;
using System.Web.Mvc;
using Runbow.TWS.Biz.System;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Web.Common
{
    public class BaseController : Controller
    {
        public ActionResult Error(string msg = null)
        {
            return View("Error", (object)msg);
        }

        protected virtual bool AutoLoadPageStyleAndScriptFiles
        {
            get { return true; }
        }

        protected void SetReadonlyFlag(bool isReadonly = true)
        {
            ViewBag.___IsReadonly___ = isReadonly;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UserInfo == null)
            {
                filterContext.Result = new RedirectResult("/Login");
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }

            if (!AutoLoadPageStyleAndScriptFiles)
            {
                ViewBag.PageCssFile = false;
                ViewBag.PageJsFile = false;
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (string.Equals("POST", filterContext.HttpContext.Request.RequestType, StringComparison.OrdinalIgnoreCase))
            {
                new LogService().Log(new LogRequest() { LogHistory = new LogHistory() { Name = UserInfo.Name, Time = DateTime.Now, Action = filterContext.RouteData.Values["action"].ToString() } });
            }
        }

        public User UserInfo
        {
            get
            {
                return Session[Constants.USER_INFO_KEY] as User;
            }
        }
    }
}