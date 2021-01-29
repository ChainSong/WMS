using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class Shipper
    {
        public Shipper()
        {
        }

        public long ID { set; get; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "代码")]
        public string Code { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "英文名")]
        public string EnglishName { get; set; }

        [Display(Name = "危险品资质")]
        public bool IsDangerous { get; set; }

        [Display(Name = "海关监管资质")]
        public bool IsCustoms { get; set; }

        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Display(Name = "法人代表")]
        public string LawPerson { get; set; }

        [Display(Name = "供应商")]
        public bool IsSupplier { get; set; }

        [Display(Name = "结算实体")]
        public bool IsBalance { get; set; }

        [Display(Name = "邮编")]
        public string PostCode { get; set; }

        [Display(Name = "地址1")]
        public string Address1 { get; set; }

        [Display(Name = "地址2")]
        public string Address2 { get; set; }

        [Display(Name = "开户银行")]
        public string Bank { get; set; }

        [Display(Name = "帐号")]
        public string Account { get; set; }

        [Display(Name = "税号")]
        public string TaxID { get; set; }

        [Display(Name = "发票抬头")]
        public string InvoiceTitle { get; set; }

        [Display(Name = "联系人1")]
        public string Contactor1 { get; set; }

        [Display(Name = "职位1")]
        public string Title1 { get; set; }

        [Display(Name = "电话1")]
        public string Phone1 { get; set; }

        [Display(Name = "传真1")]
        public string Fax1 { get; set; }

        [Display(Name = "联系人2")]
        public string Contactor2 { get; set; }

        [Display(Name = "职位2")]
        public string Title2 { get; set; }

        [Display(Name = "电话2")]
        public string Phone2 { get; set; }

        [Display(Name = "传真2")]
        public string Fax2 { get; set; }

        [Display(Name = "网址")]
        public string WebSite { get; set; }

        [Display(Name = "注册地址")]
        public string RegistAdd { get; set; }

        public string Comment { get; set; }

        [Display(Name = "创建者")]
        public string Creater { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "更新人员")]
        public string Updater { get; set; }

        [Display(Name = "更新时间")]
        public DateTime? UpdateTime { get; set; }

        [Display(Name = "保险公司")]
        public string InsuranceCompany { get; set; }

        [Display(Name = "保险种类")]
        public string InsuranceType { get; set; }

        [Display(Name = "保险单号")]
        public string InsuranceOrderNo { get; set; }

        [Display(Name = "保险金额")]
        public decimal? InsuranceCost { get; set; }

        [Display(Name = "保险起始日期")]
        public DateTime? InsuranceStartTime { get; set; }

        [Display(Name = "保险终止日期")]
        public DateTime? InsuranceEndTime { get; set; }

        [Display(Name = "是否有效")]
        public bool State { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "运输类型")]
        public string Str1 { get; set; }

        public string Str2 { get; set; }

        public string Str3 { get; set; }

        public bool IsEdit { get; set; }

        public string[] PostedShipperIdentifyIDs { get; set; }

        public IEnumerable<Config> AvailableShipperIdentifies { get; set; }

        public IEnumerable<Config> SelectedShipperIdentifies { get; set; }

        public Shipper(Runbow.TWS.Entity.Shipper shipper)
        {
            this.ID = shipper.ID;
            this.Code = shipper.Code;
            this.Name = shipper.Name;
            this.EnglishName = shipper.EnglishName;
            this.IsDangerous = shipper.IsDangerous ?? false;
            this.IsCustoms = shipper.IsCustoms ?? false;
            this.Email = shipper.Email;
            this.LawPerson = shipper.LawPerson;
            this.IsSupplier = shipper.IsSupplier ?? false;
            this.IsBalance = shipper.IsBalance ?? false;
            this.PostCode = shipper.PostCode;
            this.Address1 = shipper.Address1;
            this.Address2 = shipper.Address2;
            this.Bank = shipper.Bank;
            this.Account = shipper.Account;
            this.TaxID = shipper.TaxID;
            this.InvoiceTitle = shipper.InvoiceTitle;
            this.Contactor1 = shipper.Contactor1;
            this.Title1 = shipper.Title1;
            this.Phone1 = shipper.Phone1;
            this.Fax1 = shipper.Fax1;
            this.Contactor2 = shipper.Contactor2;
            this.Title2 = shipper.Title2;
            this.Phone2 = shipper.Phone2;
            this.Fax2 = shipper.Fax2;
            this.WebSite = shipper.WebSite;
            this.RegistAdd = shipper.RegistAdd;
            this.Comment = shipper.Comment;
            this.Creater = shipper.Creater;
            this.CreateTime = shipper.CreateTime;
            this.Updater = shipper.Updater;
            this.UpdateTime = shipper.UpdateTime;
            this.InsuranceCompany = shipper.InsuranceCompany;
            this.InsuranceType = shipper.InsuranceType;
            this.InsuranceOrderNo = shipper.InsuranceOrderNo;
            this.InsuranceCost = shipper.InsuranceCost;
            this.InsuranceStartTime = shipper.InsuranceStartTime;
            this.InsuranceEndTime = shipper.InsuranceEndTime;
            this.State = shipper.State;
            this.Remark = shipper.Remark;
            this.Str1 = shipper.Str1;
            this.Str2 = shipper.Str2;
            this.Str3 = shipper.Str3;
        }
    }
}