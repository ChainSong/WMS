using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Runbow.TWS.Entity.System;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class ProjectModel
    {
        [Display(Name = "公司名称")]
        public long ID { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "公司名称")]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "必填")]
        [Display(Name = "公司编码")]
        public string Code { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "公司描述")]
        public string Description { get; set; }

        [Display(Name = "是否启用")]
        public bool State { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateDate { get; set; }

        public ProjectModel()
        {
        }

        public ProjectModel(Project project)
        {
            this.ID = project.ID;
            this.Code = project.Code;
            this.Name = project.Name;
            this.Description = project.Description;
            this.CreateDate = project.CreateDate;
            this.State = project.State;
        }
    }
}