using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodTrackReportExportViewModel
    {
        [MaxLength(50)]
        [Display(Name = "目的省份")]
        public string  EndProvince { get; set; }
        public int EndProvinceID { get; set; }
        [MaxLength(50)]
        [Display(Name = "目的城市")]
        public string EndCity { get; set; }
        public int EndCityID { get; set; }
        
        [Display(Name = "开始订单日期")]
        public DateTime? BeginOrderDate { get; set; }
        
        [Display(Name = "结束订单日期")]
        public DateTime? EndOrderDate { get; set; }

       
        [Display(Name="开始服务应到日期")]
        public DateTime? BeginServiceDate { get; set; }

        [Display(Name = "结束服务应到日期")]
        public DateTime? EndServiceDate { get; set; }
        
        
        
        [Display(Name = "开始发货日期")]
        public DateTime? BeginDeliverGoodsDate { get; set; }

        [Display(Name = "结束发货日期")]
        public DateTime? EndDeliverGoodsDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "未及时出货原因")]
        public string NotDeliverGoodsRemark { get; set; }

        [MaxLength(50)]
        [Display(Name = "承运商")]
        public string ShipperName { get; set; }
        
        //[Display(Name = "下单时间")]
        public IEnumerable<SelectListItem> PlaceAnOrderTime
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "08:00-12:00", Text = "8:00-12:00" }, 
                    new SelectListItem() { Value = "12:01-16:30", Text = "12:01-04:30" },
                    new SelectListItem() { Value = "16:31-19:59", Text = "04:30-07:59" } 
                };
            }
        }
        public string PlaceAnOrderTimeValue { get; set; }

        
        //[Display(Name = "是否代收货款")]
        public IEnumerable<SelectListItem> IsOrNoTencod
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "0", Text = "N" }, 
                    new SelectListItem() { Value = "1", Text = "Y" } 
                };
            }
        }
        public string IsOrNoTencodValue { get; set; }
        
        //[Display(Name = "是否随货")]
        public IEnumerable<SelectListItem> IsOrNoWithTheGoods
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "0", Text = "N" }, 
                    new SelectListItem() { Value = "1", Text = "Y" } 
                };
            }
        }

        //[Display(Name = "是否随货")]
        public IEnumerable<SelectListItem> PodAttribution
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                   
                    new SelectListItem() { Value = "上海", Text = "上海" }, 
                    new SelectListItem() { Value = "广州", Text = "广州" },
                    new SelectListItem() { Value = "北京", Text = "北京" } 
                };
            }
        }

        public string IsOrNoWithTheGoodsValue { get; set; }
        
        [Display(Name = "运单状态")]
        public string PodStatesName { get; set; }

        [Display(Name = "运单归属")]
        public string SelectedAttribution { get; set; }

        [Display(Name = "运输类型")]




        public string ShipperTypeName { get; set; }

        public long? SelectedPodStatesID { get; set; }

        public IEnumerable<SelectListItem> PodStates { get; set; }

        public long? SelectedShipperTypeID { get; set; }

        public IEnumerable<SelectListItem> ShipperTypes { get; set; }

        public DataTable XLDTrackReport { get; set; }


        public string CustomerOrderNoAnd103 { get; set; }
        public string OrderTypeValue { get; set; }
        public IEnumerable<SelectListItem> OrderType
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "出货运单", Text = "出货" }, 
                    new SelectListItem() { Value = "调拨运单", Text = "调拨" },
                    new SelectListItem() { Value = "退货运单", Text = "退货" } 
                };
            }
        }
        public string  CustomerName { get; set; }
       

        public string NetWeight  { get; set; }
        public IEnumerable<SelectListItem> Weight
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "<20", Text = "<20" }, 
                    new SelectListItem() { Value = "20-50", Text = "20-50" },
                    new SelectListItem() { Value = "50-500", Text = "50-500" },
                     new SelectListItem() { Value = "500-1T", Text = "500-1T" },
                      new SelectListItem() { Value = "1T-2T", Text = "1T-2T" },
                      new SelectListItem() { Value = "2T-5T", Text = "2T-5T" },
                      new SelectListItem() { Value = "5T-10T", Text = "5T-10T" },
                      new SelectListItem() { Value = ">10T", Text = ">10T" }
                };
            }
        }

        public string GrossWeight  { get; set; }

        public string ModeOfTransport { get; set; }

        public string FTLOrLTLValue { get; set; }
        public IEnumerable<SelectListItem> FTLOrLTL
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "FTL", Text = "FTL" }, 
                    new SelectListItem() { Value = "LTL", Text = "LTL" },
                    
                };
            }
        }



        public string TheWarrantyIsOrNoWithTheGoodsValue { get; set; }
        public IEnumerable<SelectListItem> TheWarrantyIsOrNoWithTheGoods
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "0", Text = "N" }, 
                    new SelectListItem() { Value = "1", Text = "Y" } 
                };
            }
        }

        public DateTime? BeginServicePeriod { get; set; }
        public DateTime? EndServicePeriod { get; set; }

        public bool DeliveryStateValue { get; set; }
        public IEnumerable<SelectListItem> DeliveryState
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "已出库", Text = "已出库"}, 
                    new SelectListItem() { Value = "干线", Text = "干线"},
                     new SelectListItem() { Value = "目的站", Text = "目的站"},
                      new SelectListItem() { Value = "配送中", Text = "配送中"}, 
                      new SelectListItem() { Value = "已送达", Text = "已送达"} 
                      
                };
            }
        }

        public IEnumerable<SelectListItem> SelectedDeliveryState { get; set; }

        public string[] PostedIDs { get; set; }

        public string DelayClassifyingValue { get; set; }
        public IEnumerable<SelectListItem> DelayClassifying
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "不可抗力", Text = "不可抗力" }, 
                    new SelectListItem() { Value = "收货人导致", Text = "收货人导致" },
                     new SelectListItem() { Value = "物流导致", Text = "物流导致" },
                      new SelectListItem() { Value = "喜利得导致", Text = "喜利得导致" } 
                     
                      
                };
            }
        }



        public string IsOrNoNormalDeliveryValue { get; set; }
        public IEnumerable<SelectListItem> IsOrNoNormalDelivery
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "N", Text = "N" }, 
                    new SelectListItem() { Value = "Y", Text = "Y" } 
                };
            }
        }



        public string SalesOrdersOrNoSalesOrdersValue { get; set; }
        public IEnumerable<SelectListItem> SalesOrdersOrNoSalesOrders
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "销售单", Text = "销售单" }, 
                    new SelectListItem() { Value = "非销售单", Text = "非销售单" } 
                };
            }
        }


        public string Channel { get; set; }
        public string  Remarks { get; set; }

        public string IsOrNoUpLoadReceiptValue { get; set; }
        public IEnumerable<SelectListItem> IsOrNoUpLoadReceipt
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "0", Text = "N" }, 
                    new SelectListItem() { Value = "1", Text = "Y" } 
                };
            }
        }

        public string  SalespersonName { get; set; }

        public string IsOrNoRejectionValue { get; set; }
        public IEnumerable<SelectListItem> IsOrNoRejection
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "0", Text = "N" }, 
                    new SelectListItem() { Value = "1", Text = "Y" } 
                };
            }
        }

        public string IsOrNoComPlainValue { get; set; }
        public IEnumerable<SelectListItem> IsOrNoComPlain
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "0", Text = "N" }, 
                    new SelectListItem() { Value = "1", Text = "Y" } 
                };
            }
        }

        public string ComPlainTypeValue { get; set; }
        public IEnumerable<SelectListItem> ComPlainType
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "==请选择==" },
                    new SelectListItem() { Value = "0", Text = "N" }, 
                    new SelectListItem() { Value = "1", Text = "Y" } 
                };
            }
        }

        public string ComPlainRemarks { get; set; }

        public int UserType { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public double SumPoll { get; set; }
        public double SumGrossWeight { get; set; }
        public double SumNetWeight { get; set; }
        public string css { get; set; }
        public bool UpOrDown { get; set; }

        public string ReportNameValue { get; set; }
        public IEnumerable<SelectListItem> ReportName
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    
                    new SelectListItem() { Value = "跟踪报表", Text = "跟踪报表" }, 
                    new SelectListItem() { Value = "未返回单清单", Text = "未返回单清单" },
                    new SelectListItem() { Value = "回单率统计", Text = "回单率统计" },
                    new SelectListItem() { Value = "重量阶段统计", Text = "重量阶段统计" },
                    new SelectListItem() { Value = "KPI统计报表", Text = "KPI统计报表" },
                     new SelectListItem() { Value = "Hilti回单率", Text = "Hilti回单率" }
                };
            }
        }

    }
}