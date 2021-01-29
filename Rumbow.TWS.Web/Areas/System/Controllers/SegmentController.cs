using System;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.System.Models;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class SegmentController : BaseController
    {
        public ActionResult Create()
        {
            CreateSegmentViewModel vm = new CreateSegmentViewModel();
            vm.State = true;
            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(CreateSegmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Segment segment = new Segment() { Name = model.Name, Description = model.Description, State = model.State, Creator = base.UserInfo.Name, CreateTime = DateTime.Now, Str1 = model.Str1, Str2 = model.Str2, Str3 = model.Str3 };
                var response = new SegmentService().AddSegment(new AddSegmentRequest() { Segment = segment });
                if (response.IsSuccess)
                {
                    return RedirectToAction("CreateDetail", new { segmentID = response.Result });
                }
                else
                {
                    if (response.Result == -1)
                    {
                        ViewBag.Message = "已存在相同名称的段位";
                    }
                    else
                    {
                        ViewBag.Message = "新增段位失败！.";
                    }
                }
            }

            return View(model);
        }

        public ActionResult CreateDetail(long segmentID)
        {
            var response = new SegmentService().GetSegmentAndDetail(new SegmentRequest { ID = segmentID });
            CreateSegmentDetailViewModel vm = new CreateSegmentDetailViewModel();
            if (response.IsSuccess)
            {
                vm.Segment = response.Result.Segment;
                vm.SegmentDetailCollection = response.Result.SegmentDetailCollection;
            }
            else
            {
                ViewBag.Message = "获取段位信息失败！";
            }

            return View(vm);
        }

        [HttpPost]
        public JsonResult CreateDetail(long segmentID, float startVal, float endVal, string description)
        {
            var response = new SegmentService().AddSegmentDetail(new SegmentDetailRequest()
            {
                SegmentDetail = new SegmentDetail()
                {
                    SegmentID = segmentID,
                    StartVal = startVal,
                    EndVal = endVal,
                    Description = description,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now
                }
            });

            if (response.IsSuccess)
            {
                return Json(new { ID = response.Result });
            }
            else
            {
                if (response.Result == -1)
                {
                    return Json(new { ID = 0, ErrorMessage = "已存在此段位" });
                }

                return Json(new { ID = 0, ErrorMessage = "新增段位失败！" });
            }
        }

        [HttpPost]
        public JsonResult EditDetail(long detailID, long segmentID, long startVal, long endVal, string description)
        {
            var response = new SegmentService().UpdateSegmentDetail(new SegmentDetailRequest()
            {
                SegmentDetail = new SegmentDetail()
                {
                    ID = detailID,
                    SegmentID = segmentID,
                    StartVal = startVal,
                    EndVal = endVal,
                    Description = description,
                }
            });

            if (response.IsSuccess)
            {
                return Json(new { ID = response.Result });
            }
            else
            {
                if (response.Result == -1)
                {
                    return Json(new { ID = 0, ErrorMessage = "已存在此段位" });
                }

                return Json(new { ID = 0, ErrorMessage = "编辑段位失败！" });
            }
        }

        public JsonResult DeleteSegmentDetail(long detailID)
        {
            var response = new SegmentService().DeleteSegmentDetail(new SegmentRequest()
            {
                ID = detailID
            });

            if (response.IsSuccess)
            {
                return Json(new { ID = response.Result });
            }
            else
            {
                if (response.Result == -1)
                {
                    return Json(new { ID = 0, ErrorMessage = "此段位已使用，无法删除" });
                }

                return Json(new { ID = 0, ErrorMessage = "删除段位失败！" });
            }
        }

        public ActionResult List()
        {
            SegmentListViewModel vm = new SegmentListViewModel();
            var response = new SegmentService().GetSegmentsByCondition(new GetSegmentsByConditionRequest() { Name = "", State = true });
            if (response.IsSuccess)
            {
                vm.Segments = response.Result;
                vm.State = true;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult List(SegmentListViewModel vm)
        {
            var response = new SegmentService().GetSegmentsByCondition(new GetSegmentsByConditionRequest() { Name = string.IsNullOrEmpty(vm.Name) ? "" : vm.Name, State = vm.State });
            if (response.IsSuccess)
            {
                vm.Segments = response.Result;
            }
            else
            {
                ViewBag.Message = "查询失败";
            }

            return View(vm);
        }

        public ActionResult DelOrReuseSegment(long segmentID, bool state)
        {
            var response = new SegmentService().SetSegmentState(new SetSegmentStateRequest() { ID = segmentID, State = !state });
            if (!response.IsSuccess)
            {
                string msg = state ? "禁用" : "启用";
                ViewBag.Message = msg + "段位失败！";
            }

            return RedirectToAction("List");
        }
    }
}