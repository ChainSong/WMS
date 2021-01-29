using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Receipt;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Models.ASNManagement
{
    public class ASNNewBoxLabelViewModel
    {//NIKE退货仓-
        /// <summary>
        /// 项目
        /// </summary>
        public long ProjectRoleID { get; set; }

        /// <summary>
        /// 客户(货主)编号
        /// </summary>
        public string CustomerIDs { get; set; }

        /// <summary>
        /// 客户下拉列表
        /// </summary>
        public IEnumerable<SelectListItem> CustomerList { get; set; }

        /// <summary>
        /// 仓库下拉列表
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseList { get; set; }

        public ASNNewBoxLabelSearchCondition SearchCondition { get; set; }
        public IEnumerable<ASNNewBoxLabel> ASNNewBoxLabelList { get; set; }
        public IEnumerable<SelectListItem> ProductLevel
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }
                return st;
            }
        }
    }
}