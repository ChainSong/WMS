using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using UserEntity = Runbow.TWS.Entity.User;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class UserListViewModel
    {
        [Display(Name = "登陆名称")]
        public string Name { get; set; }

        [Display(Name = "显示名称")]
        public string DisplyName { get; set; }

        [Display(Name = "状态")]
        public bool State { get; set; }

        [Display(Name = "用户类型")]
        public int UserType { get; set; }

        [Display(Name = "角色")]
        public int ProjectRoleId { get; set; }

        public IEnumerable<UserEntity> Users { get; set; }

        public IEnumerable<SelectListItem> States
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "True", Text="可用"},
                    new SelectListItem(){Value = "False", Text="禁用"}
                };
            }
        }

        public IEnumerable<SelectListItem> UserTypes
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "2", Text="内部用户"},
                    new SelectListItem(){Value = "0", Text="客户"},
                    new SelectListItem(){Value = "1", Text="承运商"}
                };
            }
        }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }
    }
}