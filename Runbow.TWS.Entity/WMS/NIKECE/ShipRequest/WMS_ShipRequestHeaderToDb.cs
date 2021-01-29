using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity.WMS.NIKECE.ShipRequest
{
    public class WMS_ShipRequestHeaderToDb : SqlDataRecord
    {
        public WMS_ShipRequestHeaderToDb(WMS_ShipRequestHeader header)
            : base(s_metadata)
        {

            SetSqlString(0, header.str1);
            SetSqlString(1, header.str2);
            SetSqlString(2, header.str3);
            SetSqlString(3, header.str4);
            SetSqlString(4, header.str5);
            SetSqlString(5, header.str6);
            SetSqlString(6, header.str7);
            SetSqlString(7, header.str8);
            SetSqlString(8, header.str9);
            SetSqlString(9, header.str10);

            SetSqlString(10, header.str11);
            SetSqlString(11, header.str12);
            SetSqlString(12, header.str13);
            SetSqlString(13, header.str14);
            SetSqlString(14, header.str15);
            SetSqlString(15, header.str16);
            SetSqlString(16, header.str17);
            SetSqlString(17, header.str18);
            SetSqlString(18, header.str19);
            SetSqlString(19, header.str20);

            SetSqlString(20, header.str21);
            SetSqlString(21, header.str22);
            SetSqlString(22, header.str23);
            SetSqlString(23, header.str24);
            SetSqlString(24, header.str25);
            SetSqlString(25, header.str26);
            SetSqlString(26, header.str27);
            SetSqlString(27, header.str28);
            SetSqlString(28, header.str29);
            SetSqlString(29, header.str30);

            SetSqlString(30, header.str31);
            SetSqlString(31, header.str32);
            SetSqlString(32, header.str33);
            SetSqlString(33, header.str34);
            SetSqlString(34, header.str35);
            SetSqlString(35, header.str36);
            SetSqlString(36, header.str37);
            SetSqlString(37, header.str38);
            SetSqlString(38, header.str39);
            SetSqlString(39, header.str40);

            SetSqlString(40, header.str41);
            SetSqlString(41, header.str42);
            SetSqlString(42, header.str43);
            SetSqlString(43, header.str44);
            SetSqlString(44, header.str45);
            SetSqlString(45, header.str46);
            SetSqlString(46, header.str47);
            SetSqlString(47, header.str48);
            SetSqlString(48, header.str49);
            SetSqlString(49, header.str50);

            SetSqlString(50, header.str51);
            SetSqlString(51, header.str52);
            SetSqlString(52, header.str53);
            SetSqlString(53, header.str54);
            SetSqlString(54, header.str55);
            SetSqlString(55, header.str56);
            SetSqlString(56, header.str57);
            SetSqlString(57, header.str58);
            SetSqlString(58, header.str59);
            SetSqlString(59, header.str60);

            SetSqlString(60, header.str61);
            SetSqlString(61, header.str62);
            SetSqlString(62, header.str63);
            SetSqlString(63, header.str64);
            SetSqlString(64, header.str65);
            SetSqlString(65, header.str66);
            SetSqlString(66, header.str67);
            SetSqlString(67, header.str68);
            SetSqlString(68, header.str69);
            SetSqlString(69, header.str70);
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
          new SqlMetaData("str51",SqlDbType.NVarChar, 500),
          new SqlMetaData("str52",SqlDbType.NVarChar, 500),
          new SqlMetaData("str53",SqlDbType.NVarChar, 500),
          new SqlMetaData("str54",SqlDbType.NVarChar, 500),
          new SqlMetaData("str55",SqlDbType.NVarChar, 500),
          new SqlMetaData("str56",SqlDbType.NVarChar, 500),
          new SqlMetaData("str57",SqlDbType.NVarChar, 500),
          new SqlMetaData("str58",SqlDbType.NVarChar, 500),
          new SqlMetaData("str59",SqlDbType.NVarChar, 500),
          new SqlMetaData("str60",SqlDbType.NVarChar, 500),
          
          new SqlMetaData("str61",SqlDbType.NVarChar, 500),
          new SqlMetaData("str62",SqlDbType.NVarChar, 500),
          new SqlMetaData("str63",SqlDbType.NVarChar, 500),
          new SqlMetaData("str64",SqlDbType.NVarChar, 500),
          new SqlMetaData("str65",SqlDbType.NVarChar, 500),
          new SqlMetaData("str66",SqlDbType.NVarChar, 500),
          new SqlMetaData("str67",SqlDbType.NVarChar, 500),
          new SqlMetaData("str68",SqlDbType.NVarChar, 500),
          new SqlMetaData("str69",SqlDbType.NVarChar, 500),
          new SqlMetaData("str70",SqlDbType.NVarChar, 500),
              };
    }
}
