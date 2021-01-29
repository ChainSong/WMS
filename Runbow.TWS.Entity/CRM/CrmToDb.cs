//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;
namespace Runbow.TWS.Entity
{
   public  class CrmToDb:SqlDataRecord
    {
       public CrmToDb(CRMInfo CRMInfo) 
           : base(s_metadata) 
       {
           SetSqlInt64(0, CRMInfo.ID);
           SetSqlString(1, CRMInfo.ProjectName);
           SetSqlString(2, CRMInfo.CustomerName);
           SetSqlString(3, CRMInfo.City);
           SetSqlString(4, CRMInfo.Sex );
           SetSqlString(5, CRMInfo.Date0fBirth);
           SetSqlString(6, CRMInfo.Age);
           SetSqlString(7, CRMInfo.Phone);
           SetSqlString(8, CRMInfo.IsMarry);
           SetSqlString(9, CRMInfo.EducationalBackground);
           SetSqlString(10, CRMInfo.NativePlace);
           SetSqlString(11, CRMInfo.WorkUnit);
           SetSqlString(12, CRMInfo.OnTime);
           SetSqlString(13, CRMInfo.WorkingLife);
           SetSqlString(14, CRMInfo.WorkExperience);
           SetSqlString(15, CRMInfo.FamilyComposition);
           SetSqlString(16, CRMInfo.HomeAddress);
           SetSqlString(17, CRMInfo.FamilyInformation);
           SetSqlString(18, CRMInfo.PersonalHobbies);
           SetSqlString(19, CRMInfo.FoodPreferences);
           SetSqlString(20, CRMInfo.Dress);
           SetSqlString(21, CRMInfo.Hobby);
           SetSqlString(22, CRMInfo.TakeOfficeCompany);
           SetSqlString(35, CRMInfo.SectionResponsibleFor);
           SetSqlString(36, CRMInfo.DirectSupervisor);
           SetSqlString(37, CRMInfo.Team);
           SetSqlString(38, CRMInfo.SalaryTreatment);
           SetSqlString(39, CRMInfo.PersonalReputation);
           SetSqlString(40, CRMInfo.TheCurrentJobSatisfaction);
           SetSqlString(41, CRMInfo.WithRunbowContactTime);
           SetSqlString(42, CRMInfo.WithRunbowContactExperience);
           SetSqlString(43, CRMInfo.WithProjectsupplierContactTime);
           SetSqlString(44, CRMInfo.WithOther3PLContact);
           SetSqlString(45, CRMInfo.CRMTYPE);
           SetSqlString(46, CRMInfo.CreateTime);
           SetSqlString(47, CRMInfo.UpdateTime);
           SetSqlString(23, CRMInfo.Str1);
           SetSqlString(24, CRMInfo.Str2);
           SetSqlString(25, CRMInfo.Str3);
           SetSqlString(26, CRMInfo.Str4);
           SetSqlString(27, CRMInfo.Str5);
           SetSqlString(28, CRMInfo.Str6);
           SetSqlString(29, CRMInfo.Str7);
           SetSqlString(30, CRMInfo.Str8);
           SetSqlString(31, CRMInfo.Str9);
           SetSqlString(32, CRMInfo.Str10);
           SetSqlString(33, CRMInfo.Str11);
           SetSqlString(34, CRMInfo.Str12);
       }

       private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),   
            new SqlMetaData("ProjectName", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("City",  SqlDbType.NVarChar, 50),
            new SqlMetaData("Sex", SqlDbType.NVarChar, 50),
            new SqlMetaData("Age", SqlDbType.NVarChar, 50),
            new SqlMetaData("Phone", SqlDbType.NVarChar, 50),
            new SqlMetaData("IsMarry", SqlDbType.NVarChar, 50),
            new SqlMetaData("EducationalBackground",SqlDbType.NVarChar, 50),
            new SqlMetaData("NativePlace", SqlDbType.NVarChar, 50),
            new SqlMetaData("WorkUnit", SqlDbType.NVarChar, 50),
            new SqlMetaData("OnTime", SqlDbType.NVarChar, 50),
            new SqlMetaData("WorkingLife", SqlDbType.NVarChar, 50),
            new SqlMetaData("WorkExperience", SqlDbType.NVarChar, 50),
            new SqlMetaData("FamilyComposition", SqlDbType.NVarChar, 50),
            new SqlMetaData("FamilyInformation", SqlDbType.NVarChar, 50),
            new SqlMetaData("PersonalHobbies", SqlDbType.NVarChar, 50),
            new SqlMetaData("FoodPreferences", SqlDbType.NVarChar, 50),
            new SqlMetaData("Dress", SqlDbType.NVarChar, 50),
            new SqlMetaData("TakeOfficeCompany", SqlDbType.NVarChar, 50),
            new SqlMetaData("TakeOfficeCompanyTakeOfficeCompany", SqlDbType.NVarChar, 50),
            new SqlMetaData("SectionResponsibleFor", SqlDbType.NVarChar, 50),     
            new SqlMetaData("DirectSupervisor", SqlDbType.NVarChar, 50),
            new SqlMetaData("Team", SqlDbType.NVarChar, 50),
            new SqlMetaData("SalaryTreatment", SqlDbType.NVarChar, 50),
            new SqlMetaData("PersonalReputation", SqlDbType.NVarChar, 50),
            new SqlMetaData("TheCurrentJobSatisfaction", SqlDbType.NVarChar, 50),
            new SqlMetaData("WithRunbowContactTime", SqlDbType.NVarChar, 50),
            new SqlMetaData("WithRunbowContactExperience", SqlDbType.NVarChar, 50),
            new SqlMetaData("WithProjectsupplierContactTime", SqlDbType.NVarChar, 50),
            new SqlMetaData("WithOther3PLContact", SqlDbType.NVarChar, 50),
            new SqlMetaData("CRMTYPE", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.NVarChar, 50),
            new SqlMetaData("UpdateTime", SqlDbType.NVarChar, 50), 
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str4", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str5", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str6", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str7", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str8", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str9", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str10", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str11", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str12", SqlDbType.NVarChar, 50)
        };
      
    }
}
