using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Dao
{
    public class TransportationLineAccessor : BaseAccessor
    {
        public IEnumerable<TransportationLine> GetTransportationLinesByCondition(string name, long? startCityID, long? endCityID, bool state, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenQueryTransportationLineWhere(name, startCityID, endCityID, state);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_GetTransportationLinesByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<TransportationLine>();
        }

        public IEnumerable<TransportationLine> GetTransportationLines()
        {
            return this.ExecuteDataTable("Proc_GetTransportationLines").ConvertToEntityCollection<TransportationLine>();
        }

        public long AddTransportationLine(string name, long startCityID, string strartCityName, long endCityID, string endCityName, string distance, string remark, bool state, string creator, DateTime createTime, string str1, string str2, string str3, out int returnVal)
        {
            DbParam[] dbParams = {
                             new DbParam("@Name", DbType.String, name, ParameterDirection.Input),
                             new DbParam("@StartCityID", DbType.Int64, startCityID, ParameterDirection.Input),
                             new DbParam("@StartCityName", DbType.String, strartCityName, ParameterDirection.Input),
                             new DbParam("@EndCityID", DbType.Int64, endCityID, ParameterDirection.Input),
                             new DbParam("@EndCityName", DbType.String, endCityName, ParameterDirection.Input),
                             new DbParam("@Distance", DbType.String, distance, ParameterDirection.Input),
                             new DbParam("@Remark", DbType.String, remark, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input),
                             new DbParam("@Creator", DbType.String, creator, ParameterDirection.Input),
                             new DbParam("@CreateTime", DbType.DateTime, createTime, ParameterDirection.Input),
                             new DbParam("@Str1", DbType.String, str1, ParameterDirection.Input),
                             new DbParam("@Str2", DbType.String, str2, ParameterDirection.Input),
                             new DbParam("@Str3", DbType.String, str3, ParameterDirection.Input),
                             new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };

            long transportationLineID = base.ExecuteScalar("Proc_AddTransportationLine", dbParams).ObjectToInt64();
            returnVal = dbParams[13].Value.ObjectToInt32();
            return transportationLineID;
        }

        public void SetTransportationLineState(long id, bool state)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input)
                                 };

            this.ExecuteNoQuery("Proc_SetTransportationLineState", dbParams);
        }

        private string GenQueryTransportationLineWhere(string name, long? startCityID, long? endCityID, bool state)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where State=" + (state ? "1" : "0") + " ");
            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" and Name like '%" + name.Trim() + "%' ");
            }

            if (startCityID.HasValue)
            {
                sb.Append(" and StartCityID=" + startCityID.Value.ToString() + " ");
            }

            if (endCityID.HasValue)
            {
                //sb.Append(" and EndCityID=" + endCityID.Value.ToString() + " ");
                sb.Append(" and EndCityID in (SELECT ID FROM Func_GetReginAndSunRegions(" + endCityID.Value.ToString() + ")) ");
            }

            return sb.ToString();
        }
    }
}