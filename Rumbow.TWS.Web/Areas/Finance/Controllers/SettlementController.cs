using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.Finance.Models;
using Runbow.TWS.Web.Common;
using UtilConstants = Runbow.TWS.Common.Constants;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using Runbow.TWS.Common;
using System.Data;
using System.IO;
using MyFile = System.IO.File;
using System.Text;

namespace Runbow.TWS.Web.Areas.Finance.Controllers
{
    public class SettlementController : BaseController
    {
        public ActionResult SettledPodManage(int? SettledType, bool? ShowActionButton, bool? ShowSelectCheckBox, bool? IsForAudit, int? AuditType, bool? FinalAudit, bool? IsBatchAdjust)
        {
            SettledPodManageViewModel vm = new SettledPodManageViewModel();
            vm.SearchCondition = new SettledPodSearchCondition();
            vm.SearchCondition.ProjectID = base.UserInfo.ProjectID;
            this.GenQuerySettledPodViewModel(vm);
            vm.IsBatchAdjust = false;
            vm.SearchCondition.ActualDeliveryDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            vm.SearchCondition.EndActualDeliveryDate = vm.SearchCondition.ActualDeliveryDate.Value.AddMonths(1).AddDays(-1);
            vm.IsInnerUser = base.UserInfo.UserType == 2;
            vm.ProjectRoleID = base.UserInfo.ProjectRoleID;
            vm.SearchCondition.SettledType = SettledType ?? 0;
            vm.IsInvoiced = 0;
            vm.SearchCondition.IsForAudit = false;
            vm.Name = base.UserInfo.Name;
            vm.SearchCondition.IsAudit = true;
            vm.ShowActionButton = ShowActionButton.HasValue ? ShowActionButton.Value && vm.IsInvoiced == 0 : false;
            vm.ShowSelectCheckBox = ShowSelectCheckBox.HasValue ? ShowSelectCheckBox.Value && vm.IsInvoiced == 0 : false;
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            if (!vm.IsInnerUser)
            {
                vm.SearchCondition.CustomerOrShipperID = base.UserInfo.CustomerOrShipperID;
            }

            if (IsForAudit != null && IsForAudit.Value)
            {
                vm.SearchCondition.IsForAudit = true;
                vm.IsForAudit = true;
                vm.ShowActionButton = false;
                vm.ShowSelectCheckBox = true;
                vm.SearchCondition.SettledType = 1;
                vm.AuditType = AuditType ?? 1;
                vm.SearchCondition.IsAudit = false;
                if (AuditType == null || AuditType.Value == 1)
                {
                    vm.SearchCondition.SystemNumberSufixx = "-SD";
                }
                else if (AuditType.Value == 2)
                {
                    vm.SearchCondition.SystemNumberSufixx = "-DT";
                }
                else if (AuditType.Value == 3)
                {
                    vm.SearchCondition.SystemNumberSufixx = "-EP";
                }
                else if (AuditType == 4)
                {
                    vm.SearchCondition.IsManualSettled = true;
                }
                else
                {
                    vm.SearchCondition.SystemNumberSufixx = "-NULL";
                }
                vm.IsInvoiced = 0;

                vm.FinalAudit = FinalAudit ?? false;
            }

            if (IsBatchAdjust != null && IsBatchAdjust.Value)
            {
                vm.IsBatchAdjust = true;
                vm.IsForAudit = false;
                vm.ShowActionButton = false;
                vm.ShowSelectCheckBox = false;
                vm.IsInvoiced = 0;

            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult ExportSettledPod()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dt.Rows.Add(dr);
            ExcelHelper excelHelper = new ExcelHelper();
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
            string fileFullPath = Path.Combine(targetPath, "ExportSettledPods.xlsx");
            excelHelper.CreateExcelByDataTable(fileFullPath, dt);
            excelHelper.Dispose();
            string mimeType = "application/msexcel";
            FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
            return File(fs, mimeType, "ExportPods.xlsx");
        }

        [HttpPost]
        public ActionResult SettledPodManage(SettledPodManageViewModel vm)
        {
            if (vm.SearchCondition.UserType == 2)
            {
                vm.SearchCondition.CustomerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            }

            if (vm.IsInvoiced == 1)
            {
                vm.SearchCondition.InvoiceID = 1;
            }

            vm.ShowActionButton = vm.ShowActionButton && vm.IsInvoiced == 0;
            vm.ShowSelectCheckBox = vm.ShowSelectCheckBox && vm.IsInvoiced == 0;

            this.GenQuerySettledPodViewModel(vm);

            var response = new SettledService().GetSettledPodsByCondition(new GetSettledPodsByConditionRequest() { SearchCondition = vm.SearchCondition });

            if (response.IsSuccess && vm.IsExport)
            {
                DataTable dt = this.InitExportTable(response.Result);
                //ExcelHelper excelHelper = new ExcelHelper();
                //string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
                //string fileFullPath = Path.Combine(targetPath, "ExportSettledPods_" + base.UserInfo.Name + ".xlsx");
                //excelHelper.CreateExcelByDataTable(fileFullPath, dt);
                //excelHelper.Dispose();
                //string mimeType = "application/msexcel";
                //FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
                //return File(fs, mimeType, "ExportSettledPods.xlsx");
                return ExportDataTableToExcel(dt, "ExportSettledPods.xls");
            }

            if (response.IsSuccess)
            {
                vm.SettledPods = response.Result;
                if (vm.IsForAudit && vm.SettledPods != null && vm.SettledPods.Any())
                {
                    var auditResponse = new SettledService().GetSettledHistoryBySettledPodIDs(new GetSettledHistoryBySettledPodIDsRequest() { SettledPodIDs = response.Result.Select(i => i.ID) });
                    if (auditResponse.IsSuccess)
                    {
                        vm.SettledPodAuditHistoryCollection = auditResponse.Result;
                    }
                    else
                    {
                        vm.Message = "查询有误！";
                    }
                }
            }
            else
            {
                vm.Message = "查询有误";
            }


            return View(vm);
        }

        [HttpGet]
        public ActionResult EditSettledPod(long id)
        {
            var response = new SettledService().GetSettledPodByID(new GetSettledPodByIDRequest() { ID = id });
            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            return Error("程序出错");
        }

        public ActionResult ImportForBatchEditSettledPod(int EditType)
        {
            ViewBag.DisplyMessage = (EditType == 0 ? "导入运单应收费用调整" : "导入运单应付费用调整") + "(已开票结算无法调整)";
            ViewBag.Type = EditType;
            IEnumerable<SelectListItem> customers = new List<SelectListItem>();
            if (base.UserInfo.UserType == 2)
            {
                customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else
            {
                customers = Enumerable.Empty<SelectListItem>();
            }

            ViewBag.Customers = customers;

            return View();
        }

        [HttpPost]
        public string ImportForBatchEditSettledPod(int type, long customer)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null && ds.Tables[0] != null)
                    {
                        IEnumerable<SettledPod> settledPods = ds.Tables[0].Rows.Select((i, dr) => new SettledPod()
                        {
                            SystemNumber = dr["系统编号"].ToString(),
                            CustomerOrderNumber = dr["客户运单号"].ToString(),
                            SettledType = type,
                            ShipAmt = dr["运费"].ToString() == "" ? 0 : dr["运费"].ObjectToDecimal(),
                            PointAmt = dr["点费"].ToString() == "" ? 0 : dr["点费"].ObjectToDecimal(),
                            BAFAmt = dr["燃油附加费"].ToString() == "" ? 0 : dr["燃油附加费"].ObjectToDecimal(),
                            OtherAmt = dr["其他费用"].ToString() == "" ? 0 : dr["其他费用"].ObjectToDecimal(),
                            Remark = dr["备注"].ToString(),
                            RelatedCustomerID = customer
                        });

                        var response = new SettledService().BatchUpdateSettledPodAmt(new BatchUpdateSettledPodAmtRequest() { SettledPods = settledPods, Updator = base.UserInfo.Name, SettleType = type });

                        if (response.IsSuccess)
                        {
                            StringBuilder sb = new StringBuilder();
                            this.GenerateReturnMessage(response.Result, sb, type);
                            return new { result = sb.ToString(), IsSuccess = true, Count = response.Result.Count() }.ToJsonString();
                        }

                        return new { result = "<h3>费用调整失败</h3><br/>" + response.Exception.Message, IsSuccess = false }.ToJsonString();
                    }

                    return new { result = "<h3>费用调整失败</h3><br/>excel内容有误！", IsSuccess = false }.ToJsonString();
                }

                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }

            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }


        [HttpPost]
        public string EditSettledPod(long id, decimal? shipAmt, decimal? bafAmt, decimal? pointAmt, decimal? otherAmt, string remark, int? settledType)
        {
            SettledPod settledPod = new SettledPod()
            {
                ID = id,
                ShipAmt = shipAmt,
                BAFAmt = bafAmt,
                PointAmt = pointAmt,
                OtherAmt = otherAmt,
                TotalAmt = (shipAmt ?? 0) + (bafAmt ?? 0) + (pointAmt ?? 0) + (otherAmt ?? 0),
                Remark = remark
            };
            var response = new SettledService().EditSettledPod(new EditSettledPodRequest() { SettledPod = settledPod, SettledType = settledType ?? 0, Updator = base.UserInfo.Name });
            if (response.IsSuccess)
            {
                return "运单结算修改成功";
            }

            return "运单结算修改失败";
        }

        [HttpPost]
        public JsonResult DeleteSettledPod(long id, int settledType)
        {
            var response = new SettledService().DeleteSettledPod(new DeleteSettledPodRequest() { ID = id, SettledType = settledType });
            if (response.IsSuccess)
            {
                return Json(new { Message = "删除运单结算成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "删除运单结算失败！", IsSuccess = false });
            }
        }


        [HttpPost]
        public JsonResult CashPayPod(long id, string customerOrderNumber, decimal? totalAmt, DateTime? date, string remark)
        {
            if (totalAmt == null || totalAmt == 0)
            {
                return Json(new { Message = "实际支付金额输入有误", IsSuccess = false });
            }

            if (date == null)
            {
                return Json(new { Message = "支付日期输入有误", IsSuccess = false });
            }

            var invoiceNumber = customerOrderNumber + "-" + DateTime.Now.ToString("yyyyMMddyyyyMMddHHmmss");
            var response = new InvoiceService().AddInvoiceAndPayOrders(new AddInvoiceAndPayOrdersRequest()
            {
                SettledPodID = id,
                Date = date ?? DateTime.Now,
                Remark = remark,
                TotalAmt = totalAmt.Value,
                InvoiceNumber = invoiceNumber,
                InvoiceSystemNumber = "FPF" + DateTime.Now.ToString("yyyyMMddyyyyMMddHHmmss"),
                ReceiveOrPayOrderNumber = "FK" + DateTime.Now.ToString("yyyyMMddyyyyMMddHHmmss"),
                Creator = base.UserInfo.Name
            });

            if (response.IsSuccess)
            {
                return Json(new { Message = "现金支付成功，系统自动生虚拟发票:" + invoiceNumber, IsSuccess = true });
            }

            return Json(new { Message = "现金支付失败！", IsSuccess = false });
        }

        private void GenQuerySettledPodViewModel(SettledPodManageViewModel vm)
        {
            if (base.UserInfo.UserType == 2)
            {
                vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                vm.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID)
                                .Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.Customers = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
                vm.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID)
                                .Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            }
            else
            {
                vm.Customers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.Name });
                vm.Shippers = new List<SelectListItem> { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationShippers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }

            vm.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.PODTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.TtlOrTpls = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.TTLORTPL)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
        }

        public ActionResult AuditSettledPod(string SettledPodIDs, DateTime AuditDate, string AuditRemark, int AuditType, bool? isManualSettled)
        {
            var settledPodIDs = SettledPodIDs.Split(',').Select(id => id.ObjectToInt64());
            if (settledPodIDs == null || !settledPodIDs.Any())
            {
                throw new Exception("数据出错");
            }


            bool result = false;
            SettledService service = new SettledService();
            if (AuditType == 4)
            {
                if (isManualSettled == null || !isManualSettled.Value)
                {
                    result = service.DeleteAllExtenFeeData(new DeleteAllExtenFeeDataRequest()
                    {
                        SettledPodIDCollection = settledPodIDs
                    }).IsSuccess;
                }
                else
                {
                    result = service.DeleteManualSettledFee(new DeleteManualSettledFeeRequest()
                    {
                        SettledPodIDCollection = settledPodIDs
                    }).IsSuccess;
                }
            }
            else
            {
                result = service.AuditSettledPod(new AuditSettledPodRequest()
                {
                    SettledPodIDs = settledPodIDs,
                    Auditor = base.UserInfo.Name,
                    AuditTime = AuditDate,
                    AuditRemark = AuditRemark,
                    AuditType = AuditType,
                    AuditTypeMessage = AuditType == 1 ? "同意" : (AuditType == 2 ? "不同意" : "终审同意")
                }).IsSuccess;
            }

            if (result)
            {
                return Json(new { IsSuccess = true });
            }

            throw new Exception("费用审核失败");
        }

        private DataTable InitExportTable(IEnumerable<SettledPod> settledPods)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("系统编号", typeof(string));
            dt.Columns.Add("客户运单号", typeof(string));
            dt.Columns.Add("结算类型", typeof(string));
            dt.Columns.Add("客户或承运商", typeof(string));
            dt.Columns.Add("起运城市", typeof(string));
            dt.Columns.Add("目的城市", typeof(string));
            dt.Columns.Add("运输类型", typeof(string));
            dt.Columns.Add("运单类型", typeof(string));
            dt.Columns.Add("整车零担", typeof(string));
            dt.Columns.Add("发货日期", typeof(string));
            dt.Columns.Add("箱数", typeof(string));
            dt.Columns.Add("件数", typeof(string));
            dt.Columns.Add("重量", typeof(string));
            dt.Columns.Add("体积", typeof(string));
            dt.Columns.Add("报价", typeof(string));
            dt.Columns.Add("运费", typeof(string));
            dt.Columns.Add("点费", typeof(string));
            dt.Columns.Add("燃油附加费", typeof(string));
            dt.Columns.Add("其他费用", typeof(string));
            dt.Columns.Add("备注", typeof(string));

            settledPods.Each((i, settledPod) =>
            {
                DataRow dr = dt.NewRow();
                dr["系统编号"] = settledPod.SystemNumber;
                dr["客户运单号"] = "'" + settledPod.CustomerOrderNumber;
                dr["结算类型"] = settledPod.SettledType == 0 ? "应收" : "应付";
                dr["客户或承运商"] = settledPod.CustomerOrShipperName;
                dr["起运城市"] = settledPod.StartCityName;
                dr["目的城市"] = settledPod.EndCityName;
                dr["运输类型"] = settledPod.ShipperTypeName;
                dr["运单类型"] = settledPod.PODTypeName;
                dr["整车零担"] = settledPod.TtlOrTplName;
                dr["发货日期"] = settledPod.ActualDeliveryDate.DateTimeToString();
                dr["箱数"] = settledPod.BoxNumber.ToString();
                dr["件数"] = settledPod.GoodsNumber.ToString();
                dr["重量"] = settledPod.Weight.ToString();
                dr["体积"] = settledPod.Volume.ToString();
                dr["报价"] = settledPod.Str4;
                dr["运费"] = settledPod.ShipAmt.ToString();
                dr["点费"] = settledPod.PointAmt.ToString();
                dr["燃油附加费"] = settledPod.BAFAmt.ToString();
                dr["其他费用"] = settledPod.OtherAmt.ToString();
                dr["备注"] = settledPod.Remark;
                dt.Rows.Add(dr);
            });

            return dt;
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

        private string GenerateReturnMessage(IEnumerable<SettledPod> settledPods, StringBuilder successSB, int type)
        {
            successSB.Append("<h3>结算费用更新成功(如无更新,则此运单已开票,不能做费用变更),更新结果如下</h3><br/>");
            successSB.Append("<table><thead><tr><th>系统编号</th><th>客户运单号</th><th>结算类型</th><th>").Append(type == 0 ? "客户" : "承运商").Append("</th><th>起运城市</th><th>目的城市</th><th>运输类型</th><th>运单类型</th><th>整车零担</th><th>发货日期</th><th>箱数</th><th>件数</th><th>重量</th><th>体积</th><th>运费</th><th>点费</th><th>燃油附加费</th><th>其他费用</th><th>费用合计</th><th>备注</th></tr></thead><tbody>");

            foreach (var o in settledPods)
            {
                successSB.Append("<tr><td>").Append(o.SystemNumber)
                         .Append("</td><td>").Append(o.CustomerOrderNumber)
                         .Append("</td><td>").Append(o.SettledType == 0 ? "应收" : "应付")
                         .Append("</td><td>").Append(o.CustomerOrShipperName)
                         .Append("</td><td>").Append(o.StartCityName)
                         .Append("</td><td>").Append(o.EndCityName)
                         .Append("</td><td>").Append(o.ShipperTypeName)
                         .Append("</td><td>").Append(o.PODTypeName)
                         .Append("</td><td>").Append(o.TtlOrTplName)
                         .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                         .Append("</td><td>").Append(o.BoxNumber)
                         .Append("</td><td>").Append(o.GoodsNumber)
                         .Append("</td><td>").Append(o.Weight)
                         .Append("</td><td>").Append(o.Volume)
                         .Append("</td><td>").Append("￥").Append(o.ShipAmt)
                         .Append("</td><td>").Append("￥").Append(o.PointAmt)
                         .Append("</td><td>").Append("￥").Append(o.BAFAmt)
                         .Append("</td><td>").Append("￥").Append(o.OtherAmt)
                         .Append("</td><td>").Append("￥").Append(o.TotalAmt)
                         .Append("</td><td>").Append(o.Remark);
                successSB.Append("</tr>");
            }
            successSB.Append("</tbody></table>");

            return successSB.ToString();
        }

        [HttpGet]
        public ActionResult SettledCompare(int SettledType)
        {
            SettledCompareViewModel vm = new SettledCompareViewModel();
            vm.DisplyMessage = SettledType == 0 ? "导入应收结算比对(客户运单号|运费|点费|燃油附加费|其他费用)" : "导入应付结算比对(客户运单号|运费|点费|燃油附加费|其他费用)";
            vm.SettledType = SettledType;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            vm.SettledPodCompareCollection = Enumerable.Empty<SettledPodCompare>();
            return View(vm);
        }

        [HttpPost]
        public ActionResult SettledCompare(SettledCompareViewModel vm)
        {
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            vm.DisplyMessage = vm.SettledType == 0 ? "导入应收结算比对(客户运单号|运费|点费|燃油附加费|其他费用)" : "导入应付结算比对(客户运单号|运费|点费|燃油附加费|其他费用)";

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
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

                    if (ds == null || ds.Tables == null || ds.Tables[0].Rows.Count == 0)
                    {
                        vm.ErrorMessage = "Excel文件内容有误";
                    }
                    else
                    {
                        IList<SettledPodCompare> settledPodCompares = new List<SettledPodCompare>();
                        StringBuilder errorMessage = new StringBuilder();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string customerOrderNumber = string.Empty;
                            decimal excelShipAmt = 0;
                            decimal excelBafAmt = 0;
                            decimal excelPointAmt = 0;
                            decimal excelOtherAmt = 0;

                            if (ds.Tables[0].Columns.Contains("客户运单号"))
                            {
                                customerOrderNumber = ds.Tables[0].Rows[i]["客户运单号"].ToString();
                                if (string.IsNullOrEmpty(customerOrderNumber))
                                {
                                    errorMessage.Append("Excel中第").Append(i + 1).Append("行客户运单号列为空,请重新编辑Excel");
                                }
                            }
                            else
                            {
                                errorMessage.Append("Excel中无客户运单号列，请重新编辑").Append("<br />");
                            }

                            if (ds.Tables[0].Columns.Contains("运费"))
                            {
                                decimal tempShipAmt;
                                if (decimal.TryParse(ds.Tables[0].Rows[i]["运费"].ToString().Trim(), out tempShipAmt))
                                {
                                    excelShipAmt = tempShipAmt;
                                }
                            }

                            if (ds.Tables[0].Columns.Contains("点费"))
                            {
                                decimal tempPointAmt;
                                if (decimal.TryParse(ds.Tables[0].Rows[i]["点费"].ToString().Trim(), out tempPointAmt))
                                {
                                    excelPointAmt = tempPointAmt;
                                }
                            }

                            if (ds.Tables[0].Columns.Contains("燃油附加费"))
                            {
                                decimal tempBafAmt;
                                if (decimal.TryParse(ds.Tables[0].Rows[i]["燃油附加费"].ToString().Trim(), out tempBafAmt))
                                {
                                    excelBafAmt = tempBafAmt;
                                }
                            }

                            if (ds.Tables[0].Columns.Contains("其他费用"))
                            {
                                decimal tempOtherAmt;
                                if (decimal.TryParse(ds.Tables[0].Rows[i]["其他费用"].ToString().Trim(), out tempOtherAmt))
                                {
                                    excelOtherAmt = tempOtherAmt;
                                }
                            }

                            settledPodCompares.Add(new SettledPodCompare() {
                                CustomerOrderNumber = customerOrderNumber,
                                CompareShipAmt = excelShipAmt,
                                CompareBAFAmt = excelBafAmt,
                                ComparePointAmt = excelPointAmt,
                                CompareOtherAmt = excelOtherAmt,
                                CompareTotalAmt = excelShipAmt + excelBafAmt + excelPointAmt + excelOtherAmt,
                                SettledType = vm.SettledType,
                                ProjectID = base.UserInfo.ProjectID,
                                CustomerOrShipperID = vm.SettledType == 0 ? vm.CustomerID : vm.ShipperID,
                                RelatedCustomerID = vm.CustomerID
                            });
                        }

                        if (errorMessage.Length > 0)
                        {
                            vm.ErrorMessage = errorMessage.ToString();
                            return View(vm);
                        }

                        var Response = new SettledService().GetSettledPodByCondition(new GetSettledPodByConditionRequest() { 
                            CustomerOrderNumberCollection = settledPodCompares.Select(s=>s.CustomerOrderNumber),
                            SettledType = vm.SettledType,
                            CustomerID = vm.CustomerID,
                            ShipperID = vm.ShipperID
                        });

                        if (Response.IsSuccess)
                        {
                            settledPodCompares.Each((i, s) => {
                                var tempSettledPod = Response.Result.FirstOrDefault(r => r.CustomerOrderNumber == s.CustomerOrderNumber);
                                s.ActualDeliveryDate = DateTime.MinValue;
                                if (tempSettledPod != null)
                                {
                                    s.SystemNumber = tempSettledPod.SystemNumber;
                                    s.CustomerOrShipperName = tempSettledPod.CustomerOrShipperName;
                                    s.PodID = tempSettledPod.PodID;
                                    s.ID = tempSettledPod.ID;
                                    s.SettledNumber = tempSettledPod.SettledNumber;
                                    s.StartCityID = tempSettledPod.StartCityID;
                                    s.StartCityName = tempSettledPod.StartCityName;
                                    s.EndCityID = tempSettledPod.EndCityID;
                                    s.EndCityName = tempSettledPod.EndCityName;
                                    s.ShipperTypeID = tempSettledPod.ShipperTypeID;
                                    s.ShipperTypeName = tempSettledPod.ShipperTypeName;
                                    s.PODTypeID = tempSettledPod.PODTypeID;
                                    s.PODTypeName = tempSettledPod.PODTypeName;
                                    s.TtlOrTplID = tempSettledPod.TtlOrTplID;
                                    s.TtlOrTplName = tempSettledPod.TtlOrTplName;
                                    s.ActualDeliveryDate = tempSettledPod.ActualDeliveryDate ?? DateTime.MinValue;
                                    s.BoxNumber = tempSettledPod.BoxNumber;
                                    s.Weight = tempSettledPod.Weight;
                                    s.GoodsNumber = tempSettledPod.GoodsNumber;
                                    s.Volume = tempSettledPod.Volume;
                                    s.Remark = tempSettledPod.Remark;
                                    s.ShipAmt = tempSettledPod.ShipAmt;
                                    s.BAFAmt = tempSettledPod.BAFAmt;
                                    s.PointAmt = tempSettledPod.PointAmt;
                                    s.OtherAmt = tempSettledPod.OtherAmt;
                                    s.TotalAmt = tempSettledPod.TotalAmt;
                                    s.Str1 = tempSettledPod.Str1;
                                    s.Str2 = tempSettledPod.Str2;
                                    s.Str3 = tempSettledPod.Str3;
                                    s.Str4 = tempSettledPod.Str4;
                                    s.Str5 = tempSettledPod.Str5;
                                    s.DateTime1 = tempSettledPod.DateTime1;
                                    s.DateTime2 = tempSettledPod.DateTime2;
                                    s.Creator = tempSettledPod.Creator;
                                    s.CreateTime = tempSettledPod.CreateTime;
                                    s.InvoiceID = tempSettledPod.InvoiceID;
                                    s.IsAudit = tempSettledPod.IsAudit;
                                }   
                            });

                            vm.SettledPodCompareCollection = settledPodCompares;
                        }
                        else
                        {
                            vm.ErrorMessage = "获取系统结算信息失败！";
                        }
                    }
                }
                else
                {
                    vm.ErrorMessage = "Excel文件内容为空";
                }
            }
            else
            {
                vm.ErrorMessage = "请选择excel文件";
            }

            return View(vm);
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
    }
}
