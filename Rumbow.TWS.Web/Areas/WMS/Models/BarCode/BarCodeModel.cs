using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.BarCode
{
    [Serializable]
    public class BarCodeModel
    {
        public Table Config { get; set; }
        public Table Config1 { get; set; }
        public Table Config2 { get; set; }
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public BarCodeSearchCondition SearchCondition { get; set; }

        public IEnumerable<BarCodeInfo> BarCodeCollection { get; set; }
        /// <summary>
        /// 条码生成类型 Receipt入库单生成  Order出库单生成
        /// </summary>
        public IEnumerable<SelectListItem> BarCodeType { get; set; }

        public IEnumerable<SelectListItem> WarehouseID { get; set; }

        public IEnumerable<SelectListItem> CustomerID { get; set; }
        /// <summary>
        /// Receipt入库单   Order出库单
        /// </summary>
        public IEnumerable<SelectListItem> Type { get; set; }
        /// <summary>
        /// 状态  0有效、1无效
        /// </summary>
        public IEnumerable<SelectListItem> Status { get; set; }
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
        public IEnumerable<SelectListItem> WarehouseList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("WarehouseList");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }

        }

        public IEnumerable<SelectListItem> CustomerList;
    }
}