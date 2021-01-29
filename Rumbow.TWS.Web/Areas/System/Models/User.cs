using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class User
    {
        public long ID { set; get; }

        [Required(ErrorMessage = "必填")]
        [MaxLength(50)]
        [Display(Name = "登陆名称")]
        public string Name { get; set; }

        [MaxLength(50)]
        [Display(Name = "显示名称")]
        public string DisplayName { get; set; }

        [MaxLength(50)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "状态")]
        public bool State { get; set; }

        [Display(Name = "性别")]
        public char Sex { get; set; }

        [Display(Name = "电话")]
        public string Tel { get; set; }

        [MaxLength(50)]
        [Display(Name = "移动电话")]
        public string Mobile { get; set; }

        [MaxLength(50)]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [Display(Name = "用户类型")]
        public int UserType { get; set; }

        [MaxLength(50)]
        [Display(Name = "管辖区域")]
        public string RuleArea { get; set; }

        [MaxLength(50)]
        public string RuleAreaName { get; set; }

        public long? CustomerID { get; set; }

        public long? ShipperID { get; set; }

        public long ProjectRoleID { get; set; }

        public DateTime CreateDate { get; set; }

        public User(Runbow.TWS.Entity.User user)
        {
            this.ID = user.ID;
            this.Name = user.Name;
            this.DisplayName = user.DisplayName;
            this.Password = user.Password;
            this.State = user.State;
            this.Sex = user.Sex;
            this.Tel = user.Tel;
            this.Mobile = user.Mobile;
            this.Email = user.Email;
            this.CreateDate = user.CreateDate;
            this.UserType = user.UserType;
            this.RuleArea = user.RuleArea;
            this.RuleAreaName = user.RuleAreaName;
            this.CustomerID = user.UserType == 0 ? user.CustomerOrShipperID : 0;
            this.ShipperID = user.UserType == 1 ? user.CustomerOrShipperID : 0;
            this.ProjectRoleID = user.ProjectRoleID;
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

        public User()
        {
        }
    }
}