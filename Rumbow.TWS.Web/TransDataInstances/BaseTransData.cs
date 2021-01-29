using Runbow.TWS.Entity.WMS.PreOrders;
using Runbow.TWS.Web.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using Runbow.TWS.Entity;
using System.Data;
namespace Runbow.TWS.Web.TransDataInstances
{
    public class BaseTransData : ITransData
    {
         
        /// <summary>
        /// 转换类型 （全部OR部分）
        /// </summary>
        public string TransDataType { get; set; }

        /// <summary>
        /// 预出库单子
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public long CustomerID { get; set; }

        public long ProjectID { get; set; }

        public long WareHouseID { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserName { get; set; }

       
        //<summary>
        //分配存储过程
        //</summary>
        public string SqlProc { get; set; }

        /// <summary>
        /// 要转换的数据
        /// </summary>
        public DataSet Transdata { get; set; }
        /// <summary>
        /// 转换的结果
        /// </summary>
        public DataSet AfterData = null;


        public BaseTransData(string transType, long customerId, long projectId, long WareHouseID, DataSet transdata)
        {
            this.TransDataType = transType;

            this.CustomerID = customerId;
            this.Transdata = transdata;
            this.ProjectID = projectId;
            this.WareHouseID = WareHouseID;
        }
        public virtual void CustomerDefinedSettledTransData(ref string message)
        {

        }
        /// <summary>
        /// 数据转换方法
        /// </summary>
        /// <returns></returns>
        public DataSet TransData(ref string message)
        {
            this.CustomerDefinedSettledTransData(ref message);

            return AfterData;
        }
    }
}