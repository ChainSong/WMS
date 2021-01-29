using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.WorkOrder;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class WorkOrderToDb : SqlDataRecord
    {
        public WorkOrderToDb(WorkOrder info)
            : base(s_metadata)
        {
            SetSqlInt64(0, info.ID);
            SetSqlString(1, info.WorkOrderNumber);
            SetSqlInt64(2, info.WorkOrderType);
            SetSqlString(3, info.TypeName);
            SetSqlString(4, info.Title);
            SetSqlString(5, info.OrderContent);
            SetSqlInt64(6, info.Status);
            SetSqlString(7, info.StatusName);
            SetSqlDateTime(8, info.CreateTime);
            SetSqlString(9, info.Creator);
            SetSqlDateTime(10, info.UpdateTime);
            SetSqlString(11, info.Updator);
            SetSqlString(12, info.Remark);
            SetSqlInt64(13, info.UserID);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.Int),
            new SqlMetaData("WorkOrderNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("WorkOrderType", SqlDbType.Int),
            new SqlMetaData("TypeName", SqlDbType.NVarChar,50),
            new SqlMetaData("Title", SqlDbType.NVarChar,50),
            new SqlMetaData("OrderContent", SqlDbType.NVarChar,2000),
            new SqlMetaData("Status", SqlDbType.Int),
            new SqlMetaData("StatusName", SqlDbType.NVarChar,50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Creator", SqlDbType.NVarChar,50),
            new SqlMetaData("UpdateTime", SqlDbType.DateTime),
            new SqlMetaData("Updator", SqlDbType.NVarChar,50),
            new SqlMetaData("Remark", SqlDbType.NVarChar,500),
            new SqlMetaData("UserID", SqlDbType.Int)
        };
    }
}
