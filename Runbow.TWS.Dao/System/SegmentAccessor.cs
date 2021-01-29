using System;
using System.Collections.Generic;
using System.Data;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Dao
{
    public class SegmentAccessor : BaseAccessor
    {
        public IEnumerable<Segment> GetSegmentsByCondition(string name, bool state)
        {
            DbParam[] dbParams = {
                             new DbParam("@Name", DbType.String, name, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input)
                                 };

            return this.ExecuteDataTable("Proc_GetSegmentsByConditon", dbParams).ConvertToEntityCollection<Segment>();
        }

        public void SetSegmentState(long segmentID, bool state)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.Int64, segmentID, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input)
                                 };

            this.ExecuteNoQuery("Proc_SetSegmentState", dbParams);
        }

        public long AddSegmentDetail(long segmentID, float startVal, float endVal, string description, string creator, DateTime createTime, string str1, string str2, string str3, out int returnVal)
        {
            DbParam[] dbParams = {
                             new DbParam("@SegmentID", DbType.Int64, segmentID, ParameterDirection.Input),
                             new DbParam("@StartVal", DbType.Double, startVal, ParameterDirection.Input),
                             new DbParam("@EndVal", DbType.Double, endVal, ParameterDirection.Input),
                             new DbParam("@Description", DbType.String, description, ParameterDirection.Input),
                             new DbParam("@Creator", DbType.String, creator, ParameterDirection.Input),
                             new DbParam("@CreateTime", DbType.DateTime, createTime, ParameterDirection.Input),
                             new DbParam("@Str1", DbType.String, str1, ParameterDirection.Input),
                             new DbParam("@Str2", DbType.String, str2, ParameterDirection.Input),
                             new DbParam("@Str3", DbType.String, str3, ParameterDirection.Input),
                             new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };

            long segmentDetailID = base.ExecuteScalar("Proc_AddSegmentDetail", dbParams).ObjectToInt64();
            returnVal = dbParams[9].Value.ObjectToInt32();
            return segmentDetailID;
        }

        public long UpdateSegmentDetail(long id, long segmentID, float startVal, float endVal, string description, string str1, string str2, string str3, out int returnVal)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input),
                             new DbParam("@SegmentID", DbType.Int64, segmentID, ParameterDirection.Input),
                             new DbParam("@StartVal", DbType.Double, startVal, ParameterDirection.Input),
                             new DbParam("@EndVal", DbType.Double, endVal, ParameterDirection.Input),
                             new DbParam("@Description", DbType.String, description, ParameterDirection.Input),
                             new DbParam("@Str1", DbType.String, str1, ParameterDirection.Input),
                             new DbParam("@Str2", DbType.String, str2, ParameterDirection.Input),
                             new DbParam("@Str3", DbType.String, str3, ParameterDirection.Input),
                             new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };

            base.ExecuteNoQuery("Proc_UpdateSegmentDetail", dbParams);
            returnVal = dbParams[8].Value.ObjectToInt32();
            return id;
        }

        public long DeleteSegmentDetail(long id, out int returnVal)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input),
                             new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };

            base.ExecuteNoQuery("Proc_DeleteSegmentDetail", dbParams);
            returnVal = dbParams[1].Value.ObjectToInt32();
            return id;
        }

        public long AddSegment(string name, string description, string creator, DateTime createTime, string str1, string str2, string str3, bool state, out int returnVal)
        {
            DbParam[] dbParams = {
                             new DbParam("@Name", DbType.String, name, ParameterDirection.Input),
                             new DbParam("@Description", DbType.String, description, ParameterDirection.Input),
                             new DbParam("@Creator", DbType.String, creator, ParameterDirection.Input),
                             new DbParam("@CreateTime", DbType.DateTime, createTime, ParameterDirection.Input),
                             new DbParam("@Str1", DbType.String, str1, ParameterDirection.Input),
                             new DbParam("@Str2", DbType.String, str2, ParameterDirection.Input),
                             new DbParam("@Str3", DbType.String, str3, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input),
                             new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output)
                             };

            long segmentID = base.ExecuteScalar("Proc_AddSegment", dbParams).ObjectToInt64();
            returnVal = dbParams[8].Value.ObjectToInt32();
            return segmentID;
        }

        public string GetSegmentByCursterId(long projectId,long cId)
        {
            DbParam[] dbParams = {
                             new DbParam("@ProjectId", DbType.Int64, projectId, ParameterDirection.Input),
                              new DbParam("@CustomerId", DbType.Int64, cId, ParameterDirection.Input)
                             };
            DataTable dt = base.ExecuteDataTable("Proc_GetSegmentByCustomerId", dbParams);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0]["SegmentID"].ToString();
            else
                return "-1";
        }

        public void GetSegmentAndDetail(long id, out Segment segment, out IEnumerable<SegmentDetail> segmentDetailCollection)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input)
                             };

            DataSet segmentInfo = base.ExecuteDataSet("Proc_GetSegmentAndDetail", dbParams);

            segment = segmentInfo.Tables[0].ConvertToEntity<Segment>();
            segmentDetailCollection = segmentInfo.Tables[1].ConvertToEntityCollection<SegmentDetail>();
        }
    }
}