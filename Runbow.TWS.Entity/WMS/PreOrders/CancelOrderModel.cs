using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Runbow.TWS.Entity.WMS.PreOrders
{
    public class CancelOrderModel
    {
        /// <summary>
        /// 出库单取消查询条件
        /// </summary>
        public CancelOrderSearchCondition CancelOrderSearch { get; set; }

        /// <summary>
        /// 出库单取消列表
        /// </summary>
        public IEnumerable<CancelOrderInfo> CancelOrderList { get; set; }

        //如果入库单也有取消业务，可以在这里一起做了

        /// <summary>
        /// 客户下拉
        /// </summary>
        public IEnumerable<SelectListItem> CustomerList;

        /// <summary>
        /// 仓库下拉
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseList
        {
            get;
            set;
        }

        public long ProjectRoleID { get; set; }

    }
}
