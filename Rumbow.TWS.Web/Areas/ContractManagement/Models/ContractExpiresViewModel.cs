using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
//Addd some comment;
namespace Runbow.TWS.Web.Areas.ContractManagement.Models
{
    public class ContractExpiresViewModel
    {
        public IEnumerable<Contract> ContractCollection { get; set; }

        //0 一个月后到期  1 已经到期
        public int ContractExpireType { get; set; }

        //(当前时间+30 > 合同到期日)&&(当前时间<= 合同到期日），则符合一个月到期
        // 当前时间 > 合同到期日 ， 则符合已到期
        public IEnumerable<SelectListItem> ContractExpireTypes
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "一个月内到期" },
                    new SelectListItem() { Value = "1", Text = "已到期" }
                };
            }
        }
    }
}