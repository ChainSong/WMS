using System;
using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class QuotedPriceToDb : SqlDataRecord
    {
        public QuotedPriceToDb(QuotedPrice price)
            : base(s_metadata)
        {
            SetSqlInt64(0, price.ID);
            SetSqlInt64(1, price.ProjectID);
            SetSqlString(2, price.ProjectName);
            SetSqlInt32(3, price.Target);
            SetSqlInt64(4, price.TargetID);
            SetSqlString(5, price.TargetName);
            SetSqlInt64(6, price.SegmentDetailID);
            SetSqlDouble(7, price.StartVal);
            SetSqlDouble(8, price.EndVal);
            SetSqlInt64(9, price.TransportationLineID?? 0);
            SetSqlInt64(10, price.StartCityID);
            SetSqlString(11, price.StartCityName);
            SetSqlInt64(12, price.EndCityID);
            SetSqlString(13, price.EndCityName);
            SetSqlInt64(14, price.ShipperTypeID);
            SetSqlString(15, price.ShipperTypeName);
            SetSqlInt64(16, price.PodTypeID);
            SetSqlString(17, price.PodTypeName);
            SetSqlInt64(18, price.TplOrTtlID);
            SetSqlString(19, price.TplOrTtlName);
            SetSqlDecimal(20, price.Price);
            SetSqlDecimal(21, price.Point ?? 0);
            SetSqlDecimal(22, price.MinPrice ?? 0);
            SetSqlDateTime(23, price.EffectiveStartTime);
            SetSqlDateTime(24, price.EffectiveEndTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlBoolean(25, price.State);
            SetSqlString(26, price.Remark);
            SetSqlString(27, price.Creator);
            SetSqlDateTime(28, price.CreateTime ?? DateTime.Now);
            SetSqlString(29, price.Str1);
            SetSqlString(30, price.Str2);
            SetSqlString(31, price.Str3);
            SetSqlInt64(32, price.RelatedCustomerID ?? 0);
            SetSqlString(33, price.RelatedCustomerName);
            SetSqlDecimal(34, price.EmptyCarryPrice ?? 0);
            SetSqlDecimal(35, price.Decimal1 ?? 0);
            SetSqlDecimal(36, price.Decimal2 ?? 0);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("ProjectName", SqlDbType.NVarChar, 50),
            new SqlMetaData("Target", SqlDbType.Int),
            new SqlMetaData("TargetID",SqlDbType.BigInt),
            new SqlMetaData("TargetName", SqlDbType.NVarChar, 50),
            new SqlMetaData("SegmentDetailID", SqlDbType.BigInt),
            new SqlMetaData("StartVal", SqlDbType.Float),
            new SqlMetaData("EndVal", SqlDbType.Float),
            new SqlMetaData("TransportationLineID", SqlDbType.BigInt),
            new SqlMetaData("StartCityID", SqlDbType.BigInt),
            new SqlMetaData("StartCityName", SqlDbType.NVarChar, 50),
            new SqlMetaData("EndCityID", SqlDbType.BigInt),
            new SqlMetaData("EndCityName", SqlDbType.NVarChar, 50),
            new SqlMetaData("ShipperTypeID", SqlDbType.BigInt),
            new SqlMetaData("ShipperTypeName", SqlDbType.NVarChar, 50),
            new SqlMetaData("PodTypeID", SqlDbType.BigInt),
            new SqlMetaData("PodTypeName", SqlDbType.NVarChar, 50),
            new SqlMetaData("TplOrTtlID", SqlDbType.BigInt),
            new SqlMetaData("TplOrTtlName", SqlDbType.NVarChar, 50),
            new SqlMetaData("Price", SqlDbType.Decimal,18,3),
            new SqlMetaData("Point", SqlDbType.Decimal,18,2),
            new SqlMetaData("MinPrice", SqlDbType.Decimal,18,2),
            new SqlMetaData("EffectiveStartTime", SqlDbType.DateTime),
            new SqlMetaData("EffectiveEndTime", SqlDbType.DateTime),
            new SqlMetaData("State", SqlDbType.Bit),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 500),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str2", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str3", SqlDbType.NVarChar, 500),
            new SqlMetaData("RelatedCustomerID", SqlDbType.BigInt),
            new SqlMetaData("RelatedCustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("EmptyCarryPrice", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal1", SqlDbType.Decimal,18,2),
            new SqlMetaData("Decimal2", SqlDbType.Decimal,18,2)
        };
    }
}