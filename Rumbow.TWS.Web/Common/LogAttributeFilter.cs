﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz.System;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Web.Common
{
    public class LogAttributeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (string.Equals("POST", filterContext.HttpContext.Request.RequestType, StringComparison.OrdinalIgnoreCase))
            {
                User user = filterContext.HttpContext.Session[Constants.USER_INFO_KEY] as User;
                new LogService().Log(new LogRequest() { LogHistory = new LogHistory() { Name = user.Name, Time = DateTime.Now, Action = filterContext.RouteData.Values["action"].ToString() } });
            }
        }
    }
}