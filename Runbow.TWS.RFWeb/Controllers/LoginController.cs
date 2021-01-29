using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.RFWeb.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }
        [HttpPost]
        public string CheckUser(string UserName, string Password)
        {

            return "1";
        }       
    }
}
