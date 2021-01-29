using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Product
{
    public class WMS_ArticleDetail
    {
        [EntityPropertyExtension("ArticleNo", "ArticleNo")]
        public string ArticleNo { get; set; }

        [EntityPropertyExtension("Division", "Division")]
        public string Division { get; set; }

        [EntityPropertyExtension("GPC", "GPC")]
        public string GPC { get; set; }

        [EntityPropertyExtension("DimensionCode", "DimensionCode")]
        public string DimensionCode { get; set; }

        [EntityPropertyExtension("QualityCode", "QualityCode")]
        public string QualityCode { get; set; }

        [EntityPropertyExtension("UOM", "UOM")]
        public string UOM { get; set; }

        [EntityPropertyExtension("LongMaterial", "LongMaterial")]
        public string LongMaterial { get; set; }

        [EntityPropertyExtension("LongMaterialLocal", "LongMaterialLocal")]
        public string LongMaterialLocal { get; set; }

        [EntityPropertyExtension("LongColor", "LongColor")]
        public string LongColor { get; set; }

        [EntityPropertyExtension("LongcolorLocal", "LongcolorLocal")]
        public string LongcolorLocal { get; set; }

        [EntityPropertyExtension("ConversionFactor", "ConversionFactor")]
        public string ConversionFactor { get; set; }

        [EntityPropertyExtension("ColseOutDate", "ColseOutDate")]
        public string ColseOutDate { get; set; }

        [EntityPropertyExtension("FutureOfferBeginDate", "FutureOfferBeginDate")]
        public string FutureOfferBeginDate { get; set; }

        [EntityPropertyExtension("LifeCycle", "LifeCycle")]
        public string LifeCycle { get; set; }

        [EntityPropertyExtension("SeasonCode", "SeasonCode")]
        public string SeasonCode { get; set; }

        [EntityPropertyExtension("seasonYear", "seasonYear")]
        public string seasonYear { get; set; }

        [EntityPropertyExtension("CategoryCode", "CategoryCode")]
        public string CategoryCode { get; set; }

        [EntityPropertyExtension("SubCategoryCode", "SubCategoryCode")]
        public string SubCategoryCode { get; set; }

        [EntityPropertyExtension("WholesalePrice", "WholesalePrice")]
        public string WholesalePrice { get; set; }

        [EntityPropertyExtension("WholesalePriceCurrency", "WholesalePriceCurrency")]
        public string WholesalePriceCurrency { get; set; }

        [EntityPropertyExtension("RetailPrice", "RetailPrice")]
        public string RetailPrice { get; set; }

        [EntityPropertyExtension("RetailProceCurrency", "RetailProceCurrency")]
        public string RetailProceCurrency { get; set; }

        [EntityPropertyExtension("MasterGridNumber", "MasterGridNumber")]
        public string MasterGridNumber { get; set; }

        [EntityPropertyExtension("SilHouette", "SilHouette")]
        public string SilHouette { get; set; }

        [EntityPropertyExtension("Segment", "Segment")]
        public string Segment { get; set; }

        [EntityPropertyExtension("GenderAge", "GenderAge")]
        public string GenderAge { get; set; }

        [EntityPropertyExtension("USASizeRange", "USASizeRange")]
        public string USASizeRange { get; set; }

        [EntityPropertyExtension("SportActivity", "SportActivity")]
        public string SportActivity { get; set; }

        [EntityPropertyExtension("CarryOverFlag", "CarryOverFlag")]
        public string CarryOverFlag { get; set; }

        [EntityPropertyExtension("Fabrication", "Fabrication")]
        public string Fabrication { get; set; }

        [EntityPropertyExtension("CategoryText", "CategoryText")]
        public string CategoryText { get; set; }

        [EntityPropertyExtension("Hanger", "Hanger")]
        public string Hanger { get; set; }

        [EntityPropertyExtension("GlblCatSumCode", "GlblCatSumCode")]
        public string GlblCatSumCode { get; set; }

        [EntityPropertyExtension("GlblCatSum", "GlblCatSum")]
        public string GlblCatSum { get; set; }
    }
}
