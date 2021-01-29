using Runbow.TWS.Biz;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeGetB2COrder
{
    class Program
    {
       

        static void Main(string[] args)
        {

            #region 抓取B2C订单
            try
            {
                new OrderManage().SyncB2COrder();
            }
            catch (Exception ex)
            {
                //失败了
                List<NikeCrodOrderLog> logs = new List<NikeCrodOrderLog>()
                {
                    new NikeCrodOrderLog() {
                    OrderCode = "",
                        Type = "GetB2COrder",
                        Operation = "抓取B2C订单",
                        Remark = "抓取失败："+ex.Message.ToString(),
                        Creator = "sysService",
                        Str1 = "",
                        Str2 = "",
                        Str3 = "",
                        Str4 = "",
                        Str5 = ""
                    }
                };
                new LogOperationService().AddNikeCordOrderLog(logs);
            }
            #endregion

           
        }


    }
}