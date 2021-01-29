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
using Runbow.TWS.Web.Areas.CRM.Models;
using Runbow.TWS.Web.Areas.POD.Models;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Implement;
using Runbow.TWS.Web.Interface;
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using Runbow.TWS.Web.Models;
using System.Data.SqlClient;

using System.Collections;


namespace Runbow.TWS.Web.Areas.CRM.Controllers
{
    public class CrmController : BaseController
    {
        int id1;

        [HttpGet]
        public ActionResult CrmBase(int id)
        {
            CrmBaseViewModel vm = new CrmBaseViewModel();
            CrmTrackViewModel track = new CrmTrackViewModel();
            if (id == 0)
            {
                vm.CRMtype = "客户";
            }
            else if (id == 1)
            {
                vm.CRMtype = "承运商";
            }
            else if (id == 2)
            {
                vm.CRMtype = "经销商";
            }

            vm.ID = 0;
          
            return View(vm);
        }

        [HttpGet]
        public ActionResult CrmBasView2(long? id)
        {
            CrmBaseViewModel view = new CrmBaseViewModel();
            view.CRMInfo = new CRMService().GetCRMInfoByID(new GetCRMInfoRequest() { ID = id }).Result;
            view.WithRunbowContactExperience = view.CRMInfo.WithRunbowContactExperience;
            view.WorkUnit = view.CRMInfo.WorkUnit;
            view.WorkingLife = view.CRMInfo.WorkingLife;
            view.WorkExperience = view.CRMInfo.WorkExperience;
            view.TakeOfficeCompany = view.CRMInfo.TakeOfficeCompany;
            view.Team = view.CRMInfo.Team;
            view.WithRunbowContactTime = view.CRMInfo.WithRunbowContactTime;
            view.WithProjectsupplierContactTime = view.CRMInfo.WithProjectsupplierContactTime;
            view.TheCurrentJobSatisfaction = view.CRMInfo.TheCurrentJobSatisfaction;
            view.WithRunbowContactTime = view.CRMInfo.WithRunbowContactTime;
            view.WithOther3PLContact = view.CRMInfo.WithOther3PLContact;


            view.Age = view.CRMInfo.Age;
            RegionSelector rs = new RegionSelector();
            rs.NameKey = view.CRMInfo.City;

            //    ViewBag.js = "<script type='text/javascript'>"
            //        + "$(document).ready(function () {$(this).prev().find('.RegionName').val('" + view.CRMInfo.City + "');"
            //+ " $(this).prev().find('.RegionID').val('" + view.CRMInfo.City + "');"
            // + "$(this).next().val('" + view.CRMInfo.City + "');"
            // + "$(this).next().next().val('" + view.CRMInfo.City + "');});</script>";
             
            view.CustomerName = view.CRMInfo.CustomerName;
            view.Date0fBirth = Convert.ToDateTime(view.CRMInfo.Date0fBirth);  
            view.Directsupervisor = view.CRMInfo.DirectSupervisor;
            view.Dress = view.CRMInfo.Dress;
            view.EducationalBackground = view.CRMInfo.EducationalBackground;
            view.FamilyComposition = view.CRMInfo.FamilyComposition;
            view.FamilyInformation = view.CRMInfo.FamilyInformation;
            view.Foodpreferences = view.CRMInfo.FoodPreferences;
            view.Hobby = view.CRMInfo.Hobby;
            view.HomeAddress = view.CRMInfo.HomeAddress;
            view.IsMarry = view.CRMInfo.IsMarry;
            view.Sex = view.CRMInfo.Sex;
            view.NativePlace = view.CRMInfo.NativePlace;
            view.OnTime = view.CRMInfo.OnTime;
            view.PersonalHobbies = view.CRMInfo.PersonalHobbies;
            view.PersonalReputation = view.CRMInfo.PersonalReputation;
            view.Phone = view.CRMInfo.Phone;
            view.ProjectName = view.CRMInfo.ProjectName;
            view.salarytreatment = view.CRMInfo.SalaryTreatment;
            view.SectionResponsibleFor = view.CRMInfo.SectionResponsibleFor;
            view.CRMtype = view.CRMInfo.CRMTYPE;
            view.ProjectName = view.CRMInfo.ProjectName;
            view.ID = (int)id;
            view.CRMInfo.ID = (long)id;
            //string crmtype = "";
            //foreach (SelectListItem items in view.type)
            //{
            //    if (items.Value == id.ToString())
            //    {
            //        crmtype = items.Text;
            //        break;
            //    }
            //}
            //ViewBag.Message = crmtype + "   操作成功! ";

            return View("CrmBase", view);
        }
        [HttpGet]
        public ActionResult CrmImport()
        {

            return View();
        }

        /// <summary>
        /// 基础信息添加  修改
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CrmBase(CrmBaseViewModel vm, long? id)
        {

            vm.ResponseCRMInfo = new CRMService().OperateCRMInfo(new AddOrUpdateCRMInfoRequest() { CRMInfo = vm.Convert(vm.ID) });
            int id2 = (int)vm.ResponseCRMInfo.Result.ID;

            if (vm.ResponseCRMInfo.IsSuccess)
            {

                if (vm.CRMInfo.ID == 0)
                {
                    ViewBag.Message = " 添加成功,可继续编辑!";
                }
                else if (vm.CRMInfo.ID != 0)
                {

                    ViewBag.Message = "编辑成功!";

                }

                CrmBaseViewModel view = new CrmBaseViewModel();
                view.CRMInfo = new CRMService().GetCRMInfoByID(new GetCRMInfoRequest() { ID = id2 }).Result;
                view.WithRunbowContactExperience = view.CRMInfo.WithRunbowContactExperience;
                view.WorkUnit = view.CRMInfo.WorkUnit;
                view.WorkingLife = view.CRMInfo.WorkingLife;
                view.WorkExperience = view.CRMInfo.WorkExperience;
                view.TakeOfficeCompany = view.CRMInfo.TakeOfficeCompany;
                view.Team = view.CRMInfo.Team;
                view.WithRunbowContactTime = view.CRMInfo.WithRunbowContactTime;
                view.WithProjectsupplierContactTime = view.CRMInfo.WithProjectsupplierContactTime;
                view.TheCurrentJobSatisfaction = view.CRMInfo.TheCurrentJobSatisfaction;
                view.WithRunbowContactTime = view.CRMInfo.WithRunbowContactTime;
                view.WithOther3PLContact = view.CRMInfo.WithOther3PLContact;


                view.Age = view.CRMInfo.Age;
                view.City = view.CRMInfo.City;
                view.CustomerName = view.CRMInfo.CustomerName;
                view.Date0fBirth = Convert.ToDateTime(view.CRMInfo.Date0fBirth);
                view.Directsupervisor = view.CRMInfo.DirectSupervisor;
                view.Dress = view.CRMInfo.Dress;
                view.EducationalBackground = view.CRMInfo.EducationalBackground;
                view.FamilyComposition = view.CRMInfo.FamilyComposition;
                view.FamilyInformation = view.CRMInfo.FamilyInformation;
                view.Foodpreferences = view.CRMInfo.FoodPreferences;
                view.Hobby = view.CRMInfo.Hobby;
                view.HomeAddress = view.CRMInfo.HomeAddress;
                view.IsMarry = view.CRMInfo.IsMarry;
                view.Sex = view.CRMInfo.Sex;
                view.NativePlace = view.CRMInfo.NativePlace;
                view.OnTime = view.CRMInfo.OnTime;
                view.PersonalHobbies = view.CRMInfo.PersonalHobbies;
                view.PersonalReputation = view.CRMInfo.PersonalReputation;
                view.Phone = view.CRMInfo.Phone;
                view.ProjectName = view.CRMInfo.ProjectName;
                view.salarytreatment = view.CRMInfo.SalaryTreatment;
                view.SectionResponsibleFor = view.CRMInfo.SectionResponsibleFor;
                view.CRMtype = view.CRMInfo.CRMTYPE;
                view.ProjectName = view.CRMInfo.ProjectName;

                view.ID = (int)id2;
                view.CRMInfo.ID = (long)id2;
                ModelState.Clear();


                return View(view);
            }

            return View(vm);
        }


        public ActionResult CrmTrack(int? id, int? typeid)
        {

            CrmTrackViewModel vm = new CrmTrackViewModel();
            vm.typeid = typeid.ObjectToInt32();
            vm.CRMID = (int)id;
            vm.TempID = 0;
            return View(vm);
        }

        /// <summary>
        /// 跟踪信息 添加
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CrmTrack(CrmTrackViewModel vm)
        {
            //AddOrUpdateCRMTrackInfoRequest view = new AddOrUpdateCRMTrackInfoRequest();



            vm.ResponseTrackInfo = new CRMService().OperateCRMTrackInfo(new AddOrUpdateCRMTrackInfoRequest() { CRMTrackInfo = vm.Convert() });


            //if (vm.ResponseTrackInfo.IsSuccess)
            //{
            //    string crmtype = "";
            //    foreach (SelectListItem item in vm.type)
            //    {
            //        if (item.Value == id.ToString())
            //        {
            //            crmtype = item.Text;
            //            break;
            //        }
            //    }

            ViewBag.Message = " 操作成功!";
            // Server.Execute("CrmManage.cshtml");
            // Response.Redirect("Crm/CrmManage.cshtml");  
            //RedirectToAction("CrmManage", "CrmController");
            //Redirect("/Crm/CrmManage.cshtml");
            return RedirectToAction("CrmViewInfo", new { id = vm.CRMID, typeid = vm.typeid });
            //}
            // return View(vm);
        }

        public ActionResult CrmManage(int id)
        {
            //CrmTrackViewModel vm = new CrmTrackViewModel();
            // vm.typeid = id;
            CrmBaseViewModel vm2 = new CrmBaseViewModel();
            CrmManageViewModel vm = new CrmManageViewModel();
            foreach (SelectListItem items in vm2.type)
            {
                if (items.Value == id.ToString())
                {
                    vm.crmtype = items.Text;

                    break;
                }
            }
            vm.crmtype = vm.crmtype;
            return View(vm);
        }

        [HttpPost]
        public JsonResult DeleteCrminfo(long id)
        {
            var response = new CRMService().DeleteCRMInfo(new DeleteCRMInfoRequest() { ID = id });
            if (response.IsSuccess)
            {
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else
            {
                if (response.Result == 0)
                {
                    return Json(new { Message = "无法删除", IsSuccess = false });
                }

                return Json(new { Message = "删除失败！", IsSuccess = false });
            }
        }


        [HttpPost]
        public JsonResult DeleteCrmTrackinfo(long id)
        {
            var response = new CRMService().DeleteCRMTrackInfo(new DeleteCRMTrackInfoRequest() { ID = id });
            if (response.IsSuccess)
            {
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else
            {
                if (response.Result == 0)
                {
                    return Json(new { Message = "无法删除", IsSuccess = false });
                }

                return Json(new { Message = "删除失败！", IsSuccess = false });
            }
        }


        [HttpPost]
        public ActionResult CrmManage(CrmManageViewModel vm, int? PageIndex, int? id)
        {
            //CrmTrackViewModel track=new CrmTrackViewModel();
            //id = track.typeid;
            // id = id1;
            CrmBaseViewModel Model = new CrmBaseViewModel();
            string crmtype = "";
            foreach (SelectListItem items in Model.type)
            {
                if (items.Value == id.ToString())
                {
                    crmtype = items.Text;
                    break;
                }
            }
            vm.Message = crmtype + "列表";
            vm.CRMInfo.CRMTYPE = crmtype;
            vm.TypeID = id;
            var Result = new CRMService().GetCRMInfo(new GetCRMInfoRequest() { CRMInfo = vm.CRMInfo, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 }).Result;
            vm.IEnumerableCRMInfo = Result.IEnumerableCRMInfo;
            vm.PageIndex = Result.PageIndex;
            vm.PageSize = Result.PageSize;
            vm.PageCount = Result.PageCount;
            vm.crmtype = crmtype;
            return View(vm);

        }

        public ActionResult CrmViewInfo(long? id, int typeid)
        {

            CRMTrackInfo info = new CRMTrackInfo();
            info.CRMID = id.ObjectToInt64();

            CrmViewInfoViewModel ViewModel = new CrmViewInfoViewModel()
            {
                CRMInfo = new CRMService().GetCRMInfoByID(new GetCRMInfoRequest() { ID = id }).Result,
                CRMTrackInfoList = new CRMService().GetCRMTrackInfo(new GetCRMTrackInfoRequest() { CRMTrackInfo = info }).Result

            };
            ViewModel.TypeID = typeid;
            return View(ViewModel);
        }


        [HttpGet]
        public ActionResult CrmTrackSing(long? id, int typeid)
        {

            CrmTrackViewModel view = new CrmTrackViewModel();
            view.CRMTrackInfo = new CRMService().GetCRMTrackInfoByID(new GetCRMTrackInfoRequest() { ID = id }).Result;

            view.TempID = view.CRMTrackInfo.ID.ObjectToInt32();
            view.CRMID = view.CRMTrackInfo.CRMID;
            view.VisitPeople = view.CRMTrackInfo.VisitPeople;
            view.VisitTime = Convert.ToDateTime(view.CRMTrackInfo.VisitTime);
            view.VisitPlace = view.CRMTrackInfo.VisitPlace;
            view.VisitForm = view.CRMTrackInfo.VisitForm;
            view.GiftsArticles = view.CRMTrackInfo.GiftsArticles;
            view.VisitToCustomerEvaluation = view.CRMTrackInfo.VisitToCustomerEvaluation;
            view.VisitingPersonnelFeedbackVisit = view.CRMTrackInfo.VisitingPersonnelFeedbackVisit;
            view.ProjectCustomerCommunication = view.CRMTrackInfo.ProjectCustomerCommunication;
            view.CustomerSupportAndAssistance = view.CRMTrackInfo.CustomerSupportAndAssistance;

            return View("CrmTrack", view);
        }

        [HttpPost]
        public ActionResult CrmViewInfo(CrmViewInfoViewModel vm)
        {
            return View();
        }

        public void Excel(string type)
        {
            //CrmManageViewModel vm = new CrmManageViewModel();
            // List<CrmBaseViewModel> entityList = new List<CrmBaseViewModel>();


            //获取所有用户信息
            //

            CRMService crm = new CRMService();
            DataTable dt = crm.GetCRMInfodao(type);
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("项目名称");
            dt2.Columns.Add("名称");
            dt2.Columns.Add("地址");
            dt2.Columns.Add("性别");
            dt2.Columns.Add("出生日期");
            dt2.Columns.Add("年龄");
            dt2.Columns.Add("手机");
            dt2.Columns.Add("是否结婚");
            dt2.Columns.Add("学历");
            dt2.Columns.Add("籍贯");
            dt2.Columns.Add("工作单位");
            dt2.Columns.Add("在岗时间");
            dt2.Columns.Add("工作年限");
            dt2.Columns.Add("工作经历");
            dt2.Columns.Add("家庭组成");
            dt2.Columns.Add("家庭地址");
            dt2.Columns.Add("家庭成员信息（包括个人信息爱好）");
            dt2.Columns.Add("个人爱好");
            dt2.Columns.Add("餐饮喜好");
            dt2.Columns.Add("着装喜好");
            dt2.Columns.Add("业余消遣");
            dt2.Columns.Add("任职公司");
            dt2.Columns.Add("负责板块");
            dt2.Columns.Add("直接上司");
            dt2.Columns.Add("团队");
            dt2.Columns.Add("薪资待遇");
            dt2.Columns.Add("个人口碑");
            dt2.Columns.Add("对目前工作的满意度");
            dt2.Columns.Add("与Runbow的接触时间");
            dt2.Columns.Add("与Runbow的接触经历");
            dt2.Columns.Add("与项目供应商的接触时间");
            dt2.Columns.Add("与其他3PL的接触情况");
            dt2.Columns.Add("类型");
            dt2.Columns.Add("新建时间");
            dt2.Columns.Add("修改时间");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt2.Rows.Add(dt2.NewRow());
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    dt2.Rows[i][j] = dt.Rows[i][j];

                }
            }


            string targetPath = Server.MapPath("~/DownLoadExcel");
            NewExcelHelper excelHelper = new NewExcelHelper();
            string execlname = "CRMInfo表" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            string fileFullPath = Path.Combine(targetPath, execlname);
            excelHelper.CreateExcelByDataTable(fileFullPath, dt2);
            excelHelper.Dispose();
            FileDownLoad(fileFullPath);

            //if (fileFullPath != "")
            //{

            //    File.Delete(fileFullPath);
            //}


        }

        private void FileDownLoad(string filename)
        {
            string destFileName = filename;
            //destFileName = Server.MapPath("./") + destFileName;

            destFileName = Server.UrlDecode(destFileName);

            if (MyFile.Exists(destFileName))
            {
                FileInfo fi = new FileInfo(filename);
                Response.Clear();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.Charset = "GB2312";

                //添加头信息，为 "文件下载/另存为 "对话框指定默认文件名  
                Response.AppendHeader("Content-Disposition", "attachment;filename="
                + HttpUtility.UrlEncode(Path.GetFileName(destFileName),
              Encoding.UTF8));
                Response.AppendHeader("Content-Length", fi.Length.ToString());
                Response.ContentType = "text/plain";
                Response.Filter.Close();
                Response.WriteFile(destFileName);
                Response.Flush();
                Response.End();
            }
            else
            {
                Response.Write("<script language = javascript>alert('下载出错')</script>");
            }
        }
        [HttpPost]
        public string PodImport()
        {
            CRMInfo reuqest = new CRMInfo();
            int i = 0;
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("项目名称");
            dt2.Columns.Add("名称");
            dt2.Columns.Add("地址");
            dt2.Columns.Add("性别");
            dt2.Columns.Add("出生日期");
            dt2.Columns.Add("年龄");
            dt2.Columns.Add("手机"); if (Request.Files.Count > 0)
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                    if (hpf.ContentLength > 0)
                    {
                        DataSet ds = this.GetDataFromExcel(hpf);
                        DataTable dt = ds.Tables[0];
                        if (ds != null && ds.Tables[0] != null)
                        {

                            foreach (DataRow dr in dt.Rows)
                            {
                                string ProjectName = dr["项目名称"].ToString();
                                string CustomerName = dr["名称"].ToString();
                                string City = dr["地址"].ToString();
                                string Sex = dr["性别"].ToString();
                                string Date0fBirth = dr["出生日期"].ToString();
                                string Age = dr["年龄"].ToString();
                                string Phone = dr["手机"].ToString();
                                string IsMarry = dr["是否结婚"].ToString();
                                string EducationalBackground = dr["学历"].ToString();
                                string NativePlace = dr["籍贯"].ToString();
                                string WorkUnit = dr["工作单位"].ToString();
                                string OnTime = dr["在岗时间"].ToString();
                                string WorkingLife = dr["工作年限"].ToString();
                                string WorkExperience = dr["工作经历"].ToString();
                                string FamilyComposition = dr["家庭组成"].ToString();
                                string HomeAddress = dr["家庭地址"].ToString();
                                string FamilyInformation = dr["家庭成员信息（包括个人信息爱好）"].ToString();
                                string PersonalHobbies = dr["个人爱好"].ToString();
                                string FoodPreferences = dr["餐饮喜好"].ToString();
                                string Dress = dr["着装喜好"].ToString();
                                string Hobby = dr["业余消遣"].ToString();
                                string TakeOfficeCompany = dr["任职公司"].ToString();
                                string SectionResponsibleFor = dr["负责板块"].ToString();
                                string DirectSupervisor = dr["直接上司"].ToString();
                                string Team = dr["团队"].ToString();
                                string SalaryTreatment = dr["薪资待遇"].ToString();
                                string PersonalReputation = dr["个人口碑"].ToString();
                                string TheCurrentJobSatisfaction = dr["对目前工作的满意度"].ToString();
                                string WithRunbowContactTime = dr["与Runbow的接触时间"].ToString();
                                string WithRunbowContactExperience = dr["与Runbow的接触经历"].ToString();
                                string WithProjectsupplierContactTime = dr["与项目供应商的接触时间"].ToString();
                                string WithOther3PLContact = dr["与其他3PL的接触情况"].ToString();
                                string CRMTYPE = dr["类型"].ToString();
                                string CreateTime = dr["新建时间"].ToString();
                                string UpdateTime = dr["修改时间"].ToString();
                                string sql = string.Format("insert into [dbo].[CRMInfo](ProjectName, CustomerName, City, Sex, Date0fBirth, Age, Phone, IsMarry, EducationalBackground, NativePlace, WorkUnit, OnTime, WorkingLife, WorkExperience, FamilyComposition, HomeAddress, FamilyInformation, PersonalHobbies, FoodPreferences, Dress, Hobby, TakeOfficeCompany, SectionResponsibleFor, DirectSupervisor, Team, SalaryTreatment, PersonalReputation, TheCurrentJobSatisfaction, WithRunbowContactTime, WithRunbowContactExperience, WithProjectsupplierContactTime, WithOther3PLContact, CRMTYPE, CreateTime, UpdateTime) values( '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}')", ProjectName, CustomerName, City, Sex, Date0fBirth, Age, Phone, IsMarry, EducationalBackground, NativePlace, WorkUnit, OnTime, WorkingLife, WorkExperience, FamilyComposition, HomeAddress, FamilyInformation, PersonalHobbies, FoodPreferences, Dress, Hobby, TakeOfficeCompany, SectionResponsibleFor, DirectSupervisor, Team, SalaryTreatment, PersonalReputation, TheCurrentJobSatisfaction, WithRunbowContactTime, WithRunbowContactExperience, WithProjectsupplierContactTime, WithOther3PLContact, CRMTYPE, CreateTime, UpdateTime);
                                i = CRMService.Import(sql);
                                reuqest.ProjectName = ProjectName;
                                reuqest.CustomerName = CustomerName;
                                reuqest.City = City;
                                reuqest.Sex = Sex;
                                reuqest.Date0fBirth = Date0fBirth;
                                reuqest.Age = Age;
                                reuqest.Phone = Phone;

                                DataRow dr2;
                                dr2 = dt2.NewRow(); ;

                                dr2["项目名称"] = reuqest.ProjectName;
                                dr2["名称"] = reuqest.CustomerName;
                                dr2["地址"] = reuqest.City;
                                dr2["性别"] = reuqest.Sex;
                                dr2["出生日期"] = reuqest.Date0fBirth;
                                dr2["年龄"] = reuqest.Age;
                                dr2["手机"] = reuqest.Phone;

                                dt2.Rows.Add(dr2);





                                //dt2.Rows.Add(dr2);  dt2.Columns.Add(reuqest.ProjectName);
                                //dt2.Columns.Add(reuqest.CustomerName);
                                //dt2.Columns.Add(reuqest.City);
                                //dt2.Columns.Add(reuqest.Sex);
                                //dt2.Columns.Add(reuqest.Date0fBirth);
                                //dt2.Columns.Add(reuqest.Age);
                                //dt2.Columns.Add(reuqest.Phone);
                            }

                            //string ProjectName = dr["项目名称"].ToString();
                            //string CustomerName = dr["名称"].ToString();
                            //string City = dr["地址"].ToString();
                            //string Sex = dr["性别"].ToString();
                            //string Date0fBirth = dr["出生日期"].ToString();
                            //string Age = dr["年龄"].ToString();
                            //string Phone = dr["手机"].ToString();

                            StringBuilder successSB = new StringBuilder();
                            successSB.Append("<h3>导入成功</h3><br/>");
                            successSB.Append("<table><thead><tr><th>").Append("<tr><td>").Append("项目名称")
                           .Append("</td><td>").Append("名称")
                           .Append("</td><td>").Append("地址")
                            .Append("</td><td>").Append("性别")
                            .Append("</td><td>").Append("出生日期")
                           .Append("</td><td>").Append("年龄")
                           .Append("</td><td>").Append("手机")
                              .Append("</td></tr>");
                            if (i > 0)
                            {
                                foreach (DataRow dr in dt2.Rows)
                                {

                                    string ProjectName = dr["项目名称"].ToString();
                                    string CustomerName = dr["名称"].ToString();
                                    string City = dr["地址"].ToString();
                                    string Sex = dr["性别"].ToString();
                                    string Date0fBirth = dr["出生日期"].ToString();
                                    string Age = dr["年龄"].ToString();
                                    string Phone = dr["手机"].ToString();



                                    successSB.Append("<tr><td>").Append(ProjectName)
                                       .Append("</td><td>").Append(CustomerName)
                                       .Append("</td><td>").Append(City)
                                       .Append("</td><td>").Append(Sex)
                                       .Append("</td><td>").Append(Date0fBirth)
                                       .Append("</td><td>").Append(Age)
                                       .Append("</td><td>").Append(Phone)
                                        .Append("</th></td></tr></thead><tbody>");


                                }

                                return new { result = successSB.ToString(), IsSuccess = true }.ToJsonString();

                            }
                            else
                            {
                                return new { result = "<h3>导入失败</h3><br/>系统忙，请稍后再试", IsSuccess = false }.ToJsonString();

                            }
                        }

                        return new { result = "<h3>导入失败</h3><br/>excel内容有误！", IsSuccess = false }.ToJsonString();
                    }

                    return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
                }
            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();


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

        private IEnumerable<CRMInfo> InitPodFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb)
        {
            IList<CRMInfo> crms = new List<CRMInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CRMInfo crm = new CRMInfo();
                //crm.ProjectID = base.UserInfo.ProjectID;
                //crm.Creator = base.UserInfo.Name;
                //pod.CreateTime = DateTime.Now;
                //pod.Type = 2;
                string columnName;
                string value;
                //ID, ProjectName, CustomerName, City, Sex, Date0fBirth, Age, Phone, IsMarry, EducationalBackground, NativePlace, WorkUnit, OnTime, WorkingLife, WorkExperience, FamilyComposition, HomeAddress, FamilyInformation, PersonalHobbies, FoodPreferences, Dress, Hobby, TakeOfficeCompany, SectionResponsibleFor, DirectSupervisor, Team, SalaryTreatment, PersonalReputation, TheCurrentJobSatisfaction, WithRunbowContactTime, WithRunbowContactExperience, WithProjectsupplierContactTime, WithOther3PLContact, CRMTYPE, CreateTime, UpdateTime, Str1, Str2, Str3, Str4, Str5, Str6, Str7, Str8, Str9, Str10, Str11, Str12
                if (useCustomerOrderNumber)
                {
                    columnName = columnsConfig.First(c => c.DbColumnName == "ProjectName").DisplayName;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = dt.Rows[i][j].ToString();
                            if (string.IsNullOrEmpty(value))
                            {
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + " 不能为空<br/>");
                            }
                            crm.ProjectName = value;
                            break;
                        }
                    }
                }


                columnName = columnsConfig.First(c => c.DbColumnName == "CustomerName").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + " 不能为空<br/>");
                            break;
                        }

                        var customer = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).FirstOrDefault(c => string.Equals(c.CustomerName, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (customer == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列,  <strong>" + value + " </strong> 在系统中不存在或当前用户无权限导入此客户运单，请先配置。<br/>");
                            break;
                        }
                        crm.CustomerName = value.Trim();
                        // pod.CustomerID = customer.CustomerID;
                        break;
                    }
                }


                columnName = columnsConfig.First(c => c.DbColumnName == "City").DisplayName;
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
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                                break;
                            }
                            crm.City = value;
                            // pod.ShipperTypeID = shipperType.ID;
                            break;
                        }

                        break;
                    }
                }
                // Sex, Date0fBirth, Age, Phone, IsMarry, EducationalBackground, NativePlace, WorkUnit, OnTime, WorkingLife, WorkExperience, FamilyComposition, HomeAddress, FamilyInformation, PersonalHobbies, FoodPreferences, Dress, Hobby, TakeOfficeCompany, SectionResponsibleFor, DirectSupervisor, Team, SalaryTreatment, PersonalReputation, TheCurrentJobSatisfaction, WithRunbowContactTime, WithRunbowContactExperience, WithProjectsupplierContactTime, WithOther3PLContact, CRMTYPE, CreateTime, UpdateTime, Str1, Str2, Str3, Str4, Str5, Str6, Str7, Str8, Str9, Str10, Str11, Str12
                columnName = columnsConfig.First(c => c.DbColumnName == "Sex").DisplayName;
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
                                sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                                break;
                            }
                            crm.Sex = value;
                            // pod.TtlOrTplID = ttlOrTpl.ID;
                            break;
                        }

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "Date0fBirth").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var podType = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE).FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (podType == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.Date0fBirth = value;
                        // pod.PODTypeID = podType.ID;
                        break;
                    }
                }

                //var initPodState = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE).First(s => s.Code == "01");
                //pod.PODStateID = initPodState.ID;
                //pod.PODStateName = initPodState.Name;

                columnName = columnsConfig.First(c => c.DbColumnName == "Date0fBirth").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var startCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (startCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.Date0fBirth = value;
                        //  pod.StartCityID = startCity.ID;
                        break;
                    }
                }
                //Age, Phone, IsMarry, EducationalBackground, NativePlace, WorkUnit, OnTime, WorkingLife, WorkExperience, FamilyComposition, HomeAddress, FamilyInformation, PersonalHobbies, FoodPreferences, Dress, Hobby, TakeOfficeCompany, SectionResponsibleFor, DirectSupervisor, Team, SalaryTreatment, PersonalReputation, TheCurrentJobSatisfaction, WithRunbowContactTime, WithRunbowContactExperience, WithProjectsupplierContactTime, WithOther3PLContact, CRMTYPE, CreateTime, UpdateTime, Str1, Str2, Str3, Str4, Str5, Str6, Str7, Str8, Str9, Str10, Str11, Str12
                columnName = columnsConfig.First(c => c.DbColumnName == "Age").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.Age = value;
                        //pod.EndCityID = endCity.ID;
                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "Phone").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.Phone = value;
                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "IsMarry").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.IsMarry = value;

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "EducationalBackground").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.EducationalBackground = value;

                        break;
                    }
                }
                //NativePlace, WorkUnit, OnTime, WorkingLife, WorkExperience, FamilyComposition, HomeAddress, FamilyInformation, PersonalHobbies, FoodPreferences, Dress, Hobby, TakeOfficeCompany, SectionResponsibleFor, DirectSupervisor, Team, SalaryTreatment, PersonalReputation, TheCurrentJobSatisfaction, WithRunbowContactTime, WithRunbowContactExperience, WithProjectsupplierContactTime, WithOther3PLContact, CRMTYPE, CreateTime, UpdateTime, Str1, Str2, Str3, Str4, Str5, Str6, Str7, Str8, Str9, Str10, Str11, Str12
                columnName = columnsConfig.First(c => c.DbColumnName == "NativePlace").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.NativePlace = value;

                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "WorkUnit").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.WorkUnit = value;


                        break;
                    }
                }

                columnName = columnsConfig.First(c => c.DbColumnName == "OnTime").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.OnTime = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "WorkingLife").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.WorkingLife = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "WorkExperience").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.WorkExperience = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "FamilyComposition").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.FamilyComposition = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "HomeAddress").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.HomeAddress = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "FamilyInformation").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {  
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.FamilyInformation = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "PersonalHobbies").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.PersonalHobbies = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "FoodPreferences").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.FoodPreferences = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "Dress").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.Dress = value;


                        break;
                    }
                }
                // Hobby, TakeOfficeCompany, SectionResponsibleFor, DirectSupervisor, Team, SalaryTreatment, PersonalReputation, TheCurrentJobSatisfaction, WithRunbowContactTime, WithRunbowContactExperience, WithProjectsupplierContactTime, WithOther3PLContact, CRMTYPE, CreateTime, UpdateTime, Str1, Str2, Str3, Str4, Str5, Str6, Str7, Str8, Str9, Str10, Str11, Str12
                columnName = columnsConfig.First(c => c.DbColumnName == "Hobby").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.Hobby = value;
                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "TakeOfficeCompany").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.TakeOfficeCompany = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "SectionResponsibleFor").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.SectionResponsibleFor = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "DirectSupervisor").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.DirectSupervisor = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "Team").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.Team = value;


                        break;
                    }
                }
                // SalaryTreatment, PersonalReputation, TheCurrentJobSatisfaction, WithRunbowContactTime, WithRunbowContactExperience, WithProjectsupplierContactTime, WithOther3PLContact, CRMTYPE, CreateTime, UpdateTime, Str1, Str2, Str3, Str4, Str5, Str6, Str7, Str8, Str9, Str10, Str11, Str12
                columnName = columnsConfig.First(c => c.DbColumnName == "SalaryTreatment").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.SalaryTreatment = value;
                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "PersonalReputation").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }
                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.PersonalReputation = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "TheCurrentJobSatisfaction").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.TheCurrentJobSatisfaction = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "WithRunbowContactTime").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.WithRunbowContactTime = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "WithRunbowContactExperience").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.WithRunbowContactExperience = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "WithProjectsupplierContactTime").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.WithProjectsupplierContactTime = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "WithOther3PLContact").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.WithOther3PLContact = value;


                        break;
                    }
                }
                // CRMTYPE, CreateTime, UpdateTime, Str1, Str2, Str3, Str4, Str5, Str6, Str7, Str8, Str9, Str10, Str11, Str12
                columnName = columnsConfig.First(c => c.DbColumnName == "CRMTYPE").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.CRMTYPE = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "CreateTime").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.CreateTime = value;


                        break;
                    }
                }
                columnName = columnsConfig.First(c => c.DbColumnName == "UpdateTime").DisplayName;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = dt.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + "不能为空<br/>");
                            break;
                        }

                        var endCity = ApplicationConfigHelper.GetRegions().FirstOrDefault(c => string.Equals(c.Name, value.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (endCity == null)
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong> 在系统中不存在，请先配置。<br/>");
                            break;
                        }
                        crm.UpdateTime = value;
                        break;
                    }
                }
                #region MyRegion

                //foreach (var column in columnsConfig.Where(c => c.IsImportColumn && !c.IsKey))
                //{
                //    for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                //        {
                //            value = dt.Rows[i][j].ToString().Trim();
                //            if (!string.IsNullOrEmpty(value))
                //            {
                //                if (column.Type == "DateTime")1
                //                {
                //                    DateTime dttemp;
                //                    if (DateTime.TryParse(value, out dttemp))
                //                    {
                //                        typeof(Pod).GetProperty(column.DbColumnName).SetValue(pod, dttemp, null);
                //                    }
                //                    else
                //                    {
                //                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                //                    }
                //                }
                //                else if (column.Type == "CheckBox")
                //                {
                //                    if (value == "1" || value == "0" || value == "是" || value == "否")
                //                    {
                //                        if (value == "1" || value == "是")
                //                        {
                //                            typeof(Pod).GetProperty(column.DbColumnName).SetValue(pod, "1", null);
                //                        }
                //                        else
                //                        {
                //                            typeof(Pod).GetProperty(column.DbColumnName).SetValue(pod, "0", null);
                //                        }
                //                    }
                //                    else
                //                    {
                //                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否中一个，请修改。<br />");
                //                    }
                //                }
                //                else
                //                {
                //                    typeof(Pod).GetProperty(column.DbColumnName).SetValue(pod, value, null);
                //                }

                //                break;
                //            }

                //            break;
                //        }
                //    }
                //} 
                #endregion

                crms.Add(crm);
            }

            return crms;
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
    }


}
