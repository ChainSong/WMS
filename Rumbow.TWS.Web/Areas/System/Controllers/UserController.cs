using System.Linq;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;

using Runbow.TWS.Web.Areas.System.Models;
using Runbow.TWS.Web.Common;
using UserVM = Runbow.TWS.Web.Areas.System.Models.User;
using UtilConstants = Runbow.TWS.Common.Constants;

namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Create()
        {
            UserVM vm = new UserVM() { UserType = 2 };
           //ViewBag.Roles = ApplicationConfigHelper.GetRoles().Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Name });
            ViewBag.Roles = new RoleService().GetRoleInfo(new GetRoleByProjectIDRequest() { ProjectID = base.UserInfo.ProjectID }).Result
            .Select(pr => new SelectListItem() { Text = pr.Name, Value = pr.ProjectRoleID.ToString()});

            ViewBag.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.ShipperID.ToString(), Text = s.Name });
            ViewBag.Customers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.CustomerID.ToString(), Text = s.Name });
            ViewBag.WareHouse = ApplicationConfigHelper.GetWarehouseList().Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.WarehouseName });

            return View(vm);
        }

        /// <summary>
        /// 验证承运商名称唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsEdit">是否是编辑</param>
        /// <returns></returns>
        public string CheckName(string Name, int? Id, bool IsEdit)
        {
            UserService customer = new UserService();
            return customer.CheckNameIsExist(Name.Trim(), Id,IsEdit);
        }

        [HttpPost]
        public ActionResult Create(UserVM user)
        {
            string userName = Session["Name"].ToString();
           // string wareId = Request.Form["cWarehouse"].ToString();
            string RoleId = Request.Form["cRole"].ToString();
            if (ModelState.IsValid)
            {
                UserRequest request = new UserRequest()
                {
                    Name = user.Name,
                    DisplayName = user.DisplayName,
                    //Password = Runbow.TWS.Common.AES.Encrypt(Constants.PASSWORD),
                    Password = Constants.PASSWORD,
                    Sex = user.Sex,
                    State = true,
                    Tel = user.Tel,
                    Mobile = user.Mobile,
                    Email = user.Email,
                    UserType = user.UserType,
                    RuleArea = user.RuleArea,
                    CustomerOrShipperID = user.UserType == 0 ? user.CustomerID ?? 0 : user.UserType == 1 ? user.ShipperID ?? 0 : 0
                };

                Response<long> response = new UserService().AddUser2(request, userName, RoleId, base.UserInfo.ProjectID.ToString());

                if (response.IsSuccess)
                {
                    ApplicationConfigHelper.RefreshApplicationUsers();
                    ApplicationConfigHelper.RefreshProjectUserRole();
                    ApplicationConfigHelper.RefreshCustomers();
                    return RedirectToAction("Edit", new { ID = response.Result, message = "操作成功!" });
                }
                else
                {
                    //ViewBag.Roles = ApplicationConfigHelper.GetApplicationRoles().Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Name });
                    ViewBag.Roles = new RoleService().GetRoleInfo(new GetRoleByProjectIDRequest() { ProjectID = base.UserInfo.ProjectID }).Result
           .Select(pr => new SelectListItem() { Text = pr.Name, Value = pr.ProjectRoleID.ToString() });
                    ViewBag.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.ShipperID.ToString(), Text = s.Name });
                    ViewBag.Customers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.CustomerID.ToString(), Text = s.Name });
                    ViewBag.WareHouse = ApplicationConfigHelper.GetWarehouseList().Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.WarehouseName });

                    if (response.Result == -1)
                    {
                        ViewBag.Message = "已存在此用户，请更换用户名";
                    }
                    else
                    {
                        ViewBag.Message = "系统出错！";
                    }
                }
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult Edit(long ID, string message)
        {
            var response = new UserService().GetUserInfo(new GetUserByUserIdRequest() { UserID = ID });
            ViewBag.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.ShipperID.ToString(), Text = s.Name });
            ViewBag.Customers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.CustomerID.ToString(), Text = s.Name });
            ViewBag.Roles = new RoleService().GetRoleInfo(new GetRoleByProjectIDRequest() { ProjectID = base.UserInfo.ProjectID }).Result
          .Select(pr => new SelectListItem() { Text = pr.Name, Value = pr.ProjectRoleID.ToString() });
            ViewBag.Message = message;

            if (response.IsSuccess)
            {
                response.Result.RuleAreaName = ApplicationConfigHelper.GetRegions().Where(a => a.ID.ToString() == response.Result.RuleArea).Select(a => a.Name).FirstOrDefault();
                return View(new UserVM(response.Result));
            }
            else
            {
                ViewBag.Message = "获取数据失败!";
                return View(new UserVM());
            }
        }

        [HttpPost]
        public ActionResult Edit(UserVM user)
        {
         
            if (ModelState.IsValid)
            {
                var response = new UserService().EditUser(new UserRequest() { 
                    Name = user.Name, 
                    UserName = base.UserInfo.Name, 
                    ProjectId = base.UserInfo.ProjectID, 
                    ProjectRoleId = user.ProjectRoleID, 
                    DisplayName = user.DisplayName, 
                    State = user.State, 
                    Sex = user.Sex, 
                    Email = user.Email, 
                    UserType = user.UserType, 
                    CustomerOrShipperID = user.UserType == 0 ? user.CustomerID ?? 0 : user.UserType == 1 ? user.ShipperID ?? 0 : 0, 
                    Mobile = user.Mobile, 
                    Tel = user.Tel, 
                    ID = user.ID, 
                    RuleArea = ApplicationConfigHelper.GetRegions().Where(a => a.Name == user.RuleAreaName).Select(a => a.ID).FirstOrDefault().ToString()
                });
                ViewBag.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.ShipperID.ToString(), Text = s.Name });
                ViewBag.Customers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).Select(s => new SelectListItem() { Value = s.CustomerID.ToString(), Text = s.Name });
                ViewBag.Roles = new RoleService().GetRoleInfo(new GetRoleByProjectIDRequest() { ProjectID = base.UserInfo.ProjectID }).Result.Select(pr => new SelectListItem() { Text = pr.Name, Value = pr.ProjectRoleID.ToString() });

                if (response.IsSuccess)
                {
                    user.RuleAreaName = ApplicationConfigHelper.GetRegions().Where(a => a.ID.ToString() == user.RuleArea).Select(a => a.Name).FirstOrDefault();
                    ApplicationConfigHelper.RefreshApplicationUsers();
                    ApplicationConfigHelper.RefreshProjectUserRole();
                    ViewBag.Message = "更新成功";
                }
                else
                {
                    ViewBag.Message = "更新失败";
                }
            }
            return View(user);
        }


        [HttpGet]
        public ActionResult List()
        {
            UserListViewModel vm = new UserListViewModel();
            var result = new UserService().GetUsersByConditon(new GetUsersByConditionRequest() { ProjectID = base.UserInfo.ProjectID, Name = "", DisplyName = "", State = true, UserType = base.UserInfo.UserType, PageSize = UtilConstants.PAGESIZE, PageIndex = 0 }).Result;
            vm.Users = result.Users;
            vm.State = true;
            vm.UserType = 2;
            vm.PageIndex = result.PageIndex;
            vm.PageCount = result.PageCount;
            return View(vm);
        }

        [HttpPost]
        public ActionResult List(UserListViewModel vm, int? PageIndex)
        {
            var result = new UserService().GetUsersByConditon(new GetUsersByConditionRequest() { ProjectID = base.UserInfo.ProjectID, Name = string.IsNullOrEmpty(vm.Name) ? "" : vm.Name, DisplyName = string.IsNullOrEmpty(vm.DisplyName) ? "" : vm.DisplyName, State = vm.State, UserType = vm.UserType, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 }).Result;
            vm.Users = result.Users;
            vm.PageIndex = result.PageIndex;
            vm.PageCount = result.PageCount;
            return View(vm);
        }
    }
}