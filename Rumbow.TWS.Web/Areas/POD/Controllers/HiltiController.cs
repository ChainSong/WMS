using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Web.Areas.POD.Models;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using UtilConstants = Runbow.TWS.Common.Constants;
using MyFile = System.IO.File;
using Runbow.TWS.MessageContracts.POD.Hilti;
using System.IO;
using System.Data;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;


namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class HiltiController : BaseController
    {
        [HttpGet]
        public ActionResult PodTrackAndReplyDocumentManage()
        {
            PodTrackAndReplyDocumentManageViewModel vm = new PodTrackAndReplyDocumentManageViewModel();
            vm.IsInnerUser = base.UserInfo.UserType == 2 ? true : false;
            vm.ActualDeliveryDate = DateTime.Parse(DateTime.Now.AddMonths(-1).DateTimeToString());
            vm.EndActualDeliveryDate = DateTime.Parse(DateTime.Now.AddDays(1).DateTimeToString());
            if (base.UserInfo.UserType == 1)
            {
                vm.ShipperID = base.UserInfo.CustomerOrShipperID;
                vm.MinPodState = 2;
               
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult PodTrackAndReplyDocumentManage(PodTrackAndReplyDocumentManageViewModel vm)
        {
            IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
            if (!string.IsNullOrEmpty(vm.CustomerOrderNumber))
            {
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

                if (customerOrderNumbers == null || !customerOrderNumbers.Any())
                {
                    customerOrderNumbers = new string[] { vm.CustomerOrderNumber.Trim() };
                }
            }

            HiltiService service = new HiltiService();

            var response = service.GetPodAndPodReplyDocumentByCondition(new GetPodAndPodReplyDocumentByConditionRequest()
            {
                CustomerOrderNumbers = customerOrderNumbers,
                ShipperID = vm.ShipperID,
                StartActualArrivalDate = vm.ActualDeliveryDate,
                EndActualArrivalDate = vm.EndActualDeliveryDate,
                MinPodState = vm.MinPodState
            });

            if (response.IsSuccess)
            {
                vm.PodAllCollection = response.Result;
                var notContainsCustomerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c) &&
                        !vm.PodAllCollection.Any(p => string.Equals(p.Pod.CustomerOrderNumber, c.Trim(), StringComparison.OrdinalIgnoreCase))
                    );

                //if (notContainsCustomerOrderNumbers != null && notContainsCustomerOrderNumbers.Any())
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.Append("您输入的客户运单号:");
                //    notContainsCustomerOrderNumbers.Each((i, c) => { sb.Append(c).Append(","); });
                //    sb.Remove(sb.Length - 1, 1);
                //    sb.Append("在结果中没有出现,系统中不存在此客户运单号或此运单已结案或此运单发货日期已超期.");
                //    vm.ReturnClientMessage = sb.ToString();
                //}
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddPodTrack(long podID, string systemNumber, string customerOrderNumber, DateTime? trackDate, string location, string goodsStatus, string csusesOfDelays, string causesOFDelaysType)
        {
            PodTrack podTrack = new PodTrack()
            {
                PodID = podID,
                SystemNumber = systemNumber,
                CustomerOrderNumber = customerOrderNumber,
                Creator = base.UserInfo.Name,
                CreateTime = DateTime.Now,
                Str1 = location,
                Str2 = goodsStatus,
                Str3 = csusesOfDelays,
                Str4 = causesOFDelaysType,
                DateTime1 = trackDate.HasValue ? trackDate : DateTime.Now
            };

            var response = new PodService().AddPodTrack(new AddPodTrackRequest() { PodTrack = podTrack });

            if (response.IsSuccess)
            {
                return Json(new { TrackID = response.Result.ID });
            }

            throw response.Exception;
        }

        [HttpPost]
        public ActionResult AddPodReplyDocument(long? replyDocumentID, long podID, string systemNumber, string customerOrderNumber, DateTime? actualArrivalDate, string arrivalInNormal, string differenceDate, string signPeople)
        {
            var response = new PodService().AddOrUpdatePodReplyDocument(new AddOrUpdatePodReplyDocumentRequest()
            {
                PodReplyDocument = new PodReplyDocument()
                {
                    ID = replyDocumentID.HasValue ? replyDocumentID.Value : 0,
                    PodID = podID,
                    SystemNumber = systemNumber,
                    CustomerOrderNumber = customerOrderNumber,
                    CreateTime = DateTime.Now,
                    Creator = base.UserInfo.Name,
                    DateTime1 = actualArrivalDate,
                    Str1 = signPeople,
                    Str2 = differenceDate,
                    Str3 = arrivalInNormal,
                    AttachmentGroupID = customerOrderNumber
                }
            });

            if (response.IsSuccess)
            {
                return Json(new { ReplyDocumentID = response.Result.ID });
            }

            throw response.Exception;
        }

        [HttpPost]
        public ActionResult GetPodLastTrack(long podID)
        {
            var response = new PodService().GetPodTracksByPodIDs(new GetPodInfoRequest() { PodIDs = new long[] { podID } });

            if (response.IsSuccess && response.Result != null && response.Result.Any())
            {
                var podTrack = response.Result.Last();
                return Json(new { DateTime1 = podTrack.DateTime1.DateTimeToString(), Str1 = podTrack.Str1, Str2 = podTrack.Str2, Str3 = podTrack.Str3 });
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult GetServicePeriod(long customerID, long startCityID, long endCityID)
        {
            var servicePeriod = ApplicationConfigHelper.GetSerivePeriod(base.UserInfo.ProjectID, customerID, startCityID, endCityID);

            if (servicePeriod != null)
            {
                return Json(new { Period = servicePeriod.Period });
            }

            throw new Exception("系统获取服务时效出错，请联系管理员");
        }


        public ActionResult PodTrackReportExport() 
        {
            PodTrackReportExportViewModel view = new PodTrackReportExportViewModel();
            view.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            view.PodStates = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            if(base.UserInfo.UserType==1)
            {
               view.ShipperName  = new ShipperService().GetShipperByID(new ShipperByIDRequest() { ID = base.UserInfo.CustomerOrShipperID }).Result.Name;
            }
            view.UserType = base.UserInfo.UserType;
          
            return View(view);
        }
        public PodTrackReportExportViewModel PodTrackReportOperate(PodTrackReportExportViewModel model) 
        {
            model.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            model.PodStates = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE)
                                    .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });

            var Result = new HiltiService().GetPodTrackReport(new GetPodTrackReportExportRequest() { SqlWhere = this.GetWhere(model), PageSize = UtilConstants.PAGESIZE, PageIndex = model.PageIndex,ReportName=model.ReportNameValue.Trim() }).Result;
            if (model.ReportNameValue == "跟踪报表" && Result.XLDTrackReport.Rows.Count>0)
            {
                for (int i = 0; i < Result.XLDTrackReport.Rows.Count;i++ )
                {
                    string[] value =   Result.XLDTrackReport.Rows[i]["接收订单时间"].ToString().Split(' ');
                    if(value.Length==1)
                    {
                        Result.XLDTrackReport.Rows[i]["接收订单时间"] =value[0].ToString();
                    }
                    else if(value.Length==2)
                    {
                        Result.XLDTrackReport.Rows[i]["接收订单时间"] = value[1].ToString();
                    }
                
                }
            }

            
            
            if (model.ReportNameValue == "回单率统计" && Result.XLDTrackReport.Rows.Count > 0)
            {
                DataRow ReturnOrder = Result.XLDTrackReport.NewRow();

                int CountOrdernum = 0;//总票数
                int CountSellOrder = 0;//销售单
                int ReturnSellOrder = 0;//已返销售单
                int NoSellOrder = 0;//非销售单
                int NoSellOrderReturnNum = 0;//非销售单已返
                ReturnOrder["RowID"] = Result.XLDTrackReport.Rows.Count+1;
                ReturnOrder["承运商"] = "总计";

                for (int i = 0; i < Result.XLDTrackReport.Rows.Count;i++ )
                {
                    CountOrdernum += Convert.ToInt32(Result.XLDTrackReport.Rows[i]["总票数"] == DBNull.Value ? 0 : Result.XLDTrackReport.Rows[i]["总票数"]);
                    CountSellOrder += Convert.ToInt32(Result.XLDTrackReport.Rows[i]["销售单"] == DBNull.Value ? 0 : Result.XLDTrackReport.Rows[i]["销售单"]);
                    ReturnSellOrder += Convert.ToInt32(Result.XLDTrackReport.Rows[i]["已返销售单"] == DBNull.Value ? 0 : Result.XLDTrackReport.Rows[i]["已返销售单"]);
                    NoSellOrder += Convert.ToInt32(Result.XLDTrackReport.Rows[i]["非销售单"]==DBNull.Value ? 0 : Result.XLDTrackReport.Rows[i]["非销售单"]);
                    NoSellOrderReturnNum += Convert.ToInt32(Result.XLDTrackReport.Rows[i]["已返非销售单"] == DBNull.Value ? 0 : Result.XLDTrackReport.Rows[i]["已返非销售单"]);
                }
                
                double  HiltiReturnOrder = Convert.ToDouble(ReturnSellOrder) / Convert.ToDouble(CountSellOrder);
                string HiltiReturnOrderValue = (Convert.ToDouble(Double.Parse(HiltiReturnOrder.ToString()).ToString("F4"))*100)+"%";
                double ShipperReturnOrder = Convert.ToDouble(ReturnSellOrder + NoSellOrderReturnNum) / Convert.ToDouble(CountOrdernum);
                string ShipperReturnOrderValue = (Convert.ToDouble(Double.Parse(ShipperReturnOrder.ToString()).ToString("F4")) * 100) + "%";
                ReturnOrder["总票数"] = CountOrdernum;
                ReturnOrder["销售单"] = CountSellOrder;
                ReturnOrder["已返销售单"] = ReturnSellOrder;
                ReturnOrder["非销售单"] = NoSellOrder;
                ReturnOrder["已返非销售单"] = NoSellOrderReturnNum;

                ReturnOrder["HILTI返单率%(合格)"] = HiltiReturnOrderValue;
                ReturnOrder["承运商返单率%(合格)"] = ShipperReturnOrderValue;
                Result.XLDTrackReport.Rows.Add(ReturnOrder);
            }


            if (model.ReportNameValue == "KPI统计报表" && Result.XLDTrackReport.Rows.Count > 0)
            {
               int month = Convert.ToDateTime(model.BeginOrderDate).Month;
                for (int i = 0; i < Result.XLDTrackReport.Columns.Count; i++)
                {
                    Result.XLDTrackReport.Columns[2].ColumnName = month.ToString();
                }
            }
            model.XLDTrackReport = Result.XLDTrackReport;
            model.PageIndex = Result.PageIndex;
            model.PageSize = Result.PageSize;
            model.PageCount = Result.PageCount;
            model.SumPoll = Result.SumPoll;
            model.SumGrossWeight = Result.SumGrossWeight;
            model.SumNetWeight = Result.SumNetWeight;

            if (model.PostedIDs!=null)
            {
                model.SelectedDeliveryState = model.PostedIDs.Select(s => new SelectListItem() { Value = s, Text = s });
            }
           
            return model;

        }
        

        

        

        [HttpPost]
        public ActionResult PodTrackReportExport(PodTrackReportExportViewModel model, string Action)
        {

            if (Action == "查询")
            {
                PodTrackReportOperate(model);
            }
            else if (Action == "导出报表")
            {
                  DataTable Exprottable=new DataTable();
                if (model.ReportNameValue == "Hilti回单率" )
                {
                    Exprottable = new HiltiService().GetPodTrackReportExport(new GetPodTrackReportExportRequest() { SqlWhere = this.HiltiGetWhere(model), ReportName = model.ReportNameValue.Trim() }).Result;
                }
                else { 
                
                    Exprottable = new HiltiService().GetPodTrackReportExport(new GetPodTrackReportExportRequest() { SqlWhere = this.GetWhere(model),ReportName=model.ReportNameValue.Trim() }).Result;
                }
                string ReportName = model.ReportNameValue + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";


                if (model.ReportNameValue == "跟踪报表" && Exprottable.Rows.Count > 0)
                {
                    for (int i = 0; i < Exprottable.Rows.Count; i++)
                    {
                        string[] value = Exprottable.Rows[i]["接收订单时间"].ToString().Split(' ');
                        if (value.Length == 1)
                        {
                            Exprottable.Rows[i]["接收订单时间"] = value[0].ToString();
                        }
                        else if (value.Length == 2)
                        {
                            Exprottable.Rows[i]["接收订单时间"] = value[1].ToString();
                        }

                    }

                    if (base.UserInfo.UserType == 1)
                    {
                        Exprottable.Columns.Remove("毛重_(KG)");
                    }
                }

                if (model.ReportNameValue == "回单率统计" && Exprottable.Rows.Count > 0)
                {
                    DataRow ReturnOrder = Exprottable.NewRow();

                    int CountOrdernum = 0;//总票数
                    int CountSellOrder = 0;//销售单
                    int ReturnSellOrder = 0;//已返销售单
                    int NoSellOrder = 0;//非销售单
                    int NoSellOrderReturnNum = 0;//非销售单已返
                    
                    ReturnOrder["承运商"] = "总计";

                    for (int i = 0; i < Exprottable.Rows.Count; i++)
                    {
                        CountOrdernum += Convert.ToInt32(Exprottable.Rows[i]["总票数"] == DBNull.Value ? 0 : Exprottable.Rows[i]["总票数"]);
                        CountSellOrder += Convert.ToInt32(Exprottable.Rows[i]["销售单"] == DBNull.Value ? 0 : Exprottable.Rows[i]["销售单"]);
                        ReturnSellOrder += Convert.ToInt32(Exprottable.Rows[i]["已返销售单"] == DBNull.Value ? 0 : Exprottable.Rows[i]["已返销售单"]);
                        NoSellOrder += Convert.ToInt32(Exprottable.Rows[i]["非销售单"] == DBNull.Value ? 0 : Exprottable.Rows[i]["非销售单"]);
                        NoSellOrderReturnNum += Convert.ToInt32(Exprottable.Rows[i]["已返非销售单"] == DBNull.Value ? 0 : Exprottable.Rows[i]["已返非销售单"]);
                    }

                    double HiltiReturnOrder = Convert.ToDouble(ReturnSellOrder) / Convert.ToDouble(CountSellOrder);
                    string HiltiReturnOrderValue = (Convert.ToDouble(Double.Parse(HiltiReturnOrder.ToString()).ToString("F4")) * 100) + "%";
                    double ShipperReturnOrder = Convert.ToDouble(ReturnSellOrder + NoSellOrderReturnNum) / Convert.ToDouble(CountOrdernum);
                    string ShipperReturnOrderValue = (Convert.ToDouble(Double.Parse(ShipperReturnOrder.ToString()).ToString("F4")) * 100) + "%";
                    ReturnOrder["总票数"] = CountOrdernum;
                    ReturnOrder["销售单"] = CountSellOrder;
                    ReturnOrder["已返销售单"] = ReturnSellOrder;
                    ReturnOrder["非销售单"] = NoSellOrder;
                    ReturnOrder["已返非销售单"] = NoSellOrderReturnNum;

                    ReturnOrder["HILTI返单率%(合格)"] = HiltiReturnOrderValue;
                    ReturnOrder["承运商返单率%(合格)"] = ShipperReturnOrderValue;
                    Exprottable.Rows.Add(ReturnOrder);
                }

                if (model.ReportNameValue == "KPI统计报表" && Exprottable.Rows.Count > 0)
                {
                    int month = Convert.ToDateTime(model.BeginOrderDate).Month;
                    for (int i = 0; i < Exprottable.Columns.Count; i++)
                    {
                        Exprottable.Columns[1].ColumnName = month.ToString();
                    }
                }
                //if (model.ReportNameValue == "Hilti回单率" && Exprottable.Rows.Count > 0)
                //{
                //    int month = Convert.ToDateTime(model.BeginOrderDate).Month;
                //    for (int i = 0; i < Exprottable.Rows.Count; i++)
                //    {
                //        Exprottable.Rows[i][2] = "%";
                //    }
                //}
                return this.ExportDataTableToExcel(Exprottable, Url.Encode((model.ReportNameValue + ".xls")));
                //ExcelHelper excelHelper = new ExcelHelper();
                //string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
                //string fileFullPath = Path.Combine(targetPath, ReportName);
                //excelHelper.CreateExcelByDataTable(fileFullPath, Exprottable);
                //excelHelper.Dispose();

                //string mimeType = "application/msexcel";
                //FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
                //return File(fs, mimeType, Url.Encode(ReportName));
            }
            else if (Action == "PodTrackReportExport")
            {
                PodTrackReportOperate(model);
            }
            if (model.ReportNameValue == "跟踪报表")
            {
                model.css = "style='width:500%'";
            }

            if (model.ReportNameValue == "未返回单清单")
            {
                model.css = "style='width:120%'";
            }


            if (model.ReportNameValue == "回单率统计")
            {
                model.css = "style='width:100%'";
            }


            if (model.ReportNameValue == "重量阶段统计")
            {
                model.css = "style='width:200%'";
            }

            if (model.ReportNameValue == "KPI统计报表")
            {
                model.css = "style='width:120%'";
            }
            if (model.ReportNameValue == "Hilti回单率")
            {
                model.css = "style='width:120%'";
            }
            model.UpOrDown = true;
            return View(model);
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
        public string HiltiGetWhere(PodTrackReportExportViewModel model) 
        {
            string sql = "";
            if (!string.IsNullOrEmpty(model.BeginOrderDate.ToString()))
            {
                sql += " AND POD.DateTime1>='" + model.BeginOrderDate.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(model.EndOrderDate.ToString()))
            {
                sql += " AND POD.DateTime1<='" + model.EndOrderDate.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(model.SelectedAttribution))
            {
                sql += " AND POD.Str37<='" + model.SelectedAttribution + "'";
            }
            return sql;
        }
        public string GetWhere(PodTrackReportExportViewModel model) 
        {
            string sql = "";
            //if(!string.IsNullOrEmpty(model.EndProvince))
            //{
            //    sql += " AND POD.Str4='" + model.EndProvince + "'";
            //}

            if (!string.IsNullOrEmpty(model.ShipperName))
            {
                sql += " AND POD.ShipperName='" + model.ShipperName + "'";
            }

            if (model.EndCityID != 0)
            {
                //sql += " AND POD.EndCityName='" + model.EndCity + "'";
                sql += " and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + model.EndCityID + "))";
            }
            //if (condition.EndCityID != null && condition.EndCityID != 0)
            //{
            //    sb.Append(" and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + condition.EndCityID + ")) ");
            //}
            if (!string.IsNullOrEmpty(model.BeginOrderDate.ToString()))
            {
                sql += " AND POD.DateTime1>='" + model.BeginOrderDate.ToString() + "'";
            }

            if (!string.IsNullOrEmpty(model.EndOrderDate.ToString()))
            {
                sql += " AND POD.DateTime1<='" + model.EndOrderDate.ToString() + "'";
            }



            if (!string.IsNullOrEmpty(model.BeginServiceDate.ToString()))
            {
                sql += " AND POD.DateTime2>='" + model.BeginServiceDate.ToString() + "' ";
            }

            if (!string.IsNullOrEmpty(model.EndServiceDate.ToString()))
            {
                sql += " AND POD.DateTime2<='" + model.EndServiceDate.ToString() + "'";
            }


            if (!string.IsNullOrEmpty(model.SelectedPodStatesID.ToString()) && model.SelectedPodStatesID != 0)
            {
                sql += " AND POD.PODStateID='" + model.SelectedPodStatesID + "'";
            }

            if (!string.IsNullOrEmpty(model.SelectedShipperTypeID.ToString()) && model.SelectedShipperTypeID != 0)
            {
                sql += " AND POD.ShipperTypeID='" + model.SelectedShipperTypeID + "'";
            }


            if (!string.IsNullOrEmpty(model.BeginDeliverGoodsDate.ToString()))
            {
                sql += " AND POD.ActualDeliveryDate>='" + model.BeginDeliverGoodsDate.ToString() + "'";
            }

            if (!string.IsNullOrEmpty(model.EndDeliverGoodsDate.ToString()))
            {
                sql += " AND POD.ActualDeliveryDate<='" + model.EndDeliverGoodsDate.ToString() + "'";
            }


            if (!string.IsNullOrEmpty(model.NotDeliverGoodsRemark))
            {
                sql += " AND POD.Str26 like '%" + model.NotDeliverGoodsRemark + "%'";
            }

            if (!string.IsNullOrEmpty(model.IsOrNoTencodValue))
            {
                sql += " AND POD.Str22='" + model.IsOrNoTencodValue + "'";
            }

            if (!string.IsNullOrEmpty(model.IsOrNoWithTheGoodsValue))
            {
                sql += " AND POD.Str13='" + model.IsOrNoWithTheGoodsValue + "'";
            }

            if (!string.IsNullOrEmpty(model.PlaceAnOrderTimeValue))
            {
                
                string[] value = model.PlaceAnOrderTimeValue.Split('-');

                sql += " AND convert(datetime,'1900-01-01 ' + POD.Str23) BETWEEN convert(datetime,'1900-01-01 " + value[0].ToString() + "') AND convert(datetime,'1900-01-01 " + value[1].ToString() + "')";
            }

            if (!string.IsNullOrEmpty(model.SelectedAttribution))
            {
                sql += " AND POD.Str37='" + model.SelectedAttribution + "' ";
            }

            if(!string.IsNullOrEmpty(model.CustomerOrderNoAnd103))
            {
                string[] CustomerOrderNoAnd103List = model.CustomerOrderNoAnd103.Replace("\r\n", ",").Split(',');
               
                string CustomerOrderNo = "";
                for (int i = 0; i < CustomerOrderNoAnd103List.Length; i++)
                {
                    if (string.IsNullOrEmpty(CustomerOrderNoAnd103List[i].ToString()))
                    {
                        continue;
                    }
                    CustomerOrderNo += "'" + CustomerOrderNoAnd103List[i].ToString() + "',";

                }
                sql += " And (POD.CustomerOrderNumber in(" + CustomerOrderNo.Substring(0, CustomerOrderNo.Length - 1) + ") or POD.Str1 in(" + CustomerOrderNo.Substring(0, CustomerOrderNo.Length - 1) + "))";
            }
            if (!string.IsNullOrEmpty(model.EndProvince))
            {
                sql += " AND POD.Str4 like '%" + model.EndProvince.Substring(0,2) + "%' ";
            }

            if (!string.IsNullOrEmpty(model.CustomerName))
            {
                string[] CustomerNameList = model.CustomerName.Replace("\r\n", ",").Split(',');
                string CustomerName = "";
                for (int i = 0; i < CustomerNameList.Length; i++)
                {
                    CustomerName += "POD.Str2 like '%" + CustomerNameList[i].ToString() + "%' or ";
                }
                sql += " And (" + CustomerName.Substring(0, CustomerName.Length - 4) + ")";
            }

            if (!string.IsNullOrEmpty(model.OrderTypeValue))
            {

                sql += " AND POD.PODTypeName ='" + model.OrderTypeValue + "' ";
            }

            if (!string.IsNullOrEmpty(model.FTLOrLTLValue))
            {

                sql += " AND POD.TtlOrTplName ='" + model.FTLOrLTLValue + "' ";
            }

            if (!string.IsNullOrEmpty(model.TheWarrantyIsOrNoWithTheGoodsValue))
            {
                sql += " AND POD.Str12 = '" + model.TheWarrantyIsOrNoWithTheGoodsValue + "' ";
            }

            if (!string.IsNullOrEmpty(model.IsOrNoNormalDeliveryValue))
            {
                sql += " AND PodReplyDocument.Str3 = '" + model.IsOrNoNormalDeliveryValue + "' ";
            }

            if (!string.IsNullOrEmpty(model.SalesOrdersOrNoSalesOrdersValue))
            {

                sql += " AND POD.Str34 = '" + model.SalesOrdersOrNoSalesOrdersValue + "'";
            }

            if (!string.IsNullOrEmpty(model.Channel))
            {
                sql += " AND POD.Str31 like '%" + model.Channel + "%' ";
            }

            if (!string.IsNullOrEmpty(model.Remarks))
            {
                sql += " AND POD.Str36 like '%" + model.Remarks + "%' ";
            }

            if (!string.IsNullOrEmpty(model.IsOrNoUpLoadReceiptValue))
            {
                if (model.IsOrNoUpLoadReceiptValue=="0")
                {
                    sql += "AND Attachment.GroupID IS NULL";
                }
                else if (model.IsOrNoUpLoadReceiptValue == "1")
                {
                    sql += "AND Attachment.GroupID IS not NULL";
                }
                
            }

            if (!string.IsNullOrEmpty(model.SalespersonName))
            {
                string[] SalespersonNameList = model.SalespersonName.Replace("\r\n", ",").Split(',');
                string SalespersonName = "";
                for (int i = 0; i < SalespersonNameList.Length; i++)
                {
                    SalespersonName += "POD.Str38 = '" + SalespersonNameList[i].ToString() + "' or ";
                }
                sql += " And (" + SalespersonName.Substring(0, SalespersonName.Length - 4) + ")";
                
            }

            


            if (!string.IsNullOrEmpty(model.IsOrNoRejectionValue))
            {
                sql += " AND POD.Str15 = '" + model.IsOrNoRejectionValue + "' ";
            }

            if (!string.IsNullOrEmpty(model.IsOrNoComPlainValue))
            {
                sql += " AND POD.Str16 = '" + model.IsOrNoComPlainValue + "' ";
            }

            if (!string.IsNullOrEmpty(model.ComPlainTypeValue))
            {
                sql += " AND POD.Str17 = '" + model.ComPlainTypeValue + "%' ";
            }

            if (!string.IsNullOrEmpty(model.ComPlainRemarks))
            {
                sql += " AND POD.Str18 like '%" + model.ComPlainRemarks + "%' ";
            }


            if (!string.IsNullOrEmpty(model.DelayClassifyingValue)) 
            {
                sql += " AND PODTRACK.Str4 like '%" + model.DelayClassifyingValue + "%' ";
            }

            if(model.PostedIDs !=null)
            {
                string sqlvalue = "";
                for (int i = 0; i < model.PostedIDs.Count(); i++) 
                {
                    sqlvalue += "PODTRACK.Str2 like '%" + model.PostedIDs.ToList()[i] + "%' or ";
                }
                sql += " AND (" + sqlvalue.Substring(0,sqlvalue.Length-4)+")";
            }

            return sql;
        }



        public DataSet GetDataFromExcel(HttpPostedFileBase hpf) 
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
        /// 通过xml生成表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable NewTableByXml(string path, DataTable tableSource)
        {
           
            XmlDataDocument xml = new XmlDataDocument();//创建XML的新实例
            xml.Load(path);//加载指定路径的XML文件
            XmlNodeList list = xml.SelectNodes("Pod_For_Project/Pod_Hilti/PodInfoUpdate");//读取指定的节点
            foreach (XmlNode node in list)//遍历节点集合
            {
                foreach (XmlNode nodeone in node.ChildNodes)//遍历节点集合下面所有的子节点
                {
                    for (int i = 0; i < tableSource.Columns.Count; i++)
                    {
                        if (tableSource.Columns[i].ColumnName.Trim() == nodeone.Attributes["ModelName"].Value.Trim()) 
                        {
                            tableSource.Columns[i].ColumnName = nodeone.Attributes["Pod_ColumnName"].Value.Trim();
                        }
                    }
                   
                }
            }

            for (int j = 0; j < tableSource.Rows.Count; j++)
            {
                if (tableSource.Rows[j][0].ToString() == "")
                {
                    tableSource.Rows[j].Delete();
                }
            }

            tableSource.AcceptChanges();

            return tableSource;
        }

        public ActionResult PodInfoUpdate(PodInfoUpdateViewModel Model)
        {
            return View(Model);
        }

        [HttpPost]
        public string PodInfoUpdate(string ActualDeliveryDate)
        {



            StringBuilder sb = new StringBuilder();
            DateTime date = DateTime.MinValue;
            bool flag = DateTime.TryParse(ActualDeliveryDate, out date);
            if (!flag)
            {
                return new { result = "<h3>日期格式错误！</h3>", IsSuccess = false }.ToJsonString();
            }
            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            PodInfoUpdateViewModel Model = new PodInfoUpdateViewModel();
            
            if (file.ContentLength > 0)
            {
                DataSet ds = this.GetDataFromExcel(file);

                if (ds != null && ds.Tables[0] != null)
                {
                    
                    string xmlpath = Runbow.TWS.Web.Common.Constants.APPLICATIONFILEPATH;
                    DataTable tableNew = new DataTable("NotDeliverGoods");
                    tableNew = this.NewTableByXml(xmlpath, ds.Tables[0]);
                    UpdatePodInfoUpdateRequest request = new UpdatePodInfoUpdateRequest();
                   
                    request.NotDeliverGoods = tableNew;
                    request.ActualDeliveryDate = Convert.ToDateTime(ActualDeliveryDate);
                    var response = new HiltiService().PodInfoUpdateByTable(request);
                    if (response.IsSuccess)
                    {
                        if (response.Result.ISORSUCCESS && response.Result.ERRORSOURCEVALUE == "0")
                        {
                            sb.Append("<h3>数据更新成功！</h3><br/>");
                        }

                        if (response.Result.ISORSUCCESS && response.Result.ERRORSOURCEVALUE != "0")
                        {
                            string[] value = response.Result.ERRORSOURCEVALUE.Split(',');
                            string valueresult = "";
                            for (int i = 0; i < value.Length; i++)
                            {
                                if (value[i].ToString() == "") { continue; }
                                valueresult += value[i].ToString() + "<br/>";
                            }
                            sb.Append("<h3>数据更新成功！但是附件中单号:<br/>" + valueresult + "在系统中不存在无法更新！</h3>");
                        }


                        if (response.Result.DeliverGoods.Rows.Count > 0)
                        {
                            sb.Append("<h3>本次发货" + response.Result.DeliverGoods.Rows.Count + "单信息如下：</h3><br/>");
                        }
                        else
                        {
                            sb.Append("<h3>本次无发货信息！</h3><br/>");
                        }
                        //Model.Message = sb.ToString();


                        sb.Append("<table><tr>");
                        for (int k = 0; k < response.Result.DeliverGoods.Columns.Count; k++)
                        {
                            sb.Append("<td class='TableColumnTitle'>" + response.Result.DeliverGoods.Columns[k].ColumnName.ToString() + "</td>");
                        }
                        sb.Append("</tr>");
                        for (int i = 0; i < response.Result.DeliverGoods.Rows.Count; i++)
                        {
                            sb.Append("<tr>");
                            for (int j = 0; j < response.Result.DeliverGoods.Columns.Count; j++)
                            {
                                sb.Append("<td>" + response.Result.DeliverGoods.Rows[i][j].ToString() + "</td>");
                            }
                            sb.Append("</tr>");
                        }
                        sb.Append("</table>");
                    }
                    else 
                    {
                        sb.Append("<h3>数据更新失败！</h3><br/>");
                        return new { result = sb.ToString(), IsSuccess = false }.ToJsonString();

                    }
                     

                }
            }
            return new { result = sb.ToString(), IsSuccess = true }.ToJsonString();
        }


        /// <summary>
        /// 通过xml生成表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable CommonNewTableByXml(string xmlNodePath, string path, DataTable tableSource)
        {

            XmlDataDocument xml = new XmlDataDocument();//创建XML的新实例
            xml.Load(path);//加载指定路径的XML文件
            XmlNodeList list = xml.SelectNodes(xmlNodePath);//读取指定的节点
            foreach (XmlNode node in list)//遍历节点集合
            {
                foreach (XmlNode nodeone in node.ChildNodes)//遍历节点集合下面所有的子节点
                {
                    for (int i = 0; i < tableSource.Columns.Count; i++)
                    {
                        if (tableSource.Columns[i].ColumnName.Trim() == nodeone.Attributes["ModelName"].Value.Trim())
                        {
                            tableSource.Columns[i].ColumnName = nodeone.Attributes["Pod_ColumnName"].Value.Trim();
                        }
                    }

                }
            }

            for (int j = 0; j < tableSource.Rows.Count; j++)
            {
                if (tableSource.Rows[j][0].ToString() == "")
                {
                    tableSource.Rows[j].Delete();
                }
            }

            tableSource.AcceptChanges();

            return tableSource;
        }

        public ActionResult OrderNoInfoUpdate(OrderNoInfoUpdate view) 
        {
            return View(view);
        }


        [HttpPost]
        public ActionResult OrderNoInfoUpdate()
        {
            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            OrderNoInfoUpdate Model = new OrderNoInfoUpdate();
            if (file.ContentLength > 0)
            {
                DataSet ds = this.GetDataFromExcel(file);

                if (ds != null && ds.Tables[0] != null)
                {
                    StringBuilder sb = new StringBuilder();
                    string xmlpath = Runbow.TWS.Web.Common.Constants.APPLICATIONFILEPATH;
                    string xmlNodePath = "Pod_For_Project/Pod_Hilti/Pod103OrderNoUpdate";
                    DataTable tableNew = new DataTable("103OrderNoInfo");
                    tableNew = this.CommonNewTableByXml(xmlNodePath, xmlpath, ds.Tables[0]);
                    UpdateOrderNoInfoUpdateRequest request = new UpdateOrderNoInfoUpdateRequest();
                   
                    request.OrderNoinfo = tableNew;

                    var result = new HiltiService().OrderNoInfoUpdateByTable(request).Result;
                   
                    if (result.ISORSUCCESS && result.ERRORSOURCEVALUE == "")
                    {
                        sb.Append("<h3>数据更新成功！</h3><br/>");
                    }

                    if (result.ISORSUCCESS && result.ERRORSOURCEVALUE != "")
                    {
                        string[] value = result.ERRORSOURCEVALUE.Split(',');
                        string valueresult = "";
                        for (int i = 0; i < value.Length; i++)
                        {
                            if (value[i].ToString() == "") { continue; }
                            valueresult += value[i].ToString() + "<br/>";
                        }
                        sb.Append("<h3>数据更新成功！但是附件中单号:<br/>" + valueresult + "在系统中不存在无法更新！</h3>");
                    }

                    if (!result.ISORSUCCESS)
                    {
                        sb.Append("<h3>数据更新失败！</h3><br/>");
                    }

                    Model.Message = sb.ToString();

                }
            }
            return View(Model);
        }




        public ActionResult PrintHandoverDetailedList() 
        {
            PrintHandoverDetailedListModel model = new PrintHandoverDetailedListModel();
            model.BeginDistributionDate = DateTime.Now;
            model.EndDistributionDate = DateTime.Now;
            if (base.UserInfo.UserType == 1) 
            {
                model.Shipper = new ShipperService().GetShipperByID(new ShipperByIDRequest() { ID = base.UserInfo.CustomerOrShipperID }).Result.Name;
                model.ShipperID = Convert.ToInt32(base.UserInfo.CustomerOrShipperID);
            }
            model.UserType = base.UserInfo.UserType;
            return View(model);
        }

        [HttpPost]
        public ActionResult PrintHandoverDetailedList(PrintHandoverDetailedListModel model)
        {
           
            //model.DateList = HandleData(model);
            model.DateList = new HiltiService().CheckShipperDistributionDate(new PrintHandoverDetailedListDetailRequest() {ShipperName=model.Shipper,BeginDateTime=model.BeginDistributionDate,EndDateTime = model.EndDistributionDate }).Result;
            return View(model);
            
        }

        public List<string> HandleData(PrintHandoverDetailedListModel model) 
        {

            List<string> result = new List<string>();
            //TimeSpan? ts = model.EndActualDeliveryDate - model.BeginActualDeliveryDate;
            TimeSpan? ts = model.EndDistributionDate - model.BeginDistributionDate;
            int value = ts.Value.Days;

            for (int i = 0; i <= value; i++) 
            {
                result.Add(Convert.ToDateTime(model.BeginDistributionDate).AddDays(i).ToString());
            }
            return result;
        }


        public string GetSqlWhere(string datetime, string ShipperName) 
        {
            
            string sql = "";

            if (!string.IsNullOrEmpty(datetime))
            {
                sql += " AND POD.DateTime15>='" + Convert.ToDateTime(datetime).ToString("yyyy-MM-dd 00:00:00") + "'" + " AND POD.DateTime15<='" + Convert.ToDateTime(datetime).ToString("yyyy-MM-dd 23:59:59") + "'";
            }

            if (!string.IsNullOrEmpty(ShipperName))
            {
                sql += " AND POD.ShipperName='" + ShipperName + "'";
            }
            return sql;
        }

        private string GetTitleText(string datetime, string ShipperName) 
        {
            return "<table style='text-align:center; width:100%'><tr><td><h3>" + Convert.ToDateTime(datetime).ToString("yyyy年MM月dd日") + ShipperName + "发货清单" + "</h3></td></tr></table>";
        }

        private string GetPageNotText()
        {
            string value = @"<div style='text-align:right; width:100%'><table style='width:40%; height:50%; text-align:left; margin:auto'>
                                <tr>
                                <td style='width:5%'>仓库接货人：</td>
                                <td style='width:5%'></td>
                                <td style='width:5%'>时间：</td>
                                <td style='width:5%'></td>
                                </tr>
                                <tr>
                                <td style='width:5%'>供应商签收人：</td>
                                <td style='width:5%'></td>
                                <td style='width:5%'>时间：</td>
                                <td style='width:5%'></td>
                                </tr>
                              </table></div>";

            return value;
        }

        public void GetInvoiceInfo(PrintHandoverDetailedListDetailModel model) 
        {
            DataTable tableInfoOne = new DataTable();
            tableInfoOne.Columns.Add("订单号", typeof(string));
            tableInfoOne.Columns.Add("发票号", typeof(string));

            DataRow[] rows = model.DataTable.Select(" 发票号码 is not null");

            if(rows.Length>0)
            {
                for (int i = 0; i < rows.Length;i++ )
                {
                    DataRow row = tableInfoOne.NewRow();
                    row["订单号"] = rows[i]["订单编号"].ToString();
                    row["发票号"] = rows[i]["发票号码"].ToString();
                    tableInfoOne.Rows.Add(row);
                }
            }
            model.DataTableInvoice = tableInfoOne;


            DataTable tableInfoTwo = new DataTable();
            tableInfoTwo.Columns.Add("订单号", typeof(string));
            tableInfoTwo.Columns.Add("代收金额", typeof(string));

            DataRow[] rowstwo = model.DataTable.Select(" 代收款金额 is not null");

            if (rowstwo.Length > 0)
            {
                for (int i = 0; i < rowstwo.Length; i++)
                {
                    DataRow rowtwo = tableInfoTwo.NewRow();
                    rowtwo["订单号"] = rowstwo[i]["订单编号"].ToString();
                    rowtwo["代收金额"] = rowstwo[i]["代收款金额"].ToString();
                    tableInfoTwo.Rows.Add(rowtwo);
                }
                model.DataTableForCollection = tableInfoTwo;
            }
        }

        public ActionResult PrintHandoverDetailedListDetail(string datetime, string ShipperName,int ShipperID) 
        {
            PrintHandoverDetailedListDetailModel model = new PrintHandoverDetailedListDetailModel();
            string sqlwhere = GetSqlWhere(datetime, HttpUtility.UrlDecode(ShipperName));
            HiltiService service = new HiltiService();
            model.DataTable = service.GetPrintHandoverDetailedListDetail(new PrintHandoverDetailedListDetailRequest() { SqlWhere = sqlwhere }).Result;
            model.TitleText = GetTitleText(datetime, HttpUtility.UrlDecode(ShipperName));
            model.PageNotText = GetPageNotText();
            ViewBag.DateTime = datetime;
            ViewBag.ShipperName = ShipperName;
            ViewBag.ShipperID = ShipperID;
            GetInvoiceInfo(model);
            return View(model);
        }

        [HttpPost]
        [MultiButton("Exprot")]
        public ActionResult ExprotHandoverDetailedListDetail(string datetime, string ShipperName) 
        {
            
            string sqlwhere = GetSqlWhere(datetime, ShipperName);
            HiltiService service = new HiltiService();
            DataTable Exprottable = service.GetPrintHandoverDetailedListDetail(new PrintHandoverDetailedListDetailRequest() { SqlWhere = sqlwhere }).Result;




            string ReportName = Convert.ToDateTime(datetime).ToString("yyyy年MM月dd日") + HttpUtility.UrlDecode(ShipperName) + "发货清单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            ExcelHelper excelHelper = new ExcelHelper();
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
            string fileFullPath = Path.Combine(targetPath, ReportName);
            excelHelper.CreateExcelByDataTable(fileFullPath, Exprottable);
            excelHelper.Dispose();

            string mimeType = "application/msexcel";
            FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
            return File(fs, mimeType, Url.Encode(ReportName));
        }


        public DataTable OverrideExprotTable(DataTable table)
        {
            

            //添加3行空行
            for (int i = 0; i < 2; i++)
            {
                DataRow dr = table.NewRow();
                table.Rows.Add(dr);
            }

            int j = table.Rows.Count-1;
            //
            DataRow[] rows = table.Select(" 发票号码 is not null");
            DataRow[] rowsTwo = table.Select(" 代收款金额 is not null");

            if (rows.Length > rowsTwo.Length)
            {
                for (int i = 0; i < rows.Length + 2; i++)
                {
                    DataRow dr = table.NewRow();
                    table.Rows.Add(dr);
                }

                table.Rows[j + 1][0] = "今日发票随货统计";
                table.Rows[j + 2][0] = "订单号";
                table.Rows[j + 2][1] = "发票号";


                for (int k = 1; k <= rows.Length; k++)
                {


                    table.Rows[j + (k + 2)][0] = rows[k - 1]["订单编号"].ToString();
                    table.Rows[j + (k + 2)][1] = rows[k - 1]["发票号码"].ToString();

                }
                DataRow drtwo = table.NewRow();
                table.Rows.Add(drtwo);
                table.Rows[table.Rows.Count - 1][0] = "共计" + rows.Length + "份,请签收：";
                if (rowsTwo.Length>0)
                {
                    table.Rows[j + 1][2] = "今日待收款统计";
                    table.Rows[j + 2][2] = "订单号";
                    table.Rows[j + 2][3] = "发票号";

                    for (int y = 1; y <= rowsTwo.Length; y++)
                    {


                        table.Rows[j + (y + 2)][0] = rowsTwo[y - 1]["订单编号"].ToString();
                        table.Rows[j + (y + 2)][1] = rowsTwo[y - 1]["发票号码"].ToString();
                        if (y == rowsTwo.Length)
                        {
                            table.Rows[j + (y + 3)][2] = "共计" + rowsTwo.Length + "票,请确认：";
                        }
                    }
                }
            }


            if (rowsTwo.Length > rows.Length)
            {
                for (int i = 0; i < rowsTwo.Length+2; i++)
                {
                    DataRow dr = table.NewRow();
                    table.Rows.Add(dr);
                }

                table.Rows[j + 1][2] = "今日待收款统计";
               
                table.Rows[j + 2][2] = "订单号";
                table.Rows[j + 2][3] = "代收金额";

                for (int k=1; k <= rowsTwo.Length; k++)
                {


                    table.Rows[j+(k+2)][2] = rowsTwo[k-1]["订单编号"].ToString();
                    table.Rows[j+(k+2)][3] = rowsTwo[k-1]["代收款金额"].ToString();

                }
                DataRow drtwo = table.NewRow();
                table.Rows.Add(drtwo);
                table.Rows[table.Rows.Count-1][2] = "共计"+rowsTwo.Length+"票,请确认：";
                if (rows.Length>0)
                {
                    table.Rows[j + 1][0] = "今日发票随货统计";
                    table.Rows[j + 2][0] = "订单号";
                    table.Rows[j + 2][1] = "发票号";




                    for (int y = 1; y <= rows.Length; y++)
                    {


                        table.Rows[j + (y + 2)][0] = rows[y-1]["订单编号"].ToString();
                        table.Rows[j + (y + 2)][1] = rows[y-1]["发票号码"].ToString();
                        if (y == rows.Length)
                        {
                            table.Rows[j + (y + 3)][0] = "共计" + rows.Length + "份,请签收：";
                        }
                    }
                }
                
            }

            


            //添加2行空行
            for (int i = 0; i < 4; i++)
            {
                DataRow dr = table.NewRow();
                table.Rows.Add(dr);
            }
           


            table.Rows[table.Rows.Count - 3][16] = "仓库接货人：";
            table.Rows[table.Rows.Count - 3][18] = "时间：";
            table.Rows[table.Rows.Count - 2][16] = "供应商签收人：";
            table.Rows[table.Rows.Count - 2][18] = "时间：";


            table.AcceptChanges();
            return table;
        }


        [HttpPost]
        public ActionResult GenerateAnnexAndSendEmail(string datetime, string ShipperName,int ShipperID)
        {

            string sqlwhere = GetSqlWhere(datetime, ShipperName);
            HiltiService service = new HiltiService();
            DataTable Exprottable = service.GetPrintHandoverDetailedListDetail(new PrintHandoverDetailedListDetailRequest() { SqlWhere = sqlwhere }).Result;

            Exprottable = OverrideExprotTable(Exprottable);


            string ReportName = Convert.ToDateTime(datetime).ToString("yyyy年MM月dd日") + HttpUtility.UrlDecode(ShipperName) + "发货清单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            ExcelHelper excelHelper = new ExcelHelper();
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
            string fileFullPath = Path.Combine(targetPath, ReportName);
            excelHelper.CreateExcelByDataTable(fileFullPath, Exprottable);
            excelHelper.Dispose();
            string ZipFileName = Convert.ToDateTime(datetime).ToString("yyyy年MM月dd日") + HttpUtility.UrlDecode(ShipperName) + "发货清单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
            string ZipfilePath = Path.Combine(targetPath, ZipFileName);

            bool Result = false;
            if (Runbow.TWS.Common.ZipHelper.ZipFile(fileFullPath, ZipfilePath, ""))
            {
                //webService邮件接口
                ESPService.EmailSending sending = new ESPService.EmailSending();
                ESPService.ESPServiceSoapClient addrequest = new ESPService.ESPServiceSoapClient();
                //用户信息
                User user = Session[Runbow.TWS.Web.Common.Constants.USER_INFO_KEY] as User;
                //承运商配置信息
                var response = new ShipperService().GetShipperAllInfo(new GetShipperAllInfoRequest() { ShipperID = ShipperID, ProjectID = user.ProjectID, RelatedCustomerID = 2 });

                ShipperAllInfo allinfo = response.Result;
                if (allinfo.ShipperRelatedInfo != null)
                {
                    //发送邮件信息配置
                    sending.ProjectID = 2;
                    sending.ProjectName = "Hilti";
                    sending.EmailTitle = Convert.ToDateTime(datetime).ToString("yyyy年MM月dd日") + ShipperName + "发货清单";
                    sending.EmailAdd = allinfo.ShipperRelatedInfo.Str2;
                    sending.EmailSendContent = allinfo.ShipperRelatedInfo.Str7;
                    FileStream fs = new FileStream(ZipfilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length);
                    fs.Dispose();
                    fs.Close();
                    sending.EmailAnnex = bytes;

                    sending.Creator = user.DisplayName;

                    sending.ToCC = "";
                    sending.AnnexName = ZipFileName;
                    //发送邮件
                    Result = addrequest.AddEmailSending(sending);
                }
                else
                {
                    return Json(new { Message = "邮件信息没有配置，请先配置邮件信息!", IsSuccess = false });
                }
            }
            //判断邮件是否提交成功
            if (Result)
            {
                return Json(new { Message = "邮件请求已提交！系统会在3分钟内发送完成!", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "邮件请求失败！", IsSuccess = false });
            }

            
           
        }






        [HttpPost]
        public ActionResult PODColumnsInfoUpdate(string array)
        {


            DataTable table = JsonToTable(array);

           //DataTable table = GetJson(array);



            
            
            HiltiService service = new HiltiService();
            var response = service.PODColumnsInfoUpdateByTable(new UpdatePodColumnInfoRequest() { PodColumnInfo = table });

            if (response.IsSuccess)
            {
                return Json(new { Message = "保存成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "保存失败", IsSuccess = false });
            }

            
        }

        [HttpPost]
        public ActionResult IsBeloneToShanghai(long cityID)
        {
            Region region = ApplicationConfigHelper.GetRegions().FirstOrDefault(r => r.ID == cityID);

            if (region != null)
            {
                if (region.ID == 10 || region.SupperID == 10)
                {
                    return Json(new { IsSuccess = true });
        }

                Region supperRegion = ApplicationConfigHelper.GetRegions().FirstOrDefault(r => r.ID == region.SupperID);

                if (supperRegion != null)
                {
                    if (supperRegion.ID == 10 || supperRegion.SupperID == 10)
                    {
                        return Json(new { IsSuccess = true });
                    }
                }
            }

            return Json(new { IsSuccess = false });
        }


        public DataTable JsonToTable(string jsonStr)
        {
            List<ToJson> ToJosnList = new List<ToJson>();
            JavaScriptSerializer json = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
            List<ToJson> jsonlist = json.Deserialize<List<ToJson>>(jsonStr);    //将json数据转化为对象类型并赋值给list

            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("PackagesNumber", typeof(string));
            table.Columns.Add("ShippeNO", typeof(string));


            for (int i = 0; i < jsonlist.Count;i++ )
            {
                DataRow row = table.NewRow();

                row["ID"] = jsonlist[i].ID.ToString();
                row["PackagesNumber"] = jsonlist[i].PackagesNumber.ToString();
                row["ShippeNO"] = jsonlist[i].ShippeNO.ToString();
                table.Rows.Add(row);
            }


            return table;

           



        }


        public DataTable GetJson(string strJson) 
        {
            //取出表名  
            Regex rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名  
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据  
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');

                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.ColumnName = strCell[0].ToString();
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }



        
        public ActionResult AddOrUpdateServicePeriod(AddOrUpdateServicePeriodModel view)
        {
            return View(view);
        }

        public ActionResult AddOrUpdatePeriod(string EndCity, int EndCityID, int Period) 
        {
            Response<AddOrUpdateServicePeriodRequest> response = new Response<AddOrUpdateServicePeriodRequest>();
            response = new HiltiService().AddOrUpdateServicePeriod(new AddOrUpdateServicePeriodRequest() { EndCity = EndCity, EndCityID = EndCityID,Period = Period });
            if (!response.IsSuccess)
            {
                return Json(new { ResultValue = "操作失败！" });
            }
            else 
            {
                return Json(new { ResultValue = "操作成功！" });
            }
        }


        public ActionResult AddOrUpdateSellInfo(string SellName, string SellPhone)
        {
            Response<int> response = new Response<int>();
            response = new HiltiService().AddOrUpdateSellInfo(new AddOrUpdateServicePeriodRequest() { SellName = SellName, SellPhone = SellPhone });
            if (response.Result <=0)
            {
                return Json(new { ResultValue = "操作失败！" });
            }
            else
            {
                return Json(new { ResultValue = "操作成功！" });
            }
        }


        public ActionResult GetServicePeriodInfo(string EndCity)
        {
            Response<int> response = new Response<int>();
            response = new HiltiService().GetServicePeriodInfo(new AddOrUpdateServicePeriodRequest() {EndCity = EndCity });

            return Json(new { Period = response.Result });
           
        }



        public ActionResult GetHiltiDriverPhone(string SellName)
        {
            Response<string> response = new Response<string>();
            response = new HiltiService().GetHiltiDriverPhone(new AddOrUpdateServicePeriodRequest() { SellName = SellName });

            return Json(new { Phone = response.Result });

        }


        public ActionResult PodQuery() 
        {
            return View();
        }

        
        public ActionResult CreditUpdate(CreditUpdate vm)
        {
            return View(vm);
        }

        private int IsRepeat(DataTable dt)
        {
            IList<string> list = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                string temp = item[0].ToString();
                if (list.Contains(temp) && !string.IsNullOrWhiteSpace(temp))
                {
                    return -1;
                }
                list.Add(temp);
            }
            return 1;
        }

        [HttpPost]
        [MultiButton("operateupdate")]
        public ActionResult CreditUpdate()
        {
            HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
            Models.CreditUpdate Model = new Models.CreditUpdate();
            if (file.ContentLength > 0)
            {
                DataSet ds = this.GetDataFromExcel(file);
                try
                {
                    if (ds != null && ds.Tables != null && ds.Tables[0] != null)
                    {
                        if (IsRepeat(ds.Tables[0]) == -1)
                        {
                            Model.Message = "Excel有重复数据，请清除！";
                            return View(Model);
                        }

                        StringBuilder sb = new StringBuilder();

                        CreditUpdateRequest request = new CreditUpdateRequest();
                        request.OrderNoinfo = ds.Tables[0];

                        var result = new HiltiService().CreditUpdateByTable(request);

                        if (result.IsSuccess)
                        {
                            sb.Append("<h3>数据更新成功！</h3><br/>");
                        }

                        else
                        {
                            sb.Append("<h3>数据更新失败！" + result.SuccessMessage + "</h3><br/>");
                        }

                        Model.Message = sb.ToString();
                    }
                    else
                    {
                        Model.Message = "<h3>文件sheet名称有问题或者无数据！<h3><br/>";
                    }
                }
                catch (Exception ex)
                {
                    Model.Message = "Excel表名或格式有问题！";
                    return View(Model);
                }
            }
            return View(Model);
        }

        /// <summary>
        /// 更新POD
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePOD()
        {
            Models.CreditUpdate vm = new Models.CreditUpdate();
            CreditUpdateRequest request = new CreditUpdateRequest();
            var result = new HiltiService().UpdatePOD(request);
            if (result.IsSuccess)
            {
                return Json("更新匹配成功！", JsonRequestBehavior.AllowGet);
            }
            return Json("更新匹配失败！", JsonRequestBehavior.AllowGet);
        }

    }
}
