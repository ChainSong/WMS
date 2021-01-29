using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class WMSMachiningToDb : SqlDataRecord
    {
        public WMSMachiningToDb(WMS_MachiningHeaderAndDetail wmsInfo)
            : base(s_metadata)
        {
            SetSqlString(0, wmsInfo.MachiningType);
            SetSqlInt64(1, wmsInfo.CustomerID);
            SetSqlString(2, wmsInfo.CustomerName);
            SetSqlString(3, wmsInfo.Location);
            SetSqlDateTime(4, wmsInfo.ExpectDate ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(5, wmsInfo.MachiningNumber);
            SetSqlString(6, wmsInfo.CarOrBoxNumber);
            SetSqlString(7, wmsInfo.Tel);
            SetSqlString(8, wmsInfo.Remark);
            SetSqlString(9, wmsInfo.SKU);
            SetSqlString(10, wmsInfo.GoodsName);
            SetSqlString(11, wmsInfo.BatchNumber);
            SetSqlString(12, wmsInfo.SKUType);
            SetSqlDateTime(13, wmsInfo.ExpectCompleteTime);
            SetSqlString(14, wmsInfo.QianFengNumber);
            SetSqlString(15, wmsInfo.ExpectWeight);
            SetSqlString(16, wmsInfo.ActualWeight);

            SetSqlString(17, wmsInfo.WashWeight);
            SetSqlString(18, wmsInfo.OtherWeight);
            SetSqlString(19, wmsInfo.PackageType);
            SetSqlString(20, wmsInfo.FillingType);
            SetSqlString(21, wmsInfo.EstimateFillingCount);
            SetSqlString(22, wmsInfo.ActualFillingBucket);
            SetSqlString(23, wmsInfo.MoreThanExpected);
            SetSqlString(24, wmsInfo.FillingWeightSUM);
            SetSqlString(25, wmsInfo.FillingBucketSUM);
            SetSqlString(26, wmsInfo.ProportioningSKU);
            SetSqlString(27, wmsInfo.ActualLossWeight);
            SetSqlString(28, wmsInfo.ActualLossRate);
            SetSqlString(29, wmsInfo.str1);
            SetSqlString(30, wmsInfo.str2);
            SetSqlString(31, wmsInfo.str3);
            SetSqlString(32, wmsInfo.Creator);
            SetSqlDateTime(33, wmsInfo.CreateTime);
            SetSqlString(34, wmsInfo.WashLocation);
            SetSqlString(35, wmsInfo.MoreThanLocation);
            SetSqlString(36, wmsInfo.IDS);
            SetSqlString(37, wmsInfo.Specifications);
            SetSqlString(38, wmsInfo.WashSpecifications);
            SetSqlString(39, wmsInfo.MoreThanSpecifications);
            SetSqlString(40, wmsInfo.IDDS);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("MachiningType", SqlDbType.NVarChar, 50),          
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("Location", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExpectDate", SqlDbType.DateTime),
            new SqlMetaData("MachiningNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CarOrBoxNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("Tel", SqlDbType.NVarChar, 50),
            new SqlMetaData("Remark", SqlDbType.NVarChar, 50),
            new SqlMetaData("SKU", SqlDbType.NVarChar, 50),
            new SqlMetaData("GoodsName", SqlDbType.NVarChar, 50),
            new SqlMetaData("BatchNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("SKUType", SqlDbType.NVarChar, 50),
            new SqlMetaData("ExpectCompleteTime",SqlDbType.DateTime),
            new SqlMetaData("QianFengNumber",  SqlDbType.NVarChar, 50),
            new SqlMetaData("ExpectWeight", SqlDbType.NVarChar, 50),
            new SqlMetaData("ActualWeight", SqlDbType.NVarChar, 50),

            new SqlMetaData("WashWeight", SqlDbType.NVarChar, 50),
            new SqlMetaData("OtherWeight", SqlDbType.NVarChar, 50),
            new SqlMetaData("PackageType", SqlDbType.NVarChar, 50),
            new SqlMetaData("FillingType", SqlDbType.NVarChar, 50),
            new SqlMetaData("EstimateFillingCount", SqlDbType.NVarChar, 50),
            new SqlMetaData("ActualFillingBucket", SqlDbType.NVarChar, 50),
            new SqlMetaData("MoreThanExpected", SqlDbType.NVarChar, 50),
            new SqlMetaData("FillingWeightSUM", SqlDbType.NVarChar, 50),
            new SqlMetaData("FillingBucketSUM", SqlDbType.NVarChar, 50),
            new SqlMetaData("ProportioningSKU", SqlDbType.NVarChar, 50),
            new SqlMetaData("ActualLossWeight", SqlDbType.NVarChar, 50),
            new SqlMetaData("ActualLossRate", SqlDbType.NVarChar, 50),
            new SqlMetaData("str1", SqlDbType.NVarChar, 50),
            new SqlMetaData("str2", SqlDbType.NVarChar, 50),
            new SqlMetaData("str3", SqlDbType.NVarChar, 50),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
            new SqlMetaData("WashLocation", SqlDbType.NVarChar, 50),
            new SqlMetaData("MoreThanLocation", SqlDbType.NVarChar, 50),
            new SqlMetaData("IDS", SqlDbType.NVarChar,100),
            new SqlMetaData("Specifications", SqlDbType.NVarChar,100),
            new SqlMetaData("WashSpecifications", SqlDbType.NVarChar,100),
            new SqlMetaData("MoreThanSpecifications", SqlDbType.NVarChar,100),
            new SqlMetaData("IDDS", SqlDbType.NVarChar,100)
        };
    }
}
