using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Web.Areas.WMS.Storer;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class StorerController : Controller
    {
        //
        // GET: /WMS/Storer/

        /// <summary>
        /// 货主
        /// </summary>
        public ActionResult Index(int? PageIndex, IndexViewModel vm)
        {
            return View(vm);
        }

        /// <summary>
        /// 货主编辑
        /// </summary>
        public ActionResult Create(CreateViewModel vm)
        {
            return View(vm);
        }

        /// <summary>
        /// 货主编辑
        /// </summary>
        public ActionResult Edit(IndexViewModel vm)
        {
            return View(vm);
        }

    }
}
