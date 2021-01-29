using System.Collections.Generic;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.POD;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class QueryPodViewModel
    {
        public Table Config { get; set; }

        public PodSearchCondition SearchCondition { get; set; }

        public IEnumerable<PodWithAttachment> PodCollection { get; set; }

        public IEnumerable<SelectListItem> PodStates { get; set; }

        public IEnumerable<SelectListItem> ShipperTypes { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Shippers { get; set; }

        public IEnumerable<SelectListItem> PODTypes { get; set; }

        public IEnumerable<SelectListItem> TtlOrTpls { get; set; }

        //Url,GroupID
        public string Url { get; set; }
        public string GroupID { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public bool HideActionButton { get; set; }

        public bool ShowEditRelated { get; set; }

        public bool ShowCustomerOrShipperDrop { get; set; }

        public bool IsInnerUser { get; set; }

        public long ProjectRoleID { get; set; }

        public bool IsForExport { get; set; }

        public string ExportType { get; set; }

        public bool IsUsedForSendForecast { get; set; }

        public bool IsReturnPodStatus { get; set; }
        public bool IsWenXinStatus { get; set; }

        public long? ReturnPodStatusID { get; set; }

        public IEnumerable<SelectListItem> TrueOrFalse
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "N" }, 
                    new SelectListItem() { Value = "1", Text = "Y" } 
                };
            }
        }
        public IEnumerable<SelectListItem> PODType
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "提货车辆", Text = "提货车辆" }, 
                    new SelectListItem() { Value = "干线车辆", Text = "干线车辆" },
                    new SelectListItem() { Value = "配送车辆", Text = "配送车辆"} 
                };
            }
        }
        public string ReturnClientMessage { get; set; }
    }
}