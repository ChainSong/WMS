using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NikeGetB2COrder
{
    [XmlRoot("OrderConfig")]
    public class OrderConfig
    {
        [XmlElement("CustomerStore")]
        public List<CustomerStore> customerStores { get; set; }
        [XmlElement("OrderTypeConfig")]
        public List<OrderTypeConfig> orderTypes { get; set; }
    }




    /// <summary>
    /// 客户和门店对应关系
    /// </summary>
    public class CustomerStore
    {
        [XmlElement("CustomerID")]
        public long CustomerID { get; set; }
        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }
        [XmlElement("WarehouseID")]
        public long WarehouseID { get; set; }
        [XmlElement("WarehouseName")]
        public string WarehouseName { get; set; }
        [XmlElement("StoreKey")]
        public string StoreKey { get; set; }
        /// <summary>
        /// 根据门店判断数据库配置 1.上海仓，2.其他仓库（成都武汉西安）
        /// </summary>
        [XmlElement("DBType")]
        public string DBType { get; set; }
    }

    /// <summary>
    /// 订单状态对应关系
    /// </summary>
    public class OrderTypeConfig
    {
        [XmlElement("PlanType")]
        public string PlanType { get; set; }
        [XmlElement("OrderTypeCode")]
        public int OrderTypeCode { get; set; }
        [XmlElement("OrderType")]
        public string OrderType { get; set; }

    }
}
