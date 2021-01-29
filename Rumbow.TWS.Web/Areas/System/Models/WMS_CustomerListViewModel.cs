using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.MessageContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class WMS_CustomerListViewModel
    {
       
        [Required(ErrorMessage = "必填")]
        [Display(Name = "货主代码")]
        public string StorerKey { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Display(Name = "状态")]
        public string Status { get; set; }

        [Display(Name = "公司名字")]
        public string Company { get; set; }

        [Display(Name = "入货天数")]
        public string ReceiptPrefix { get; set; }

        [Display(Name = "到货天数")]
        public string OrderPrefix { get; set; }

        [Display(Name = "公司代码")]
        public string CompanyCode { get; set; }

        [Display(Name = "类型")]
        public string Type { get; set; }

        [Display(Name = "地址1")]
        public string AddressLine1 { get; set; }


        [Display(Name = "地址2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "地址3")]
        public string AddressLine3 { get; set; }

        [Display(Name = "地址4")]
        public string AddressLine4 { get; set; }

        [Display(Name = "城市")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "邮编号")]
        public string PostCode { get; set; }

        [Display(Name = "国家")]
        public string Country { get; set; }

        [Display(Name = "国家代码")]
        public string CountryCode { get; set; }

        [Display(Name = "联系人1")]
        public string Contact1 { get; set; }

        [Display(Name = "联系人2")]
        public string Contact2 { get; set; }

        [Display(Name = "联系号码1")]
        public string PhoneNum1 { get; set; }

        [Display(Name = "联系号码2")]
        public string PhoneNum2 { get; set; }


        [Display(Name = "传真号码1")]
        public string FaxNum1 { get; set; }

        [Display(Name = "传真号码2")]
        public string FaxNum2 { get; set; }


        [Display(Name = "邮箱1")]
        public string Email1 { get; set; }

        [Display(Name = "邮箱2")]
        public string Email2 { get; set; }

        public long? CustomerID { get; set; }
        public string UserDef1 { get; set; }
        public string UserDef2 { get; set; }
        public string UserDef3 { get; set; }
        public string UserDef4 { get; set; }
        public string UserDef5 { get; set; }
        public string UserDef6 { get; set; }
        public string UserDef7 { get; set; }
        public string UserDef8 { get; set; }
        public string UserDef9 { get; set; }

        public IEnumerable<WMS_Customer> Customer { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public Response<WMS_Customer> ResponseCustomer { get; set; }
        public WMS_Customer Convert()
        {
            WMS_Customer customer = new WMS_Customer();
            customer.StorerKey = this.StorerKey;
            customer.Active = this.Active;
            customer.Status = this.Status;
            customer.Company = this.Company;
            customer.ReceiptPrefix = this.ReceiptPrefix;
            customer.OrderPrefix = this.OrderPrefix;
            customer.CompanyCode = this.CompanyCode;
            customer.Type = this.Type;
            customer.AddressLine1 = this.AddressLine1;
            customer.AddressLine2 = this.AddressLine2;
            customer.AddressLine3 = this.AddressLine3;
            customer.AddressLine4 = this.AddressLine4;
            customer.PostCode = this.PostCode;
            customer.City = this.City;
            customer.State = this.State;
            customer.Country = this.Country;
            customer.CountryCode = this.CountryCode;
            customer.Contact1 = this.Contact1;
            customer.Contact2 = this.Contact2;
            customer.PhoneNum1 = this.PhoneNum1;
            customer.PhoneNum2 = this.PhoneNum2;
            customer.FaxNum1 = this.FaxNum1;
            customer.FaxNum2 = this.FaxNum2;
            customer.Email1 = this.Email1;
            customer.Email2 = this.Email2;
            customer.CustomerID = this.CustomerID;
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
        public IEnumerable<SelectListItem> StorerID
        {
            get;
            set;
            //get
            //{
            //    IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_SKUClass");
            //    List<SelectListItem> st = new List<SelectListItem>();
            //    foreach (WMSConfig w in wms)
            //    {
            //        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //    }

            //    return st;
            //}
        }
        public OrderSearchCondition SearchCondition { get; set; }
        public ASNSearchCondition ASNCondition { get; set; }
    }
}