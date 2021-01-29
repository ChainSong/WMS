using System.Linq;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.Login.Models;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.Login.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login(long? ProjectID)
        {
            ViewBag.ProjectName = "";
            if (ProjectID != null)
            {
                if (ProjectID == 0)
                {
                    ViewBag.ProjectName = "Aden";
                }
                else
                {
                    var procus = ApplicationConfigHelper.GetAllProjects().Where(m => m.ID == ProjectID.Value);


                    if (procus.Count() > 0)
                    {
                        ViewBag.ProjectName = procus.ToArray()[0].Name;
                    }
                }
            }
            return View();
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

        public string IsLogin(string UserName, string Password, long ProjectID)
        { 
            UserService us = new UserService();
             User user = us.CheckLogin(new GetUserByCheckLoginRequest() { Name = UserName, Password = Password, ProjectID = ProjectID }).Result;
             if (user != null) {
                 return "true";
             }
             return "false";
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserService us = new UserService();
                //Runbow.TWS.Common.AES.Encrypt(model.Password)
                User user = us.CheckLogin(new GetUserByCheckLoginRequest() { Name = model.UserName, Password = model.Password, ProjectID = model.ProjectID }).Result;
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
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("", "../");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    //ViewBag.IsValidateUserNameOrPassword = "false";
                    ModelState.AddModelError("", "登录信息有误，请重试.");
                }
            }

            return View(model);
        }
    }
}