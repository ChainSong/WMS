using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.CRM.Models
{
    public class CrmManageViewModel 
    {
        public Response<GetCRMInfoRequest> ResponseGetCRMInfoRequest { get; set; }
        public IEnumerable<CRMInfo> IEnumerableCRMInfo { get; set; }
        public int? TypeID { get; set; }
        public CRMInfo CRMInfo { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public string Message { get; set; }
        public string crmtype { get; set; }
        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string CustomerName { get; set; }


        [MaxLength(50)]
        [Display(Name = "项目名称")]
        public string ProjectName { get; set; }



        [MaxLength(50)]
        [Display(Name = "所在城市")]
        public string City { get; set; }



        [MaxLength(50)]
        [Display(Name = "工作单位")]
        public string WorkUnit { get; set; }
    }
}