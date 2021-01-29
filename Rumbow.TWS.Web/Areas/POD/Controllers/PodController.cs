using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.POD.Models;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Implement;
using Runbow.TWS.Web.Interface;
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using SystemReflection = System.Reflection;
using Runbow.TWS.Web.ESPService;
using System.Reflection;
using Runbow.TWS.Entity.POD;
using Runbow.TWS.Biz.POD;
using Runbow.TWS.MessageContracts.POD.Adidas;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Runbow.TWS.MessageContracts.POD.Nike;
using Runbow.TWS.Entity.POD.Nike;
namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class PodController : BaseController
    {
        private string[] podStatusTrack_Status = { "提货", "干线", "配送" };

        private string[] podStatusTrack_TrackStatus = { "订单下达", "提货调车", "到车情况", "装车情况", "离开情况", "到达HUB", "干线配载", "干线发车", "干线跟踪", "到达终端", "配送调车", "配送跟踪", "运单签收", "回单上传" };

        public ActionResult Index(long? id)
        {
            var PodAll = new PodService().GetPodAndReleatedInfo(new GetPodAndReleatedInfoRequest() { ID = 13 }).Result;
            var ModuleConfig = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001");
            PodAllViewModel viewModel = new PodAllViewModel()
            {
                PodAll = PodAll,
                ModuleConfig = ModuleConfig
            };

            return View("ViewPodAll", viewModel);
        }

        public ActionResult ViewPodAll(long id, bool? showEditRelated, int? returnStep)
        {
            var PodAll = new PodService().GetPodAndReleatedInfo(new GetPodAndReleatedInfoRequest() { ID = id }).Result;
            var ModuleConfig = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001");
            PodAllViewModel viewModel = new PodAllViewModel()
            {
                PodAll = PodAll,
                ModuleConfig = ModuleConfig,
                ShowEditRelated = showEditRelated,
                ProjectRole = base.UserInfo.ProjectRoleID,
                ReturnStep = returnStep ?? 0
            };

            if (viewModel.PodAll.WXPODBarCode != null)
            {
                string ticketkey = viewModel.PodAll.WXPODBarCode.ticketkey;
                if (!string.IsNullOrEmpty(ticketkey))
                {
                    string url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticketkey;
                    ViewBag.WXPODBarCode = "<a href='" + url + "' target=\"_blank\">(查看二维码)</a>";
                }
            }
            else
            {
                ViewBag.WXPODBarCode = "";
            }

            if (viewModel.PodAll.Pod.CustomerID == 40)
            {

            }

            return View(viewModel);
        }

        public ActionResult ViewPodAllForOuterUser(long id, bool? showEditRelated, int? returnStep)
        {
            var PodAll = new PodService().GetPodAndReleatedInfo(new GetPodAndReleatedInfoRequest() { ID = id }).Result;
            var ModuleConfig = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001");
            ViewBag.ShowTrackEdit = base.UserInfo.UserType == 1 ? true : false;
            PodAllViewModel viewModel = new PodAllViewModel()
            {
                PodAll = PodAll,
                ModuleConfig = ModuleConfig,
                ShowEditRelated = showEditRelated,
                ProjectRole = base.UserInfo.ProjectRoleID,
                ReturnStep = returnStep ?? 0
            };
            return View(viewModel);
        }

        public ActionResult CreatePod(long? id, long? customerID)
        {
            PodViewModel model = new PodViewModel();
            model.IsEditModel = false;
            Runbow.TWS.Entity.Application.Project project = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
            model.ModuleConfig = project.ModuleCollection.First(m => m.Id == "M001");
            if (id.HasValue)
            {
                model.Pod = new PodService().GetPodAndReleatedInfo(new GetPodAndReleatedInfoRequest() { ID = id.Value }).Result.Pod;
                ViewBag.EditModel = true;
                model.IsEditModel = true;
            }
            else
            {
                model.Pod = new Pod();
                model.Pod.Type = 2;
                model.Pod.ProjectID = base.UserInfo.ProjectID;
                model.Pod.Creator = base.UserInfo.Name;
                model.Pod.ActualDeliveryDate = DateTime.Now;
                model.Pod.CreateTime = DateTime.Now;
                model.Pod.SystemNumber = "系统自动生成";
                model.Pod.PODStateID = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE).First(c => c.Code == "01").ID;
                model.Pod.PODStateName = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE).First(c => c.Code == "01").Name;
                ViewBag.EditModel = false;
            }
            model.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            model.PodStates = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            if (base.UserInfo.UserType == 0)
            {
                model.Customers = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                model.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            model.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID)
                                .Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            model.PODTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            model.TtlOrTpls = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.TTLORTPL)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            if (!id.HasValue)
            {
                if (!customerID.HasValue)
                {
                    model.Pod.CustomerID = model.Customers.First().Value.ObjectToInt64();
                    //model.Pod.CustomerID = model.Customers.FirstOrDefault().Value.ObjectToInt64();
                }
                else
                {
                    model.Pod.CustomerID = customerID.Value;
                }

                this.SetViewModelDefaultValue(model.ModuleConfig.Tables.TableCollection.First(t => t.Name == "Pod").ColumnCollection, typeof(Pod), model.Pod, model.Pod.CustomerID.Value);

                var ColumnCollection = model.ModuleConfig.Tables.TableCollection.First(t => t.Name == "Pod").ColumnCollection.Where(c => (c.IsKey && !c.IsHide) || (!c.IsKey && !c.IsHide) || (!c.IsKey && c.IsHide && c.InnerColumns.Any(innerc => innerc.CustomerID == model.Pod.CustomerID.Value))).Select(c =>
                {
                    if (c.InnerColumns.Count == 0)
                    {
                        return c;
                    }
                    else
                    {
                        if (c.InnerColumns.Any(innerc => innerc.CustomerID == model.Pod.CustomerID.Value))
                        {
                            return c.InnerColumns.First(innerc => innerc.CustomerID == model.Pod.CustomerID.Value);
                        }

                        return c;
                    }
                });

                if (ColumnCollection != null)
                {
                    ColumnCollection.Each((i, c) =>
                    {
                        if (string.Equals("FileUpload", c.Type, StringComparison.OrdinalIgnoreCase))
                        {
                            string groupID = Guid.NewGuid().ToString();
                            Type entityType = model.Pod.GetType();
                            PropertyInfo propertyInfo = entityType.GetProperty(c.DbColumnName);
                            if (propertyInfo != null)
                            {
                                propertyInfo.SetValue(model.Pod, groupID, null);
                            }
                        }
                    });
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CreatePod(PodViewModel vm)
        {
            var project = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
            vm.Pod.SystemNumber = this.GeneratePodSystemNumber(project.PODNumberCreator);
            var model = project.ModuleCollection.First(m => m.Id == "M001");
            if (!model.UseCustomerOrderNumber)
            {
                vm.Pod.CustomerOrderNumber = vm.Pod.SystemNumber;
            }

            var response = new PodService().AddPod(new AddPodRequest() { Pod = vm.Pod });
            if (response.IsSuccess)
            {
                return RedirectToAction("ViewPodAll", new { id = response.Result.ID, returnStep = 2 });
            }
            else
            {
                return RedirectToAction("Error", new { msg = "新增运单失败！" });
            }
        }

        [HttpPost]
        public JsonResult DeletePod(long id)
        {
            var response = new PodService().DeletePodAndRelatedInfo(new DeletePodInfoRequest() { ID = id });
            if (response.IsSuccess)
            {
                return Json(new { Message = "删除运单成功", IsSuccess = true });
            }
            else
            {
                if (response.Result == 0)
                {
                    return Json(new { Message = "运单已被其他信息关联，无法删除", IsSuccess = false });
                }

                return Json(new { Message = "运单删除失败！", IsSuccess = false });
            }
        }

        [HttpPost]
        public JsonResult SplitPod(long id, int splitNumber)
        {
            var response = new PodService().SplitPod(new SplitPodRequest() { ID = id, SplitNumber = splitNumber });
            if (response.IsSuccess)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string i in response.Result)
                {
                    sb.Append(i);
                    sb.Append(",");
                }
                return Json(new { Message = "成功将运单拆成" + sb.ToString() + "请查询", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "运单拆分失败！", IsSuccess = false });
            }
        }

        private string GeneratePodSystemNumber(string implementClass)
        {
            ICreatePodSystemNumber creator;

            if (!string.IsNullOrEmpty(implementClass))
            {
                creator = Activator.CreateInstance(Type.GetType(implementClass)) as ICreatePodSystemNumber;
                if (creator == null)
                {
                    creator = new DefaultCreatePodSystemNumber();
                }
            }
            else
            {
                creator = new DefaultCreatePodSystemNumber();
            }

            return creator.CreatePodSystemNumber();
        }

        private int GetTodayPodNumber(string implementClass, out string systemNumberPrefix)
        {
            ICreatePodSystemNumber creator;

            if (!string.IsNullOrEmpty(implementClass))
            {
                creator = Activator.CreateInstance(Type.GetType(implementClass)) as ICreatePodSystemNumber;

                if (creator == null)
                {
                    creator = new DefaultCreatePodSystemNumber();
                }
            }
            else
            {
                creator = new DefaultCreatePodSystemNumber();
            }

            return creator.GetTodaysPodNumber(out systemNumberPrefix);
        }

        public ActionResult PodDetailManage(long id, string systemNumber, string customerOrderNumber, long customerID)
        {
            IList<long> ids = new List<long>();
            ids.Add(id);

            PodDetailManageViewModel vm = new PodDetailManageViewModel()
            {
                PodDetail = new PodDetail() { PodID = id, SystemNumber = systemNumber, CustomerOrderNumber = customerOrderNumber, Creator = base.UserInfo.Name, CreateTime = DateTime.Now },
                PodDetails = new PodService().GetPodDetailsByPodIDs(new GetPodInfoRequest() { PodIDs = ids }).Result,
                Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodDetail"),
                IsEditModel = true,
                CustomerID = customerID
            };

            this.SetViewModelDefaultValue(vm.Config.ColumnCollection, typeof(PodDetail), vm.PodDetail, customerID);
            return View(vm);
        }

        [HttpPost]
        public ActionResult PodDetailManage(PodDetailManageViewModel vm)
        {
            PodDetail podDetail = vm.PodDetail;
            var response = new PodService().AddPodDetail(new AddPodDetailRequest() { PodDetail = podDetail });
            if (response.IsSuccess)
            {
                return RedirectToAction("PodDetailManage", new { id = vm.PodDetail.PodID, systemNumber = vm.PodDetail.SystemNumber, customerOrderNumber = vm.PodDetail.CustomerOrderNumber, customerID = vm.CustomerID });
            }
            else
            {
                return RedirectToAction("Error", new { msg = "新增运单明细失败！" });
            }
        }

        [HttpPost]
        public ActionResult DeletePodDetail(long id)
        {
            if (new PodService().DeletePodDetailByID(new DeletePodInfoRequest() { ID = id }).IsSuccess)
            {
                return Json("删除运单明细成功");
            }

            throw new Exception("删除运单明细失败");
        }

        public ActionResult PodStatusLogManage(long id, string systemNumber, string customerOrderNumber, long customerID)
        {
            IList<long> ids = new List<long>();
            ids.Add(id);

            PodStatusLogManageViewModel vm = new PodStatusLogManageViewModel()
            {
                PodStatusLog = new PodStatusLog() { PodID = id, SystemNumber = systemNumber, CustomerOrderNumber = customerOrderNumber, Creator = base.UserInfo.Name, CreateTime = DateTime.Now },
                PodStatusLogs = new PodService().GetPodStatusLogsByPodIDs(new GetPodInfoRequest() { PodIDs = ids }).Result,
                Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodStatusLog"),
                IsEditModel = true,
                CustomerID = customerID
            };

            var ColumnCollection = vm.Config.ColumnCollection.Where(c => (c.IsKey && !c.IsHide) || (!c.IsKey && !c.IsHide) || (!c.IsKey && c.IsHide && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.CustomerID))).Select(c =>
            {
                if (c.InnerColumns.Count == 0)
                {
                    return c;
                }
                else
                {
                    if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.CustomerID))
                    {
                        return c.InnerColumns.First(innerc => innerc.CustomerID == vm.CustomerID);
                    }

                    return c;
                }
            });

            if (ColumnCollection != null)
            {
                ColumnCollection.Each((i, c) =>
                {
                    if (string.Equals("FileUpload", c.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        string groupID = customerOrderNumber + "_" + Guid.NewGuid().ToString();
                        Type entityType = vm.PodStatusLog.GetType();
                        PropertyInfo propertyInfo = entityType.GetProperty(c.DbColumnName);
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(vm.PodStatusLog, groupID, null);
                        }
                    }
                });
            }

            this.SetViewModelDefaultValue(vm.Config.ColumnCollection, typeof(PodStatusLog), vm.PodStatusLog, customerID);
            return View(vm);
        }

        [HttpPost]
        public ActionResult PodStatusLogManage(PodStatusLogManageViewModel vm)
        {
            PodStatusLog podStatusLog = vm.PodStatusLog;
            var response = new PodService().AddPodStatusLog(new AddPodStatusLogRequest() { PodStatusLog = podStatusLog });
            if (response.IsSuccess)
            {
                return RedirectToAction("PodStatusLogManage", new { id = vm.PodStatusLog.PodID, systemNumber = vm.PodStatusLog.SystemNumber, customerOrderNumber = vm.PodStatusLog.CustomerOrderNumber, customerID = vm.CustomerID });
            }
            else
            {
                return RedirectToAction("Error", new { msg = "新增运单车辆信息失败！" });
            }
        }

        [HttpPost]
        public ActionResult PodStatusTrackManage(PodStatusTrackManageViewModel vm)
        {
            PodStatusTrack podStatusTrack = vm.PodStatusTrack;
            var response = new PodService().AddPodStatusTrack(new AddPodStatusTrackRequest() { PodStatusTrack = podStatusTrack });
            if (response.IsSuccess)
            {
                return RedirectToAction("PodStatusTrackManage", new { id = vm.PodStatusTrack.PodID, systemNumber = vm.PodStatusTrack.SystemNumber, customerOrderNumber = vm.PodStatusTrack.CustomerOrderNumber, customerID = vm.CustomerID });
            }
            else
            {
                return RedirectToAction("Error", new { msg = "新增运单跟踪失败！" });
            }
        }

        public ActionResult PodStatusTrackManage(long id, string systemNumber, string customerOrderNumber, long customerID)
        {
            IList<long> ids = new List<long>();
            ids.Add(id);

            PodStatusTrackManageViewModel vm = new PodStatusTrackManageViewModel()
            {
                PodStatusTrack = new PodStatusTrack() { PodID = id, SystemNumber = systemNumber, CustomerOrderNumber = customerOrderNumber, Creator = base.UserInfo.Name, CreateTime = DateTime.Now },
                PodStatusTracks = new PodService().GetPodStatusTracksByPodIDs(new GetPodInfoRequest() { PodIDs = ids }).Result,
                Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodStatusTrack"),
                IsEditModel = true,
                CustomerID = customerID
            };

            var ColumnCollection = vm.Config.ColumnCollection.Where(c => (c.IsKey && !c.IsHide) || (!c.IsKey && !c.IsHide) || (!c.IsKey && c.IsHide && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.CustomerID))).Select(c =>
            {
                if (c.InnerColumns.Count == 0)
                {
                    return c;
                }
                else
                {
                    if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.CustomerID))
                    {
                        return c.InnerColumns.First(innerc => innerc.CustomerID == vm.CustomerID);
                    }

                    return c;
                }
            });

            if (ColumnCollection != null)
            {
                ColumnCollection.Each((i, c) =>
                {
                    if (string.Equals("FileUpload", c.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        string groupID = customerOrderNumber + "_" + Guid.NewGuid().ToString();
                        Type entityType = vm.PodStatusTrack.GetType();
                        PropertyInfo propertyInfo = entityType.GetProperty(c.DbColumnName);
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(vm.PodStatusTrack, groupID, null);
                        }
                    }
                });
            }

            this.SetViewModelDefaultValue(vm.Config.ColumnCollection, typeof(PodStatusTrack), vm.PodStatusTrack, customerID);
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeletePodStatusLog(long id)
        {
            if (new PodService().DeletePodStatusLogByID(new DeletePodInfoRequest() { ID = id }).IsSuccess)
            {
                return Json("删除运单车辆信息成功");
            }

            throw new Exception("删除运单车辆信息失败");
        }

        [HttpPost]
        public ActionResult DeletePodStatusTrack(long id)
        {
            if (new PodService().DeletePodStatusTrackByID(new DeletePodInfoRequest() { ID = id }).IsSuccess)
            {
                return Json("删除运单状态跟踪成功");
            }

            throw new Exception("删除运单状态跟踪失败");
        }

        public ActionResult PodExceptionManage(long id, string systemNumber, string customerOrderNumber, long customerID)
        {
            IList<long> ids = new List<long>();
            ids.Add(id);

            PodExceptionManageViewModel vm = new PodExceptionManageViewModel()
            {
                PodException = new PodException() { PodID = id, SystemNumber = systemNumber, CustomerOrderNumber = customerOrderNumber, Creator = base.UserInfo.Name, CreateTime = DateTime.Now },
                PodExceptions = new PodService().GetPodExceptionsByPodIDs(new GetPodInfoRequest() { PodIDs = ids }).Result,
                Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodException"),
                IsEditModel = true,
                CustomerID = customerID
            };

            this.SetViewModelDefaultValue(vm.Config.ColumnCollection, typeof(PodException), vm.PodException, customerID);
            return View(vm);
        }

        [HttpPost]
        public ActionResult PodExceptionManage(PodExceptionManageViewModel vm)
        {
            PodException podException = vm.PodException;
            var response = new PodService().AddPodException(new AddPodExceptionRequest() { PodException = podException });
            if (response.IsSuccess)
            {
                return RedirectToAction("PodExceptionManage", new { id = vm.PodException.PodID, systemNumber = vm.PodException.SystemNumber, customerOrderNumber = vm.PodException.CustomerOrderNumber, customerID = vm.CustomerID });
            }
            else
            {
                return RedirectToAction("Error", new { msg = "新增运单异常失败！" });
            }
        }

        [HttpPost]
        public ActionResult DeletePodException(long id)
        {
            if (new PodService().DeletePodExceptionByID(new DeletePodInfoRequest() { ID = id }).IsSuccess)
            {
                return Json("删除运单异常成功");
            }

            throw new Exception("删除运单异常失败");
        }

        public ActionResult PodTrackManage(long id, string systemNumber, string customerOrderNumber, long customerID)
        {
            IList<long> ids = new List<long>();
            ids.Add(id);
            PodTrackManageViewModel vm = new PodTrackManageViewModel()
            {

                PodTrack = new PodTrack() { PodID = id, SystemNumber = systemNumber, CustomerOrderNumber = customerOrderNumber, Creator = base.UserInfo.Name, CreateTime = DateTime.Now },
                PodTracks = new PodService().GetPodTracksByPodIDs(new GetPodInfoRequest() { PodIDs = ids, PodID = id }).Result,
                Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodTrack"),
                IsEditModel = true,
                CustomerID = customerID,
                IsOuterUser = base.UserInfo.UserType == 2 ? false : true
            };

            this.SetViewModelDefaultValue(vm.Config.ColumnCollection, typeof(PodTrack), vm.PodTrack, customerID);
            return View(vm);
        }

        private void SetViewModelDefaultValue(IEnumerable<Column> columnCollection, Type type, Object obj, long customerID)
        {
            columnCollection
                   .Where(c => (!c.IsKey && !c.IsHide) || (!c.IsKey && c.IsHide && c.InnerColumns.Any(innerc => innerc.CustomerID == customerID)))
                   .Select(c =>
                   {
                       if (c.InnerColumns.Count == 0)
                       {
                           return c;
                       }
                       else
                       {
                           if (c.InnerColumns.Any(innerc => innerc.CustomerID == customerID))
                           {
                               return c.InnerColumns.First(innerc => innerc.CustomerID == customerID);
                           }

                           return c;
                       }
                   }).Each((i, k) =>
                   {
                       if (!string.IsNullOrEmpty(k.DefaultValue))
                       {
                           SystemReflection.PropertyInfo property = type.GetProperty(k.DbColumnName);
                           if (property != null)
                           {
                               if (string.Equals(k.Type, "DateTime", StringComparison.OrdinalIgnoreCase))
                               {
                                   property.SetValue(obj, DateTime.Now.AddDays(k.DefaultValue.ObjectToInt32()), null);
                               }
                               else
                               {
                                   property.SetValue(obj, k.DefaultValue, null);
                               }
                           }
                       }
                   });
        }
        [HttpPost]
        public ActionResult PodTrackManage(PodTrackManageViewModel vm)
        {
            PodTrack podTrack = vm.PodTrack;
            var response = new PodService().AddPodTrack(new AddPodTrackRequest() { PodTrack = podTrack });
            if (response.IsSuccess)
            {
                return RedirectToAction("PodTrackManage", new { id = vm.PodTrack.PodID, systemNumber = vm.PodTrack.SystemNumber, customerOrderNumber = vm.PodTrack.CustomerOrderNumber, customerID = vm.CustomerID });
            }
            else
            {
                return RedirectToAction("Error", new { msg = "新增运单跟踪失败！" });
            }
        }

        [HttpPost]
        public ActionResult DeletePodTrack(long id)
        {
            if (new PodService().DeletePodTrackByID(new DeletePodInfoRequest() { ID = id }).IsSuccess)
            {
                return Json("删除运单跟踪成功");
            }

            throw new Exception("删除运单跟踪失败");
        }

        public ActionResult PodFeadBackManage(long id, string systemNumber, string customerOrderNumber, long customerID)
        {
            PodFeadBackManageViewModel vm = new PodFeadBackManageViewModel()
            {
                PodFeadBack = new PodService().GetPodFeadBackByPodID(new GetPodInfoRequest() { PodID = id }).Result,
                Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodFeadBack"),
                IsEditModel = true,
                CustomerID = customerID
            };

            if (vm.PodFeadBack == null)
            {
                vm.PodFeadBack = new PodFeadBack();
                vm.PodFeadBack.PodID = id;
                vm.PodFeadBack.SystemNumber = systemNumber;
                vm.PodFeadBack.CustomerOrderNumber = customerOrderNumber;
                this.SetViewModelDefaultValue(vm.Config.ColumnCollection, typeof(PodFeadBack), vm.PodFeadBack, customerID);
            }

            vm.PodFeadBack.Creator = base.UserInfo.Name;
            vm.PodFeadBack.CreateTime = DateTime.Now;

            return View(vm);
        }

        [HttpPost]
        public ActionResult PodFeadBackManage(PodFeadBackManageViewModel vm)
        {
            var response = new PodService().AddOrUpdatePodFeadBack(new AddOrUpdatePodFeadBackRequest() { PodFeadBack = vm.PodFeadBack });
            if (response.IsSuccess)
            {
                return RedirectToAction("ViewPodAll", new { id = vm.PodFeadBack.PodID, returnStep = 2 });
            }
            else
            {
                return RedirectToAction("Error", new { msg = "新增编辑运单回访信息失败！" });
            }
        }

        public ActionResult PodFeeManage(long id, string systemNumber, string customerOrderNumber, long customerID)
        {
            PodFeeManageViewModel vm = new PodFeeManageViewModel()
            {
                PodFee = new PodService().GetPodFeeByPodID(new GetPodInfoRequest() { PodID = id }).Result,
                Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodFee"),
                IsEditModel = true,
                CustomerID = customerID,
                IsCoverOld = true
            };

            if (vm.PodFee == null)
            {
                vm.PodFee = new PodFee();
                vm.PodFee.PodID = id;
                vm.PodFee.SystemNumber = systemNumber;
                vm.PodFee.CustomerOrderNumber = customerOrderNumber;
                this.SetViewModelDefaultValue(vm.Config.ColumnCollection, typeof(PodFee), vm.PodFee, customerID);
            }

            vm.PodFee.Creator = base.UserInfo.Name;
            vm.PodFee.CreateTime = DateTime.Now;

            return View(vm);
        }

        [HttpPost]
        public ActionResult PodFeeManage(PodFeeManageViewModel vm)
        {
            var response = new PodService().AddPodFees(new AddPodFeesRequest()
            {
                PodFees = new PodFee[] { vm.PodFee },
                CustomerID = vm.CustomerID
            });

            if (response.IsSuccess)
            {
                return RedirectToAction("ViewPodAll", new { id = vm.PodFee.PodID });
            }
            else
            {
                return RedirectToAction("Error", new { msg = "新增编辑费用结算信息失败！" });
            }
        }

        public ActionResult PodReplyDocumentManage(long id, string systemNumber, string customerOrderNumber, long customerID)
        {
            PodReplyDocumentManageViewModel vm = new PodReplyDocumentManageViewModel()
            {
                PodReplyDocument = new PodService().GetPodReplyDocumentByPodID(new GetPodInfoRequest() { PodID = id }).Result,
                Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodReplyDocument"),
                IsEditModel = true,
                CustomerID = customerID,
                IsCoverOld = true
            };

            //TODO: Hilti Special
            if (customerID == 2)
            {
                vm.IsCoverOld = false;
            }

            if (vm.PodReplyDocument == null)
            {
                vm.PodReplyDocument = new PodReplyDocument();
                vm.PodReplyDocument.PodID = id;
                vm.PodReplyDocument.SystemNumber = systemNumber;
                vm.PodReplyDocument.CustomerOrderNumber = customerOrderNumber;
                this.SetViewModelDefaultValue(vm.Config.ColumnCollection, typeof(PodReplyDocument), vm.PodReplyDocument, customerID);
            }

            vm.PodReplyDocument.Creator = base.UserInfo.Name;
            vm.PodReplyDocument.CreateTime = DateTime.Now;

            return View(vm);
        }

        [HttpPost]
        public ActionResult PodReplyDocumentManage(PodReplyDocumentManageViewModel vm)
        {
            var response = new PodService().AddOrUpdatePodReplyDocument(new AddOrUpdatePodReplyDocumentRequest() { PodReplyDocument = vm.PodReplyDocument });
            if (response.IsSuccess)
            {
                return RedirectToAction("ViewPodAll", new { id = vm.PodReplyDocument.PodID });
            }
            else
            {
                return RedirectToAction("Error", new { msg = "新增编辑回单信息失败！" });
            }
        }

        [HttpPost]
        public ActionResult AllocatePodShipperAsync(string ids)
        {
            var podIDs = ids.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);

            var response = new PodService().SetPodShipper(new SetPodShipperRequest() { ProjectID = base.UserInfo.ProjectID, IDs = podIDs });
            if (response.IsSuccess)
            {
                return Json(response.Result);
            }

            throw response.Exception;
        }

        [HttpPost]
        public ActionResult AllocatePodShipperManuallyAsync(string ids, long shipperID, string shipperName)
        {
            var podIDs = ids.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);

            var response = new PodService().SetPodShipperManually(new SetPodShipperManuallyRequest() { ProjectID = base.UserInfo.ProjectID, IDs = podIDs, ShipperID = shipperID, ShipperName = shipperName });
            if (response.IsSuccess)
            {
                return Json(response.Result);
            }

            throw response.Exception;
        }

        public ActionResult BatchEditPods(int? id, bool? hideActionButton, bool? isSplitPod, bool? showEditRelated, bool? isAllocateShipper, bool? isSettled, int? settledType, bool? isUsedForOriginalPOD, long? customerID, int? extenFeeType, int? manualSettledType, bool? isUsedForSendForecast, bool? IsReturnPodStatus, bool? IsWenXinStatus, bool? isPODDistributionVehicle, bool? WaybillReach)
        {
            BatchEditPodsViewModel vm = new BatchEditPodsViewModel();
            vm.SearchCondition = new PodSearchCondition();
            this.GenQueryPodViewModel(vm);
            if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.ShipperID = base.UserInfo.CustomerOrShipperID;
            }

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }

            vm.SearchCondition.PODStateID = id;
            vm.HideActionButton = hideActionButton ?? false;
            vm.ActionButtonName = this.GenActionButtonNameByID(id ?? 0);
            vm.DestPodState = id.HasValue ? (long)(id.Value + 1) : 1;
            vm.IsSplit = isSplitPod ?? false;
            vm.DestPodStateName = vm.PodStates.First(i => string.Equals(i.Value, vm.DestPodState.ToString(), StringComparison.OrdinalIgnoreCase)).Text;
            vm.ShowEditRelated = showEditRelated ?? true;
            vm.IsAllocateShipper = isAllocateShipper ?? false;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            vm.ProjectRoleID = base.UserInfo.ProjectRoleID;
            vm.IsSettled = isSettled ?? false;
            vm.SettltedType = settledType;
            vm.SearchCondition.IsUsedForOriginalPOD = isUsedForOriginalPOD ?? false;
            vm.IsPODDistributionVehicle = isPODDistributionVehicle ?? false;
            vm.WaybillReach = WaybillReach ?? false;
            if (vm.IsSettled)
            {
                if (vm.SettltedType.Value == 0)
                {
                    vm.SearchCondition.IsSettledForCustomer = false;
                }
                else if (vm.SettltedType.Value == 1)
                {
                    vm.SearchCondition.IsSettledForShipper = false;
                }
            }

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.ShipperID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }

            string customerName = string.Empty;
            if (vm.SearchCondition.CustomerID.HasValue)
            {
                var customer = ApplicationConfigHelper.GetApplicationCustomers().FirstOrDefault(c => c.ID == vm.SearchCondition.CustomerID.Value);
                if (customer != null)
                {
                    customerName = customer.Name;
                }
            }

            //TODO: HardCode
            if (!string.Equals(customerName, "Hilti", StringComparison.OrdinalIgnoreCase))
            {
                vm.SearchCondition.ActualDeliveryDate = DateTime.Now.AddDays(-7);
                vm.SearchCondition.EndActualDeliveryDate = DateTime.Now.AddDays(7);
            }

            if (extenFeeType != null)
            {
                vm.SearchCondition.PODStateID = null;
                vm.IsExternFee = true;
                vm.ExternFeeType = extenFeeType.Value;
                switch (extenFeeType.Value)
                {
                    case 1:
                        vm.SearchCondition.HasShortDial = false;
                        break;
                    case 2:
                        vm.SearchCondition.HasDistribution = false;
                        break;
                    case 3:
                        vm.SearchCondition.HasExpress = false;
                        break;
                    default:
                        throw new Exception("Error parameter");
                }
            }

            if (manualSettledType != null)
            {
                vm.SearchCondition.PODStateID = null;
                vm.IsManualSettled = true;
                vm.ManualSettledType = manualSettledType.Value;
                if (manualSettledType.Value == 0)
                {
                    vm.SearchCondition.IsSettledForCustomer = false;
                }
                else if (manualSettledType.Value == 1)
                {
                    vm.SearchCondition.IsSettledForShipper = false;
                }
            }

            if (isUsedForSendForecast != null && isUsedForSendForecast.Value)
            {
                vm.IsUsedForSendForecast = true;
                vm.SearchCondition.PODStateID = null;
                vm.SearchCondition.PodMinStateID = 2;
            }

            if (IsReturnPodStatus.HasValue)
            {
                vm.IsReturnPodStatus = IsReturnPodStatus.Value;
                vm.SearchCondition.PODStateID = 0;
            }

            if (IsWenXinStatus.HasValue)
            {
                vm.IsWenXinStatus = IsWenXinStatus.Value;
                vm.SearchCondition.PODStateID = id;
            }
            return View(vm);
        }

        private string GenActionButtonNameByID(int id)
        {
            switch (id)
            {
                case 1:
                    return "审核";

                case 2:
                    return "调度确认";

                case 3:
                    return "发运确认";

                case 4:
                    return "HUB确认";

                case 5:
                    return "配送确认";

                case 6:
                    return "签收确认";

                case 7:
                    return "结案确认";

                default:
                    return string.Empty;
            }
        }

        [HttpPost]
        public ActionResult BatchEditPods(BatchEditPodsViewModel vm)
        {
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            if (vm.SearchCondition.UserType == 2)
            {
                vm.SearchCondition.CustomerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            }

            vm.SearchCondition.Types = new List<int>();

            if (vm.IsSplit)
            {
                vm.SearchCondition.Types.Clear();
                vm.SearchCondition.Types.Add(0);
                vm.SearchCondition.Types.Add(1);
                vm.SearchCondition.Types.Add(2);
            }

            if (vm.IsAllocateShipper)
            {
                vm.SearchCondition.Types.Clear();
                vm.SearchCondition.Types.Add(1);
                vm.SearchCondition.Types.Add(2);

                if (vm.HasAllocateShipper.HasValue)
                {
                    vm.SearchCondition.HasAllocateShipper = vm.HasAllocateShipper.Value == 0 ? false : true;
                }
            }

            if (vm.SearchCondition.IsUsedForOriginalPOD)
            {
                vm.SearchCondition.Types.Clear();
                vm.SearchCondition.Types.Add(0);
            }

            if (vm.IsSettled)
            {
                vm.SearchCondition.Types.Clear();
                if (vm.SettltedType.Value == 0)
                {
                    vm.SearchCondition.Types.Add(0);
                    vm.SearchCondition.Types.Add(2);
                }
                else
                {
                    vm.SearchCondition.Types.Add(1);
                    vm.SearchCondition.Types.Add(2);
                }
            }

            if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.Types.Clear();
                vm.SearchCondition.Types.Add(1);
                vm.SearchCondition.Types.Add(2);
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.Types.Clear();
                vm.SearchCondition.Types.Add(0);
                vm.SearchCondition.Types.Add(2);
            }

            if (vm.IsExternFee)
            {
                vm.SearchCondition.Types.Clear();
                vm.SearchCondition.Types.Add(0);
                vm.SearchCondition.Types.Add(2);
            }

            if (vm.IsManualSettled)
            {
                vm.SearchCondition.Types.Clear();
                vm.SearchCondition.Types.Add(0);
                vm.SearchCondition.Types.Add(2);
            }

            vm.PodCollection = new PodService().QueryPodWithNoPaging(new QueryPodRequest() { SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result.Select(p => new PodWithAttachment(p));
            this.GenQueryPodViewModel(vm);

            if (!string.IsNullOrEmpty(vm.SearchCondition.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (vm.SearchCondition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = vm.SearchCondition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (vm.SearchCondition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = vm.SearchCondition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers == null || !customerOrderNumbers.Any())
                {
                    customerOrderNumbers = new string[] { vm.SearchCondition.CustomerOrderNumber };
                }

                var notContainsCustomerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c) &&
                        !vm.PodCollection.Any(p => string.Equals(p.CustomerOrderNumber, c.Trim(), StringComparison.OrdinalIgnoreCase))
                    );

                if (notContainsCustomerOrderNumbers != null && notContainsCustomerOrderNumbers.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("您输入的客户运单号:");
                    notContainsCustomerOrderNumbers.Each((i, c) => { sb.Append(c).Append(","); });
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("在结果中没有出现,系统中不存在此客户运单号或者请变更其他查询条件.");
                    vm.ReturnClientMessage = sb.ToString();
                }
            }

            return View(vm);
        }
        [HttpGet]
        public ActionResult GetPODDistributionVehicle(int? PageIndex)
        {
            NikePODForBSModel nm = new NikePODForBSModel();
            NikePodForBSCondition np = new NikePodForBSCondition();
            np.StartDeliveryTime = (DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            np.EndDeliveryTime = (DateTime.Now.AddDays(0).ToString("yyyy-MM-dd"));
            np.IsConversion = 0;
            nm.Condition = np;
            var response = new PodService().GetPODDistributionVehicle(new NikePODForBSRequest()
            {
                Condition = nm.Condition,
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE
            });
            if (response.IsSuccess)
            {
                nm.PodCollection = response.Result.PodCollection;
                nm.PageCount = response.Result.PageCount;
                nm.PageIndex = response.Result.PageIndex;

            }
            return View(nm);
        }
        [HttpPost]
        public ActionResult GetPODDistributionVehicle(NikePODForBSModel nm, int? PageIndex)
        {
            var response = new PodService().GetPODDistributionVehicle(new NikePODForBSRequest()
            {
                Condition = nm.Condition,
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE
            });
            //var response = new NikeService().GetNikePOD(new NikePODForBSRequest()
            //{
            //    Condition = nm.Condition,
            //    PageIndex = PageIndex ?? 0,
            //    PageSize = UtilConstants.PAGESIZE
            //});
            if (response.IsSuccess)
            {
                nm.PodCollection = response.Result.PodCollection;
                nm.PageCount = response.Result.PageCount;
                nm.PageIndex = response.Result.PageIndex;
            }
            return View(nm);
        }
        public JsonResult CancelPODDistributionVehicle(string ids, string PODType)
        {
            IEnumerable<long> podIDs = ids.Split(',').Select(p => p.ObjectToInt64());
            var result = new PodService().CancelPODDistributionVehicle(new PODDistributionVehicle { Ids = podIDs, PODType = PODType });
            if (result > 0)
            {
                return Json(new { Code = 1 });
            }
            return Json(new { Code = 0 });
        }

        public JsonResult PODDistributionVehicle(string ids, string CarNo, string DriverName, string DriverPhone, string PODType, string StartTime, string GPSCode)
        {
            //BatchEditPodsViewModel bm = new BatchEditPodsViewModel();
            //PodSearchCondition pc = new PodSearchCondition();
            //bm.SearchCondition = pc;
            if (string.IsNullOrEmpty(GPSCode.Trim()))
            {
                GPSCode = CarNo;
            }
            try
            {
                IEnumerable<long> podIDs = ids.Split(',').Select(p => p.ObjectToInt64());
                var result = new PodService().PODDistributionVehicle(new PODDistributionVehicle { Ids = podIDs, CarNo = CarNo.Trim(), DriverName = DriverName.Trim(), DriverPhone = DriverPhone.Trim(), PODType = PODType, StartTime = StartTime, UserName = base.UserInfo.Name, GPSCode = GPSCode.Trim() });
                if (result > 0)
                {
                    return Json(new { Code = 1 });
                }
                return Json(new { Code = 0 });
            }
            catch (Exception e)
            {

                return Json(new { Code = 0 });
            }

        }
        public JsonResult GetCarInfo(string CarNo)
        {
            // ApplicationConfigHelper.RefreshGetWarehouseAreaList(str);
            var CarInfo = ApplicationConfigHelper.GetCarInfo();
            return Json(CarInfo.Where(s => s.VehicleNo.IndexOf(CarNo, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Name = "", Phone = "", Value = t.VehicleNo, Text = t.VehicleNo }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult WaybillReach(string ids, string EndTime, string PODType, string Hub)
        {
            BatchEditPodsViewModel bm = new BatchEditPodsViewModel();
            //PodSearchCondition pc = new PodSearchCondition();
            //bm.SearchCondition = pc;
            var podIDs = ids.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);
            var result = new PodService().WaybillReach(new PODDistributionVehicle { Ids = podIDs, EndTime = EndTime, PODType = PODType, Hub = Hub });
            if (result > 0)
            {
                return Json(new { Code = 1 });
            }
            return Json(new { Code = 0 });
        }
        [HttpPost]
        public JsonResult SendForecastEmail(long CustomerID, string ids)
        {
            var podIDs = ids.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);
            var customerName = ApplicationConfigHelper.GetApplicationCustomers().FirstOrDefault(c => c.ID == CustomerID).Name;
            PodService service = new PodService();
            var response = service.GetPodForecastInfoCollection(
                new GetPodForecastInfoCollectionRequest()
                {
                    PodIDs = podIDs,
                    CustomerID = CustomerID,
                    ProjectID = base.UserInfo.ProjectID
                });
            StringBuilder IDsSb = new StringBuilder();
            if (response.IsSuccess)
            {
                IEnumerable<Column> columnCollection = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection
                    .First(
                        p => p.Id == base.UserInfo.ProjectID.ToString()
                    ).ModuleCollection
                    .First(m =>
                        m.Id == "M001").Tables.TableCollection
                    .First(t =>
                        t.Name == "Pod").ColumnCollection
                    .Select(
                        c =>
                        {
                            if (c.InnerColumns.Count == 0)
                            {
                                return c;
                            }
                            else
                            {
                                if (c.InnerColumns.Any(innec => innec.CustomerID == CustomerID))
                                {
                                    return c.InnerColumns.First(innerc => innerc.CustomerID == CustomerID);
                                }
                                return c;
                            }
                        })
                    .Where(
                        c =>
                            ((c.IsKey && c.ForView) || (!c.IsKey && !c.IsHide)) && c.ShowRoleIDs.Contains(3)
                    );
                foreach (var podForecastInfo in response.Result)
                {
                    if (string.IsNullOrEmpty(podForecastInfo.EmailAddress) || string.IsNullOrEmpty(podForecastInfo.EmailContent))
                    {
                        IDsSb.Append(podForecastInfo.IDs).Append(",");
                        continue;
                    }

                    var pIDs = podForecastInfo.IDs.Split(',');
                    if (!pIDs.Any())
                    {
                        continue;
                    }

                    var podCollectionResult = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = pIDs.Select(i => i.ObjectToInt64()) });
                    if (!podCollectionResult.IsSuccess)
                    {
                        IDsSb.Append(podForecastInfo.IDs).Append(",");
                        continue;
                    }

                    podCollectionResult.Result.Each((i, p) =>
                    {
                        if (p.CustomerOrderNumber.StartsWith("0"))
                        {
                            p.CustomerOrderNumber = "'" + p.CustomerOrderNumber;
                        }
                    });

                    DataTable dt = new DataTable();

                    columnCollection.Each((i, c) => dt.Columns.Add(c.DbColumnName));
                    var resultTable = podCollectionResult.Result.ConverToTable(dt);

                    columnCollection.Each((i, c) =>
                    {
                        if (resultTable.Columns.Contains(c.DbColumnName))
                        {
                            resultTable.Columns[c.DbColumnName].ColumnName = c.DisplayName;
                        }
                    });

                    columnCollection.Where(c => !c.IsKey && (c.Type == "CheckBox" || c.Type == "DropDownList"))
                        .Each((i, c) =>
                        {
                            for (int j = 0; j < resultTable.Rows.Count; j++)
                            {
                                resultTable.Rows[j][c.DisplayName] = resultTable.Rows[j][c.DisplayName].ToString().Trim() == "1" ? "Y" : "N";
                            }

                        });


                    ExcelHelper excelHelper = new ExcelHelper();
                    string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
                    string name = DateTime.Now.ToString("yyyyMMddHHmmss") + podForecastInfo.ShipperName + "发货预报";
                    string fileFullPath = Path.Combine(targetPath, name + ".xlsx");
                    excelHelper.CreateExcelByDataTable(fileFullPath, resultTable);
                    excelHelper.Dispose();
                    string ZipfilePath = Path.Combine(targetPath, name + ".zip");
                    if (Runbow.TWS.Common.ZipHelper.ZipFile(fileFullPath, ZipfilePath, ""))
                    {
                        EmailSending sending = new EmailSending();
                        ESPServiceSoapClient addrequest = new ESPServiceSoapClient();
                        sending.ProjectID = CustomerID.ObjectToInt32();
                        sending.ProjectName = customerName;
                        sending.EmailTitle = DateTime.Now.ToString("yyyy年MM月dd日") + podForecastInfo.ShipperName + "发货预报";
                        sending.EmailAdd = podForecastInfo.EmailAddress;
                        sending.EmailSendContent = podForecastInfo.EmailContent;
                        FileStream fs = new FileStream(ZipfilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        byte[] bytes = new byte[fs.Length];
                        fs.Read(bytes, 0, (int)fs.Length);
                        fs.Dispose();
                        fs.Close();
                        sending.EmailAnnex = bytes;

                        sending.Creator = base.UserInfo.Name;

                        sending.ToCC = "";
                        sending.AnnexName = name + ".zip";
                        if (!addrequest.AddEmailSending(sending))
                        {
                            IDsSb.Append(podForecastInfo.IDs).Append(",");
                            continue;
                        }
                    }
                    else
                    {
                        IDsSb.Append(podForecastInfo.IDs).Append(",");
                        continue;
                    }
                }

                if (IDsSb.Length > 0)
                {
                    IDsSb.Remove(IDsSb.Length - 1, 1);
                }

                return Json(new { IsSuccess = true, Message = "发货预报邮件请求已提交,系统会在3分钟内发送完成", NotSendPodIDs = IDsSb.ToString() });
            }

            return Json(new { IsSuccess = false, Message = response.Exception.Message });
        }


        [HttpPost]
        public JsonResult BatchEditPodStatusAsync(string ids, long podStateID, string podStateName)
        {
            var podIDs = ids.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);
            var response = new PodService().SetPodStatus(new SetPodStatusRequest() { IDs = podIDs, PodStatusID = podStateID, PodStatusName = podStateName, IsSendMessage = "true" });
            if (response.IsSuccess)
            {
                return Json(podIDs.Select(p => new { ID = p }));
            }

            throw response.Exception;
        }

        /// <summary>
        /// 批量微信生成二维码
        /// </summary>
        [HttpPost]
        public string BatchEditPodWenXin(string ids)
        {
            IEnumerable<long> podIDs = ids.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);
            string podIds = string.Empty;
            foreach (long i in podIDs)
            {
                podIds += i.ToString() + ",";
            }
            WXCustomerService wxservice = new WXCustomerService();
            try
            {
                IEnumerable<WXPODBarCode> bc = wxservice.GetPodBarCode(podIds);
                podIds = "";
                if (bc.Count() > 0)
                {
                    long ticketID = 0;
                    string josn_content = string.Empty;
                    string ticketKey = string.Empty;
                    string sql = string.Empty;
                    foreach (WXPODBarCode pod in bc)
                    {
                        WXPODBarCode bc1 = wxservice.GetWXTicketID();
                        ticketID = bc1.TicketID + 1;
                        //josn_content = GetPage(ticketID);
                        //string[] test = josn_content.Split(',');
                        //test[0] = test[0].Replace("\"", "");
                        //ticketKey = test[0].Substring(8);

                        ticketKey = GetticketKey(ticketID);
                        string[] test = ticketKey.Split('|');

                        WXPODBarCode bc2 = new WXPODBarCode();
                        bc2.TicketID = Convert.ToInt64(test[1]);
                        bc2.PODID = pod.PODID;
                        bc2.ticketkey = test[0];
                        wxservice.AddWXPODBarCode(bc2);
                        podIds += pod.CustomerOrderNumber + ",";
                    }
                }
            }
            catch (Exception ex)
            {
            }

            if (string.IsNullOrEmpty(podIds))
            {
                podIds = "很抱歉，您选择的运单号都已经生成过二维码";
            }
            else
            {
                podIds += "运单号已成功生成二维码";
            }

            return new { msg = podIds }.ToJsonString();
        }


        /// <summary>
        /// 递归获取正确ticketKey
        /// </summary>
        /// <param name="ticketID"></param>
        /// <returns></returns>
        private string GetticketKey(long ticketID)
        {
            string josn_content = GetPage(ticketID);
            string[] test = josn_content.Split(',');
            test[0] = test[0].Replace("\"", "").Replace("\\", ""); ;
            string ticketKey = test[0].Substring(8);
            //if (ticketKey.Contains(@"\") || ticketKey.Contains("/"))
            //{
            //    ticketID += 1;
            //    ticketKey = GetticketKey(ticketID);
            //}

            return test[0].Substring(8) + "|" + ticketID.ToString();
        }

        /// <summary>
        /// POST提交
        ///Get获取图片地址 https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=TICKET
        /// </summary>
        private string GetPage(long ticketID)
        {
            string Token = IsExistAccess_Token();
            //"pcyfIHXV__I22r1LojWxZM8KCvy-me1GEEgTBTCM0TeZmn8i8QysPd13io_3ub8LvkWIje-nhWxC55KZ8R6Eo4WVSuJjUqBildV1t-zOwZU";
            //https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=TOKEN
            //https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}
            string posturl = string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", Token);
            //string postData = "{\"touser\": \"" + weixinName + "\",\"msgtype\": \"text\", \"text\": {\"content\": \"恭喜您注册已通过审核\"}}";
            string postData = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + ticketID + "}}}";
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }


        /// <summary>  
        /// 根据当前日期 判断Access_Token 是否超期  
        /// 如果超期返回新的Access_Token  否则返回之前的Access_Token  
        /// </summary>  
        public string IsExistAccess_Token()
        {
            string Token = string.Empty;
            WXCustomerService ws = new WXCustomerService();
            //查询已存在的记录
            WXAccessToken at = ws.GetQueryWXAccessToken();
            Token = at.Access_token;
            if (at.Expires_in <= DateTime.Now)
            {
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;

                var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}",
                                    "client_credential", "wx1a4fb84b6f156f44", "2b72be8ef2978b11a34fa9ecfadf313b");
                string josn_Token = wc.DownloadString(url);
                if (!josn_Token.Contains("errcode"))
                {
                    string[] test = josn_Token.Split(',');
                    test[0] = test[0].Replace("\"", "");
                    Token = test[0].Substring(14);
                }
                //修改WXAccessToken表
                WXAccessToken wx = new WXAccessToken();
                wx.Access_token = Token;
                wx.Expires_in = DateTime.Now.AddSeconds(7000);
                ws.UploadWXAccessToken(wx);
            }

            return Token;
        }


        [HttpPost]
        public JsonResult BackPodStatus(string ids, long targetStatusID)
        {
            var podIDs = ids.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);
            var targetStatusName = ApplicationConfigHelper.GetApplicationConfigs("PodState").First(p => p.ID == targetStatusID).Name;
            var response = new PodService().SetPodStatus(new SetPodStatusRequest() { IDs = podIDs, PodStatusID = targetStatusID, PodStatusName = targetStatusName, IsSendMessage = "false" });
            if (response.IsSuccess)
            {
                var IDs = podIDs.Select(p => p);
                return Json(new { StatusName = targetStatusName, ID = IDs });
            }
            throw response.Exception;
            //return View();
            //var response = new PodService().SetPodStatus(new SetPodStatusRequest() { IDs = podIDs, PodStatusID = podStateID, PodStatusName = podStateName });
            //if (response.IsSuccess)
            //{
            //    return Json(podIDs.Select(p => new { ID = p }));
            //}

            //throw response.Exception;

            //throw new NotImplementedException();
        }

        public ActionResult QueryPod(bool? hideActionButton, bool? showEditRelated, long? customerID)
        {

            QueryPodViewModel vm = new QueryPodViewModel();

            vm.SearchCondition = new PodSearchCondition();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.HideActionButton = hideActionButton ?? false;
            vm.ShowEditRelated = showEditRelated ?? true;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            vm.ProjectRoleID = base.UserInfo.ProjectRoleID;

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.ShipperID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }

            this.GenQueryPodViewModel(vm);
            return View(vm);
        }


        [HttpPost]// async Task<ActionResult>
        public ActionResult QueryPod(QueryPodViewModel vm, int? PageIndex, string Action)
        {
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.SearchCondition.RuleArea = base.UserInfo.RuleArea;
            vm.SearchCondition.UserID = base.UserInfo.ID.ToString();
            if (vm.SearchCondition.UserType == 2)
            {
                vm.SearchCondition.CustomerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            }
            else if (vm.SearchCondition.UserType == 0)
            {
                vm.SearchCondition.Types = new List<int>();
                vm.SearchCondition.Types.Add(0);
                vm.SearchCondition.Types.Add(2);
            }
            else
            {
                vm.SearchCondition.Types = new List<int>();
                vm.SearchCondition.Types.Add(1);
                vm.SearchCondition.Types.Add(2);
            }
            //下载附件,下载符合查询条件的附件

            if (Action == "导出全部异常")
            {
                var podAlls = new PodService().BaiXingTrackingReport(new QueryPodRequest()
                {
                    SearchCondition = vm.SearchCondition,
                    ProjectID = base.UserInfo.ProjectID,
                    PageIndex = PageIndex ?? 0,
                    PageSize = UtilConstants.PAGESIZE
                }).Result;
                //导出Adidas报表
                return this.ExportBaiXingPodExceptionToExcel(podAlls);
            }

            if (Action == "附件下载")
            {
                var request = new QueryPodRequest();
                request.SearchCondition = vm.SearchCondition;
                var podphotourl = new PodService().PodWithAttachment(request).Result;
                string str = this.ExportPodsToFile(podphotourl);
                this.ExportPodsZipFile(str);
            }


            var result = new PodService().QueryPod(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            vm.PodCollection = result.PodCollections;
            vm.PageIndex = result.PageIndex;
            vm.PageCount = result.PageCount;
            if (Action == "打印运单")
            {
                var idsData = from q in result.PodCollections
                              group q by new { } into g
                              let ids = g.Select(b => b.SystemNumber).ToArray()
                              select new { ids = string.Join(",", ids) };
                string str = "";
                foreach (var item in idsData)
                {
                    str = item.ids;

                }

                Session["QueryPodViewModel"] = str;
                return RedirectToAction("BSPrinPOD");
            }
            this.GenQueryPodViewModel(vm);

            if (vm.IsForExport)
            {
                long? customerID;
                if (vm.Customers.Count() == 1)
                {
                    customerID = vm.Customers.First().Value.ObjectToInt64();
                }
                else
                {
                    customerID = vm.SearchCondition.CustomerID;
                }
                //导出运单,导出当前页运单
                if (string.Equals(vm.ExportType, "Pod", StringComparison.OrdinalIgnoreCase))
                {
                    //导出Adidas专用
                    if (customerID == 1)
                    {
                        var podAlls = new PodService().AdidasTrackingReport(new QueryPodRequest()
                        {
                            SearchCondition = vm.SearchCondition,
                            ProjectID = base.UserInfo.ProjectID,
                            PageIndex = PageIndex ?? 0,
                            PageSize = UtilConstants.PAGESIZE
                        }).Result;
                        //导出Adidas报表
                        return this.ExportADPodsToExcel(podAlls);
                    }
                    //导出百姓网订单专用
                    if (customerID == 40)
                    {
                        var podAlls = new PodService().BaiXingTrackingReport(new QueryPodRequest()
                        {
                            SearchCondition = vm.SearchCondition,
                            ProjectID = base.UserInfo.ProjectID,
                            PageIndex = PageIndex ?? 0,
                            PageSize = UtilConstants.PAGESIZE
                        }).Result;
                        //导出Adidas报表
                        return this.ExportBaiXingPodsToExcel(podAlls);
                    }
                    //导出VF专用
                    if (customerID == 34)
                    {
                        var podAlls = new PodService().VFTrackingReport(new QueryPodRequest()
                        {
                            SearchCondition = vm.SearchCondition,
                            ProjectID = base.UserInfo.ProjectID,
                            PageIndex = PageIndex ?? 0,
                            PageSize = UtilConstants.PAGESIZE
                        }).Result;
                        //导出客服导出日报表
                        return this.ExportVFPodsToExcel(podAlls);
                    }
                    //导出艺康专用
                    if (customerID == 35)
                    {
                        var resultTable = new PodService().YKTrackingReport(new QueryPodRequest()
                        {
                            SearchCondition = vm.SearchCondition,
                            ProjectID = base.UserInfo.ProjectID,
                            PageIndex = PageIndex ?? 0,
                            PageSize = UtilConstants.PAGESIZE
                        });
                        //return this.ExportVFPodsToExcel(podAlls);
                        return this.ExportDataTableToExcel(resultTable, "ExportPods.xls");
                    }
                    //导出东方CJ专用
                    if (customerID == 15)
                    {
                        return this.ExportDfcjPodsToExcel(result.PodCollections);
                    }

                    return this.ExportPodsToExcel(result.PodCollections, vm.Config.ColumnCollection, customerID, 1);
                }
                //导出全部运单,导出符合查询条件的运单
                else if (string.Equals(vm.ExportType, "PodAll", StringComparison.OrdinalIgnoreCase))
                {
                    //导出VF专用
                    if (customerID == 34)
                    {
                        var podAlls = new PodService().VFTrackingReport(new QueryPodRequest()
                        {
                            SearchCondition = vm.SearchCondition,
                            ProjectID = base.UserInfo.ProjectID
                        }).Result;
                        //导出客服导出日报表
                        return this.ExportVFPodsToExcel(podAlls);

                    }
                    var podAll = new PodService().QueryPodWithNoPaging(new QueryPodRequest()
                    {
                        SearchCondition = vm.SearchCondition,
                        ProjectID = base.UserInfo.ProjectID
                    }).Result;

                    //导出东方CJ专用
                    if (customerID == 15)
                    {
                        return this.ExportDfcjPodsToExcel(podAll);
                    }
                    return this.ExportPodsToExcel(podAll, vm.Config.ColumnCollection, customerID, true);
                }
                //导出跟踪,导出当前页运单的跟踪信息
                else if (string.Equals(vm.ExportType, "PodTrack", StringComparison.OrdinalIgnoreCase))
                {
                    //导出VF专用
                    if (customerID == 34)
                    {
                        var podAll = new PodService().VFTrackingReport(new QueryPodRequest()
                        {
                            SearchCondition = vm.SearchCondition,
                            ProjectID = base.UserInfo.ProjectID
                        }).Result;
                        //导出客服导出日报表
                        //return this.ExportVFPodsToExcel(podAll);
                        //导出客服导出，供应商的填写后导入Customer
                        return this.ExportVFPodsToExcelCustomer(podAll);
                    }
                    //导出宝胜专用
                    if (customerID == 32)
                    {
                        var SuperPodAll = new PodService().BSTrackingReport(new QueryPodRequest()
                        {
                            SearchCondition = vm.SearchCondition,
                            ProjectID = base.UserInfo.ProjectID
                        }).Result;
                        //导出客服导出日报表
                        // return this.ExportVFPodsToExcel(podAll);
                        //导出客服导出，供应商的填写后导入Customer
                        return this.ExportBSPodsToExcelCustomer(SuperPodAll);
                    }
                    var columns = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodTrack")
                        .ColumnCollection.Where(c => (c.IsKey && c.ForView) || (!c.IsKey && !c.IsHide) || (!c.IsKey && c.IsHide && c.InnerColumns.Any(innerc => innerc.CustomerID == customerID)))
                        .Select(c =>
                        {
                            if (c.InnerColumns.Count == 0)
                            {
                                return c;
                            }
                            else
                            {
                                if (c.InnerColumns.Any(innerc => innerc.CustomerID == customerID))
                                {
                                    return c.InnerColumns.First(innerc => innerc.CustomerID == customerID);
                                }

                                return c;
                            }
                        });
                    return this.ExportPodTracksToExcel(result.PodCollections.Select(p => p.ID), columns);
                }
            }


            if (!string.IsNullOrEmpty(vm.SearchCondition.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (vm.SearchCondition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = vm.SearchCondition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }

                if (vm.SearchCondition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = vm.SearchCondition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers == null || !customerOrderNumbers.Any())
                {
                    customerOrderNumbers = new string[] { vm.SearchCondition.CustomerOrderNumber };
                }

                var notContainsCustomerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c) &&
                        !vm.PodCollection.Any(p => string.Equals(p.CustomerOrderNumber, c.Trim(), StringComparison.OrdinalIgnoreCase))
                    );

                if (notContainsCustomerOrderNumbers != null && notContainsCustomerOrderNumbers.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("您输入的客户运单号:");
                    notContainsCustomerOrderNumbers.Each((i, c) => { sb.Append(c).Append(","); });
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("在本页结果中没有出现,系统中不存在此客户运单号或者此运单号在其他页或者请变更其他查询条件.");
                    vm.ReturnClientMessage = sb.ToString();
                }
            }

            return View(vm);
            // });
        }
        //记录Log日志
        public void WriterLogFile(string fileInfotxt)
        {
            try
            {
                string path = "D:\\";
                //如果不存在，则创建目录
                if (!Directory.Exists(path + "Log/"))
                {
                    Directory.CreateDirectory(path + "Log/");
                }
                FileStream fs = new FileStream(path + "\\Log/log" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(fs);
                streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                streamWriter.WriteLine(DateTime.Now.ToString() + "     " + fileInfotxt);
                streamWriter.Flush();
                streamWriter.Close();
                fs.Close();
            }
            catch { }
        }

        //        private int m_currentPageIndex;
        //        //声明一个Stream对象的列表用来保存报表的输出数据
        //        //LocalReport对象的Render方法会将报表按页输出为多个Stream对象。
        //        private List<Stream> m_streams;
        //        //用来提供Stream对象的函数，用于LocalReport对象的Render方法的第三个参数。
        //        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        //        {
        //            Stream stream = new FileStream(Path.GetTempPath() + name + "." + fileNameExtension, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        //            //Stream stream = new FileStream(Path.GetTempFileName(), FileMode.Create,FileAccess.ReadWrite,FileShare.ReadWrite,8000,true);  
        //            m_streams.Add(stream);
        //            return stream;
        //            //如果需要将报表输出的数据保存为文件，请使用FileStream对象。
        //            //Stream stream = new MemoryStream();//MemoryStream
        //            //m_streams.Add(stream);
        //            //return stream;
        //        }

        //        private void Export(LocalReport report)
        //        {

        //            string deviceInfo =
        //             @"<DeviceInfo>
        //             <OutputFormat>EMF</OutputFormat>
        //             </DeviceInfo>";
        //            Warning[] warnings;
        //            m_streams = new List<Stream>();
        //            //将报表的内容按照deviceInfo指定的格式输出到CreateStream函数提供的Stream中。
        //            try
        //            {
        //                report.Render("Image", deviceInfo, CreateStream, out warnings);
        //            }
        //            catch (Exception ex)
        //            {
        //                Exception innerEx = ex.InnerException;//取内异常。因为内异常的信息才有用，才能排除问题。  
        //                while (innerEx != null)
        //                {
        //                    //  MessageBox.Show(innerEx.Message);
        //                    innerEx = innerEx.InnerException;
        //                }
        //            }
        //            foreach (Stream stream in m_streams)
        //                stream.Position = 0;
        //        }

        //        //打印方法
        //        private void Print()
        //        {
        //            m_currentPageIndex = 0;

        //            if (m_streams == null || m_streams.Count == 0)
        //            {
        //                return;
        //            }
        //            //声明PrintDocument对象用于数据的打印

        //            PrintDocument printDoc = new PrintDocument();
        //            //指定需要使用的打印机的名称，使用空字符串""来指定默认打印机
        //            //printDoc.PrinterSettings.PrinterName = "";
        //            //PaperSize ps = new PaperSize("A4", 2479, 3508);
        //            //printDoc.DefaultPageSettings.PaperSize = ps;
        //            //判断指定的打印机是否可用

        //            //printDoc.PrinterSettings.PrinterName= LocalPrinter.GetLocalPrinters();

        //            WriterLogFile(printDoc.PrinterSettings.PrinterName);
        //            if (!printDoc.PrinterSettings.IsValid)
        //            {
        //                // MessageBox.Show("Can't find printer");
        //                return;
        //            }
        //            //声明PrintDocument对象的PrintPage事件，具体的打印操作需要在这个事件中处理。
        //            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
        //            //执行打印操作，Print方法将触发PrintPage事件。
        //            printDoc.Print();

        //        }
        //        [HttpPost]
        //        public ActionResult Preview(string id)
        //        {
        //            QueryPodViewModel vm = new QueryPodViewModel();
        //            PodSearchCondition Condition = new PodSearchCondition();
        //            Condition.ID = Convert.ToInt64(id);
        //            vm.SearchCondition = Condition;
        //            var result = new PodService().QueryBSPod(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
        //            //var result = new PodService().QueryPod(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
        //            var key = result.PodCollections.Select(a => a.Url).First();
        //            LocalReport localReport = new LocalReport();
        //            localReport.ReportPath = Server.MapPath("~/Rpts/BSReport.rdlc");//加上报表的路径  
        //            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", result.PodCollections);

        //            //ftp:///
        //            //ReportParameter param = new ReportParameter("LogoUrl", " https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQHe8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL19YVnNVT2JsVEJVMVBSd3AtVm1FAAIEvLkYVgMEAAAAAA==");
        //            //localReport.SetParameters(new ReportParameter[] { param });
        //            ReportDataSource reportDataSource2 = new ReportDataSource("DataSet2", AddImageRow(key).Tables[0]);//result.PodCollections.Select(a => a.Url).First(
        //            localReport.EnableExternalImages = true;
        //            localReport.DataSources.Add(reportDataSource);
        //            localReport.DataSources.Add(reportDataSource2);

        //            string reportType = "PDF";
        //            string mimeType;
        //            string encoding;
        //            string fileNameExtension;
        //            string deviceInfo =
        //            "<DeviceInfo>" +
        //            "  <OutputFormat>PDF</OutputFormat>" +
        //            "  <PageWidth>9.5in</PageWidth>" +
        //            "  <PageHeight>11in</PageHeight>" +
        //            "  <MarginTop>0.5in</MarginTop>" +
        //            "  <MarginLeft>1in</MarginLeft>" +
        //            "  <MarginRight>1in</MarginRight>" +
        //            "  <MarginBottom>0.5in</MarginBottom>" +
        //            "</DeviceInfo>";
        //            Warning[] warnings;
        //            string[] streams;
        //            byte[] renderedBytes;

        //            //Render the report
        //            renderedBytes = localReport.Render(
        //                reportType,
        //                deviceInfo,
        //                out mimeType,
        //                out encoding,
        //                out fileNameExtension,
        //                out streams,
        //                out warnings);
        //            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);
        //            return File(renderedBytes, mimeType);

        //        }
        //获得图片
        //private DataSet AddImageRow(string filename)
        //{
        //    DataSetBS datas = new DataSetBS();
        //    DataTable tbl = datas.Tables[0];
        //    try
        //    {

        //        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + filename);//"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQHe8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL19YVnNVT2JsVEJVMVBSd3AtVm1FAAIEvLkYVgMEAAAAAA==");
        //        webRequest.Method = "GET";
        //        webRequest.KeepAlive = false;
        //        webRequest.AllowAutoRedirect = true;
        //        webRequest.ContentType = "application/x-www-form-urlencoded";
        //        webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
        //        //webRequest.CookieContainer = cookieContainer;
        //        HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
        //        Stream s = webResponse.GetResponseStream();
        //        List<byte> list = new List<byte>();
        //        while (true)
        //        {
        //            int data = s.ReadByte();
        //            if (data == -1)
        //                break;
        //            else
        //            {
        //                byte b = (byte)data;
        //                list.Add(b);
        //            }
        //        }
        //        byte[] br = list.ToArray();
        //        s.Close();
        //        DataRow row;
        //        //FileStream fs = new FileStream(filename, FileMode.Open); // 创建文件流
        //        //BinaryReader br = new BinaryReader(fs);    // 创建二进制读取器
        //        // 创建一个新的数据行
        //        row = tbl.NewRow();
        //        row[0] = br;
        //        //row[1] = br.ReadBytes((int)br.BaseStream.Length);
        //        // 将数据行添加到表中
        //        tbl.Rows.Add(row);

        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return datas;
        //}
        //private void PrintPage(object sender, PrintPageEventArgs ev)
        //{
        //    //Metafile对象用来保存EMF或WMF格式的图形，
        //    //我们在前面将报表的内容输出为EMF图形格式的数据流。
        //    Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
        //    //调整打印机区域的边距
        //    Rectangle adjustedRect = new Rectangle(
        //        ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
        //        ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
        //        ev.PageBounds.Width,
        //        ev.PageBounds.Height);

        //    //绘制一个白色背景的报告
        //    //ev.Graphics.FillRectangle(Brushes.White, adjustedRect);
        //    //获取报告内容
        //    //这里的Graphics对象实际指向了打印机
        //    ev.Graphics.DrawImage(pageImage, adjustedRect);
        //    //ev.Graphics.DrawImage(pageImage, ev.PageBounds);
        //    // 准备下一个页,已确定操作尚未结束
        //    m_currentPageIndex++;

        //    //设置是否需要继续打印
        //    ev.HasMorePages = (m_currentPageIndex < m_streams.Count);

        //}
        //将获取的图片存入文件夹中
        private string ExportPodsToFile(IEnumerable<Attachment> photoUrl)
        {
            string downLoadFiles = Runbow.TWS.Common.Constants.DownLoadFiles + DateTime.Now.ToString("yyyyMMddHHmmss") + "\\";  //存放图片的文件夹
            string downLoadFilesTEMP = Runbow.TWS.Common.Constants.DownLoadFilesTEMP + DateTime.Now.ToString("yyyy-MM-dd") + "\\"; //压缩文件完整路径
            string path2 = DateTime.Now.ToString("yyyyMMddHHmmss") + ".ZIP";
            if (!Directory.Exists(downLoadFiles))
            {
                Directory.CreateDirectory(downLoadFiles);
            }

            foreach (var item in photoUrl)
            {
                if (item.Url != null)
                {
                    string filename = Path.GetFileName(item.Url);
                    MyFile.Copy(item.Url, downLoadFiles + filename, true);
                }
            }
            //压缩
            bool a = ZipHelper.Zip(downLoadFiles, path2);
            //将压缩后的zip文件放入downLoadFilesTEMP文件夹中
            if (!Directory.Exists(downLoadFilesTEMP))
            {
                Directory.CreateDirectory(downLoadFilesTEMP);
            }
            MyFile.Copy(path2, downLoadFilesTEMP + path2, true);

            return downLoadFilesTEMP + path2;

        }
        private string VFCity(string city)
        {
            try
            {
                var c = ApplicationConfigHelper.GetRegions().Where(a => a.Name == city).First();
                var sr = ApplicationConfigHelper.GetParentRegionByChildRegion(c.ID, 2);
                return sr.Name;
            }
            catch (Exception)
            {

                return "";
            }

        }
        /// <summary>
        /// 宝胜跟踪运单导出
        /// </summary>
        /// <param name="PodAll"></param>
        /// <returns></returns>
        private ActionResult ExportBSPodsToExcelCustomer(SuperPodAll Pods)
        {
            #region 宝胜
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("BU", typeof(string));
            dtPod.Columns.Add("发货单号", typeof(string));
            dtPod.Columns.Add("发货时间", typeof(string));
            dtPod.Columns.Add("出发城市", typeof(string));
            dtPod.Columns.Add("目的城市", typeof(string));
            dtPod.Columns.Add("客户代码", typeof(string));
            dtPod.Columns.Add("客户名称", typeof(string));
            dtPod.Columns.Add("箱数", typeof(string));
            dtPod.Columns.Add("件数", typeof(string));
            dtPod.Columns.Add("在途时限", typeof(string));
            dtPod.Columns.Add("预计到达时间", typeof(string));
            dtPod.Columns.Add("状态", typeof(string));
            dtPod.Columns.Add("承运商", typeof(string));
            dtPod.Columns.Add("实际签收时间", typeof(string));
            dtPod.Columns.Add("托运单备注", typeof(string));
            dtPod.Columns.Add("跟踪备注", typeof(string));
            //新
            dtPod.Columns.Add("卸货地址", typeof(string));
            dtPod.Columns.Add("联系人", typeof(string));
            dtPod.Columns.Add("联系人方式", typeof(string));
            dtPod.Columns.Add("回单是否上传", typeof(string));

            Pods.PodCollections.OrderBy(p => p.ID).Each((i, p) =>
            {
                DataRow dr = dtPod.NewRow();
                dr["BU"] = p.Str23;
                dr["发货单号"] = p.CustomerOrderNumber;
                dr["发货时间"] = p.ActualDeliveryDate;
                dr["出发城市"] = p.StartCityName;
                dr["目的城市"] = p.EndCityName;
                dr["客户代码"] = p.Str1;
                dr["客户名称"] = p.Str3;
                dr["箱数"] = p.BoxNumber;
                dr["件数"] = p.GoodsNumber;
                dr["在途时限"] = p.Str5;
                dr["预计到达时间"] = p.DateTime6.DateTimeToString();
                #region 跟踪
                Pods.PodTracks.Where(a => a.PodID == p.ID).Each((j, r) =>
                 {
                     dr["状态"] = r.Str1;
                     dr["承运商"] = p.ShipperName;
                     dr["实际签收时间"] = r.DateTime2.DateTimeToString();
                     dr["托运单备注"] = r.Str2;
                     dr["跟踪备注"] = r.Str10;
                 });

                #endregion
                dr["卸货地址"] = p.Str7;
                dr["联系人"] = p.Str4;
                dr["联系人方式"] = p.Str6;
                dr["回单是否上传"] = p.IsUploadPod == 1 ? "Y" : "N";
                dtPod.Rows.Add(dr);
            });
            #endregion
            ExportDataToExcelHelper.ExportDataSetToExcel(dtPod, "ExportPods", "");
            return new EmptyResult();
        }
        private ActionResult ExportVFPodsToExcelCustomer(PodAll Pods)
        {
            #region VF
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("客户运单号", typeof(string));
            dtPod.Columns.Add("3PL单号", typeof(string));
            dtPod.Columns.Add("客户下单日", typeof(string));
            dtPod.Columns.Add("订单类型", typeof(string));
            dtPod.Columns.Add("始发地", typeof(string));
            dtPod.Columns.Add("目的地省份", typeof(string));
            dtPod.Columns.Add("目的地城市", typeof(string));
            dtPod.Columns.Add("发货人编码", typeof(string));
            dtPod.Columns.Add("发货人名称", typeof(string));
            dtPod.Columns.Add("收货人编码", typeof(string));
            dtPod.Columns.Add("收货人名称", typeof(string));
            dtPod.Columns.Add("目的地地址", typeof(string));
            dtPod.Columns.Add("联系人", typeof(string));
            dtPod.Columns.Add("联系电话", typeof(string));
            dtPod.Columns.Add("运输方式", typeof(string));
            dtPod.Columns.Add("BRAND(品牌)", typeof(string));
            //新
            dtPod.Columns.Add("货物品名", typeof(string));
            dtPod.Columns.Add("总箱数", typeof(string));
            dtPod.Columns.Add("总件数", typeof(string));
            dtPod.Columns.Add("总体积(CBM)", typeof(string));
            //dtPod.Columns.Add("实际到达VF仓库时间", typeof(string));
            //dtPod.Columns.Add("交接完成时间", typeof(string));
            dtPod.Columns.Add("实际出发日", typeof(string));
            //新
            dtPod.Columns.Add("时效", typeof(string));
            dtPod.Columns.Add("理论到货日", typeof(string));
            //新
            dtPod.Columns.Add("订单备注预报", typeof(string));
            //dtPod.Columns.Add("实际到货日", typeof(string));

            dtPod.Columns.Add("POD返回3PL备注", typeof(string));
            //dtPod.Columns.Add("是否预约", typeof(string));
            dtPod.Columns.Add("第一天", typeof(string));
            dtPod.Columns.Add("第二天", typeof(string));
            dtPod.Columns.Add("第三天", typeof(string));
            dtPod.Columns.Add("第四天", typeof(string));
            dtPod.Columns.Add("第五天", typeof(string));
            dtPod.Columns.Add("第六天", typeof(string));
            dtPod.Columns.Add("第七天", typeof(string));
            dtPod.Columns.Add("第八天", typeof(string));
            dtPod.Columns.Add("POD理论返回日期", typeof(string));
            dtPod.Columns.Add("跟踪日期", typeof(string));
            dtPod.Columns.Add("在途位置", typeof(string));
            dtPod.Columns.Add("订单状态", typeof(string));
            dtPod.Columns.Add("预约是否成功", typeof(string));
            dtPod.Columns.Add("预约失败原因", typeof(string));
            dtPod.Columns.Add("按时到货", typeof(string));
            dtPod.Columns.Add("实际到货日", typeof(string));
            dtPod.Columns.Add("签收方", typeof(string));
            dtPod.Columns.Add("迟到责任归属", typeof(string));
            dtPod.Columns.Add("迟到原因", typeof(string));
            dtPod.Columns.Add("迟到原因描述", typeof(string));
            dtPod.Columns.Add("意外事件备注", typeof(string));
            dtPod.Columns.Add("POD按时返回", typeof(string));
            dtPod.Columns.Add("POD实际返回日期", typeof(string));
            dtPod.Columns.Add("使用客户运单号", typeof(string));

            Pods.PodPod.OrderBy(p => p.ID).Each((i, p) =>
            {
                DataRow dr = dtPod.NewRow();
                dr["客户运单号"] = p.CustomerOrderNumber;
                dr["3PL单号"] = p.SystemNumber;
                dr["客户下单日"] = p.DateTime2.DateTimeToString();
                dr["订单类型"] = p.PODTypeName;
                dr["始发地"] = p.StartCityName;
                dr["目的地省份"] = VFCity(p.EndCityName);
                dr["目的地城市"] = p.EndCityName;
                dr["发货人编码"] = "W100";
                dr["发货人名称"] = "昆山CDC";
                dr["收货人编码"] = p.Str7;
                dr["收货人名称"] = p.Str9;
                dr["目的地地址"] = p.Str10;
                dr["联系人"] = p.Str11;
                dr["联系电话"] = p.Str12;
                dr["运输方式"] = p.TtlOrTplName;
                dr["BRAND(品牌)"] = p.Str5;
                //新
                dr["货物品名"] = "服装";// CommodityName(p.Str5);
                dr["总箱数"] = "";
                dr["总箱数"] = p.BoxNumber;
                dr["总件数"] = p.GoodsNumber;
                dr["总体积(CBM)"] = SumVolume(p.BoxNumber, p.Str5, p.ActualDeliveryDate); // p.Volume; 
                //dr["实际到达VF仓库时间"] = p.DateTime3;
                //dr["交接完成时间"] = p.DateTime5;
                dr["实际出发日"] = p.DateTime5.DateTimeToString();
                //时效
                dr["时效"] = p.Str14;
                var DateTime6 = p.DateTime5 != null ? Convert.ToDateTime(p.DateTime5).AddDays(Convert.ToInt32(p.Str14 == "" ? 0 : Convert.ToInt32(p.Str14))).DateTimeToString() : "";
                dr["理论到货日"] = DateTime6;//p.DateTime5 != null ? Convert.ToDateTime(p.DateTime5).AddDays(Convert.ToInt32(p.Str14 == "" ? 0 : Convert.ToInt32(p.Str14))).DateTimeToString() : ""; //p.DateTime6.DateTimeToString();
                //新
                dr["订单备注预报"] = p.Str36;


                List<string> Date = new List<string>();
                var PodTracks = from q in Pods.PodTracks
                                where (q.PodID == p.ID & !string.IsNullOrEmpty(q.DateTime1.ToString()))
                                group q by new { q.PodID } into g
                                let Str1 = g.Select(b => (b.DateTime1.DateTimeToString() + "," + b.Str1.ToString()).ToString()).ToArray()
                                //   let DateTime1 = g.Select(b => b.DateTime1.ToString()).ToArray()
                                select new
                                {
                                    //  PodID = g.Key,
                                    Str1 = Str1

                                    //Str1 = g.Max(a => a.Str1),
                                    //DateTime1 = g.Max(a => a.DateTime1)

                                }.Str1.ToList();
                foreach (var item in PodTracks)
                {
                    foreach (var DateList in item)
                    {
                        Date.Add(DateList);
                    }
                }
                //var PDateTime1 =from q in  Pods.PodTracks
                //                where q.PodID==p.ID
                //                orderby q.DateTime1  
                //                select new{
                //                 q.DateTime1
                //                };
                //var PDateTime1 = Pods.PodTracks.OrderBy(a => a.DateTime1).Where(a => a.PodID == p.ID).Select(a => a.DateTime1).LastOrDefault();
                //-p.ActualDeliveryDate
                //for (int y = 0; y < (Convert.ToDateTime(PDateTime1) - Convert.ToDateTime(p.ActualDeliveryDate)).Days; y++)
                //{
                //    foreach (var item in PodTracks)
                //    {
                //        foreach (string P1 in item)
                //        {
                //            if (Convert.ToDateTime(p.ActualDeliveryDate).AddDays(y + 1) ==  Convert.ToDateTime(P1.Split(',')[0]))
                //            {

                //            }
                //        }

                //    }
                //}
                dr["第一天"] = Date.Where(a => Convert.ToDateTime(a.Split(',')[0]) == Convert.ToDateTime(p.ActualDeliveryDate).AddDays(1)).Select(a => a).FirstOrDefault();
                dr["第二天"] = Date.Where(a => Convert.ToDateTime(a.Split(',')[0]) == Convert.ToDateTime(p.ActualDeliveryDate).AddDays(2)).Select(a => a).FirstOrDefault();
                dr["第三天"] = Date.Where(a => Convert.ToDateTime(a.Split(',')[0]) == Convert.ToDateTime(p.ActualDeliveryDate).AddDays(3)).Select(a => a).FirstOrDefault();
                dr["第四天"] = Date.Where(a => Convert.ToDateTime(a.Split(',')[0]) == Convert.ToDateTime(p.ActualDeliveryDate).AddDays(4)).Select(a => a).FirstOrDefault();
                dr["第五天"] = Date.Where(a => Convert.ToDateTime(a.Split(',')[0]) == Convert.ToDateTime(p.ActualDeliveryDate).AddDays(5)).Select(a => a).FirstOrDefault();
                dr["第六天"] = Date.Where(a => Convert.ToDateTime(a.Split(',')[0]) == Convert.ToDateTime(p.ActualDeliveryDate).AddDays(6)).Select(a => a).FirstOrDefault();
                dr["第七天"] = Date.Where(a => Convert.ToDateTime(a.Split(',')[0]) == Convert.ToDateTime(p.ActualDeliveryDate).AddDays(7)).Select(a => a).FirstOrDefault();
                dr["第八天"] = Date.Where(a => Convert.ToDateTime(a.Split(',')[0]) == Convert.ToDateTime(p.ActualDeliveryDate).AddDays(8)).Select(a => a).FirstOrDefault();
                //dr["第一天"] = Date.Count() >= 1 ? Date[0] == "" ? "" : Date[0].ToString() : "";
                //dr["第二天"] = Date.Count() >= 2 ? Date[1] == "" ? "" : Date[1].ToString() : "";
                //dr["第三天"] = Date.Count() >= 3 ? Date[2] == "" ? "" : Date[2].ToString() : "";
                //dr["第四天"] = Date.Count() >= 4 ? Date[3] == "" ? "" : Date[3].ToString() : "";
                //dr["第五天"] = Date.Count() >= 5 ? Date[4] == "" ? "" : Date[4].ToString() : "";
                //dr["第六天"] = Date.Count() >= 6 ? Date[5] == "" ? "" : Date[5].ToString() : "";
                //dr["第七天"] = Date.Count() >= 7 ? Date[6] == "" ? "" : Date[6].ToString() : "";
                //dr["第八天"] = Date.Count() >= 8 ? Date[7] == "" ? "" : Date[7].ToString() : "";
                if (Pods.PodTracks.Count() > 0)
                {
                    //var PodTracksdata = Pods.PodTracks.Where(a => a.PodID == p.ID).OrderByDescending(a => a.ID).First();
                    var PodTracksdata = from q in Pods.PodTracks
                                        where q.PodID == p.ID
                                        orderby q.ID descending
                                        select new
                                        {
                                            q.ID,
                                            q.CustomerOrderNumber,
                                            q.PodID,
                                            q.Str1,
                                            q.Str2,
                                            q.Str3,
                                            q.Str4,
                                            q.Str5,
                                            q.Str6,
                                            q.Str7,
                                            q.Str8,
                                            q.Str9,
                                            q.Str10,
                                            q.Str11,
                                            q.Str12,
                                            q.Str13,
                                            q.Str14,
                                            q.Str15,
                                            q.DateTime1,
                                            q.DateTime2,
                                            q.DateTime3,
                                            q.DateTime4,
                                            q.DateTime5,
                                            q.DateTime6,
                                            q.DateTime7,
                                            q.DateTime8,
                                            q.DateTime9,
                                            q.DateTime10,
                                        };
                    bool PodTracksdataCount = PodTracksdata.ToList().Count > 0;
                    dr["签收方"] = PodTracksdataCount ? PodTracksdata.FirstOrDefault().Str13 : "";
                    dr["POD返回3PL备注"] = PodTracksdataCount ? PodTracksdata.First().Str4 : "";
                    //var data= PodTracks.Where(b=>b.PodID==p.ID);
                    dr["POD理论返回日期"] = PODArrivalGoods(p.DateTime5.ToString()); //PodTracksdataCount ? (PodTracksdata.First().DateTime2 != null ? Convert.ToDateTime(PodTracksdata.First().DateTime2).AddDays(Convert.ToInt32(p.Str15 == "" ? null : p.Str15)).DateTimeToString() : "") : "";
                    dr["跟踪日期"] = "";//PodTracksdataCount ? PodTracksdata.First().DateTime1.ToString() : "";
                    dr["在途位置"] = "";// PodTracksdataCount ? PodTracksdata.First().Str1 : "";
                    dr["订单状态"] = PodTracksdataCount ? PodTracksdata.First().Str2 : "";//p.PODStateName; //
                    dr["预约是否成功"] = PodTracksdataCount ? PodTracksdata.First().Str3 : "";
                    dr["预约失败原因"] = PodTracksdataCount ? PodTracksdata.First().Str4 : "";
                    var DateTime2 = PodTracksdataCount ? PodTracksdata.Max(a => a.DateTime2).DateTimeToString() : "";
                    try
                    {
                        dr["按时到货"] = string.IsNullOrEmpty(DateTime2) ? (DateTime.Now.Date > Convert.ToDateTime(DateTime6) ? "N" : "") : Convert.ToDateTime(DateTime2) > Convert.ToDateTime(DateTime6) ? "N" : "Y"; //PodTracksdataCount ? PodTracksdata.First().Str6 : "";
                    }
                    catch (Exception e)
                    {

                        dr["按时到货"] = "";
                    }

                    dr["实际到货日"] = DateTime2;// PodTracksdataCount ? PodTracksdata.First().DateTime2.DateTimeToString() : "";
                    dr["迟到责任归属"] = PodTracksdataCount ? PodTracksdata.First().Str7 : "";
                    dr["迟到原因"] = PodTracksdataCount ? PodTracksdata.First().Str8 : "";
                    dr["迟到原因描述"] = PodTracksdataCount ? PodTracksdata.First().Str9 : "";
                    dr["意外事件备注"] = PodTracksdataCount ? PodTracksdata.First().Str10 : "";
                    dr["POD按时返回"] = PodTracksdataCount ? PodTracksdata.First().Str5 : "";
                    dr["POD实际返回日期"] = PodTracksdataCount ? PodTracksdata.First().DateTime3.DateTimeToString() : "";
                    dr["使用客户运单号"] = 1;
                }
                dtPod.Rows.Add(dr);
            });
            #endregion

            // return this.ExportDataTableToExcel(dtPod, "ExportPods.xls");
            ExportDataToExcelHelper.ExportDataSetToExcel(dtPod, "ExportPods", "");
            return new EmptyResult();
        }
        /// <summary>
        /// 计算VF运单体积
        /// </summary>
        /// <returns></returns>
        private string SumVolume(double? BoxNumber, string str, DateTime? date)
        {
            string Sum = "";
            try
            {
                if (date.Value >= DateTime.Parse("2016-08-01"))
                {
                    switch (str)
                    {
                        case "TNF"://0.086
                            Sum = (BoxNumber * 0.0808).ToString();
                            break;
                        case "VANS"://0.083
                            Sum = (BoxNumber * 0.0804).ToString();
                            break;
                        case "TBL"://0.09
                            Sum = (BoxNumber * 0.0715).ToString();
                            break;
                        case "KIPL"://0.095
                            Sum = (BoxNumber * 0.0655).ToString();
                            break;
                        case "LEE"://0.072
                            Sum = (BoxNumber * 0.0605).ToString();
                            break;

                        default:
                            Sum = "";
                            break;
                    }
                }
                else
                {
                    switch (str)
                    {
                        case "TNF"://0.086
                            Sum = (BoxNumber * 0.086).ToString();
                            break;
                        case "VANS"://0.083
                            Sum = (BoxNumber * 0.083).ToString();
                            break;
                        case "TBL"://0.09
                            Sum = (BoxNumber * 0.09).ToString();
                            break;
                        case "KIPL"://0.095
                            Sum = (BoxNumber * 0.095).ToString();
                            break;
                        case "LEE"://0.072
                            Sum = (BoxNumber * 0.072).ToString();
                            break;

                        default:
                            Sum = "";
                            break;
                    }
                }



                //switch (date)
                //{
                //    case date.Value > "2016-08-01"://0.086
                //        switch (str)
                //        {
                //            case "TNF"://0.086
                //                Sum = (BoxNumber * 0.0808).ToString();
                //                break;
                //            case "VANS"://0.083
                //                Sum = (BoxNumber * 0.0804).ToString();
                //                break;
                //            case "TBL"://0.09
                //                Sum = (BoxNumber * 0.0715).ToString();
                //                break;
                //            case "KIPL"://0.095
                //                Sum = (BoxNumber * 0.0655).ToString();
                //                break;
                //            case "LEE"://0.072
                //                Sum = (BoxNumber * 0.0605).ToString();
                //                break;

                //            default:
                //                Sum = "";
                //                break;
                //        }
                //    default:
                //        break;
                //}

            }
            catch (Exception)
            {

                return Sum;
            }
            return Sum;
        }

        private ActionResult ExportVFPodsToExcel(PodAll Pods)
        {
            #region VF
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("客户下单日", typeof(string));
            dtPod.Columns.Add("3PL单号", typeof(string));
            dtPod.Columns.Add("客户订单号", typeof(string));
            dtPod.Columns.Add("订单类型", typeof(string));
            dtPod.Columns.Add("始发地", typeof(string));
            dtPod.Columns.Add("目的地省份", typeof(string));
            dtPod.Columns.Add("目的地城市", typeof(string));
            dtPod.Columns.Add("发货人编码", typeof(string));
            dtPod.Columns.Add("发货人名称", typeof(string));
            dtPod.Columns.Add("收货人编码", typeof(string));
            dtPod.Columns.Add("收货人名称", typeof(string));
            dtPod.Columns.Add("目的地地址", typeof(string));
            dtPod.Columns.Add("联系人", typeof(string));
            dtPod.Columns.Add("联系电话", typeof(string));
            dtPod.Columns.Add("运输方式", typeof(string));
            dtPod.Columns.Add("BRAND(品牌)", typeof(string));
            dtPod.Columns.Add("货物品名", typeof(string));
            dtPod.Columns.Add("总箱数", typeof(string));
            dtPod.Columns.Add("总件数", typeof(string));
            dtPod.Columns.Add("总体积(CBM)", typeof(string));
            dtPod.Columns.Add("实际到达VF仓库时间", typeof(string));
            dtPod.Columns.Add("交接完成时间", typeof(string));
            dtPod.Columns.Add("实际出发日", typeof(string));
            dtPod.Columns.Add("理论到货日", typeof(string));
            dtPod.Columns.Add("实际到货日", typeof(string));
            dtPod.Columns.Add("签收方", typeof(string));
            dtPod.Columns.Add("Standard LT", typeof(string));
            dtPod.Columns.Add("按时到货", typeof(string));
            dtPod.Columns.Add("迟到责任归属", typeof(string));
            dtPod.Columns.Add("迟到原因", typeof(string));
            dtPod.Columns.Add("迟到原因描述", typeof(string));
            dtPod.Columns.Add("意外事件备注", typeof(string));
            dtPod.Columns.Add("POD理论返回日期", typeof(string));
            dtPod.Columns.Add("POD实际返回日期", typeof(string));
            dtPod.Columns.Add("POD按时返回", typeof(string));
            dtPod.Columns.Add("POD返回3PL备注", typeof(string));
            dtPod.Columns.Add("是否预约", typeof(string));
            dtPod.Columns.Add("订单备注预报", typeof(string));
            dtPod.Columns.Add("预约是否成功", typeof(string));
            dtPod.Columns.Add("预约失败原因", typeof(string));
            Pods.PodPod.OrderBy(p => p.ID).Each((i, p) =>
           {
               DataRow dr = dtPod.NewRow();
               dr["客户下单日"] = p.DateTime2.DateTimeToString();
               dr["3PL单号"] = p.SystemNumber;
               dr["客户订单号"] = p.CustomerOrderNumber;
               dr["订单类型"] = p.PODTypeName;
               dr["始发地"] = p.StartCityName;
               dr["目的地省份"] = VFCity(p.EndCityName);
               dr["目的地城市"] = p.EndCityName;
               dr["发货人编码"] = "W100";
               dr["发货人名称"] = "昆山CDC";
               dr["收货人编码"] = p.Str7;
               dr["收货人名称"] = p.Str9;
               dr["目的地地址"] = p.Str10;
               dr["联系人"] = p.Str11;
               dr["联系电话"] = p.Str12;
               dr["运输方式"] = p.TtlOrTplName;
               dr["BRAND(品牌)"] = p.Str5;
               dr["货物品名"] = "服装";
               dr["总箱数"] = p.BoxNumber;
               dr["总件数"] = p.GoodsNumber;
               dr["总体积(CBM)"] = SumVolume(p.BoxNumber, p.Str5, p.ActualDeliveryDate); // p.Volume;
               dr["实际到达VF仓库时间"] = p.DateTime3;
               dr["交接完成时间"] = p.DateTime5;
               dr["实际出发日"] = p.DateTime5.DateTimeToString();
               var DateTime2 = p.DateTime5 != null ? Convert.ToDateTime(p.DateTime5).AddDays(Convert.ToInt32(p.Str14 == "" ? 0 : Convert.ToInt32(p.Str14))).DateTimeToString() : "";
               dr["理论到货日"] = DateTime2;//p.DateTime5 != null ? Convert.ToDateTime(p.DateTime5).AddDays(Convert.ToInt32(p.Str14 == "" ? 0 : Convert.ToInt32(p.Str14))).DateTimeToString() : ""; //p.DateTime6.DateTimeToString();

               dr["Standard LT"] = p.Str14;
               dr["POD理论返回日期"] = PODArrivalGoods(p.DateTime5.ToString());

               Pods.PodTracks.Where(a => a.PodID == p.ID).Each((j, r) =>
               {
                   //PodTracksdataCount ? PodTracksdata.Max(a => a.DateTime2).DateTimeToString() : "";
                   var DateTime6 = Pods.PodTracks.Where(a => a.PodID == p.ID).Max(a => a.DateTime2).DateTimeToString();
                   dr["实际到货日"] = DateTime6;//p.DateTime2.DateTimeToString();
                   if (string.IsNullOrEmpty(DateTime2))
                   {
                       dr["按时到货"] = "N";
                   }
                   else
                   {
                       dr["按时到货"] = string.IsNullOrEmpty(DateTime6) ? (DateTime.Now.Date > Convert.ToDateTime(DateTime2) ? "N" : "") : Convert.ToDateTime(DateTime6) > Convert.ToDateTime(DateTime2) ? "N" : "Y"; //r.Str6; // DateTime6 == "" ? "" : Convert.ToDateTime(DateTime2) < Convert.ToDateTime(DateTime6) ? "N" : "Y";
                   }
                   dr["迟到责任归属"] = r.Str7;
                   dr["签收方"] = r.Str13;
                   dr["迟到原因"] = r.Str8;
                   dr["迟到原因描述"] = r.Str9;
                   dr["意外事件备注"] = r.Str10;
                   dr["POD实际返回日期"] = r.DateTime3.DateTimeToString();
                   dr["POD按时返回"] = r.Str11;
                   dr["POD返回3PL备注"] = "";
                   dr["是否预约"] = p.Str21;
                   dr["订单备注预报"] = p.Str36;
                   dr["预约是否成功"] = r.Str3;
                   dr["预约失败原因"] = r.Str4;
               });
               dtPod.Rows.Add(dr);
           });
            #endregion
            ExportDataToExcelHelper.ExportDataSetToExcel(dtPod, "ExportPods", "");
            return new EmptyResult();
        }
        private ActionResult ExportADPodsToExcel(PodAll Pods)
        {
            #region Adidas
            #region 表头
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("Rbow", typeof(string));
            dtPod.Columns.Add("3PL单号", typeof(string));
            dtPod.Columns.Add("提货日期", typeof(string));
            dtPod.Columns.Add("送货单号", typeof(string));
            dtPod.Columns.Add("客户编码", typeof(string));
            dtPod.Columns.Add("客户名称", typeof(string));
            dtPod.Columns.Add("联系方式", typeof(string));
            dtPod.Columns.Add("送货省份", typeof(string));
            dtPod.Columns.Add("送货城市", typeof(string));
            dtPod.Columns.Add("送货箱数", typeof(string));
            dtPod.Columns.Add("立方数", typeof(string));
            dtPod.Columns.Add("运费单价", typeof(string));
            dtPod.Columns.Add("派点费", typeof(string));
            dtPod.Columns.Add("运费", typeof(string));
            dtPod.Columns.Add("其他费用", typeof(string));
            dtPod.Columns.Add("燃油补贴", typeof(string));
            dtPod.Columns.Add("总费用", typeof(string));
            //dtPod.Columns.Add("支付方式", typeof(string));
            dtPod.Columns.Add("单据类型", typeof(string));
            dtPod.Columns.Add("运输方式", typeof(string));
            dtPod.Columns.Add("零担/整车", typeof(string));
            dtPod.Columns.Add("正常/加急", typeof(string));
            dtPod.Columns.Add("出货类型/SP始发地", typeof(string));
            dtPod.Columns.Add("到货期限", typeof(string));
            dtPod.Columns.Add("应到日期", typeof(string));
            dtPod.Columns.Add("实际到达日期", typeof(string));
            dtPod.Columns.Add("备注", typeof(string));
            dtPod.Columns.Add("供应商", typeof(string));
            dtPod.Columns.Add("迟到记录", typeof(string));
            dtPod.Columns.Add("签收情况", typeof(string));
            dtPod.Columns.Add("回单应回AD日期", typeof(string));
            dtPod.Columns.Add("回单应回日期", typeof(string));
            dtPod.Columns.Add("回否(Y/N)", typeof(string));
            dtPod.Columns.Add("实际收单日期", typeof(string));
            dtPod.Columns.Add("Original", typeof(string));
            dtPod.Columns.Add("交单日期", typeof(string));
            dtPod.Columns.Add("签收人", typeof(string));
            dtPod.Columns.Add("发货通知", typeof(string));
            dtPod.Columns.Add("是否有附件", typeof(string));
            dtPod.Columns.Add("电话回访", typeof(string));
            dtPod.Columns.Add("录单员", typeof(string));
            dtPod.Columns.Add("回访员", typeof(string));
            dtPod.Columns.Add("回单类别", typeof(string));
            #endregion

            Pods.PodPod.OrderBy(p => p.ID).Each((i, p) =>
            {
                DataRow dr = dtPod.NewRow();
                dr["Rbow"] = "Rbow";
                dr["3PL单号"] = p.SystemNumber;
                dr["提货日期"] = p.ActualDeliveryDate;
                dr["送货单号"] = p.CustomerOrderNumber;
                dr["客户编码"] = p.Str5;
                dr["客户名称"] = p.Str6;
                dr["联系方式"] = p.EndCityName + "/" + p.Str8;
                dr["送货省份"] = p.Str20;
                dr["送货城市"] = p.EndCityName;
                dr["送货箱数"] = p.BoxNumber.ToString();
                dr["立方数"] = p.Volume.ToString();
                dr["运费单价"] = "";
                dr["派点费"] = "";
                dr["运费"] = "";
                dr["其他费用"] = "";
                dr["燃油补贴"] = "";
                dr["总费用"] = "";

                dr["运输方式"] = p.ShipperTypeName;
                dr["零担/整车"] = p.TtlOrTplName;
                dr["正常/加急"] = p.Str15 == "0" ? "Normal" : "加急";
                dr["出货类型/SP始发地"] = p.ShipperTypeName + "/" + p.StartCityName;
                dr["到货期限"] = p.Str17;
                dr["回单应回AD日期"] = p.DateTime6;
                dr["回单应回日期"] = p.DateTime7;
                if (!string.IsNullOrEmpty(p.Str17) && p.ActualDeliveryDate != null)
                {
                    DateTime delivery = Convert.ToDateTime(p.ActualDeliveryDate);
                    dr["应到日期"] = delivery.AddDays(double.Parse(p.Str17));
                }
                else
                {
                    dr["应到日期"] = "";
                }

                dr["供应商"] = p.ShipperName;

                //跟踪信息表
                //Pods.PodTracks.Where(a => a.PodID == p.ID).Each((j, r) =>
                //{
                //    dr["实际到达日期"] = r.DateTime2;
                //    dr["备注"] = r.Str1;
                //});

                //dr["支付方式"] = p.Str20;
                dr["单据类型"] = p.Str22;
                dr["录单员"] = p.Str19;
                dr["回单类别"] = p.Str21;

                //回单信息表
                Pods.PodReplyDocuments.Where(a => a.PodID == p.ID).Each((j, r) =>
                {
                    dr["签收情况"] = r.Remark;

                    dr["回否(Y/N)"] = r.Str3;
                    dr["实际收单日期"] = r.Str4;
                    dr["Original"] = r.Str2;
                    dr["交单日期"] = r.ReplyTime.ToString();
                    dr["签收人"] = r.Replier;
                    dr["发货通知"] = "";
                    dr["是否有附件"] = r.Str5 == "0" ? "N" : "Y";
                });

                //回访信息表
                Pods.PodFeadBacks.Where(a => a.PodID == p.ID).Each((j, r) =>
                {
                    dr["电话回访"] = r.Str7;
                    dr["回访员"] = r.Str8;
                    dr["迟到记录"] = r.Str9;
                    dr["实际到达日期"] = r.DateTime2;
                    dr["备注"] = r.Str10;
                });

                dtPod.Rows.Add(dr);
            });

            #endregion
            ExportDataToExcelHelper.ExportDataSetToExcel(dtPod, "AdidasExportPods", "");
            return new EmptyResult();
        }

        /// <summary>
        /// 百姓网导出报表
        /// </summary>
        private ActionResult ExportBaiXingPodsToExcel(PodAll Pods)
        {
            #region 表头
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("委托方客户卡号*", typeof(string));
            dtPod.Columns.Add("取件方联系人*", typeof(string));
            dtPod.Columns.Add("取件方联系电话*", typeof(string));
            dtPod.Columns.Add("取件方联系地址*", typeof(string));
            dtPod.Columns.Add("取件方城市代码*", typeof(string));
            dtPod.Columns.Add("到件方联系人*", typeof(string));
            dtPod.Columns.Add("到件方联系电话*", typeof(string));
            dtPod.Columns.Add("到件方联系地址*", typeof(string));
            dtPod.Columns.Add("委托方联系人*", typeof(string));
            dtPod.Columns.Add("委托方手机*", typeof(string));
            dtPod.Columns.Add("委托方联系地址*", typeof(string));
            dtPod.Columns.Add("委托方城市代码*", typeof(string));
            dtPod.Columns.Add("寄件类型*", typeof(string));
            dtPod.Columns.Add("物品名称*", typeof(string));
            dtPod.Columns.Add("付款方式*", typeof(string));
            dtPod.Columns.Add("订单备注", typeof(string));
            dtPod.Columns.Add("短信备注", typeof(string));
            dtPod.Columns.Add("付款方月结卡号", typeof(string));
            dtPod.Columns.Add("付款方公司名称", typeof(string));
            dtPod.Columns.Add("付款方联系人", typeof(string));
            dtPod.Columns.Add("付款方联系电话", typeof(string));
            dtPod.Columns.Add("取件方公司名称", typeof(string));
            dtPod.Columns.Add("取件方客户卡号", typeof(string));
            dtPod.Columns.Add("到件方公司名称", typeof(string));
            dtPod.Columns.Add("到件方客户卡号", typeof(string));
            dtPod.Columns.Add("到件方税号", typeof(string));
            dtPod.Columns.Add("委托方公司名称", typeof(string));
            dtPod.Columns.Add("委托方座机", typeof(string));
            dtPod.Columns.Add("委托方传真", typeof(string));
            dtPod.Columns.Add("委托方E-mail", typeof(string));
            dtPod.Columns.Add("地址是否保密", typeof(string));
            dtPod.Columns.Add("是否保价", typeof(string));
            dtPod.Columns.Add("预约取件时间", typeof(string));
            dtPod.Columns.Add("声明/申报价值", typeof(string));
            dtPod.Columns.Add("币种", typeof(string));
            dtPod.Columns.Add("数量（件）", typeof(string));
            dtPod.Columns.Add("重量（KG）", typeof(string));
            dtPod.Columns.Add("货物规格长（cm）", typeof(string));
            dtPod.Columns.Add("货物规格宽（cm）", typeof(string));
            dtPod.Columns.Add("货物规格高（cm）", typeof(string));

            #endregion

            #region 赋值
            Pods.PodPod.OrderBy(p => p.ID).Each((i, p) =>
            {
                DataRow dr = dtPod.NewRow();
                dr["委托方客户卡号*"] = "70001201606220001"; //月结账号(顺丰提供)
                dr["取件方联系人*"] = p.Str4;
                dr["取件方联系电话*"] = p.Str5;
                dr["取件方联系地址*"] = p.Str6 + p.Str7 + p.Str8 + p.Str36;//p.Str6 + p.Str7 + p.Str8
                dr["取件方城市代码*"] = "";//p.Str13
                dr["到件方联系人*"] = p.Str10;
                dr["到件方联系电话*"] = p.Str11;
                dr["到件方联系地址*"] = p.Str12 + p.Str13 + p.Str14 + p.Str38;//p.Str12 + p.Str13 + p.Str14
                dr["委托方联系人*"] = p.CustomerOrderNumber; //百姓网订单号
                dr["委托方手机*"] = "13524407929"; //客服号码
                dr["委托方联系地址*"] = "上海市闵行区七莘路1839号财富108广场南座20楼";//地址
                dr["委托方城市代码*"] = "021";
                dr["寄件类型*"] = "快件";
                dr["物品名称*"] = p.Str2;
                dr["付款方式*"] = "转第三方月结";
                dr["订单备注"] = "";
                dr["短信备注"] = "";
                dr["付款方月结卡号"] = "70001201606220001";//月结账号(顺丰提供)
                dr["付款方公司名称"] = "上海物流科技股份有限公司";//顺丰提供
                dr["付款方联系人"] = "dana.chen";//顺丰提供
                dr["付款方联系电话"] = "021-54431003";//顺丰提供
                dr["取件方公司名称"] = "";
                dr["取件方客户卡号"] = "";
                dr["到件方公司名称"] = "";
                dr["到件方客户卡号"] = "";
                dr["到件方税号"] = "";
                dr["委托方公司名称"] = "上海物流科技股份有限公司";
                dr["委托方座机"] = "021-54431003";
                dr["委托方传真"] = "021-54431002";
                dr["委托方E-mail"] = "dana.chen@runbow.com.cn";
                dr["地址是否保密"] = "";
                dr["是否保价"] = "";
                dr["预约取件时间"] = "";
                dr["声明/申报价值"] = "";
                dr["币种"] = "";
                dr["数量（件）"] = "";
                dr["重量（KG）"] = "";
                dr["货物规格长（cm）"] = "";
                dr["货物规格宽（cm）"] = "";
                dr["货物规格高（cm）"] = "";


                dtPod.Rows.Add(dr);
            });
            #endregion

            ExportDataToExcelHelper.ExportDataSetToExcel(dtPod, "BaiXingExportPods", "");
            return new EmptyResult();
        }

        /// <summary>
        /// 导出异常信息(百姓网)
        /// </summary>
        private ActionResult ExportBaiXingPodExceptionToExcel(PodAll Pods)
        {
            #region 表头
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("客户订单号", typeof(string));
            dtPod.Columns.Add("异常描述", typeof(string));
            dtPod.Columns.Add("责任归属", typeof(string));
            dtPod.Columns.Add("解决方式", typeof(string));
            dtPod.Columns.Add("解决人", typeof(string));
            dtPod.Columns.Add("创建时间", typeof(string));
            #endregion

            #region old
            Pods.PodExceptions.OrderBy(p => p.ID).Each((i, p) =>
            {
                DataRow dr = dtPod.NewRow();
                dr["客户订单号"] = p.CustomerOrderNumber;
                dr["异常描述"] = p.Str1;
                dr["责任归属"] = p.Str2;
                dr["解决方式"] = p.Str3;
                dr["解决人"] = p.Str4;
                dr["创建时间"] = p.CreateTime.ToString();
                dtPod.Rows.Add(dr);
            });
            #endregion
            ExportDataToExcelHelper.ExportDataSetToExcel(dtPod, "ExportBaiXingPodExceptions", "");
            return new EmptyResult();
        }

        private string PODArrivalGoods(string data)
        {
            try
            {

                return string.IsNullOrEmpty(data) ? "" : (Convert.ToDateTime(data)).AddMonths(1).ToString("yyyy-MM-15");
            }
            catch (Exception)
            {

                return "";
            }

        }
        public void ExportPodsZipFile(string str)
        {
            Stream iSteam = null;
            byte[] buffer = new byte[10000];
            int length;
            long dataToRead;
            string filename = Path.GetFileName(str);
            try
            {
                iSteam = new FileStream(str, FileMode.Open, FileAccess.Read, FileShare.Read);
                dataToRead = iSteam.Length;
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                while (dataToRead > 0)
                {
                    if (Response.IsClientConnected)
                    {
                        length = iSteam.Read(buffer, 0, 10000);
                        Response.OutputStream.Write(buffer, 0, length);
                        Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write("Error:" + ex.Message);
            }
            finally
            {
                if (iSteam != null)
                {
                    iSteam.Close();
                }
            }

        }


        private ActionResult ExportPodTracksToExcel(IEnumerable<long> podIDs, IEnumerable<Column> podTrackColumns)
        {
            DataTable dt = new DataTable();
            IEnumerable<Column> columns;
            if (base.UserInfo.UserType != 2)
            {
                columns = podTrackColumns.Where(c => ((c.IsKey && c.ForView) || (!c.IsKey && !c.IsHide)) && c.ShowRoleIDs.Contains(base.UserInfo.ProjectRoleID));
            }
            else
            {
                columns = podTrackColumns.Where(c => (c.IsKey && c.ForView) || (!c.IsKey && !c.IsHide));
            }

            if (columns != null && columns.Any())
            {
                columns.Each((i, c) => dt.Columns.Add(c.DbColumnName));
            }

            var podTracksTable = new PodService().GetPodTracksByPodIDs(new GetPodInfoRequest() { PodIDs = podIDs }).Result.ConverToTable(dt);
            if (columns != null && columns.Any())
            {
                columns.Each((i, c) =>
                {
                    if (podTracksTable.Columns.Contains(c.DbColumnName))
                    {
                        podTracksTable.Columns[c.DbColumnName].ColumnName = c.DisplayName;
                    }
                });

                columns.Where(c => !c.IsKey && (c.Type == "CheckBox" || c.Type == "DropDownList"))
                    .Each((i, c) =>
                    {
                        for (int j = 0; j < podTracksTable.Rows.Count; j++)
                        {
                            podTracksTable.Rows[j][c.DisplayName] = podTracksTable.Rows[j][c.DisplayName].ToString().Trim() == "1" ? "Y" : "N";
                        }

                    });
            }

            return this.ExportDataTableToExcel(podTracksTable, "ExportPodTracks.xls");
        }

        #region Old Export function
        //private ActionResult ExportPodsToExcel(IEnumerable<Pod> pods, IEnumerable<Column> podColumns, long? customerID)
        //{
        //    DataTable dt = new DataTable();
        //    DataTable resultTable;
        //    if (customerID.HasValue && customerID == 13)
        //    {
        //        //HardCode for AdidasPurchase
        //        this.InitAdidsaPurchaseExportData(pods, out resultTable);
        //    }
        //    else
        //    {
        //        pods.Each((i, p) =>
        //        {
        //            if (p.CustomerOrderNumber.StartsWith("0"))
        //            {
        //                p.CustomerOrderNumber = "'" + p.CustomerOrderNumber;
        //            }
        //        });

        //        podColumns = podColumns.Select(
        //            c =>
        //            {
        //                if (c.InnerColumns.Count == 0)
        //                {
        //                    return c;
        //                }
        //                else
        //                {
        //                    if (customerID.HasValue && c.InnerColumns.Any(innec => innec.CustomerID == customerID.Value))
        //                    {
        //                        return c.InnerColumns.First(innerc => innerc.CustomerID == customerID.Value);
        //                    }
        //                    return c;
        //                }
        //            });

        //        IEnumerable<Column> columns;
        //        if (base.UserInfo.UserType != 2)
        //        {
        //            columns = podColumns.Where(c => ((c.IsKey && c.ForView) || (!c.IsKey && !c.IsHide)) && c.ShowRoleIDs.Contains(base.UserInfo.ProjectRoleID));
        //        }
        //        else
        //        {
        //            columns = podColumns.Where(c => (c.IsKey && c.ForView) || (!c.IsKey && !c.IsHide));
        //        }

        //        if (columns != null && columns.Any())
        //        {
        //            columns.Each((i, c) => dt.Columns.Add(c.DbColumnName));
        //        }

        //        resultTable = pods.ConverToTable(dt);

        //        if (columns != null && columns.Any())
        //        {
        //            columns.Each((i, c) =>
        //            {
        //                if (resultTable.Columns.Contains(c.DbColumnName))
        //                {
        //                    resultTable.Columns[c.DbColumnName].ColumnName = c.DisplayName;
        //                }
        //            });

        //            columns.Where(c => !c.IsKey && (c.Type == "CheckBox" || c.Type == "DropDownList"))
        //                .Each((i, c) =>
        //                {
        //                    for (int j = 0; j < resultTable.Rows.Count; j++)
        //                    {
        //                        resultTable.Rows[j][c.DisplayName] = resultTable.Rows[j][c.DisplayName].ToString().Trim() == "1" ? "Y" : "N";
        //                    }

        //                });
        //        }
        //    }

        //    ExcelHelper excelHelper = new ExcelHelper();
        //    string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
        //    string fileFullPath = Path.Combine(targetPath, "ExportPods.xlsx");
        //    excelHelper.CreateExcelByDataTable(fileFullPath, resultTable);
        //    excelHelper.Dispose();
        //    string mimeType = "application/msexcel";
        //    FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
        //    return File(fs, mimeType, "ExportPods.xlsx");
        //}
        #endregion

        private ActionResult ExportPodsToExcel(IEnumerable<Pod> pods, IEnumerable<Column> podColumns, long? customerID, bool IsAll)
        {
            return this.ExportPodsToExcel(pods.Select(p => new PodWithAttachment(p)), podColumns, customerID, 0);
        }


        private ActionResult ExportPodsToExcel(IEnumerable<PodWithAttachment> pods, IEnumerable<Column> podColumns, long? customerID, int? notAddAttachment)
        {
            DataTable dt = new DataTable();
            DataTable resultTable;
            if (customerID.HasValue && customerID == 13)
            {
                //HardCode for AdidasPurchase
                this.InitAdidsaPurchaseExportData(pods, out resultTable);
            }
            else
            {
                //pods.Each((i, p) =>
                //{
                //    if (p.CustomerOrderNumber.StartsWith("0"))
                //    {
                //        p.CustomerOrderNumber = "'" + p.CustomerOrderNumber;
                //    }
                //});

                podColumns = podColumns.Select(
                    c =>
                    {
                        if (c.InnerColumns.Count == 0)
                        {
                            return c;
                        }
                        else
                        {
                            if (customerID.HasValue && c.InnerColumns.Any(innec => innec.CustomerID == customerID.Value))
                            {
                                return c.InnerColumns.First(innerc => innerc.CustomerID == customerID.Value);
                            }
                            return c;
                        }
                    });

                IEnumerable<Column> columns;
                if (base.UserInfo.UserType != 2)
                {
                    columns = podColumns.Where(c => ((c.IsKey && c.ForView) || (!c.IsKey && !c.IsHide)) && c.ShowRoleIDs.Contains(base.UserInfo.ProjectRoleID));
                }
                else
                {
                    columns = podColumns.Where(c => (c.IsKey && c.ForView) || (!c.IsKey && !c.IsHide));
                }

                if (columns != null && columns.Any())
                {
                    columns.Each((i, c) => dt.Columns.Add(c.DbColumnName));
                }

                if (notAddAttachment.HasValue && notAddAttachment.Value > 0)
                {
                    dt.Columns.Add("IsUploadPod");
                }

                resultTable = pods.ConverToTable(dt);

                if (columns != null && columns.Any())
                {
                    columns.Each((i, c) =>
                    {
                        if (resultTable.Columns.Contains(c.DbColumnName))
                        {
                            resultTable.Columns[c.DbColumnName].ColumnName = c.DisplayName;
                        }
                    });

                    columns.Where(c => !c.IsKey && (c.Type == "CheckBox" || c.Type == "DropDownList"))
                        .Each((i, c) =>
                        {
                            for (int j = 0; j < resultTable.Rows.Count; j++)
                            {
                                resultTable.Rows[j][c.DisplayName] = resultTable.Rows[j][c.DisplayName].ToString().Trim() == "1" ? "Y" : "N";
                            }

                        });
                }
                if (notAddAttachment.HasValue && notAddAttachment.Value > 0)
                {
                    resultTable.Columns["IsUploadPod"].ColumnName = "是否有附件";
                    for (int i = 0; i < resultTable.Rows.Count; i++)
                    {
                        string isUploadPod = resultTable.Rows[i]["是否有附件"].ToString();
                        if (!string.IsNullOrEmpty(isUploadPod))
                        {
                            resultTable.Rows[i]["是否有附件"] = isUploadPod == "0" ? "N" : "Y";
                        }
                        else
                        {
                            resultTable.Rows[i]["是否有附件"] = "N";
                        }
                    }
                }
            }

            return this.ExportDataTableToExcel(resultTable, "ExportPods.xls");

        }

        private ActionResult ExportDataTableToExcel(DataTable dt, string FileName)
        {
            var sbHtml = new StringBuilder();
            sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", dt.Columns[i].ColumnName);
            }

            sbHtml.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i][j].ToString());
                }
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AppendHeader("content-disposition", "attachment;filename=" + FileName);
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        private ActionResult ExportDfcjPodsToExcel(IEnumerable<Pod> Pods)
        {
            #region 东方CJ
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("序号", typeof(string));
            dtPod.Columns.Add("类别", typeof(string));
            dtPod.Columns.Add("线路", typeof(string));
            dtPod.Columns.Add("运单号", typeof(string));
            dtPod.Columns.Add("计划号", typeof(string));
            dtPod.Columns.Add("计划日期", typeof(string));
            dtPod.Columns.Add("计划数量", typeof(string));
            dtPod.Columns.Add("运输方式", typeof(string));
            dtPod.Columns.Add("车型", typeof(string));
            dtPod.Columns.Add("装车车号", typeof(string));
            dtPod.Columns.Add("抵达仓库时间", typeof(string));
            dtPod.Columns.Add("开始装车时间", typeof(string));
            dtPod.Columns.Add("装车完成时间", typeof(string));
            dtPod.Columns.Add("装货耗时计算", typeof(string));
            dtPod.Columns.Add("实提数量", typeof(string));
            dtPod.Columns.Add("重量", typeof(string));
            dtPod.Columns.Add("体积", typeof(string));
            dtPod.Columns.Add("单件体积", typeof(string));
            dtPod.Columns.Add("是否需中转", typeof(string));
            dtPod.Columns.Add("中转地点", typeof(string));
            dtPod.Columns.Add("是否自有车辆", typeof(string));
            dtPod.Columns.Add("GPS号", typeof(string));
            dtPod.Columns.Add("发车时间", typeof(string));
            dtPod.Columns.Add("车型(干线)", typeof(string));
            dtPod.Columns.Add("发车车号(干线)", typeof(string));
            dtPod.Columns.Add("在途情况", typeof(string));
            dtPod.Columns.Add("预计抵达时间", typeof(string));
            dtPod.Columns.Add("实际抵达时间(到达末端站点)", typeof(string));
            dtPod.Columns.Add("送达时间(到达OCJ仓库)", typeof(string));
            dtPod.Columns.Add("车辆离开时间", typeof(string));
            dtPod.Columns.Add("应达时间", typeof(string));
            dtPod.Columns.Add("卸货耗时计算", typeof(string));
            dtPod.Columns.Add("到货数量", typeof(string));
            dtPod.Columns.Add("货差数量", typeof(string));
            dtPod.Columns.Add("货损数量", typeof(string));
            dtPod.Columns.Add("货损率", typeof(string));
            dtPod.Columns.Add("时效", typeof(string));
            dtPod.Columns.Add("时效差异", typeof(string));
            dtPod.Columns.Add("准时性", typeof(string));
            dtPod.Columns.Add("缺损原因", typeof(string));
            dtPod.Columns.Add("缺少原因", typeof(string));
            dtPod.Columns.Add("迟到原因", typeof(string));
            dtPod.Columns.Add("计划阶段", typeof(string));
            dtPod.Columns.Add("装运阶段", typeof(string));
            dtPod.Columns.Add("运输阶段", typeof(string));
            dtPod.Columns.Add("派送阶段", typeof(string));
            dtPod.Columns.Add("延迟责属", typeof(string));
            dtPod.Columns.Add("备注", typeof(string));

            Pods.OrderBy(p => p.DateTime1).Each((i, p) =>
            {
                DataRow dr = dtPod.NewRow();
                dr["序号"] = (i + 1).ToString();
                dr["类别"] = p.PODTypeName;
                dr["线路"] = p.StartCityName + "-" + p.EndCityName;
                dr["运单号"] = p.CustomerOrderNumber;
                dr["计划号"] = p.Str1;
                dr["计划日期"] = p.DateTime1.HasValue ? p.DateTime1.Value.ToString("yyyy-MM-dd") : "";
                dr["计划数量"] = p.Str2;
                dr["运输方式"] = (p.ShipperTypeName == "公路" || p.ShipperTypeName == "陆运") ? p.TtlOrTplName : p.ShipperTypeName;
                dr["车型"] = p.Str3;
                dr["装车车号"] = p.Str4;
                dr["抵达仓库时间"] = p.DateTime2.HasValue ? p.DateTime2.Value.ToString("yyyy-MM-dd HH:mm") : "";
                dr["开始装车时间"] = p.DateTime3.HasValue ? p.DateTime3.Value.ToString("yyyy-MM-dd HH:mm") : "";
                dr["装车完成时间"] = p.DateTime4.HasValue ? p.DateTime4.Value.ToString("yyyy-MM-dd HH:mm") : "";
                if (!string.IsNullOrEmpty(p.Str5))
                {
                    float timespan = 0;
                    if (float.TryParse(p.Str5, out timespan))
                    {
                        dr["装货耗时计算"] = timespan.ToString("f3");
                    }
                    else
                    {
                        dr["装货耗时计算"] = p.Str5;
                    }
                }

                dr["实提数量"] = p.GoodsNumber.HasValue ? p.GoodsNumber.Value.ToString() : "";
                dr["重量"] = p.Weight.HasValue ? p.Weight.Value.ToString() : "";
                dr["体积"] = p.Volume.HasValue ? p.Volume.Value.ToString() : "";
                if (!string.IsNullOrEmpty(p.Str6))
                {
                    float singleVolume = 0;
                    if (float.TryParse(p.Str6, out singleVolume))
                    {
                        dr["单件体积"] = singleVolume.ToString("f3");
                    }
                    else
                    {
                        dr["单件体积"] = p.Str6;
                    }
                }

                dr["是否需中转"] = (p.Str7 == "0" || string.IsNullOrEmpty(p.Str7)) ? "N" : "Y";
                dr["中转地点"] = p.Str8;
                dr["是否自有车辆"] = (p.Str9 == "0" || string.IsNullOrEmpty(p.Str9)) ? "N" : "Y";
                dr["GPS号"] = p.Str10;
                dr["发车时间"] = p.ActualDeliveryDate.HasValue ? p.ActualDeliveryDate.Value.ToString("yyyy-MM-dd HH:mm") : "";
                dr["车型(干线)"] = p.Str28;
                dr["发车车号(干线)"] = p.Str27;
                dr["在途情况"] = p.Str11;
                dr["预计抵达时间"] = p.DateTime6.HasValue ? p.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : "";
                dr["实际抵达时间(到达末端站点)"] = p.DateTime7.HasValue ? p.DateTime7.Value.ToString("yyyy-MM-dd HH:mm") : "";
                dr["送达时间(到达OCJ仓库)"] = p.DateTime8.HasValue ? p.DateTime8.Value.ToString("yyyy-MM-dd HH:mm") : "";
                dr["车辆离开时间"] = p.DateTime10.HasValue ? p.DateTime10.Value.ToString("yyyy-MM-dd HH:mm") : "";
                dr["应达时间"] = p.DateTime9.HasValue ? p.DateTime9.Value.ToString("yyyy-MM-dd HH:mm") : "";
                if (!string.IsNullOrEmpty(p.Str12))
                {
                    float timeSpan = 0;
                    if (float.TryParse(p.Str12, out timeSpan))
                    {
                        dr["卸货耗时计算"] = timeSpan.ToString("f2");
                    }
                    else
                    {
                        dr["卸货耗时计算"] = p.Str12;
                    }
                }

                dr["到货数量"] = p.Str13;
                dr["货差数量"] = p.Str14;
                dr["货损数量"] = p.Str15;
                dr["货损率"] = p.Str29;
                dr["时效"] = p.Str16;

                if (!string.IsNullOrEmpty(p.Str17))
                {
                    float leadTime = 0;
                    if (float.TryParse(p.Str17, out leadTime))
                    {
                        int intLeadTime = (int)leadTime;
                        if (intLeadTime < 1)
                        {
                            dr["时效差异"] = "0";
                        }
                        else
                        {
                            dr["时效差异"] = intLeadTime.ToString();
                        }
                    }
                    else
                    {
                        dr["时效差异"] = p.Str17;
                    }
                }
                dr["准时性"] = p.Str18;
                dr["缺损原因"] = p.Str19;
                dr["缺少原因"] = p.Str20;
                dr["迟到原因"] = p.Str21;
                dr["计划阶段"] = p.Str22;
                dr["装运阶段"] = p.Str23;
                dr["运输阶段"] = p.Str24;
                dr["派送阶段"] = p.Str25;
                dr["延迟责属"] = p.Str26;
                dr["备注"] = p.Str36;

                dtPod.Rows.Add(dr);
            });

            #endregion

            return this.ExportDataTableToExcel(dtPod, "ExportPods.xls");
        }

        private void GenQueryPodViewModel(QueryPodViewModel vm)
        {
            //vm.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "Pod");
            vm.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.FirstOrDefault(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.FirstOrDefault(t => t.Name == "Pod");
            vm.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.PodStates = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            if (base.UserInfo.UserType == 2)
            {
                vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.Customers = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                vm.Customers = Enumerable.Empty<SelectListItem>();
            }

            vm.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID)
                                .Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            vm.PODTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.TtlOrTpls = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.TTLORTPL)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
        }

        public ActionResult PodImport(int? id)
        {
            return View();
        }

        private string GetDisplyMessageByType(string type)
        {
            string returnval = string.Empty;
            switch (type)
            {
                case "PodDetail":
                    returnval = "导入运单明细";
                    break;

                case "PodFeadBack":
                    returnval = "导入运单回访信息";
                    break;

                case "PodTrack":
                    returnval = "导入运单跟踪信息";
                    break;

                case "PodException":
                    returnval = "导入运单异常信息";
                    break;

                case "PodStatusLog":
                    returnval = "导入运单车辆信息";
                    break;

                case "PodReplyDocument":
                    returnval = "导入运单回单信息";
                    break;

                default:
                    returnval = string.Empty;
                    break;
            }

            return returnval;
        }

        public ActionResult PodRelatedImport(int? id, string type)
        {
            ViewBag.DisplyMessage = this.GetDisplyMessageByType(type);
            ViewBag.Type = type;
            IEnumerable<SelectListItem> customers = new List<SelectListItem>();
            if (base.UserInfo.UserType == 2 || base.UserInfo.UserType == 1)
            {
                customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else if (base.UserInfo.UserType == 0)
            {
                customers = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }

            ViewBag.Customers = customers;

            return View();
        }

        private DataSet GetDataFromExcel(HttpPostedFileBase hpf)
        {
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), Runbow.TWS.Common.Constants.TEMPFOLDER);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string fileName = base.UserInfo.ID.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(hpf.FileName);
            string fullPath = Path.Combine(targetPath, fileName);
            hpf.SaveAs(fullPath);
            hpf.InputStream.Close();

            Runbow.TWS.Common.ExcelHelper excelHelper = new Runbow.TWS.Common.ExcelHelper(fullPath);
            DataSet ds = excelHelper.GetAllDataFromAllSheets();
            excelHelper.Dispose();
            MyFile.Delete(fullPath);

            return ds;
        }

        private string GenerateLog56NotRegistedPhoneMessage(IEnumerable<Log56PhoneStatus> log56PhoneStatus)
        {
            StringBuilder successSB = new StringBuilder();
            successSB.Append("<br /")
                .Append("<h4>以下号码还未在路歌平台中注册,请通知相关人员对这些号码进行注册.</h4>").Append("<br />")
                .Append("<table><thead><tr><th>手机号码</th></tr></thead><tbody>");
            log56PhoneStatus.Each((i, l) =>
            {
                successSB.Append("<tr><td>").Append(l.Phone).Append("</td></tr>");
            });

            successSB.Append("</tbody></table>");

            return successSB.ToString();
        }

        private string GenerateReturnMessage<T>(IEnumerable<T> objs, IEnumerable<Column> columns, StringBuilder successSB, string displyMessage, int originalCount)
        {
            successSB.Append("<h3>" + displyMessage + "成功</h3><br/>");
            if (objs.Count() < originalCount)
            {
                successSB.Append("<h4>导入" + objs.Count() + "条, 实际导入" + originalCount + ", 缺少条数由于数据库中数据与导入数据完全一致, 不用重复导入.</h4><br />");
            }
            successSB.Append("<table><thead><tr><th>").Append(columns.First(c => c.DbColumnName == "SystemNumber").DisplayName)
                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "CustomerOrderNumber").DisplayName).Append("</th>");
            foreach (var o in columns.Where(c => c.IsImportColumn && !c.IsKey).OrderBy(c => c.Order))
            {
                successSB.Append("<th>").Append(o.DisplayName).Append("</th>");
            }

            successSB.Append("</tr></thead><tbody>");

            foreach (var o in objs)
            {
                successSB.Append("<tr><td>").Append(typeof(T).GetProperty("SystemNumber").GetValue(o).ToString())
                         .Append("</td><td>").Append(typeof(T).GetProperty("CustomerOrderNumber").GetValue(o).ToString())
                         .Append("</td>");
                foreach (var c in columns.Where(t => t.IsImportColumn && !t.IsKey).OrderBy(c => c.Order))
                {
                    successSB.Append("<td>").Append(typeof(T).GetProperty(c.DbColumnName).GetValue(o)).Append("</td>");
                }
                successSB.Append("</tr>");
            }
            successSB.Append("</tbody></table>");

            return successSB.ToString();
        }

        [HttpPost]
        public string PodRelatedImport(string type, long customer)
        {
            if (Request.Files.Count > 0)
            {
                type = type.Trim();
                string displyMessage = this.GetDisplyMessageByType(type);
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null && ds.Tables[0] != null)
                    {
                        Runbow.TWS.Entity.Module module = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001");
                        IEnumerable<Column> columns;
                        try
                        {
                            columns = module.Tables.TableCollection.First(t => t.Name == type).ColumnCollection.Where(c => (c.IsKey) || (!c.IsKey && !c.IsHide) || (!c.IsKey && c.IsHide && c.InnerColumns.Any(innerc => innerc.CustomerID == customer)))
                            .Select(c =>
                            {
                                if (c.InnerColumns.Count == 0)
                                {
                                    return c;
                                }
                                else
                                {
                                    if (c.InnerColumns.Any(innerc => innerc.CustomerID == customer))
                                    {
                                        return c.InnerColumns.First(innerc => innerc.CustomerID == customer);
                                    }

                                    return c;
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        //bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
                        //Because of Use POD Split, we need to use SystemNumber as the unique id
                        bool useCustomerOrderNumber = false;
                        StringBuilder sb = new StringBuilder();
                        string errorMessage = new { result = "<h3>" + displyMessage + "失败</h3><br/>系统忙或文件内容有误，请检查后再试", IsSuccess = false }.ToJsonString();
                        StringBuilder returnSb = new StringBuilder();
                        string successMessage = string.Empty;
                        int count = 0;
                        switch (type)
                        {
                            case "PodDetail":
                                IEnumerable<PodDetail> podDetails = this.InitPodRelatedFromDataTable<PodDetail>(ds.Tables[0], columns, useCustomerOrderNumber, sb);
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    return new { result = "<h3>" + displyMessage + "失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                                }

                                var responsePodDetail = new PodService().AddPodDetails(new AddPodDetailsRequest() { PodDetails = podDetails, CustomerID = customer });
                                if (!responsePodDetail.IsSuccess)
                                {
                                    return errorMessage;
                                }
                                else
                                {
                                    count = responsePodDetail.Result.Count();
                                    successMessage = this.GenerateReturnMessage<PodDetail>(responsePodDetail.Result, columns, returnSb, displyMessage, podDetails.Count());
                                }
                                break;

                            case "PodFeadBack":
                                IEnumerable<PodFeadBack> podFeadBacks = this.InitPodRelatedFromDataTable<PodFeadBack>(ds.Tables[0], columns, useCustomerOrderNumber, sb);
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    return new { result = "<h3>" + displyMessage + "失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                                }

                                var responsePodFeadBack = new PodService().AddPodFeadBacks(new AddPodFeadBacksRequest() { PodFeadBacks = podFeadBacks, CustomerID = customer });
                                if (!responsePodFeadBack.IsSuccess)
                                {
                                    return errorMessage;
                                }
                                else
                                {
                                    count = responsePodFeadBack.Result.Count();
                                    successMessage = this.GenerateReturnMessage<PodFeadBack>(responsePodFeadBack.Result, columns, returnSb, displyMessage, podFeadBacks.Count());
                                }
                                break;

                            case "PodTrack":
                                IEnumerable<PodTrack> podTracks = this.InitPodRelatedFromDataTable<PodTrack>(ds.Tables[0], columns, useCustomerOrderNumber, sb);
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    return new { result = "<h3>" + displyMessage + "失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                                }

                                if (customer == 8)
                                {
                                    var checkNikePodTrackResponse = new PodService().CheckNikePodTrack(new AddPodTracksRequest() { PodTracks = podTracks, CustomerID = customer });
                                    if (!checkNikePodTrackResponse.IsSuccess)
                                    {
                                        StringBuilder sbNike = new StringBuilder();
                                        sbNike.Append("<h3>上传Nike跟踪失败</h3><br/>");
                                        sbNike.Append("运单:<br/>");
                                        foreach (string number in checkNikePodTrackResponse.Result)
                                        {
                                            sbNike.Append(number).Append("<br/>");
                                        }
                                        sbNike.Append("已在这个时段导入跟踪，无需重复导入.").Append("<br/>");
                                        return new { result = sbNike.ToString(), IsSuccess = false }.ToJsonString();
                                    }
                                }

                                var responsePodTrack = new PodService().AddPodTracks(new AddPodTracksRequest() { PodTracks = podTracks, CustomerID = customer });
                                if (!responsePodTrack.IsSuccess)
                                {
                                    return errorMessage;
                                }
                                else
                                {
                                    count = responsePodTrack.Result.Count();
                                    successMessage = this.GenerateReturnMessage<PodTrack>(responsePodTrack.Result, columns, returnSb, displyMessage, podTracks.Count());
                                }
                                break;

                            case "PodException":
                                IEnumerable<PodException> podExceptions = this.InitPodRelatedFromDataTable<PodException>(ds.Tables[0], columns, useCustomerOrderNumber, sb);
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    return new { result = "<h3>" + displyMessage + "失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                                }

                                var responsePodException = new PodService().AddPodExceptions(new AddPodExceptionsRequest() { PodExceptions = podExceptions, CustomerID = customer });
                                if (!responsePodException.IsSuccess)
                                {
                                    return errorMessage;
                                }
                                else
                                {
                                    count = responsePodException.Result.Count();
                                    successMessage = this.GenerateReturnMessage<PodException>(responsePodException.Result, columns, returnSb, displyMessage, podExceptions.Count());
                                }
                                break;

                            case "PodStatusLog":
                                IEnumerable<PodStatusLog> podStatusLogs = this.InitPodRelatedFromDataTable<PodStatusLog>(ds.Tables[0], columns, useCustomerOrderNumber, sb);
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    return new { result = "<h3>" + displyMessage + "失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                                }

                                sb.Clear();

                                podStatusLogs.Each((i, log) =>
                                {
                                    if (string.IsNullOrEmpty(log.Str2))
                                    {
                                        sb.Append("运单号:").Append(log.CustomerOrderNumber).Append(" 车牌号不能为空").Append("<br/>");
                                    }

                                    if (string.IsNullOrEmpty(log.Str4) || (!string.Equals("提货车辆", log.Str4, StringComparison.OrdinalIgnoreCase) && !string.Equals("干线车辆", log.Str4.Trim(), StringComparison.OrdinalIgnoreCase) && !string.Equals("配送车辆", log.Str4.Trim(), StringComparison.OrdinalIgnoreCase)))
                                    {
                                        sb.Append("运单号:").Append(log.CustomerOrderNumber).Append(" 车辆类型(提/干/配) 只能是 提货车辆/干线车辆/配送车辆 中的一种").Append("<br/>");
                                    }

                                    if (log.DateTime1 == null)
                                    {
                                        sb.Append("运单号:").Append(log.CustomerOrderNumber).Append(" 实际发车时间不能为空").Append("<br/>");
                                    }

                                    if (log.DateTime2 == null)
                                    {
                                        sb.Append("运单号:").Append(log.CustomerOrderNumber).Append(" 预计到达时间不能为空").Append("<br/>");
                                    }

                                });

                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    return new { result = "<h3>" + displyMessage + "失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                                }

                                var responsePodStatusLog = new PodService().AddPodStatusLogs(new AddPodStatusLogsRequest() { PodStatusLogs = podStatusLogs, CustomerID = customer });
                                if (!responsePodStatusLog.IsSuccess)
                                {
                                    return errorMessage;
                                }
                                else
                                {
                                    count = responsePodStatusLog.Result.Count();
                                    successMessage = this.GenerateReturnMessage<PodStatusLog>(responsePodStatusLog.Result, columns, returnSb, displyMessage, podStatusLogs.Count());

                                    //var phoneStatus = responsePodStatusLog.Result.Select(p => new Log56PhoneStatus() { Phone = p.Str1 });
                                    //var updateLog56PhoneStatusResponse = new PodService().UpdateLog56PhoneStatus(new UpdateLog56PhoneStatusRequest() { Log56PhoneStatus = phoneStatus });
                                    //if (updateLog56PhoneStatusResponse.IsSuccess && updateLog56PhoneStatusResponse.Result != null && updateLog56PhoneStatusResponse.Result.Any())
                                    //{
                                    //    successMessage += this.GenerateLog56NotRegistedPhoneMessage(updateLog56PhoneStatusResponse.Result);
                                    //}

                                }
                                break;
                            case "PodStatusTrack":
                                IEnumerable<PodStatusTrack> podStatusTracks = this.InitPodRelatedFromDataTable<PodStatusTrack>(ds.Tables[0], columns, useCustomerOrderNumber, sb);
                                this.CheckPodStatusTrackData(podStatusTracks, sb);
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    return new { result = "<h3>" + displyMessage + "失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                                }

                                var responsePodStatusTrack = new PodService().AddPodStatusTracks(new AddPodStatusTracksRequest() { PodStatusTracks = podStatusTracks, CustomerID = customer });
                                if (!responsePodStatusTrack.IsSuccess)
                                {
                                    return errorMessage;
                                }
                                else
                                {
                                    count = responsePodStatusTrack.Result.Count();
                                    successMessage = this.GenerateReturnMessage<PodStatusTrack>(responsePodStatusTrack.Result, columns, returnSb, displyMessage, podStatusTracks.Count());

                                }
                                break;
                            case "PodReplyDocument":
                                IEnumerable<PodReplyDocument> podReplyDocuments = this.InitPodRelatedFromDataTable<PodReplyDocument>(ds.Tables[0], columns, useCustomerOrderNumber, sb);
                                if (!string.IsNullOrEmpty(sb.ToString()))
                                {
                                    return new { result = "<h3>" + displyMessage + "失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                                }

                                var responsePodReplyDocument = new PodService().AddPodReplyDocuments(new AddPodReplyDocumentsRequest() { PodReplyDocuments = podReplyDocuments, CustomerID = customer });
                                if (!responsePodReplyDocument.IsSuccess)
                                {
                                    return errorMessage;
                                }
                                else
                                {
                                    count = responsePodReplyDocument.Result.Count();
                                    successMessage = this.GenerateReturnMessage<PodReplyDocument>(responsePodReplyDocument.Result, columns, returnSb, displyMessage, podReplyDocuments.Count());
                                }
                                break;

                            default:
                                break;
                        }

                        return new { result = successMessage, IsSuccess = true, Count = count }.ToJsonString();
                    }

                    return new { result = "<h3>" + displyMessage + "失败</h3><br/>excel内容有误！", IsSuccess = false }.ToJsonString();
                }

                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }

            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }

        //检测状态跟踪字段的合法性
        private void CheckPodStatusTrackData(IEnumerable<PodStatusTrack> podStatusTracks, StringBuilder sb)
        {

            podStatusTracks.Each((i, p) =>
            {
                if (string.IsNullOrEmpty(p.Str1.Trim()))
                {
                    sb.Append("第" + (i + 1).ToString() + "行，运单状态列不能为空，请修改。<br/>");
                }


                if (!this.podStatusTrack_Status.Contains(p.Str1.Trim()))
                {
                    sb.Append("第" + (i + 1).ToString() + "行，运单状态列, <strong>" + p.Str1 + "</strong>数据不合法，请修改。<br/>");
                }

                if (string.IsNullOrEmpty(p.Str2.Trim()))
                {
                    sb.Append("第" + (i + 1).ToString() + "行，跟踪状态列不能为空，请修改。<br/>");
                }

                if (!this.podStatusTrack_TrackStatus.Contains(p.Str2.Trim()))
                {
                    sb.Append("第" + (i + 1).ToString() + "行，跟踪状态列, <strong>" + p.Str2 + "</strong>数据不合法，请修改。<br/>");
                }

                if (p.DateTime1 == null)
                {
                    sb.Append("第" + (i + 1).ToString() + "行，时间列不能为空，请修改。<br/>");
                }
            });
        }

        //导入
        [HttpPost]
        public string PodImport()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null && ds.Tables[0] != null)
                    {

                        var project = ApplicationConfigHelper.GetApplicationConfig().ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                        Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M001");
                        IEnumerable<Column> columns = module.Tables.TableCollection.First(t => t.Name == "Pod").ColumnCollection;
                        var customerColumn = columns.First(c => c.DbColumnName == "CustomerName").DisplayName;
                        var customerName = ds.Tables[0].Rows[0][customerColumn].ToString();
                        var customerID = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).First(c => c.Name.Equals(customerName, StringComparison.OrdinalIgnoreCase)).CustomerID;
                        columns = columns
                           .Select(c =>
                           {
                               if (c.InnerColumns.Count == 0)
                               {
                                   return c;
                               }
                               else
                               {
                                   if (c.InnerColumns.Any(innerc => innerc.CustomerID == customerID))
                                   {
                                       return c.InnerColumns.First(innerc => innerc.CustomerID == customerID);
                                   }

                                   return c;
                               }
                           });

                        bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
                        StringBuilder sb = new StringBuilder();
                        IEnumerable<Pod> pods = this.InitPodFromDataTable(ds.Tables[0], columns, useCustomerOrderNumber, sb);

                        if (!string.IsNullOrEmpty(sb.ToString()))
                        {
                            return new { result = "<h3>运单导入失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }

                        if (pods == null || !pods.Any())
                        {
                            return new { result = "<h3>Excel无数据</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }

                        IEnumerable<string> existsPodCustomerOrderNumbers = Enumerable.Empty<string>();

                        if (useCustomerOrderNumber)
                        {
                            var checkPodExistsResponse = new PodService().CheckIfPodExistsByPodCustomerOrderNumber(new CheckIfPodExistsByPodCustomerOrderNumberRequest()
                            {
                                CustomerOrderNumberCollection = pods.Select(p => p.CustomerOrderNumber.Trim()),
                                ProjectID = base.UserInfo.ProjectID,
                                CustomerID = pods.First().CustomerID.Value
                            });

                            if (!checkPodExistsResponse.IsSuccess)
                            {
                                return new { result = "<h3>系统在检查是否存在运单时出错！</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                            }

                            existsPodCustomerOrderNumbers = checkPodExistsResponse.Result;
                        }

                        //string systemNumberBase;
                        int todayPodNum = 0;

                        pods.Each((k, p) =>
                        {
                            if (useCustomerOrderNumber)
                            {
                                if (!existsPodCustomerOrderNumbers.Any(c => c.Trim() == p.CustomerOrderNumber.Trim()))
                                {
                                    todayPodNum++;
                                    p.SystemNumber = string.Concat("Runbow", DateTime.Now.ToString("yyyyMMddHHmmssff"), (10000 + todayPodNum).ToString().Substring(1));
                                }
                                else
                                {
                                    p.SystemNumber = "TempSystemNumber";
                                }
                            }
                            else
                            {
                                todayPodNum++;
                                p.SystemNumber = string.Concat("Runbow", DateTime.Now.ToString("yyyyMMddHHmmssff"), (10000 + todayPodNum).ToString().Substring(1));
                                p.CustomerOrderNumber = p.SystemNumber;
                            }

                        });

                        var response = new PodService().AddPods(new AddPodsRequest() { Pods = pods });

                        if (response.IsSuccess)
                        {
                            StringBuilder successSB = new StringBuilder();
                            successSB.Append("<h3>导入运单成功</h3><br/>");
                            successSB.Append("<table><thead><tr><th>").Append(columns.First(c => c.DbColumnName == "SystemNumber").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "CustomerOrderNumber").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "PODStateName").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "ShipperName").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "ShipperTypeName").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "StartCityName").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "EndCityName").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "PODTypeName").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "TtlOrTplName").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "ActualDeliveryDate").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "BoxNumber").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "GoodsNumber").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "Weight").DisplayName)
                                .Append("</th><th>").Append(columns.First(c => c.DbColumnName == "Volume").DisplayName)
                                .Append("</th></tr></thead><tbody>");
                            foreach (var o in response.Result)
                            {
                                successSB.Append("<tr><td>").Append(o.SystemNumber)
                                    .Append("</td><td>").Append(o.CustomerOrderNumber)
                                    .Append("</td><td>").Append(o.PODStateName)
                                    .Append("</td><td>").Append(o.ShipperName)
                                    .Append("</td><td>").Append(o.ShipperTypeName)
                                    .Append("</td><td>").Append(o.StartCityName)
                                    .Append("</td><td>").Append(o.EndCityName)
                                    .Append("</td><td>").Append(o.PODTypeName)
                                    .Append("</td><td>").Append(o.TtlOrTplName)
                                    .Append("</td><td>").Append(o.ActualDeliveryDate)
                                    .Append("</td><td>").Append(o.BoxNumber)
                                    .Append("</td><td>").Append(o.GoodsNumber)
                                    .Append("</td><td>").Append(o.Weight)
                                    .Append("</td><td>").Append(o.Volume)
                                    .Append("</td></tr>");
                            }
                            successSB.Append("</tbody></table>");
                            return new { result = successSB.ToString(), IsSuccess = true }.ToJsonString();
                        }
                        else
                        {
                            return new { result = "<h3>导入运单失败</h3><br/>系统忙，请稍后再试", IsSuccess = false }.ToJsonString();
                        }
                    }

                    return new { result = "<h3>导入运单失败</h3><br/>excel内容有误！", IsSuccess = false }.ToJsonString();
                }

                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }
            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }

        #region InitPod

        private IEnumerable<T> InitPodRelatedFromDataTable<T>(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb) where T : new()
        {
            IList<T> podRelatedCollection = new List<T>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Columns.Contains("使用客户运单号"))
                {
                    string v = dt.Rows[i]["使用客户运单号"].ToString().Trim();
                    if (!string.IsNullOrEmpty(v))
                    {
                        useCustomerOrderNumber = (v == "1" || v == "true" || v == "是" || v == "Y" || v == "y") ? true : false;
                    }
                    else
                    {
                        useCustomerOrderNumber = false;
                    }
                }
                T podRelated = new T();
                typeof(T).GetProperty("Creator").SetValue(podRelated, base.UserInfo.Name, null);
                typeof(T).GetProperty("CreateTime").SetValue(podRelated, DateTime.Now, null);

                string value;
                int j = 0;
                for (j = 0; j < dt.Columns.Count; j++)
                {
                    if ((useCustomerOrderNumber && string.Equals(dt.Columns[j].ColumnName.Trim(), columnsConfig.First(c => c.DbColumnName == "CustomerOrderNumber").DisplayName, StringComparison.OrdinalIgnoreCase))
                        || (!useCustomerOrderNumber && string.Equals(dt.Columns[j].ColumnName.Trim(), columnsConfig.First(c => c.DbColumnName == "SystemNumber").DisplayName, StringComparison.OrdinalIgnoreCase)))
                    {
                        value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                        //value = dt.Rows[i][j].ToString().Trim();
                        if (useCustomerOrderNumber)
                        {
                            typeof(T).GetProperty("CustomerOrderNumber").SetValue(podRelated, value, null);
                        }
                        else
                        {
                            typeof(T).GetProperty("SystemNumber").SetValue(podRelated, value, null);
                            //typeof(T).GetProperty("CustomerOrderNumber").SetValue(podRelated, value, null);
                        }

                        break;
                    }
                }

                if (j == dt.Columns.Count)
                {
                    sb.Append("Excel文件内容有误，每行必须有系统运单号或者客户运单号作为标识");
                }

                foreach (var column in columnsConfig.Where(c => c.IsImportColumn))
                {
                    for (j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(T).GetProperty(column.DbColumnName).SetValue(podRelated, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(T).GetProperty(column.DbColumnName).SetValue(podRelated, "1", null);
                                        }
                                        else
                                        {
                                            typeof(T).GetProperty(column.DbColumnName).SetValue(podRelated, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否,Y,N中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    typeof(T).GetProperty(column.DbColumnName).SetValue(podRelated, value, null);
                                }

                                break;
                            }

                            break;
                        }
                    }
                }

                podRelatedCollection.Add(podRelated);
            }

            return podRelatedCollection;
        }

        private IEnumerable<Pod> InitPodFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb)
        {
            IList<Pod> pods = new List<Pod>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Pod pod = new Pod();
                pod.ProjectID = base.UserInfo.ProjectID;
                pod.Creator = base.UserInfo.Name;
                pod.CreateTime = DateTime.Now;
                pod.Type = 2;
                string columnName;
                string value;

                if (useCustomerOrderNumber)
                {
                    columnName = columnsConfig.First(c => c.DbColumnName == "CustomerOrderNumber").DisplayName;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (string.IsNullOrEmpty(value))
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + " 不能为空<br/>");
                            }

                            if (pods.Any(p => p.CustomerOrderNumber == value))
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + " 重复<br/>");
                            }
                            pod.CustomerOrderNumber = value;
                            break;
                        }
                    }
                }

                if (base.UserInfo.UserType == 0)
                {
                    pod.CustomerID = base.UserInfo.CustomerOrShipperID;
                    pod.CustomerName = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name;
                }
                else
                {
                    columnName = columnsConfig.First(c => c.DbColumnName == "CustomerName").DisplayName;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (string.IsNullOrEmpty(value))
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + " 不能为空<br/>");
                                break;
                            }

                            var customer = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).FirstOrDefault(c => string.Equals(c.CustomerName, value.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (customer == null)
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列,  <strong>" + value + " </strong> 在系统中不存在或当前用户无权限导入此客户运单，请先配置。<br/>");
                                break;
                            }
                            pod.CustomerName = value.Trim();
                            pod.CustomerID = customer.CustomerID;
                            break;
                        }
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "ShipperTypeName").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            var shipperType = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE).FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (shipperType == null)
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                                break;
                            }
                            pod.ShipperTypeName = value;
                            pod.ShipperTypeID = shipperType.ID;
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "TtlOrTplName").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            var ttlOrTpl = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.TTLORTPL).FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (ttlOrTpl == null)
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                                break;
                            }
                            pod.TtlOrTplName = value;
                            pod.TtlOrTplID = ttlOrTpl.ID;
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "PODTypeName").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var podType = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE).FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (podType == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        pod.PODTypeName = value;
                        pod.PODTypeID = podType.ID;
                        break;
                    }
                }

                var initPodState = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE).First(s => s.Code == "01");
                pod.PODStateID = initPodState.ID;
                pod.PODStateName = initPodState.Name;

                columnName = columnsConfig.First(c => c.DbColumnName == "StartCityName").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var startCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (startCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        pod.StartCityName = value;
                        pod.StartCityID = startCity.ID;
                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "EndCityName").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        pod.EndCityName = value;
                        pod.EndCityID = endCity.ID;
                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "ActualDeliveryDate").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            DateTime dateTimeTemp;
                            if (DateTime.TryParse(value.Trim(), out dateTimeTemp))
                            {
                                pod.ActualDeliveryDate = dateTimeTemp;
                            }
                            else
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 不是日期格式，请修改。<br/>");
                            }
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "BoxNumber").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            double boxNumber;
                            if (double.TryParse(value.Trim(), out boxNumber))
                            {
                                pod.BoxNumber = boxNumber;
                            }
                            else
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列,<strong> " + value + "</strong>不是数字格式，请修改。<br/>");
                            }
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "GoodsNumber").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            double goodsNumber;
                            if (double.TryParse(value.Trim(), out goodsNumber))
                            {
                                pod.GoodsNumber = goodsNumber;
                            }
                            else
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是数字格式，请修改。<br/>");
                            }
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "Weight").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            double weight;
                            if (double.TryParse(value.Trim(), out weight))
                            {
                                pod.Weight = weight;
                            }
                            else
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是数字格式，请修改。<br/>");
                            }
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "Volume").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            double volume;
                            if (double.TryParse(value.Trim(), out volume))
                            {
                                pod.Volume = volume;
                            }
                            else
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是数字格式，请修改。<br/>");
                            }
                            break;
                        }

                        break;
                    }
                }

                foreach (var column in columnsConfig.Where(c => c.IsImportColumn && !c.IsKey))
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(Pod).GetProperty(column.DbColumnName).SetValue(pod, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(Pod).GetProperty(column.DbColumnName).SetValue(pod, "1", null);
                                        }
                                        else
                                        {
                                            typeof(Pod).GetProperty(column.DbColumnName).SetValue(pod, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    typeof(Pod).GetProperty(column.DbColumnName).SetValue(pod, value, null);
                                }

                                break;
                            }

                            break;
                        }
                    }
                }

                pods.Add(pod);
            }

            return pods;
        }

        #endregion InitPod

        ///// <summary>
        ///// 回单附件批量上传
        ///// </summary>
        //public ActionResult PodAttachmentImport()
        //{
        //    return View();
        //}

        //回单附件批量上传
        [HttpGet]
        public ActionResult PodAttachmentImport()
        {
            ViewBag.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                       .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View();
        }
        [HttpPost]
        public string PodAttachmentImport(long customer, string customerName)
        {
            //新路径
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(uploadFolderPath, base.UserInfo.ProjectID.ToString(), DateTime.Now.DateTimeToString());
            DateTime createDate;
            string attachmentGroupID = string.Empty, url = string.Empty, actualNameInServer = string.Empty, displayName = string.Empty, ext = string.Empty;
            IList<Attachment> attachments = new List<Attachment>();

            if (string.IsNullOrEmpty(targetPath) || !Path.IsPathRooted(targetPath))
            {
                return new { msg = "程序出错！" }.ToJsonString();
            }

            bool isMultiple = true;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    displayName = Path.GetFileName(hpf.FileName);
                    ext = Path.GetExtension(hpf.FileName);

                    if (!ext.ToLower().Equals(".zip"))
                    {
                        return new { msg = "批量上传，请用zip格式压缩" }.ToJsonString();
                    }
                    actualNameInServer = customerName + "_" + UserInfo.ProjectName + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                    url = Path.Combine(targetPath, actualNameInServer);
                    hpf.SaveAs(url);
                    hpf.InputStream.Close();
                    if (ext.ToLower().Equals(".zip"))
                    {
                        IList<string> unZipedFileName = new List<string>();
                        ZipHelper.UnZipNew(url, targetPath, customerName, unZipedFileName);
                        MyFile.Delete(url);
                        unZipedFileName.Each((k, fileName) =>
                        {
                            //actualNameInServer = Path.GetFileName(fileName);
                            //ext = Path.GetExtension(fileName);
                            //displayName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(actualNameInServer));
                            //createDate = DateTime.Now;
                            //amsUpload.Add(new AMSUpload() { FileName = displayName, FileType = ext, ServerName = actualNameInServer, FilePath = fileName, ProjectID = customer, ProjectName = customerName, OrderNo = "", Creator = UserInfo.Name, CreateTime = createDate, Updator = "", UpdateTime = createDate, Status = false });
                            actualNameInServer = Path.GetFileName(fileName);
                            ext = Path.GetExtension(fileName);
                            string groupID = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(actualNameInServer));
                            displayName = actualNameInServer.Substring(actualNameInServer.LastIndexOf('_') + 1);
                            createDate = DateTime.Now;
                            attachments.Add(new Attachment() { ActualNameInServer = actualNameInServer, DisplayName = displayName, Extension = ext, Url = fileName, GroupID = groupID, CreateDate = createDate, CreateUserID = base.UserInfo.ID, Creator = base.UserInfo.Name });
                        });
                    }
                    else
                    {
                        isMultiple = false;
                        attachments.Add(new Attachment() { ActualNameInServer = actualNameInServer, DisplayName = displayName, Extension = ext, Url = url, GroupID = string.IsNullOrEmpty(attachmentGroupID) ? Guid.NewGuid().ToString() : attachmentGroupID, CreateDate = DateTime.Now, CreateUserID = base.UserInfo.ID, Creator = base.UserInfo.Name });
                        //amsUpload.Add(new AMSUpload() { FileName = displayName, FileType = ext, ServerName = actualNameInServer, FilePath = url, ProjectID = UserInfo.ProjectID, ProjectName = UserInfo.ProjectName, OrderNo = "", Creator = UserInfo.Name, CreateTime = DateTime.Now, Updator = "", UpdateTime = DateTime.Now, Status = false });
                    }

                    AttachmentService service = new AttachmentService();
                    Response<IEnumerable<Attachment>> response = service.AddAttachment(new AddAttachmentRequest() { attachments = attachments, IsCoverOld = true });

                    if (response.IsSuccess)
                    {
                        if (isMultiple)
                        {
                            //return new
                            //{
                            //    msg = "批量上传文件成功",
                            //    aids = response.Result.Select(a => a.ID),
                            //    anms = response.Result.Select(a => a.DisplayName),
                            //    times = response.Result.Select(a => a.CreateDate),
                            //    creators = response.Result.Select(a => a.Creator)
                            //}.ToJsonString();
                            return new { result = "批量上传文件成功!", IsSuccess = true }.ToJsonString();
                        }
                        else
                        {
                            //return new
                            //{
                            //    msg = "上传文件成功",
                            //    gid = response.Result.First().GroupID,
                            //    aid = response.Result.First().ID,
                            //    anm = response.Result.First().DisplayName,
                            //    time = response.Result.First().CreateDate,
                            //    creator = response.Result.First().Creator
                            //}.ToJsonString();
                            return new { result = "上传文件成功!", IsSuccess = true }.ToJsonString();
                        }
                    }
                    else
                    {
                        return new { msg = "上传文件失败！" }.ToJsonString();
                    }

                    //AMSUploadService service = new AMSUploadService();
                    //查询已存在的记录
                    //Response<IEnumerable<AMSUpload>> resams = service.GetAMSUpload(new AddAMSUploadRequest() { amsUpload = amsUpload });

                    //执行新增修改操作
                    //Response<IEnumerable<AMSUpload>> response = service.AddAMSUpload(new AddAMSUploadRequest() { amsUpload = amsUpload });
                    //if (response.IsSuccess)
                    //{
                    //    #region 删除已存在记录的图片
                    //    if (resams.IsSuccess)
                    //    {
                    //        if (resams.Result != null)
                    //        {
                    //            foreach (AMSUpload a in resams.Result)
                    //            {
                    //                if (MyFile.Exists(a.FilePath)) MyFile.Delete(a.FilePath);
                    //            }
                    //        }
                    //    }
                    //    #endregion
                    //    return new { result = "批量上传文件成功!", IsSuccess = true }.ToJsonString();
                    //}
                }
                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }

            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }

        [HttpPost]
        public ActionResult SettledPod(string SelectedSettledPodIDs, int SettltedType)
        {
            var podIDs = SelectedSettledPodIDs.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);
            var response = new PodService().SettledPodSearch(new SettledPodRequest() { IDs = podIDs, SettledType = SettltedType });
            if (response.IsSuccess)
            {
                return View(new SettledPodViewModel() { GroupedPods = response.Result.GroupedPods, PodIDs = response.Result.PodIDs, SettledType = response.Result.SettledType });
            }
            return Error(response.Exception.Message);
        }

        [HttpGet]
        public ActionResult ViewPodUseList(string PodIDs)
        {
            ViewPodUseListViewModel vm = new ViewPodUseListViewModel();
            var podIDs = PodIDs.Split('|').Select(i => i.ObjectToInt64());
            vm.ProjectRoleID = base.UserInfo.ProjectRoleID;
            vm.IsInnerUser = base.UserInfo.UserType == 2;
            vm.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "Pod");
            var response = new PodService().QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = podIDs });
            if (response.IsSuccess)
            {
                vm.PodCollection = response.Result;
                return View(vm);
            }

            return Error(response.Exception.Message);
        }

        [HttpPost]
        public ActionResult SettlingPod(string PodIDs, int SettledType)
        {
            var podIDs = PodIDs.Split('|').Select(i => i.ObjectToInt64());
            PodService service = new PodService();
            var queryPodByIDsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = podIDs });
            StringBuilder message = new StringBuilder();
            if (queryPodByIDsResponse.IsSuccess)
            {
                if (SettledType == 0)
                {
                    queryPodByIDsResponse.Result.GroupBy(p => p.CustomerID).Each((i, g) => this.SettingCustomerOrShipperPods(SettledType, g.Key.Value, g.First().CustomerID.Value, g.Select(p => p.ID), ref message));
                }
                else if (SettledType == 1)
                {
                    queryPodByIDsResponse.Result.GroupBy(p => p.ShipperID).Each((i, g) => this.SettingCustomerOrShipperPods(SettledType, g.Key.Value, g.First().CustomerID.Value, g.Select(p => p.ID), ref message));
                }

            }

            if (message.Length > 0)
            {
                ViewData["ErrorMessage"] = message.ToString();
                return View("ViewSettledErrorMessage");
            }

            var response = new SettledService().GetSettledPodByPodIDs(new GetSettledPodByPodIDsRequest() { PodIDs = podIDs, SettledType = SettledType });
            if (response.IsSuccess)
            {
                ViewData["SettledType"] = SettledType;
                return View("ViewSettledPod", response.Result);
            }

            return Error(response.Exception.Message);
        }

        [HttpGet]
        public ActionResult PODReplyDocumentAudit()
        {
            PodReplyDocumentAuditViewModel vm = new PodReplyDocumentAuditViewModel();
            vm.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.PODTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE)
                               .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.SearchCondition = new PodReplyDocumentSearchCondition();
            vm.IsInnerUser = true;

            if (base.UserInfo.UserType != 2)
            {
                vm.IsInnerUser = false;
                if (base.UserInfo.UserType == 1)
                {
                    vm.ShipperID = base.UserInfo.CustomerOrShipperID;
                }
            }

            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            if (customerIDs != null && customerIDs.Count() == 1 && customerIDs.First() == 2)
            {
                vm.StartTime = DateTime.Now.AddMonths(-4).DateTimeToString();
                vm.IsShowForHilti = true;
            }
            return View(vm);
        }

        [HttpPost]
        public string CancelAttachmentAudit(long attachmentID, long podID)
        {
            var response = new PodService().CancelAttachmentAudit(new CancenAttachmentAuditRequest() { AttachmentID = attachmentID, PodID = podID });
            if (response.IsSuccess)
            {
                return "True";
            }

            throw response.Exception;
        }

        [HttpPost]
        public string SetAttachmentRemark(long id, string remark)
        {
            var response = new PodService().SetAttachmentRemark(new SetAttachmentRemarkRequest() { ID = id, Remark = remark, AuditUser = base.UserInfo.Name });
            if (response.IsSuccess)
            {
                IList<PodReplyDocumentWithAttachment> podReplyDocumentWithAttachments = (IList<PodReplyDocumentWithAttachment>)Session["PODReplyDocuments"];
                if (podReplyDocumentWithAttachments == null)
                {
                    throw new Exception("请重新进入此页面");
                }

                var podReplyDocumentWithAttachment = podReplyDocumentWithAttachments.FirstOrDefault(p => p.AttachmentID == id);

                if (podReplyDocumentWithAttachment == null)
                {
                    throw new Exception("数据有误！");
                }

                podReplyDocumentWithAttachment.Remark = remark;

                Session["PODReplyDocuments"] = podReplyDocumentWithAttachments;

                return "True";
            }

            throw response.Exception;
        }

        private void DeleteFolder(string path)
        {
            string[] strTemp;

            //先删除该目录下的文件
            strTemp = Directory.GetFiles(path);
            foreach (string str in strTemp)
            {
                MyFile.Delete(str);
            }
            //删除子目录，递归
            strTemp = Directory.GetDirectories(path);
            foreach (string str in strTemp)
            {
                DeleteFolder(str);
            }
            //删除该目录
            Directory.Delete(path);
        }


        [HttpPost]
        public ActionResult ImageAuditDetail(ImageAuditDetailViewModel vm)
        {
            IList<PodReplyDocumentWithAttachment> podReplyDocumentWithAttachments = (IList<PodReplyDocumentWithAttachment>)Session["PODReplyDocuments"];
            if (podReplyDocumentWithAttachments == null)
            {
                throw new Exception("请重新进入此页面");
            }

            var podReplyDocumentWithAttachment = podReplyDocumentWithAttachments.FirstOrDefault(p => p.PodID == vm.PodID);

            if (podReplyDocumentWithAttachment == null)
            {
                throw new Exception("数据有误！");
            }

            Response<bool> response = null;
            if (!vm.IsOK)
            {
                response = new PodService().SetAttachmentRemark(new SetAttachmentRemarkRequest() { ID = podReplyDocumentWithAttachment.AttachmentID, Remark = vm.Remark, AuditUser = base.UserInfo.Name });

                if (response.IsSuccess)
                {
                    podReplyDocumentWithAttachment.Remark = vm.Remark;
                }
            }
            else
            {
                string systemNumber = podReplyDocumentWithAttachment.SystemNumber;
                response = new PodService().AuditPodReplyDocument(new AuditPodReplyDocumentRequest() { SystemNumbers = new List<string>() { systemNumber }, AuditUser = base.UserInfo.Name });
                if (response.IsSuccess)
                {
                    podReplyDocumentWithAttachment.IsAudit = true;
                    podReplyDocumentWithAttachment.Remark = "";
                }
            }

            if (response.IsSuccess)
            {
                Session["PODReplyDocuments"] = podReplyDocumentWithAttachments;
                long id = vm.NextPodID == 0 ? vm.PodID : vm.NextPodID;
                string message = vm.NextPodID == 0 ? "本次审核已经全部完成" : "";
                return RedirectToAction("ImageAuditDetail", new { id = id, Message = message });
            }
            else
            {
                return RedirectToAction("ImageAuditDetail", new { id = vm.PodID, Message = "审核出错！" });
            }
        }

        [HttpGet]
        public ActionResult ImageAuditDetail(long id, string Message)
        {
            ImageAuditDetailViewModel vm = new ImageAuditDetailViewModel();
            vm.URLPrev = Runbow.TWS.Common.Constants.Audit_ReplyDocument_Picture_Url_Pre;
            vm.CurrentFolder = (string)Session["AuditReplyDocumentFolder"];

            if (string.IsNullOrEmpty(vm.CurrentFolder))
            {
                Session["AuditReplyDocumentFolder"] = base.UserInfo.Name + DateTime.Now.ToString("yyyyMMddHHmmss");
                vm.CurrentFolder = (string)Session["AuditReplyDocumentFolder"];
            }

            string deletePath = Path.Combine(Runbow.TWS.Common.Constants.Audit_ReplyDocument_FOLDER_PATH, vm.CurrentFolder);
            if (Directory.Exists(deletePath))
            {
                DeleteFolder(deletePath);
            }

            vm.UserName = base.UserInfo.Name;
            vm.CurrentImageName = string.Empty;


            IList<PodReplyDocumentWithAttachment> podReplyDocumentWithAttachments = (IList<PodReplyDocumentWithAttachment>)Session["PODReplyDocuments"];

            if (podReplyDocumentWithAttachments == null)
            {
                vm.Message = "页面出错，请在父页面重新查询并打开此页面";
                return View(vm);
            }

            vm.PodID = id;

            PodReplyDocumentWithAttachment currentPodReplyDocument = null;
            PodReplyDocumentWithAttachment prevPodReplyDocument = null;
            PodReplyDocumentWithAttachment nextPodReplyDocument = null;

            podReplyDocumentWithAttachments.Each((i, p) =>
            {
                if (p.PodID == id)
                {
                    currentPodReplyDocument = p;
                    prevPodReplyDocument = (i == 0) ? null : podReplyDocumentWithAttachments.ElementAt(i - 1);
                    nextPodReplyDocument = (i == podReplyDocumentWithAttachments.Count - 1) ? null : podReplyDocumentWithAttachments.ElementAt(i + 1);
                }
            });

            if (currentPodReplyDocument == null)
            {
                vm.Message = "页面出错，请在父页面重新查询并打开此页面";
                return View(vm);
            }

            vm.IsOK = true;

            if (!string.IsNullOrEmpty(currentPodReplyDocument.Remark))
            {
                vm.IsOK = false;
            }

            vm.PodID = currentPodReplyDocument.PodID;
            vm.PrevPodID = prevPodReplyDocument == null ? 0 : prevPodReplyDocument.PodID;
            vm.NextPodID = nextPodReplyDocument == null ? 0 : nextPodReplyDocument.PodID;
            vm.Remark = currentPodReplyDocument.Remark;
            vm.PictureUrl = currentPodReplyDocument.Url;
            vm.AttachmentID = currentPodReplyDocument.AttachmentID;

            string currentPolder = Path.Combine(Runbow.TWS.Common.Constants.Audit_ReplyDocument_FOLDER_PATH, vm.CurrentFolder);

            if (Directory.Exists(currentPolder))
            {
                Directory.Delete(currentPolder, true);
            }

            Directory.CreateDirectory(currentPolder);

            string sourceFilePath = currentPodReplyDocument.Url;

            string extension = Path.GetExtension(sourceFilePath);
            string fileName = Path.GetFileName(sourceFilePath);

            if (MyFile.Exists(sourceFilePath))
            {
                if (extension.ToLower() != ".zip")
                {
                    MyFile.Copy(sourceFilePath, Path.Combine(currentPolder, fileName));
                    vm.CurrentImageName = fileName;
                    vm.CurrentImageExtension = extension;
                    vm.ImageNames = fileName;
                }
                else
                {
                    IList<string> fileNames = new List<string>();
                    if (ZipHelper.UnZip(sourceFilePath, currentPolder, fileNames))
                    {
                        vm.ImageNames = string.Empty;
                        fileNames.Each((i, f) =>
                        {
                            if (i == fileNames.Count - 1)
                            {
                                vm.ImageNames += Path.GetFileName(f);
                            }
                            else
                            {
                                vm.ImageNames = vm.ImageNames + Path.GetFileName(f) + "|";
                            }
                        });

                        if (string.IsNullOrEmpty(vm.CurrentImageName))
                        {
                            vm.CurrentImageName = Path.GetFileName(fileNames.First());
                            vm.CurrentImageExtension = Path.GetExtension(vm.CurrentImageName);

                        }
                    }
                }
            }

            var podResponse = new PodService().QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = new List<long>() { vm.PodID } });
            if (podResponse.IsSuccess)
            {
                vm.Pod = podResponse.Result.FirstOrDefault();
            }

            return View(vm);
        }

        [HttpGet]
        public ActionResult ShowImage(string ImageFolder, string ImageName)
        {
            Image image = Image.FromFile(@"D:\TWS\1\TEMP\AuditImages\HiltiAdmin\1.jpg");
            return new ImageResult()
            {
                Image = image,
                ImageFormat = ImageFormat.Jpeg
            };
        }

        [HttpPost]
        public ActionResult PODReplyDocumentAudit(PodReplyDocumentAuditViewModel vm)
        {
            vm.SearchCondition.CustomerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            vm.SearchCondition.IsInnerUser = vm.IsInnerUser;
            if (!vm.IsInnerUser && base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.ShipperID = vm.ShipperID;
            }

            if (!vm.IsForExport)
            {
                var result = new PodService().GetPodReplyDocumentWithAttachmentByCondition(new GetPodReplyDocumentWithAttachmentByConditionRequest() { SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID });

                if (result.IsSuccess)
                {
                    vm.ReplyDocumentWithAttachments = result.Result;
                    Session["PODReplyDocuments"] = result.Result.Where(r => r.IsAudit.HasValue && !r.IsAudit.Value && r.AttachmentID > 0).ToList();
                    var auditPods = result.Result.Where(p => p.IsAudit.HasValue && p.IsAudit.Value).Count();
                    var hasnotAuditPods = result.Result.Where(p => (!p.IsAudit.HasValue || (p.IsAudit.HasValue && !p.IsAudit.Value)) && string.IsNullOrEmpty(p.Remark)).Count();
                    var issuePods = result.Result.Where(p => (!p.IsAudit.HasValue || (p.IsAudit.HasValue && !p.IsAudit.Value)) && !string.IsNullOrEmpty(p.Remark)).Count();
                    if (!vm.SearchCondition.IsAudit.HasValue)
                    {
                        vm.Tip += "(" + auditPods.ToString() + "票回单通过审核, " + hasnotAuditPods.ToString() + "票回单未审核";
                        if (issuePods > 0)
                        {
                            vm.Tip += ", " + issuePods.ToString() + "票回单存在问题";
                        }
                        vm.Tip += ")";
                    }
                    else
                    {
                        if (vm.SearchCondition.IsAudit.Value)
                        {
                            vm.Tip = "(" + auditPods.ToString() + "票回单通过审核";
                            if (issuePods > 0)
                            {
                                vm.Tip += ", " + issuePods.ToString() + "票回单存在问题";
                            }
                            vm.Tip += ")";
                        }
                        else
                        {
                            vm.Tip = "(" + hasnotAuditPods.ToString() + "票回单未审核";
                            if (issuePods > 0)
                            {
                                vm.Tip += ", " + issuePods.ToString() + "票回单存在问题";
                            }
                            vm.Tip += ")";
                        }
                    }
                }


                vm.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                    .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                vm.PODTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE)
                                   .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });


                return View(vm);
            }
            else
            {
                var response = new PodService().GetExportPodReplyDocumentWhtiAttachmentByCondityin(new GetPodReplyDocumentWithAttachmentByConditionRequest() { SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID });
                if (response.IsSuccess && response.Result != null)
                {
                    return this.ExportDataTableToExcel(response.Result, "ReplyDocument.xls");
                }
                else
                {
                    vm.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                    .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                    vm.PODTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE)
                                       .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });


                    return View(vm);
                }
            }
        }

        public ActionResult AuditPodReplyDocumentAsync(string ids)
        {
            var systemNumbers = ids.FromJsonStringTo<IEnumerable<PodReplyDocument>>().Select(p => p.SystemNumber);
            var response = new PodService().AuditPodReplyDocument(new AuditPodReplyDocumentRequest() { SystemNumbers = systemNumbers, AuditUser = base.UserInfo.Name });
            if (response.IsSuccess)
            {
                var replyDocumentWithAttachments = (IList<PodReplyDocumentWithAttachment>)Session["PODReplyDocuments"];
                if (replyDocumentWithAttachments != null)
                {
                    systemNumbers.Each((i, s) =>
                    {
                        replyDocumentWithAttachments.Remove(replyDocumentWithAttachments.FirstOrDefault(t => t.SystemNumber == s));
                    });
                }

                Session["PODReplyDocuments"] = replyDocumentWithAttachments.Where(r => r.IsAudit.HasValue && !r.IsAudit.Value && r.AttachmentID > 0).ToList(); ;
                return Json(systemNumbers.Select(s => new { SystemNumber = s }));
            }

            throw response.Exception;
        }

        private mySettledConfig GenSettlementInstance(int settledType, long customerOrShipperID, long relatedCustomerID)
        {
            mySettledConfig config = new mySettledConfig();
            config.UseOld = false;
            config.IsGroupedPods = false;

            var settledConfig = ApplicationConfigHelper.GetApplicationConfig().ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).SettledConfig;

            config.SettlementInstance = settledType == 0 ? settledConfig.DefaultSelltedForReceiveInstance : settledConfig.DefaultSettledToPayInstance;

            var customerSettledConfig = settledConfig.CustomerSettledCollection.FirstOrDefault(t => t.RelatedCustomerID == relatedCustomerID);

            if (customerSettledConfig == null)
            {
                return config;
            }

            if (settledType == 0)
            {
                config.SettlementInstance = string.IsNullOrEmpty(customerSettledConfig.DefaultSettledForReceiveInstance) ? config.SettlementInstance : customerSettledConfig.DefaultSettledForReceiveInstance;
                config.UseOld = string.IsNullOrEmpty(customerSettledConfig.DefaultSettledForReceiveInstance) ? false : customerSettledConfig.UseOldReceiveMethod;
                config.IsGroupedPods = string.IsNullOrEmpty(customerSettledConfig.DefaultSettledForReceiveInstance) ? false : customerSettledConfig.IsGroupedPodsForReceive;

            }
            else
            {
                var shipperSettledConfig = customerSettledConfig.ShipperSettledCollection.FirstOrDefault(t => t.ShipperID == customerOrShipperID);

                if (shipperSettledConfig == null)
                {
                    config.SettlementInstance = string.IsNullOrEmpty(customerSettledConfig.DefaultSettledForPayInstance) ? config.SettlementInstance : customerSettledConfig.DefaultSettledForPayInstance;
                    config.UseOld = string.IsNullOrEmpty(customerSettledConfig.DefaultSettledForPayInstance) ? false : customerSettledConfig.UseOldPayMethod;
                    config.IsGroupedPods = string.IsNullOrEmpty(customerSettledConfig.DefaultSettledForPayInstance) ? false : customerSettledConfig.IsGroupedPodsForPay;
                }
                else
                {
                    config.SettlementInstance = shipperSettledConfig.SettledInstance;
                    config.UseOld = shipperSettledConfig.UseOldMethod;
                    config.IsGroupedPods = shipperSettledConfig.IsGroupedPods;
                }
            }

            return config;
        }

        private class mySettledConfig
        {
            public string SettlementInstance { get; set; }
            public bool UseOld { get; set; }
            public bool IsGroupedPods { get; set; }
        }

        private void SettingCustomerOrShipperPods(int SettledType, long customerOrShipperID, long relatedCustomerID, IEnumerable<long> podIDs, ref StringBuilder message)
        {
            mySettledConfig config = this.GenSettlementInstance(SettledType, customerOrShipperID, relatedCustomerID);

            //var settledConfig = ApplicationConfigHelper.GetApplicationConfig().ProjectCollection
            //    .First(p => p.Id == base.UserInfo.ProjectID.ToString())
            //    .SettledPodConfigs.SettledPodConfigCollection
            //    .First(s => s.TargetID == customerOrShipperID && s.Target == SettledType);

            //string implementClassName = settledConfig.InstanceName;
            //int useNew = settledConfig.UseNew;
            //bool isGroupedPods = settledConfig.IsGroupedPods == 1 ? true : false;
            if (config.UseOld)
            {
                ISettledForPod settledInstance = Activator.CreateInstance(Type.GetType(config.SettlementInstance)) as ISettledForPod;
                if (settledInstance == null)
                {
                    throw new Exception("结算实例初始化失败");
                }

                if (SettledType == 0)
                {
                    settledInstance.SettledPodForReceive(podIDs, base.UserInfo.Name, base.UserInfo.ProjectID, SettledType, customerOrShipperID, relatedCustomerID, message);
                }
                else if (SettledType == 1)
                {
                    settledInstance.SettledPodForPay(podIDs, base.UserInfo.Name, base.UserInfo.ProjectID, SettledType, customerOrShipperID, relatedCustomerID, message);
                }
            }
            else
            {
                Object[] parameters = new Object[7];
                parameters[0] = SettledType;
                parameters[1] = customerOrShipperID;
                parameters[2] = config.IsGroupedPods;
                parameters[3] = podIDs;
                parameters[4] = base.UserInfo.ProjectID;
                parameters[5] = base.UserInfo.Name;
                parameters[6] = relatedCustomerID;
                //else
                //{
                //    parameters[0] = SettledType;
                //    parameters[1] = customerOrShipperID;
                //    parameters[2] = config.IsGroupedPods;
                //    parameters[3] = podIDs;
                //    parameters[4] = base.UserInfo.ProjectID;
                //    parameters[5] = base.UserInfo.Name;
                //    parameters[6] = relatedCustomerID;
                //}

                IsettledForPodNew settledInstance = Activator.CreateInstance(Type.GetType(config.SettlementInstance), parameters) as IsettledForPodNew;

                if (settledInstance == null)
                {
                    throw new Exception("结算实例初始化失败");
                }

                message = settledInstance.SettledForPod();
            }
        }

        private ISettledForPod CreateSettledInstance(int target, long customerOrShipperID)
        {
            ISettledForPod settled = null;
            int useNew = 0;
            string implementClassName = this.GetSettledInstanceName(target, customerOrShipperID, ref useNew);
            if (!string.IsNullOrEmpty(implementClassName) && useNew == 0)
            {
                settled = Activator.CreateInstance(Type.GetType(implementClassName)) as ISettledForPod;

                if (settled == null)
                {
                    settled = new SettledDefault();
                }
            }
            else
            {
                settled = new SettledDefault();
            }

            return settled;
        }

        private IsettledForPodNew CreateSettledInstance_New(int target, long customerOrShipperID)
        {
            IsettledForPodNew settled;
            int useNew = 0;
            string implementClassName = this.GetSettledInstanceName(target, customerOrShipperID, ref useNew);
            if (!string.IsNullOrEmpty(implementClassName) && useNew == 1)
            {
                settled = Activator.CreateInstance(Type.GetType(implementClassName)) as IsettledForPodNew;

                if (settled == null)
                {
                    settled = null;
                }
            }
            else
            {
                settled = null;
            }

            return settled;
        }

        private string GetSettledInstanceName(int target, long customerOrShipperID, ref int UserNew)
        {
            var settledConfig = ApplicationConfigHelper.GetApplicationConfig().ProjectCollection
                .First(p => p.Id == base.UserInfo.ProjectID.ToString())
                .SettledPodConfigs.SettledPodConfigCollection
                .First(s => s.TargetID == customerOrShipperID && s.Target == target);

            UserNew = settledConfig.UseNew;

            return settledConfig.InstanceName;
        }


        [HttpPost]
        public ActionResult GetUserShipper(string name)
        {
            var shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID);
            return Json(shippers.Where(s => s.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ShipperID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ManualSettledPod(string ids, long shipperID, string shipperName, DateTime settledDate, decimal fee, string remark, int apportionType)
        {
            var podIDs = ids.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);
            if (podIDs == null || !podIDs.Any())
            {
                throw new Exception("数据出错！");
            }

            PodService service = new PodService();
            var podsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = podIDs });
            if (!podsResponse.IsSuccess)
            {
                throw podsResponse.Exception;
            }

            string settledNumber = Guid.NewGuid().ToString();
            IList<SettledPod> SettledPodCollection = new List<SettledPod>();
            podsResponse.Result.Each((i, pod) =>
            {
                //TODO: For Hilti Only
                #region
                if (pod.CustomerID == 2)
                {
                    try
                    {
                        pod.Weight = pod.Str24.ObjectToNullableDouble();
                    }
                    catch
                    {
                        pod.Weight = 0;
                    }
                }
                #endregion

                pod.ShipperID = shipperID;
                pod.ShipperName = shipperName;
                pod.IsSettledForShipper = true;

                SettledPod settledPod = new SettledPod()
                {
                    ProjectID = pod.ProjectID,
                    SettledNumber = settledNumber,
                    PodID = pod.ID,
                    SystemNumber = pod.SystemNumber,
                    CustomerOrderNumber = pod.CustomerOrderNumber,
                    SettledType = 1,
                    CustomerOrShipperID = pod.ShipperID,
                    CustomerOrShipperName = pod.ShipperName,
                    StartCityID = pod.StartCityID,
                    StartCityName = pod.StartCityName,
                    EndCityID = pod.EndCityID,
                    EndCityName = pod.EndCityName,
                    ShipperTypeID = pod.ShipperTypeID,
                    ShipperTypeName = pod.ShipperTypeName,
                    PODTypeID = pod.PODTypeID,
                    PODTypeName = pod.PODTypeName,
                    TtlOrTplID = pod.TtlOrTplID,
                    TtlOrTplName = pod.TtlOrTplName,
                    ActualDeliveryDate = pod.ActualDeliveryDate,
                    BoxNumber = pod.BoxNumber,
                    Weight = pod.Weight,
                    GoodsNumber = pod.GoodsNumber,
                    Volume = pod.Volume,
                    Remark = remark,
                    Creator = base.UserInfo.Name,
                    CreateTime = settledDate,
                    InvoiceID = 0,
                    RelatedCustomerID = pod.CustomerID,
                    IsAudit = false,
                    Str3 = "1"
                };

                SettledPodCollection.Add(settledPod);
            });

            if (apportionType == 1)
            {
                var totalWeight = SettledPodCollection.Sum(s => s.Weight);
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = fee * (decimal)(settledPod.Weight / totalWeight);
                });
            }
            else if (apportionType == 2)
            {
                var totalBoxNumber = SettledPodCollection.Sum(s => s.BoxNumber);
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = fee * (decimal)(settledPod.BoxNumber / totalBoxNumber);
                });
            }
            else if (apportionType == 3)
            {
                var totalGoodsNumber = SettledPodCollection.Sum(s => s.GoodsNumber);
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = fee * (decimal)(settledPod.GoodsNumber / totalGoodsNumber);
                });
            }
            else if (apportionType == 4)
            {
                var totalVolume = SettledPodCollection.Sum(s => s.Volume);
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = fee * (decimal)(settledPod.Volume / totalVolume);
                });
            }
            else
            {
                var totalCount = SettledPodCollection.Count();
                var eachFee = fee / totalCount;
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = eachFee;
                });
            }

            var response = new PodService().ManualSettledPod(new ManualSettledPodRequest()
            {
                PodIDs = podIDs,
                ShipperID = shipperID,
                ShipperName = shipperName,
                SettledPodCOllection = SettledPodCollection
            });

            if (response.IsSuccess)
            {
                return Json(new { IsSuccess = true });
            }

            throw response.Exception;
        }

        [HttpPost]
        public ActionResult ExternFeePod(string ids, int type, long shipperID, string shipperName,
                            DateTime shipperDate, long startPlaceID, string startPlaceName,
                            long endPlaceID, string endPlaceName, decimal fee, string remark, int? settledType)
        {
            var podIDs = ids.FromJsonStringTo<IEnumerable<Pod>>().Select(p => p.ID);
            if (podIDs == null || !podIDs.Any())
            {
                throw new Exception("数据出错");
            }

            PodService service = new PodService();
            var podsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = podIDs });
            if (!podsResponse.IsSuccess)
            {
                throw podsResponse.Exception;
            }
            string systemNumberSuffix = string.Empty;
            string message = string.Empty;
            switch (type)
            {
                case 1:
                    systemNumberSuffix = "-SD";
                    message = "短拨";
                    break;
                case 2:
                    systemNumberSuffix = "-DT";
                    message = "配送";
                    break;
                case 3:
                    systemNumberSuffix = "-EP";
                    message = "快递";
                    break;
                default:
                    throw new Exception("类型出错");
            }

            string settledNumber = Guid.NewGuid().ToString();
            IList<SettledPod> SettledPodCollection = new List<SettledPod>();

            podsResponse.Result.Each((i, pod) =>
            {
                //TODO: For Hilti Only
                #region
                if (pod.CustomerID == 2)
                {
                    try
                    {
                        pod.Weight = pod.Str24.ObjectToNullableDouble();
                    }
                    catch
                    {
                        pod.Weight = 0;
                    }
                }
                #endregion

                pod.SystemNumber += systemNumberSuffix;
                pod.ShipperID = shipperID;
                pod.ShipperName = shipperName;
                pod.StartCityID = startPlaceID;
                pod.EndCityID = endPlaceID;
                pod.StartCityName = startPlaceName;
                pod.EndCityName = endPlaceName;
                pod.ActualDeliveryDate = shipperDate;
                pod.Type = 1;
                pod.IsSettledForShipper = true;
                pod.PODStateID = 9;
                pod.PODStateName = "待开票";
                pod.Str36 = remark + "(" + "承运商:" + shipperName + shipperDate.DateTimeToString() + "从" + startPlaceName + "至" + endPlaceName + message + "总付费:" + fee + ")";
                pod.CreateTime = DateTime.Now;
                pod.Creator = base.UserInfo.Name;
                pod.DateTime15 = DateTime.Now;
                SettledPod settledPod = new SettledPod()
                {
                    ProjectID = pod.ProjectID,
                    SettledNumber = settledNumber,
                    PodID = 0,
                    SystemNumber = pod.SystemNumber,
                    CustomerOrderNumber = pod.CustomerOrderNumber,
                    SettledType = 1,
                    CustomerOrShipperID = pod.ShipperID,
                    CustomerOrShipperName = pod.ShipperName,
                    StartCityID = pod.StartCityID,
                    StartCityName = pod.StartCityName,
                    EndCityID = pod.EndCityID,
                    EndCityName = pod.EndCityName,
                    ShipperTypeID = pod.ShipperTypeID,
                    ShipperTypeName = pod.ShipperTypeName,
                    PODTypeID = pod.PODTypeID,
                    PODTypeName = pod.PODTypeName,
                    TtlOrTplID = pod.TtlOrTplID,
                    TtlOrTplName = pod.TtlOrTplName,
                    ActualDeliveryDate = pod.ActualDeliveryDate,
                    BoxNumber = pod.BoxNumber,
                    Weight = pod.Weight,
                    GoodsNumber = pod.GoodsNumber,
                    Volume = pod.Volume,
                    Remark = pod.Str36,
                    Creator = pod.Creator,
                    CreateTime = pod.CreateTime,
                    InvoiceID = 0,
                    RelatedCustomerID = pod.CustomerID,
                    IsAudit = false
                };

                SettledPodCollection.Add(settledPod);
            });

            if (settledType == null || settledType.Value == 1)
            {
                var totalWeight = SettledPodCollection.Sum(s => s.Weight);
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = fee * (decimal)(settledPod.Weight / totalWeight);
                });
            }
            else if (settledType.Value == 2)
            {
                var totalBoxNumber = SettledPodCollection.Sum(s => s.BoxNumber);
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = fee * (decimal)(settledPod.BoxNumber / totalBoxNumber);
                });
            }
            else if (settledType.Value == 3)
            {
                var totalGoodsNumber = SettledPodCollection.Sum(s => s.GoodsNumber);
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = fee * (decimal)(settledPod.GoodsNumber / totalGoodsNumber);
                });
            }
            else if (settledType.Value == 4)
            {
                var totalVolume = SettledPodCollection.Sum(s => s.Volume);
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = fee * (decimal)(settledPod.Volume / totalVolume);
                });
            }
            else
            {
                var totalCount = SettledPodCollection.Count();
                var eachFee = fee / totalCount;
                SettledPodCollection.Each((i, settledPod) =>
                {
                    settledPod.TotalAmt = settledPod.ShipAmt = eachFee;
                });
            }

            var response = new PodService().AddPodExtensionFee(new AddPodExtensionFeeRequest()
            {
                PodIDs = podIDs,
                PodCollection = podsResponse.Result,
                SettledPodCOllection = SettledPodCollection,
                Type = type
            });

            if (response.IsSuccess)
            {
                return Json(new { IsSuccess = true });
            }

            throw response.Exception;
        }

        private void InitAdidsaPurchaseExportData(IEnumerable<Pod> pods, out DataTable resultTable)
        {
            DataTable dt = new DataTable();
            #region 编辑表头
            dt.Columns.Add("运单号", typeof(string));
            dt.Columns.Add("阿迪达斯转运单号", typeof(string));
            dt.Columns.Add("P.O号", typeof(string));
            dt.Columns.Add("货品种类", typeof(string));
            dt.Columns.Add("生产商", typeof(string));
            dt.Columns.Add("货号", typeof(string));
            dt.Columns.Add("数量(箱)", typeof(string));
            dt.Columns.Add("提货城市", typeof(string));
            dt.Columns.Add("目的城市", typeof(string));
            dt.Columns.Add("总件数", typeof(string));
            dt.Columns.Add("总体积(立方米)", typeof(string));
            dt.Columns.Add("是否工厂提货（Y/N）", typeof(string));
            dt.Columns.Add("发货日", typeof(string));
            dt.Columns.Add("预计送达日", typeof(string));
            dt.Columns.Add("与工厂交接时备注", typeof(string));
            dt.Columns.Add("装货完成时间", typeof(string));
            dt.Columns.Add("Adidas确认日期", typeof(string));
            dt.Columns.Add("Adidas确认时间", typeof(string));
            dt.Columns.Add("组数", typeof(string));
            dt.Columns.Add("承运商", typeof(string));
            dt.Columns.Add("车牌号码", typeof(string));
            dt.Columns.Add("司机联系方式", typeof(string));
            dt.Columns.Add("第一天10点", typeof(string));
            dt.Columns.Add("第一天15点", typeof(string));
            dt.Columns.Add("第二天10点", typeof(string));
            dt.Columns.Add("第二天15点", typeof(string));
            dt.Columns.Add("第三天10点", typeof(string));
            dt.Columns.Add("第三天15点", typeof(string));
            dt.Columns.Add("实际收货日期", typeof(string));
            dt.Columns.Add("实际收货时间", typeof(string));
            dt.Columns.Add("实际到车时间", typeof(string));
            dt.Columns.Add("车辆到达是否准时", typeof(string));
            dt.Columns.Add("车况", typeof(string));
            dt.Columns.Add("服务态度", typeof(string));
            dt.Columns.Add("货物是否按要求配载", typeof(string));
            dt.Columns.Add("货物是否安全送达", typeof(string));
            dt.Columns.Add("总得分", typeof(string));
            dt.Columns.Add("Adidas", typeof(string));
            dt.Columns.Add("Runbow", typeof(string));
            dt.Columns.Add("签收情况", typeof(string));
            #endregion

            var podTracksResponse = new PodService().GetPodTracksByPodIDs(new GetPodInfoRequest()
            {
                PodIDs = pods.Select(p => p.ID)
            });

            if (!podTracksResponse.IsSuccess)
            {
                throw podTracksResponse.Exception;
            }

            var podFeadBackResponse = new PodService().GetPodFeadBacksByPodIDs(new GetPodInfoRequest()
            {
                PodIDs = pods.Select(p => p.ID)
            });

            if (!podFeadBackResponse.IsSuccess)
            {
                throw podFeadBackResponse.Exception;
            }

            pods.Each((i, p) =>
            {
                DataRow dr = dt.NewRow();
                dr[0] = p.CustomerOrderNumber;
                dr[1] = p.Str1.Trim();
                dr[2] = p.Str11;
                dr[3] = p.Str2.Trim();
                dr[4] = p.Str3.Trim();
                dr[5] = p.Str4.Trim();
                dr[6] = p.BoxNumber.HasValue ? p.BoxNumber.Value.ToString() : "";
                dr[7] = p.StartCityName;
                dr[8] = p.EndCityName;
                dr[9] = p.GoodsNumber.HasValue ? p.GoodsNumber.Value.ToString() : "";
                dr[10] = p.Volume.HasValue ? p.Volume.Value.ToString() : "";
                dr[11] = p.Str5.Trim();
                dr[12] = p.ActualDeliveryDate.DateTimeToString();
                dr[13] = p.DateTime1.HasValue ? p.DateTime1.Value.DateTimeToString() : "";
                dr[14] = p.Str36.Trim();
                dr[15] = p.Str6.Trim().IndexOf("1899/12/30") >= 0 ? p.Str6.Remove(p.Str6.Trim().IndexOf("1899/12/30"), 11) : p.Str6;
                dr[16] = p.DateTime2.HasValue ? p.DateTime2.Value.DateTimeToString() : "";
                dr[17] = p.Str7.Trim().IndexOf("1899/12/30") >= 0 ? p.Str7.Remove(p.Str7.Trim().IndexOf("1899/12/30"), 11) : p.Str7;
                dr[18] = p.Str8.Trim();
                dr[19] = p.ShipperName;
                dr[20] = p.Str9.Trim();
                dr[21] = p.Str10.Trim();

                var tracks = podTracksResponse.Result.Where(t => t.PodID == p.ID).OrderBy(t => t.DateTime1);

                if (tracks != null && tracks.Any())
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        var innerTracks = tracks.Where(t => t.DateTime1 >= DateTime.Parse(p.ActualDeliveryDate.Value.AddDays(j).DateTimeToString()) && t.DateTime1 < DateTime.Parse(p.ActualDeliveryDate.Value.AddDays(j + 1).DateTimeToString()));
                        if (innerTracks != null && innerTracks.Any())
                        {
                            if (innerTracks.Count() == 1)
                            {
                                dr[21 + j * 2 - 1] = innerTracks.First().Str1.Trim();
                                dr[21 + j * 2] = "";
                            }
                            else
                            {
                                dr[21 + j * 2 - 1] = innerTracks.First().Str1.Trim();
                                dr[21 + j * 2] = innerTracks.Last().Str1.Trim();
                            }
                        }
                    }
                }

                var feadBack = podFeadBackResponse.Result.Where(f => f.PodID == p.ID);
                if (feadBack != null && feadBack.Any())
                {
                    var innerFeadBack = feadBack.First();
                    dr[28] = innerFeadBack.DateTime1.HasValue ? innerFeadBack.DateTime1.Value.DateTimeToString() : "";
                    dr[29] = innerFeadBack.Str1.Trim();
                    dr[30] = innerFeadBack.Str2.Trim();
                    dr[31] = innerFeadBack.Str3.Trim();
                    dr[32] = innerFeadBack.Str4.Trim();
                    dr[33] = innerFeadBack.Str5.Trim();
                    dr[34] = innerFeadBack.Str6.Trim();
                    dr[35] = innerFeadBack.Str7.Trim();
                    dr[36] = innerFeadBack.Str8.Trim();
                    dr[37] = innerFeadBack.Str9.Trim();
                    dr[38] = innerFeadBack.Str10.Trim();
                }

                dt.Rows.Add(dr);
            });

            resultTable = dt;
        }

        public ActionResult Log56PodTrackImport(int? id)
        {
            return View();
        }

        [HttpPost]
        public string Log56PodTrackImport()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<Log56Track> usefulTracks = new List<Log56Track>();
                        IList<Log56Track> uselessTracks = new List<Log56Track>();
                        IList<Log56Track> allTracks = new List<Log56Track>();
                        this.InitLog56TrackData(usefulTracks, uselessTracks, allTracks, ds.Tables[0]);

                        var response = new PodService().AddLog56Tracks(new AddLog56TracksRequest()
                        {
                            UsefulTracks = usefulTracks,
                            UselessTracks = uselessTracks,
                            AllTracks = allTracks
                        });

                        if (response.IsSuccess)
                        {
                            this.UpdateLog56PhoneStatusFromLog56(allTracks);
                            StringBuilder successSB = new StringBuilder();
                            if (response.Result.TracksHaveAdded != null && response.Result.TracksHaveAdded.Any())
                            {
                                successSB.Append("<h3>导入下列运单跟踪信息成功</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("项目名称")
                                    .Append("</th><th>").Append("运单号")
                                    .Append("</th><th>").Append("跟踪时间")
                                    .Append("</th><th>").Append("当前位置")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.TracksHaveAdded)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerName)
                                        .Append("</td><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.TrackTime.DateTimeToString())
                                        .Append("</td><td>").Append(o.CurrentLocation)
                                        .Append("</td></tr>");
                                }
                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.TracksWithIssues != null && response.Result.TracksWithIssues.Any())
                            {
                                successSB.Append("<h3>由于路歌定位信息失败，下列运单跟踪不成功</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("项目名称")
                                    .Append("</th><th>").Append("运单号")
                                    .Append("</th><th>").Append("失败原因")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.TracksWithIssues)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerName)
                                        .Append("</td><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.CurrentLocation)
                                        .Append("</td></tr>");
                                }
                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.TracksNotAdded != null && response.Result.TracksNotAdded.Any())
                            {
                                successSB.Append("<h3>以下运单由于缺少随车手机信息或者手机没有在路歌平台中注册，无法跟踪</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("项目名称")
                                    .Append("</th><th>").Append("运单号")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.TracksNotAdded)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerName)
                                        .Append("</td><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td></tr>");
                                }
                                successSB.Append("</tbody></table>");
                            }

                            return new { result = successSB.ToString(), IsSuccess = true }.ToJsonString();
                        }

                        return new { result = "数据库存储出错！", IsSuccess = false }.ToJsonString();

                    }

                    return new { result = "<h3>导入运单跟踪信息失败</h3><br/>excel内容有误！", IsSuccess = false }.ToJsonString();
                }

                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }

            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }

        private void InitLog56TrackData(IList<Log56Track> usefulTracks, IList<Log56Track> uselessTracks, IList<Log56Track> allTracks, DataTable basicData)
        {
            basicData.Rows.Each((i, dr) =>
            {
                string currentLocation = dr["当前位置"].ToString().Trim();
                DateTime trackTime = string.IsNullOrEmpty(dr["定位时间"].ToString()) ? DateTime.Parse(dr["定位时间"].ToString().Trim()) : DateTime.Now;
                string phone = dr["随车手机"].ToString().Trim();
                string serviceStatus = dr["服务状态"].ToString().Trim();
                if (!string.IsNullOrEmpty(phone))
                {
                    Log56Track track = new Log56Track()
                    {
                        Phone = phone,
                        CurrentLocation = currentLocation,
                        TrackTime = trackTime,
                        Trackor = "Log56",
                        CreateTime = DateTime.Now,
                        ServiceStatus = serviceStatus
                    };

                    if (currentLocation == "手机关机" || currentLocation == "定位失败" || string.IsNullOrEmpty(currentLocation))
                    {
                        uselessTracks.Add(track);
                    }
                    else
                    {
                        usefulTracks.Add(track);
                    }

                    allTracks.Add(track);
                }
            });
        }

        private void UpdateLog56PhoneStatusFromLog56(IEnumerable<Log56Track> tracks)
        {
            var updateLog56PhoneStatusList = tracks.Where(t => t.ServiceStatus == "注册" || (t.CurrentLocation != "" && t.CurrentLocation != "定位失败" && t.CurrentLocation != "手机关机"))
                .Select(t => new Log56PhoneStatus()
                {
                    Phone = t.Phone,
                    Status = "注册"
                }).Distinct();

            new PodService().UpdateLog56PhoneStatusFromLog56(new UpdateLog56PhoneStatusRequest() { Log56PhoneStatus = updateLog56PhoneStatusList });
        }

        [HttpGet]
        public ActionResult AdidasImportBill(int? id)
        {
            return View();
        }


        [HttpPost]
        public string AdidasImportBill()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<UpdateAdidasPod> pods = new List<UpdateAdidasPod>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            pods.Add(new UpdateAdidasPod()
                            {
                                CustomerOrderNumber = ds.Tables[0].Rows[i]["Delivery number"].ToString().Trim(),
                                TS = ds.Tables[0].Rows[i]["QTY per stage"].ToString().Trim().ObjectToDouble(),
                                //Volume = ds.Tables[0].Rows[i]["Volume"].ToString().Trim().ObjectToDouble(),
                                EndCityName = ds.Tables[0].Rows[i]["City Name"].ToString().Trim(),
                                ActualDeliveryDate = ds.Tables[0].Rows[i]["Calendar day"].ToString().Trim().ObjectToDateTime(),

                            });
                        }
                        //,Volume=g.Sum(k=>k.Volume)
                        var groupedPods = from p in pods group p by new { p.CustomerOrderNumber, p.ActualDeliveryDate, p.EndCityName } into g select new UpdateAdidasPod() { CustomerID = 1, CustomerOrderNumber = g.Key.CustomerOrderNumber, TS = g.Sum(k => k.TS), EndCityName = g.Key.EndCityName, ActualDeliveryDate = g.Key.ActualDeliveryDate };
                        var response = new AKZOService().UpdateAdidasPodAndGetTheDifference(new UpdateAdidasPodAndGetTheDifferenceRequest() { PodAD = groupedPods });
                        if (response.IsSuccess)
                        {
                            StringBuilder successSB = new StringBuilder();
                            successSB.Append("<h3>共导入数据").Append(pods.Count).Append("条,合并后数据为").Append(groupedPods.Count()).Append("条.<br/>");
                            if (response.Result.UpdatedPodAD != null && response.Result.UpdatedPodAD.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.UpdatedPodAD.Count()).Append("条)更新成Excel中内容</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")

                                    .Append("</th><th>").Append("箱数")
                                    .Append("</th><th>").Append("地址")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.UpdatedPodAD)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())

                                        .Append("</td><td>").Append(o.TS.ToString())
                                     .Append("</td><td>").Append(o.EndCityName.ToString())
                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.NotUpdatedPodAD != null && response.Result.NotUpdatedPodAD.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.NotUpdatedPodAD.Count()).Append("条)没有更新成Excel中内容,系统中无对应运单</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")

                                    .Append("</th><th>").Append("箱数")

                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.NotUpdatedPodAD)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())

                                        .Append("</td><td>").Append(o.TS.ToString())

                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.CityNotMatchPodAD != null && response.Result.CityNotMatchPodAD.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.CityNotMatchPodAD.Count()).Append("条)与系统中运单目的城市不一致</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")
                                    .Append("</th><th>").Append("目的城市")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.CityNotMatchPodAD)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                                        .Append("</td><td>").Append(o.EndCityName)
                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            return new { result = successSB.ToString(), IsSuccess = true }.ToJsonString();
                        }

                        return new { result = "数据库存储出错！", IsSuccess = false }.ToJsonString();

                    }

                    return new { result = "<h3>更新运单失败</h3><br/>excel内容有误！", IsSuccess = false }.ToJsonString();
                }

                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }

            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }
        [HttpGet]
        public ActionResult AdidasPurchaseImportBill(int? id)
        {
            return View();
        }
        [HttpPost]
        public string AdidasPurchaseImportBill()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<UpdateAdidasPurchasePod> pods = new List<UpdateAdidasPurchasePod>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            pods.Add(new UpdateAdidasPurchasePod()
                            {
                                //（"客户运单号","箱数","体积","发货时间","目的城市
                                CustomerOrderNumber = ds.Tables[0].Rows[i]["客户运单号"].ToString().Trim(),
                                TS = ds.Tables[0].Rows[i]["箱数"].ToString().Trim().ObjectToDouble(),
                                Volume = ds.Tables[0].Rows[i]["体积"].ToString().Trim().ObjectToDouble(),
                                EndCityName = ds.Tables[0].Rows[i]["目的城市"].ToString().Trim(),
                                ActualDeliveryDate = ds.Tables[0].Rows[i]["发货时间"].ToString().Trim().ObjectToDateTime(),
                                str1 = ds.Tables[0].Rows[i]["转运单号"].ToString().Trim(),

                            });
                        }
                        //
                        var groupedPods = from p in pods group p by new { p.CustomerOrderNumber, p.ActualDeliveryDate, p.EndCityName, p.str1 } into g select new UpdateAdidasPurchasePod() { CustomerID = 13, CustomerOrderNumber = g.Key.CustomerOrderNumber, TS = g.Sum(k => k.TS), Volume = g.Sum(k => k.Volume), EndCityName = g.Key.EndCityName, ActualDeliveryDate = g.Key.ActualDeliveryDate, str1 = g.Key.str1 };
                        var response = new AKZOService().UpdateAdidasPurchasePodAndGetTheDifference(new UpdateAdidasPurchasePodAndGetTheDifferenceRequest() { PodAD = groupedPods });
                        if (response.IsSuccess)
                        {
                            StringBuilder successSB = new StringBuilder();
                            successSB.Append("<h3>共导入数据").Append(pods.Count).Append("条,合并后数据为").Append(groupedPods.Count()).Append("条.<br/>");
                            if (response.Result.UpdatedPodAD != null && response.Result.UpdatedPodAD.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.UpdatedPodAD.Count()).Append("条)更新成Excel中内容</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")
                                    .Append("</th><th>").Append("体积")
                                    .Append("</th><th>").Append("箱数")
                                    .Append("</th><th>").Append("目的城市")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.UpdatedPodAD)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                                           .Append("</td><td>").Append(o.Volume.ToString())
                                        .Append("</td><td>").Append(o.TS.ToString())
                                     .Append("</td><td>").Append(o.EndCityName.ToString())
                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.NotUpdatedPodAD != null && response.Result.NotUpdatedPodAD.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.NotUpdatedPodAD.Count()).Append("条)没有更新成Excel中内容,系统中无对应运单</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")
                                     .Append("</th><th>").Append("体积")
                                    .Append("</th><th>").Append("箱数")

                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.NotUpdatedPodAD)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                                          .Append("</td><td>").Append(o.Volume.ToString())
                                        .Append("</td><td>").Append(o.TS.ToString())

                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.CityNotMatchPodAD != null && response.Result.CityNotMatchPodAD.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.CityNotMatchPodAD.Count()).Append("条)与系统中运单目的城市不一致</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")
                                    .Append("</th><th>").Append("目的城市")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.CityNotMatchPodAD)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                                        .Append("</td><td>").Append(o.EndCityName)
                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            return new { result = successSB.ToString(), IsSuccess = true }.ToJsonString();
                        }

                        return new { result = "数据库存储出错！", IsSuccess = false }.ToJsonString();

                    }

                    return new { result = "<h3>更新运单失败</h3><br/>excel内容有误！", IsSuccess = false }.ToJsonString();
                }

                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }

            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }


        //add by hujiaoqiang  20150810
        [HttpPost]
        public ActionResult GetAllUsersbyUserID(string name)
        {
            var users = ApplicationConfigHelper.GetApplicationUsers();
            return Json(users.Where(s => s.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult BSPrinPOD()
        {
            QueryPodViewModel vm = new QueryPodViewModel();
            PodSearchCondition SearchCondition = new PodSearchCondition();

            string pod = Session["QueryPodViewModel"].ToString();
            SearchCondition.SystemNumber = pod;
            ViewBag.Id = pod;

            vm.SearchCondition = SearchCondition;

            vm.SearchCondition.CustomerID = 32;
            var results = new PodService().QueryBSPod(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;

            if (results.PodCollections.Count() == 0)
            {
                ViewBag.Msg = "当前只支持宝胜运单，请重新选择筛选条件！";
            }
            else
            {
                ViewBag.Msg = "仅打印宝胜运单！";
            }
            return View();
        }
        [HttpPost]
        public string BSPrinPOD(string Ids, string type)
        {
            QueryPodViewModel vm = new QueryPodViewModel();

            PodSearchCondition SearchCondition = new PodSearchCondition();
            SearchCondition.SystemNumber = Ids;
            vm.SearchCondition = SearchCondition;

            vm.SearchCondition.CustomerID = 32;
            var results = new PodService().QueryBSPod(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);

            return jsonStr;


        }
    }
}



