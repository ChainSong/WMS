using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;


namespace Runbow.TWS.Dao
{
    public class CRMInfoAccessor : BaseAccessor
    {
        public IEnumerable<CRMInfo> GetCRMInfo(CRMInfo Info,int PageIndex,int PageSize,out int RowCount)
        {
            string strSQL = this.GetSqlWhere(Info);
            DbParam[] dbParams = {
                           new DbParam("@where",DbType.String,strSQL,ParameterDirection.Input),
                           new DbParam("@PageIndex",DbType.Int32,PageIndex,ParameterDirection.Input),
                           new DbParam("@PageSize",DbType.Int32,PageSize,ParameterDirection.Input),
                           new DbParam("@RowCount",DbType.Int32,0,ParameterDirection.Output)
                          };            
                IEnumerable <CRMInfo> crmlist =base.ExecuteDataTable("Proc_GetCRMinfo", dbParams).ConvertToEntityCollection<CRMInfo>();
                RowCount = (int)dbParams[3].Value;
                return crmlist;
        }
        public CRMInfo OperateCRMInfo(CRMInfo info)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var CRMList = new List<CRMInfoToDb>();
                CRMList.Add(new CRMInfoToDb(info));
                SqlCommand cmd = new SqlCommand("Proc_OperateCRMinfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CRMINFOSOURCE", CRMList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@outputID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                conn.Open();

                cmd.ExecuteNonQuery();

                info.ID = cmd.Parameters[1].Value.ObjectToInt64();
                
                
                return info; 
            }
        }

        public int DeleteCRMInfo(long id)
        {
            DbParam[] Db = {
                           new DbParam("@ID",DbType.Int64,id,ParameterDirection.Input),
                           new DbParam("@ReturnVal",DbType.Int32,0,ParameterDirection.Output)
                          };
            base.ExecuteNoQuery("Proc_DeleteCRMinfo", Db);
            int returnval = Db[1].Value.ObjectToInt32();
            return returnval;
        }

        public string GetSqlWhere(CRMInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" 1=1 ");
            if (info.ID.ObjectToInt64() != 0)
            {
                sb.Append(" AND ID = '" + info.ID + "'");
            }
            if(!string.IsNullOrEmpty(info.CustomerName))
            {
                sb.Append(" AND CustomerName like '%"+info.CustomerName+"%'");
            }
            if (!string.IsNullOrEmpty(info.CRMTYPE))
            {
                sb.Append(" AND CRMTYPE='" + info.CRMTYPE + "'");
            }
            if (!string.IsNullOrEmpty(info.ProjectName))
            {
                sb.Append(" AND ProjectName like '%" + info.ProjectName + "%'");
            }
            if (!string.IsNullOrEmpty(info.City))
            {
                sb.Append(" AND City='" + info.City + "'");
            }
            if (!string.IsNullOrEmpty(info.WorkUnit))
            {
                sb.Append(" AND WorkUnit like '%" + info.WorkUnit + "%'");
            }

            return sb.ToString();
        }

        public CRMInfo GetCRMInfoByID(long? ID)
        {
            DbParam[] dbParams = {
                           new DbParam("@ID",DbType.String,ID,ParameterDirection.Input)
                          };

            return base.ExecuteDataTable("[Proc_GetCRMInfoByID]", dbParams).ConvertToEntity<CRMInfo>();
        }

        public DataTable selectCRMInfo(string  type)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql =string.Format( "select  ProjectName, CustomerName, City, Sex, Date0fBirth, Age, Phone, IsMarry, EducationalBackground, NativePlace, WorkUnit, OnTime, WorkingLife, WorkExperience, FamilyComposition, HomeAddress, FamilyInformation, PersonalHobbies, FoodPreferences, Dress, Hobby, TakeOfficeCompany, SectionResponsibleFor, DirectSupervisor, Team, SalaryTreatment, PersonalReputation, TheCurrentJobSatisfaction, WithRunbowContactTime, WithRunbowContactExperience, WithProjectsupplierContactTime, WithOther3PLContact, CRMTYPE, CreateTime, UpdateTime from  [dbo].[CRMInfo] where CRMTYPE='{0}'",type);
                SqlCommand comm = new SqlCommand(sql, conn);
               
                 //comm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sdr = new SqlDataAdapter(comm);
                DataTable dt = new DataTable();
                sdr.Fill(dt);
                return dt;
            }
        }
        #region NoUse

        //public CRMInfo Items(SqlDataReader sdr)
        //{
        //    Runbow.TWS.Entity.CRM.CRMInfo crminfo = new Entity.CRM.CRMInfo();
        //    //change to ObjectTo...
        //    crminfo.ID = sdr["ID"] != DBNull.Value ? sdr["ID"].ObjectToInt64() : Convert.ToInt64(DBNull.Value);
        //    crminfo.ProjectName = sdr["ProjectName"] != DBNull.Value ? sdr["ProjectName"].ToString() : DBNull.Value.ToString();
        //    crminfo.CustomerName = sdr["CustomerName"] != DBNull.Value ? sdr["CustomerName"].ToString() : DBNull.Value.ToString();
        //    crminfo.City = sdr["City"] != DBNull.Value ? sdr["City"].ToString() : DBNull.Value.ToString();
        //    crminfo.Sex = sdr["Sex"] != DBNull.Value ? sdr["Sex"].ToString() : DBNull.Value.ToString();
        //    crminfo.Date0fBirth = sdr["Date0fBirth"] != DBNull.Value ? sdr["Date0fBirth"].ToString() : DBNull.Value.ToString();
        //    crminfo.Age = sdr["Age"] != DBNull.Value ? sdr["Age"].ToString() : DBNull.Value.ToString();
        //    crminfo.Phone = sdr["Phone"] != DBNull.Value ? sdr["Phone"].ToString() : DBNull.Value.ToString();
        //    crminfo.IsMarry = sdr["IsMarry"] != DBNull.Value ? sdr["IsMarry"].ToString() : DBNull.Value.ToString();
        //    crminfo.EducationalBackground = sdr["EducationalBackground"] != DBNull.Value ? sdr["EducationalBackground"].ToString() : DBNull.Value.ToString();
        //    crminfo.NativePlace = sdr["NativePlace"] != DBNull.Value ? sdr["NativePlace"].ToString() : DBNull.Value.ToString();
        //    crminfo.WorkUnit = sdr["WorkUnit"] != DBNull.Value ? sdr["WorkUnit"].ToString() : DBNull.Value.ToString();
        //    crminfo.OnTime = sdr["OnTime"] != DBNull.Value ? sdr["OnTime"].ToString() : DBNull.Value.ToString();
        //    crminfo.WorkingLife = sdr["WorkingLife"] != DBNull.Value ? sdr["WorkingLife"].ToString() : DBNull.Value.ToString();
        //    crminfo.WorkExperience = sdr["WorkExperience"] != DBNull.Value ? sdr["WorkExperience"].ToString() : DBNull.Value.ToString();
        //    crminfo.FamilyComposition = sdr["FamilyComposition"] != DBNull.Value ? sdr["FamilyComposition"].ToString() : DBNull.Value.ToString();
        //    crminfo.HomeAddress = sdr["HomeAddress"] != DBNull.Value ? sdr["HomeAddress"].ToString() : DBNull.Value.ToString();
        //    crminfo.FamilyInformation = sdr["FamilyInformation"] != DBNull.Value ? sdr["FamilyInformation"].ToString() : DBNull.Value.ToString();
        //    crminfo.PersonalHobbies = sdr["PersonalHobbies"] != DBNull.Value ? sdr["PersonalHobbies"].ToString() : DBNull.Value.ToString();
        //    crminfo.FoodPreferences = sdr["FoodPreferences"] != DBNull.Value ? sdr["FoodPreferences"].ToString() : DBNull.Value.ToString();

        //    crminfo.Dress = sdr["Dress"] != DBNull.Value ? sdr["Dress"].ToString() : DBNull.Value.ToString();
        //    crminfo.Hobby = sdr["Hobby"] != DBNull.Value ? sdr["Hobby"].ToString() : DBNull.Value.ToString();
        //    crminfo.TakeOfficeCompany = sdr["TakeOfficeCompany"] != DBNull.Value ? sdr["TakeOfficeCompany"].ToString() : DBNull.Value.ToString();
        //    crminfo.SectionResponsibleFor = sdr["SectionResponsibleFor"] != DBNull.Value ? sdr["SectionResponsibleFor"].ToString() : DBNull.Value.ToString();
        //    crminfo.DirectSupervisor = sdr["DirectSupervisor"] != DBNull.Value ? sdr["DirectSupervisor"].ToString() : DBNull.Value.ToString();
        //    crminfo.Team = sdr["Team"] != DBNull.Value ? sdr["Team"].ToString() : DBNull.Value.ToString();
        //    crminfo.SalaryTreatment = sdr["SalaryTreatment"] != DBNull.Value ? sdr["SalaryTreatment"].ToString() : DBNull.Value.ToString();
        //    crminfo.PersonalReputation = sdr["PersonalReputation"] != DBNull.Value ? sdr["PersonalReputation"].ToString() : DBNull.Value.ToString();
        //    crminfo.TheCurrentJobSatisfaction = sdr["TheCurrentJobSatisfaction"] != DBNull.Value ? sdr["TheCurrentJobSatisfaction"].ToString() : DBNull.Value.ToString();
        //    crminfo.WithRunbowContactTime = sdr["WithRunbowContactTime"] != DBNull.Value ? sdr["WithRunbowContactTime"].ToString() : DBNull.Value.ToString();
        //    crminfo.WithRunbowContactExperience = sdr["WithRunbowContactExperience"] != DBNull.Value ? sdr["WithRunbowContactExperience"].ToString() : DBNull.Value.ToString();
        //    crminfo.WithProjectsupplierContactTime = sdr["WithProjectsupplierContactTime"] != DBNull.Value ? sdr["WithProjectsupplierContactTime"].ToString() : DBNull.Value.ToString();

        //    crminfo.WithOther3PLContact = sdr["WithOther3PLContact"] != DBNull.Value ? sdr["WithOther3PLContact"].ToString() : DBNull.Value.ToString();
        //    crminfo.CRMTYPE = sdr["CRMTYPE"] != DBNull.Value ? sdr["CRMTYPE"].ToString() : DBNull.Value.ToString();
        //    crminfo.CreateTime = sdr["CreateTime"] != DBNull.Value ? sdr["CreateTime"].ToString() : DBNull.Value.ToString();
        //    crminfo.UpdateTime = sdr["UpdateTime"] != DBNull.Value ? sdr["UpdateTime"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str1 = sdr["Str1"] != DBNull.Value ? sdr["Str1"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str2 = sdr["Str2"] != DBNull.Value ? sdr["Str2"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str3 = sdr["Str3"] != DBNull.Value ? sdr["Str3"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str4 = sdr["Str4"] != DBNull.Value ? sdr["Str4"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str5 = sdr["Str5"] != DBNull.Value ? sdr["Str5"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str6 = sdr["Str6"] != DBNull.Value ? sdr["Str6"].ToString() : DBNull.Value.ToString();

        //    crminfo.Str7 = sdr["Str7"] != DBNull.Value ? sdr["Str7"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str8 = sdr["Str8"] != DBNull.Value ? sdr["Str8"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str9 = sdr["Str9"] != DBNull.Value ? sdr["Str9"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str10 = sdr["Str10"] != DBNull.Value ? sdr["Str10"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str11 = sdr["Str11"] != DBNull.Value ? sdr["Str11"].ToString() : DBNull.Value.ToString();
        //    crminfo.Str12 = sdr["Str12"] != DBNull.Value ? sdr["Str12"].ToString() : DBNull.Value.ToString();
        //    return crminfo;
        //}

        #endregion NoUse
        public CRMInfo AddCrm(CRMInfo CRMInfo)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                var CrmList = new List<CrmToDb>();
                CrmList.Add(new CrmToDb(CRMInfo));
                SqlCommand cmd = new SqlCommand("Proc_AddCrm", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CrmData", CrmList);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@OutCrmID", 0);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                conn.Open();

                cmd.ExecuteNonQuery();

                CRMInfo.ID = cmd.Parameters[1].Value.ObjectToInt64();

                return CRMInfo;
            }
        }


        public IEnumerable<CRMInfo> AddCrms(IEnumerable<CRMInfo> crms)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddCrm", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                 //cmd.Parameters.AddWitValue("@CrmData", crms.Select(p => new CrmToDb(p)));
                cmd.Parameters.AddWithValue("@CrmData", crms.Select(p => new CrmToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;

                conn.Open();
                IList<CRMInfo> returnCrms = new List<CRMInfo>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnCrms.Add(
                        new CRMInfo()
                        {
                           // ID = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            ProjectName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            CustomerName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            City = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Sex = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            Date0fBirth = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            Age = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            Phone = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            IsMarry = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                            WorkingLife = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                            FamilyInformation = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                            Dress = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                            Hobby = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                            TakeOfficeCompany = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                            SectionResponsibleFor = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                            DirectSupervisor = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                            Team = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                            SalaryTreatment = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                            PersonalReputation = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                            TheCurrentJobSatisfaction = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                            WithRunbowContactTime = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                            WithRunbowContactExperience = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                            WithProjectsupplierContactTime = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                            WithOther3PLContact = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                            CRMTYPE = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                            CreateTime = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                            UpdateTime = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                            Str1 = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                            Str2 = reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                            Str3 = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                            Str4 = reader.IsDBNull(27) ? string.Empty : reader.GetString(27),
                            Str6 = reader.IsDBNull(28) ? string.Empty : reader.GetString(28),
                            Str7 = reader.IsDBNull(29) ? string.Empty : reader.GetString(29),
                            Str8 = reader.IsDBNull(30) ? string.Empty : reader.GetString(30),
                            Str9 = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),

                            Str10 = reader.IsDBNull(32) ? string.Empty : reader.GetString(32),
                            Str11= reader.IsDBNull(33) ? string.Empty : reader.GetString(33),
                            Str12 = reader.IsDBNull(34) ? string.Empty : reader.GetString(34),
                        });
                }

                return returnCrms;
            }
        }


        public static int Import(string sql)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                
                SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@PodData", podList);
                //cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                //cmd.Parameters.AddWithValue("@OutputID", 0);
                //cmd.Parameters[1].Direction = ParameterDirection.Output;
                //cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}