using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    [Serializable]
    public class CRMShipperSearchCondition
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///属性地区
        /// </summary>
        public string Attribution { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        public string RegisteredCapitalRange { get; set; }

        /// <summary>
        /// 年营业额
        /// </summary>
        public string AnnualTurnoverRange { get; set; }

        /// <summary>
        /// 出发地ID
        /// </summary>
        public string StartPlaceIDs { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public string StartPlaceNames { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public string EndPlaceIDs { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public string EndPlaceNames { get; set; }

        /// <summary>
        /// 覆盖范围ID
        /// </summary>
        public string CoverRegionIDs { get; set; }

        /// <summary>
        /// 覆盖范围
        /// </summary>
        public string CoverRegionNames { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public string TransportMode { get; set; }

        /// <summary>
        /// 干线车类型
        /// </summary>
        public string TrunkOfVehicleType { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// 发车频次
        /// </summary>
        public string FrequencyOfDeparture { get; set; }

        /// <summary>
        /// 干线车辆
        /// </summary>
        public string TrunkOfVehicleRange { get; set; }

        /// <summary>
        /// 提货车辆
        /// </summary>
        public string DeliveryOfVehicleRange { get; set; }

        /// <summary>
        /// 仓库面积
        /// </summary>
        public string WarehouseAreaRange { get; set; }

        /// <summary>
        /// 推荐星级
        /// </summary>
        public string Recommended { get; set; }

        /// <summary>
        /// 关键字搜索
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 合作类型
        /// </summary>
        public string PartnerShipType { get; set; }
    }
}
