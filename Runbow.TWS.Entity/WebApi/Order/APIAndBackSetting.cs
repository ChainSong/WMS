using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class APIAndBackSetting
    { 
        /// <summary>
        /// 用户ID
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [EntityPropertyExtension("UserName", "UserName")]
        public string UserName { get; set; }

        ///// <summary>
        ///// 用户密码
        ///// </summary>
        //public string PassWord { get; set; }

        /// <summary>
        /// 用户key
        /// </summary>
        [EntityPropertyExtension("APPKey", "APPKey")]
        public string APPKey { get; set; }

        /// <summary>
        /// 用户密钥
        /// </summary>
        [EntityPropertyExtension("APPSecret", "APPSecret")]
        public string APPSecret { get; set; }

        /// <summary>
        /// 用户口令
        /// </summary>
        [EntityPropertyExtension("APPToken", "APPToken")]
        public string APPToken { get; set; }

     

        /// <summary>
        /// 用户状态 1为可用  0为不可用
        /// </summary>
        [EntityPropertyExtension("UserStatus", "UserStatus")]
        public int UserStatus { get; set; }


        /// <summary>
        /// APIType 类型 (每个api方法只查询自己type的配置信息)
        /// </summary>
        [EntityPropertyExtension("APIType", "APIType")]
        public string APIType { get; set; }


        /// <summary>
        /// API订单类型 入库（PO ） 出库 （SO）
        /// </summary>
        [EntityPropertyExtension("OrderType", "OrderType")]
        public string OrderType { get; set; }

        /// <summary>
        /// 反馈状态类型 出库的 已分配  已捡货 已出库等 ，入库的到货 、上架 等
        /// </summary>
        [EntityPropertyExtension("StatusType", "StatusType")]
        public string StatusType { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [EntityPropertyExtension("DisplayName", "DisplayName")]
        public string DisplayName { get; set; }
        ///// <summary>
        ///// 反馈状态类型 出库的 已分配  已捡货 已出库等 ，入库的到货 、上架 等
        ///// </summary>
        //[EntityPropertyExtension("StatusType", "StatusType")]
        //public string StatusType { get; set; }


        /// <summary>
        /// 反馈数据的API地址
        /// </summary>
        [EntityPropertyExtension("CallBackURL", "CallBackURL")]
        public string CallBackURL { get; set; }

        /// <summary>
        /// 接口传输方式  Post  Get  Webservices 等
        /// </summary>
        [EntityPropertyExtension("HttpType", "HttpType")]
        public string HttpType { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }


        /// <summary>
        /// 客户名称
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }


        /// <summary>
        /// 仓库ID
        /// </summary>
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public string WarehouseID { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }



        /// <summary>
        /// 备注
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public string CreateUser { get; set; }

        /// <summary>
        /// 备用字段1（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        /// <summary>
        /// 备用字段2（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        /// <summary>
        /// 备用字段3（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        /// <summary>
        /// 备用字段4（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }

        /// <summary>
        /// 备用字段5（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }

        /// <summary>
        /// 备用字段1（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("Int1", "Int1")]
        public int Int1 { get; set; }


        /// <summary>
        /// 备用字段2（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("Int2", "Int2")]
        public int Int2 { get; set; }

        ///// <summary>
        ///// 备用字段2（根据需求自定义）
        ///// </summary>
        //[EntityPropertyExtension("Long2", "Long2")]
        //public int Long2 { get; set; }

        /// <summary>
        /// 备用字段1（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("Long1", "Long1")]
        public long Long1 { get; set; }

        /// <summary>
        /// 备用字段1（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime DateTime1 { get; set; }


        /// <summary>
        /// 备用字段2（根据需求自定义）
        /// </summary>
        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime DateTime2 { get; set; }
    }
}
