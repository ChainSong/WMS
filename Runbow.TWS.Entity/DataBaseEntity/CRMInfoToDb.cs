using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using System.Data;
namespace Runbow.TWS.Entity
{
    public class CRMInfoToDb : SqlDataRecord
    {
        public CRMInfoToDb(CRMInfo info)
            : base(s_metadata)
        {
            SetSqlInt64(0, info.ID);
            SetSqlString(1, info.ProjectName);
            SetSqlString(2, info.CustomerName);
            SetSqlString(3, info.City);
            SetSqlString(4, info.Sex);
            SetSqlString(5, info.Date0fBirth);
            SetSqlString(6, info.Age);
            SetSqlString(7, info.Phone);
            SetSqlString(8, info.IsMarry);
            SetSqlString(9, info.EducationalBackground);
            SetSqlString(10, info.NativePlace);
            SetSqlString(11, info.WorkUnit);
            SetSqlString(12, info.OnTime);
            SetSqlString(13, info.WorkingLife);
            SetSqlString(14, info.WorkExperience);
            SetSqlString(15, info.FamilyComposition);
            SetSqlString(16, info.HomeAddress);
            SetSqlString(17, info.FamilyInformation);
            SetSqlString(18, info.PersonalHobbies);
            SetSqlString(19, info.FoodPreferences);
            SetSqlString(20, info.Dress);
            SetSqlString(21, info.Hobby);
            SetSqlString(22, info.TakeOfficeCompany);
            SetSqlString(23, info.SectionResponsibleFor);
            SetSqlString(24, info.DirectSupervisor);
            SetSqlString(25, info.Team);
            SetSqlString(26, info.SalaryTreatment);
            SetSqlString(27, info.PersonalReputation);
            SetSqlString(28, info.TheCurrentJobSatisfaction);
            SetSqlString(29, info.WithRunbowContactTime);
            SetSqlString(30, info.WithRunbowContactExperience);
            SetSqlString(31, info.WithProjectsupplierContactTime);
            SetSqlString(32, info.WithOther3PLContact);
            SetSqlString(33, info.CRMTYPE);
            SetSqlString(34, info.CreateTime);
            SetSqlString(35, info.UpdateTime);
            SetSqlString(36, info.Str1);
            SetSqlString(37, info.Str2);
            SetSqlString(38, info.Str3);
            SetSqlString(39, info.Str4);
            SetSqlString(40, info.Str5);
            SetSqlString(41, info.Str6);
            SetSqlString(42, info.Str7);
            SetSqlString(43, info.Str8);
            SetSqlString(44, info.Str9);
            SetSqlString(45, info.Str10);
            SetSqlString(46, info.Str11);
            SetSqlString(47, info.Str12);

        }

        private static readonly SqlMetaData[] s_metadata =
        {
             new SqlMetaData("ID", SqlDbType.BigInt),
             new SqlMetaData("ProjectName", SqlDbType.NVarChar,50),
             new SqlMetaData("CustomerName", SqlDbType.NVarChar,50),
             new SqlMetaData("City", SqlDbType.NVarChar,50),
             new SqlMetaData("Sex", SqlDbType.NVarChar,50),
             new SqlMetaData("Date0fBirth", SqlDbType.NVarChar,50),
             new SqlMetaData("Age", SqlDbType.NVarChar,50),
             new SqlMetaData("Phone", SqlDbType.NVarChar,50),
             new SqlMetaData("IsMarry", SqlDbType.NVarChar,50),
             new SqlMetaData("EducationalBackground", SqlDbType.NVarChar,50),
             new SqlMetaData("NativePlace", SqlDbType.NVarChar,50),
             new SqlMetaData("WorkUnit", SqlDbType.NVarChar,50),
             new SqlMetaData("OnTime", SqlDbType.NVarChar,50),
             new SqlMetaData("WorkingLife", SqlDbType.NVarChar,50),
             new SqlMetaData("WorkExperience", SqlDbType.NVarChar,500),
             new SqlMetaData("FamilyComposition", SqlDbType.NVarChar,500),
             new SqlMetaData("HomeAddress", SqlDbType.NVarChar,500),
             new SqlMetaData("FamilyInformation", SqlDbType.NVarChar,500),
             new SqlMetaData("PersonalHobbies", SqlDbType.NVarChar,50),
             new SqlMetaData("FoodPreferences", SqlDbType.NVarChar,50),
             new SqlMetaData("Dress", SqlDbType.NVarChar,50),
             new SqlMetaData("Hobby", SqlDbType.NVarChar,50),
             new SqlMetaData("TakeOfficeCompany", SqlDbType.NVarChar,50),
             new SqlMetaData("SectionResponsibleFor", SqlDbType.NVarChar,50),
             new SqlMetaData("DirectSupervisor", SqlDbType.NVarChar,50),
             new SqlMetaData("Team", SqlDbType.NVarChar,50),
             new SqlMetaData("SalaryTreatment", SqlDbType.NVarChar,50),
             new SqlMetaData("PersonalReputation", SqlDbType.NVarChar,50),
             new SqlMetaData("TheCurrentJobSatisfaction", SqlDbType.NVarChar,50),
             new SqlMetaData("WithRunbowContactTime", SqlDbType.NVarChar,50),
             new SqlMetaData("WithRunbowContactExperience", SqlDbType.NVarChar,50),
             new SqlMetaData("WithProjectsupplierContactTime", SqlDbType.NVarChar,50),
             new SqlMetaData("WithOther3PLContact", SqlDbType.NVarChar,50),
             new SqlMetaData("CRMTYPE", SqlDbType.NVarChar,50),
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
             new SqlMetaData("Str9", SqlDbType.NVarChar,50),
             new SqlMetaData("Str10", SqlDbType.NVarChar,50),
             new SqlMetaData("Str11", SqlDbType.NVarChar,50),
             new SqlMetaData("Str12", SqlDbType.NVarChar,50)
        };
    }
}
