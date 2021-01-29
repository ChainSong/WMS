using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class QuotedPriceViewModel
    {
        public long ID { get; set; }

        public long ProjectID { get; set; }

        public string ProjectName { get; set; }

        public int Target { get; set; }

        [Required(ErrorMessage = "必填")]
        public long TargetID { get; set; }

        public string TargetName { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "段位名称")]
        public long SegmentDetailID { get; set; }

        public long StartVal { get; set; }

        public long EndVal { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "线路")]
        public string TransportationLine { get; set; }

        public long TransportationLineID { get; set; }

        [Display(Name = "起始城市")]
        public long StartCityID { get; set; }

        public string StartCityName { get; set; }

        [Display(Name = "目的城市")]
        public long EndCityID { get; set; }

        public string EndCityName { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "运输方式")]
        public long ShipperTypeID { get; set; }

        public string ShipperTypeName { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "运单类型")]
        public long PodTypeID { get; set; }

        public string PodTypeName { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "整车/零担")]
        public long TplOrTtlID { get; set; }

        public string TplOrTtlName { get; set; }

        [Display(Name = "点费")]
        public decimal? Point { get; set; }

        [Display(Name = "最低收费")]
        public decimal? MinPrice { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "开始日期")]
        public DateTime EffectiveStartTime { get; set; }

        [Display(Name = "截止日期")]
        public DateTime? EffectiveEndTime { get; set; }

        public bool State { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        public string Str1 { get; set; }

        public string Str2 { get; set; }

        public string Str3 { get; set; }

        public string Creator { get; set; }

        public DateTime? CreateTime { get; set; }

        [Display(Name = "关联客户")]
        public long RelatedCustomerID { get; set; }

        public string RelatedCustomerName { get; set; }

        [Display(Name = "空放费")]
        public decimal? EmptyCarryPrice { get; set; }

        public decimal? Decimal1 { get; set; }

        public decimal? Decimal2 { get; set; }

        public string SettedConfigs { get; set; }

        public IEnumerable<QuotedPriceDetail> QuotedPriceDetails { get; set; }

        public IEnumerable<SelectListItem> CustomerOrShippers { get; set; }

        public IEnumerable<SelectListItem> TransportationLines { get; set; }

        public IEnumerable<SelectListItem> ShipperTypes { get; set; }

        public IEnumerable<SelectListItem> PodTypes { get; set; }

        public IEnumerable<SelectListItem> TplOrTtl { get; set; }

        public IEnumerable<SelectListItem> RelatedCustomers { get; set; }
    }

    public class QuotedPriceDetail
    {
        public long SegmentDetailID { get; set; }

        public float StartVal { get; set; }

        public float EndVal { get; set; }

        public decimal Price { get; set; }
    }
}