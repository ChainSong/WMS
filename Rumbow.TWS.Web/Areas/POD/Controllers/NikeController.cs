
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using Runbow.TWS.Web.Common;
using MyFile = System.IO.File;
using System.Xml;
using Runbow.TWS.Web.Areas.POD.Models;
using Runbow.TWS.Common;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;
using System.Text;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Web.Implement;
using Runbow.TWS.MessageContracts.POD.Nike;
using Runbow.TWS.Biz.POD;
using Runbow.TWS.Entity.POD.Nike;
using System.Web.Script.Serialization;



namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class NikeController : BaseController
    {
        //
        // GET: /ImportAndExport/Nike/

        public ActionResult NikeReportExport()
        {
            NikeReportExportViewModel view = new NikeReportExportViewModel();
            view.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            view.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID)
                               .Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            view.PodStates = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            view.ReportNames = GetReportType();
            return View(view);
        }

        [HttpPost]
        public ActionResult NikeReportExport(NikeReportExportViewModel view, int? PageIndex)
        {

            //if (Action == "生成运单")
            //{
            //    HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;

            //    if (file.ContentLength > 0)
            //    {
            //        DataSet ds = this.GetDataFromExcel(file);

            //        if (ds != null && ds.Tables[0] != null)
            //        {
            //            string xmlpath = Runbow.TWS.Web.Common.Constants.APPLICATIONFILEPATH;
            //            DataTable tableone = this.NewTableByXml(xmlpath);
            //            DataTable newtable = null;
            //            string modelname = file.FileName.Substring(0, file.FileName.LastIndexOf('.'));
            //            newtable = this.DataExtractionByXml(ds.Tables[0], tableone, xmlpath, modelname);//处理过后的数据
            //            view.HtmlStr = this.PodImport(newtable, modelname);

            //        }
            //    }
            //}
            //else 
            view.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                             .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //view.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID)
            //                   .Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            view.PodStates = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE)
                          .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            view.ReportNames = GetReportType();
            if (view.IsExport)
            {
                return ExprotReport(view);
            }
            else
            {
                string SQL = this.GetSqlWhere(view);

                GetNikeReportExportRequest NikeRequest = new GetNikeReportExportRequest();

                NikeRequest = new NikeService().GetNikeReportQutry(new GetNikeReportExportRequest() { SqlWhere = SQL, ReportName = view.SelectedReportID, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 }).Result;
                view.NikeReport = NikeRequest.NikeReport;
                view.RowCount = NikeRequest.RowCount;
                view.PageIndex = PageIndex ?? 0;
                view.PageCount = NikeRequest.RowCount;
            }
            return View(view);
        }


        public ActionResult ExprotReport(NikeReportExportViewModel view)
        {
            DataTable Exprottable = GenNikeExportDataByReportName(view.SelectedReportID, view);
            //string SQL = this.GetSqlWhere(view);
            string ReportName = view.SelectedReportID + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            //DataTable Exprottable = new NikeService().GetNikeReportExprot(new GetNikeReportExportRequest() { SqlWhere = SQL, ReportName = view.SelectedReportID }).Result;
            var sbHtml = new StringBuilder();
            sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            for (int i = 0; i < Exprottable.Columns.Count; i++)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", Exprottable.Columns[i].ColumnName);
            }

            sbHtml.Append("</tr>");
            for (int i = 0; i < Exprottable.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                for (int j = 0; j < Exprottable.Columns.Count; j++)
                {
                    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", Exprottable.Rows[i][j].ToString());
                }
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");

            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AppendHeader("content-disposition", "attachment;filename=\"" + Url.Encode(ReportName) + "\"");
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
            //byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            //return File(fileContents, "<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>", Url.Encode(ReportName));

        }



        public string GetSqlWhereWhenExport(NikeReportExportViewModel vm)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(vm.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (vm.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = vm.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (vm.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = vm.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {
                    customerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {

                    sb.Append(" and a.CustomerOrderNumber in ( ");
                    foreach (string s in customerOrderNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and a.CustomerOrderNumber like '%" + vm.CustomerOrderNumber.Trim() + "%' ");
                }
            }

            if (vm.SelectedPodStatesID.HasValue)
            {
                sb.Append(" and a.ShipperID=").Append(vm.SelectedPodStatesID).Append(" ");
            }

            if (!string.IsNullOrEmpty(vm.ShipperName))
            {
                sb.Append(" and a.ShipperName like '%").Append(vm.ShipperName.Trim()).Append("%' ");
            }

            if (vm.SelectedShipperTypeID.HasValue)
            {
                sb.Append(" and a.ShipperTypeID=").Append(vm.SelectedShipperTypeID.Value).Append(" ");
            }

            if (!string.IsNullOrEmpty(vm.CustomerCode))
            {
                sb.Append(" and a.Str1 like '%").Append(vm.CustomerCode.Trim()).Append("%' ");
            }

            if (vm.BeginActualShipTime.HasValue)
            {
                sb.Append(" and a.ActualDeliveryDate >='").Append(vm.BeginActualShipTime.Value.DateTimeToString()).Append(" 00:00:000' ");
            }

            if (vm.EndActualShipTime.HasValue)
            {
                sb.Append(" and a.ActualDeliveryDate <'").Append(vm.EndActualShipTime.Value.AddDays(1).DateTimeToString()).Append("' ");
            }

            if (vm.BeginActualCompleteTime.HasValue)
            {
                sb.Append(" and b.DateTime6 >='").Append(vm.BeginActualCompleteTime.Value.DateTimeToString()).Append(" 00:00:000' ");
            }

            if (vm.EndActualCompleteTime.HasValue)
            {
                sb.Append(" and b.DateTime6 <'").Append(vm.BeginActualCompleteTime.Value.AddDays(1).DateTimeToString()).Append("' ");
            }

            if (vm.BeginExpectedDeliveryTime.HasValue)
            {
                sb.Append(" and b.DateTime1 >='").Append(vm.BeginExpectedDeliveryTime.Value.DateTimeToString()).Append(" 00:00:000' ");
            }

            if (vm.EndExpectedDeliveryTime.HasValue)
            {
                sb.Append(" and b.DateTime1 <'").Append(vm.EndExpectedDeliveryTime.Value.AddDays(1).DateTimeToString()).Append("' ");
            }

            if (vm.SelectedReportID == "大仓出货-客服" || vm.SelectedReportID == "大仓出货-耐克")
            {
                sb.Append(" and a.PODTypeName='大仓出货' ");
            }

            if (vm.SelectedReportID == "退货运单-客服" || vm.SelectedReportID == "退货运单-耐克" || vm.SelectedReportID == "退货运单-召回")
            {
                sb.Append(" and a.PODTypeName='退货运单' ");
            }

            if (vm.SelectedReportID == "工厂直发-客服" || vm.SelectedReportID == "工厂直发-耐克")
            {
                sb.Append(" and a.PODTypeName='工厂直发' ");
            }

            if (vm.SelectedReportID == "门店调拨-客服" || vm.SelectedReportID == "门店调拨-耐克")
            {
                sb.Append(" and a.PODTypeName='门店调拨' ");
            }
            return sb.ToString();
        }



        public string GetSqlWhere(NikeReportExportViewModel model)
        {
            string sqlstr = "";
            if (!string.IsNullOrEmpty(model.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (model.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = model.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (model.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = model.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {
                    customerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {
                    sqlstr += "  and POD.CustomerOrderNumber in ( ";
                    foreach (string s in customerOrderNumbers)
                    {
                        sqlstr = sqlstr + "'" + s + ("',");
                    }

                    sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);

                    sqlstr += " ) ";
                }
                else
                {
                    sqlstr += " and POD.CustomerOrderNumber like '%" + model.CustomerOrderNumber.Trim() + "%' ";
                }
            }

            if (model.SelectedPodStatesID.HasValue && !string.IsNullOrEmpty(model.SelectedShipperID.ToString()) && model.SelectedShipperID.ToString() != "0")
            {

                sqlstr += " AND POD.ShipperID='" + model.SelectedShipperID + "'";
            }
            if (!string.IsNullOrEmpty(model.ShipperName) && model.ShipperID.ToString() != "0")
            {

                sqlstr += " AND POD.ShipperID='" + model.ShipperID + "'";
            }
            if (model.SelectedShipperTypeID.HasValue && !string.IsNullOrEmpty(model.SelectedShipperTypeID.ToString()) && model.SelectedShipperTypeID.ToString() != "0")
            {

                sqlstr += " AND POD.ShipperTypeID='" + model.SelectedShipperTypeID + "'";
            }
            if (!string.IsNullOrEmpty(model.SelectedPodStatesID.ToString()) && model.SelectedPodStatesID.ToString() != "0")
            {

                sqlstr += " AND POD.PODStateID='" + model.SelectedPodStatesID + "'";
            }
            if (!string.IsNullOrEmpty(model.CustomerCode))
            {
                sqlstr += " AND POD.CustomerCode='" + model.CustomerCode + "'";
            }
            if (!string.IsNullOrEmpty(model.BeginActualShipTime.ToString()))
            {
                sqlstr += " AND   POD.ActualDeliveryDate >='" + Convert.ToDateTime(model.BeginActualShipTime).ToString("yyyy-MM-dd") + "'";
            }
            if (!string.IsNullOrEmpty(model.EndActualShipTime.ToString()))
            {
                sqlstr += " AND POD.ActualDeliveryDate <='" + Convert.ToDateTime(model.EndActualShipTime).ToString("yyyy-MM-dd") + "'";
            }

            if (!string.IsNullOrEmpty(model.BeginActualCompleteTime.ToString()))
            {
                sqlstr += " AND ##PODTRACK.DateTime6 >='" + Convert.ToDateTime(model.BeginActualCompleteTime).ToString("yyyy-MM-dd") + "'";
            }
            if (!string.IsNullOrEmpty(model.EndActualCompleteTime.ToString()))
            {
                sqlstr += " AND ##PODTRACK.DateTime6 <='" + Convert.ToDateTime(model.EndActualCompleteTime).ToString("yyyy-MM-dd") + "'";
            }


            if (!string.IsNullOrEmpty(model.BeginExpectedDeliveryTime.ToString()))
            {
                sqlstr += " AND ##PODTRACK.DateTime1 >='" + Convert.ToDateTime(model.BeginExpectedDeliveryTime).ToString("yyyy-MM-dd") + "'";
            }

            if (!string.IsNullOrEmpty(model.EndExpectedDeliveryTime.ToString()))
            {
                sqlstr += " AND ##PODTRACK.DateTime1 <='" + Convert.ToDateTime(model.EndExpectedDeliveryTime).ToString("yyyy-MM-dd") + "'";
            }



            return sqlstr;

        }


        public string GetSingModelColumnName(string modelname, string CoumnName)
        {
            string ModelCoumnName = "";
            string xmlpath = Runbow.TWS.Web.Common.Constants.APPLICATIONFILEPATH;
            XmlDataDocument xml = new XmlDataDocument();//创建XML的新实例
            xml.Load(xmlpath);//加载指定路径的XML文件
            XmlNodeList list = xml.SelectNodes("Pod_For_Project/Pod_Nike");//读取指定的节点
            foreach (XmlNode node in list)//遍历节点集合
            {
                foreach (XmlNode nodeone in node.ChildNodes)//遍历节点集合下面所有的子节点
                {
                    if (nodeone.Attributes["name"].Value == modelname)
                    {
                        foreach (XmlNode nodetwo in nodeone.ChildNodes)
                        {
                            if (nodetwo.Attributes["NewPodName"].Value == CoumnName)
                            {
                                return ModelCoumnName = nodetwo.Attributes["ModelName"].Value;
                            }

                        }

                    }
                }
            }
            return ModelCoumnName;
        }



        public List<SelectListItem> GetReportType()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            string xmlpath = Runbow.TWS.Web.Common.Constants.APPLICATIONFILEPATH;
            XmlDataDocument xml = new XmlDataDocument();//创建XML的新实例
            xml.Load(xmlpath);//加载指定路径的XML文件
            XmlNodeList list = xml.SelectNodes("Pod_For_Project/Pod_Nike/Report");//读取指定的节点
            foreach (XmlNode node in list)//遍历节点集合
            {
                foreach (XmlNode nodeone in node.ChildNodes)//遍历节点集合下面所有的子节点
                {
                    items.Add(new SelectListItem() { Value = nodeone.InnerText, Text = nodeone.InnerText });

                }
            }
            return items;
        }

        public ActionResult Upload()
        {

            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            if (file.ContentLength > 0)
            {
                DataSet ds = this.GetDataFromExcel(file);

                if (ds != null && ds.Tables[0] != null)
                {
                    string xmlpath = Runbow.TWS.Web.Common.Constants.APPLICATIONFILEPATH;
                    DataTable tableone = this.NewTableByXml(xmlpath);
                    DataTable newtable = null;
                    string modelname = file.FileName.Substring(0, file.FileName.LastIndexOf('.'));
                    newtable = this.DataExtractionByXml(ds.Tables[0], tableone, xmlpath, modelname);//处理过后的数据
                    ExcelHelper excelHelper = new ExcelHelper();
                    string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
                    string fileFullPath = Path.Combine(targetPath, "NewPOD.xlsx");
                    excelHelper.CreateExcelByDataTable(fileFullPath, newtable);
                    excelHelper.Dispose();
                    string mimeType = "application/msexcel";
                    FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
                    return File(fs, mimeType, "NewPOD.xlsx");


                }
            }
            return View("Export");
        }

        /// <summary>
        /// 读取excel通过HttpPostedFileBase
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
        /// 读取excel通过path
        /// </summary>
        /// <param name="hpf"></param>
        /// <returns></returns>
        private DataSet GetDataFromExcel(string path)
        {

            Runbow.TWS.Common.ExcelHelper excelHelper = new Runbow.TWS.Common.ExcelHelper(path);
            DataSet ds = excelHelper.GetAllDataFromAllSheets();
            excelHelper.Dispose();
            MyFile.Delete(path);

            return ds;
        }


        /// <summary>
        /// 通过xml生成新的空表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable NewTableByXml(string path)
        {
            DataTable table = new DataTable("NewPod");
            XmlDataDocument xml = new XmlDataDocument();//创建XML的新实例
            xml.Load(path);//加载指定路径的XML文件
            XmlNodeList list = xml.SelectNodes("Pod_For_Project/Pod_Nike/NewPodModel");//读取指定的节点
            foreach (XmlNode node in list)//遍历节点集合
            {
                foreach (XmlNode nodeone in node.ChildNodes)//遍历节点集合下面所有的子节点
                {

                    table.Columns.Add(nodeone.InnerText.ToString(), typeof(string));
                }
            }
            return table;
        }


        /// <summary>
        /// 生成新表
        /// </summary>
        /// <param name="Handletable"></param>
        /// <param name="Generationtable"></param>
        /// <param name="path"></param>
        /// <param name="ModelHasName"></param>
        /// <returns></returns>
        public DataTable DataExtractionByXml(DataTable Handletable, DataTable Generationtable, string path, string ModelHasName)
        {

            XmlDataDocument xml = new XmlDataDocument();//创建XML的新实例
            xml.Load(path);//加载指定路径的XML文件
            XmlNodeList list = xml.SelectNodes("Pod_For_Project/Pod_Nike");//读取指定的节点
            foreach (XmlNode node in list)//遍历节点集合
            {

                foreach (XmlNode nodeone in node.ChildNodes)//遍历节点集合下面所有的子节点
                {

                    if (nodeone.Attributes["name"].Value == ModelHasName)
                    {
                        for (int i = 0; i < Handletable.Rows.Count; i++)
                        {
                            DataRow row = Generationtable.NewRow();
                            foreach (XmlNode nodetwo in nodeone.ChildNodes)
                            {
                                if (nodetwo.Attributes["ModelName"].Value.ToString() == "")
                                {
                                    if (nodetwo.InnerText.ToString() != "")
                                    {
                                        row[nodetwo.Attributes["NewPodName"].Value.ToString()] = nodetwo.InnerText.ToString();
                                    }
                                    else
                                    {
                                        row[nodetwo.Attributes["NewPodName"].Value.ToString()] = "";
                                    }
                                }
                                else
                                {
                                    row[nodetwo.Attributes["NewPodName"].Value.ToString()] = Handletable.Rows[i][nodetwo.Attributes["ModelName"].Value.ToString()].ToString();
                                }

                            }

                            Generationtable.Rows.Add(row);
                        }

                    }
                }
            }
            return Generationtable;
        }





        public string PodImport(DataTable table, string modelName)
        {

            string MessAge = "";
            // DataSet ds = this.GetDataFromExcel(path);

            if (table != null)
            {

                var project = ApplicationConfigHelper.GetApplicationConfig().ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                Module module = project.ModuleCollection.First(m => m.Id == "M001");
                IEnumerable<Column> columns = module.Tables.TableCollection.First(t => t.Name == "Pod").ColumnCollection;
                var customerColumn = columns.First(c => c.DbColumnName == "CustomerName").DisplayName;
                var customerName = table.Rows[0][customerColumn].ToString();
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
                IEnumerable<Pod> pods = this.InitPodFromDataTable(table, columns, useCustomerOrderNumber, sb, modelName);

                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    return MessAge = "<h3>运单导入失败</h3><br/>" + sb.ToString();
                    //return new { result = "<h3>运单导入失败</h3><br/>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                }

                string systemNumberBase;
                int todayPodNum = this.GetTodayPodNumber(project.PODNumberCreator, out systemNumberBase);

                pods.Each((k, p) =>
                {
                    todayPodNum++;
                    p.SystemNumber = string.Concat(systemNumberBase, (10000 + todayPodNum).ToString().Substring(1));

                    if (!useCustomerOrderNumber)
                    {
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
                    return MessAge = successSB.ToString();
                    //return new { result = successSB.ToString(), IsSuccess = true }.ToJsonString();
                }
                else
                {
                    return MessAge = "<h3>导入运单失败</h3><br/>系统忙，请稍后再试";
                    //return new { result = "<h3>导入运单失败</h3><br/>系统忙，请稍后再试", IsSuccess = false }.ToJsonString();
                }
            }

            return MessAge = "<h3>导入运单失败</h3><br/>excel内容有误！";
            //return new { result = "<h3>导入运单失败</h3><br/>excel内容有误！", IsSuccess = false }.ToJsonString();

        }



        private IEnumerable<Pod> InitPodFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb, string modelName)
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
                    string column1 = GetSingModelColumnName(modelName, columnName);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = dt.Rows[i][j].ToString();
                            if (string.IsNullOrEmpty(value))
                            {
                                sb.Append("第" + (i + 2).ToString() + "行 " + column1 + " 列 不能为空<br/>");
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
                    string column2 = GetSingModelColumnName(modelName, columnName);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                        {

                            value = dt.Rows[i][j].ToString();
                            if (string.IsNullOrEmpty(value))
                            {
                                //sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + " 不能为空<br/>");
                                sb.Append("第" + (i + 2).ToString() + "行 " + column2 + " 列 不能为空<br/>");
                                break;
                            }

                            var customer = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).FirstOrDefault(c => string.Equals(c.CustomerName, value.Trim(), StringComparison.OrdinalIgnoreCase));

                            if (customer == null)
                            {
                                sb.Append("<strong>" + value + " </strong> 在系统中不存在或当前用户无权限导入此客户运单，请先配置。<br/>");
                                break;
                            }
                            pod.CustomerName = value.Trim();
                            pod.CustomerID = customer.CustomerID;
                            break;
                        }
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "ShipperTypeName").DisplayName;
                string column4 = GetSingModelColumnName(modelName, columnName);
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
                                sb.Append("第" + (i + 2).ToString() + "行，" + column4 + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
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
                string column5 = GetSingModelColumnName(modelName, columnName);
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
                                sb.Append("第" + (i + 2).ToString() + "行，" + column5 + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
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
                string column6 = GetSingModelColumnName(modelName, columnName);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {

                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，" + column6 + " 列 不能为空<br/>");
                            break;
                        }

                        var podType = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE).FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (podType == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，" + column6 + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
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
                string column7 = GetSingModelColumnName(modelName, columnName);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {

                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，列, " + column7 + "不能为空<br/>");
                            break;
                        }

                        var startCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (startCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，" + column7 + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        pod.StartCityName = value;
                        pod.StartCityID = startCity.ID;
                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "EndCityName").DisplayName;
                string column8 = GetSingModelColumnName(modelName, columnName);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {

                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，" + column8 + "列，不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，" + column8 + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        pod.EndCityName = value;
                        pod.EndCityID = endCity.ID;
                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "ActualDeliveryDate").DisplayName;
                string column9 = GetSingModelColumnName(modelName, columnName);
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
                                sb.Append("第" + (i + 2).ToString() + "行，" + column9 + "列, <strong>" + value + "</strong> 不是日期格式，请修改。<br/>");
                            }
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "BoxNumber").DisplayName;
                string column10 = GetSingModelColumnName(modelName, columnName);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {

                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            float boxNumber;
                            if (float.TryParse(value.Trim(), out boxNumber))
                            {
                                pod.BoxNumber = boxNumber;
                            }
                            else
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，" + column10 + "列,<strong> " + value + "</strong>不是数字格式，请修改。<br/>");
                            }
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "GoodsNumber").DisplayName;
                string column11 = GetSingModelColumnName(modelName, columnName);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {

                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            float goodsNumber;
                            if (float.TryParse(value.Trim(), out goodsNumber))
                            {
                                pod.GoodsNumber = goodsNumber;
                            }
                            else
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，" + column11 + "列, <strong>" + value + "</strong>不是数字格式，请修改。<br/>");
                            }
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "Weight").DisplayName;
                string column12 = GetSingModelColumnName(modelName, columnName);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {

                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            float weight;
                            if (float.TryParse(value.Trim(), out weight))
                            {
                                pod.Weight = weight;
                            }
                            else
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，" + column12 + "列, <strong>" + value + "</strong>不是数字格式，请修改。<br/>");
                            }
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "Volume").DisplayName;
                string column13 = GetSingModelColumnName(modelName, columnName);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {

                        value = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            float volume;
                            if (float.TryParse(value.Trim(), out volume))
                            {
                                pod.Volume = volume;
                            }
                            else
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，" + column13 + "列, <strong>" + value + "</strong>不是数字格式，请修改。<br/>");
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
                            string column14 = GetSingModelColumnName(modelName, columnName);
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
                                        sb.Append("第" + (i + 2).ToString() + "行，" + column14 + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox")
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否")
                                    {
                                        if (value == "1" || value == "是")
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
                                        sb.Append("第" + (i + 2).ToString() + "行，" + column14 + "列，<strong>" + value + "</strong>必须为0，1，是，否中一个，请修改。<br />");
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




        private DataTable InitDataTableByReportName(string reportName)
        {
            DataTable dt = new DataTable();
            #region Init DataTable By reportName
            switch (reportName)
            {
                case "大仓出货-客服":
                    dt.Columns.Add("BU", typeof(string));
                    dt.Columns.Add("发货单号", typeof(string));
                    dt.Columns.Add("发货时间", typeof(string));
                    dt.Columns.Add("出发城市", typeof(string));
                    dt.Columns.Add("目的城市", typeof(string));
                    dt.Columns.Add("客户代码", typeof(string));
                    dt.Columns.Add("卸货地址", typeof(string));
                    dt.Columns.Add("箱数", typeof(string));
                    dt.Columns.Add("件数", typeof(string));
                    dt.Columns.Add("运输类型", typeof(string));
                    dt.Columns.Add("在途时限", typeof(string));
                    dt.Columns.Add("预计到达时间", typeof(string));
                    dt.Columns.Add("状态", typeof(string));
                    dt.Columns.Add("承运商(包括干线商和终端运输商)", typeof(string));
                    dt.Columns.Add("实际签收时间", typeof(string));
                    dt.Columns.Add("跟踪备注", typeof(string));
                    dt.Columns.Add("托运单备注", typeof(string));
                    for (int i = 1; i <= 15; i++)
                    {
                        dt.Columns.Add("第" + i.ToString() + "天", typeof(string));
                    }
                    dt.Columns.Add("Abnormal Issue 异常信息", typeof(string));
                    dt.Columns.Add("客户名称", typeof(string));
                    dt.Columns.Add("联系人", typeof(string));
                    dt.Columns.Add("联系方式", typeof(string));
                    dt.Columns.Add("联系人手机", typeof(string));
                    break;
                case "大仓出货-耐克":
                    dt.Columns.Add("发货单号", typeof(string));
                    dt.Columns.Add("发货时间", typeof(string));
                    dt.Columns.Add("出发城市", typeof(string));
                    dt.Columns.Add("目的城市", typeof(string));
                    dt.Columns.Add("客户代码", typeof(string));
                    dt.Columns.Add("客户名称", typeof(string));
                    dt.Columns.Add("卸货地址", typeof(string));
                    dt.Columns.Add("箱数", typeof(string));
                    dt.Columns.Add("件数", typeof(string));
                    dt.Columns.Add("运输类型", typeof(string));
                    dt.Columns.Add("在途时限", typeof(string));
                    dt.Columns.Add("预计到达时间", typeof(string));
                    dt.Columns.Add("状态", typeof(string));
                    dt.Columns.Add("承运商(包括干线商和终端运输商)", typeof(string));
                    dt.Columns.Add("实际到货时间", typeof(string));
                    dt.Columns.Add("跟踪备注", typeof(string));
                    dt.Columns.Add("Abnormal Issue 异常信息", typeof(string));
                    for (int i = 1; i <= 129; i++)
                    {
                        dt.Columns.Add("第" + i.ToString() + "天", typeof(string));
                    }
                    break;
                case "退货运单-客服":
                    dt.Columns.Add("客户代码", typeof(string));
                    dt.Columns.Add("退货类型", typeof(string));
                    dt.Columns.Add("客户名称", typeof(string));
                    dt.Columns.Add("退货编号", typeof(string));
                    dt.Columns.Add("状态", typeof(string));
                    dt.Columns.Add("通知箱数", typeof(string));
                    dt.Columns.Add("通知数量", typeof(string));
                    dt.Columns.Add("通知提货时间", typeof(string));
                    dt.Columns.Add("承运商", typeof(string));
                    dt.Columns.Add("提货城市", typeof(string));
                    dt.Columns.Add("目的城市", typeof(string));
                    dt.Columns.Add("要求退回时间", typeof(string));
                    dt.Columns.Add("提货时间", typeof(string));
                    dt.Columns.Add("货物出发日期", typeof(string));
                    dt.Columns.Add("货物到达上海日期", typeof(string));
                    dt.Columns.Add("仓库实际收货时间", typeof(string));
                    dt.Columns.Add("仓库实际收货数量", typeof(string));
                    dt.Columns.Add("提货是否及时", typeof(string));
                    dt.Columns.Add("返货是否及时", typeof(string));
                    dt.Columns.Add("异常备注", typeof(string));
                    dt.Columns.Add("责任归属", typeof(string));
                    dt.Columns.Add("货物情况", typeof(string));
                    dt.Columns.Add("在途情况", typeof(string));
                    dt.Columns.Add("预计退仓时间", typeof(string));
                    dt.Columns.Add("提货实收件数", typeof(string));
                    dt.Columns.Add("提货实收箱数", typeof(string));
                    dt.Columns.Add("RSO编号", typeof(string));
                    dt.Columns.Add("联系人", typeof(string));
                    dt.Columns.Add("联系电话", typeof(string));
                    dt.Columns.Add("提货地址", typeof(string));
                    dt.Columns.Add("货物性质", typeof(string));
                    break;
                case "退货运单-耐克":
                    dt.Columns.Add("客户代码", typeof(string));
                    dt.Columns.Add("退货类型", typeof(string));
                    dt.Columns.Add("客户名称", typeof(string));
                    dt.Columns.Add("退货编号", typeof(string));
                    dt.Columns.Add("状态", typeof(string));
                    dt.Columns.Add("通知箱数", typeof(string));
                    dt.Columns.Add("通知数量", typeof(string));
                    dt.Columns.Add("通知提货时间", typeof(string));
                    dt.Columns.Add("承运商", typeof(string));
                    dt.Columns.Add("提货城市", typeof(string));
                    dt.Columns.Add("目的城市", typeof(string));
                    dt.Columns.Add("要求退回时间", typeof(string));
                    dt.Columns.Add("提货时间", typeof(string));
                    dt.Columns.Add("实际收货时间", typeof(string));
                    dt.Columns.Add("实收数量", typeof(string));
                    dt.Columns.Add("提货是否及时（7天内从客户处提取货物）", typeof(string));
                    dt.Columns.Add("返货是否及时", typeof(string));
                    dt.Columns.Add("异常备注", typeof(string));
                    dt.Columns.Add("责任归属", typeof(string));
                    dt.Columns.Add("货物情况", typeof(string));
                    dt.Columns.Add("在途情况", typeof(string));
                    dt.Columns.Add("预计退仓时间", typeof(string));
                    dt.Columns.Add("提货实收件数", typeof(string));
                    dt.Columns.Add("提货实收箱数", typeof(string));
                    break;
                case "退货运单-召回":
                    dt.Columns.Add("客户代码", typeof(string));
                    dt.Columns.Add("退货类型", typeof(string));
                    dt.Columns.Add("客户名称", typeof(string));
                    dt.Columns.Add("退货编号", typeof(string));
                    dt.Columns.Add("状态", typeof(string));
                    dt.Columns.Add("通知箱数", typeof(string));
                    dt.Columns.Add("通知数量", typeof(string));
                    dt.Columns.Add("通知提货时间", typeof(string));
                    dt.Columns.Add("承运商", typeof(string));
                    dt.Columns.Add("提货城市", typeof(string));
                    dt.Columns.Add("目的城市", typeof(string));
                    dt.Columns.Add("要求退回时间", typeof(string));
                    dt.Columns.Add("提货时间", typeof(string));
                    dt.Columns.Add("仓库实际收货时间", typeof(string));
                    dt.Columns.Add("仓库实际收货数量", typeof(string));
                    dt.Columns.Add("提货是否及时", typeof(string));
                    dt.Columns.Add("返货是否及时", typeof(string));
                    dt.Columns.Add("异常备注", typeof(string));
                    dt.Columns.Add("责任归属", typeof(string));
                    dt.Columns.Add("货物情况", typeof(string));
                    dt.Columns.Add("预计到仓时间", typeof(string));
                    dt.Columns.Add("预计退仓时间", typeof(string));
                    dt.Columns.Add("提货实收件数", typeof(string));
                    dt.Columns.Add("RSO编号", typeof(string));
                    break;
                case "工厂直发-客服":
                    dt.Columns.Add("发货号/DAMCO WMS Packing List#", typeof(string));
                    dt.Columns.Add("发货日期", typeof(string));
                    dt.Columns.Add("出发城市", typeof(string));
                    dt.Columns.Add("目的城市", typeof(string));
                    dt.Columns.Add("目的省份", typeof(string));
                    dt.Columns.Add("收货人代码", typeof(string));
                    dt.Columns.Add("客户名称", typeof(string));
                    dt.Columns.Add("联系人", typeof(string));
                    dt.Columns.Add("联系方式", typeof(string));
                    dt.Columns.Add("卸货地址", typeof(string));
                    dt.Columns.Add("箱数", typeof(string));
                    dt.Columns.Add("件数", typeof(string));
                    dt.Columns.Add("产品类型", typeof(string));
                    dt.Columns.Add("陆运/铁运/空运", typeof(string));
                    dt.Columns.Add("在途时间", typeof(string));
                    dt.Columns.Add("预计到货日期", typeof(string));
                    dt.Columns.Add("状态", typeof(string));
                    dt.Columns.Add("承运商(包括干线商和终端运输商)", typeof(string));
                    dt.Columns.Add("实际到货时间", typeof(string));
                    dt.Columns.Add("跟踪备注", typeof(string));
                    for (int i = 1; i <= 12; i++)
                    {
                        dt.Columns.Add("第" + i.ToString() + "天", typeof(string));
                    }
                    dt.Columns.Add("Abnormal Issue", typeof(string));
                    dt.Columns.Add("Import VL B/L#  or AF HAWB#", typeof(string));
                    dt.Columns.Add("PO#", typeof(string));
                    dt.Columns.Add("PO ITEM#", typeof(string));
                    break;
                case "工厂直发-耐克":
                    dt.Columns.Add("发货号/DAMCO WMS Packing List#", typeof(string));
                    dt.Columns.Add("发货日期", typeof(string));
                    dt.Columns.Add("出发城市", typeof(string));
                    dt.Columns.Add("目的城市", typeof(string));
                    dt.Columns.Add("目的省份", typeof(string));
                    dt.Columns.Add("收货人代码", typeof(string));
                    dt.Columns.Add("收货人", typeof(string));
                    dt.Columns.Add("卸货地址", typeof(string));
                    dt.Columns.Add("箱数", typeof(string));
                    dt.Columns.Add("件数", typeof(string));
                    dt.Columns.Add("产品类型", typeof(string));
                    dt.Columns.Add("陆运/铁运/空运", typeof(string));
                    dt.Columns.Add("在途时间", typeof(string));
                    dt.Columns.Add("预计到货日期", typeof(string));
                    dt.Columns.Add("状态", typeof(string));
                    dt.Columns.Add("承运商(包括干线商和终端运输商)", typeof(string));
                    dt.Columns.Add("实际到货时间", typeof(string));
                    dt.Columns.Add("跟踪备注", typeof(string));
                    for (int i = 1; i <= 12; i++)
                    {
                        dt.Columns.Add("第" + i.ToString() + "天", typeof(string));
                    }
                    dt.Columns.Add("Abnormal Issue", typeof(string));
                    dt.Columns.Add("Import VL B/L#  or AF HAWB#", typeof(string));
                    dt.Columns.Add("PO#", typeof(string));
                    dt.Columns.Add("PO ITEM#", typeof(string));
                    break;
                case "门店调拨-客服":
                    dt.Columns.Add("单号", typeof(string));
                    dt.Columns.Add("订单日期", typeof(string));
                    dt.Columns.Add("出发城市", typeof(string));
                    dt.Columns.Add("调出店", typeof(string));
                    dt.Columns.Add("调出店联系方式", typeof(string));
                    dt.Columns.Add("目的城市", typeof(string));
                    dt.Columns.Add("客户代码", typeof(string));
                    dt.Columns.Add("卸货地址", typeof(string));
                    dt.Columns.Add("调入店", typeof(string));
                    dt.Columns.Add("调入店联系方式", typeof(string));
                    dt.Columns.Add("箱数", typeof(string));
                    dt.Columns.Add("在途时间", typeof(string));
                    dt.Columns.Add("预计提货日期", typeof(string));
                    dt.Columns.Add("实际提货日期", typeof(string));
                    dt.Columns.Add("预计到达时间", typeof(string));
                    dt.Columns.Add("实际到达日期", typeof(string));
                    dt.Columns.Add("状态", typeof(string));
                    dt.Columns.Add("承运商(包括干线商和终端运输商)", typeof(string));
                    dt.Columns.Add("跟踪备注", typeof(string));
                    for (int i = 1; i <= 12; i++)
                    {
                        dt.Columns.Add("第" + i.ToString() + "天", typeof(string));
                    }
                    dt.Columns.Add("备注", typeof(string));
                    break;
                case "门店调拨-耐克":
                    dt.Columns.Add("单号", typeof(string));
                    dt.Columns.Add("订单日期", typeof(string));
                    dt.Columns.Add("出发城市", typeof(string));
                    dt.Columns.Add("调出店", typeof(string));
                    dt.Columns.Add("目的城市", typeof(string));
                    dt.Columns.Add("目的城市客户代码", typeof(string));
                    dt.Columns.Add("卸货地址", typeof(string));
                    dt.Columns.Add("调入店", typeof(string));
                    dt.Columns.Add("箱数", typeof(string));
                    dt.Columns.Add("在途时间", typeof(string));
                    dt.Columns.Add("预计提货日期", typeof(string));
                    dt.Columns.Add("实际提货日期", typeof(string));
                    dt.Columns.Add("预计到达时间", typeof(string));
                    dt.Columns.Add("实际到达日期", typeof(string));
                    dt.Columns.Add("状态", typeof(string));
                    dt.Columns.Add("承运商(包括干线商和终端运输商)", typeof(string));
                    dt.Columns.Add("跟踪备注", typeof(string));
                    for (int i = 1; i <= 12; i++)
                    {
                        dt.Columns.Add("第" + i.ToString() + "天", typeof(string));
                    }
                    dt.Columns.Add("备注", typeof(string));
                    break;

            }
            #endregion

            return dt;
        }

        public DataTable GenNikeExportDataByReportName(string reportName, NikeReportExportViewModel vm)
        {
            DataTable dt = this.InitDataTableByReportName(reportName);
            var response = new NikeService().GetNikeExportPodAllByCondition(new GetNikeExportPodAllByConditionRequest() { Condition = this.GetSqlWhereWhenExport(vm) });
            if (response.IsSuccess)
            {
                response.Result.Each((i, podAll) =>
                {
                    var pod = podAll.Pod;
                    var podTracks = podAll.PodTracks.OrderBy(ptrack => ptrack.DateTime1);
                    PodTrack lastTrack = null;
                    if (podTracks.Count() > 0)
                    {
                        lastTrack = podTracks.Last();
                    }

                    DataRow dr = dt.NewRow();
                    switch (reportName)
                    {
                        case "大仓出货-客服":
                            dr["BU"] = pod.Str23;
                            dr["发货单号"] = pod.CustomerOrderNumber;
                            dr["发货时间"] = pod.ActualDeliveryDate.HasValue ? pod.ActualDeliveryDate.Value.DateTimeToString() : "";
                            dr["出发城市"] = pod.StartCityName;
                            dr["目的城市"] = pod.EndCityName;
                            dr["客户代码"] = pod.Str1;
                            dr["卸货地址"] = pod.Str7;
                            dr["箱数"] = pod.BoxNumber.HasValue ? pod.BoxNumber.Value.ToString() : "";
                            dr["件数"] = pod.GoodsNumber.HasValue ? pod.GoodsNumber.Value.ToString() : "";
                            dr["运输类型"] = pod.ShipperTypeName;
                            dr["在途时限"] = pod.Str15;
                            dr["预计到达时间"] = pod.DateTime6.HasValue ? (pod.ShipperTypeName == "空运" ? pod.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : pod.DateTime6.Value.DateTimeToString()) : "";
                            dr["状态"] = lastTrack != null ? lastTrack.Str6 : "";
                            dr["承运商(包括干线商和终端运输商)"] = pod.ShipperName;
                            dr["实际签收时间"] = lastTrack != null ? (lastTrack.DateTime6.HasValue ? pod.ShipperTypeName == "空运" ? lastTrack.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : lastTrack.DateTime6.Value.DateTimeToString() : "") : "";
                            dr["跟踪备注"] = lastTrack != null ? lastTrack.Str10 : "";
                            dr["托运单备注"] = pod.Str37;
                            podTracks.Each((j, track) =>
                            {
                                if (pod.ActualDeliveryDate.HasValue && track.DateTime1.HasValue)
                                {
                                    var dayDiff = (track.DateTime1.Value - pod.ActualDeliveryDate.Value).Days;
                                    if (dayDiff > 0 && dayDiff <= 15)
                                    {
                                        dr["第" + dayDiff + "天"] = track.Str12;
                                    }
                                }
                            });
                            dr["Abnormal Issue 异常信息"] = pod.Str38;
                            dr["客户名称"] = pod.Str3;
                            dr["联系人"] = pod.Str4;
                            dr["联系方式"] = pod.Str5;
                            dr["联系人手机"] = pod.Str6;
                            break;
                        case "大仓出货-耐克":
                            dr["发货单号"] = pod.CustomerOrderNumber;
                            dr["发货时间"] = pod.ActualDeliveryDate.HasValue ? pod.ActualDeliveryDate.Value.DateTimeToString() : "";
                            dr["出发城市"] = pod.StartCityName;
                            dr["目的城市"] = pod.EndCityName;
                            dr["客户代码"] = pod.Str1;
                            dr["客户名称"] = pod.Str3;
                            dr["卸货地址"] = pod.Str7;
                            dr["箱数"] = pod.BoxNumber.HasValue ? pod.BoxNumber.Value.ToString() : "";
                            dr["件数"] = pod.GoodsNumber.HasValue ? pod.GoodsNumber.Value.ToString() : "";
                            dr["运输类型"] = pod.ShipperTypeName;
                            dr["在途时限"] = pod.Str15;
                            dr["预计到达时间"] = pod.DateTime6.HasValue ? (pod.ShipperTypeName == "空运" ? pod.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : pod.DateTime6.Value.DateTimeToString()) : "";
                            dr["状态"] = lastTrack != null ? lastTrack.Str6 : "";
                            dr["承运商(包括干线商和终端运输商)"] = pod.ShipperName;
                            dr["实际到货时间"] = lastTrack != null ? (lastTrack.DateTime6.HasValue ? pod.ShipperTypeName == "空运" ? lastTrack.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : lastTrack.DateTime6.Value.DateTimeToString() : "") : "";
                            dr["跟踪备注"] = lastTrack != null ? lastTrack.Str10 : "";
                            dr["Abnormal Issue 异常信息"] = pod.Str38;
                            podTracks.Each((j, track) =>
                            {
                                if (pod.ActualDeliveryDate.HasValue && track.DateTime1.HasValue)
                                {
                                    var dayDiff = (track.DateTime1.Value - pod.ActualDeliveryDate.Value).Days;
                                    if (dayDiff > 0 && dayDiff <= 129)
                                    {
                                        dr["第" + dayDiff + "天"] = track.Str12;
                                    }
                                }
                            });
                            break;
                        case "退货运单-客服":
                            dr["客户代码"] = pod.Str1;
                            dr["退货类型"] = pod.Str27;
                            dr["客户名称"] = pod.Str3;
                            dr["退货编号"] = pod.CustomerOrderNumber;
                            dr["状态"] = lastTrack != null ? lastTrack.Str6 : "";
                            dr["通知数量"] = pod.Str8;
                            dr["通知箱数"] = pod.Str9;
                            dr["通知提货时间"] = pod.ActualDeliveryDate.HasValue ? pod.ActualDeliveryDate.Value.DateTimeToString() : "";
                            dr["承运商"] = pod.ShipperName;
                            dr["提货城市"] = pod.StartCityName;
                            dr["目的城市"] = pod.EndCityName;
                            dr["要求退回时间"] = pod.DateTime6.HasValue ? pod.DateTime6.Value.DateTimeToString() : "";
                            dr["提货时间"] = lastTrack != null ? (lastTrack.DateTime3.HasValue ? lastTrack.DateTime3.Value.DateTimeToString() : "") : "";
                            dr["货物出发日期"] = lastTrack != null ? (lastTrack.DateTime4.HasValue ? lastTrack.DateTime4.Value.DateTimeToString() : "") : "";
                            dr["货物到达上海日期"] = lastTrack != null ? (lastTrack.DateTime5.HasValue ? lastTrack.DateTime5.Value.DateTimeToString() : "") : "";
                            dr["仓库实际收货时间"] = lastTrack != null ? (lastTrack.DateTime8.HasValue ? lastTrack.DateTime8.Value.DateTimeToString() : "") : "";
                            dr["仓库实际收货数量"] = lastTrack != null ? lastTrack.Str14 : "";
                            if (lastTrack != null && !string.IsNullOrEmpty(lastTrack.Str1))
                            {
                                dr["提货是否及时"] = lastTrack.Str1 == "1" ? "是" : "否";
                            }
                            if (lastTrack != null && !string.IsNullOrEmpty(lastTrack.Str2))
                            {
                                dr["返货是否及时"] = lastTrack.Str2 == "1" ? "是" : "否";
                            }
                            dr["异常备注"] = lastTrack != null ? lastTrack.Str10 : "";
                            dr["责任归属"] = lastTrack != null ? lastTrack.Str5 : "";
                            dr["货物情况"] = lastTrack != null ? lastTrack.Str3 : "";
                            dr["在途情况"] = lastTrack != null ? lastTrack.Str12 : "";
                            dr["预计退仓时间"] = lastTrack != null ? (lastTrack.DateTime7.HasValue ? lastTrack.DateTime7.Value.DateTimeToString() : "") : "";
                            dr["提货实收件数"] = lastTrack != null ? lastTrack.Str7 : "";
                            dr["提货实收箱数"] = lastTrack != null ? lastTrack.Str8 : "";
                            dr["RSO编号"] = pod.Str28;
                            dr["联系人"] = pod.Str4;
                            dr["联系电话"] = pod.Str5;
                            dr["提货地址"] = pod.Str22;
                            dr["货物性质"] = pod.Str23;
                            break;
                        case "退货运单-耐克":
                            dr["客户代码"] = pod.Str1;
                            dr["退货类型"] = pod.Str27;
                            dr["客户名称"] = pod.Str3;
                            dr["退货编号"] = pod.CustomerOrderNumber;
                            dr["状态"] = lastTrack != null ? lastTrack.Str6 : "";
                            dr["通知数量"] = pod.Str8;
                            dr["通知箱数"] = pod.Str9;
                            dr["通知提货时间"] = pod.ActualDeliveryDate.HasValue ? pod.ActualDeliveryDate.Value.DateTimeToString() : "";
                            dr["承运商"] = pod.ShipperName;
                            dr["提货城市"] = pod.StartCityName;
                            dr["目的城市"] = pod.EndCityName;
                            dr["要求退回时间"] = pod.DateTime6.HasValue ? pod.DateTime6.Value.DateTimeToString() : "";
                            dr["提货时间"] = lastTrack != null ? (lastTrack.DateTime3.HasValue ? lastTrack.DateTime3.Value.DateTimeToString() : "") : "";
                            dr["实际收货时间"] = lastTrack != null ? (lastTrack.DateTime8.HasValue ? lastTrack.DateTime8.Value.DateTimeToString() : "") : "";
                            dr["实收数量"] = lastTrack != null ? lastTrack.Str14 : "";
                            if (lastTrack != null && !string.IsNullOrEmpty(lastTrack.Str1))
                            {
                                dr["提货是否及时（7天内从客户处提取货物）"] = lastTrack.Str1 == "1" ? "是" : "否";
                            }
                            if (lastTrack != null && !string.IsNullOrEmpty(lastTrack.Str2))
                            {
                                dr["返货是否及时"] = lastTrack.Str2 == "1" ? "是" : "否";
                            }
                            dr["异常备注"] = lastTrack != null ? lastTrack.Str10 : "";
                            dr["责任归属"] = lastTrack != null ? lastTrack.Str5 : "";
                            dr["货物情况"] = lastTrack != null ? lastTrack.Str3 : "";
                            dr["在途情况"] = lastTrack != null ? lastTrack.Str12 : "";
                            dr["预计退仓时间"] = lastTrack != null ? (lastTrack.DateTime7.HasValue ? lastTrack.DateTime7.Value.DateTimeToString() : "") : "";
                            dr["提货实收件数"] = lastTrack != null ? lastTrack.Str7 : "";
                            dr["提货实收箱数"] = lastTrack != null ? lastTrack.Str8 : "";
                            break;
                        case "退货运单-召回":
                            dr["客户代码"] = pod.Str1;
                            dr["退货类型"] = pod.Str27;
                            dr["客户名称"] = pod.Str3;
                            dr["退货编号"] = pod.CustomerOrderNumber;
                            dr["状态"] = lastTrack != null ? lastTrack.Str6 : "";
                            dr["通知数量"] = pod.Str8;
                            dr["退货编号"] = pod.Str9;
                            dr["通知提货时间"] = pod.ActualDeliveryDate.HasValue ? pod.ActualDeliveryDate.Value.DateTimeToString() : "";
                            dr["承运商"] = pod.ShipperName;
                            dr["提货城市"] = pod.StartCityName;
                            dr["目的城市"] = pod.EndCityName;
                            dr["要求退回时间"] = pod.DateTime6.HasValue ? pod.DateTime6.Value.DateTimeToString() : "";
                            dr["提货时间"] = lastTrack != null ? (lastTrack.DateTime3.HasValue ? lastTrack.DateTime3.Value.DateTimeToString() : "") : "";
                            dr["仓库实际收货时间"] = lastTrack != null ? (lastTrack.DateTime8.HasValue ? lastTrack.DateTime8.Value.DateTimeToString() : "") : "";
                            dr["仓库实际收货数量"] = lastTrack != null ? lastTrack.Str14 : "";
                            if (lastTrack != null && !string.IsNullOrEmpty(lastTrack.Str1))
                            {
                                dr["提货是否及时"] = lastTrack.Str1 == "1" ? "是" : "否";
                            }
                            if (lastTrack != null && !string.IsNullOrEmpty(lastTrack.Str2))
                            {
                                dr["返货是否及时"] = lastTrack.Str2 == "1" ? "是" : "否";
                            }
                            dr["异常备注"] = lastTrack != null ? lastTrack.Str10 : "";
                            dr["责任归属"] = lastTrack != null ? lastTrack.Str5 : "";
                            dr["货物情况"] = lastTrack != null ? lastTrack.Str3 : "";
                            dr["预计到仓时间"] = lastTrack != null ? (lastTrack.DateTime6.HasValue ? lastTrack.DateTime6.Value.DateTimeToString() : "") : "";
                            dr["预计退仓时间"] = lastTrack != null ? (lastTrack.DateTime7.HasValue ? lastTrack.DateTime7.Value.DateTimeToString() : "") : "";
                            dr["提货实收件数"] = lastTrack != null ? lastTrack.Str7 : "";
                            dr["RSO编号"] = pod.Str28;
                            break;
                        case "工厂直发-客服":
                            dr["发货号/DAMCO WMS Packing List#"] = pod.CustomerOrderNumber.Substring(0, pod.CustomerOrderNumber.Length - pod.Str25.Length - pod.Str26.Length);
                            dr["发货日期"] = pod.ActualDeliveryDate.HasValue ? pod.ActualDeliveryDate.Value.DateTimeToString() : "";
                            dr["出发城市"] = pod.StartCityName;
                            dr["目的城市"] = pod.EndCityName;
                            dr["目的省份"] = pod.Str2;
                            dr["收货人代码"] = pod.Str1;
                            dr["客户名称"] = pod.Str3;
                            dr["联系人"] = pod.Str4;
                            dr["联系方式"] = pod.Str5;
                            dr["卸货地址"] = pod.Str7;
                            dr["箱数"] = pod.BoxNumber.HasValue ? pod.BoxNumber.Value.ToString() : "";
                            dr["件数"] = pod.GoodsNumber.HasValue ? pod.GoodsNumber.Value.ToString() : "";
                            dr["产品类型"] = pod.Str23;
                            dr["陆运/铁运/空运"] = pod.ShipperTypeName;
                            dr["在途时间"] = pod.Str15;
                            dr["预计到货日期"] = pod.DateTime6.HasValue ? (pod.ShipperTypeName == "空运" ? pod.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : pod.DateTime6.Value.DateTimeToString()) : "";
                            dr["状态"] = lastTrack != null ? lastTrack.Str6 : "";
                            dr["承运商(包括干线商和终端运输商)"] = pod.ShipperName;
                            dr["实际到货时间"] = lastTrack != null ? (lastTrack.DateTime6.HasValue ? pod.ShipperTypeName == "空运" ? lastTrack.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : lastTrack.DateTime6.Value.DateTimeToString() : "") : "";
                            dr["跟踪备注"] = lastTrack != null ? lastTrack.Str10 : "";
                            podTracks.Each((j, track) =>
                            {
                                if (pod.ActualDeliveryDate.HasValue && track.DateTime1.HasValue)
                                {
                                    var dayDiff = (track.DateTime1.Value - pod.ActualDeliveryDate.Value).Days;
                                    if (dayDiff > 0 && dayDiff <= 12)
                                    {
                                        dr["第" + dayDiff + "天"] = track.Str12;
                                    }
                                }
                            });
                            dr["Abnormal Issue"] = pod.Str38;
                            dr["Import VL B/L#  or AF HAWB#"] = pod.Str24;
                            dr["PO#"] = pod.Str25;
                            dr["PO ITEM#"] = pod.Str26;
                            break;
                        case "工厂直发-耐克":
                            dr["发货号/DAMCO WMS Packing List#"] = pod.CustomerOrderNumber.Substring(0, pod.CustomerOrderNumber.Length - pod.Str25.Length - pod.Str26.Length);
                            dr["发货日期"] = pod.ActualDeliveryDate.HasValue ? pod.ActualDeliveryDate.Value.DateTimeToString() : "";
                            dr["出发城市"] = pod.StartCityName;
                            dr["目的城市"] = pod.EndCityName;
                            dr["目的省份"] = pod.Str2;
                            dr["收货人代码"] = pod.Str1;
                            dr["收货人"] = pod.Str3;
                            dr["卸货地址"] = pod.Str7;
                            dr["箱数"] = pod.BoxNumber.HasValue ? pod.BoxNumber.Value.ToString() : "";
                            dr["件数"] = pod.GoodsNumber.HasValue ? pod.GoodsNumber.Value.ToString() : "";
                            dr["产品类型"] = pod.Str23;
                            dr["陆运/铁运/空运"] = pod.ShipperTypeName;
                            dr["在途时间"] = pod.Str15;
                            dr["预计到货日期"] = pod.DateTime6.HasValue ? (pod.ShipperTypeName == "空运" ? pod.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : pod.DateTime6.Value.DateTimeToString()) : "";
                            dr["状态"] = lastTrack != null ? lastTrack.Str6 : "";
                            dr["承运商(包括干线商和终端运输商)"] = pod.ShipperName;
                            dr["实际到货时间"] = lastTrack != null ? (lastTrack.DateTime6.HasValue ? pod.ShipperTypeName == "空运" ? lastTrack.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : lastTrack.DateTime6.Value.DateTimeToString() : "") : "";
                            dr["跟踪备注"] = lastTrack != null ? lastTrack.Str10 : "";
                            podTracks.Each((j, track) =>
                            {
                                if (pod.ActualDeliveryDate.HasValue && track.DateTime1.HasValue)
                                {
                                    var dayDiff = (track.DateTime1.Value - pod.ActualDeliveryDate.Value).Days;
                                    if (dayDiff > 0 && dayDiff <= 12)
                                    {
                                        dr["第" + dayDiff + "天"] = track.Str12;
                                    }
                                }
                            });
                            dr["Abnormal Issue"] = pod.Str38;
                            dr["Import VL B/L#  or AF HAWB#"] = pod.Str24;
                            dr["PO#"] = pod.Str25;
                            dr["PO ITEM#"] = pod.Str26;
                            break;
                        case "门店调拨-客服":
                            dr["单号"] = pod.CustomerOrderNumber;
                            dr["订单日期"] = pod.ActualDeliveryDate.HasValue ? pod.ActualDeliveryDate.Value.DateTimeToString() : "";
                            dr["出发城市"] = pod.StartCityName;
                            dr["调出店"] = pod.Str11;
                            dr["调出店联系方式"] = pod.Str12;
                            dr["目的城市"] = pod.EndCityName;
                            dr["客户代码"] = pod.Str1;
                            dr["卸货地址"] = pod.Str7;
                            dr["调入店"] = pod.Str13;
                            dr["调入店联系方式"] = pod.Str14;
                            dr["箱数"] = pod.BoxNumber.HasValue ? pod.BoxNumber.Value.ToString() : "";
                            dr["在途时间"] = pod.Str15;
                            dr["预计提货日期"] = lastTrack != null ? (lastTrack.DateTime2.HasValue ? lastTrack.DateTime2.Value.DateTimeToString() : "") : "";
                            dr["实际提货日期"] = lastTrack != null ? (lastTrack.DateTime3.HasValue ? lastTrack.DateTime3.Value.DateTimeToString() : "") : "";
                            dr["预计到达时间"] = pod.DateTime6.HasValue ? (pod.ShipperTypeName == "空运" ? pod.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : pod.DateTime6.Value.DateTimeToString()) : "";
                            dr["实际到达日期"] = lastTrack != null ? (lastTrack.DateTime6.HasValue ? lastTrack.DateTime6.Value.DateTimeToString() : "") : "";
                            dr["状态"] = lastTrack != null ? lastTrack.Str6 : "";
                            dr["承运商(包括干线商和终端运输商)"] = pod.ShipperName;
                            dr["跟踪备注"] = lastTrack != null ? lastTrack.Str10 : "";
                            podTracks.Each((j, track) =>
                            {
                                if (pod.ActualDeliveryDate.HasValue && track.DateTime1.HasValue)
                                {
                                    var dayDiff = (track.DateTime1.Value - pod.ActualDeliveryDate.Value).Days;
                                    if (dayDiff > 0 && dayDiff <= 12)
                                    {
                                        dr["第" + dayDiff + "天"] = track.Str12;
                                    }
                                }
                            });
                            dr["备注"] = pod.Str36;
                            break;
                        case "门店调拨-耐克":
                            dr["单号"] = pod.CustomerOrderNumber;
                            dr["订单日期"] = pod.ActualDeliveryDate.HasValue ? pod.ActualDeliveryDate.Value.DateTimeToString() : "";
                            dr["出发城市"] = pod.StartCityName;
                            dr["调出店"] = pod.Str11;
                            dr["目的城市"] = pod.EndCityName;
                            dr["目的城市客户代码"] = pod.Str1;
                            dr["卸货地址"] = pod.Str7;
                            dr["调入店"] = pod.Str13;
                            dr["箱数"] = pod.BoxNumber.HasValue ? pod.BoxNumber.Value.ToString() : "";
                            dr["在途时间"] = pod.Str15;
                            dr["预计提货日期"] = lastTrack != null ? (lastTrack.DateTime2.HasValue ? lastTrack.DateTime2.Value.DateTimeToString() : "") : "";
                            dr["实际提货日期"] = lastTrack != null ? (lastTrack.DateTime3.HasValue ? lastTrack.DateTime3.Value.DateTimeToString() : "") : "";
                            dr["预计到达时间"] = pod.DateTime6.HasValue ? (pod.ShipperTypeName == "空运" ? pod.DateTime6.Value.ToString("yyyy-MM-dd HH:mm") : pod.DateTime6.Value.DateTimeToString()) : "";
                            dr["实际到达日期"] = lastTrack != null ? (lastTrack.DateTime6.HasValue ? lastTrack.DateTime6.Value.DateTimeToString() : "") : "";
                            dr["状态"] = lastTrack != null ? lastTrack.Str6 : "";
                            dr["承运商(包括干线商和终端运输商)"] = pod.ShipperName;
                            dr["跟踪备注"] = lastTrack != null ? lastTrack.Str10 : "";
                            podTracks.Each((j, track) =>
                            {
                                if (pod.ActualDeliveryDate.HasValue && track.DateTime1.HasValue)
                                {
                                    var dayDiff = (track.DateTime1.Value - pod.ActualDeliveryDate.Value).Days;
                                    if (dayDiff > 0 && dayDiff <= 12)
                                    {
                                        dr["第" + dayDiff + "天"] = track.Str12;
                                    }
                                }
                            });
                            dr["备注"] = pod.Str36;
                            break;
                    }
                    dt.Rows.Add(dr);
                });
            }

            return dt;
        }

        [HttpGet]
        public ActionResult UpdateNikePodSettledInfo(int? id)
        {
            return View();
        }

        [HttpPost]
        public string UpdateNikePodSettledInfo()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<UpdateNikePod> pods = new List<UpdateNikePod>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            pods.Add(new UpdateNikePod()
                            {
                                CustomerOrderNumber = ds.Tables[0].Rows[i]["客户运单号"].ToString().Trim(),
                                Weight = ds.Tables[0].Rows[i]["重量"].ToString().Trim().ObjectToDouble(),
                                TS = ds.Tables[0].Rows[i]["箱数"].ToString().Trim().ObjectToDouble(),
                                SS = ds.Tables[0].Rows[i]["立方"].ToString().Trim().ObjectToDouble(),
                                EndCityName = ds.Tables[0].Rows[i]["目的城市"].ToString().Trim(),
                                ActualDeliveryDate = ds.Tables[0].Rows[i]["发货日期"].ToString().Trim().ObjectToDateTime(),
                                StartCityName = ds.Tables[0].Rows[i]["起运城市"].ToString().Trim()
                            });
                        }

                        var groupedPods = from p in pods group p by new { p.CustomerOrderNumber, p.ActualDeliveryDate, p.EndCityName, p.StartCityName } into g select new UpdateNikePod() { CustomerOrderNumber = g.Key.CustomerOrderNumber, Weight = g.Sum(k => k.Weight), TS = g.Sum(k => k.TS), SS = g.Sum(k => k.SS), EndCityName = g.Key.EndCityName, ActualDeliveryDate = g.Key.ActualDeliveryDate, StartCityName = g.Key.StartCityName };
                        var response = new NikeService().UpdateNikePodAndGetTheDifference(new UpdateNikePodAndGetTheDifferenceRequest() { Pods = groupedPods });
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
                                    .Append("</th><th>").Append("箱数")
                                    .Append("</th><th>").Append("升数")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.UpdatedPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                                        .Append("</td><td>").Append(o.Weight.ToString())
                                        .Append("</td><td>").Append(o.TS.ToString())
                                        .Append("</td><td>").Append(o.SS.ToString())
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
                                    .Append("</th><th>").Append("箱数")
                                    .Append("</th><th>").Append("升数")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.NotUpdatedPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                                        .Append("</td><td>").Append(o.Weight.ToString())
                                        .Append("</td><td>").Append(o.TS.ToString())
                                        .Append("</td><td>").Append(o.SS.ToString())
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

                            if (response.Result.StartCityNotMatchPods != null && response.Result.StartCityNotMatchPods.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.StartCityNotMatchPods.Count()).Append("条)与系统中运单起运城市不一致</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("实际发货日期")
                                    .Append("</th><th>").Append("起运城市")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.StartCityNotMatchPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.ActualDeliveryDate.DateTimeToString())
                                        .Append("</td><td>").Append(o.StartCityName)
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
        public ActionResult UpdateNikePod(int? id)
        {
            return View();
        }

        [HttpPost]
        public string UpdateNikePod()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = this.GetDataFromExcel(hpf);

                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<UpdateNikePodBG> pods = new List<UpdateNikePodBG>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            pods.Add(new UpdateNikePodBG()
                            {
                                CustomerOrderNumber = ds.Tables[0].Rows[i]["客户运单号"].ToString().Trim(),
                                BoxNumber = ds.Tables[0].Rows[i]["箱数"].ToString().Trim().ToDouble(),
                                GoodsNumber = ds.Tables[0].Rows[i]["件数"].ToString().Trim().ToDouble()
                            });
                        }
                        var response = new NikeService().UpdateNikePodBGAndGetTheDifference(new UpdateNikePodBGAndGetTheDifferenceRequest() { Pods = pods });
                        if (response.IsSuccess)
                        {
                            StringBuilder successSB = new StringBuilder();
                            successSB.Append("<h3>共导入数据").Append(pods.Count).Append("条.<br/>");
                            if (response.Result.UpdatedPods != null && response.Result.UpdatedPods.Any())
                            {
                                successSB.Append("<h3>成功更新运单(").Append(response.Result.UpdatedPods.Count()).Append("条)</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("箱数")
                                    .Append("</th><th>").Append("件数")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.UpdatedPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.BoxNumber.ToString())
                                        .Append("</td><td>").Append(o.GoodsNumber.ToString())
                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.NotUpdatedPods != null && response.Result.NotUpdatedPods.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.NotUpdatedPods.Count()).Append("条)系统中无对应运单</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("箱数")
                                    .Append("</th><th>").Append("件数")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.NotUpdatedPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.BoxNumber.ToString())
                                        .Append("</td><td>").Append(o.GoodsNumber.ToString())
                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.StateNotMatchPods != null && response.Result.StateNotMatchPods.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.StateNotMatchPods.Count()).Append("条)所对应系统中运单的状态不是“待审核”</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("箱数")
                                    .Append("</th><th>").Append("件数")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.StateNotMatchPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.BoxNumber.ToString())
                                        .Append("</td><td>").Append(o.GoodsNumber.ToString())
                                        .Append("</td></tr>");
                                }

                                successSB.Append("</tbody></table>");
                            }

                            if (response.Result.RepeatPods != null && response.Result.RepeatPods.Any())
                            {
                                successSB.Append("<h3>下列运单(").Append(response.Result.RepeatPods.Count()).Append("条)在Excel中是重复的</h3><br/>");
                                successSB.Append("<table><thead><tr><th>").Append("运单号")
                                    .Append("</th><th>").Append("箱数")
                                    .Append("</th><th>").Append("件数")
                                    .Append("</th></tr></thead><tbody>");
                                foreach (var o in response.Result.RepeatPods)
                                {
                                    successSB.Append("<tr><td>").Append(o.CustomerOrderNumber)
                                        .Append("</td><td>").Append(o.BoxNumber.ToString())
                                        .Append("</td><td>").Append(o.GoodsNumber.ToString())
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
        public ActionResult TYscan(int? PageIndex)
        {
            Runbow.TWS.Web.Areas.POD.QueryTYscanViewModel vm = new Runbow.TWS.Web.Areas.POD.QueryTYscanViewModel();
            vm.SearchCondition = new TYscanSearchCondition();
            //vm.PageIndex = 0;
            //vm.PageCount = 0;
            //string startTime = Request.QueryString["st"].ToString();
            //string endesTime = Request.QueryString["et"].ToString();
            //string types = Request.QueryString["type"].ToString();
            DateTime dtNows = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            vm.SearchCondition.StatCreateTime = Request.QueryString["st"] != null ? Convert.ToDateTime(Request.QueryString["st"]) : dtNows;
            vm.SearchCondition.EndCreateTime = Request.QueryString["et"] != null ? Convert.ToDateTime(Request.QueryString["et"]) : dtNows;
            vm.SearchCondition.type = Request.QueryString["type"] != null ? Convert.ToInt32(Request.QueryString["type"]) : -1;

            var response = (new TYscanService()).GetQueryTYscan(new QueryTYscanRequests()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = 10000,
                SearchCondition = vm.SearchCondition,
            });

            if (response.IsSuccess)
            {
                vm.TYscanCollection = response.Result.TYscanCollection;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult TYscan(QueryTYscanViewModel vm, int? PageIndex)
        {
            if (vm.IsExport)
            {
                return ExprotTYscan(vm);
            }
            else
            {
                var response = (new TYscanService()).GetQueryTYscanGroupBy(new QueryTYscanRequests()
                {
                    PageIndex = PageIndex ?? 0,
                    PageSize = UtilConstants.PAGESIZE,
                    SearchCondition = vm.SearchCondition,
                });

                if (response.IsSuccess)
                {
                    vm.TYscanCollectionGroupBy = response.Result.TYscanGroupByCollection;
                    vm.PageCount = response.Result.PageCount;
                    vm.PageIndex = response.Result.PageIndex;
                }
            }
            return View(vm);
        }

        /// <summary>
        /// 导出天翼扫描明细
        /// </summary>
        public ActionResult ExprotTYscan(QueryTYscanViewModel vm)
        {
            string starTime = vm.SearchCondition.StatCreateTime.ToString();
            string endsTime = vm.SearchCondition.EndCreateTime.ToString();
            DataTable Exprottable = (new TYscanService()).Proc_GetTYscanData(starTime, endsTime);
            if (Exprottable == null || Exprottable.Rows.Count <= 0)
            {
                return new EmptyResult();
            }
            string ReportName = "天翼扫描明细" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            var sbHtml = new StringBuilder();
            sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            for (int i = 0; i < Exprottable.Columns.Count; i++)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", Exprottable.Columns[i].ColumnName);
            }

            sbHtml.Append("</tr>");
            for (int i = 0; i < Exprottable.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                for (int j = 0; j < Exprottable.Columns.Count; j++)
                {
                    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", Exprottable.Rows[i][j].ToString());
                }
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");

            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AppendHeader("content-disposition", "attachment;filename=\"" + Url.Encode(ReportName) + "\"");
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }


        public ActionResult TYscanDetail(QueryTYscanViewModel vm, int? PageIndex)
        {
            string podID = Request.QueryString["PODID"].ToString();


            var response = (new TYscanService()).GetQueryTYscanDetail(new QueryTYscanRequests()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = 10000,//UtilConstants.PAGESIZE
                Customers = podID,
                //Customers = sb.ToString().Substring(0, sb.Length - 1).ToString()
            });

            if (response.IsSuccess)
            {
                vm.TYscanCollectionDetail = response.Result.TYscanDetailCollection;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
            }
            return View(vm);
        }
        [HttpGet]
        public ActionResult NikePodForBS(int? PageIndex)
        {
            NikePODForBSModel nm = new NikePODForBSModel();
            NikePodForBSCondition np = new NikePodForBSCondition();
            np.StartDeliveryTime = (DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
            np.EndDeliveryTime = (DateTime.Now.AddDays(0).ToString("yyyy-MM-dd"));
            np.IsConversion = 0;
            nm.Condition = np;
            var response = new NikeService().GetNikePOD(new NikePODForBSRequest()
            {
                Condition = nm.Condition,
                PageIndex = PageIndex??0,
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
        public ActionResult NikePodForBS(NikePODForBSModel nm, int? PageIndex)
        {

            var response = new NikeService().GetNikePOD(new NikePODForBSRequest()
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
        public string CancelNikePodForBS(string str)
        {
            var PodCollection = JosnToModel<NikeforBSPOD>(str);
            var response = new NikeService().CancelNikePodForBS(new NikePODForBSRequest()
            {
                PodCollection = PodCollection 
            });

            return response;
        }
        public string AddNikePodForBS(string str, string ShipperName)
        {

            var PodCollection = JosnToModel<NikeforBSPOD>(str);
            int todayPodNum = 0;
            PodCollection.Each((k, p) =>
            {
                todayPodNum++;
                p.SystemNumber = string.Concat("Runbow", DateTime.Now.ToString("yyyyMMddHHmmssff"), (10000 + todayPodNum).ToString().Substring(1));
                p.CustomerOrderNumber = "YY"+p.CustomerOrderNumber;
               // p.ProjectID = 1;
            });
            var response = new NikeService().AddNikePodForBS(new NikePODForBSRequest()
            {
                PodCollection = PodCollection,
                UserName = base.UserInfo.Name,
                ShipperName = ShipperName
            });
            return response;
        }

        private static List<T> JosnToModel<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }


    }
}
