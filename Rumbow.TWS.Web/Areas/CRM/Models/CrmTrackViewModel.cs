using Runbow.TWS.Entity;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.CRM.Models
{
    public class CrmTrackViewModel
    {
        public IEnumerable<CRMTrackInfo> CRMTrackInfoList { get; set; }
        public CRMTrackInfo CRMTrackInfo { get; set; }
        public Response<CRMTrackInfo> ResponseTrackInfo { get; set; }

        public CrmTrackViewModel() { }
       
        public int TempID{ get; set; }//	主键ID

        public int typeid { get; set; }//	typeid

        public long CRMID{ get; set; }//外建

      
        [Display(Name = "拜访人")]
        public string VisitPeople{ get; set; }  //	拜访人				

      
        [Display(Name = "拜访时间")]
        public DateTime VisitTime{ get; set; }  //	拜访时间				

       
        [Display(Name = "拜访地点")]
        public string VisitPlace{ get; set; }  //	拜访地点				

        
        [Display(Name = "拜访形式")]
        public string VisitForm{ get; set; }  //	拜访形式				

        
        [Display(Name = "赠送礼品")]
        public string GiftsArticles{ get; set; }  //	赠送礼品
			
      
        [Display(Name = "拜访人员对客户的评价")]
        public string VisitToCustomerEvaluation{ get; set; }  //	拜访人员对客户的评价	
			
        
        [Display(Name = "拜访人员反馈拜访情况")]
        public string VisitingPersonnelFeedbackVisit{ get; set; }  //	拜访人员反馈拜访情况	
			
        
        [Display(Name = "日常项目客户沟通记录")]
        public string ProjectCustomerCommunication{ get; set; }  //	日常项目客户沟通记录
		
        [Display(Name = "项目日常运作所获得该客户支持与协助记录")]
        public string CustomerSupportAndAssistance{ get; set; }  //	项目日常运作所获得该客户支持与协助记录				

     
        public string CRMtype { get; set; }

        public IEnumerable<SelectListItem> type
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "2", Text="经销商"},
                    new SelectListItem(){Value = "0", Text="客户跟踪信息"},
                    new SelectListItem(){Value = "1", Text="承运商"}
                };
            }
        }

        public CRMTrackInfo Convert()
        {
            CRMTrackInfo crmInfo = new CRMTrackInfo()
            {
                ID=this.TempID,
                CRMID= this.CRMID,
                VisitPeople = this.VisitPeople,
                VisitTime = this.VisitTime.ToString(),
                VisitPlace = this.VisitPlace,
                VisitForm = this.VisitForm,
                GiftsArticles = this.GiftsArticles,
                VisitToCustomerEvaluation = this.VisitToCustomerEvaluation,
                VisitingPersonnelFeedbackVisit = this.VisitingPersonnelFeedbackVisit,
                ProjectCustomerCommunication = this.ProjectCustomerCommunication,
                CustomerSupportAndAssistance = this.CustomerSupportAndAssistance
            };
            return crmInfo;
        }

       
    }
}