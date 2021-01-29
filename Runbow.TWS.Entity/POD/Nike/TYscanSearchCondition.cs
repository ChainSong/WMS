using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 天翼扫描记录查询结果Model
    /// </summary>
    public class TYscanSearchCondition
    {
        public long ID { get; set; }
        public long PODID { get; set; }
        /// <summary>
        /// 客户运单号
        /// </summary>
        public string CustomerOrderNumber { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        public string Str5 { get; set; }

        /// <summary>
        /// 运单状态
        /// </summary>
        public string PODStateName { get; set; }

        /// <summary>
        /// 起运城市     
        /// </summary>
        public string StartCityName { get; set; }

        /// <summary>
        /// 目的城市 
        /// </summary>
        public string EndCityName { get; set; }

        /// <summary>
        /// 运单类型 
        /// </summary>
        public string PODTypeName { get; set; }

        /// <summary>
        /// 发货日期 
        /// </summary>
        public string ActualDeliveryDate { get; set; }

        /// <summary>
        /// 是否异常(或同步)
        /// </summary>
        public int? type { get; set; }

        //订单日期
        public DateTime? StatCreateTime { get; set; }

        public DateTime? EndCreateTime { get; set; }


        //发货日期
        //public DateTime? StatActualDeliveryDate { get; set; }
        //public DateTime? EndActualDeliveryDate { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Str42 { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string Str7 { get; set; }

        #region MyRegion
        //public long ID { get; set; }
        //public long PODID { get; set; }
        ///// <summary>
        ///// 客户运单号
        ///// </summary>
        //public string OrderNumber { get; set; }

        ///// <summary>
        ///// 承运商
        ///// </summary>
        //public string Str5 { get; set; }

        ///// <summary>
        ///// 运单状态
        ///// </summary>
        //public string Str14 { get; set; }

        ///// <summary>
        ///// 起运城市     
        ///// </summary>
        //public string Str10 { get; set; }

        ///// <summary>
        ///// 目的城市 
        ///// </summary>
        //public string Str12 { get; set; }

        ///// <summary>
        ///// 运单类型 
        ///// </summary>
        //public string Str32 { get; set; }

        ///// <summary>
        ///// 发货日期 
        ///// </summary>
        //public string Str8 { get; set; }

        ///// <summary>
        ///// 是否异常(或同步)
        ///// </summary>
        //public int? type { get; set; }

        ////订单日期
        //public DateTime? StatCreateTime { get; set; }

        //public DateTime? EndCreateTime { get; set; } 
        #endregion
    }
}
