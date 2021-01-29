using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class QueryOrOperatePodViewModel
    {
        public bool IsForQuery { get; set; }
        public bool IsDaoChu { get; set; }
        public PodDistribution SearchCondition { get; set; }
        public IEnumerable<PodToDistribution> PodCollection { get; set; }
        public bool IsChaXun { get; set; }
        public int PageIndex { get; set; }

        public int PageCount { get; set; }
        public IEnumerable<SelectListItem> IsDistributionOrSettlement
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "==请选择",Selected=false },
                    new SelectListItem() { Value = "1", Text = "N" ,Selected=true}, 
                    new SelectListItem() { Value = "2", Text = "Y",Selected=false } 
                };
            }
            set { 

            }
        }
        public IEnumerable<SelectListItem> CarModels
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "==请选择" },
                    new SelectListItem() { Value = "1", Text = "面包车" }, 
                    new SelectListItem() { Value = "2", Text = "4.2" },
                    new SelectListItem() { Value = "3", Text = "7.6" }
                };
            }
        }
        public string ReturnClientMessage { get; set; }
        public string SelectedIDs { get; set; }
        
    }
}