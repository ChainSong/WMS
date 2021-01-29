using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class RoleManageListViewModel
    {
        public IEnumerable<Role> IEnumerableRole { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public string Message { get; set; }

        [Display(Name = "序号")]
        public long ID { get; set; }

        
        [Display(Name = "角色名称")]
        public string Name { get; set; }

        [Display(Name = "角色描述")]
        public string Description { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "是否启用")]
        public bool Satate { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateDate { get; set; }

        public IEnumerable<SelectListItem> States
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "True", Text="可用" },
                    new SelectListItem(){Value = "False", Text="禁用"}
                };
            }
        }

        public IEnumerable<RoleModel> RoleModel { get; set; }

        //public int PageIndex { get; set; }
        public Role Role { get; set; }
        //public int PageCount { get; set; }
    }
}   