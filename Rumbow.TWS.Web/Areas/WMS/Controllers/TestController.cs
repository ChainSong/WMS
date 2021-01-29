using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class TestController : AsyncController
    {
        //
        // GET: /WMS/Test/

        public string Test(string name)
        {
            //for (int i = 0; i < 100000; i++)
            //{
            //    for (int j = 0; j < 50000; j++)
            //    {

            //    }
            //}
            Thread.Sleep(10000);
            return name + "25";
        }

        public ActionResult Index()
        {
            return View();
        }

        //public void IndexAsync()
        //{
        //    //AsyncManager.OutstandingOperations.Increment(2);//两个异步
        //    //AsyncManager.Parameters["name"] = "objectboy";

        //    ////for (int i = 0; i < 100000; i++)
        //    ////{
        //    ////    for (int j = 0; j < 50000; j++)
        //    ////    {

        //    ////    }
        //    ////}

        //    //Thread.Sleep(10000);

        //    //AsyncManager.OutstandingOperations.Decrement();

        //    //AsyncManager.Parameters["age"] = "25";
        //    //AsyncManager.OutstandingOperations.Decrement();
        //}
        //public ActionResult IndexCompleted(string name, string age)
        //{
        //    return Content(name + age);
        //}
    }
}
