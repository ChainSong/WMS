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
    public class WMS_CustomerModel
    {
        public int ViewType { set; get; }       //状态类型(查看或编辑)
        public int customerType { set; get; }   //栏目类型(货主或客户)

        [Required(ErrorMessage = "必填")]
        [Display(Name = "货主代码")]
        public string StorerKey { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Display(Name = "状态")]
        public string Status { get; set; }

        [Display(Name = "店铺名称")]
        public string Company { get; set; }

        [Display(Name = "入货在途天数")]
        public string  ReceiptPrefix { get; set; }

        [Display(Name = "到货在途天数")]
        public string OrderPrefix { get; set; }

        [Display(Name = "SAP代码")]
        public string CompanyCode { get; set; }

        [Display(Name = "类型")]
        public string Type { get; set; }

        [Display(Name = "地址1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "地址2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "地址3")]
        public string AddressLine3{ get; set; }

        [Display(Name = "地址4")]
        public string AddressLine4 { get; set; }

        [Display(Name = "城市")]
        public string City { get; set; }

        [Display(Name = "地区")]
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

        [Display(Name = "联系电话1")]
        public string PhoneNum1 { get; set; }

        [Display(Name = "联系电话2")]
        public string PhoneNum2 { get; set; }

        [Display(Name = "传真号码1")]
        public string FaxNum1 { get; set; }

        [Display(Name = "传真号码2")]
        public string FaxNum2 { get; set; }

        [Display(Name = "邮箱1")]
        public string Email1 { get; set; }

        [Display(Name = "邮箱2")]
        public string Email2 { get; set; }


        public string UserDef1 { get; set; }
        [Display(Name="库区关联")]
        public string UserDef2 { get; set; }
        [Display(Name ="门店属性")]
        public string UserDef3 { get; set; }
        public string UserDef4 { get; set; }
        public string UserDef5 { get; set; }
        public string UserDef6 { get; set; }
        public string UserDef7 { get; set; }
        public string UserDef8 { get; set; }
        public string UserDef9 { get; set; }

        [Display(Name = "店铺简称")]
        public string UserDef10 { get; set; }
        [Display(Name = "客户")]
        public long? CustomerID { get; set; }

        /// <summary>
        /// 门店属性下拉
        /// </summary>
        public IEnumerable<SelectListItem> StorerType
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "NFS", Text="NFS"},
                    new SelectListItem(){Value = "NSO", Text="NSO"}
                };
            }
        }

        public IEnumerable<SelectListItem> StorerID
        {
            get;
            set;
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

        public IEnumerable<SelectListItem> Segments { get; set; }

        public long SelectedCustomerOrShipperSegment { get; set; }

        public IEnumerable<SelectListItem> GetTypes
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("storeType");
                List<SelectListItem> st = new List<SelectListItem>();
                st.Add(new SelectListItem() { Value = "0", Text = "==请选择==" });
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }

        public IEnumerable<SelectListItem> GetStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("storeStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                st.Add(new SelectListItem() { Value = "0", Text = "==请选择==" });
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }

        public Response<WMS_Customer> ResponseCustomer { get; set; }

        public WMS_Customer Convert()
        {
            WMS_Customer customer = new WMS_Customer();
            customer.StorerKey = this.StorerKey;
            customer.Active = this.Active;
            customer.Status = this.Status;
            customer.Company = this.Company ;
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
            customer.UserDef10 = this.UserDef10;
            customer.CompanyCode = this.CompanyCode;
            customer.UserDef2 = this.UserDef2;
            customer.UserDef3 = this.UserDef3;//门店属性
            return customer;
        }

        public WMS_Customer ConvertDesc(WMS_Customer customer)
        {
            this.CustomerID = customer.CustomerID;
            this.StorerKey = customer.StorerKey;
            this.Active = customer.Active;
            this.Status = customer.Status;
            this.Company = customer.Company;
            this.ReceiptPrefix = customer.ReceiptPrefix;
            this.OrderPrefix = customer.OrderPrefix;
            this.CompanyCode = customer.CompanyCode;
            this.Type = customer.Type;
            this.AddressLine1 = customer.AddressLine1;
            this.AddressLine2 = customer.AddressLine2;
            this.AddressLine3 = customer.AddressLine3;
            this.AddressLine4 = customer.AddressLine4;
            this.PostCode = customer.PostCode;
            this.City = customer.City;
            this.State = customer.State;
            this.Country = customer.Country;
            this.CountryCode = customer.CountryCode;
            this.Contact1 = customer.Contact1;
            this.Contact2 = customer.Contact2;
            this.PhoneNum1 = customer.PhoneNum1;
            this.PhoneNum2 = customer.PhoneNum2;
            this.FaxNum1 = customer.FaxNum1;
            this.FaxNum2 = customer.FaxNum2;
            this.Email1 = customer.Email1;
            this.Email2 = customer.Email2;
            this.UserDef10 = customer.UserDef10;
            this.CompanyCode = customer.CompanyCode;
            this.UserDef2 = customer.UserDef2;
            this.UserDef3 = customer.UserDef3;
            return customer;
        }
    }
}