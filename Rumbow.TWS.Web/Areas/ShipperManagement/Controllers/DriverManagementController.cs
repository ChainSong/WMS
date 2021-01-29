using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz.ShipperManagement;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;
using Runbow.TWS.MessageContracts.ShipperManagement.DriverManagement;
using Runbow.TWS.Web.Areas.ShipperManagement.Models.DriverManagement;
using Runbow.TWS.Web.Common;
using UtilConstants = Runbow.TWS.Common.Constants;

namespace Runbow.TWS.Web.Areas.ShipperManagement.Controllers
{
    public class DriverManagementController : BaseController
    {
        //
        // GET: /ShipperManagement/DriverManagement/
        [HttpGet]
        public ActionResult Index(bool? useSession)
        {
            QueryCRMDriverViewModel di = new QueryCRMDriverViewModel();
            di.SearchCondition = new CRMDriverSearchCondition();
            di.PageIndex = 0;
            di.PageCount = 0;
            di.ShowEditButton = false;
            //特殊操作
            if (base.UserInfo.ProjectRoleID == 28 || base.UserInfo.ProjectRoleID == 1)
            {
                di.ShowEditButton = true;
            }
            
            //if (useSession.HasValue && useSession.Value)
            //{
            //    if (Session["DriverCRM_SearchCondition"] != null)
            //    {
            //        di.SearchCondition = (CRMDriverSearchCondition)Session["DriverCRM_SearchCondition"];
            //        di.PageIndex = Session["DriverCRM_PageIndex"] != null ? (int)Session["DriverCRM_PageIndex"] : 0;
            //    }
            //    else
            //    {
            //        di.SearchCondition = new CRMDriverSearchCondition();
            //        di.PageIndex = 0;
            //    }
            //}

            var getCRMDriverByConditionResponse = new DriverManagementService().GetCRMDriverByCondition(new GetCRMDriverByConditionRequest()
            {
                SearchCondition = di.SearchCondition,
                PageSize = UtilConstants.PAGESIZE,
                PageIndex = di.PageIndex,
            });

            if (getCRMDriverByConditionResponse.IsSuccess)
            {
                di.CRMDriverCollection = getCRMDriverByConditionResponse.Result.CRMDriverCollection;
                di.UserID = base.UserInfo.ID.ToString();
                di.PageIndex = getCRMDriverByConditionResponse.Result.PageIndex;
                di.PageCount = getCRMDriverByConditionResponse.Result.PageCount;
            }

            //else
            //{
            //    //CRMDriverSearchCondition dri = new CRMDriverSearchCondition();
            //    //di.SearchCondition = dri;
            //    di.SearchCondition = new CRMDriverSearchCondition();
            //} 
            return View(di);
        }



        [HttpPost]
        public ActionResult Index(QueryCRMDriverViewModel di, string Action)
        {
            //查询导出
            var request = new GetCRMDriverByConditionRequest();
            
            if (Action == "查询" || Action == "Index")
            {
                request.SearchCondition = di.SearchCondition;
                request.PageSize = UtilConstants.PAGESIZE;
                request.PageIndex = di.PageIndex;
                di.ShowEditButton = false;
                //特殊操作
                if (base.UserInfo.ProjectRoleID == 28 || base.UserInfo.ProjectRoleID == 1)
                {
                    di.ShowEditButton = true;
                }
            }
            else if (Action == "导出")
            {
                request.SearchCondition = di.SearchCondition;
                request.PageSize = 0;
                request.PageIndex = 0;
            }
            var response = new DriverManagementService().GetCRMDriverByCondition(request);
            //{
            //    SearchCondition = requset.SearchCondition,
            //    PageIndex = requset.PageIndex,
            //    PageSize = requset.PageSize
            //});
            if (response.IsSuccess)
            {
                //di.CRMDriverCollection = response.Result.CRMDriverCollection;
                //di.PageIndex = response.Result.PageIndex;
                //di.PageCount = response.Result.PageCount;
                //Session["DriverCRM_SearchCondition"] = di.SearchCondition;
                //Session["DriverCRM_PageIndex"] = di.PageIndex;

                //string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_AMS_PATH;//新路径
                if (Action == "导出")
                {
                    return this.Export(response.Result.CRMDriverCollection);
                }
                else
                {
                    di.CRMDriverCollection = response.Result.CRMDriverCollection;
                    di.PageIndex = response.Result.PageIndex;
                    di.PageCount = response.Result.PageCount;
                }

            }
            return View(di);
        }

        //导出
        private ActionResult Export(IEnumerable<CRMDriver> crmDrivers)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("司机姓名", typeof(string));
            dt.Columns.Add("出生日期", typeof(string));
            dt.Columns.Add("联系电话", typeof(string));
            dt.Columns.Add("开始为服务日期", typeof(string));
            dt.Columns.Add("身份证号码", typeof(string));
            dt.Columns.Add("物流公司", typeof(string));
            dt.Columns.Add("物流公司联系人", typeof(string));
            dt.Columns.Add("物流公司联系电话", typeof(string));
            dt.Columns.Add("驾驶证档案号", typeof(string));
            dt.Columns.Add("驾照类型", typeof(string));
            dt.Columns.Add("是否在服务中", typeof(string));
            dt.Columns.Add("驾驶车辆牌号", typeof(string));
            dt.Columns.Add("司机登记号", typeof(string));
            dt.Columns.Add("登记证签发地", typeof(string));
            dt.Columns.Add("下次年审日期", typeof(string));
            dt.Columns.Add("初次驾照领证日期", typeof(string));
            dt.Columns.Add("下次体检日期", typeof(string));
            dt.Columns.Add("服务区域", typeof(string));
            dt.Columns.Add("主要行驶路线", typeof(string));

            crmDrivers.Each((i, s) =>
            {
                DataRow dr = dt.NewRow();
                dr[0] = s.DriverName;
                dr[1] = s.DriverBirthday.ToString("yyyy-MM-dd");
                dr[2] = s.DriverPhone;
                dr[3] = s.DriverStartServeForRunbowDate.ToString("yyyy-MM-dd");
                dr[4] = s.DriverIDCard;
                dr[5] = s.DriverLogisticsCompany;
                dr[6] = s.DriverLogisticsContactPerson;
                dr[7] = s.DriverLogisticsCompanyContactPhone;
                dr[8] = s.DriverCardNo;
                dr[9] = s.DriverCardType;
                dr[10] = s.DriverIsServing == true ? "是" : "否";
                //dr[10] = s.DriverIsServing;
                dr[11] = s.DriverCarNo;
                dr[12] = s.DriverRegistrationNo;
                dr[13] = s.DriverRegistrationCardSignedAddress;
                dr[14] = s.DriverNextYearCheckDate.ToString("yyyy-MM-dd");
                dr[15] = s.DriverFirstTimeGetCardDate.ToString("yyyy-MM-dd");
                dr[16] = s.DriverNextYearCheckBodyDate.ToString("yyyy-MM-dd");
                dr[17] = s.DriverServiceArea;
                dr[18] = s.DriverMainRoute;
                dt.Rows.Add(dr);
            });

            return this.ExportDataTableToExcel(dt, "Driver.xls"); 
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


        //点击id显示信息
        [HttpGet]
        public ActionResult Create(string id, int? type)
        {
            QueryCRMDriverViewModel vi = new QueryCRMDriverViewModel();
            if (string.IsNullOrEmpty(id))
            {
                CRMDriver v = new CRMDriver();
                v.Str1 = Guid.NewGuid().ToString();
                v.Str2 = Guid.NewGuid().ToString();
                v.Str3 = Guid.NewGuid().ToString();
                v.Str4 = Guid.NewGuid().ToString();
                v.DriverIsServing = true;
                vi.CreateDriver = v;
                vi.ViewType = 0;
            }
            else 
            {
                if (type == 1)
                {
                    var response = new DriverManagementService().GetSearcheDriver(id);
                    vi.CreateDriver = response;
                    vi.ViewType = 1;
                }
                else
                {
                    var response = new DriverManagementService().GetSearcheDriver(id);
                    vi.CreateDriver = response;
                    vi.ViewType = 0;
                }
            }
            return View(vi);
        }


        /// <summary>
        /// type 0 添加 1 查询  3 修改 
        /// </summary>
        /// <param name="vi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(QueryCRMDriverViewModel vi)
        {
            if (vi.ViewType == 1) 
            {
                vi.CreateDriver.CreateUser = base.UserInfo.ID.ToString();
                vi.CreateDriver.CreateTime = DateTime.Now;
            }
            vi.CreateDriver.UpdateUser = base.UserInfo.ID.ToString();
            vi.CreateDriver.UpdateTime = DateTime.Now;
                

            var response = new DriverManagementService().addCreateDriver(new GetCRMDriverByConditionRequest()
            {
                AddDriver = vi.CreateDriver
            });
            vi.ViewType = 1;
            return View(vi);
        }


        [HttpPost]
        public JsonResult DeleteCRMDriver(string id)
        {
            bool response = new DriverManagementService().DeleteCRMDriver(id);
            //var response = new VehicleManagementService().DeleteCRMVehicle(id);
            if (response)
            {
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "删除失败！", IsSuccess = false });
            }
        }


        ////删除
        //[HttpPost]
        //public JsonResult DeleteCRMDriver(long id)
        //{
        //    var response = new DriverManagementService().DeleteCRMDriver(new DeleteCRMDriverRequest() { CRMDriverID = id });
        //    if (response.IsSuccess)
        //    {
        //        return Json(new { Message = "删除成功", IsSuccess = true });
        //    }
        //    else
        //    {
        //        return Json(new { Message = "删除失败！", IsSuccess = false });
        //    }
        //}

    }
    
}
