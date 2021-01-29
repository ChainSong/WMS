using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.ContractManagement.Models
{
    public class ContractOperationViewModel
    {
        public Contract Contract { get; set; }

        ///下拉列表定义
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public IEnumerable<SelectListItem> BusinessList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> ContractTypeList { get; set; }
        public IEnumerable<SelectListItem> YesNOList { get; set; }

        //0 View 1 Create 2 Edit
        public int? ViewType { get; set; }



        public string[] CertificateCodes { get; set; }
        public IEnumerable<SelectListItem> AvaliableCertificates { get; set; }
        public IEnumerable<SelectListItem> SelectedCertificates { get; set; }

    }
}