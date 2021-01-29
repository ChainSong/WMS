using Runbow.TWS.Biz.POD;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.POD.Total;
using Runbow.TWS.Web.Areas.POD.Models.TotalModel;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using UtilConstants = Runbow.TWS.Common.Constants;
using MyFile = System.IO.File;
using System.Data;
using Runbow.TWS.Common;
using System.IO;


namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class TotalController : BaseController
    {
        //
        // GET: /POD/Total/

        public ActionResult MessageHistoryQuery()
        {
            MessageHistoryQueryModel model = new MessageHistoryQueryModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult MessageHistoryQuery(MessageHistoryQueryModel model, int? PageIndex)
        {
            if (model.IsExprot)
            {
                return this.GetMessageHistoryInfoReport(model);
            }
            else 
            {
            GetMessageHistoryQueryRequest request = new GetMessageHistoryQueryRequest() { SqlWhere = GetSqlWhere(model),PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 };
            Response<GetMessageHistoryQueryRequest> Response = new TotalService().GetMessageHistoryInfo(request);
            model.MessageHistoryTable = Response.Result.MessageHistoryTable;
            model.PageIndex = Response.Result.PageIndex;
            model.PageSize = Response.Result.PageSize;
            model.PageCount = Response.Result.PageCount;
            }
            return View(model);
        }

        public ActionResult GetMessageHistoryInfoReport(MessageHistoryQueryModel view)
        {
            string SQL = this.GetSqlWhere(view);
            string ReportName = "Total短信信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            GetMessageHistoryQueryRequest request = new GetMessageHistoryQueryRequest() { SqlWhere = GetSqlWhere(view)};
            Response<GetMessageHistoryQueryRequest> Response = new TotalService().GetMessageHistoryInfoReport(request);

            DataTable Exprottable = Response.Result.MessageHistoryTable;
            ExcelHelper excelHelper = new ExcelHelper();
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), "Temp");
            string fileFullPath = Path.Combine(targetPath, ReportName);
            excelHelper.CreateExcelByDataTable(fileFullPath, Exprottable);
            excelHelper.Dispose();

            string mimeType = "application/msexcel";
            FileStream fs = MyFile.Open(fileFullPath, FileMode.Open);
            return File(fs, mimeType, Url.Encode(ReportName));
        }


        public string GetSqlWhere(MessageHistoryQueryModel model) 
        {
            string sql = "";
            if (!string.IsNullOrEmpty(model.CustomerOrderNumber))
            {
                string[] NumberList = model.CustomerOrderNumber.Replace("\r\n",",").Split(',');
                string CustomerOrderNumber = "";
                for (int i = 0; i < NumberList.Length;i++ )
                {
                    CustomerOrderNumber += "'"+NumberList[i].ToString()+"',";
                }
                sql += " And SendHistory.Str1 in(" + CustomerOrderNumber.Substring(0,CustomerOrderNumber.Length-1) + ")";
            }

            if(!string.IsNullOrEmpty(model.Phone))
            {
                string[] PhoneList = model.Phone.Replace("\r\n", ",").Split(',');
                string Phone = "";
                for (int i = 0; i < PhoneList.Length; i++)
                {
                    Phone += "'" + PhoneList[i].ToString() + "',";
                }
                sql += " And SendHistory.PhoneNumber in(" + Phone.Substring(0, Phone.Length - 1) + ")";
            }
            if (!string.IsNullOrEmpty(model.BeginSendTime.ToString()))
            {
                sql += " AND SendHistory.SendTime>='" +Convert.ToDateTime( model.BeginSendTime).ToString("yyyy-MM-dd") + " 00:00:00'";
            }

            if (!string.IsNullOrEmpty(model.EndSendTime.ToString()))
            {
                sql += " AND SendHistory.SendTime<='" + Convert.ToDateTime(model.EndSendTime).ToString("yyyy-MM-dd") + " 23:59:59'";
            }
            return sql;
        }
        [HttpGet]
        public ActionResult QueryDocument()
        {
            TotalPODModel model = new TotalPODModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult QueryDocument(TotalPODModel model, int? PageIndex)
        {

            var response = new TotalService().GetTotalPODReport(new GetTotalPODRequest()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
                SearchCondition = model.SearchCondition,
                StateID = model.SearchCondition.StateID
            });
            model.TotalPODCollection = response.Result.TotalPODCollection;
            model.PageIndex = response.Result.PageIndex;
            model.PageCount = response.Result.PageCount;
            model.RowCount = response.Result.RowCount;
            return View(model);
        }
        
    }
}
