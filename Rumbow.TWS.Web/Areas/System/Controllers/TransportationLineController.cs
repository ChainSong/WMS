using System;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.System.Models;
using Runbow.TWS.Web.Common;
using UtilConstants = Runbow.TWS.Common.Constants;

namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class TransportationLineController : BaseController
    {
        public ActionResult Create(string message)
        {
            CreateTransportationLineViewModel vm = new CreateTransportationLineViewModel();
            vm.State = true;
            ViewBag.Message = message;
            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(CreateTransportationLineViewModel model)
        {
            if (ModelState.IsValid)
            {
                TransportationLine transportationLine = new TransportationLine()
                {
                    Name = model.Name,
                    StartCityID = model.StartCityID.Value,
                    StartCityName = model.StartCityName,
                    EndCityID = model.EndCityID.Value,
                    EndCityName = model.EndCityName,
                    Distance = model.Distance,
                    State = model.State,
                    Remark = model.Remark,
                    CreateTime = DateTime.Now,
                    Creator = base.UserInfo.Name,
                    Str1 = model.Str1,
                    Str2 = model.Str2,
                    Str3 = model.Str3
                };

                var response = new TransportationLineService().AddTransportationLine(new AddTransportationLineRequest() { TransportationLine = transportationLine });
                if (response.IsSuccess)
                {
                    ApplicationConfigHelper.RefreshTransportationLines();
                    return RedirectToAction("Create", new { message = "新增线路成功，继续增加或者返回" });
                }
                else
                {
                    if (response.Result == -1)
                    {
                        ViewBag.Message = "已存在此路线";
                    }
                    else
                    {
                        ViewBag.Message = "新增路线失败！.";
                    }
                }
            }

            return View(model);
        }

        public ActionResult List()
        {
            TransportationLineListViewModel vm = new TransportationLineListViewModel();
            var response = new TransportationLineService().GetTransportationLinesByConditon(new GetTransportationLinesByConditionRequest() { Name = "", State = true, PageSize = UtilConstants.PAGESIZE, PageIndex = 0 });
            if (response.IsSuccess)
            {
                vm.TransportationLineCollection = response.Result.TransportationLines;
                vm.State = true;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult List(TransportationLineListViewModel vm, int? PageIndex)
        {
            var response = new TransportationLineService().GetTransportationLinesByConditon(new GetTransportationLinesByConditionRequest() { Name = vm.Name, State = vm.State, StartCityID = vm.StartCityID, EndCityID = vm.EndCityID, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 });
            if (response.IsSuccess)
            {
                vm.TransportationLineCollection = response.Result.TransportationLines;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
            }
            else
            {
                ViewBag.Message = "查询失败";
            }

            return View(vm);
        }

        public JsonResult DelOrReuse(long id, bool state)
        {
            var response = new TransportationLineService().SetTransportationLineState(new SetTransportationLineStateRequest()
            {
                ID = id,
                State = !state
            });

            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshTransportationLines();
                return Json(new { ID = id });
            }
            else
            {
                string msg = state ? "禁用" : "启用";
                return Json(new { ID = 0, ErrorMessage = msg + "失败！" });
            }
        }
    }
}