using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class CustomerListViewModel
    {
        public long ProjecctID { get; set; }

        public long ID { set; get; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "公司代码")]
        public string Code { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "货主名称")]
        public string Name { get; set; }

        [Display(Name = "全称")]
        public string Description { get; set; }

        [Display(Name = "类型")]
        public int StoreType { get; set; }

        [Display(Name = "客户类型")]
        public int Types { get; set; }

        [Display(Name = "状态")]
        public int StoreStatus { get; set; }

        [Display(Name = "信用额度")]
        public string CreditLine { get; set; }

        [Display(Name = "省份城市")]
        public string ProvinceCity { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "创建日期")]
        public bool CreateDate { get; set; }

        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Display(Name = "法人代表")]
        public string LawPerson { get; set; }

        [Display(Name = "邮政编码")]
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

        [Display(Name = "是否有效")]
        public bool State { get; set; }
        public string Str1 { get; set; }
        public string Str2 { get; set; }
        public string Str3 { get; set; }
         
        public IEnumerable<Customer> Customer { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public Response<Customer> ResponseCustomer { get; set; }
        public Customer Convert()
        {
            Customer customer = new Customer();
            customer.ID = this.ID;
            customer.Code = this.Code;
            customer.Name = this.Name;
            customer.Description = this.Description;
            customer.StoreType = this.StoreType;
            customer.Types = this.Types;
            customer.StoreStatus = this.StoreStatus;
            customer.CreditLine = this.CreditLine;
            customer.ProvinceCity = this.ProvinceCity;
            customer.Remark = this.Remark;
            customer.Email = this.Email;
            customer.LawPerson = this.LawPerson;
            customer.PostCode = this.PostCode;
            customer.Address1 = this.Address1;
            customer.Address2 = this.Address2;
            customer.Bank = this.Bank;
            customer.Account = this.Account;
            customer.TaxID = this.TaxID;
            customer.InvoiceTitle = this.InvoiceTitle;
            customer.Contactor1 = this.Contactor1;
            customer.Title1 = this.Title1;
            customer.Phone1 = this.Phone1;
            customer.Fax1 = this.Fax1;
            customer.Contactor2 = this.Contactor2;
            customer.Title2 = this.Title2;
            customer.Phone2 = this.Phone2;
            customer.Fax2 = this.Fax2;
            customer.WebSite = this.WebSite;
            customer.RegistAdd = this.RegistAdd;
            customer.State = this.State;
            return customer;
        }
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
        public IEnumerable<SelectListItem> Type
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){Value = "99", Text="=请选择=",Selected=true},
                    new SelectListItem(){Value = "1", Text="运输"},
                    new SelectListItem(){Value = "2", Text="仓储"},
                    new SelectListItem(){Value = "3", Text="混合"}
                  
                };
            }
        }
    }
    
}