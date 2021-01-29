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
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts.POD.Adidas;


namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class AdidasController : BaseController
    {
        //
        // GET: /POD/Adidas/

        public ActionResult ImportScanData(int? id)
        {
            return View();
        }


        /// <summary>
        /// 导入数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ImportScanData()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null && ds.Tables[0] != null)
                    {
                        DataTable dt = ds.Tables[0];

                        ///验证导入的表格式
                        if (dt.Columns.Contains("运单号") && dt.Columns.Contains("箱数") && dt.Columns.Contains("承运商") && dt.Columns.Count == 3)
                        {
                            StringBuilder sb = new StringBuilder();
                            IEnumerable<ScanInfo> scanInfoList = this.InitScanInfoFromDataTable(dt, sb);

                            if (!string.IsNullOrEmpty(sb.ToString()))
                            {
                                return new { result = "<h3>运单导入失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                            }

                            if (scanInfoList == null || !scanInfoList.Any())
                            {
                                return new { result = "<h3>Excel无数据</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                            }

                            IEnumerable<string> existsPodCustomerOrderNumbers = Enumerable.Empty<string>();



                            var response = new AdidasService().AddScanDatas(new AddScanDatasRequest() { scanInfos = scanInfoList });

                            if (response.IsSuccess)
                            {
                                StringBuilder successSB = new StringBuilder();
                                successSB.Append("<b>导入运单成功</b><br/>");
                                if (!string.IsNullOrEmpty(response.SuccessMessage.Split(',')[0]))
                                {
                                    successSB.Append("<b>" + response.SuccessMessage.ToString().Split(',')[0] + "等运单有重复</b><br/>");
                                }
                                if (!string.IsNullOrEmpty(response.SuccessMessage.Split(',')[1]))
                                {
                                    successSB.Append("<b>" + response.SuccessMessage.ToString().Split(',')[1] + "等承运商有错误</b><br/>");
                                }

                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("箱数")
                                    .Append("</th><th>").Append("承运商")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.BoxNumber)
                                        .Append("</td><td>").Append(o.Shipper)
                                        .Append("</td></tr>");
                                }
                                successSB.Append("</tbody></table>");
                                return new { result = successSB.ToString(), IsSuccess = true }.ToJsonString();
                            }
                            else
                            {
                                return new { result = "<h3>导入数据失败</h3><br/>系统忙，请稍后再试", IsSuccess = false }.ToJsonString();
                            }
                        }

                        return new { result = "<h3>导入数据失败</h3><br/>Excel文件表单1(Sheet1)的格式不正确，请检查", IsSuccess = false }.ToJsonString();
                    }
                    return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
                }
            }
            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }


        /// <summary>
        /// 根据路径获取Excel数据，并转为excel
        /// </summary>
        /// <param name="hpf"></param>
        /// <returns></returns>
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



        /// <summary>
        /// 将datatable转为list，并更新转换日志
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnsConfig"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        private IEnumerable<ScanInfo> InitScanInfoFromDataTable(DataTable dt, StringBuilder sb)
        {
            IList<ScanInfo> scanInfoList = new List<ScanInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ScanInfo scanInfo = new ScanInfo();
                scanInfo.ID = base.UserInfo.ProjectID;
                scanInfo.Creater = base.UserInfo.Name;
                scanInfo.CreateTime = DateTime.Now;
                ///以下数据为无效数据，只是为了防止报错
                scanInfo.OperateTime = DateTime.Now;
                scanInfo.ModifyTime = DateTime.Now;
                string value;
                value = dt.Rows[i]["运单号"].ToString().Trim();

                ///去除空白行
                if (dt.Rows[i]["运单号"].ToString().Trim() == "" && dt.Rows[i]["箱数"].ToString().Trim() == "" && dt.Rows[i]["承运商"].ToString().Trim() == "")
                {
                    continue;
                }
                if (string.IsNullOrEmpty(value))
                {
                    sb.Append("第" + (i + 1).ToString() + "行，" + dt.Columns["运单号"].ColumnName + " 不能为空<br/>");

                }

                else if (scanInfoList.Any(p => p.CustomerOrderNumber == value))
                {
                    sb.Append("第" + (i + 1).ToString() + "行，" + dt.Columns["运单号"].ColumnName + " 重复<br/>");
                }
                else
                {//满足条件才添加
                    scanInfo.CustomerOrderNumber = value;
                    scanInfo.Shipper = dt.Rows[i]["承运商"].ToString();
                    scanInfo.BoxNumber = Convert.ToInt64(dt.Rows[i]["箱数"].ToString());
                    scanInfoList.Add(scanInfo);
                }
            }
            return scanInfoList;
        }



        // GET: /POD/Adidas/
        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueryScanData(int? id)
        {

            ///赋值ViewModel
            AdidasScanDataViewModel ASDViewModel = new AdidasScanDataViewModel();

            ASDViewModel.PageIndex = 0;
            ASDViewModel.PageCount = 0;
            ASDViewModel.SearchCondition = new AdidasScanDataSearchCondition();  ///查询条件

            ///获取查询结果
            return View(ASDViewModel);
        }


        /// <summary>
        /// 查询界面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryScanData(AdidasScanDataViewModel ASDViewModel, int? PageIndex, string Action)
        {
            ///by tim 2015.12.16 增加判断，若角色为“承运商扫描数据查询”，则默认给指定承运商查询
            if (UserInfo.ProjectRoleID == 50)
            { 
                ASDViewModel.SearchCondition.Shipper ="ShipperID:"+UserInfo.CustomerOrShipperID.ToString(); //关键字标识，表示筛选的条件为承运商ID
            }
            var response = new AdidasService().GetQueryScanDatas(new QuerySearchConditionRequest
            {
                SearchCondition = ASDViewModel.SearchCondition,
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE  ///每页的数据
            });



            if (response.IsSuccess)
            {
                ASDViewModel.ScanInfoCollection = response.Result.ScanDataCollection;
                ASDViewModel.PageCount = response.Result.PageCount / UtilConstants.PAGESIZE + ((response.Result.PageCount % UtilConstants.PAGESIZE) == 0 ? 0 : 1); ;
                ASDViewModel.PageIndex = response.Result.PageIndex;
            }

            if (response.IsSuccess && Action == "导出")
            {
                DataTable dt = new DataTable();
                dt = EnumerableExtension.ToDataTable<ScanInfo>(ASDViewModel.ScanInfoCollection.ToList());        //List转化为Datatable

                ///对查询出来的列进行处理
                dt.Columns.Remove("ID");
                dt.Columns.Remove("ShipperID");
                dt.Columns.Remove("Creater");
                dt.Columns.Remove("CreateTime");
                dt.Columns.Remove("Modifier");
                dt.Columns.Remove("ModifyTime");


                dt.Columns["CustomerOrderNumber"].ColumnName = "运单号";
                dt.Columns["BoxNumber"].ColumnName = "运单箱数";
                dt.Columns["ScanBoxNumber"].ColumnName = "实际扫描箱数";
                dt.Columns["TrailerNo"].ColumnName = "拖号";
                dt.Columns["PlateNumber"].ColumnName = "车牌号";
                dt.Columns["Shipper"].ColumnName = "物流公司";
                dt.Columns["CloseFlag"].ColumnName = "关闭状态(1为关闭)";
                dt.Columns["CompleteFlag"].ColumnName = "确认完成次数(0为未完成)";
                dt.Columns["Operator"].ColumnName = "操作人";
                dt.Columns["OperateTime"].ColumnName = "操作时间";
                dt.Columns["Remark"].ColumnName = "备注";


                this.WriteExcel(dt, "Shipper_" + UserInfo.Name + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");  //生成Excel
                return new EmptyResult();

            }
            return View(ASDViewModel);
        }

        /// <summary>
        /// 审核 用户
        /// </summary>
        [HttpPost]
        public JsonResult ClosePOD(string id)
        {
            ///更改某一个运单号的状态为关闭
            if (new AdidasService().ClosePOD(id))
            {
                ///成功
                return Json(new { Message = "关闭成功！", IsSuccess = false });

            }
            else
            {
                ///根据结果返回不同的提示信息
                return Json(new { Message = "关闭失败，请刷新界面重试！", IsSuccess = false });
            }
        }
        [HttpGet]
        public ActionResult PayPriceImportController()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PayPriceImportController(string id)
        {
            return View();
        }


        /// <summary>
        /// 根据datatable生成Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="path"></param>
        public void WriteExcel(DataTable dt, string fileName)
        {
            try
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
                Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
                Response.ContentEncoding = Encoding.UTF8;
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
                Response.Flush();
                Response.End();

            }
            catch (Exception ex)
            {

            }
        }
        [HttpGet]
        public ActionResult UpdateBAF()
        {

            DateTime dt = DateTime.Now;
            GetBAFMobile vm = new GetBAFMobile();
            ViewBag.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID);
            vm.BAFStartTime = dt.AddDays(1 - dt.Day).ToString("yyyy-MM-dd");
            vm.BAFEndTime = dt.AddDays(1 - dt.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            vm.abfRiceInfos = null;
            vm.PageCount = 0;
            vm.PageIndex = 0;
            return View(vm);
        }
        [HttpPost]
        public ActionResult UpdateBAF(GetBAFMobile vm)
        {
            var response = new AdidasService().GetABFPrice(new ABFPriceRequest
            {
                BAFStartTime = vm.BAFStartTime,
                BAFEndTime = vm.BAFEndTime,
                BAFPrice = vm.BAFPrice,
                PageSize = 100,
                PageIndex = 0
            });
            if (response.IsSuccess)
            {
                vm.abfRiceInfos = response.Result.bafPriceInfo;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;

            }
            return View(vm);
        }
        [HttpGet]
        public ActionResult AddBAF()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddBAF(GetBAFMobile vm)
        {

            vm.abfRiceInfo.ProjectID = 1;
            vm.abfRiceInfo.ProjectName = "Runbow";
            vm.abfRiceInfo.TragetID = 1;
            vm.abfRiceInfo.TragetName = "Adidas";
            ABFPriceRequest SearchCondition = new ABFPriceRequest();

            SearchCondition.Info = vm.abfRiceInfo;
            bool response = new AdidasService().addABFPrice(SearchCondition);
            if (response)
            {
                return Redirect("/POD/Adidas/UpdateBAF");
            }
            else
            {
                return View("添加失败！");
            }
            //new ABFPriceRequest
            // {

            //ProjectID = 1,
            //ProjectName = "Runbow",
            //TragetID = 1,
            //TragetName = "Adidas",
            //BAFPrice = vm.BAFPrice,
            //BAFStartTime = vm.BAFStartTime,
            //BAFEndTime = vm.BAFEndTime,
            //PageIndex = vm.PageIndex,
            //PageSize = UtilConstants.PAGESIZE  ///每页的数据
            //  });

        }
        public bool Edit(int id)
        {
            bool response = new AdidasService().del(id);

            return true;
        }
    }
}
