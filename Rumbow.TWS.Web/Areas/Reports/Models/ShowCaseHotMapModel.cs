using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.MessageContracts.Reports;

namespace Runbow.TWS.Web.Areas.Reports.Models
{
    public class ShowCaseHotMapModel
    {
        public ShowCaseHotMapResponse Response { get; set; }
        public ShowCaseHotMapRequest Request { get; set; }
        public IEnumerable<SelectListItem> Customers
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "2", Text = "Nike" ,Selected=true} ,
                    new SelectListItem() { Value = "1", Text = "Adidas" } ,
                   
                };

            }
        }
        public IEnumerable<SelectListItem> HotMap
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "单量" } ,
                    new SelectListItem() { Value = "1", Text = "货量" } ,
                };

            }
        }
    }
}