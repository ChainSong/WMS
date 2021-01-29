using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.WMS.Models.OrderManagement;

namespace Runbow.TWS.Web.Areas.WMS.Controllers.NikeOSRBJ
{
    public class NikeNFSPrintController : Controller
    {
        //
        // GET: /WMS/NikeNFSPrint/

        public ActionResult Index(string id, string type)
        {
            if (type == "1")
            {
                GetOrderByConditionResponse PackageModel = new OrderManagementService().GetPackageByCondition(int.Parse(id.ToString())).Result;
                return View(PackageModel);
            }
            else if (type == "0")
            {
                return View();
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// GZNFS箱清单打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult PrintBox(string id, string type)
        {
            NFSPrintBox PackageModel = new NFSPrintBox();

            PackageModel.EnumerableCustomerInfo = new OrderManagementService().GetNFSPackageBoxCarton(id.ToString(), type).EnumerableBoxListinfo;

            return View(PackageModel);



        }


    }
}
