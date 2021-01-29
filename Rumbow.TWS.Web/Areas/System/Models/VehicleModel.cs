//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;

//namespace Runbow.TWS.Web.Areas.System.Models
//{
//    public class VehicleModel
//    {
//        [Required(ErrorMessage = "必填")]
//        [MaxLength(50)]
//        [Display(Name = "车牌号")]
//        public string PlateNumber { get; set; }

//        [MaxLength(50)]
//        [Display(Name = "司机")]
//        public string Pilot { get; set; }

//        [MaxLength(50)]
//        [Display(Name = "工号")]
//        public string JobNumber { get; set; }

//        [MaxLength(50)]
//        [Display(Name = "联系电话")]
//        public string Contract { get; set; }

//        [MaxLength(50)]
//        [Display(Name = "体积容量")]
//        public string Str1 { get; set; }

//        [MaxLength(50)]
//        [Display(Name = "重量容量")]
//        public string Str2 { get; set; }

//        [MaxLength(50)]
//        [Display(Name = "票数容量")]
//        public string Str3 { get; set; }

//        [MaxLength(50)]
//        [Display(Name = "车辆类型")]
//        public string Str4 { get; set; }

//        [MaxLength(500)]
//        [Display(Name = "备注")]
//        public string Str7 { get; set; }
//    }
//}