using System.Collections.Generic;
using System.Data;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Text;

namespace Runbow.TWS.Dao
{
    public class BaiXingAccessor : BaseAccessor
    {
        /// <summary>
        /// 获取百姓网订单信息
        /// </summary>
        public IEnumerable<Pod> GetBaiXingPod(string CustomerOrderNumber)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerOrderNumber", DbType.String, CustomerOrderNumber, ParameterDirection.Input),
            };
            return this.ExecuteDataTable("Proc_GetBaiXingPod", dbParams).ConvertToEntityCollection<Pod>();
        }

        /// <summary>
        /// 根据运单号获取订单跟踪信息
        /// </summary>
        /// <param name="bill_id"></param>
        /// <returns></returns>
        public IEnumerable<PodTrack> GetPodTracksByCustomerOrderNumber(string bill_id)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_GetPodTracksByCustomerOrderNumber", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerOrderNumber", bill_id);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                conn.Open();
                IList<PodTrack> podTracks = new List<PodTrack>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    podTracks.Add(new PodTrack()
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt64(0),
                        PodID = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                        SystemNumber = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        CustomerOrderNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Creator = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        CreateTime = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                        Str1 = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        Str2 = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Str3 = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Str4 = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                        Str5 = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        Str6 = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                        Str7 = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                        Str8 = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        Str9 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        Str10 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                        DateTime1 = reader.IsDBNull(16) ? null : (DateTime?)reader.GetDateTime(16),
                        DateTime2 = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        DateTime3 = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                        Str11 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                        Str12 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                        Str13 = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                        Str14 = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                        Str15 = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                        DateTime4 = reader.IsDBNull(24) ? null : (DateTime?)reader.GetDateTime(24),
                        DateTime5 = reader.IsDBNull(25) ? null : (DateTime?)reader.GetDateTime(25),
                        DateTime6 = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                        DateTime7 = reader.IsDBNull(27) ? null : (DateTime?)reader.GetDateTime(27),
                        DateTime8 = reader.IsDBNull(28) ? null : (DateTime?)reader.GetDateTime(28),
                        DateTime9 = reader.IsDBNull(29) ? null : (DateTime?)reader.GetDateTime(29),
                        DateTime10 = reader.IsDBNull(30) ? null : (DateTime?)reader.GetDateTime(30)
                    });
                }

                return podTracks;
            }
        }

        /// <summary>
        /// 百姓网订单导入顺丰快递信息
        /// </summary>
        public IEnumerable<PodKey> AddPods_BX(IEnumerable<Pod> pods)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddPods_BX", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PodData", pods.Select(p => new PodToDb(p)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.CommandTimeout = 180;
                conn.Open();
                IList<PodKey> returnPods = new List<PodKey>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    returnPods.Add(
                        new PodKey()
                        {
                            SystemNumber = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            CustomerOrderNumber = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            PODStateName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            ShipperName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            ShipperTypeName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            StartCityName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            EndCityName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            PODTypeName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                            TtlOrTplName = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                            ActualDeliveryDate = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                            BoxNumber = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                            GoodsNumber = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                            Weight = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                            Volume = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        });
                }

                return returnPods;
            }
        }


        /// <summary>
        /// 获取百姓网报表日报表
        /// </summary>
        public DataTable GetBaiXingPodImport()
        {
            return this.ExecuteDataTable("Proc_GetBaiXingPod");
        }


        /// <summary>
        /// 百姓网订单报表查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="projectID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<PodWithAttachment> QueryPod(PodSearchCondition condition, long projectID, int pageIndex, int pageSize, out int rowCount)
        {
            if (condition.UserType == 2 && !condition.CustomerIDs.Any())
            {
                rowCount = 0;
                return Enumerable.Empty<PodWithAttachment>();
            }
            string endCities = string.Empty;
            string endCitiesRuleArea = string.Empty;
            //if (condition.EndCities != null && condition.EndCities.IndexOf(',') > 0)
            //{
            //    endCities = endCitiesId(condition.EndCities);
            //}
            //if (!string.IsNullOrEmpty(condition.RuleArea) && condition.RuleArea.IndexOf(',') > 0)
            //{
            //    endCitiesRuleArea = endCitiesId(condition.RuleArea);
            //}

            string sqlWhere = this.GenQueryPodWhere(condition, projectID, endCities, endCitiesRuleArea);
            int tempRowCount = 0;

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_QueryPods", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<PodWithAttachment>();
        }


        private string GenQueryPodWhere(PodSearchCondition condition, long projectID, string endCities, string endCitiesRuleArea)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ProjectID=" + projectID + " ");

            //百姓网订单排除快递单号
            if (condition.CustomerID == 40)
            {
                sb.Append(" and [Type]='2' ");
            }

            #region
            if (condition.StartCityID != null && condition.StartCityID != 0)
            {
                sb.Append(" and StartCityID=" + condition.StartCityID + " ");
            }

            if (condition.EndCityID != null && condition.EndCityID != 0)
            {
                sb.Append(" and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + condition.EndCityID + ")) ");
            }
            else
            {
                if (!string.IsNullOrEmpty(endCities))
                {
                    sb.Append(" and EndCityID in (").Append(endCities).Append(") ");
                }
            }

            if (condition.UserType == 1)
            {
                sb.Append(" and PODStateID>2 ");
            }
            sb.Append(" and CustomerID=40 ");
            //if (condition.UserType == 2)
            //{
            //    if (condition.CustomerID == null || condition.CustomerID == 0)
            //    {
            //        sb.Append(" and CustomerID in (");
            //        foreach (long i in condition.CustomerIDs)
            //        {
            //            sb.Append(i.ToString());
            //            sb.Append(",");
            //        }
            //        sb.Remove(sb.Length - 1, 1);
            //        sb.Append(") ");
            //    }
            //    else
            //    {
            //        sb.Append(" and CustomerID=" + condition.CustomerID + " ");
            //    }
            //}
            //else
            //{
            //    if (condition.CustomerID != null && condition.CustomerID != 0)
            //    {
            //        sb.Append(" and CustomerID=" + condition.CustomerID + " ");
            //    }
            //}

            if (condition.Types != null && condition.Types.Any())
            {
                sb.Append(" and Type in (");
                foreach (int type in condition.Types)
                {
                    sb.Append(type.ToString()).Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(") ");
            }

            if (condition.PODStateID != null && condition.PODStateID != 0)
            {
                sb.Append(" and PODStateID=" + condition.PODStateID + " ");
            }

            if (condition.TtlOrTplID != null && condition.TtlOrTplID != 0)
            {
                sb.Append(" and TtlOrTplID=" + condition.TtlOrTplID + " ");
            }

            if (condition.PODTypeID != null && condition.PODTypeID != 0)
            {
                sb.Append(" and PODTypeID=" + condition.PODTypeID + " ");
            }

            if (condition.ShipperTypeID != null && condition.ShipperTypeID != 0)
            {
                sb.Append(" and ShipperTypeID=" + condition.ShipperTypeID + " ");
            }

            if (condition.ShipperIDIsNull)
            {
                sb.Append(" and (ShipperID is null or ShipperID=0) ");
            }
            else
            {
                if (condition.ShipperID != null && condition.ShipperID != 0)
                {
                    sb.Append(" and ShipperID=" + condition.ShipperID + " ");
                }
            }

            if (condition.BoxNumber != null && condition.BoxNumber != 0)
            {
                sb.Append(" and BoxNumber=" + condition.BoxNumber + " ");
            }

            if (condition.Weight != null && condition.Weight != 0)
            {
                sb.Append(" and Weight=" + condition.Weight + " ");
            }

            if (condition.GoodsNumber != null && condition.GoodsNumber != 0)
            {
                sb.Append(" and GoodsNumber=" + condition.GoodsNumber + " ");
            }

            if (condition.Volume != null && condition.Volume != 0)
            {
                sb.Append(" and BoxNumber=" + condition.Volume + " ");
            }

            if (!string.IsNullOrEmpty(condition.Creator))
            {
                sb.Append(" and Creator='" + condition.Creator + "' ");
            }

            if (condition.ActualDeliveryDate.HasValue)
            {
                sb.Append(" and ActualDeliveryDate >= '" + condition.ActualDeliveryDate.Value.DateTimeToString() + "' ");
            }

            if (condition.EndActualDeliveryDate.HasValue)
            {
                sb.Append(" and ActualDeliveryDate <= '" + condition.EndActualDeliveryDate.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.CreateTime.HasValue)
            {
                sb.Append(" and CreateTime >= '" + condition.CreateTime.Value.DateTimeToString() + "' ");
            }
            if (condition.StartCreateTime.HasValue)
            {
                sb.Append(" and CreateTime >= '" + condition.StartCreateTime.Value.DateTimeToString() + "' ");
            }

            if (condition.EndCreateTime.HasValue)
            {
                sb.Append(" and CreateTime <= '" + condition.EndCreateTime.Value.DateTimeToString() + " 23:59' ");
            }


            if (!string.IsNullOrEmpty(condition.SystemNumber))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (condition.SystemNumber.IndexOf("\n") > 0)
                {
                    systemNumbers = condition.SystemNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.SystemNumber.IndexOf(',') > 0)
                {
                    systemNumbers = condition.SystemNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" and SystemNumber in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and SystemNumber like '%" + condition.SystemNumber.Trim() + "%' ");
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

                    sb.Append(" and CustomerOrderNumber in ( ");
                    foreach (string s in customerOrderNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and CustomerOrderNumber like '%" + condition.CustomerOrderNumber.Trim() + "%' ");
                }
            }


            if (!string.IsNullOrEmpty(condition.Str1))
            {
                IEnumerable<string> Str1 = Enumerable.Empty<string>();
                if (condition.Str1.IndexOf("\n") > 0)
                {
                    Str1 = condition.Str1.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.Str1.IndexOf(',') > 0)
                {
                    Str1 = condition.Str1.Split(',').Select(s => { return s.Trim(); });
                }

                if (Str1 != null && Str1.Any())
                {
                    Str1 = Str1.Where(c => !string.IsNullOrEmpty(c));
                }

                if (Str1 != null && Str1.Any())
                {
                    sb.Append(" and CustomerOrderNumber in (SELECT DISTINCT CustomerOrderNumber FROM dbo.POD WHERE Str1 in(");
                    foreach (string s in Str1)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ))");
                }
                else
                {
                    sb.Append(" and CustomerOrderNumber IN(SELECT DISTINCT CustomerOrderNumber FROM dbo.POD WHERE Str1 LIKE '%" + condition.Str1 + "%')");
                }
            }

            //if (!string.IsNullOrEmpty(condition.Str1))
            //{
            //    //sb.Append(" and Str1 like '%" + condition.Str1 + "%' ");
            //    sb.Append(" and CustomerOrderNumber IN(SELECT DISTINCT CustomerOrderNumber FROM dbo.POD WHERE Str1 LIKE '%" + condition.Str1 + "%')");
            //}

            if (!string.IsNullOrEmpty(condition.Str2))
            {
                sb.Append(" and Str2 like '%" + condition.Str2 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str3))
            {
                sb.Append(" and Str3 like '%" + condition.Str3 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str4))
            {
                sb.Append(" and Str4 like '%" + condition.Str4 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str5))
            {
                sb.Append(" and Str5 like '%" + condition.Str5 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str6))
            {
                sb.Append(" and Str6 like '%" + condition.Str6 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str7))
            {
                sb.Append(" and Str7 like '%" + condition.Str7 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str8))
            {
                sb.Append(" and Str8 like '%" + condition.Str8 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str9))
            {
                sb.Append(" and Str9 like '%" + condition.Str9 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str10))
            {
                sb.Append(" and Str10 like '%" + condition.Str10 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str11))
            {
                sb.Append(" and Str11 like '%" + condition.Str11 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str12))
            {
                sb.Append(" and Str12 like '%" + condition.Str12 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str13))
            {
                sb.Append(" and Str13 like '%" + condition.Str13 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str14))
            {
                sb.Append(" and Str14 like '%" + condition.Str14 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str15))
            {
                sb.Append(" and Str15 like '%" + condition.Str15 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str16))
            {
                sb.Append(" and Str16 like '%" + condition.Str16 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str17))
            {
                sb.Append(" and Str17 like '%" + condition.Str17 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str18))
            {
                sb.Append(" and Str18 like '%" + condition.Str18 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str19))
            {
                sb.Append(" and Str19 like '%" + condition.Str19 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str20))
            {
                sb.Append(" and Str20 like '%" + condition.Str20 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str21))
            {
                sb.Append(" and Str21 like '%" + condition.Str21 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str22))
            {
                sb.Append(" and Str22 like '%" + condition.Str22 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str23))
            {
                sb.Append(" and Str23 like '%" + condition.Str23 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str24))
            {
                sb.Append(" and Str24 like '%" + condition.Str24 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str25))
            {
                sb.Append(" and Str25 like '%" + condition.Str25 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str26))
            {
                sb.Append(" and Str26 like '%" + condition.Str26 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str27))
            {
                sb.Append(" and Str27 like '%" + condition.Str27 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str28))
            {
                sb.Append(" and Str28 like '%" + condition.Str28 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str29))
            {
                sb.Append(" and Str29 like '%" + condition.Str29 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str30))
            {
                sb.Append(" and Str30 like '%" + condition.Str30 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str31))
            {
                sb.Append(" and Str31 like '%" + condition.Str31 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str32))
            {
                sb.Append(" and Str32 like '%" + condition.Str32 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str33))
            {
                sb.Append(" and Str33 like '%" + condition.Str33 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str34))
            {
                sb.Append(" and Str34 like '%" + condition.Str34 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str35))
            {
                sb.Append(" and Str35 like '%" + condition.Str35 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str36))
            {
                sb.Append(" and Str36 like '%" + condition.Str36 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str37))
            {
                sb.Append(" and Str37 like '%" + condition.Str37 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str38))
            {
                sb.Append(" and Str38 like '%" + condition.Str38 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str39))
            {
                sb.Append(" and Str39 like '%" + condition.Str39 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str40))
            {
                sb.Append(" and Str40 like '%" + condition.Str40 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str41))
            {
                sb.Append(" and Str41 like '%" + condition.Str41 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str42))
            {
                sb.Append(" and Str42 like '%" + condition.Str42 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str43))
            {
                sb.Append(" and Str43 like '%" + condition.Str43 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str44))
            {
                sb.Append(" and Str44 like '%" + condition.Str44 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str45))
            {
                sb.Append(" and Str45 like '%" + condition.Str45 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str46))
            {
                sb.Append(" and Str46 like '%" + condition.Str46 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str47))
            {
                sb.Append(" and Str47 like '%" + condition.Str47 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str48))
            {
                sb.Append(" and Str48 like '%" + condition.Str48 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str49))
            {
                sb.Append(" and Str49 like '%" + condition.Str49 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str50))
            {
                sb.Append(" and Str50 like '%" + condition.Str50 + "%' ");
            }

            if (condition.DateTime1.HasValue)
            {
                sb.Append(" and DateTime1 >= '" + condition.DateTime1.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime1.HasValue)
            {
                sb.Append(" and DateTime1 <= '" + condition.EndDateTime1.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime2.HasValue)
            {
                sb.Append(" and DateTime2 >= '" + condition.DateTime2.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime2.HasValue)
            {
                sb.Append(" and DateTime2 <= '" + condition.EndDateTime2.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime3.HasValue)
            {
                sb.Append(" and DateTime3 >= '" + condition.DateTime3.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime3.HasValue)
            {
                sb.Append(" and DateTime3 <= '" + condition.EndDateTime3.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime4.HasValue)
            {
                sb.Append(" and DateTime4 >= '" + condition.DateTime4.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime4.HasValue)
            {
                sb.Append(" and DateTime4 <= '" + condition.EndDateTime4.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime5.HasValue)
            {
                sb.Append(" and DateTime5 >= '" + condition.DateTime5.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime5.HasValue)
            {
                sb.Append(" and DateTime5 <= '" + condition.EndDateTime5.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime6.HasValue)
            {
                sb.Append(" and DateTime6 >= '" + condition.DateTime6.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime6.HasValue)
            {
                sb.Append(" and DateTime6 <= '" + condition.EndDateTime6.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime7.HasValue)
            {
                sb.Append(" and DateTime7 >= '" + condition.DateTime7.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime7.HasValue)
            {
                sb.Append(" and DateTime7 <= '" + condition.EndDateTime7.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime8.HasValue)
            {
                sb.Append(" and DateTime8 >= '" + condition.DateTime8.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime8.HasValue)
            {
                sb.Append(" and DateTime8 <= '" + condition.EndDateTime8.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime9.HasValue)
            {
                sb.Append(" and DateTime9 >= '" + condition.DateTime9.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime9.HasValue)
            {
                sb.Append(" and DateTime9 <= '" + condition.EndDateTime9.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime10.HasValue)
            {
                sb.Append(" and DateTime10 >= '" + condition.DateTime10.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime10.HasValue)
            {
                sb.Append(" and DateTime10 <= '" + condition.EndDateTime10.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime11.HasValue)
            {
                sb.Append(" and DateTime11 >= '" + condition.DateTime11.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime11.HasValue)
            {
                sb.Append(" and DateTime11 <= '" + condition.EndDateTime11.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime12.HasValue)
            {
                sb.Append(" and DateTime12 >= '" + condition.DateTime12.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime12.HasValue)
            {
                sb.Append(" and DateTime12 <= '" + condition.EndDateTime12.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime13.HasValue)
            {
                sb.Append(" and DateTime13 >= '" + condition.DateTime13.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime13.HasValue)
            {
                sb.Append(" and DateTime13 <= '" + condition.EndDateTime13.Value.DateTimeToString() + " 23:59' ");
            }

            if (condition.DateTime15.HasValue)
            {
                sb.Append(" and DateTime15 >= '" + condition.DateTime14.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime15.HasValue)
            {
                sb.Append(" and DateTime15 <= '" + condition.EndDateTime14.Value.DateTimeToString() + " 23:59' ");
            }

            //for Tianjin Hub 查询 
            if (condition.DateTime14.HasValue)
            {
                sb.Append(" AND CASE CustomerID WHEN 1 THEN DateTime4 WHEN 8 THEN DateTime6 WHEN 2 THEN DateTime2 WHEN 7 THEN DateTime1 END >= '" + condition.DateTime14.Value.DateTimeToString() + "' ");
            }

            if (condition.EndDateTime14.HasValue)
            {
                sb.Append(" AND CASE CustomerID WHEN 1 THEN DateTime4 WHEN 8 THEN DateTime6 WHEN 2 THEN DateTime2 WHEN 7 THEN DateTime1 END <= '" + condition.EndDateTime14.Value.DateTimeToString() + "  23:59'  ");
            }

            if (condition.IsSettledForCustomer.HasValue)
            {
                sb.Append(" and IsSettledForCustomer=" + (condition.IsSettledForCustomer.Value ? "1" : "0") + " ");
            }

            if (condition.IsSettledForShipper.HasValue)
            {
                sb.Append(" and IsSettledForShipper=" + (condition.IsSettledForShipper.Value ? "1" : "0") + " ");
            }

            if (condition.HasShortDial.HasValue)
            {
                sb.Append(" and HasShortDial=" + (condition.HasShortDial.Value ? "1" : "0") + " ");
            }

            if (condition.HasDistribution.HasValue)
            {
                sb.Append(" and HasDistribution=" + (condition.HasDistribution.Value ? "1" : "0") + " ");
            }

            if (condition.HasExpress.HasValue)
            {
                sb.Append(" and HasExpress=" + (condition.HasExpress.Value ? "1" : "0") + " ");
            }

            if (condition.PodMinStateID.HasValue)
            {
                sb.Append(" and PodStateID >= " + condition.PodMinStateID.Value + " ");
            }

            if (condition.HasAllocateShipper.HasValue)
            {
                if (condition.HasAllocateShipper.Value)
                {
                    sb.Append(" and (ShipperID != 0 or ShipperID IS NOT NULL) ");
                }
                else
                {
                    sb.Append(" and (ShipperID = 0 OR ShipperID IS NULL) ");
                }
            }
            if (condition.ID != 0)
            {
                sb.Append(" and ID = " + condition.ID + " ");
            }

            if (!string.IsNullOrEmpty(condition.RuleArea) && condition.RuleArea.Split(',').Count() == 1)
            {
                sb.Append(" and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + condition.RuleArea + ")) ");
            }
            else
            {
                if (!string.IsNullOrEmpty(endCitiesRuleArea))
                {
                    sb.Append(" and EndCityID in (").Append(endCitiesRuleArea).Append(") ");
                }
            }
            #endregion

            return sb.ToString();
        }


        /// <summary>
        /// 百姓网订单报表导出
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="projectID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public DataTable ExportAll(PodSearchCondition condition, long projectID, int pageIndex, int pageSize)
        {
            string endCities = string.Empty;
            string endCitiesRuleArea = string.Empty;

            string sqlWhere = this.ExportAllWhere(condition, projectID, endCities, endCitiesRuleArea);

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetBaiXingreportNew", dbParams);
            //rowCount = (int)dbParams[3].Value;
            //return dt.ConvertToEntityCollection<PodWithAttachment>();
        }


        private string ExportAllWhere(PodSearchCondition condition, long projectID, string endCities, string endCitiesRuleArea)
        {
            StringBuilder sb = new StringBuilder();
            //百姓网订单排除快递单号
            //if (condition.CustomerID == 40)
            //{
            //    sb.Append(" and [Type]='2' ");
            //}
            #region

            if (condition.StartCreateTime.HasValue)
            {
                sb.Append(" and CreateTime >= '" + condition.StartCreateTime.Value.DateTimeToString() + "' ");
            }

            if (condition.EndCreateTime.HasValue)
            {
                sb.Append(" and CreateTime <= '" + condition.EndCreateTime.Value.DateTimeToString() + " 23:59' ");
            }


            if (!string.IsNullOrEmpty(condition.SystemNumber))
            {
                IEnumerable<string> systemNumbers = Enumerable.Empty<string>();
                if (condition.SystemNumber.IndexOf("\n") > 0)
                {
                    systemNumbers = condition.SystemNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.SystemNumber.IndexOf(',') > 0)
                {
                    systemNumbers = condition.SystemNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    systemNumbers = systemNumbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (systemNumbers != null && systemNumbers.Any())
                {
                    sb.Append(" and SystemNumber in ( ");
                    foreach (string s in systemNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and SystemNumber like '%" + condition.SystemNumber.Trim() + "%' ");
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

                    sb.Append(" and CustomerOrderNumber in ( ");
                    foreach (string s in customerOrderNumbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and CustomerOrderNumber like '%" + condition.CustomerOrderNumber.Trim() + "%' ");
                }
            }

            if (!string.IsNullOrEmpty(condition.Str1))
            {
                IEnumerable<string> Str1 = Enumerable.Empty<string>();
                if (condition.Str1.IndexOf("\n") > 0)
                {
                    Str1 = condition.Str1.Split('\n').Select(s => { return s.Trim(); });
                }
                if (condition.Str1.IndexOf(',') > 0)
                {
                    Str1 = condition.Str1.Split(',').Select(s => { return s.Trim(); });
                }

                if (Str1 != null && Str1.Any())
                {
                    Str1 = Str1.Where(c => !string.IsNullOrEmpty(c));
                }

                if (Str1 != null && Str1.Any())
                {
                    sb.Append(" and CustomerOrderNumber in (SELECT DISTINCT CustomerOrderNumber FROM dbo.POD WHERE Str1 in(");
                    foreach (string s in Str1)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ))");
                }
                else
                {
                    sb.Append(" and CustomerOrderNumber IN(SELECT DISTINCT CustomerOrderNumber FROM dbo.POD WHERE Str1 LIKE '%" + condition.Str1 + "%')");
                }
            }

            //if (!string.IsNullOrEmpty(condition.Str1))
            //{
            //    //sb.Append(" and Str1 like '%" + condition.Str1 + "%' ");
            //    sb.Append(" and CustomerOrderNumber IN(SELECT DISTINCT CustomerOrderNumber FROM dbo.POD WHERE Str1 LIKE '%" + condition.Str1 + "%')");
            //}

            if (!string.IsNullOrEmpty(condition.Str2))
            {
                sb.Append(" and Str2 like '%" + condition.Str2 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str7))
            {
                sb.Append(" and Str7 like '%" + condition.Str7 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str13))
            {
                sb.Append(" and Str13 like '%" + condition.Str13 + "%' ");
            }

            if (!string.IsNullOrEmpty(condition.Str17))
            {
                sb.Append(" and Str17 like '%" + condition.Str17 + "%' ");
            }

            
            #endregion

            return sb.ToString();
        }

        ///// <summary>
        ///// 获取百姓网订单的快递信息
        ///// </summary>
        //public IEnumerable<Pod> GetBaiXingPod_KD(string CustomerOrderNumber, string SystemNumber)
        //{
        //    DbParam[] dbParams = new DbParam[]{
        //        new DbParam("@CustomerOrderNumber", DbType.String, CustomerOrderNumber, ParameterDirection.Input),
        //        new DbParam("@SystemNumber", DbType.String, SystemNumber, ParameterDirection.Input),
        //    };
        //    return this.ExecuteDataTable("Proc_GetBaiXingPod_KD", dbParams).ConvertToEntityCollection<Pod>();
        //}
    }
}
