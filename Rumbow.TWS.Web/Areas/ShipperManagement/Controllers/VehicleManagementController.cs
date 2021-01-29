using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Runbow.TWS.Biz;
using Runbow.TWS.Biz.ShipperManagement;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;
using Runbow.TWS.MessageContracts.ShipperManagement.DriverManagement;
using Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement;
using Runbow.TWS.Web.Areas.ShipperManagement.Models.VehicleManagement;
using Runbow.TWS.Web.Common;
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;


namespace Runbow.TWS.Web.Areas.ShipperManagement.Controllers
{
    public class VehicleManagementController : BaseController
    {
        //
        // GET: /ShipperManagement/VehicleManagement/
        [HttpGet]
        public ActionResult Index()
        {
            QueryCRMVehicleViewModel vi = new QueryCRMVehicleViewModel();
            vi.SearchCondition = new CRMVehicleSearchCondition();
            vi.PageIndex = 0;
            vi.PageCount = 0;
            vi.ShowEditButton = false;
            //特殊操作
            if (base.UserInfo.ProjectRoleID == 28 || base.UserInfo.ProjectRoleID == 1)
            {
                vi.ShowEditButton = true;
            }
            var getCRMVehicleByConditionResponse = new VehicleManagementService().GetCRMVehicleByCondition(new GetCRMVehicleByConditionRequest()
                {
                    SearchCondition = vi.SearchCondition,
                    PageSize = UtilConstants.PAGESIZE,
                    PageIndex = vi.PageIndex,
                });

            if (getCRMVehicleByConditionResponse.IsSuccess)
            {
                vi.CRMVehicleCollection = getCRMVehicleByConditionResponse.Result.CRMVehicleCollection;
                vi.UserID = base.UserInfo.ID.ToString();
                vi.PageIndex = getCRMVehicleByConditionResponse.Result.PageIndex;
                vi.PageCount = getCRMVehicleByConditionResponse.Result.PageCount;
            }
            return View(vi);
        }


        [HttpPost]
        public ActionResult Index(QueryCRMVehicleViewModel vi,int? index, string Action)
        {
            //查询导出
            var request = new GetCRMVehicleByConditionRequest();

            if (Action == "查询" || Action == "Index")
            {
                request.SearchCondition = vi.SearchCondition;
                request.PageSize = UtilConstants.PAGESIZE;
                request.PageIndex = vi.PageIndex;
                vi.ShowEditButton = false;
                //特殊操作
                if (base.UserInfo.ProjectRoleID == 28 || base.UserInfo.ProjectRoleID == 1)
                {
                    vi.ShowEditButton = true;
                }
            }
            else if (Action == "导出")
            {
                request.SearchCondition = vi.SearchCondition;
                request.PageSize = 0;
                request.PageIndex = 0;
            }
            var response = new VehicleManagementService().GetCRMVehicleByCondition(request);
            
            if (response.IsSuccess)
            {
                if (Action == "导出")
                {
                    return this.Export(response.Result.CRMVehicleCollection);
                }
                else
                {
                    vi.CRMVehicleCollection = response.Result.CRMVehicleCollection;
                    vi.PageIndex = response.Result.PageIndex;
                    vi.PageCount = response.Result.PageCount;
                }

            }
            return View(vi);
        }


        //导出
        private ActionResult Export(IEnumerable<CRMVehicle> crmVehicles)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("车牌号码", typeof(string));
            dt.Columns.Add("营运证号", typeof(string));
            dt.Columns.Add("车型编码", typeof(string));
            dt.Columns.Add("车辆VIN", typeof(string));
            dt.Columns.Add("物流公司", typeof(string));
            dt.Columns.Add("物流公司安全专员联系电话", typeof(string));
            dt.Columns.Add("已行驶公里数", typeof(string));
            dt.Columns.Add("车龄", typeof(string));
            dt.Columns.Add("资质", typeof(string));
            dt.Columns.Add("速度传感制式", typeof(string));
            dt.Columns.Add("号牌种类", typeof(string));
            dt.Columns.Add("燃料种类", typeof(string));
            dt.Columns.Add("车身颜色", typeof(string));
            dt.Columns.Add("生产厂家", typeof(string));
            dt.Columns.Add("整备质量(吨)", typeof(string));
            dt.Columns.Add("加入服务时间", typeof(string));
            dt.Columns.Add("上牌日期", typeof(string));
            dt.Columns.Add("下次年检日期", typeof(string));
            dt.Columns.Add("保险有效截止日期", typeof(string));
            dt.Columns.Add("主要行驶路线", typeof(string));
            dt.Columns.Add("核载(吨)", typeof(string));
            dt.Columns.Add("核定载客(人)", typeof(string));
            dt.Columns.Add("尺寸", typeof(string));
            dt.Columns.Add("总质量(吨)", typeof(string));
            dt.Columns.Add("牵引总质量(吨)", typeof(string));
            dt.Columns.Add("安全带数量", typeof(string));
            dt.Columns.Add("倒车蜂鸣器", typeof(string));
            dt.Columns.Add("油罐车溢油保护装置", typeof(string));
            dt.Columns.Add("防溢油工具", typeof(string));
            dt.Columns.Add("反射条", typeof(string));
            dt.Columns.Add("高位刹车灯", typeof(string));
            dt.Columns.Add("危险品标记", typeof(string));
            dt.Columns.Add("后部行人保护装置", typeof(string));
            dt.Columns.Add("三点式带传感器安全带", typeof(string));
            dt.Columns.Add("翻车保护装置", typeof(string));
            dt.Columns.Add("ABS", typeof(string));
            dt.Columns.Add("承运范围", typeof(string));
            dt.Columns.Add("安全气囊数量", typeof(string));
            dt.Columns.Add("车型类别", typeof(string));
            dt.Columns.Add("挂车号牌", typeof(string));
            dt.Columns.Add("挂车核载(吨)", typeof(string));
            dt.Columns.Add("挂车尺寸", typeof(string));
            dt.Columns.Add("挂车总质量(吨)", typeof(string));
            dt.Columns.Add("挂车整备质量(吨)", typeof(string));
            dt.Columns.Add("挂车车型编号", typeof(string));
            dt.Columns.Add("挂车上牌日期", typeof(string));
            dt.Columns.Add("挂车车辆VIN", typeof(string));
            dt.Columns.Add("挂车下次年检日期", typeof(string));




            crmVehicles.Each((i, s) =>
            {
                DataRow dr = dt.NewRow();
                dr[0] = s.CarNo;
                dr[1] = s.RunNo;
                dr[2] = s.CarTypeNo;
                dr[3] = s.CarVin;
                dr[4] = s.LogisticCompany;
                dr[5] = s.SecurityContactNum;
                dr[6] = s.DrivedJourney;
                dr[7] = s.CarAge;
                dr[8] = s.Qualify;
                dr[9] = s.Velocity_transducers;
                dr[10] = s.CarNumType;
                dr[11] = s.FuelType;
                dr[12] = s.CarBodyColor;
                dr[13] = s.Manufacturer;
                dr[14] = s.EntireCarWeight;
                dr[15] = s.StartServiceDate.ToString("yyyy-MM-dd");
                dr[16] = s.BoardlotDate.ToString("yyyy-MM-dd");
                dr[17] = s.NextYearCheckDate.ToString("yyyy-MM-dd");
                dr[18] = s.InsuranceEndDate.ToString("yyyy-MM-dd");
                dr[19] = s.MainRoute;
                dr[20] = s.LoadWeight;
                dr[21] = s.LoadPerson;
                dr[22] = s.Size;
                dr[23] = s.TotalWeight;
                dr[24] = s.TractionWeight;
                dr[25] = s.SafetyBeltAmount;
                dr[26] = s.BackUpBuzze == true ? "有" : "无";
                dr[27] = s.TheTankerOilSpillProtectionDevice == true ? "有" : "无";
                dr[28] = s.OilSpillPreventiontools == true ? "有" : "无";
                dr[29] = s.ReflectBar == true ? "有" : "无";
                dr[30] = s.HighSideStopLamps == true ? "有" : "无";
                dr[31] = s.DangerousMark == true ? "有" : "无";
                dr[32] = s.BackProtection == true ? "有" : "无";
                dr[33] = s.ThreePointBelt == true ? "有" : "无";
                dr[34] = s.RolloverProtect == true ? "有" : "无";
                dr[35] = s.ABS == true ? "有" : "无";
                dr[36] = s.CarriageScope == true ? "有" : "无";
                dr[37] = s.AirbagAmount;
                dr[38] = s.CarType;
                dr[39] = s.TrailerNo;
                dr[40] = s.TrailerLoadWeight;
                dr[41] = s.TrailerSize;
                dr[42] = s.TrailerTotalWeight;
                dr[43] = s.TrailerEntireWeight;
                dr[44] = s.TrailerTypeNo;
                dr[45] = s.TrailerBoardlotDate;
                dr[46] = s.TrailerVin;
                dr[47] = s.TrailerNextYearCheckDate;
                dt.Rows.Add(dr);
            }); 

            return this.ExportDataTableToExcel(dt, "Vehicle.xls");
        }
        //导入Excel
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



        [HttpGet]
        public ActionResult Create(string id, int? ViewType)
        {
            QueryCRMVehicleViewModel vi = new QueryCRMVehicleViewModel();
            if (string.IsNullOrEmpty(id))
            {
                //CRMVehicle v = new CRMVehicle();
                //v.CarBodyPhoto = Guid.NewGuid().ToString();
                //v.CarFrontPhoto = Guid.NewGuid().ToString();
                //v.CarBackPhoto = Guid.NewGuid().ToString();
                //v.CarFloorPhoto = Guid.NewGuid().ToString();
                //vi.CRMVehicle = v;
                vi.ViewType = 0;
            }
            else
            {
                if (ViewType == 1)
                {
                    var response = new VehicleManagementService().GetSearchVehicle(id);
                    vi.CRMVehicle = response;
                    vi.ViewType = 1;
                }
                else
                {
                    var response = new VehicleManagementService().GetSearchVehicle(id);
                    vi.CRMVehicle = response;
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
        public ActionResult Create(QueryCRMVehicleViewModel vi)
        {
            //string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            //string targetPath = Path.Combine(uploadFolderPath, "AMSTempFile");
            //string url = string.Empty, actualNameInServer = string.Empty, ext = string.Empty;
            if (vi.ViewType == 1) 
            {
                vi.CRMVehicle.CreateUser = base.UserInfo.ID.ToString();
                vi.CRMVehicle.CreateTime = DateTime.Now; 
            }
            vi.CRMVehicle.UpdateUser = base.UserInfo.ID.ToString();
            vi.CRMVehicle.UpdateTime = DateTime.Now;

            var response = new VehicleManagementService().addCreateVehicle(new GetCRMVehicleByConditionRequest()
            {
                CreateFiles = vi.CRMVehicle
                //base.UserInfo.Name
            });
            vi.ViewType = 1;
            return View(vi);
        }


        //删除
        [HttpPost]
        public JsonResult DeleteCRMVehicle(string id)
        {
            bool response = new VehicleManagementService().DeleteCRMVehicle(id);
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
        //车辆司机管理，分页
        [HttpGet]
        public ActionResult VehicleToDriver(string id, int? type)
        {
            VehicleToDriverViewModel vd = new VehicleToDriverViewModel()
            {
                Vehicle = ApplicationConfigHelper.GetVehicleList(),
                Driver = ApplicationConfigHelper.GetDriverList()
            };

            int pagesize = 17;
            var response = new DriverManagementService().GetAllDriver(new GetCRMDriverByConditionRequest()
            {
                PageIndex = vd.PageIndex,
                PageSize = pagesize,
            });

            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshGetCarInfo(); 
                vd.Driver = response.Result.CRMDriverCollection;
                vd.PageIndex = response.Result.PageIndex;
                vd.PageCount = response.Result.PageCount;
            }


            return View(vd);
        }

        //查询
        public string SearchVehicleToDriver(string driverName)//int? Index,  string Action
        {
            VehicleToDriverViewModel sv = new VehicleToDriverViewModel();

            sv.DriverName = driverName;
            var request = new GetCRMDriverByConditionRequest();

            int pagesize = 17;
            request.PageIndex = sv.PageIndex;
            request.PageSize = pagesize;
            request.driverName = driverName;

            var response = new DriverManagementService().GetAllDriver(request);

            if (response.IsSuccess)
            {
                sv.Driver = response.Result.CRMDriverCollection;
                sv.PageIndex = response.Result.PageIndex;
                sv.PageCount = response.Result.PageCount;
            }
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string js = Serializer.Serialize(response);

            return js;
        }


        [HttpPost]
        public string VehicleToDriver(string vehicleNo, long VID, int? Index)
        {
            VehicleToDriverViewModel vd = new VehicleToDriverViewModel();
            vd.PageIndex = Index ?? 0;
            vd.VID = (VID);
            vd.VehicleNo = vehicleNo;
            var request = new GetCRMDriverByConditionRequest();

            int pagesize = 17;
            request.PageIndex = vd.PageIndex;
            request.PageSize = pagesize;

            var response = new DriverManagementService().GetAllDriver(request);

            if (response.IsSuccess)
            {
               
                vd.Driver = response.Result.CRMDriverCollection;
                vd.PageIndex = response.Result.PageIndex;
                vd.PageCount = response.Result.PageCount;
            }
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string js = Serializer.Serialize(response);

            return js;
        }

        public bool AddVehicleToDriver(string vehicle, string jsonStr)
        {

            var request = JsonToModel<CRMDriverName>(jsonStr);
            var response = new DriverManagementService().AddVehicleToDriver(new VehicleMappingDriverRequest
            {
                driver = request,
                VehicleNo = vehicle,
                UserName = base.UserInfo.Name
            });
            if (response == "操作成功")
            {
                ApplicationConfigHelper.RefreshGetCarInfo(); 
                return true;
            }
            else
            {
                return false;
            }


        }

        public static List<T> JsonToModel<T>(string jsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            return Serializer.Deserialize<List<T>>(jsonStr);
        }

        /// <summary>
        /// js-ajax url
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllVehiclesbyVID(string name)
        {
            var vehicles = ApplicationConfigHelper.GetVehicleList();
            return Json(vehicles.Where(s => s.CarNo.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.CarNo }), JsonRequestBehavior.AllowGet);
        }

        public string GetCRM_VehicleMappingDriver(string name)
        {
            VehicleToDriverViewModel sv = new VehicleToDriverViewModel();

            var request = new VehicleMappingDriverRequest();
            request.VehicleNo = name;



            var response = new DriverManagementService().GetCRM_VehicleMappingDriver(request);

            if (response.IsSuccess)
            {
                sv.Driver = response.Result.CRMDriverCollection;
                sv.PageIndex = response.Result.PageIndex;
                sv.PageCount = response.Result.PageCount;
            }



            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string js = Serializer.Serialize(response);

            return js;


        }


        //右边表格查询
        public string GetCRMVehicleMappingDriver(string name, string drivername)
        {
            VehicleToDriverViewModel sv = new VehicleToDriverViewModel();
            sv.VehicleNo = name;
            sv.DriverName = drivername;

            var request = new VehicleMappingDriverRequest();
            request.VehicleNo = name;
            request.DriverName = drivername;


            var response = new DriverManagementService().GetCRMVehicleMappingDriver(request);

            if (response.IsSuccess)
            {
                sv.Driver = response.Result.CRMDriverCollection;
            }

            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string js = Serializer.Serialize(response);

            return js;

        }


    }
}
