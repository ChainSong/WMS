using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.Finance.Models;
using Runbow.TWS.Web.Common;
using UtilConstants = Runbow.TWS.Common.Constants;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using Runbow.TWS.Common;

namespace Runbow.TWS.Web.Areas.Finance.Controllers
{
    public class ReceiveOrPayOrdersController : BaseController
    {
        public ActionResult ReceiveOrPay(long id)
        {
            ReceiveOrPayViewModel vm = new ReceiveOrPayViewModel();
            var InvoiceResponse = new InvoiceService().GetInvoiceByID(new GetInvoiceByIDRequest() { ID = id });
            if (!InvoiceResponse.IsSuccess)
            {
                return Error(InvoiceResponse.Exception.Message);
            }

            vm.Invoice = InvoiceResponse.Result;

            var ReceiveOrPayOrdersResponse = new ReceiveOrPayOrdersService().GetReceiveOrPayOrderByInvoiceID(new GetReceiveOrPayOrderByInvoiceIDRequest() { InvoiceID = id });
            if (!ReceiveOrPayOrdersResponse.IsSuccess)
            {
                return Error(ReceiveOrPayOrdersResponse.Exception.Message);
            }

            vm.ReceiveOrPayOrders = ReceiveOrPayOrdersResponse.Result;

            return View(vm);
        }

        [HttpPost]
        public JsonResult CancelOrCompleteInvoice(long id, bool currentState)
        {
            var response = new ReceiveOrPayOrdersService().CompleteOrCancelInvoice(new CompleteOrCancelInvoiceRequest() { ID = id, CurrentCompleteState = currentState });
            if (response.IsSuccess)
            {
                return Json(new { Message = !currentState ? "发票确认完成成功" : "发票取消完成成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = !currentState ? "发票确认完成失败！" : "发票取消完成失败！", IsSuccess = false });
            }
        }

        [HttpPost]
        public JsonResult DeleteReceiveOrPayOrder(long id)
        {
            var response = new ReceiveOrPayOrdersService().DeleteReceiveOrPayOrder(new DeleteReceiveOrPayOrderRequest() { ID = id });
            if (response.IsSuccess)
            {
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "删除失败！", IsSuccess = false });
            }
        }

        [HttpPost]
        public JsonResult AddReceiveOrPayOrders(long InvoiceID, string InvoiceNumber, int Target, long CustomerOrShipperID,
            string CustomerOrShipperName, decimal AMT, DateTime Date, string Remark, long RelatedCustomerID)
        {
            var response = new ReceiveOrPayOrdersService().AddReceiveOrPayOrders(new AddReceiveOrPayOrdersRequest()
            {
                ReceiveOrPayOrders = new ReceiveOrPayOrders()
                {
                    ReceiveOrPayNumber = (Target==0 ? "SK" : "FK") + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    InvoiceID = InvoiceID,
                    InvoiceNumber = InvoiceNumber,
                    Target = Target,
                    CustomerOrShipperID = CustomerOrShipperID,
                    CustomerOrShipperName = CustomerOrShipperName,
                    AMT = AMT,
                    Date = Date,
                    Remark = Remark,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now,
                    RelatedCustomerID = RelatedCustomerID
                }
            });

            if (response.IsSuccess)
            {
                return Json(new { IsSuccess = true, ID=response.Result });
            }
            else
            {
                return Json(new { IsSuccess = false, Message=response.Exception.Message });
            }
        }

    }
}
