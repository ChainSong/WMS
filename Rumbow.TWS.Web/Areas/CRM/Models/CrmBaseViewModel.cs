using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.MessageContracts;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.CRM.Models
{
    public class ViewModel
    {
        public int ID { get; set; }
        public CRMInfo CRMInfo { get; set; }
    }
    public class CrmBaseViewModel
    {
        [Required(ErrorMessage = "必填")]
        
        [Display(Name = "名称")]
        public string CustomerName { get; set; }

        [MaxLength(50)]
        [Display(Name = "项目名称")]
        public string ProjectName { get; set; }


        [Display(Name = "ID")]
        public int ID { get; set; }

       
        public string ID2 { get; set; }

        [MaxLength(50)]
        [Display(Name = "所在城市")]
        public string City { get; set; }

        [Display(Name = "性别")]
        public string Sex { get; set; }

        [Display(Name = "出生日期")]
        public DateTime Date0fBirth { get; set; }

        [Display(Name = "年龄")]
        public string Age { get; set; }


        [Display(Name = "籍贯")]
        public string NativePlace { get; set; }

        [Display(Name = "手机 ")]
        public string Phone { get; set; }

        [Display(Name = "是否结婚")]
        public string IsMarry { get; set; }

        [Display(Name = "学历")]
        public string EducationalBackground { get; set; }

        [Display(Name = "工作单位")]
        public string WorkUnit { get; set; }

        [Display(Name = "在岗时间")]
        public string OnTime { get; set; }

        [Display(Name = "工作年限")]
        public string WorkingLife { get; set; }

        [Display(Name = "工作经历")]
        public string WorkExperience { get; set; }

        [Display(Name = "家庭组成")]
        public string FamilyComposition { get; set; }

        [Display(Name = "家庭地址")]
        public string HomeAddress { get; set; }

        [Display(Name = "家庭成员信息（包括个人信息爱好）")]
        public string FamilyInformation { get; set; }

        [Display(Name = "个人爱好")]
        public string PersonalHobbies { get; set; }

        [Display(Name = "餐饮喜好")]
        public string Foodpreferences { get; set; }

        [Display(Name = "着装喜好")]
        public string Dress { get; set; }

        [Display(Name = "业余消遣")]
        public string Hobby { get; set; }

        [Display(Name = "任职公司")]
        public string TakeOfficeCompany { get; set; }

        [Display(Name = "负责板块")]
        public string SectionResponsibleFor { get; set; }

        [Display(Name = "直接上司")]
        public string Directsupervisor { get; set; }

        [Display(Name = "团队")]
        public string Team { get; set; }

        [Display(Name = "薪资待遇")]
        public string salarytreatment { get; set; }

        [Display(Name = "个人口碑")]
        public string PersonalReputation { get; set; }

        [Display(Name = "对目前工作的满意度")]
        public string TheCurrentJobSatisfaction { get; set; }

        [Display(Name = "与Runbow的接触时间")]
        public string WithRunbowContactTime { get; set; }

        [Display(Name = "与Runbow的接触经历")]
        public string WithRunbowContactExperience { get; set; }

        [Display(Name = "与项目供应商的接触时间")]
        public string WithProjectsupplierContactTime { get; set; }

        [Display(Name = "与其他3PL的接触情况")]
        public string WithOther3PLContact { get; set; }

        [Display(Name = "客户类型")]
        public string CRMtype { get; set; }

        public string Str1 { get; set; }

        public CRMInfo Convert(long? id)
        {
            CRMInfo crmInfo = new CRMInfo()
            {
               

               ID=(long)id,
               
               ProjectName=this.ProjectName,
               CustomerName =this.CustomerName,
               City =this.City,
               Sex =this.Sex,
               Date0fBirth = this.Date0fBirth.ToString(),
               Age =this.Age,
               Phone = this.Phone,
               IsMarry = this.IsMarry,
               EducationalBackground = this.EducationalBackground,
               NativePlace = this.NativePlace,
               WorkUnit =this.WorkUnit,
               OnTime =this.OnTime,
               WorkingLife =this.WorkingLife,
               WorkExperience = this.WorkExperience,
               FamilyComposition =this.FamilyComposition,
               HomeAddress =this.HomeAddress,
               FamilyInformation =this.FamilyInformation,
               PersonalHobbies = this.PersonalHobbies,
               FoodPreferences = this.Foodpreferences,
               Dress = this.Dress,
               Hobby =this.Hobby,
               TakeOfficeCompany = this.TakeOfficeCompany,
               SectionResponsibleFor =this.SectionResponsibleFor,
               DirectSupervisor = this.Directsupervisor,
               Team = this.Team,
               SalaryTreatment =this.salarytreatment,
               PersonalReputation = this.PersonalReputation,
               TheCurrentJobSatisfaction = this.TheCurrentJobSatisfaction,
               WithRunbowContactTime = this.WithRunbowContactTime,
               WithRunbowContactExperience = this.WithRunbowContactExperience,
               WithProjectsupplierContactTime = this.WithProjectsupplierContactTime,
               WithOther3PLContact = this.WithOther3PLContact,
               CRMTYPE = this.CRMtype,
               CreateTime = DateTime.Now.ToString("yyyy-MM-dd"),
            
           
               UpdateTime = DateTime.Now.ToString("yyyy-MM-dd")
             
                ////
                //City = this.City
            };

            return crmInfo;
        }
        public IEnumerable<SelectListItem> type
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "2", Text="经销商"},
                    new SelectListItem(){Value = "0", Text="客户"},
                    new SelectListItem(){Value = "1", Text="承运商"}
                };
            }
        }

        public IEnumerable<SelectListItem> IsMarried
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "已婚", Text="已婚"},
                    new SelectListItem(){Value = "未婚", Text="未婚"}
                };
            }
        }

        public IEnumerable<SelectListItem> issex
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "男", Text="男"},
                    new SelectListItem(){Value = "女", Text="女"}
                };
            }
        }

        public Response<CRMInfo> ResponseCRMInfo { get; set; }
        public CRMInfo CRMInfo { get; set; }
        

    }
}