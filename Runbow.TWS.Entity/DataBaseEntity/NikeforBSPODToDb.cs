using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.POD.Nike;
using SqlTypes = global::System.Data.SqlTypes;
namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class NikeforBSPODToDb : SqlDataRecord
    {
        public NikeforBSPODToDb(NikeforBSPOD pod)
            : base(s_metadata)
        {
            SetSqlInt64(0, pod.ID);
            SetSqlInt64(1, pod.ProjectID);
            SetSqlString(2, pod.SystemNumber);
            SetSqlString(3, pod.CustomerOrderNumber);
            SetSqlInt64(4, pod.CustomerID);
            SetSqlString(5, pod.CustomerName);
            SetSqlInt64(6, pod.ShipperID);
            SetSqlString(7, pod.ShipperName);
            SetSqlDateTime(8, pod.ActualDeliveryDate ?? SqlTypes.SqlDateTime.Null);
            SetSqlInt64(9, pod.StartCityID);
            SetSqlString(10, pod.StartCityName);
            SetSqlInt64(11, pod.EndCityID);
            SetSqlString(12, pod.EndCityName);
            SetSqlInt64(13, pod.PODStateID);
            SetSqlString(14, pod.PODStateName);
            SetSqlInt64(15, pod.ShipperTypeID);
            SetSqlString(16, pod.ShipperTypeName);
            SetSqlDouble(17, pod.BoxNumber);
            SetSqlDouble(18, pod.Weight);
            SetSqlDouble(19, pod.GoodsNumber);
            SetSqlDouble(20, pod.Volume);
            SetSqlString(21, pod.Creator);

            SetSqlString(22, pod.Str1);
            SetSqlString(23, pod.Str2);
            SetSqlString(24, pod.Str3);
            SetSqlString(25, pod.Str4);
            SetSqlString(26, pod.Str5);
            SetSqlString(27, pod.Str6);
            SetSqlString(28, pod.Str7);
            SetSqlString(29, pod.Str8);
            SetSqlString(30, pod.Str9);
            SetSqlInt64(31, pod.PODTypeID);
            SetSqlString(32, pod.PODTypeName);
            SetSqlInt64(33, pod.TtlOrTplID);
            SetSqlString(34, pod.TtlOrTplName);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("SystemNumber", SqlDbType.NVarChar, 50),
            new SqlMetaData("CustomerOrderNumber", SqlDbType.NVarChar,50),
            new SqlMetaData("CustomerID",SqlDbType.BigInt),
            new SqlMetaData("CustomerName",SqlDbType.NVarChar,50),
            new SqlMetaData("ShipperID",SqlDbType.BigInt),
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
            new SqlMetaData("Creator", SqlDbType.NVarChar,50),
            new SqlMetaData("Str1", SqlDbType.NVarChar, 50),
             new SqlMetaData("Str2", SqlDbType.NVarChar, 50),
              new SqlMetaData("Str3", SqlDbType.NVarChar, 50),
               new SqlMetaData("Str4", SqlDbType.NVarChar, 50),
                new SqlMetaData("Str5", SqlDbType.NVarChar, 50),
                 new SqlMetaData("Str6", SqlDbType.NVarChar, 50),
                  new SqlMetaData("Str7", SqlDbType.NVarChar, 50),
                   new SqlMetaData("Str8", SqlDbType.NVarChar, 50),
                    new SqlMetaData("Str9", SqlDbType.NVarChar, 50),
                      new SqlMetaData("PODTypeID", SqlDbType.BigInt),
                       new SqlMetaData("PODTypeName", SqlDbType.NVarChar, 50),
                        new SqlMetaData("TtlOrTplID", SqlDbType.BigInt),
                         new SqlMetaData("TtlOrTplName", SqlDbType.NVarChar, 50),
           
           
                                       
        };
    }
}
