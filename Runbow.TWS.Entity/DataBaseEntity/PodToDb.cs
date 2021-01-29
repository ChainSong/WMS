using System.Data;
using Microsoft.SqlServer.Server;
using SqlTypes = global::System.Data.SqlTypes;

namespace Runbow.TWS.Entity
{
    public class PodToDb : SqlDataRecord
    {
        public PodToDb(Pod pod)
            : base(s_metadata)
        {
            SetSqlInt64(0, pod.ID);
            SetSqlInt64(1, pod.ProjectID);
            SetSqlString(2, pod.SystemNumber);
            SetSqlString(3, pod.CustomerOrderNumber);
            SetSqlInt64(4, pod.CustomerID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(5, pod.CustomerName);
            SetSqlInt64(6, pod.ShipperID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(7, pod.ShipperName);
            SetSqlDateTime(8, pod.ActualDeliveryDate ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt64(9, pod.StartCityID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(10, pod.StartCityName);
            SetSqlInt64(11, pod.EndCityID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(12, pod.EndCityName);
            SetSqlInt64(13, pod.PODStateID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(14, pod.PODStateName);
            SetSqlInt64(15, pod.ShipperTypeID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(16, pod.ShipperTypeName);
            SetSqlDouble(17, pod.BoxNumber ?? SqlTypes.SqlDouble.Null);
            SetSqlDouble(18, pod.Weight ?? SqlTypes.SqlDouble.Null);
            SetSqlDouble(19, pod.GoodsNumber ?? SqlTypes.SqlDouble.Null);
            SetSqlDouble(20, pod.Volume ?? SqlTypes.SqlDouble.Null);
            SetSqlString(21, pod.Creator);
            SetSqlDateTime(22, pod.CreateTime ?? SqlTypes.SqlDateTime.Null);
            SetSqlString(23, pod.Str1);
            SetSqlString(24, pod.Str2);
            SetSqlString(25, pod.Str3);
            SetSqlString(26, pod.Str4);
            SetSqlString(27, pod.Str5);
            SetSqlString(28, pod.Str6);
            SetSqlString(29, pod.Str7);
            SetSqlString(30, pod.Str8);
            SetSqlString(31, pod.Str9);
            SetSqlString(32, pod.Str10);
            SetSqlString(33, pod.Str11);
            SetSqlString(34, pod.Str12);
            SetSqlString(35, pod.Str13);
            SetSqlString(36, pod.Str14);
            SetSqlString(37, pod.Str15);
            SetSqlString(38, pod.Str16);
            SetSqlString(39, pod.Str17);
            SetSqlString(40, pod.Str18);
            SetSqlString(41, pod.Str19);
            SetSqlString(42, pod.Str20);
            SetSqlString(43, pod.Str21);
            SetSqlString(44, pod.Str22);
            SetSqlString(45, pod.Str23);
            SetSqlString(46, pod.Str24);
            SetSqlString(47, pod.Str25);
            SetSqlString(48, pod.Str26);
            SetSqlString(49, pod.Str27);
            SetSqlString(50, pod.Str28);
            SetSqlString(51, pod.Str29);
            SetSqlString(52, pod.Str30);
            SetSqlString(53, pod.Str31);
            SetSqlString(54, pod.Str32);
            SetSqlString(55, pod.Str33);
            SetSqlString(56, pod.Str34);
            SetSqlString(57, pod.Str35);
            SetSqlString(58, pod.Str36);
            SetSqlString(59, pod.Str37);
            SetSqlString(60, pod.Str38);
            SetSqlString(61, pod.Str39);
            SetSqlString(62, pod.Str40);
            SetSqlString(63, pod.Str41);
            SetSqlString(64, pod.Str42);
            SetSqlString(65, pod.Str43);
            SetSqlString(66, pod.Str44);
            SetSqlString(67, pod.Str45);
            SetSqlString(68, pod.Str46);
            SetSqlString(69, pod.Str47);
            SetSqlString(70, pod.Str48);
            SetSqlString(71, pod.Str49);
            SetSqlString(72, pod.Str50);
            SetSqlDateTime(73, pod.DateTime1 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(74, pod.DateTime2 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(75, pod.DateTime3 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(76, pod.DateTime4 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(77, pod.DateTime5 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(78, pod.DateTime6 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(79, pod.DateTime7 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(80, pod.DateTime8 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(81, pod.DateTime9 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(82, pod.DateTime10 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(83, pod.DateTime11 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(84, pod.DateTime12 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(85, pod.DateTime13 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(86, pod.DateTime14 ?? SqlTypes.SqlDateTime.Null);
            SetSqlDateTime(87, pod.DateTime15 ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt64(88, pod.PODTypeID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(89, pod.PODTypeName);
            SetSqlInt64(90, pod.TtlOrTplID ?? SqlTypes.SqlInt64.Null);
            SetSqlString(91, pod.TtlOrTplName);
            SetSqlBoolean(92, pod.IsSettledForCustomer ?? false);
            SetSqlBoolean(93, pod.IsSettledForShipper ?? false);
            SetSqlInt32(94, pod.Type ?? 2);
            SetSqlBoolean(95, pod.HasShortDial ?? false);
            SetSqlBoolean(96, pod.HasDistribution ?? false);
            SetSqlBoolean(97, pod.HasExpress ?? false);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("SystemNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerOrderNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerID", SqlDbType.BigInt),
            new SqlMetaData("CustomerName", SqlDbType.NVarChar, 50),
            new SqlMetaData("ShipperID", SqlDbType.BigInt),
            new SqlMetaData("ShipperName", SqlDbType.NVarChar, 50),
            new SqlMetaData("ActualDeliveryDate", SqlDbType.DateTime),
            new SqlMetaData("StartCityID", SqlDbType.BigInt),
            new SqlMetaData("StartCityName", SqlDbType.NVarChar, 50),
            new SqlMetaData("EndCityID", SqlDbType.BigInt),
            new SqlMetaData("EndCityName", SqlDbType.NVarChar, 50),
            new SqlMetaData("PODStateID", SqlDbType.BigInt),
            new SqlMetaData("PODStateName", SqlDbType.NVarChar, 50),
            new SqlMetaData("ShipperTypeID", SqlDbType.BigInt),
            new SqlMetaData("ShipperTypeName", SqlDbType.NVarChar, 50),
            new SqlMetaData("BoxNumber", SqlDbType.Float),
            new SqlMetaData("Weight", SqlDbType.Float),
            new SqlMetaData("GoodsNumber", SqlDbType.Float),
            new SqlMetaData("Volume", SqlDbType.Float),
            new SqlMetaData("Creator", SqlDbType.NVarChar, 50),
            new SqlMetaData("CreateTime", SqlDbType.DateTime),
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
            new SqlMetaData("Str12", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str13", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str14", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str15", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str16", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str17", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str18", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str19", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str20", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str21", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str22", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str23", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str24", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str25", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str26", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str27", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str28", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str29", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str30", SqlDbType.NVarChar, 50),
            new SqlMetaData("Str31", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str32", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str33", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str34", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str35", SqlDbType.NVarChar, 100),
            new SqlMetaData("Str36", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str37", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str38", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str39", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str40", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str41", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str42", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str43", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str44", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str45", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str46", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str47", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str48", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str49", SqlDbType.NVarChar, 500),
            new SqlMetaData("Str50", SqlDbType.NVarChar, 500),
            new SqlMetaData("DateTime1", SqlDbType.DateTime),
            new SqlMetaData("DateTime2", SqlDbType.DateTime),
            new SqlMetaData("DateTime3", SqlDbType.DateTime),
            new SqlMetaData("DateTime4", SqlDbType.DateTime),
            new SqlMetaData("DateTime5", SqlDbType.DateTime),
            new SqlMetaData("DateTime6", SqlDbType.DateTime),
            new SqlMetaData("DateTime7", SqlDbType.DateTime),
            new SqlMetaData("DateTime8", SqlDbType.DateTime),
            new SqlMetaData("DateTime9", SqlDbType.DateTime),
            new SqlMetaData("DateTime10", SqlDbType.DateTime),
            new SqlMetaData("DateTime11", SqlDbType.DateTime),
            new SqlMetaData("DateTime12", SqlDbType.DateTime),
            new SqlMetaData("DateTime13", SqlDbType.DateTime),
            new SqlMetaData("DateTime14", SqlDbType.DateTime),
            new SqlMetaData("DateTime15", SqlDbType.DateTime),
            new SqlMetaData("PODTypeID", SqlDbType.BigInt),
            new SqlMetaData("PODTypeName", SqlDbType.NVarChar, 50),
            new SqlMetaData("TtlOrTplID", SqlDbType.BigInt),
            new SqlMetaData("TtlOrTplName", SqlDbType.NVarChar, 50),
            new SqlMetaData("IsSettledForCustomer", SqlDbType.Bit),
            new SqlMetaData("IsSettledForShipper", SqlDbType.Bit),
            new SqlMetaData("Type",SqlDbType.Int),
            new SqlMetaData("HasShortDial", SqlDbType.Bit),
            new SqlMetaData("HasDistribution", SqlDbType.Bit),
            new SqlMetaData("HasExpress", SqlDbType.Bit)
        };
    }
}