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
    public class InvoiceController : BaseController
    {
        [HttpGet]
        public ActionResult Invoice(int Type, long CustomerOrShipperID, string IDs)
        {
            if (string.IsNullOrEmpty(IDs))
            {
                return Error();
            }

            var SettledPodResponse = new SettledService().GetSettledPodByIDs(new GetSettledPodByIDsRequest() { IDs = IDs.Split(',').Select(id => id.ObjectToInt64()) });
            if (!SettledPodResponse.IsSuccess)
            {
                return Error("获取结算单失败！");
            }

            if (SettledPodResponse.Result.Select(s => s.RelatedCustomerID).Distinct().Count() > 1)
            {
                return Error("多家客户的运单不能同时开在一张发票中");
            }

            InvoiceViewModel vm = new InvoiceViewModel();
            vm.IsViewModel = false;
            vm.SettledPods = SettledPodResponse.Result;
            vm.Invoice = new Invoice();
            vm.Invoice.Target = Type;
            vm.Invoice.ProjectID = base.UserInfo.ProjectID;
            var sum = SettledPodResponse.Result.Sum(s => s.TotalAmt);
            vm.Invoice.Sum = sum ?? 0;
            
            if (Type == 0)
            {
                var customer = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == CustomerOrShipperID);
                vm.Invoice.CustomerOrShipperID = customer.ID;
                vm.Invoice.CustomerOrShipperName = customer.Name;
                vm.Invoice.TaxID = customer.TaxID;
                vm.Invoice.Address = customer.Address1;
                vm.Invoice.Tel = customer.Phone1;
                vm.Invoice.Bank = customer.Bank;
                vm.Invoice.BankAccount = customer.Account;
                vm.Invoice.RelatedCustomerID = CustomerOrShipperID;
            }
            else
            {
                var shipper = ApplicationConfigHelper.GetApplicationShippers().First(s => s.ID == CustomerOrShipperID);
                vm.Invoice.CustomerOrShipperID = shipper.ID;
                vm.Invoice.CustomerOrShipperName = shipper.Name;
                vm.Invoice.TaxID = shipper.TaxID;
                vm.Invoice.Address = shipper.Address1;
                vm.Invoice.Tel = shipper.Phone1;
                vm.Invoice.Bank = shipper.Bank;
                vm.Invoice.BankAccount = shipper.Account;
                vm.Invoice.RelatedCustomerID = SettledPodResponse.Result.First().RelatedCustomerID;
            }

            vm.Invoice.SystemNumber = (Type == 0 ? "FPS" : "FPF") + DateTime.Now.ToString("yyyyMMddHHssmm");
            vm.InvoiceTypes = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.INVOICETYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });

            return View(vm);
        }

        [HttpPost]
        public ActionResult Invoice(InvoiceViewModel vm)
        {
            var settledPods = vm.ServerValues.FromJsonStringTo<IEnumerable<SettledPod>>();
            if (string.IsNullOrEmpty(vm.Invoice.InvoiceNumber))
            {
                vm.Invoice.InvoiceNumber = string.Empty;
            }

            vm.Invoice.InvoiceTypeName = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.INVOICETYPE).First(c => c.ID == vm.Invoice.InvoiceType).Name;
            vm.Invoice.Creator = base.UserInfo.Name;
            vm.Invoice.CreateTime = DateTime.Now;
            vm.Invoice.Remain = vm.Invoice.Sum;
            vm.Invoice.IsComplete = false;
            vm.Invoice.State = true;
            var Response = new InvoiceService().AddInvoice(new AddInvoiceRequest() { SettledPods = settledPods, Invoice = vm.Invoice });
            if (Response.IsSuccess)
            {
                return RedirectToAction("ViewInvoice", new { id = Response.Result, returnType = 2 });
            }

            return Error("新增发票失败");
        }

        [HttpGet]
        public ActionResult ViewInvoice(long id, int? returnType)
        {
            var SettledPodResponse = new SettledService().GetSettledPodsByInvoiceID(new GetSettledPodsByInvoiceIDRequest() { InvoiceID = id });
            if (!SettledPodResponse.IsSuccess)
            {
                return Error("获取结算单失败");
            }

            InvoiceViewModel vm = new InvoiceViewModel();
            vm.IsViewModel = true;
            vm.SettledPods = SettledPodResponse.Result;
            var InvoiceResponse = new InvoiceService().GetInvoiceByID(new GetInvoiceByIDRequest() { ID = id });
            if (!InvoiceResponse.IsSuccess)
            {
                return Error("获取发票失败");
            }
            vm.ReturnType = returnType ?? 1;
            vm.Invoice = InvoiceResponse.Result;

            return View("Invoice", vm);
        }

        public ActionResult InvoiceManage(int invoiceType, bool? forReceiveOrPay)
        {
            InvoiceManageViewModel vm = new InvoiceManageViewModel();
            vm.SearchCondition = new InvoiceSearchCondition();
            vm.SearchCondition.Target = invoiceType;
            vm.SearchCondition.ProjectID = base.UserInfo.ProjectID;
            vm.ForReceiveOrPay = forReceiveOrPay ?? false;
            vm.InvoiceTypes = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.INVOICETYPE).Select(i => new SelectListItem() { Value = i.ID.ToString(), Text = i.Name });
            if (invoiceType == 0)
            {
                vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c=>new SelectListItem(){Value=c.CustomerID.ToString(),Text = c.CustomerName});
            }
            else
            {
                vm.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.ShipperID.ToString(), Text = s.Name });
            }

            vm.SearchCondition.EstimateDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            vm.SearchCondition.EndEstimateDate = vm.SearchCondition.EstimateDate.Value.AddMonths(1).AddDays(-1);

            return View(vm);
        }

        [HttpPost]
        public ActionResult InvoiceManage(InvoiceManageViewModel vm)
        {
            if (base.UserInfo.UserType == 2)
            {
                vm.SearchCondition.ProjectUserCustomerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            }
            
            var GetInvoicesByConditionResponse = new InvoiceService().GetInvoicesByCondition(new GetInvoicesByConditionRequest() { SearchCondition = vm.SearchCondition });
            if (GetInvoicesByConditionResponse.IsSuccess)
            {
                vm.Invoices = GetInvoicesByConditionResponse.Result;
                vm.InvoiceTypes = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.INVOICETYPE).Select(i => new SelectListItem() { Value = i.ID.ToString(), Text = i.Name });
                if (vm.SearchCondition.Target == 0)
                {
                    vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                }
                else
                {
                    vm.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.ShipperID.ToString(), Text = s.Name });
                }

                return View(vm);
            }

            return Error(GetInvoicesByConditionResponse.Exception.Message);
        }

        [HttpPost]
        public void UpdateInvoiceNumber(long id, string invoiceNumber)
        {
            var UpdateInvoiceNumberResponse = new InvoiceService().UpdateInvoiceNumber(new UpdateInvoiceNumberRequest() { ID = id, InvoiceNumber = invoiceNumber });
            if (!UpdateInvoiceNumberResponse.IsSuccess)
            {
                throw UpdateInvoiceNumberResponse.Exception;
            }
        }

        [HttpPost]
        public JsonResult DeleteInvoice(long id)
        {
            var response = new InvoiceService().DeleteInvoice(new DeleteInvoiceRequest() { ID = id });
            if (response.IsSuccess)
            {
                return Json(new { Message = "发票作废成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "发票作废失败！", IsSuccess = false });
            }
        }
    }
}
