using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Dao;
using System.Data;
using Runbow.TWS.Entity.WMS.WXApp;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class WXAppService : BaseService
    {
        private WXAppAccessor accessor = new WXAppAccessor();
        public DataTable GetDataTable()
        {
            return accessor.GetDataTable();
        }

        public string GetSoldTrades(List<SoldTrade> soldtrade, List<SoldTradeOrder> soldtradeDetail)
        {
            Response<string> response = new Response<string>();
            try
            {
                WXAppAccessor accessor = new WXAppAccessor();
                response.Result = accessor.GetSoldTrades(soldtrade, soldtradeDetail);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = Common.ErrorCode.Technical;
            }

            return response.Result;
        }



        public DataSet AddSoldProductOrDetail(List<Product> ps, List<ProductSku> pds)
        {
            return accessor.AddSoldProductOrDetail(ps, pds);
        }

        public string AddSoldRefundDetail(List<RefundDetail> rds)
        {
            string message = string.Empty;
            try
            {
                message = accessor.AddSoldRefundDetail(rds);
                if (message.Contains("添加成功"))
                {

                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

    }
}
