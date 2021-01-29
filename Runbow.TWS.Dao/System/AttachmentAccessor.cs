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
    public class AttachmentAccessor : BaseAccessor
    {
        public IEnumerable<Attachment> AddAttachments(IEnumerable<Attachment> attachments, bool IsCoverOld)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                IList<Attachment> result = new List<Attachment>();
                SqlCommand cmd = new SqlCommand("Proc_AddAttachment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Attachments", attachments == null ? null : attachments.Select(attachment => new AttachmentsToDb(attachment)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@IsCoverOld", IsCoverOld);
                cmd.Parameters[1].SqlDbType = SqlDbType.Bit;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(
                        new Attachment()
                        {
                            ID = reader.GetInt64(0),
                            GroupID = reader.GetString(1),
                            Url = reader.GetString(2),
                            ActualNameInServer = reader.GetString(3),
                            DisplayName = reader.GetString(4),
                            Extension = reader.GetString(5),
                            CreateDate = reader.GetDateTime(6),
                            CreateUserID = reader.GetInt64(7),
                            Creator = reader.GetString(8),
                            IsAudit = reader.GetBoolean(9)
                        });
                }

                return result;
            }
        }

        public Attachment DeleteAttachment(long id)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input),
            };
            return this.ExecuteDataTable("Proc_DeleteAttachmentByID", dbParams).ConvertToEntity<Attachment>();
        }

        public Attachment GetAttachmentByID(long id)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input),
            };
            return this.ExecuteDataTable("Proc_GetAttachmentByID", dbParams).ConvertToEntity<Attachment>();
        }

        public IEnumerable<Attachment> GetAttachmentsByGroupID(string groupID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@GroupID", DbType.String, groupID, ParameterDirection.Input),
            };
            return this.ExecuteDataTable("Proc_GetAttachmentsByGroupID", dbParams).ConvertToEntityCollection<Attachment>();
        }

        public string getTips(long DeliverID)
        {
            string message = "";
            string sql = @"SELECT TOP (1) Tip FROM [dbo].[WMS_Tips] WHERE Int1=1 ORDER BY CreateTime DESC";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    message = cmd.ExecuteScalar().ToString();
                    conn.Close();
                }
                catch (Exception e)
                { }
            }
            return message;
        }


        ////导出图片
        //public IEnumerable<Attachment> PodWithAttachment(PodSearchCondition Searchcondition)
        //{

        //    string Where = this.GetPodWithAttachment(Searchcondition);

        //    DbParam[] dbParams = new DbParam[]{
        //       new DbParam("@Where", DbType.String, Where, ParameterDirection.Input)
        //    };
        //    return this.ExecuteDataTable("Proc_PodWithAttachment", dbParams).ConvertToEntityCollection<Attachment>();
        //}


        //public string GetPodWithAttachment(PodSearchCondition Searchcondition)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    //查询条件
        //    #region
        //    //系统运单号
        //    if (!string.IsNullOrEmpty(Searchcondition.SystemNumber))
        //    {
        //        IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
        //        if (Searchcondition.SystemNumber.IndexOf("\n") > 0)
        //        {
        //            systemNumbers = Searchcondition.SystemNumber.Split('\n').Select(s => { return s.Trim(); });
        //        }
        //        if (Searchcondition.SystemNumber.IndexOf(',') > 0)
        //        {
        //            systemNumbers = Searchcondition.SystemNumber.Split(',').Select(s => { return s.Trim(); });
        //        }

        //        if (systemNumbers != null && systemNumbers.Any())
        //        {
        //            systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
        //        }

        //        if (systemNumbers != null && systemNumbers.Any())
        //        {
        //            sb.Append(" and SystemNumber in ( ");
        //            foreach (string s in systemNumbers)
        //            {
        //                sb.Append("'").Append(s).Append("',");
        //            }
        //            sb.Remove(sb.Length - 1, 1);
        //            sb.Append(" ) ");
        //        }
        //        else
        //        {
        //            sb.Append(" and SystemNumber like '%" + Searchcondition.SystemNumber.Trim() + "%' ");
        //        }
        //    }

        //    //起运城市
        //    if (Searchcondition.StartCityID != null && Searchcondition.StartCityID != 0)
        //    {
        //        sb.Append(" and StartCityID=" + Searchcondition.StartCityID + " ");
        //    }
        //    //目的城市
        //    if (Searchcondition.EndCityID != null && Searchcondition.EndCityID != 0)
        //    {
        //        sb.Append(" and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + Searchcondition.EndCityID + ")) ");
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(Searchcondition.EndCities))
        //        {
        //            sb.Append(" and EndCityID in (").Append(Searchcondition.EndCities).Append(") ");
        //        }
        //    }
        //    //客户运单号
        //    if (!string.IsNullOrEmpty(Searchcondition.CustomerOrderNumber))
        //    {
        //        IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
        //        if (Searchcondition.CustomerOrderNumber.IndexOf("\n") > 0)
        //        {
        //            customerOrderNumbers = Searchcondition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
        //        }
        //        if (Searchcondition.CustomerOrderNumber.IndexOf(',') > 0)
        //        {
        //            customerOrderNumbers = Searchcondition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
        //        }

        //        if (customerOrderNumbers != null && customerOrderNumbers.Any())
        //        {
        //            customerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c));
        //        }

        //        if (customerOrderNumbers != null && customerOrderNumbers.Any())
        //        {

        //            sb.Append(" and CustomerOrderNumber in ( ");
        //            foreach (string s in customerOrderNumbers)
        //            {
        //                sb.Append("'").Append(s).Append("',");
        //            }
        //            sb.Remove(sb.Length - 1, 1);
        //            sb.Append(" ) ");
        //        }
        //        else
        //        {
        //            sb.Append(" and CustomerOrderNumber like '%" + Searchcondition.CustomerOrderNumber.Trim() + "%' ");
        //        }
        //    }
        //    //运单状态
        //    if (Searchcondition.PODStateID != null && Searchcondition.PODStateID != 0)
        //    {
        //        sb.Append(" and PODStateID=" + Searchcondition.PODStateID + " ");
        //    }
        //    //客户
        //    if (Searchcondition.UserType == 2)
        //    {
        //        if (Searchcondition.CustomerID == null || Searchcondition.CustomerID == 0)
        //        {
        //            sb.Append(" and CustomerID in (");
        //            foreach (long i in Searchcondition.CustomerIDs)
        //            {
        //                sb.Append(i.ToString());
        //                sb.Append(",");
        //            }
        //            sb.Remove(sb.Length - 1, 1);
        //            sb.Append(") ");
        //        }
        //        else
        //        {
        //            sb.Append(" and CustomerID=" + Searchcondition.CustomerID + " ");
        //        }
        //    }
        //    else
        //    {
        //        if (Searchcondition.CustomerID != null && Searchcondition.CustomerID != 0)
        //        {
        //            sb.Append(" and CustomerID=" + Searchcondition.CustomerID + " ");
        //        }
        //    }
        //    //承运商
        //    if (Searchcondition.ShipperIDIsNull)
        //    {
        //        sb.Append(" and (ShipperID is null or ShipperID=0) ");
        //    }
        //    else
        //    {
        //        if (Searchcondition.ShipperID != null && Searchcondition.ShipperID != 0)
        //        {
        //            sb.Append(" and ShipperID=" + Searchcondition.ShipperID + " ");
        //        }
        //    }
        //    //运输类型
        //    if (Searchcondition.ShipperTypeID != null && Searchcondition.ShipperTypeID != 0)
        //    {
        //        sb.Append(" and ShipperTypeID=" + Searchcondition.ShipperTypeID + " ");
        //    }
        //    //运单类型
        //    if (Searchcondition.PODTypeID != null && Searchcondition.PODTypeID != 0)
        //    {
        //        sb.Append(" and PODTypeID=" + Searchcondition.PODTypeID + " ");
        //    }
        //    //整车/零担
        //    if (Searchcondition.TtlOrTplID != null && Searchcondition.TtlOrTplID != 0)
        //    {
        //        sb.Append(" and TtlOrTplID=" + Searchcondition.TtlOrTplID + " ");
        //    }
        //    //发货日期
        //    if (Searchcondition.ActualDeliveryDate.HasValue)
        //    {
        //        sb.Append(" and ActualDeliveryDate >= '" + Searchcondition.ActualDeliveryDate.Value.DateTimeToString() + "' ");
        //    }

        //    if (Searchcondition.EndActualDeliveryDate.HasValue)
        //    {
        //        sb.Append(" and ActualDeliveryDate <= '" + Searchcondition.EndActualDeliveryDate.Value.DateTimeToString() + " 23:59' ");
        //    }

        //    #endregion

        //    return sb.ToString();
        //}

    }
}