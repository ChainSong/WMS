using Runbow.TWS.Biz.POD;
using Runbow.TWS.MessageContracts.POD.AKZO;
using Runbow.TWS.Web.Areas.POD.Models;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using UtilConstants = Runbow.TWS.Common.Constants;
using MyFile = System.IO.File;
using Runbow.TWS.Common;
using System.IO;
using System.Data;
using Runbow.TWS.Entity;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;
using System.Text;


namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class AKZOController : BaseController
    {
        private long _CustomerID = 7;
        [HttpGet]
        public ActionResult AbnormalPODSearch()
        {
            AbnormalPODSearchModel model = new AbnormalPODSearchModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AbnormalPODSearch(AbnormalPODSearchModel model, int? PageIndex)
        {
            if(model.IsExport)
            {
              model.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "Pod");
              return    this.GetAbnormalPODSearchToExcel(model);
            }
            else if (model.IsExportTrack)
            {
               return   this.GetAbnormalPODTrackSearchToExcel(model);
            }
            else
            {
                var Result = new AKZOService().GetAbnormalPODSearch(new GetAbnormalPODSearchRequest() { SqlWhere = this.GetWhere(model), PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 }).Result;
                model.AbnormalTable = Result.AbnormalTable;
                model.PageIndex = Result.PageIndex;
                model.PageSize = Result.PageSize;
                model.PageCount = Result.PageCount;
                
            }

            return View(model);
        }

        public string GetWhere(AbnormalPODSearchModel model)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(model.BeginActualDeliveryDate.ToString()))
            {
                sql += " AND POD.ActualDeliveryDate>='" + model.BeginActualDeliveryDate.ToString() + "'";
            }

            if (!string.IsNullOrEmpty(model.EndActualDeliveryDate.ToString()))
            {
                sql += " AND POD.ActualDeliveryDate<='" + model.EndActualDeliveryDate.ToString() + "'";
            }

            return sql;
        }

       


        
        
        public ActionResult GetAbnormalPODSearchToExcel(AbnormalPODSearchModel model)
        {
            var podCollection = new AKZOService().GetAbnormalPODSearchToExcel(new GetAbnormalPODSearchRequest() { SqlWhere = this.GetWhere(model)}).Result;

            this.GenQueryAbnormalPODSearchModel(model);
                long? customerID;
                if (model.Customers.Count() == 1)
                {
                    customerID = model.Customers.First().Value.ObjectToInt64();
                }
                else
                {
                    customerID = model.SearchCondition.CustomerID;
                }

                return this.ExportPodsToExcel(podCollection, model.Config.ColumnCollection, customerID);


        }




        
        public ActionResult GetAbnormalPODTrackSearchToExcel(AbnormalPODSearchModel model)
        {

            var Result = new AKZOService().GetAbnormalPODTrackSearchToExcel(new GetAbnormalPODSearchRequest() { SqlWhere = this.GetWhere(model) }).Result;

            var columns = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "PodTrack")
                      .ColumnCollection.Where(c => (c.IsKey && c.ForView) || (!c.IsKey && !c.IsHide) || (!c.IsKey && c.IsHide && c.InnerColumns.Any(innerc => innerc.CustomerID == _CustomerID)))
                      .Select(c =>
                      {
                          if (c.InnerColumns.Count == 0)
                          {
                              return c;
                          }
                          else
                          {
                              if (c.InnerColumns.Any(innerc => innerc.CustomerID == _CustomerID))
                              {
                                  return c.InnerColumns.First(innerc => innerc.CustomerID == _CustomerID);
                              }

                              return c;
                          }
                      });
            return this.ExportPodTracksToExcel(Result, columns);





            //var Result = new AKZOService().GetAbnormalPODTrackSearchToExcel(new GetAbnormalPODSearchRequest() { SqlWhere = this.GetWhere(model) }).Result;
            //DataTable Exprottable = Result.AbnormalTable;


            //string ReportName = "POD跟踪异常数据报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            //ExcelHelper excelHelper = new ExcelHelper();
            //string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
            //string fileFullPath = Path.Combine(targetPath, ReportName);
            //excelHelper.CreateExcelByDataTable(fileFullPath, Exprottable);
            //excelHelper.Dispose();

            //string mimeType = "application/msexcel";
            //FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
            //return File(fs, mimeType, Url.Encode(ReportName));



        }




        private ActionResult ExportPodsToExcel(IEnumerable<Pod> pods, IEnumerable<Column> podColumns, long? customerID)
        {
            DataTable dt = new DataTable();
            pods.Each((i, p) =>
            {
                if (p.CustomerOrderNumber.StartsWith("0"))
                {
                    p.CustomerOrderNumber = "'" + p.CustomerOrderNumber;
                }
            });

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

            var resultTable = pods.ConverToTable(dt);

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
            string ReportName = "POD异常数据报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            ExcelHelper excelHelper = new ExcelHelper();
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
            string fileFullPath = Path.Combine(targetPath, ReportName);
            excelHelper.CreateExcelByDataTable(fileFullPath, resultTable);
            excelHelper.Dispose();
            string mimeType = "application/msexcel";
            FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
            return File(fs, mimeType,  Url.Encode(ReportName));
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

            ExcelHelper excelHelper = new ExcelHelper();
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
            string fileFullPath = Path.Combine(targetPath, "ExportPodTracks.xlsx");
            excelHelper.CreateExcelByDataTable(fileFullPath, podTracksTable);
            excelHelper.Dispose();
            string mimeType = "application/msexcel";
            FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
            return File(fs, mimeType, "ExportPodTracks.xlsx");
        }
        



        private void GenQueryAbnormalPODSearchModel(AbnormalPODSearchModel model)
        {
            model.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "Pod");
           
            if (base.UserInfo.UserType == 2)
            {
                model.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else if (base.UserInfo.UserType == 0)
            {
                model.Customers = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                model.Customers = Enumerable.Empty<SelectListItem>();
            }

           
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

        [HttpGet]
        public ActionResult UpdateAKZOPodSettledInfo(int? id)
        {
            return View();
        }

        [HttpPost]
        public string UpdateAKZOPodSettledInfo()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<UpdateAKZOPod> pods = new List<UpdateAKZOPod>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            pods.Add(new UpdateAKZOPod() {
                                CustomerOrderNumber = ds.Tables[0].Rows[i]["Delivery number"].ToString().Trim(),
                                Weight = ds.Tables[0].Rows[i]["Gross wght per stage"].ToString().Trim().ObjectToDouble(),
                                TS = ds.Tables[0].Rows[i]["QTY per stage"].ToString().Trim().ObjectToDouble(),
                                SS = ds.Tables[0].Rows[i]["Volume per stage"].ToString().Trim().ObjectToDouble(),
                                EndCityName = ds.Tables[0].Rows[i]["City Name"].ToString().Trim(),
                                ActualDeliveryDate = ds.Tables[0].Rows[i]["Calendar day"].ToString().Trim().ObjectToDateTime(),
                                CustomerAccount = ds.Tables[0].Rows[i]["Customer Account"].ToString().Trim()
                            });
                        }

                        var groupedPods = from p in pods group p by new { p.CustomerOrderNumber, p.ActualDeliveryDate, p.EndCityName,p.CustomerAccount } into g select new UpdateAKZOPod() { CustomerOrderNumber = g.Key.CustomerOrderNumber, Weight = g.Sum(k=>k.Weight), TS = g.Sum(k=>k.TS), SS = g.Sum(k =>k.SS), EndCityName = g.Key.EndCityName, ActualDeliveryDate = g.Key.ActualDeliveryDate,CustomerAccount=g.Key.CustomerAccount};
                        var response = new AKZOService().UpdateAkzoPodAndGetTheDifference(new UpdateAkzoPodAndGetTheDifferenceRequest() { Pods = groupedPods });
                        if (response.IsSuccess)
                        {
                            StringBuilder successSB = new StringBuilder();
                            successSB.Append("<h3>共导入数据").Append(pods.Count).Append("条,合并后数据为").Append(groupedPods.Count()).Append("条.<br/>");
                            if (response.Result.UpdatedPods != null && response.Result.UpdatedPods.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.UpdatedPods.Count()).Append("条)更新成Excel中内容</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")
                                    .Append("</th><th>").Append("重量")
                                    .Append("</th><th>").Append("桶数")
                                    .Append("</th><th>").Append("升数")
                                    .Append("</th><th>").Append("客户账号")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.UpdatedPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                                        .Append("</td><td>").Append(o.Weight.ToString())
                                        .Append("</td><td>").Append(o.TS.ToString())
                                        .Append("</td><td>").Append(o.SS.ToString())
                                        .Append("</td><td>").Append(o.CustomerAccount.ToString())
                                        .Append("</td></tr>");
                                }
                                
                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.NotUpdatedPods != null && response.Result.NotUpdatedPods.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.NotUpdatedPods.Count()).Append("条)没有更新成Excel中内容,系统中无对应运单</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")
                                    .Append("</th><th>").Append("重量")
                                    .Append("</th><th>").Append("桶数")
                                    .Append("</th><th>").Append("升数")
                                    .Append("</th><th>").Append("客户账号")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.NotUpdatedPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                                        .Append("</td><td>").Append(o.Weight.ToString())
                                        .Append("</td><td>").Append(o.TS.ToString())
                                        .Append("</td><td>").Append(o.SS.ToString())
                                        .Append("</td><td>").Append(o.CustomerAccount.ToString())
                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.CityNotMatchPods != null && response.Result.CityNotMatchPods.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.CityNotMatchPods.Count()).Append("条)与系统中运单目的城市不一致</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")
                                    .Append("</th><th>").Append("目的城市")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.CityNotMatchPods)
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
        public ActionResult UpdateAKZOModelsPodSettledInfo(int? id)
        {
            return View();
        }

        #region 车型同步
        [HttpPost]
        public string UpdateAKZOModelsPodSettledInfo()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<UpdateAKZOModelsPod> pods = new List<UpdateAKZOModelsPod>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            pods.Add(new UpdateAKZOModelsPod()
                            {
                                CustomerOrderNumber = ds.Tables[0].Rows[i]["Delivery number"].ToString().Trim(),

                                Models = ds.Tables[0].Rows[i]["Car Models"].ToString().Trim(),

                            });
                        }

                        var groupedPods = from p in pods group p by new { p.CustomerOrderNumber, p.Models } into g select new UpdateAKZOModelsPod() { CustomerOrderNumber = g.Key.CustomerOrderNumber, Models = g.Key.Models };
                        var response = new AKZOService().UpdateAkzoModelsPodAndGetTheDifference(new UpdateAkzoModelsPodAndGetTheDifferenceRequest() { Pods = groupedPods });
                        if (response.IsSuccess)
                        {
                            StringBuilder successSB = new StringBuilder();
                            successSB.Append("<h3>共导入数据").Append(pods.Count).Append("条,合并后数据为").Append(groupedPods.Count()).Append("条.<br/>");
                            if (response.Result.UpdatedPods != null && response.Result.UpdatedPods.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.UpdatedPods.Count()).Append("条)更新成Excel中内容</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("车型")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.UpdatedPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.Models.ToString())
                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.NotUpdatedPods != null && response.Result.NotUpdatedPods.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.NotUpdatedPods.Count()).Append("条)没有更新成Excel中内容,系统中无对应运单</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("车型")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.NotUpdatedPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.Models.ToString())
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
        #endregion
    }
}
