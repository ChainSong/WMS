using System;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Areas.System.Models;
using UtilConstants = Runbow.TWS.Common.Constants;
using System.Collections.Generic;
using System.Linq;


namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class CustomerController : BaseController
    {
        [HttpGet]
        public ActionResult Create(int? id, CustomerModel vm, int? ViewType)
        {

            vm.ViewType = ViewType != null ? (int)ViewType : 2;
           
            var segments = new SegmentService().GetSegmentsByCondition(new GetSegmentsByConditionRequest() { Name = "", State = true }).Result;
            vm.Segments = segments.Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Name + "------>>详情>>" + s.Description });
            if (id != null && id > 0)
            {
                string segmentId = new SegmentService().GetSegmentByCursterId((int)base.UserInfo.ProjectID, (int)id).Result;
                vm.SelectedCustomerOrShipperSegment = long.Parse(segmentId);
                CustomerService customer = new CustomerService();
                Customer cus = customer.selectCustomer((int)id);
                vm.ConvertDesc(cus);

            }
            else {
                vm.State = true;
            }
         
            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(CustomerModel vm)
        {
            CustomerService service = new CustomerService();
            IList<Customer> customer = new List<Customer>();
            
            var segments = new SegmentService().GetSegmentsByCondition(new GetSegmentsByConditionRequest() { Name = "", State = true }).Result;
            vm.Segments = segments.Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Name + "------>>详情>>" + s.Description });
            vm.Types = vm.StoreStatus > 0 ? 1 : 0;
            customer.Add(vm.Convert());
            Response<IEnumerable<Customer>> response = service.AddCustomer(new AddCustomerRequest() { customers = customer }, base.UserInfo.ID.ToString(), base.UserInfo.Name, (int)base.UserInfo.ProjectID, vm.SelectedCustomerOrShipperSegment.ToString() == "" ? 0 : (int)vm.SelectedCustomerOrShipperSegment);
            if (vm.ID > 0) 
            {
                Customer cus = service.selectCustomer((int)vm.ID);
                vm.ConvertDesc(cus);
            }

            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshApplicationCustomers();
                ApplicationConfigHelper.RefreshGetApplicationCustomer();
                ApplicationConfigHelper.RefreshCustomers();
                if (vm.StoreType > 1)
                    ViewBag.Message = response.Result.Select(c => c.ID).FirstOrDefault().ToString();
                else
                    ViewBag.Message = "0";
            }
            return View(vm);
        }


        /// <summary>
        /// 验证客户名称唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsEdit">是否是编辑</param>
        /// <returns></returns>
        public string CheckName(string Name, int? Id, bool IsEdit)
        {
            CustomerService customer = new CustomerService();
            return customer.CheckNameIsExist(Name.Trim(), Id,base.UserInfo.ProjectID.ToString(), IsEdit);
        }

        [HttpGet]
        public ActionResult Index(int? customerType)
        {            
            CustomerListViewModel vm = new CustomerListViewModel();

            vm.Types = customerType != null ? (int)customerType : 0;
            vm.State = true;
            var response = new CustomerService().GetCustomerByConditon(new GetCustomerByConditionRequest() { Code = "", Name = "", UserId = base.UserInfo.ID, ProjectId = base.UserInfo.ProjectID, State = true, StoreType = 99, PageSize = UtilConstants.PAGESIZE, PageIndex = 0 });
            if (response.IsSuccess)
            {
                vm.Customer = response.Result.Customer;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                
            }
            else
            {
                ViewBag.Message = "查询失败.";
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(CustomerListViewModel vm, int? PageIndex)
        {
            var response = new CustomerService().GetCustomerByConditon(new GetCustomerByConditionRequest() { Code = string.IsNullOrEmpty(vm.Code) ? "" : vm.Code, Name = string.IsNullOrEmpty(vm.Name) ? "" : vm.Name, UserId = base.UserInfo.ID, ProjectId = base.UserInfo.ProjectID, StoreType = vm.StoreType, State = vm.State, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 });
            if (response.IsSuccess)
            {
                vm.Customer = response.Result.Customer;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            else
            {
                ViewBag.Message = "查询失败.";
            }

            return View(vm);
        }

        public ActionResult Edit(int ID)
        {
            CustomerService customer=new CustomerService();
            Customer cus= customer.selectCustomer(ID);
            CustomerModel c = new CustomerModel();
            //ID, Code, Name, Description, State, CreateDate, Email, LawPerson, PostCode, 
            c.ID = cus.ID;
            c.Code = cus.Code;
            c.Name = cus.Name;
            c.Description = cus.Description;
            c.State = cus.State;
            c.Email = cus.Email;
            c.LawPerson = cus.LawPerson;
            c.PostCode = cus.PostCode;

            

            //Address1, Address2, Bank, Account, TaxID, InvoiceTitle, Contactor1, Title1, Phone1,
            c.Address1 = cus.Address1;
            c.Address2 = cus.Address2;
            c.Bank = cus.Bank;
            c.TaxID = cus.TaxID;
            c.InvoiceTitle = cus.InvoiceTitle;
            c.Contactor1 = cus.Contactor1;
            c.Title1 = cus.Title1;
            c.Phone1 = cus.Phone1;
            c.Fax1 = cus.Fax1;
            //Fax1, Contactor2, Title2, Phone2, Fax2, WebSite, RegistAdd
            c.Contactor2 = cus.Contactor2;
            c.Title2 = cus.Title2;
            c.Phone2 = cus.Phone2;
            c.Fax2 = cus.Fax2;
            c.WebSite = cus.WebSite;
            c.RegistAdd = cus.RegistAdd;

            return View(c);
        }
        

        [HttpPost]
        public ActionResult Edit(CustomerModel vm)
        {
            vm.ResponseCustomer = new CustomerService().UpdateCustomer(new AddCustomerRequest() { Customer = vm.Convert() });
            if (vm.ResponseCustomer.IsSuccess)
            {
                ViewBag.Message = "编辑成功!";

            }
            ApplicationConfigHelper.RefreshProjectUserCustomers();
            ApplicationConfigHelper.RefreshApplicationCustomers();
            return RedirectToAction("List");
          
        }

        [HttpPost]
        public string Delete(long ID)
        {
            CustomerService service = new CustomerService();
            Response<Customer> response = new Response<Customer>();
            try
            {

                 response = service.DeleteCustomer(ID);
                if (response.IsSuccess)
                {
                    ApplicationConfigHelper.RefreshProjectUserCustomers();
                    ApplicationConfigHelper.RefreshApplicationCustomers();
                }
            }
            catch 
            {

            }
            return response.SuccessMessage;
        }
    }
}