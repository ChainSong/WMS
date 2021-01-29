using System;
using System.Web.Mvc;

using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Areas.System.Models;
using UtilConstants = Runbow.TWS.Common.Constants;
using RoleVM = Runbow.TWS.Web.Areas.System.Models.RoleModel;
using Runbow.TWS.Entity;
using System.Web.UI.HtmlControls;
using System.Web.UI;
namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class RoleController : BaseController
    {
        //
        // GET: /System/Role/

        [HttpPost]
        public ActionResult RoleManage(RoleManageListViewModel vm, int? PageIndex, int? id)
        {
            //此查询方法 查的是该项目下的所有角色
            var Result = new Runbow.TWS.Biz.RoleService().GetRoleInfo(new RoleRequest() { Role = vm.Role, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0, Satate = vm.Satate ? 1 : 0, ProjectId = base.UserInfo.ProjectID }).Result;
            vm.Satate = Result.Satate == 1 ? true : false;
            vm.IEnumerableRole = Result.IEnumerableRole;
            vm.PageIndex = Result.PageIndex;
            vm.PageSize = Result.PageSize;
            vm.PageCount = Result.PageCount;
            return View(vm);

        }

        [HttpGet]
        public ActionResult RoleManage()
        {
            RoleManageListViewModel vm = new RoleManageListViewModel();
            //此查询方法 查的是该项目下的所有角色
            var Result = new Runbow.TWS.Biz.RoleService().GetRoleInfo(new RoleRequest() { Role = vm.Role, PageSize = UtilConstants.PAGESIZE, PageIndex = 0, Satate = 1 , ProjectId = base.UserInfo.ProjectID }).Result;
            vm.IEnumerableRole = Result.IEnumerableRole;
            vm.Satate = true;
            vm.PageIndex = Result.PageIndex;
            vm.PageSize = Result.PageSize;
            vm.PageCount = Result.PageCount;
            return View(vm);
        }

        [HttpGet]
        public ActionResult RoleAdd()
        {
            ViewBag.Message = "新增角色";
            return View(); //新增成功跳转到角色详细
        }

        /// <summary>
        /// 验证角色名称唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsEdit">是否是编辑</param>
        /// <returns></returns>
        public string CheckName(string Name, int? Id, bool IsEdit)
        {
            RoleService customer = new RoleService();
            return customer.CheckNameIsExist(Name.Trim(), Id, base.UserInfo.ProjectID.ToString(), IsEdit);
        }

        [HttpGet]
        public ActionResult RoleDetail()
        {
            RoleVM role = new RoleVM();
            role.Name = Request["Name"].ToString();
            role.Description = Request["Description"].ToString();
            role.Satate = Request["State"] == "True" ? true : false;
            return View(role); 
        }

        [HttpPost]
        public JsonResult GetUserProjectRole(long userID)  
        {
            var response = new RoleService().GetUserProjectRole(new GetUserProjectRoleRequest() { UserID = userID, ProjectID = base.UserInfo.ProjectID });
            if (response.IsSuccess)
            {
                if (response.Result != null)
                {
                    return Json(response.Result.ProjectRoleID.ToString());
                }
                else
                {
                    return Json("");
                }
            }

            throw new Exception("获取用户项目角色失败！");
        }

        //

        // GET: /HelloWorld/Welcome/

        [HttpGet]
        public ActionResult EditRole(int id)
        {

            ViewBag.Message = "修改角色";
            RoleVM VM = new RoleVM();
            try
            {
                int i = id;
                Role rvlm = Runbow.TWS.Biz.RoleService.UpdateSelct(id);
                VM.Name = rvlm.Name;
                VM.Description = rvlm.Description;
                VM.ID = rvlm.ID;
                VM.Satate = rvlm.State;
            }
            catch 
            {

            }
           
            return View("RoleAdd", VM);
        }

        [HttpPost]
        public ActionResult RoleAdd(RoleVM role)
        {
            RoleRequest r = new RoleRequest();
            ViewBag.IsSuccess = "0";
            ViewBag.Message = "新增角色";

            r.Name = role.Name;
            r.Satate = role.Satate ? 1 : 0;
            r.Description = role.Description;
            r.ProjectId = base.UserInfo.ProjectID;
            r.ID = role.ID;
            int i = 0;

            if (r.ID == 0)
            {
                if (RoleService.CheckInput(r) == "-1") //该角色已存在
                {
                    ViewBag.Message = "该角色已存在！";
                    return View(role);
                }
                i = Runbow.TWS.Biz.RoleService.roleinsert(r);
                ViewBag.rId = i.ToString();  
            }
            else
            {

                i = Runbow.TWS.Biz.RoleService.RoleUpdate(r);
            }

            if (i > 0)
            {
                ViewBag.Message = " 操作成功";
                return View("RoleDetail", role); //新增成功跳转到角色详细
            }
            return View(role);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="paremIds">角色Id：传入角色id 可批量删除 id以“,”隔开 </param>
        /// <returns></returns>
        [HttpPost]
        public string DeleteRole(string paremIds)
        {
            RoleService  rs= new RoleService();
            Response<bool> response = new Response<bool>();
            try
            {
                response = rs.DeleteRole(new RoleRequest { Name = paremIds, ProjectId = base.UserInfo.ProjectID });
                if (response.IsSuccess)
                {
                    ApplicationConfigHelper.RefreshRole((int)base.UserInfo.ProjectID);
                    ApplicationConfigHelper.RefreshProjectRoles();
                }
            }
            catch 
            {

            }
            return response.SuccessMessage;
        }
    }
}