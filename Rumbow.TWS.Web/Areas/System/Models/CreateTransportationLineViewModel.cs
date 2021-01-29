using System.ComponentModel.DataAnnotations;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class CreateTransportationLineViewModel
    {
        public long ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "起始城市")]
        public long? StartCityID { get; set; }

        [Display(Name = "到达城市")]
        public long? EndCityID { get; set; }

        [MaxLength(50)]
        [Display(Name = "实际距离")]
        public string Distance { get; set; }

        public string StartCityName { get; set; }

        public string EndCityName { get; set; }

        [MaxLength(500)]
        [Display(Name = "描述")]
        public string Remark { get; set; }

        public bool State { get; set; }

        public string Str1 { get; set; }

        public string Str2 { get; set; }

        public string Str3 { get; set; }
    }
}