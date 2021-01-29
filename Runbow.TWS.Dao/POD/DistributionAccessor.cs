using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.MessageContracts.POD.AKZO;
using System.Data;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts;
using System.Data.SqlClient;
using Runbow.TWS.MessageContracts.POD.Adidas;
using Runbow.TWS.Entity.POD;
using Runbow.TWS.Entity.POD.Distribution;

namespace Runbow.TWS.Dao.POD
{
    public class DistributionAccessor : BaseAccessor
    {
        public IEnumerable<PodToDistribution> QueryOrOperatePod(PodDistribution condition, long projectID, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenQueryPodWhere(condition, projectID);
            int tempRowCount = 0;
            DbParam[] dbParams=null;
            DataTable dt = null;
           
            if (!condition.IsPaging)
            {
                dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                 new DbParam("@IsintDistribution", DbType.Int32, condition.IsintDistribution, ParameterDirection.Input),

            };
                dt = this.ExecuteDataTable("Proc_QueryPodDistribution", dbParams);
                rowCount = 0;
            }
            else
            {
                dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@IsintDistribution", DbType.Int32, condition.IsintDistribution, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                dt = this.ExecuteDataTable("Proc_QueryPodsDistribution", dbParams);
                rowCount = (int)dbParams[4].Value;
            }
            //PodToDistribution podtodis=new PodToDistribution();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    podtodis.Decimal10 = dt.Rows[i]["Decimal10"].ObjectToDecimal();
            //    podtodis.Decimal11 = dt.Rows[i]["Decimal11"].ObjectToDecimal();
            //}
                return dt.ConvertToEntityCollection<PodToDistribution>();
            //return podtodis;
        }
        //public IEnumerable<Pod> QueryPodWithNoPaging(PodDistribution condition, long projectID)
        //{
        //    //if (condition.UserType == 2 && !condition.CustomerIDs.Any())
        //    //{
        //    //    return Enumerable.Empty<Pod>();
        //    //}

        //    //string endCities = string.Empty;
        //    //if (condition.EndCities != null && condition.EndCities.IndexOf(',') > 0)
        //    //{
        //    //    using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
        //    //    {
        //    //        DataTable dtable = new DataTable();
        //    //        SqlCommand cmd = new SqlCommand("Proc_GetReginAndSunRegionsByRegionIDs", conn);
        //    //        cmd.CommandTimeout = 180;
        //    //        cmd.CommandType = CommandType.StoredProcedure;
        //    //        cmd.Parameters.AddWithValue("@EndCityIDs", condition.EndCities.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
        //    //        cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
        //    //        SqlDataAdapter Adp = new SqlDataAdapter(cmd);
        //    //        Adp.Fill(dtable);
        //    //        for (int i = 0; i < dtable.Rows.Count; i++)
        //    //        {
        //    //            endCities += dtable.Rows[i][0].ToString() + ",";
        //    //        }
        //    //        endCities = endCities.Substring(0, endCities.Length - 1);
        //    //    }
        //    //}


        //    string sqlWhere = this.GenQueryPodWhere(condition, projectID);
        //    DbParam[] dbParams = new DbParam[]{
        //        new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
        //    };

        //    return this.ExecuteDataTable("Proc_QueryPodsWithNoPaging", dbParams).ConvertToEntityCollection<Pod>();
        //}
        public IEnumerable<DbToExcel> DbToExcels(PodDistribution condition)
        {
            string sqlWhere = this.GenQueryPodWhere(condition, 1);
            DbParam[] dbParams = null;
            DataTable dt = null;
            if (condition.IsExport)
            {
                dbParams = new DbParam[]{
                  new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
                  };
                dt = this.ExecuteDataTable("Proc_PodDbToExcel", dbParams);

            }
            return dt.ConvertToEntityCollection<DbToExcel>();
        }
        private string GenQueryPodWhere(PodDistribution condition, long projectID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where CustomerName='城市配送' ");

            #region

            if (!condition.IsPaging)
            {
                sb.Append(" and POD.PODStateID=2 ");
            }
            if (condition.ActualDeliveryDate.HasValue)
            {
                sb.Append(" and ActualDeliveryDate >= '" + condition.ActualDeliveryDate.Value.DateTimeToString() + "' ");
            }
            if (condition.EndActualDeliveryDate.HasValue)
            {
                sb.Append(" and ActualDeliveryDate <= '" + condition.EndActualDeliveryDate.Value.DateTimeToString()).Append(" 23:59' ");
            }
            //if (condition.CreateTime.HasValue)
            //{
            //    sb.Append(" and CreateTime >= '" + condition.CreateTime.Value.DateTimeToString() + "' ");
            //}

            if (!string.IsNullOrEmpty(condition.podID))
            {
                 IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (condition.podID.IndexOf("\n") > 0)
                {
                    systemNumbers = condition.podID.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.podID.IndexOf(',') > 0)
                {
                    systemNumbers = condition.podID.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" and POD.ID in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and POD.ID like '%" + condition.podID.Trim() + "%' ");
                }
            }
            if (!string.IsNullOrEmpty(condition.Str1))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (condition.Str1.IndexOf("\n") > 0)
                {
                    systemNumbers = condition.Str1.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.Str1.IndexOf(',') > 0)
                {
                    systemNumbers = condition.Str1.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" and POD.Str1 in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and POD.Str1 like '%" + condition.Str1.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(condition.Str4))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (condition.Str4.IndexOf("\n") > 0)
                {
                    systemNumbers = condition.Str4.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.Str4.IndexOf(',') > 0)
                {
                    systemNumbers = condition.Str4.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" and POD.Str4 in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and POD.Str4 like '%" + condition.Str4.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(condition.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (condition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = condition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = condition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {
                    customerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (customerOrderNumbers != null && customerOrderNumbers.Any())
                {

                    sb.Append(" and POD.CustomerOrderNumber in ( ");
                    foreach (string s in customerOrderNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and POD.CustomerOrderNumber like '%" + condition.CustomerOrderNumber.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(condition.Str2))
            {
                sb.Append(" and POD.Str2 like '%" + condition.Str2.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str3))
            {
                sb.Append(" and POD.Str3 like '%" + condition.Str3.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str5))
            {
                sb.Append(" and POD.Str5 like '%" + condition.Str5.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str6))
            {
                sb.Append(" and POD.Str6 like '%" + condition.Str6.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str7))
            {
                sb.Append(" and POD.Str7 like '%" + condition.Str7.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str8))
            {
                sb.Append(" and POD.Str8 like '%" + condition.Str8.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str9))
            {
                sb.Append(" and POD.Str9 like '%" + condition.Str9.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str10))
            {
                sb.Append(" and POD.Str10 like '%" + condition.Str10.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str11))
            {
                sb.Append(" and POD.Str11 = '" + condition.Str11.Trim() + "' ");
            }

            if (!string.IsNullOrEmpty(condition.Str12))
            {
                sb.Append(" and POD.Str12 = '" + condition.Str12.Trim() + "' ");
            }

            if (!string.IsNullOrEmpty(condition.Str13))
            {
                sb.Append(" and POD.Str13 = '" + condition.Str13.Trim() + "' ");
            }

            if (!string.IsNullOrEmpty(condition.Str14))
            {
                sb.Append(" and POD.Str14 like '%" + condition.Str14.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str15))
            {
                sb.Append(" and POD.Str15 like '%" + condition.Str15.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str16))
            {
                sb.Append(" and POD.Str16 = '" + condition.Str16.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(condition.EndCityName))
            {
                sb.Append(" and POD.EndCityName like '%" + condition.EndCityName.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(condition.Str17))
            {
                sb.Append(" and POD.Str17 like '%" + condition.Str17.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str18))
            {
                sb.Append(" and POD.Str18 like '%" + condition.Str18.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str19))
            {
                sb.Append(" and POD.Str19 like '%" + condition.Str19.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str20))
            {
                sb.Append(" and POD.Str20 like '%" + condition.Str20.Trim() + "%' ");
            }

    

            //if (condition.IsSettledForCustomer.HasValue)
            //{
            //    sb.Append(" and IsSettledForCustomer=" + (condition.IsSettledForCustomer.Value ? "1" : "0") + " ");
            //}

            if (!string.IsNullOrEmpty(condition.FeeStr1))
            {
                sb.Append(" and PodFee.Str1 = '" + condition.FeeStr1.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(condition.FeeStr2))
            {
                sb.Append(" and PodFee.Str2 = '" + condition.FeeStr2.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(condition.FeeStr3))
            {
                sb.Append(" and PodFee.Str3 = '" + condition.FeeStr3.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(condition.FeeStr4))
            {
                sb.Append(" and PodFee.Str4 = '" + condition.FeeStr4.Trim() + "' ");
            }
            if (!string.IsNullOrEmpty(condition.FeeStr5))
            {
                sb.Append(" and PodFee.Str5 = '" + condition.FeeStr5.Trim() + "' ");
            }
            #endregion

            return sb.ToString();
        }

        public void SettlePodsDistribution(IEnumerable<SettlePodDistribution> SettledPodsDistribution)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_SettlePodsDistribution", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SettledPodsDistribution", SettledPodsDistribution.Select(p => new SettledPodDistributionToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void selectPodFee(string podID)
        {
            string DeleteSql = DeleteSqlWhere(podID);
             DbParam[] dbParams=null;
             dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, DeleteSql, ParameterDirection.Input)
             };
             this.ExecuteNoQuery("[Proc_DeletePodFee]", dbParams);
        }
        public string selectPodFee()
        {
            DbParam[] dbParams = new DbParam[]{
                 new DbParam("@datetime", DbType.DateTime, DateTime.Now.YearMonthDay(), ParameterDirection.Input)
             };
            return this.ExecuteScalar("Proc_startBatchNumberDistribution", dbParams).ToString();
        }
        private string DeleteSqlWhere(string podID)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(podID))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (podID.IndexOf("\n") > 0)
                {
                    systemNumbers = podID.Split('\n').Select(s => { return s.Trim(); });
                }
                if (podID.IndexOf(',') > 0)
                {
                    systemNumbers = podID.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" where PodFee.podID in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" where PodFee.podID ='" + podID.Trim() + "' ");
                }
            }
            return sb.ToString();
        }
    }
}
