using System.Data;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity
{
    public class APIAndBackSettingToDb : SqlDataRecord
    {//ID,FileName,FileType,ServerName,FilePath,ProjectID,ProjectName,OrderNo,Creator,CreateTime,Updator,UpdateTime,Status
        public APIAndBackSettingToDb(APIAndBackSetting apiandbacksetting)
            : base(s_metadata)
        {
            SetSqlInt64(0, apiandbacksetting.ID);
            SetSqlString(1, apiandbacksetting.UserName);
            SetSqlString(2, apiandbacksetting.APPKey);
            SetSqlString(3, apiandbacksetting.APPSecret);
            SetSqlString(4, apiandbacksetting.APPToken);
            SetSqlInt32(5, apiandbacksetting.UserStatus);
            SetSqlString(6, apiandbacksetting.APIType);
            SetSqlString(7, apiandbacksetting.OrderType);
            SetSqlString(8, apiandbacksetting.StatusType);
            SetSqlString(9, apiandbacksetting.DisplayName);
            SetSqlString(10, apiandbacksetting.CallBackURL);
            SetSqlString(11, apiandbacksetting.HttpType);
            SetSqlInt64(12, apiandbacksetting.CustomerID);
            SetSqlString(13, apiandbacksetting.CustomerName);
            SetSqlString(14, apiandbacksetting.WarehouseID);
            SetSqlString(15, apiandbacksetting.WarehouseName);
            SetSqlString(16, apiandbacksetting.Remark);
            SetSqlDateTime(17, apiandbacksetting.CreateTime);
            SetSqlString(18, apiandbacksetting.CreateUser);
            SetSqlString(19, apiandbacksetting.Str1);
            SetSqlString(20, apiandbacksetting.Str2);
            SetSqlString(21, apiandbacksetting.Str3);
            SetSqlString(22, apiandbacksetting.Str4);
            SetSqlString(23, apiandbacksetting.Str5);
            SetSqlInt32(24, apiandbacksetting.Int1);
            SetSqlInt32(25, apiandbacksetting.Int2);
            SetSqlInt64(26, apiandbacksetting.Long1);
            SetSqlDateTime(27, apiandbacksetting.DateTime1);
            SetSqlDateTime(28, apiandbacksetting.DateTime2);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt, 100),
            new SqlMetaData("UserName", SqlDbType.VarChar, 100),
            new SqlMetaData("APPKey", SqlDbType.VarChar, 100),
            new SqlMetaData("APPSecret", SqlDbType.VarChar, 100),
            new SqlMetaData("APPToken", SqlDbType.VarChar, 100),
            new SqlMetaData("UserStatus", SqlDbType.Int),
            new SqlMetaData("APIType", SqlDbType.VarChar, 50),
            new SqlMetaData("OrderType", SqlDbType.VarChar, 50),
            new SqlMetaData("StatusType", SqlDbType.VarChar, 50),
            new SqlMetaData("DisplayName", SqlDbType.VarChar, 200),
            new SqlMetaData("CallBackURL", SqlDbType.VarChar, 200),
            new SqlMetaData("HttpType", SqlDbType.VarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.VarChar, 100),
            new SqlMetaData("WarehouseID", SqlDbType.BigInt),
            new SqlMetaData("WarehouseName", SqlDbType.VarChar, 100),
            new SqlMetaData("Remark", SqlDbType.VarChar, 500),
            new SqlMetaData("CreateTime", SqlDbType.DateTime, 50),
            new SqlMetaData("CreateUser", SqlDbType.VarChar, 100),
            new SqlMetaData("Str1", SqlDbType.VarChar, 500),
            new SqlMetaData("Str2", SqlDbType.VarChar, 500),
            new SqlMetaData("Str3", SqlDbType.VarChar, 500),
            new SqlMetaData("Str4", SqlDbType.VarChar, 500),
            new SqlMetaData("Str5", SqlDbType.VarChar, 500),
            new SqlMetaData("Int1", SqlDbType.Int),
            new SqlMetaData("Int2", SqlDbType.Int),
            new SqlMetaData("Long1", SqlDbType.BigInt, 100),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime), 
        };
    }
}
