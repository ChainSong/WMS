using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.Reports;
using Runbow.TWS.MessageContracts.Reports;
using Runbow.TWS.Common;

namespace Runbow.TWS.Dao
{
    public class WMSOrderStatusAccessor : BaseAccessor
    {
        public IEnumerable<WMSOrderStatusInfo> QueryWMSOrderRange(WMSOrderStatusRequest Request)//, out int RowCount
        {
            string SqlWhere = GenQueryAttachmentSql(Request);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SqlWhere", DbType.String, SqlWhere, ParameterDirection.Input),
               // new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                //new DbParam("@EndTime", DbType.DateTime, EndTime, ParameterDirection.Input),
                //new DbParam("@ShipperID", DbType.Int64, ShipperID, ParameterDirection.Input)
            };
            //RowCount = (int)dbParams[3].Value;
            return base.ExecuteDataTable("Proc_WMSOrderStatus", dbParams).ConvertToEntityCollection<WMSOrderStatusInfo>();
        }

        private string GenQueryAttachmentSql(WMSOrderStatusRequest Request)
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(Request.Project))
            {
                sb.Append(" and Project='" + Request.Project + "'");
            }

           
            if (Request.StartCreateTime!=null)
            {
                sb.Append("and CreateTime>='" + Request.StartCreateTime + "'");
            }
            if (Request.EndCreateTime!=null)
            {
                sb.Append("and CreateTime<'" + Request.EndCreateTime + "'");

            }
            return sb.ToString();
        }
    }
}
