
using Runbow.TWS.Biz.RFWeb;
using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.RFWeb.Controllers
{
    public class ReceiptController : Controller
    {
        //
        // GET: /Receipt/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetReceiptDetail(string ReceiptNumber)
        {
            IEnumerable<ASNDetail> recModelList;
            recModelList = new ReceiptManagementService().GetReceiptDetailList(ReceiptNumber);
            return View(recModelList);
        }

    }
}
