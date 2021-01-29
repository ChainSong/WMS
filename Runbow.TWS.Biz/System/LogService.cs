using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz.System
{
    public class LogService : BaseService
    {
        public void Log(LogRequest request)
        {
            if (request == null || request.LogHistory == null)
            {
                ArgumentNullException ex = new ArgumentNullException("EditUser request");
                LogError(ex);
            }

            try
            {
                LogAccessor accessor = new LogAccessor();
                accessor.Log(request.LogHistory);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        public void AkzoAs2ApiLog(string SourceFileName,string ToFileName,string Type,string Flag,string ResultDesc)
        {
            try
            {
                LogAccessor accessor = new LogAccessor();
                accessor.AkzoAs2ApiLog( SourceFileName, ToFileName, Type, Flag, ResultDesc);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        public void AkzoAs2ApiLog(string SourceFileName, string ToFileName, string Type, string Flag, string ResultDesc,string Str1,string Str2,string Str3)
        {
            try
            {
                LogAccessor accessor = new LogAccessor();
                accessor.AkzoAs2ApiLog(SourceFileName, ToFileName, Type, Flag, ResultDesc, Str1, Str2, Str3);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        public bool CheckInvenUpload(string Str1,string Str2,string Str3)
        {
            try
            {
                LogAccessor accessor = new LogAccessor();
                return accessor.CheckInvenUpload(Str1, Str2, Str3);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return true;
            }
        }

        public void NikeCELog(string TransactionID, string SaveFile, string Type, string Flag, string Message)
        {
            try
            {
                LogAccessor accessor = new LogAccessor();
                accessor.NikeCELog(TransactionID,SaveFile,Type,Flag,Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
    }
}
