using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Dao
{
    public class LogAccessor : BaseAccessor
    {
        public void Log(LogHistory logHistory)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Name", DbType.String, string.IsNullOrEmpty(logHistory.Name) ?  System.Data.SqlTypes.SqlString.Null : logHistory.Name , ParameterDirection.Input),
                new DbParam("@Time", DbType.DateTime, logHistory.Time, ParameterDirection.Input),
                new DbParam("@Action", DbType.String, string.IsNullOrEmpty(logHistory.Action) ? System.Data.SqlTypes.SqlString.Null : logHistory.Action, ParameterDirection.Input)
            };

            this.ExecuteNoQuery("Proc_AddLog", dbParams);
        }
        public void AkzoAs2ApiLog(string SourceFileName, string ToFileName, string Type, string Flag, string ResultDesc)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SourceFileName", DbType.String, SourceFileName , ParameterDirection.Input),
                new DbParam("@ToFileName", DbType.String, ToFileName, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input),
                new DbParam("@Flag", DbType.String, Flag, ParameterDirection.Input),
                new DbParam("@ResultDesc", DbType.String,ResultDesc, ParameterDirection.Input)
            };

            this.ExecuteNoQuery("Proc_WMS_AddAkzoAs2ApiLog", dbParams);
        }
        public void AkzoAs2ApiLog(string SourceFileName, string ToFileName, string Type, string Flag, string ResultDesc,string Str1,string Str2,string Str3)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@SourceFileName", DbType.String, SourceFileName , ParameterDirection.Input),
                new DbParam("@ToFileName", DbType.String, ToFileName, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input),
                new DbParam("@Flag", DbType.String, Flag, ParameterDirection.Input),
                new DbParam("@ResultDesc", DbType.String,ResultDesc, ParameterDirection.Input),
                                new DbParam("@Str1", DbType.String, Str1, ParameterDirection.Input),
                                                new DbParam("@Str2", DbType.String, Str2, ParameterDirection.Input),
                                                                new DbParam("@Str3", DbType.String, Str3, ParameterDirection.Input)
            };

            this.ExecuteNoQuery("Proc_WMS_AddAkzoAs2ApiLog", dbParams);
        }

        public bool CheckInvenUpload(string Str1,string Str2,string Str3)
        {
            DbParam[] dbParams = new DbParam[]{
     new DbParam("@Str1", DbType.String, Str1, ParameterDirection.Input),
                                                new DbParam("@Str2", DbType.String, Str2, ParameterDirection.Input),
                                                                new DbParam("@Str3", DbType.String, Str3, ParameterDirection.Input)
               };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_CheckInvenUpload", dbParams);
            if (dt.Rows.Count > 0)
                return true;
            return false;
           
        }

        public void NikeCELog(string TransactionID, string SaveFile, string Type, string Flag, string Message)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@TransactionID", DbType.String, TransactionID , ParameterDirection.Input),
                new DbParam("@SaveFile", DbType.String, SaveFile, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input),
                new DbParam("@Flag", DbType.String, Flag, ParameterDirection.Input),
                new DbParam("@Message", DbType.String,Message, ParameterDirection.Input)
            };

            this.ExecuteNoQuery("Proc_WMS_AddNikeCELog", dbParams);
        }
    }
}
