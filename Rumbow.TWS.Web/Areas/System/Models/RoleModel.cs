using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class RoleModel
    {
        public Response<RoleRequest> ResponseGetRoleInfoRequest { get; set; }
        public IEnumerable<Role> IEnumerableRole { get; set; }


        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public string Message { get; set; }
        //public IEnumerable<Role> IEnumerableRoleInfo { get; set; }
        public Role Role { get; set; }
        [Display(Name = "序号")]
        public long ID { get; set; }

       // [Required(ErrorMessage = "必填")]
        [Display(Name = "角色名称")]
        public string Name { get; set; }

        [Display(Name = "角色描述")]
        public string Description { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "是否启用")]
        public bool Satate { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateDate { get; set; }

     
    }
}