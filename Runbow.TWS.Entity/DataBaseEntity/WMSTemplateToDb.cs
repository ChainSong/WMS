using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSTemplateToDb : SqlDataRecord
    {
        public WMSTemplateToDb(TableColumn wmsInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, wmsInfo.ID);
            SetSqlInt64(1, wmsInfo.ProjectID);
            SetSqlInt64(2, wmsInfo.CustomerID);
            SetSqlString(3, wmsInfo.TableName);
            SetSqlString(4, wmsInfo.DisplayName);
            SetSqlString(5, wmsInfo.DbColumnName);
            SetSqlBoolean(6, wmsInfo.IsKey);
            SetSqlBoolean(7, wmsInfo.IsSearchCondition);
            SetSqlBoolean(8, wmsInfo.IsHide);
            SetSqlBoolean(9, wmsInfo.IsReadOnly);
            SetSqlString(10, wmsInfo.Group);
            SetSqlString(11, wmsInfo.Type);
            SetSqlString(12, wmsInfo.DefaultValue);
            SetSqlInt64(13, wmsInfo.Order);
            SetSqlBoolean(14, wmsInfo.IsShowInList);
            SetSqlBoolean(15, wmsInfo.IsImportColumn);
            SetSqlInt64(16, wmsInfo.SearchConditionOrder);
            SetSqlBoolean(17, wmsInfo.ForView);
            SetSqlString(18, wmsInfo.Module);
            SetSqlInt64(19, wmsInfo.IsInnerColumn);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),          
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("TableName", SqlDbType.NVarChar, 100),
            new SqlMetaData("DisplayName", SqlDbType.NVarChar, 100),
            new SqlMetaData("DbColumnName", SqlDbType.NVarChar, 100),
            new SqlMetaData("IsKey", SqlDbType.Bit),
            new SqlMetaData("IsSearchCondition", SqlDbType.Bit),
            new SqlMetaData("IsHide", SqlDbType.Bit),
            new SqlMetaData("IsReadOnly", SqlDbType.Bit),
            new SqlMetaData("Group", SqlDbType.NVarChar, 100),
            new SqlMetaData("Type", SqlDbType.NVarChar, 100),
            new SqlMetaData("DefaultValue", SqlDbType.NVarChar, 100),
            new SqlMetaData("Order",SqlDbType.BigInt),
            new SqlMetaData("IsShowInList",  SqlDbType.Bit),
            new SqlMetaData("IsImportColumn", SqlDbType.Bit),
            new SqlMetaData("SearchConditionOrder", SqlDbType.BigInt),
            new SqlMetaData("ForView", SqlDbType.Bit),
            new SqlMetaData("Module", SqlDbType.NVarChar, 100),
            new SqlMetaData("IsInnerColumn", SqlDbType.BigInt)

        };
    }
}
