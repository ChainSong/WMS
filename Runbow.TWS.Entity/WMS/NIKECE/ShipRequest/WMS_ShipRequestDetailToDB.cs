using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class WMS_ShipRequestDetailToDB: SqlDataRecord
    {
        public WMS_ShipRequestDetailToDB(WMS_ShipRequestDetail detial)
            : base(s_metadata)
        {

            SetSqlString(0, detial.str1);
            SetSqlString(1, detial.str2);
            SetSqlString(2, detial.str3);
            SetSqlString(3, detial.str4);
            SetSqlString(4, detial.str5);
            SetSqlString(5, detial.str6);
            SetSqlString(6, detial.str7);
            SetSqlString(7, detial.str8);
            SetSqlString(8, detial.str9);
            SetSqlString(9, detial.str10);

            SetSqlString(10, detial.str11);
            SetSqlString(11, detial.str12);
            SetSqlString(12, detial.str13);
            SetSqlString(13, detial.str14);
            SetSqlString(14, detial.str15);
            SetSqlString(15, detial.str16);
            SetSqlString(16, detial.str17);
            SetSqlString(17, detial.str18);
            SetSqlString(18, detial.str19);
            SetSqlString(19, detial.str20);

            SetSqlString(20, detial.str21);
            SetSqlString(21, detial.str22);
            SetSqlString(22, detial.str23);
            SetSqlString(23, detial.str24);
            SetSqlString(24, detial.str25);
            SetSqlString(25, detial.str26);
            SetSqlString(26, detial.str27);
            SetSqlString(27, detial.str28);
            SetSqlString(28, detial.str29);
            SetSqlString(29, detial.str30);

            SetSqlString(30, detial.str31);
            SetSqlString(31, detial.str32);
            SetSqlString(32, detial.str33);
            SetSqlString(33, detial.str34);
            SetSqlString(34, detial.str35);
            SetSqlString(35, detial.str36);
            SetSqlString(36, detial.str37);
            SetSqlString(37, detial.str38);
            SetSqlString(38, detial.str39);
            SetSqlString(39, detial.str40);

            SetSqlString(40, detial.str41);
            SetSqlString(41, detial.str42);
            SetSqlString(42, detial.str43);
            SetSqlString(43, detial.str44);
            SetSqlString(44, detial.str45);
            SetSqlString(45, detial.str46);
            SetSqlString(46, detial.str47);
            SetSqlString(47, detial.str48);
            SetSqlString(48, detial.str49);
            SetSqlString(49, detial.str50);
    }
        private static readonly SqlMetaData[] s_metadata = {
          new SqlMetaData("str1",SqlDbType.NVarChar, 500),
          new SqlMetaData("str2",SqlDbType.NVarChar, 500),
          new SqlMetaData("str3",SqlDbType.NVarChar, 500),
          new SqlMetaData("str4",SqlDbType.NVarChar, 500),
          new SqlMetaData("str5",SqlDbType.NVarChar, 500),
          new SqlMetaData("str6",SqlDbType.NVarChar, 500),
          new SqlMetaData("str7",SqlDbType.NVarChar, 500),
          new SqlMetaData("str8",SqlDbType.NVarChar, 500),
          new SqlMetaData("str9",SqlDbType.NVarChar, 500),
          new SqlMetaData("str10",SqlDbType.NVarChar, 500),
          new SqlMetaData("str11",SqlDbType.NVarChar, 500),
          new SqlMetaData("str12",SqlDbType.NVarChar, 500),
          new SqlMetaData("str13",SqlDbType.NVarChar, 500),
          new SqlMetaData("str14",SqlDbType.NVarChar, 500),
          new SqlMetaData("str15",SqlDbType.NVarChar, 500),
          new SqlMetaData("str16",SqlDbType.NVarChar, 500),
          new SqlMetaData("str17",SqlDbType.NVarChar, 500),
          new SqlMetaData("str18",SqlDbType.NVarChar, 500),
          new SqlMetaData("str19",SqlDbType.NVarChar, 500),
          new SqlMetaData("str20",SqlDbType.NVarChar, 500),
          
          new SqlMetaData("str21",SqlDbType.NVarChar, 500),
          new SqlMetaData("str22",SqlDbType.NVarChar, 500),
          new SqlMetaData("str23",SqlDbType.NVarChar, 500),
          new SqlMetaData("str24",SqlDbType.NVarChar, 500),
          new SqlMetaData("str25",SqlDbType.NVarChar, 500),
          new SqlMetaData("str26",SqlDbType.NVarChar, 500),
          new SqlMetaData("str27",SqlDbType.NVarChar, 500),
          new SqlMetaData("str28",SqlDbType.NVarChar, 500),
          new SqlMetaData("str29",SqlDbType.NVarChar, 500),
          new SqlMetaData("str30",SqlDbType.NVarChar, 500),
          new SqlMetaData("str31",SqlDbType.NVarChar, 500),
          new SqlMetaData("str32",SqlDbType.NVarChar, 500),
          new SqlMetaData("str33",SqlDbType.NVarChar, 500),
          new SqlMetaData("str34",SqlDbType.NVarChar, 500),
          new SqlMetaData("str35",SqlDbType.NVarChar, 500),
          new SqlMetaData("str36",SqlDbType.NVarChar, 500),
          new SqlMetaData("str37",SqlDbType.NVarChar, 500),
          new SqlMetaData("str38",SqlDbType.NVarChar, 500),
          new SqlMetaData("str39",SqlDbType.NVarChar, 500),
          new SqlMetaData("str40",SqlDbType.NVarChar, 500),
          
          new SqlMetaData("str41",SqlDbType.NVarChar, 500),
          new SqlMetaData("str42",SqlDbType.NVarChar, 500),
          new SqlMetaData("str43",SqlDbType.NVarChar, 500),
          new SqlMetaData("str44",SqlDbType.NVarChar, 500),
          new SqlMetaData("str45",SqlDbType.NVarChar, 500),
          new SqlMetaData("str46",SqlDbType.NVarChar, 500),
          new SqlMetaData("str47",SqlDbType.NVarChar, 500),
          new SqlMetaData("str48",SqlDbType.NVarChar, 500),
          new SqlMetaData("str49",SqlDbType.NVarChar, 500),
          new SqlMetaData("str50",SqlDbType.NVarChar, 500),
              };
    }
}
