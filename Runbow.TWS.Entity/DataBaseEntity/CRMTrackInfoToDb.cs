using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class CRMTrackInfoToDb : SqlDataRecord
    {
        public CRMTrackInfoToDb(CRMTrackInfo TrackInfo)
            : base(s_metadata)
        {
            SetSqlInt64(0, TrackInfo.ID);
            SetSqlInt64(1, TrackInfo.CRMID);
            SetSqlString(2, TrackInfo.VisitPeople);
            SetSqlString(3, TrackInfo.VisitTime);
            SetSqlString(4, TrackInfo.VisitPlace);
            SetSqlString(5, TrackInfo.VisitForm);
            SetSqlString(6, TrackInfo.GiftsArticles);
            SetSqlString(7, TrackInfo.VisitToCustomerEvaluation);
            SetSqlString(8, TrackInfo.VisitingPersonnelFeedbackVisit);
            SetSqlString(9, TrackInfo.ProjectCustomerCommunication);
            SetSqlString(10, TrackInfo.CustomerSupportAndAssistance);
            SetSqlString(11, TrackInfo.CreateTime);
            SetSqlString(12, TrackInfo.UpdateTime);
            SetSqlString(13, TrackInfo.Str1);
            SetSqlString(14, TrackInfo.Str2);
            SetSqlString(15, TrackInfo.Str3);
            SetSqlString(16, TrackInfo.Str4);
            SetSqlString(17, TrackInfo.Str5);
            SetSqlString(18, TrackInfo.Str6);
            SetSqlString(19, TrackInfo.Str7);
            SetSqlString(20, TrackInfo.Str8);

        }

        private static readonly SqlMetaData[] s_metadata =
        {
             new SqlMetaData("ID", SqlDbType.BigInt),
             new SqlMetaData("CRMID", SqlDbType.BigInt),
             new SqlMetaData("VisitPeople", SqlDbType.NVarChar,50),
             new SqlMetaData("VisitTime", SqlDbType.NVarChar,50),
             new SqlMetaData("VisitPlace", SqlDbType.NVarChar,50),
             new SqlMetaData("VisitForm", SqlDbType.NVarChar,50),
             new SqlMetaData("GiftsArticles", SqlDbType.NVarChar,50),
             new SqlMetaData("VisitToCustomerEvaluation", SqlDbType.NVarChar,500),
             new SqlMetaData("VisitingPersonnelFeedbackVisit", SqlDbType.NVarChar,500),
             new SqlMetaData("ProjectCustomerCommunication", SqlDbType.NVarChar,500),
             new SqlMetaData("CustomerSupportAndAssistance", SqlDbType.NVarChar,500),
             new SqlMetaData("CreateTime", SqlDbType.NVarChar,50),
             new SqlMetaData("UpdateTime", SqlDbType.NVarChar,50),
             new SqlMetaData("Str1", SqlDbType.NVarChar,50),
             new SqlMetaData("Str2", SqlDbType.NVarChar,50),
             new SqlMetaData("Str3", SqlDbType.NVarChar,50),
             new SqlMetaData("Str4", SqlDbType.NVarChar,50),
             new SqlMetaData("Str5", SqlDbType.NVarChar,50),
             new SqlMetaData("Str6", SqlDbType.NVarChar,50),
             new SqlMetaData("Str7", SqlDbType.NVarChar,50),
             new SqlMetaData("Str8", SqlDbType.NVarChar,50),
        };
    }
}
