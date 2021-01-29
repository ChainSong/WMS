using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodTrackAndReplyDocumentManageViewModel
    {
        public string SystemNumber { get; set; }

        public string CustomerOrderNumber { get; set; }

        public long? ShipperID { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        public DateTime? EndActualDeliveryDate { get; set; }

        public IEnumerable<PodAll> PodAllCollection { get; set; }

        public string ReturnClientMessage { get; set; }

        public bool IsInnerUser { get; set; }

        public int MinPodState { get; set; }

        public IEnumerable<SelectListItem> GoodsStatus
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "在途", Text = "在途" }, 
                    new SelectListItem() { Value = "干线", Text = "干线" }, 
                    new SelectListItem() { Value = "目的站", Text = "目的站" }, 
                    new SelectListItem() { Value = "配送中", Text = "配送中" }, 
                    new SelectListItem() { Value = "已送达", Text = "已送达" } 
                };

            }
        }


        public IEnumerable<SelectListItem> CausesOFDelaysType
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "请选择" ,Selected=true }, 
                    new SelectListItem() { Value = "仓库导致", Text = "仓库导致" }, 
                    new SelectListItem() { Value = "物流导致", Text = "物流导致" }, 
                    new SelectListItem() { Value = "收货人导致", Text = "收货人导致" }, 
                    new SelectListItem() { Value = "喜利得导致", Text = "喜利得导致" }, 
                    new SelectListItem() { Value = "不可抗力", Text = "不可抗力" },
                    new SelectListItem() { Value = "太平仓库导致", Text = "太平仓库导致" } 
                };

            }
        }

        public IEnumerable<SelectListItem> ArrivalInNormal
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "请选择" }, 
                    new SelectListItem() { Value = "Y", Text = "Y" }, 
                    new SelectListItem() { Value = "N", Text = "N" } 
                };
            }
        }

       
    }
}