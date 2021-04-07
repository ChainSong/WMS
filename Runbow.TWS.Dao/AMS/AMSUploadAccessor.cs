using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Runbow.TWS.Dao
{
    public class AMSUploadAccessor : BaseAccessor
    {
        /// <summary>
        /// 获取2月内已上传回单
        /// </summary>
        public IEnumerable<AMSUpload> GetAMSUpload(string names, string projects)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetAMSUpload", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ids", names);
                cmd.Parameters.AddWithValue("@project", projects);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                conn.Open();
                IList<AMSUpload> returnAMSUploads = new List<AMSUpload>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnAMSUploads.Add(
                        new AMSUpload()
                        {
                            ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                            ProjectName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            FileName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            FilePath = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        });
                }

                return returnAMSUploads;
            }
        }

        /// <summary>
        /// 获取2月内现有回单
        /// </summary>
        public IEnumerable<AMSUpload> GetAMSUploadOrPODInfo()
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetPODInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                IList<AMSUpload> returnAMSUploads = new List<AMSUpload>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnAMSUploads.Add(
                        new AMSUpload()
                        {
                            ProjectName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            ProjectID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                            ID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            FileType = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        });
                }

                return returnAMSUploads;
            }
        }

        /// <summary>
        /// 获取已上传附件运单记录
        /// </summary>
        public IEnumerable<Attachment> GetAttachmentOrAMS()
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetAttachmentOrAMS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                IList<Attachment> returnAMSUploads = new List<Attachment>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnAMSUploads.Add(
                        new Attachment()
                        {
                            GroupID = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            Url = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Creator = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            ActualNameInServer = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        });
                }

                return returnAMSUploads;
            }
        }

        /// <summary>
        /// 手动同步回单
        /// </summary>
        public void InsertAMSUpload()
        {
            base.ExecuteNoQuery("Proc_InsertAMSUpload");
        }

        /// <summary>
        /// 验证通过修改验证状态
        /// </summary>
        public void UpdateAMSUploadStatus(string ids)
        {
            ids = ids.ToString().TrimEnd(',');
            DbParam[] dbParams = new DbParam[] { new DbParam("@ids", DbType.String, ids, ParameterDirection.Input) };
            base.ExecuteNoQuery("Proc_UpdateAMSUploadStatus", dbParams);
        }

        /// <summary>
        /// 新增回单
        /// </summary>
        public IEnumerable<AMSUpload> AddAttachments(IEnumerable<AMSUpload> amsupload)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                IList<AMSUpload> result = new List<AMSUpload>();
                SqlCommand cmd = new SqlCommand("Proc_AddAMSUpload", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AMSUploads", amsupload == null ? null : amsupload.Select(attachments => new AMSUploadToDb(attachments)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(
                        new AMSUpload()
                        {
                            ID = reader.GetInt64(0),
                            //FileName = reader.GetString(1),
                            //FileType = reader.GetString(2),
                            //ServerName = reader.GetString(3),
                            //FilePath = reader.GetString(4),
                            //ProjectID = reader.GetInt64(5),
                            //ProjectName = reader.GetString(6),
                            //OrderNo = reader.GetString(7),
                            //Creator = reader.GetString(8),
                            //CreateTime = reader.GetDateTime(9),
                            //Updator = reader.GetString(10),
                            //UpdateTime = reader.GetDateTime(11),
                            //Status = reader.GetBoolean(12)
                        });
                }

                return result;
            }

        }

        /// <summary>
        /// 对于装箱单方面的查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="StateID">查询是否有装箱单</param>
        /// <param name="Customers">控制查询权限</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public IEnumerable<AMSUpload> GetQueryAttachments(AMSSearchCondition SearchCondition, string Customers, int PageIndex,
            int PageSize, out int RowCount)
        {

            string sqlWhere = this.GenQueryAttachmentSql(SearchCondition, Customers);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_QueryAMSupload", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<AMSUpload>();
        }
        /// <summary>
        /// 装箱单页面查询post  
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="Customers"></param>
        /// <returns></returns>
        public IEnumerable<AMSUpload> GetBoxNumberAttachments(AMSSearchCondition SearchCondition, string Customers)
        {

            string sqlWhere = this.GenQueryAttachmentSql(SearchCondition, Customers);
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("Proc_GetBoxNumberAttachmentSql", dbParams);
            return dt.ConvertToEntityCollection<AMSUpload>();
        }

        /// <summary>
        /// 生成装箱单号
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<AMSUpload> AddAMSController(string sqlWhere)
        {
            string time = DateTime.Now.ToString("yyyyMMddhhmmss");

            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@time", DbType.String, time, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("proc_AddPackingLists", dbParams);
            return dt.ConvertToEntityCollection<AMSUpload>();
        }
        /// <summary>
        /// 查询装箱单号拼接sql
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="Customers"></param>
        /// <returns></returns>
        private string GenQueryAttachmentSql(AMSSearchCondition condition, string Customers)
        {
            StringBuilder sb = new StringBuilder();
            if (condition.CustomerID == null)
            {
                sb.Append("and ProjectName in(" + Customers + ")");
            }
            if (!string.IsNullOrEmpty(condition.Numbers))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (condition.Numbers.IndexOf("\n") > 0)
                {
                    numbers = condition.Numbers.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.Numbers.IndexOf(',') > 0)
                {
                    numbers = condition.Numbers.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and FileName in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and FileName  like '%" + condition.Numbers.Trim() + "%' ");
                }
            }
            if (condition.StateNumID.HasValue)
            {
                if (condition.StateNumID == 1)
                {
                    sb.Append("and status =1");
                }
                else
                {
                    sb.Append("and status =0");
                }
            }
            if (condition.StatUpLoadTime.HasValue)
            {
                sb.Append("and (Convert(date, CreateTime)>='" + condition.StatUpLoadTime.Value.ToString("yyyy-MM-dd") + "')");
            }

            if (condition.EndUpLoadTime.HasValue)
            {
                sb.Append("and (Convert (date, CreateTime)<='" + condition.EndUpLoadTime.Value.ToString("yyyy-MM-dd") + "')");

            }
            if (condition.CustomerID.HasValue)
            {
                sb.Append("and ProjectID=" + (int)condition.CustomerID.Value);

            }
            if (!string.IsNullOrEmpty(condition.BoxNumber))
            {
                IEnumerable<string> boxnumber = Enumerable.Empty<string>();
                if (condition.BoxNumber.IndexOf("\n") > 0)
                {
                    boxnumber = condition.BoxNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.BoxNumber.IndexOf(',') > 0)
                {
                    boxnumber = condition.BoxNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (boxnumber != null && boxnumber.Any())
                {
                    boxnumber = boxnumber.Where(c => !string.IsNullOrEmpty(c));
                }

                if (boxnumber != null && boxnumber.Any())
                {
                    sb.Append(" and OrderNo in ( ");
                    foreach (string s in boxnumber)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append("and OrderNo  like '%" + condition.BoxNumber.Trim() + "%' ");
                }
            }
            else
            {
                if (condition.StateID.HasValue)
                {
                    if (condition.StateID == 0)
                    {
                        sb.Append("and OrderNo=''");
                    }
                    else if (condition.StateID == 1)
                    {
                        sb.Append("and OrderNo !=''");
                    }
                }
            }
            return sb.ToString();
        }

        public AMSUpload GetAmsUploadByID(long id)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input),
            };
            return this.ExecuteDataTable("Proc_GetAmsUploadByID", dbParams).ConvertToEntity<AMSUpload>();
        }

        /// <summary>
        /// 成品订单查询
        /// </summary>
        public IEnumerable<WMS_Package> GetWMS_PackageByCondition(WMS_Package SearchCondition, string Customers, int PageIndex,
            int PageSize, out int RowCount)
        {

            string sqlWhere = "'";//this.GenQueryAttachmentSql(SearchCondition, Customers);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_GetWMS_PackageByCondition", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<WMS_Package>();
        }

    }
}
