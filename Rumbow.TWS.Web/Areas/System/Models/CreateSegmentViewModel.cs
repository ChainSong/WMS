using System.ComponentModel.DataAnnotations;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class CreateSegmentViewModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "必填")]
        [MaxLength(50)]
        [Display(Name = "段位名称")]
        public string Name { get; set; }

        [MaxLength(500)]
        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "是否可用")]
        public bool State { get; set; }

        public string Str1 { get; set; }

        public string Str2 { get; set; }

        public string Str3 { get; set; }
    }
}