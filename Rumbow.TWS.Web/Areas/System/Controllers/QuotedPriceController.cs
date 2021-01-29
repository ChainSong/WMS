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
using Runbow.TWS.Entity.System;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.System.Models;
using Runbow.TWS.Web.Common;
using MyFile = System.IO.File;
using WebConstants = Runbow.TWS.Web.Common.Constants;

namespace Runbow.TWS.Web.Areas.System.Controllers
{
    public class QuotedPriceController : BaseController
    {
        public ActionResult Create(int id, string message)
        {
            QuotedPriceViewModel vm = new QuotedPriceViewModel();
            vm.Target = id;
            if (vm.Target == 0)
            {
                vm.CustomerOrShippers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else
            {
                vm.CustomerOrShippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            }

            if (vm.CustomerOrShippers == null || !vm.CustomerOrShippers.Any())
            {
                return Error("请先关联项目" + (id == 0 ? "客户" : "承运商"));
            }

            var transportationLines = ApplicationConfigHelper.GetTransportationLine();
            if (!transportationLines.Any())
            {
                return Error("请先设置系统运输线路");
            }
            else
            {
                vm.TransportationLines = transportationLines.Select(t => new SelectListItem() { Value = t.ID.ToString() + "|" + t.StartCityID.ToString() + "|" + t.StartCityName + "|" + t.EndCityID.ToString() + "|" + t.EndCityName, Text = t.StartCityName + "-" + t.EndCityName });
            }

            vm.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.PodTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.TplOrTtl = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.TTLORTPL)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.RelatedCustomers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            vm.SettedConfigs = string.Empty;

            ViewBag.Message = message;

            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(QuotedPriceViewModel vm)
        {
            vm.QuotedPriceDetails = vm.SettedConfigs.FromJsonStringTo<IEnumerable<QuotedPriceDetail>>();
            vm.RelatedCustomerID = vm.Target == 0 ? vm.TargetID : vm.RelatedCustomerID;
            vm.RelatedCustomerName = vm.TargetID == 0 ? vm.TargetName : vm.RelatedCustomerName;

            AddQuotedPriceRequest request = new AddQuotedPriceRequest()
            {
                QuotedPrices = vm.QuotedPriceDetails.Select(q => new QuotedPrice()
                {
                    ProjectID = base.UserInfo.ProjectID,
                    ProjectName = base.UserInfo.ProjectName,
                    Target = vm.Target,
                    TargetID = vm.TargetID,
                    TargetName = vm.TargetName,
                    ShipperTypeID = vm.ShipperTypeID,
                    ShipperTypeName = vm.ShipperTypeName,
                    PodTypeID = vm.PodTypeID,
                    PodTypeName = vm.PodTypeName,
                    TplOrTtlID = vm.TplOrTtlID,
                    TplOrTtlName = vm.TplOrTtlName,
                    TransportationLineID = vm.TransportationLineID,
                    StartCityID = vm.StartCityID,
                    StartCityName = vm.StartCityName,
                    EndCityID = vm.EndCityID,
                    EndCityName = vm.EndCityName,
                    SegmentDetailID = q.SegmentDetailID,
                    StartVal = q.StartVal,
                    EndVal = q.EndVal,
                    MinPrice = vm.MinPrice ?? 0,
                    Point = vm.Point ?? 0,
                    Price = q.Price,
                    EffectiveStartTime = vm.EffectiveStartTime,
                    EffectiveEndTime = vm.EffectiveEndTime,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now,
                    Remark = vm.Remark,
                    State = true,
                    RelatedCustomerID = vm.RelatedCustomerID,
                    RelatedCustomerName = vm.RelatedCustomerName,
                    EmptyCarryPrice = vm.EmptyCarryPrice ?? 0,
                    Decimal1 = vm.Decimal1 ?? 0,
                    Decimal2 = vm.Decimal2 ?? 0
                })
            };

            var response = new QuotedPriceService().AddQuotedPrice(request);

            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshProjectQuotedPrice(base.UserInfo.ProjectID, vm.Target, vm.TargetID, vm.RelatedCustomerID);
                return RedirectToAction("Create", new { id = vm.Target, message = "新增报价成功，继续增加" });
            }

            return Error("新增" + (vm.Target == 0 ? "客户" : "承运商") + "报价失败！");
        }
        [HttpGet]
        public ActionResult ExeclQuotedPrice(int? id)
        {
            return View();
        }
        [HttpPost]
        public string ExeclQuotedPrice()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<QuotedPrice> Info = new List<QuotedPrice>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Info.Add(new QuotedPrice()
                            {
                                ProjectID = base.UserInfo.ProjectID,
                                ProjectName = base.UserInfo.ProjectName,
                                Target = Convert.ToInt32(ds.Tables[0].Rows[i]["Target"]),
                                TargetID = Convert.ToInt32(ds.Tables[0].Rows[i]["TargetID"]),
                                TargetName = ds.Tables[0].Rows[i]["TargetName"].ToString(),
                                ShipperTypeID = 0,
                                ShipperTypeName = ds.Tables[0].Rows[i]["ShipperTypeName"].ToString(),
                                PodTypeID = 0,
                                PodTypeName = ds.Tables[0].Rows[i]["PodTypeName"].ToString(),
                                TplOrTtlID = 0,
                                TplOrTtlName = ds.Tables[0].Rows[i]["TplOrTtlName"].ToString(),
                                TransportationLineID = 0,
                                StartCityID = 0,
                                StartCityName = ds.Tables[0].Rows[i]["StartCityName"].ToString(),
                                EndCityID = 0,
                                EndCityName = ds.Tables[0].Rows[i]["EndCityName"].ToString(),
                                SegmentDetailID = 0,
                                StartVal = Convert.ToInt32(ds.Tables[0].Rows[i]["StartVal"]),
                                EndVal = Convert.ToInt32(ds.Tables[0].Rows[i]["EndVal"]),
                                //hjq
                                MinPrice = 0,
                                Point = 0,// Convert.ToDecimal(ds.Tables[0].Rows[i]["Point"]),
                                Price = Convert.ToDecimal(ds.Tables[0].Rows[i]["Price"]),
                                EffectiveStartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EffectiveStartTime"]),
                                EffectiveEndTime = null,
                                Creator = base.UserInfo.Name,
                                CreateTime = DateTime.Now,
                                Remark = "",
                                State = true,
                                RelatedCustomerID = Convert.ToInt32(ds.Tables[0].Rows[i]["RelatedCustomerID"]),
                                RelatedCustomerName = ds.Tables[0].Rows[i]["RelatedCustomerName"].ToString(),
                                //hjq
                                EmptyCarryPrice = 0,
                                Decimal1 = 0,
                                Decimal2 = 0
                            });
                        }
                        AddQuotedPriceRequest request = new AddQuotedPriceRequest()
                        {
                            QuotedPrices = Info.Select(q => new QuotedPrice()
                            {
                                ProjectID = base.UserInfo.ProjectID,
                                ProjectName = base.UserInfo.ProjectName,
                                Target = q.Target,
                                TargetID = q.TargetID,
                                TargetName = q.TargetName,
                                ShipperTypeID = q.ShipperTypeID,
                                ShipperTypeName = q.ShipperTypeName,
                                PodTypeID = q.PodTypeID,
                                PodTypeName = q.PodTypeName,
                                TplOrTtlID = q.TplOrTtlID,
                                TplOrTtlName = q.TplOrTtlName,
                                TransportationLineID = q.TransportationLineID,
                                StartCityID = q.StartCityID,
                                StartCityName = q.StartCityName,
                                EndCityID = q.EndCityID,
                                EndCityName = q.EndCityName,
                                SegmentDetailID = q.SegmentDetailID,
                                StartVal = q.StartVal,
                                EndVal = q.EndVal,
                                MinPrice = q.MinPrice ?? 0,
                                Point = q.Point ?? 0,
                                Price = q.Price,
                                EffectiveStartTime = q.EffectiveStartTime,
                                EffectiveEndTime = q.EffectiveEndTime,
                                Creator = base.UserInfo.Name,
                                CreateTime = DateTime.Now,
                                Remark = q.Remark,
                                State = true,
                                RelatedCustomerID = q.RelatedCustomerID,
                                RelatedCustomerName = q.RelatedCustomerName,
                                EmptyCarryPrice = q.EmptyCarryPrice ?? 0,
                                Decimal1 = q.Decimal1 ?? 0,
                                Decimal2 = q.Decimal2 ?? 0
                            })
                        };
                        // var groupedInfos = from g in Info select new QuotedPrice() { ID = g.ID, Remark = g.Remark };
                        var response = new QuotedPriceService().AddQuotedPrice(request);

                        if (response.IsSuccess)
                        {
                            return new { result = "导入成功！", IsSuccess = true }.ToJsonString();
                        }
                    }

                    return new { result = "<h3>导入失败!</h3><br/>excel内容有误！", IsSuccess = false }.ToString();
                }

                return new { result = "文件内容为空", IsSuccess = false }.ToString();
            }

            return new { result = "请选择文件", IsSuccess = false }.ToString();
        }

        [HttpGet]
        public ActionResult ExeclQuotedPrices(int? id)
        {
            return View();
        }
        [HttpPost]
        public string ExeclQuotedPrices()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<QuotedPrices> qprice = new List<QuotedPrices>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            qprice.Add(new QuotedPrices()
                                {
                                    ProjectID = base.UserInfo.ProjectID,
                                    ProjectName = base.UserInfo.ProjectName,
                                    Target = Convert.ToInt32(ds.Tables[0].Rows[i]["Target"].ToString()),
                                    TargetName = ds.Tables[0].Rows[i]["TargetName"].ToString(),
                                    StartCityName = ds.Tables[0].Rows[i]["StartCityName"].ToString(),
                                    EndCityName = ds.Tables[0].Rows[i]["EndCityName"].ToString(),
                                    PodTypeName = ds.Tables[0].Rows[i]["PodTypeName"].ToString(),
                                    ShipperTypeName = ds.Tables[0].Rows[i]["ShipperTypeName"].ToString(),
                                    P200 = Convert.ToDecimal(ds.Tables[0].Rows[i]["P200"].ToString()),
                                    P500 = Convert.ToDecimal(ds.Tables[0].Rows[i]["P500"].ToString()),
                                    P1000 = Convert.ToDecimal(ds.Tables[0].Rows[i]["P1000"].ToString()),
                                    P2000 = Convert.ToDecimal(ds.Tables[0].Rows[i]["P2000"].ToString()),
                                    P5000 = Convert.ToDecimal(ds.Tables[0].Rows[i]["P5000"].ToString()),
                                    P10000 = Convert.ToDecimal(ds.Tables[0].Rows[i]["P10000"].ToString()),
                                    P20000 = Convert.ToDecimal(ds.Tables[0].Rows[i]["P20000"].ToString()),
                                    P30000 = Convert.ToDecimal(ds.Tables[0].Rows[i]["P30000"].ToString()),
                                    P99999 = Convert.ToDecimal(ds.Tables[0].Rows[i]["P99999"].ToString()),
                                    RelatedCustomerName = ds.Tables[0].Rows[i]["RelatedCustomerName"].ToString()
                                });
                        }
                        AddQuotedPriceRequest request = new AddQuotedPriceRequest()
                        {
                            QuotedPrices_New = qprice.Select(q => new QuotedPrices()
                            {
                                ProjectID = base.UserInfo.ProjectID,
                                ProjectName = base.UserInfo.ProjectName,
                                Target = q.Target,
                                TargetName = q.TargetName,
                                StartCityName = q.StartCityName,
                                EndCityName = q.EndCityName,
                                PodTypeName = q.PodTypeName,
                                ShipperTypeName = q.ShipperTypeName,
                                P200 = q.P200 ?? 0,
                                P500 = q.P500 ?? 0,
                                P1000 = q.P1000 ?? 0,
                                P2000 = q.P2000 ?? 0,
                                P5000 = q.P5000 ?? 0,
                                P10000 = q.P10000 ?? 0,
                                P20000 = q.P20000 ?? 0,
                                P30000 = q.P30000 ?? 0,
                                P99999 = q.P99999 ?? 0,
                                RelatedCustomerName = q.RelatedCustomerName
                            })
                        };
                        var response = new QuotedPriceService().AddQuotedPrices(request);
                        if (response.IsSuccess)
                        {
                            return new { result = "导入成功", IsSuccess = true }.ToJsonString();
                        }
                    }

                    return new { result = "<h3>导入失败!</h3><br/>excel内容有误！", IsSuccess = false }.ToString();
                }

                return new { result = "文件内容为空", IsSuccess = false }.ToString();
            }

            return new { result = "请选择文件", IsSuccess = false }.ToString();
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

        public ActionResult List(int id)
        {
            QuotedPriceViewModel vm = new QuotedPriceViewModel();
            vm.Target = id;
            if (vm.Target == 0)
            {
                //vm.CustomerOrShippers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.Name });
                vm.CustomerOrShippers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else
            {
                vm.CustomerOrShippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            }

            if (vm.CustomerOrShippers == null || !vm.CustomerOrShippers.Any())
            {
                return Error("请先关联项目" + (id == 0 ? "客户" : "承运商"));
            }

            var transportationLines = ApplicationConfigHelper.GetTransportationLine();
            if (!transportationLines.Any())
            {
                return Error("请先设置系统运输线路");
            }
            else
            {
                vm.TransportationLines = transportationLines.Select(t => new SelectListItem() { Value = t.ID.ToString() + "|" + t.StartCityID.ToString() + "|" + t.StartCityName + "|" + t.EndCityID.ToString() + "|" + t.EndCityName, Text = t.StartCityName + "-" + t.EndCityName });
            }


            vm.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.PodTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.TplOrTtl = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.TTLORTPL)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.RelatedCustomers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            return View(vm);
        }

        [HttpPost]
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSegmentDetailByTargetID(int target, long targetID, long relatedCustomerID)
        {
            var response = new QuotedPriceService().GetSegmentDetailByCustomerOrShipper(new GetSegmentDetailByCustomerOrShipperRequest() { ProjectID = base.UserInfo.ProjectID, Target = target, CustomerOrShipperID = targetID, RelatedCustomerID = relatedCustomerID });
            if (response.IsSuccess && response.Result != null)
            {
                return Json(response.Result.Select(r => new { SegmentDetailID = r.ID, StartVal = r.StartVal, EndVal = r.EndVal }));
            }

            return Json(string.Empty);
        }

        [HttpPost]
        public JsonResult DeleteQuetedPrice(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Json(new { ReturnVal = -1, Message = "传入数据有误！" });
            }

            var response = new QuotedPriceService().DeleteQuetedPrice(new DeleteQuetedPriceRequest() { QutedPriceIDs = ids.Split(',').Select(id => id.ObjectToInt64()) });
            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshProjectQuotedPrice(response.Result.ProjectID, response.Result.Target, response.Result.TargetID, response.Result.Target == 0 ? response.Result.TargetID : response.Result.RelatedCustomerID.Value);

                return Json(new { ReturnVal = 1, Message = "删除成功" });
            }

            return Json(new { ReturnVal = -2, Message = "删除失败！" });
        }

        [HttpPost]
        public JsonResult GetHistoryQuetedPrice(int target, long? targetID, long? transportationLineID, long? shipperTypeID, long? podTypeID, long? tplOrTtlID, DateTime? effectiveStartTime, DateTime? effectiveEndTime, long? startCityID, long? endCityID, long? relatedCustomerID)
        {
            var response = new QuotedPriceService().GetQuotedPriceByCondition(new GetQuotedPriceByConditionRequest()
            {
                ProjectID = base.UserInfo.ProjectID,
                Target = target,
                CustomerOrShipperID = targetID,
                TransportationLineID = transportationLineID,
                ShipperTypeID = shipperTypeID,
                PodTypeID = podTypeID,
                TtlOrTplID = tplOrTtlID,
                EffectiveStartTime = effectiveStartTime,
                EffectiveEndTime = effectiveEndTime,
                StartCityID = startCityID,
                EndCityID = endCityID,
                RelatedCustomerID = relatedCustomerID
            });

            if (response.IsSuccess && response.Result != null)
            {
                var result = from q in response.Result
                             group q by new
                             {
                                 q.TargetName,
                                 q.RelatedCustomerName,
                                 q.StartCityName,
                                 q.EndCityName,
                                 q.ShipperTypeName,
                                 q.PodTypeName,
                                 q.TplOrTtlName,
                                 q.EffectiveStartTime,
                                 q.EffectiveEndTime
                             } into g
                             select new { g.Key, g };

                return Json(result.OrderBy(p => p.Key.RelatedCustomerName).OrderBy(p => p.Key.StartCityName).OrderBy(p => p.Key.EndCityName).OrderBy(p => p.Key.ShipperTypeName)
                    .OrderBy(p => p.Key.PodTypeName).OrderBy(p => p.Key.TplOrTtlName).OrderBy(p => p.Key.EffectiveStartTime).Select(p => new
                {
                    TargetName = p.Key.TargetName,
                    RelatedCustomerName = p.Key.RelatedCustomerName,
                    TransportationLine = p.Key.StartCityName + "-" + p.Key.EndCityName,
                    ShipperType = p.Key.ShipperTypeName,
                    PodType = p.Key.PodTypeName,
                    EffectiveTime = p.Key.EffectiveStartTime.DateTimeToString() + "~" + p.Key.EffectiveEndTime.DateTimeToString(),
                    TtlOrTpl = p.Key.TplOrTtlName,
                    QuotedPrice = GenQuotedPriceString(p.g),
                    IDs = p.g.Select(c => c.ID)
                }));
            }

            return Json(string.Empty);
        }

        private string GenQuotedPriceString(IEnumerable<QuotedPrice> prices)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var p in prices)
            {
                sb.Append("[");
                sb.Append(p.StartVal);
                sb.Append("~");
                sb.Append(p.EndVal);
                sb.Append(")");
                sb.Append("￥" + p.Price + " ");
            }

            sb.Append("点费:￥" + prices.First().Point);
            sb.Append("最低收费:￥" + prices.First().MinPrice);
            sb.Append("空放费:￥" + (prices.First().EmptyCarryPrice ?? 0));
            sb.Append("备注:" + prices.First().Remark);
            return sb.ToString();
        }

        [HttpPost]
        public ActionResult GetTransPortationLines(string name)
        {
            var transportationLines = ApplicationConfigHelper.GetTransportationLine();
            var temp1 = transportationLines.Where(t => t.Name.IndexOf(name) >= 0);
            var temp2 = temp1.Select(t => new { Value = t.ID.ToString() + "|" + t.StartCityID.ToString() + "|" + t.StartCityName + "|" + t.EndCityID.ToString() + "|" + t.EndCityName, Text = t.StartCityName + "-" + t.EndCityName });
            return Json(transportationLines.Where(t => t.Name.IndexOf(name) >= 0).Select(t => new { Value = t.ID.ToString() + "|" + t.StartCityID.ToString() + "|" + t.StartCityName + "|" + t.EndCityID.ToString() + "|" + t.EndCityName, Text = t.StartCityName + "-" + t.EndCityName }), JsonRequestBehavior.AllowGet);
        }
    }
}