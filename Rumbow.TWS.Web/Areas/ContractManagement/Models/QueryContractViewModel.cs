using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.ContractManagement.Models
{
    public class QueryContractViewModel
    {
        public IEnumerable<Contract> ContractCollection { get; set; }

        public ContractSearchCondition SearchCondition { get; set; }

        //0 全显示 1 显示即将过期 2 显示已过期
        public int? SearchType1 { get; set; }

        //0全显示 1 显示未过期 2 显示已过期
        public int? SearchType2 { get; set; }


        public bool ShowEditButton { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        ///下拉列表定义
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public IEnumerable<SelectListItem> BusinessList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> ContractTypeList { get; set; }
        public IEnumerable<SelectListItem> YesNOList { get; set; }

    }
}