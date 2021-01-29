using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.System.Models;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class ProjectRoleMenuController : BaseController
    {
        [HttpGet]
        public ActionResult Index(long? id)
        {
            long projectRoleID = id.GetValueOrDefault(-1);

            ViewBag.Roles = new RoleService().GetRoleInfo(new GetRoleByProjectIDRequest() { ProjectID = base.UserInfo.ProjectID }).Result
                .Select(pr => new SelectListItem() { Text = pr.Name, Value = pr.ProjectRoleID.ToString(), Selected = pr.ProjectRoleID == projectRoleID });

            ViewBag.Menus = "null";
            ViewBag.Url = "/System/ProjectRoleMenu/Index/";
            ViewBag.ProjectRoleID = projectRoleID;

            if (projectRoleID != -1)
            {
                //if (UserInfo.ID != 59)
                //    ViewBag.Menus = new MenuService().GetAllCheckedMenuInfo(new GetMenuByProjectRoleIDRequest() { ProjectRoleID = projectRoleID, GetAll = true }).Result.Select(cm => { var treeItem = new TreeItem(cm); treeItem.isChecked = true; treeItem.open = true; return treeItem; }).ToJsonString();
                //else
                    ViewBag.Menus = new MenuService().GetAllCheckedMenuInfo(new GetMenuByProjectRoleIDRequest() { ProjectRoleID = projectRoleID, GetAll = true }).Result.Select(cm => { var treeItem = new TreeItem(cm); treeItem.open = true; return treeItem; }).ToJsonString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(long ProjectRoleID, IEnumerable<int> MenuItems)
        {
            MenuService service = new MenuService();
            var response = service.SetProjectRoleMenu(new SetProjectRoleMenuRequest() { ProjectRoleID = ProjectRoleID, MenuIDs = MenuItems });

            if (response.Result)
            {
                var refreshResponse = service.RefreshProjectRoleMenus(new GetMenuByProjectRoleIDRequest() { ProjectRoleID = ProjectRoleID });
                if (refreshResponse.IsSuccess)
                {
                    return Json("角色菜单设置成功");
                }
                else
                {
                    return Json("角色菜单设置成功，但缓存更新失败，此角色暂时无法使用新更换菜单");
                }
            }

            throw response.Exception;
        }
    }
}