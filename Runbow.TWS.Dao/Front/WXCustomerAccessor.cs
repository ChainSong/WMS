using System.Data.SqlClient;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;

namespace Runbow.TWS.Dao
{
    public class WXCustomerAccessor : BaseAccessor
    {
        /// <summary>
        /// 查询微信注册用户
        /// </summary>
        public IEnumerable<WXCustomer> GetQueryWXCustomer(WXCustomerSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenQueryWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_QueryWXCustomer", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<WXCustomer>();
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        private string GenQueryWhere(WXCustomerSearchCondition wxCustomer)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(wxCustomer.RealName))
            {
                sb.Append(" and RealName like '%" + wxCustomer.RealName + "%' ");
            }
            if (!string.IsNullOrEmpty(wxCustomer.Phone))
            {
                sb.Append(" and Phone like '%" + wxCustomer.Phone + "%' ");
            }

            if (!string.IsNullOrEmpty(wxCustomer.UnitName))
            {
                sb.Append(" and unitName like '%" + wxCustomer.UnitName + "%' ");
            }

            if (wxCustomer.StatCreateTime.HasValue)
            {
                sb.Append(" and CreateTime>='" + wxCustomer.StatCreateTime.Value.ToString("yyyy-MM-dd") + "') ");
            }

            if (wxCustomer.EndCreateTime.HasValue)
            {
                sb.Append(" and CreateTime<='" + wxCustomer.EndCreateTime.Value.ToString("yyyy-MM-dd") + "')");

            }

            if (wxCustomer.Status.HasValue)
            {
                if (wxCustomer.Status == 1)
                {
                    sb.Append(" and [status] =1 ");
                }
                else
                {
                    sb.Append(" and [status] =0 ");
                }
            }
   
            return sb.ToString();
        }


        /// <summary>
        /// 微信用户审核
        /// </summary>
        public int UploadWXCustomer(long podID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, podID, ParameterDirection.Input),
                new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output),
            };

            base.ExecuteNoQuery("Proc_UploadWXCustomerById", dbParams);

            return dbParams[1].Value.ObjectToInt32();
        }

        /// <summary>
        /// 查询AccessToken
        /// </summary>
        public WXAccessToken GetQueryWXAccessToken()
        {
            DataTable dt = this.ExecuteDataSetBySqlString("SELECT TOP 1 Access_token,Expires_in FROM WXAccessToken order by expires_in desc").Tables[0];

            return dt.ConvertToEntity<WXAccessToken>();
        }

        /// <summary>
        /// 更新最新AccessToken
        /// </summary>
        public void UploadWXAccessToken(WXAccessToken wx)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Access_token", DbType.String, wx.Access_token, ParameterDirection.Input),
                new DbParam("@Expires_in", DbType.String, wx.Expires_in.ToString(), ParameterDirection.Input),
            };

            base.ExecuteNoQueryBySqlString("update WXAccessToken set Access_token=@Access_token,Expires_in=@Expires_in", dbParams);
            //return dbParams[1].Value.ObjectToInt32();
        }


        /// <summary>
        /// 查询未生成二维码的PODID
        /// </summary>
        public IEnumerable<WXPODBarCode> GetPodBarCode(string podIds)
        {
            DataTable dt = this.ExecuteDataSetBySqlString("select ID AS 'PODID',CustomerOrderNumber from POD WHERE ID IN(" + podIds.TrimEnd(',') + ") and ID not in(select PODID from WXPODBarCode)").Tables[0];

            return dt.ConvertToEntityCollection<WXPODBarCode>();
        }

        /// <summary>
        /// 查询最大TicketID
        /// </summary>
        public WXPODBarCode GetWXTicketID()
        {
            DataTable dt = this.ExecuteDataSetBySqlString("SELECT MAX(ticketID) AS 'TicketID' FROM dbo.WXPODBarCode").Tables[0];

            return dt.ConvertToEntity<WXPODBarCode>();
        }


        /// <summary>
        /// 新增微信二维码配置表
        /// </summary>
        public void AddWXPODBarCode(WXPODBarCode pb)
        {
            string sql = "INSERT INTO WXPODBarCode(ticketID,PODID,ticketkey)  VALUES(@ticketID,@PODID,@ticketkey)";

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ticketID", DbType.Int64, pb.TicketID, ParameterDirection.Input),
                new DbParam("@PODID", DbType.Int64, pb.PODID, ParameterDirection.Input),
                new DbParam("@ticketkey", DbType.String, pb.ticketkey, ParameterDirection.Input),
            };

            base.ExecuteNoQueryBySqlString(sql, dbParams);
        }
    }
}
