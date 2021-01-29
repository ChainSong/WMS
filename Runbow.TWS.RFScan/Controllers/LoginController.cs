using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.RFScan.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.RFScan.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReturnIndex()
        {
            return View();
        }

        public ActionResult Main(long? CustomerID, string WareHouseName, long? WareHouseID)
        {

            if (CustomerID == null || WareHouseName == null || WareHouseID == null || Session["ProjectID"] == null || Session["Name"] == null)
            {
                return RedirectToAction("", "login");
            }
            IEnumerable<Menu> menus = new MenuService().GetMenuInfo2(new GetMenuByProjectRoleIDRequest() { ProjectRoleID = Convert.ToInt64(Session["ProjectRoleID"].ToString()) }).Result.Where(m => m.Scenarios == 3 && m.Link != null && m.Link != "").Select(m => (Menu)m);
            //ViewBag.MenuList = menus;
            //long WarehouseID=15;
            //ApplicationConfigHelper.GetWarehouseLocationList(WarehouseID);
            ViewBag.CustomerID = CustomerID.ToString();
            ViewBag.WareHouseName = WareHouseName;
            ViewBag.WareHouseID = WareHouseID;
            return View(menus);
        }
        public ActionResult SelectProject()
        {
            if (Session["ProjectID"] == null || Session["Name"] == null)
            {
                return RedirectToAction("", "login");
            }
            IEnumerable<SelectListItem> CustomerList = null;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(Convert.ToInt64(Session["ProjectID"].ToString()),Convert.ToInt64( Session["ID"].ToString()));
            CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.ProjectID == Convert.ToInt64(Session["ProjectID"].ToString()) && c.UserID == Convert.ToInt64(Session["ID"].ToString()))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WarehouseList;
            return View();
        }
        [HttpPost]
        public string CheckUser(string UserName, string Password,string ProjectID)
        {
            UserService us = new UserService();
            //Runbow.TWS.Common.AES.Encrypt(model.Password)
            //resp.Result.Select(x => new { ProjectID = x.ID, ProjectName = x.Name }).ToList()[0].ProjectID
            User user = us.CheckLogin(new GetUserByCheckLoginRequest() { Name = UserName, Password = Password, ProjectID = Convert.ToInt64( ProjectID) }).Result;
            if (user != null)
            {
                Session[Constants.USER_INFO_KEY] = user;
                Session["Name"] = user.Name;
                Session["ID"] = user.ID;
                Session["DisplayName"] = user.DisplayName;
                Session["ProjectID"] = user.ProjectID;
                Session["ProjectName"] = user.ProjectName;
                Session["ProjectRoleID"] = user.ProjectRoleID;
                Session["UserType"] = user.UserType;
                Session["CustomerOrShipperID"] = user.CustomerOrShipperID;
                Session["RuleArea"] = user.RuleArea;
                //if (string.IsNullOrEmpty(returnUrl))
                //{
                //    return RedirectToAction("", "../");
                //}
                //else
                //{
                //    return Redirect(returnUrl);
                //}
                return "1";
            }
            else
            {
                return "登录信息有误，请重试.";
            }
            #region
            //var resp = new UserService().GetUserProjects(new GetUserProjectsRequest() { UserName = UserName });
            //if (resp.IsSuccess)
            //{
            //    if (resp.Result.Count() == 0)
            //    {
            //        return "该用户没有分配到项目中";
            //    }

            //    UserService us = new UserService();
            //    //Runbow.TWS.Common.AES.Encrypt(model.Password)
            //    //resp.Result.Select(x => new { ProjectID = x.ID, ProjectName = x.Name }).ToList()[0].ProjectID
            //    User user = us.CheckLogin(new GetUserByCheckLoginRequest() { Name = UserName, Password =Password, ProjectID = resp.Result.Select(x => new { ProjectID = x.ID, ProjectName = x.Name }).ToList()[0].ProjectID}).Result;
            //    if (user != null)
            //    {
            //        Session[Constants.USER_INFO_KEY] = user;
            //        Session["Name"] = user.Name;
            //        Session["ID"] = user.ID;
            //        Session["DisplayName"] = user.DisplayName;
            //        Session["ProjectID"] = user.ProjectID;
            //        Session["ProjectName"] = user.ProjectName;
            //        Session["ProjectRoleID"] = user.ProjectRoleID;
            //        Session["UserType"] = user.UserType;
            //        Session["CustomerOrShipperID"] = user.CustomerOrShipperID;
            //        Session["RuleArea"] = user.RuleArea;
            //        //if (string.IsNullOrEmpty(returnUrl))
            //        //{
            //        //    return RedirectToAction("", "../");
            //        //}
            //        //else
            //        //{
            //        //    return Redirect(returnUrl);
            //        //}
            //        return "1";
            //    }
            //    else
            //    {
            //        return "登录信息有误，请重试.";
            //    }
            //}
            //return "用户不存在";
            #endregion
        }

        [HttpGet]
        public ActionResult GetUserProjects(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return Json("用户名称不能为空.", JsonRequestBehavior.AllowGet);
            }

            var resp = new UserService().GetUserProjects(new GetUserProjectsRequest() { UserName = userName });
            if (resp.IsSuccess)
            {
                if (resp.Result.Count() == 0)
                {
                    return Json("该用户没有分配到项目中", JsonRequestBehavior.AllowGet);
                }

                return Json(resp.Result.Select(x => new { ProjectID = x.ID, ProjectName = x.Name }), JsonRequestBehavior.AllowGet);
            }

            return Json("服务器内部错误，请稍后尝试.", JsonRequestBehavior.AllowGet);
        }
    }
}
