using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.System.Models;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class ProjectController : BaseController
    {
        //
        // GET: /System/Project/
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            ProjectModel vm=new ProjectModel();
           

            if (ModelState.IsValid)
            {
                ProjectService PS = new ProjectService();

                try
                {
                    GetProjectByProjectIdRequest myRequest = new GetProjectByProjectIdRequest()
                    {
                        Name = Request.Form["Name"].ToString(),
                        Description = Request.Form["Description"].ToString(),
                        Code = Request.Form["Code"].ToString()
                    };
                    Response<GetProjectByProjectIdRequest> respone = PS.SaveProject(myRequest);

                    if (respone.IsSuccess)
                    {
                        vm.Name = myRequest.Name;
                        vm.Description = myRequest.Description;
                        vm.Code = myRequest.Code;

                        //刷新项目缓存
                        ApplicationConfigHelper.RefreshProject(); 
                        //刷新项目角色缓存 
                        ApplicationConfigHelper.RefreshProjectRole(new GetRoleByProjectIDRequest { ProjectID = base.UserInfo.ProjectID, GetAll = false });
                        //刷新新增项目下的所有用户 如果要获取当前项目想的所有用户 取 ProjectId=base.UserInfo.ProjectID 即可
                        ApplicationConfigHelper.RefreshUserByProjectId(new UserRequest { ProjectId = base.UserInfo.ProjectID });
                        //刷新项目下的用户角色
                        ApplicationConfigHelper.GetProjectUserRoles(base.UserInfo.ProjectID);
                        if (base.UserInfo.ProjectRoleID != 1) //排除超级管理员角色 因为其本身是最高级
                            //刷新某个角色下的菜单
                            ApplicationConfigHelper.RefreshProjectRoleMenu(base.UserInfo.ProjectRoleID);

                    }
                    ViewBag.Message = respone.SuccessMessage;
                }
                catch(Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
               
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult List(int? id)
        {
            ProjectService service = new ProjectService();
 
            try
            {
                ViewData["ddl"] = new SelectListItem[] {
                    new SelectListItem(){ Value = "1", Text="可用",Selected=true},
                    new SelectListItem(){Value = "0", Text="禁用"}
                };
                ViewData["Projects"] = service.GetAllProjects().Result;
            }
            catch
            {
                ViewBag.Message = "获得数据失败!";
            }
            return View();
        }

        /// <summary>
        /// （模糊）查询项目信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult List()
        {
            ProjectService service = new ProjectService();
            try
            {
                string name = Request.Form["txtName"] != null ? Request.Form["txtName"].ToString() : "";
                string des = Request.Form["txtDescription"] != null ? Request.Form["txtDescription"].ToString() : "";
                string state = Request.Form["ddL"].ToString();

                ViewData["ddl"] = new SelectListItem[] {
                    new SelectListItem(){ Value = "1", Text="可用"},
                    new SelectListItem(){Value = "0", Text="禁用",Selected=true}};
             

                ViewData["Projects"] = service.GetAllProjects(name, des,state).Result;
            }
            catch
            {
                ViewBag.Message = "获得数据失败!";
            }
            return View();
        }

        /// <summary>
        /// 验证公司编号唯一性
        /// </summary>
        /// <param name="code"></param>
        /// <param name="IsEdit">是否是编辑</param>
        /// <returns></returns>
        public string CheckCode(string code,int? Id ,bool IsEdit)
        {
            ProjectService service = new ProjectService();
            return service.CheckCodeIsExist(code.Trim(),Id, IsEdit);
        }

        public ActionResult Edit(long ID, string state)
        {
            ProjectModel vm = null;
            try
            {
             
                ProjectService service = new ProjectService();
                GetProjectByProjectIdRequest PID = new GetProjectByProjectIdRequest();
                PID.ProjectID = ID;
                PID.State = state.ToLower().Equals("false") ? "0" : "1";
                vm = new ProjectModel(service.GetProjectInfo(PID).Result);
            }catch(Exception err)
            {
                
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(ProjectModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ProjectService PS = new ProjectService();

                    Project project = new Project();
                    project.ID = vm.ID;
                    project.Name = vm.Name;
                    project.Code = vm.Code;
                    project.Description = vm.Description;
                    project.State = vm.State;
                    var response = PS.UpdateProject(project);
                    if (response.IsSuccess)
                    {
                        //刷新项目缓存
                        ApplicationConfigHelper.RefreshProject();
                        //刷新项目角色缓存 
                        ApplicationConfigHelper.RefreshProjectRole(new GetRoleByProjectIDRequest { ProjectID = base.UserInfo.ProjectID, GetAll = false });
                        //刷新新增项目下的所有用户 
                        ApplicationConfigHelper.RefreshUserByProjectId(new UserRequest { ProjectId = base.UserInfo.ProjectID });
                        //刷新项目下的用户角色
                        ApplicationConfigHelper.GetProjectUserRoles(base.UserInfo.ProjectID);
                        if (base.UserInfo.ProjectRoleID != 1)
                            //刷新某个角色下的菜单
                            ApplicationConfigHelper.RefreshProjectRoleMenu(base.UserInfo.ProjectRoleID);
                    }
                    vm.Name = project.Name;
                    vm.Description = project.Description;
                    vm.ID = project.ID;

                    ViewBag.Message = response.SuccessMessage;
                }
            }
            catch (Exception err)
            {
 
            }
            return View(vm);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="ID">项目Id</param>
        /// <returns></returns>
        public string Delete(long ID)
        {
            try
            {
                ProjectService service = new ProjectService();
                if (service.DeleteProject(ID))
                {
                    //刷新项目缓存
                    ApplicationConfigHelper.RefreshProject();
                    //刷新项目角色缓存 
                    ApplicationConfigHelper.RefreshProjectRole(new GetRoleByProjectIDRequest { ProjectID = base.UserInfo.ProjectID, GetAll = false });
                    //刷新新增项目下的所有用户 如果要获取当前项目想的所有用户 取 ProjectId=base.UserInfo.ProjectID 即可
                    ApplicationConfigHelper.RefreshUserByProjectId(new UserRequest { ProjectId = base.UserInfo.ProjectID });
                    //刷新项目下的用户角色
                    ApplicationConfigHelper.GetProjectUserRoles(base.UserInfo.ProjectID);
                    if (base.UserInfo.ProjectRoleID != 1)
                        //刷新某个角色下的菜单
                        ApplicationConfigHelper.RefreshProjectRoleMenu(base.UserInfo.ProjectRoleID);

                    ViewBag.Message = "删除成功！";
                }
            }
            catch (Exception err)
            {
                ViewBag.Message = "删除失败！" + err.Message;
            }
            return ViewBag.Message;
        }

        [HttpGet]
        public ActionResult ProjectUserCustomerAllocate(long ID)
        {
            ProjectUserCustomerAllocateViewModel vm = new ProjectUserCustomerAllocateViewModel()
            {
                UserID=ID,
                ProjectUsers = ApplicationConfigHelper.GetProjectUserRoles(base.UserInfo.ProjectID).Select(pur => new SelectListItem() { Value = pur.UserID.ToString(), Text = pur.UserName }),
                ProjectCustomers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID), //获取项目下的所有用户 
                ProjectCustomer = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, ID), //获取改用户下分配的用户
                ProjectID = base.UserInfo.ProjectID
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult ProjectUserCustomerAllocate(long projectID, long userID, string SelectedCustomers)
        {
            var customerIDs = SelectedCustomers.FromJsonStringTo<IEnumerable<long>>();
            var response = new ProjectService().SetProjectUserCustomers(new SetProjectUserCustomersRequest() { UserID = userID, ProjectID = projectID, CustomerIDs = customerIDs, Creator = base.UserInfo.Name, CreateTime = DateTime.Now });
            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshProjectUserCustomers();
            }

            ProjectUserCustomerAllocateViewModel vm = new ProjectUserCustomerAllocateViewModel()
            {
                UserID = userID,
                ProjectUsers = ApplicationConfigHelper.GetProjectUserRoles(projectID).Select(pur => new SelectListItem() { Value = pur.UserID.ToString(), Text = pur.UserName }),
                ProjectCustomer = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, userID),
                ProjectCustomers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID),
                ProjectID = projectID
            };

            return View(vm); 

           
        }

        [HttpPost]
        public ActionResult GetUserProjectCustomers(long projectID, long userID)
        {
            var result = ApplicationConfigHelper.GetProjectUserCustomers(projectID, userID);
            if (result != null)
            {
                return Json(result.Select(r => r.CustomerID));
            }

            return Json(Enumerable.Empty<long>());
        }

        [HttpGet]
        public ActionResult ProjectUserAllocate()
        {
            var vm = new ProjectUserAllocateViewModel()
            {
                Users = ApplicationConfigHelper.GetApplicationUsers().Select(u => new SelectListItem() { Value = u.ID.ToString(), Text = u.Name }),
                ProjectRoles = ApplicationConfigHelper.GetProjectRoles().Where(pr => pr.ProjectID == base.UserInfo.ProjectID)
            };

            return View(vm);
        }

        [HttpPost]
        public JsonResult ProjectUserAllocate(long userID, long projectRoleID)
        {
            var response = new ProjectService().SetUserProjectRole(new SetUserProjectRoleRequest() { UserID = userID, ProjectRoleID = projectRoleID, ProjectID = base.UserInfo.ProjectID });
            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshProjectUserRole();
                return Json("项目用户设置成功");
            }

            throw new Exception("项目用户设置失败！");
        }

        [HttpGet]
        public ActionResult ProjectRoleAllocate()
        {
            var vm = new ProjectRoleAllocateViewModel()
            {
                ProjectRoles = new RoleService().GetRoleInfo(new GetRoleByProjectIDRequest()
                {
                    ProjectID =base.UserInfo.ProjectID,
                    GetAll = true
                }).Result
            };

            vm.ProjectRoles.Each((i, pr) => pr.Checked = pr.ProjectID != 0);

            vm.SelectedRoleIDs = vm.ProjectRoles
                .Where(pr => pr.ProjectID != 0)
                .Select(pr => new
                {
                    ID = pr.ID
                }).ToJsonString();
            return View(vm);
        }

        [HttpPost]
        public ActionResult ProjectRoleAllocate(string MenuIDs)
        {
            var projectRoles = MenuIDs.FromJsonStringTo<IEnumerable<ProjectRole>>();

            var roleIDs = projectRoles.Select(pr => pr.ID);

            var response = new RoleService().SetProjectRole(new SetProjectRoleRequest() { ProjectID = base.UserInfo.ProjectID, RoleIDs = roleIDs });

            if (response.Result)
            {
                ApplicationConfigHelper.RefreshProjectRoles();
                ApplicationConfigHelper.RefreshProjectUserRole();
                ViewBag.IsSuccess = "1";
                return Json("项目角色设置成功");
            }

            throw response.Exception;
        }

        public ActionResult ProjectCustomerOrShipperAllocate(int id)
        {
            ProjectCustomersOrShippersAllocateViewModel vm = new ProjectCustomersOrShippersAllocateViewModel();
            vm.Target = id;
            vm.ProjectID = base.UserInfo.ProjectID;
            vm.CustomerOrShippersCollection = new ProjectService().GetProjectCustomersOrShippers(new GetProjectCustomersOrShippersRequest() { Target = id, ProjectID = base.UserInfo.ProjectID }).Result;
            vm.SelectedConfig = vm.CustomerOrShippersCollection.Where(s => s.ProjectShipperOrCustomerID > 0 && s.State == true).Select(s => new { ID = s.ProjectShipperOrCustomerID, IsDefault = s.IsDefault }).ToJsonString();
            return View(vm);
        }

        [HttpPost]
        public ActionResult ProjectCustomerOrShipperAllocate(string CustomerOrShipperIDs, int Target)
        {
            var projectCustomerOrShippers = CustomerOrShipperIDs.FromJsonStringTo<IEnumerable<ProjectCustomersOrShippers>>();
            projectCustomerOrShippers.Each((i, p) => { p.ProjectID = base.UserInfo.ProjectID; p.Target = Target; p.State = true; });

            var response = new ProjectService().SetProjectCustomerOrShippers(new SetProjectCustomerOrShippersRequest() { ProjectID = base.UserInfo.ProjectID, Target = Target, ProjectCustomersOrShippers = projectCustomerOrShippers });

            if (response.Result)
            {
                if (Target == 0)
                {
                    ApplicationConfigHelper.RefreshCustomers();
                }
                else
                {
                    ApplicationConfigHelper.RefreshShippers();
                }

                return Target == 0 ? Json("项目客户设置成功") : Json("项目承运商设置成功");
            }

            throw response.Exception;
        }

        [HttpGet]
        public ActionResult CustomerOrShipperSegmentAllocate(int id)
        {
            var customerOrShippers = new ProjectService().GetProjectCustomersOrShippers(new GetProjectCustomersOrShippersRequest() { ProjectID = base.UserInfo.ProjectID, Target = id }).Result.Where(cs => cs.State);
            var customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID);
            var segments = new SegmentService().GetSegmentsByCondition(new GetSegmentsByConditionRequest() { Name = "", State = true }).Result;
            if (customerOrShippers == null || !customerOrShippers.Any())
            {
                return Error("请先设置项目" + (id == 0 ? "客户" : "承运商"));
            }

            if (segments == null || !segments.Any())
            {
                return Error("请先配置段位");
            }

            if (id == 1 && (customers == null || !customers.Any()))
            {
                return Error("请先分配项目客户");
            }

            CustomerOrShipperSegmentViewModel vm = new CustomerOrShipperSegmentViewModel()
            {
                Target = id,
                ProjectID = base.UserInfo.ProjectID,
                CustomerOrShippersCollection = customerOrShippers.Select(s => new SelectListItem() { Value = s.CustomerOrShipperID.ToString(), Text = s.CustomerOrShipperName }),
                Segments = segments.Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Name }),
                Customers = customers.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName })
            };

            return View(vm);
        }

        [HttpPost]
        public JsonResult GetCustomerOrShipperSegment(int target, long customerOrShipperID, long relatedCustomerID, long projectID)
        {
            var projectCustomerOrShipperSegments = ApplicationConfigHelper.GetProjectCustomerOrShiperSegment();
            if (projectCustomerOrShipperSegments == null)
            {
                return Json(new { SegmentID = 0 });
            }

            var result = projectCustomerOrShipperSegments.Where(p => p.Target == target && p.CustomerOrShipperID == customerOrShipperID && p.RelatedCustomerID == relatedCustomerID && p.ProjectID == projectID).FirstOrDefault();
            if (result == null)
            {
                return Json(new { SegmentID = 0 });
            }

            return Json(new { SegmentID = result.SegmentID });
        }

        [HttpPost]
        public JsonResult CustomerOrShipperSegmentAllocate(long customerOrShipperID, long segmentID, int target, long projectID, long relatedCustomerID)
        {
            var response = new ProjectService().SetProjectCustomerOrShipperSegment(new SetProjectCustomerOrShipperSegmentRequest()
            {
                ProjectID = projectID,
                CustomerOrShipperID = customerOrShipperID,
                Target = target,
                SegmentID = segmentID,
                RelatedCustomerID = relatedCustomerID
            });

            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshProjectCustomerOrShiperSegment();
                return Json(new { result = true });
            }

            return Json(new { result = false });
        }
        [HttpGet]
        public ActionResult UserWarehouse()
        {
            ProjectUserCustomerAllocateViewModel po = new ProjectUserCustomerAllocateViewModel();
          
            return View(po);
        }
        [HttpPost]
        public JsonResult UserWarehouse(string id)
        {
            return Json(new { result = false });
        }

    }
}