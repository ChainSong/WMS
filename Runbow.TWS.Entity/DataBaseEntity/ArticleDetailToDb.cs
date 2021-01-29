using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class ArticleDetailToDb:SqlDataRecord
    {
        public ArticleDetailToDb(WMS_ArticleDetail info)
            : base(s_metadata)
        {
            SetSqlString(0, info.ArticleNo);
            SetSqlString(1, info.Division);
            SetSqlString(2, info.GPC);
            SetSqlString(3, info.DimensionCode);
            SetSqlString(4, info.QualityCode);
            SetSqlString(5, info.UOM);
            SetSqlString(6, info.LongMaterial);
            SetSqlString(7, info.LongMaterialLocal);
            SetSqlString(8, info.LongColor);
            SetSqlString(9, info.LongcolorLocal);
            SetSqlString(10, info.ConversionFactor);
            SetSqlString(11, info.ColseOutDate);
            SetSqlString(12, info.FutureOfferBeginDate);
            SetSqlString(13, info.LifeCycle);
            SetSqlString(14, info.SeasonCode);
            SetSqlString(15, info.seasonYear);
            SetSqlString(16, info.CategoryCode);
            SetSqlString(17, info.SubCategoryCode);
            SetSqlString(18, info.WholesalePrice);
            SetSqlString(19, info.WholesalePriceCurrency);
            SetSqlString(20, info.RetailPrice);
            SetSqlString(21, info.RetailProceCurrency);
            SetSqlString(22, info.MasterGridNumber);
            SetSqlString(23, info.SilHouette);
            SetSqlString(24, info.Segment);
            SetSqlString(25, info.GenderAge);
            SetSqlString(26, info.USASizeRange);
            SetSqlString(27, info.SportActivity);
            SetSqlString(28, info.CarryOverFlag);
            SetSqlString(29, info.Fabrication);
            SetSqlString(30, info.CategoryText);
            SetSqlString(31, info.Hanger);
            SetSqlString(32, info.GlblCatSumCode);
            SetSqlString(33, info.GlblCatSum);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ArticleNo", SqlDbType.NVarChar, 50),
            new SqlMetaData("Division", SqlDbType.NVarChar, 50),
            new SqlMetaData("GPC", SqlDbType.NChar, 10),
            new SqlMetaData("DimensionCode", SqlDbType.NChar, 10),
            new SqlMetaData("QualityCode", SqlDbType.NChar, 10),
            new SqlMetaData("UOM", SqlDbType.NChar, 10),
            new SqlMetaData("LongMaterial", SqlDbType.NVarChar, 50),
            new SqlMetaData("LongMaterialLocal", SqlDbType.NVarChar, 100),
            new SqlMetaData("LongColor", SqlDbType.NVarChar, 100),
            new SqlMetaData("LongcolorLocal", SqlDbType.NVarChar, 300),
            new SqlMetaData("ConversionFactor", SqlDbType.NChar, 10),
            new SqlMetaData("ColseOutDate", SqlDbType.NChar, 10),
            new SqlMetaData("FutureOfferBeginDate", SqlDbType.NChar, 10),
            new SqlMetaData("LifeCycle", SqlDbType.NChar, 10),
            new SqlMetaData("SeasonCode", SqlDbType.NChar, 10),
            new SqlMetaData("seasonYear", SqlDbType.NChar, 10),
            new SqlMetaData("CategoryCode", SqlDbType.NChar, 10),
            new SqlMetaData("SubCategoryCode", SqlDbType.NChar, 10),
            new SqlMetaData("WholesalePrice", SqlDbType.NVarChar, 50),
            new SqlMetaData("WholesalePriceCurrency", SqlDbType.NChar, 10),
            new SqlMetaData("RetailPrice", SqlDbType.NChar, 10),
            new SqlMetaData("RetailProceCurrency", SqlDbType.NChar, 10),
            new SqlMetaData("MasterGridNumber", SqlDbType.NChar, 10),
            new SqlMetaData("SilHouette", SqlDbType.NChar, 10),
            new SqlMetaData("Segment", SqlDbType.NChar, 10),
            new SqlMetaData("GenderAge", SqlDbType.NChar, 10),
            new SqlMetaData("USASizeRange", SqlDbType.NVarChar, 150),
            new SqlMetaData("SportActivity", SqlDbType.NChar, 10),
            new SqlMetaData("CarryOverFlag", SqlDbType.NChar, 10),
            new SqlMetaData("Fabrication", SqlDbType.NChar, 10),
            new SqlMetaData("CategoryText", SqlDbType.NVarChar, 200),
            new SqlMetaData("Hanger", SqlDbType.NVarChar, 50),
            new SqlMetaData("GlblCatSumCode", SqlDbType.NVarChar, 50),
            new SqlMetaData("GlblCatSum", SqlDbType.NVarChar, 200)
        };
    }
}
