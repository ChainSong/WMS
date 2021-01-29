using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.System.Models;
using Runbow.TWS.Web.Common;
using ShipperListVM = Runbow.TWS.Web.Areas.System.Models.ShipperListViewModel;
using ShipperVM = Runbow.TWS.Web.Areas.System.Models.Shipper;
using UtilConstants = Runbow.TWS.Common.Constants;
using ShipperEntity = Runbow.TWS.Entity.Shipper;
using System.Data;

namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class ShipperController : BaseController
    {
        public ActionResult List()
        {
            ShipperListVM vm = new ShipperListVM();
            var response = new ShipperService().GetShippersByConditon(new GetShippersByConditionRequest() { ProjectId = base.UserInfo.ProjectID, Code = "", Name = "", EnglishName = "", State = true, PageSize = UtilConstants.PAGESIZE, PageIndex = 0 });
            if (response.IsSuccess)
            {
                vm.Shippers = response.Result.Shippers;
                vm.State = true;
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
        public ActionResult List(ShipperListVM vm, int? PageIndex)
        {
            var response = new ShipperService().GetShippersByConditon(new GetShippersByConditionRequest() { ProjectId=base.UserInfo.ProjectID, Code = string.IsNullOrEmpty(vm.Code) ? "" : vm.Code, Name = string.IsNullOrEmpty(vm.Name) ? "" : vm.Name, EnglishName = string.IsNullOrEmpty(vm.EnglishName) ? "" : vm.EnglishName, State = vm.State, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 });
            if (response.IsSuccess)
            {
                vm.Shippers = response.Result.Shippers;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            else
            {
                ViewBag.Message = "查询失败.";
            }

            return View(vm);
        }

        public ActionResult Create()
        {
            ShipperVM shipper = new ShipperVM();
            shipper.AvailableShipperIdentifies = ApplicationConfigHelper.GetApplicationConfigs(Constants.SHIPPERIDENTIFY);
            shipper.SelectedShipperIdentifies = shipper.AvailableShipperIdentifies;
            shipper.IsEdit = false;
            shipper.State = true;
            return View(shipper);
        }

        [HttpGet]
        public ActionResult ShiperSegment(int id,string message)
        {
            var customerOrShippers = new ProjectService().GetProjectCustomersOrShippers(new GetProjectCustomersOrShippersRequest() { ProjectID = base.UserInfo.ProjectID, Target = 1 }).Result.Where(cs => cs.CustomerOrShipperID == id);
            var customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID);
            var segments = new SegmentService().GetSegmentsByCondition(new GetSegmentsByConditionRequest() { Name = "", State = true }).Result;
            var projectShipperSegments = ApplicationConfigHelper.GetProjectCustomerOrShiperSegment().Where(p => p.Target == 1 && p.CustomerOrShipperID == id && p.ProjectID == base.UserInfo.ProjectID);


            CustomerOrShipperSegmentViewModel vm = new CustomerOrShipperSegmentViewModel()
            {
                Target = 1,
                SelectedCustomerOrShipperID = id.ToString(),
                ProjectID = base.UserInfo.ProjectID,
                ProjectCustomerOrShipperSegments = projectShipperSegments,
                ShipperName = customerOrShippers.Where(c => c.CustomerOrShipperID == id).FirstOrDefault().CustomerOrShipperName,
                CustomerOrShippersCollection = customerOrShippers.Select(s => new SelectListItem() { Value = s.CustomerOrShipperID.ToString(), Text = s.CustomerOrShipperName }),
                Segments = segments.Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Name + "------>>详情>>" + s.Description }),
                Customers = customers.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName })
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult ShiperSegment()
        {
            string segmentId= Request.Form["segmentId"].ToString().Substring(1); //多个段位拼接
            string relateCurmerId = Request.Form["relateCurmerId"].ToString().Substring(1);//段位对应的客户Id
            string SelectedCustomerOrShipperID = Request.Form["SelectedCustomerOrShipperID"].ToString();
           
            var response = new ProjectService().SetProjectCustomerOrShipperSegment(new SetProjectCustomerOrShipperSegmentRequest()
            {
                ProjectID = base.UserInfo.ProjectID,
                CustomerOrShipperID = long.Parse(SelectedCustomerOrShipperID),
                Target = 1,
                SegmentIDs = segmentId,
                RelatedCustomerIDs = relateCurmerId
            });


            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshProjectCustomerOrShiperSegment();
                ApplicationConfigHelper.RefreshProjectQuotedPrice();
                return RedirectToAction("ShiperSegment", new { id = SelectedCustomerOrShipperID, message = "更新成功" });
            }
            return RedirectToAction("ShiperSegment", new { id = SelectedCustomerOrShipperID});
        }

        [HttpPost]
        public ActionResult Create(ShipperVM model)
        {
            if (ModelState.IsValid)
            {
                ShipperEntity shipper = new ShipperEntity()
                    {
                        ID = model.ID,
                        Code = model.Code,
                        Name = model.Name,
                        EnglishName = model.EnglishName,
                        IsDangerous = model.IsDangerous,
                        IsCustoms = model.IsCustoms,
                        Email = model.Email,
                        LawPerson = model.LawPerson,
                        IsSupplier = model.IsSupplier,
                        IsBalance = model.IsBalance,
                        PostCode = model.PostCode,
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        Bank = model.Bank,
                        Account = model.Account,
                        TaxID = model.TaxID,
                        InvoiceTitle = model.InvoiceTitle,
                        Contactor1 = model.Contactor1,
                        Title1 = model.Title1,
                        Phone1 = model.Phone1,
                        Fax1 = model.Fax1,
                        Contactor2 = model.Contactor2,
                        Title2 = model.Title2,
                        Phone2 = model.Phone2,
                        Fax2 = model.Fax2,
                        WebSite = model.WebSite,
                        RegistAdd = model.RegistAdd,
                        Comment = model.Comment,
                        Creater = base.UserInfo.Name,
                        CreateTime = DateTime.Now,
                        Updater = string.Empty,
                        UpdateTime = null,
                        InsuranceCompany = model.InsuranceCompany,
                        InsuranceType = model.InsuranceType,
                        InsuranceOrderNo = model.InsuranceOrderNo,
                        InsuranceCost = model.InsuranceCost,
                        InsuranceStartTime = model.InsuranceStartTime,
                        InsuranceEndTime = model.InsuranceEndTime,
                        Remark = model.Remark,
                        State = true,
                        SegmentID = 0,
                        SegmentName = string.Empty,
                        Str1 = model.PostedShipperIdentifyIDs != null && model.PostedShipperIdentifyIDs.Any() ? string.Join(",", model.PostedShipperIdentifyIDs) : string.Empty,
                        Str2 = model.Str2,
                        Str3 = model.Str3
                    };

               
                Response<long> response = new ShipperService().AddOrUpdateShipper(new AddOrUpdateShipperRequest() { Shipper = shipper, ProjectId = base.UserInfo.ProjectID });
                if (response.IsSuccess)
                {
                    ApplicationConfigHelper.RefreshApplicationShippers();
                    ApplicationConfigHelper.RefreshProjectUserCustomers();
                    ApplicationConfigHelper.RefreshCustomers();
                    ViewBag.Message = response.SuccessMessage;
                }

                model.AvailableShipperIdentifies = ApplicationConfigHelper.GetApplicationConfigs(Constants.SHIPPERIDENTIFY);
                model.SelectedShipperIdentifies = model.AvailableShipperIdentifies.Where(t => model.PostedShipperIdentifyIDs.Contains(t.ID.ToString()));
                model.IsEdit = false;
            }
            return View("Create", model);
        }


        /// <summary>
        /// 验证承运商名称唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsEdit">是否是编辑</param>
        /// <returns></returns>
        public string CheckName(string Name, int? Id, bool IsEdit)
        {
            ShipperService customer = new ShipperService();
            return customer.CheckNameIsExist(Name.Trim(), Id, base.UserInfo.ProjectID.ToString(), IsEdit);
        }

        public ActionResult Edit(long id)
        {
            var response = new ShipperService().GetShipperByID(new ShipperByIDRequest() { ID = id });
            if (response.IsSuccess)
            {
                ShipperVM model = new ShipperVM(response.Result);
                model.AvailableShipperIdentifies = ApplicationConfigHelper.GetApplicationConfigs(Constants.SHIPPERIDENTIFY);
                model.SelectedShipperIdentifies = model.AvailableShipperIdentifies.Where(t => response.Result.Str1.Split(',').Contains(t.ID.ToString()));
                model.IsEdit = true;
                return View("Create", model);
            }
            else
            {
                ShipperVM model = new ShipperVM();
                model.AvailableShipperIdentifies = ApplicationConfigHelper.GetApplicationConfigs(Constants.SHIPPERIDENTIFY);
                model.SelectedShipperIdentifies = Enumerable.Empty<Config>();
                model.IsEdit = false;
                ViewBag.Message = "获取数据失败!";
                return View("Create", model);
            }
        }

        public ActionResult VehicleManagement(long id)
        {
            var response = new ShipperService().GetShipperVehicle(new ShipperByIDRequest() { ID = id });
            VehicleManagementViewModel vm = new VehicleManagementViewModel();
            vm.ShipperID = id;
            vm.IsEdit = false;
            vm.Vehicles = response.Result;
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteVehicle(long id)
        {
            var response = new ShipperService().DeleteVehicle(new DeleteVehicleRequest() { ID = id });
            if (response.IsSuccess)
            {
                return Json(new { IsSuccess = true });
            }

            throw new Exception("删除车辆失败");
        }
        [HttpPost]
        public ActionResult AddOrUpdateVehicle(bool isEdit, long id, long shipperID, string plateNumber, string pilot, string jobNumber, string contract, string str1, string str2, string str3, string str4, string str7)
        {
            Vehicle vehicle = new Vehicle()
            {
                ID = id,
                ShipperID = shipperID,
                PlateNumber = plateNumber,
                Pilot = pilot,
                JobNumber = jobNumber,
                Contract = contract,
                Str1 = str1,
                Str2 = str2,
                Str3 = str3,
                Str4 = str4,
                Str5 = string.Empty,
                Str6 = string.Empty,
                Str7 = str7,
                Str8 = string.Empty,
                DateTime1 = null,
                DateTime2 = null,
                Decimal1 = null,
                Creator = base.UserInfo.Name,
                CreateTime = DateTime.Now,
                State = true
            };

            var response = new ShipperService().AddOrUpdateVehicle(new AddOrUpdateVehicleRequest() { Vehicle = vehicle });
            if (response.IsSuccess)
            {
                return Json(new { IsSuccess = true, IsEdit = isEdit, Vehicle = response.Result });
            }

            throw new Exception("更新或者新增车辆失败");
        }

        [HttpPost]
        public ActionResult Edit(ShipperVM model)
        {
            if (ModelState.IsValid)
            {
                ShipperEntity shipper = new ShipperEntity()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    EnglishName = model.EnglishName,
                    IsDangerous = model.IsDangerous,
                    IsCustoms = model.IsCustoms,
                    Email = model.Email,
                    LawPerson = model.LawPerson,
                    IsSupplier = model.IsSupplier,
                    IsBalance = model.IsBalance,
                    PostCode = model.PostCode,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Bank = model.Bank,
                    Account = model.Account,
                    TaxID = model.TaxID,
                    InvoiceTitle = model.InvoiceTitle,
                    Contactor1 = model.Contactor1,
                    Title1 = model.Title1,
                    Phone1 = model.Phone1,
                    Fax1 = model.Fax1,
                    Contactor2 = model.Contactor2,
                    Title2 = model.Title2,
                    Phone2 = model.Phone2,
                    Fax2 = model.Fax2,
                    WebSite = model.WebSite,
                    RegistAdd = model.RegistAdd,
                    Comment = model.Comment,
                    Creater = model.Creater,
                    CreateTime = model.CreateTime,
                    InsuranceCompany = model.InsuranceCompany,
                    InsuranceType = model.InsuranceType,
                    InsuranceOrderNo = model.InsuranceOrderNo,
                    InsuranceCost = model.InsuranceCost,
                    InsuranceStartTime = model.InsuranceStartTime,
                    InsuranceEndTime = model.InsuranceEndTime,
                    Remark = model.Remark,
                    State = model.State,
                    Updater = base.UserInfo.Name,
                    UpdateTime = DateTime.Now,
                    Str1 = model.PostedShipperIdentifyIDs != null && model.PostedShipperIdentifyIDs.Any() ? string.Join(",", model.PostedShipperIdentifyIDs) : string.Empty,
                    Str2 = model.Str2,
                    Str3 = model.Str3
                };

                Response<long> response = new ShipperService().AddOrUpdateShipper(new AddOrUpdateShipperRequest() { Shipper = shipper });
                if (response.IsSuccess)
                {
                    ApplicationConfigHelper.RefreshApplicationShippers();
                }
                ViewBag.Message = response.SuccessMessage;
            }

            model.AvailableShipperIdentifies = ApplicationConfigHelper.GetApplicationConfigs(Constants.SHIPPERIDENTIFY);
            model.SelectedShipperIdentifies = model.AvailableShipperIdentifies.Where(t => model.PostedShipperIdentifyIDs.Contains(t.ID.ToString()));
            model.IsEdit = true;
            
            return View("Create",model);
        }

        private IEnumerable<SelectListItem> GetSegments()
        {
            var response = new SegmentService().GetSegmentsByCondition(new GetSegmentsByConditionRequest() { Name = "", State = true });
            if (response.IsSuccess)
            {
                return response.Result.Select(r => new SelectListItem() { Value = r.ID.ToString(), Text = r.Name });
            }

            return Enumerable.Empty<SelectListItem>();
        }

        public ActionResult ShipperRelatedInfo()
        {
            ShipperRelatedInfoViewModel vm = new ShipperRelatedInfoViewModel();
            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                       .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            if (customerIDs != null && customerIDs.Count() == 1)
            {
                vm.RelatedCustomerID = customerIDs.First();
            }

            vm.ProjectID = base.UserInfo.ProjectID;

            vm.ShipperRelatedInfo = new ShipperRelatedInfo();
            return View(vm);
        }

        [HttpPost]
        public ActionResult ShipperRelatedInfo(ShipperRelatedInfoViewModel vm)
        {
            var response = new ShipperService().GetShipperAllInfo(new GetShipperAllInfoRequest() { ShipperID = vm.ShipperID, ProjectID = vm.ProjectID, RelatedCustomerID = vm.RelatedCustomerID });
            if (response.IsSuccess)
            {
                vm.ShipperRelatedInfo = response.Result.ShipperRelatedInfo;
                vm.ShipperRegionCoveredCollection = response.Result.ShipperRegionCoveredCollection;
            }

            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                       .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            ModelState.Clear();
            return View(vm);
        }

        [HttpPost]
        public ActionResult ManageShipperEmailInfo(int Type, long ProjectID, long RelatedCustomerID, long ShipperID, string ShipperName, string EmailAddress, string EmailContent)
        {
            var response = new ShipperService().ManageShipperEmailInfo(
                new ManageShipperEmailInfoRequest()
                {
                    ProjectID = ProjectID,
                    RelatedCustomerID = RelatedCustomerID,
                    ShipperID = ShipperID,
                    ShipperName = ShipperName,
                    EmailAddress = EmailAddress,
                    EmailContent = EmailContent,
                    Type = Type
                });
            if (response.IsSuccess)
            {
                return Json(new { IsSuccess = true });
            }

            return Json(new { IsSuccess = false });
        }

        [HttpPost]
        public ActionResult ManageShipperRegionCovered(long ProjectID, long RelatedCustomerID, long ShipperID, string ShipperName, long StartCityID, string StartCityName, long EndCityID, string EndCityName)
        {
            var response = new ShipperService().ManageShipperRegionCovered(
               new ManageShipperRegionCoveredRequest()
               {
                   ShipperRegionCovered = new ShipperRegionCovered()
                   {
                       ProjectID = ProjectID,
                       RelatedCustomerID = RelatedCustomerID,
                       ShipperID = ShipperID,
                       ShipperName = ShipperName,
                       StartCityID = StartCityID,
                       StartCityName = StartCityName,
                       EndCityID = EndCityID,
                       EndCityName = EndCityName
                   }
               });

            if (response.IsSuccess)
            {
                return Json(new
                {
                    IsSuccess = true,
                    ProjectID = ProjectID,
                    RelatedCustomerID = RelatedCustomerID,
                    ShipperID = ShipperID,
                    ShipperName = ShipperName,
                    StartCityID = StartCityID,
                    StartCityName = StartCityName,
                    EndCityID = EndCityID,
                    EndCityName = EndCityName
                });
            }

            return Json(new { IsSuccess = false });
        }

        [HttpPost]
        public ActionResult DeleteShipperRegionCovered(long ProjectID, long RelatedCustomerID, long ShipperID, long StartCityID, long EndCityID)
        {
            var response = new ShipperService().DeleteShipperRegionCovered(
                new DeleteShipperRegionCoveredRequest()
                {
                    ProjectID = ProjectID,
                    RelatedCustomerID = RelatedCustomerID,
                    ShipperID = ShipperID,
                    StartCityID = StartCityID,
                    EndCityID = EndCityID
                });
            
            if (response.IsSuccess)
            {
                return Json(new { IsSuccess = true });
            }

            return Json(new { IsSuccess = false });
        }
    }
}