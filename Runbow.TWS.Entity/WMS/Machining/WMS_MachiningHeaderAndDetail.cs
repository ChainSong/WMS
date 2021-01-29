using Runbow.TWS.Common;
using System;

namespace Runbow.TWS.Entity
{
    public class WMS_MachiningHeaderAndDetail
    {
        [EntityPropertyExtension("PageIndex", "PageIndex")]
        public string PageIndex { get; set; }

        [EntityPropertyExtension("PictureStr", "PictureStr")]
        public string PictureStr { get; set; }

        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        [EntityPropertyExtension("WashSpecifications", "WashSpecifications")]
        public string WashSpecifications { get; set; }

        [EntityPropertyExtension("MoreThanSpecifications", "MoreThanSpecifications")]
        public string MoreThanSpecifications { get; set; }

        [EntityPropertyExtension("IDS", "IDS")]
        public string IDS { get; set; }

        [EntityPropertyExtension("IDDS", "IDDS")]
        public string IDDS { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public int Status { get; set; }

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("MID", "MID")]
        public long MID { get; set; }

        [EntityPropertyExtension("MachiningType", "MachiningType")]
        public string MachiningType { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        [EntityPropertyExtension("ExpectDate", "ExpectDate")]
        public DateTime? ExpectDate { get; set; }

        [EntityPropertyExtension("MachiningNumber", "MachiningNumber")]
        public string MachiningNumber { get; set; }

        [EntityPropertyExtension("CarOrBoxNumber", "CarOrBoxNumber")]
        public string CarOrBoxNumber { get; set; }

        [EntityPropertyExtension("Tel", "Tel")]
        public string Tel { get; set; }


        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }

        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }


        [EntityPropertyExtension("SKUType", "SKUType")]
        public string SKUType { get; set; }

        [EntityPropertyExtension("ExpectCompleteTime", "ExpectCompleteTime")]
        public DateTime ExpectCompleteTime { get; set; }

        [EntityPropertyExtension("QianFengNumber", "QianFengNumber")]
        public string QianFengNumber { get; set; }

        [EntityPropertyExtension("ExpectWeight", "ExpectWeight")]
        public string ExpectWeight { get; set; }

        [EntityPropertyExtension("ActualWeight", "ActualWeight")]
        public string ActualWeight { get; set; }

        [EntityPropertyExtension("WashWeight", "WashWeight")]
        public string WashWeight { get; set; }

        [EntityPropertyExtension("OtherWeight", "OtherWeight")]
        public string OtherWeight { get; set; }

        [EntityPropertyExtension("PackageType", "PackageType")]
        public string PackageType { get; set; }

        [EntityPropertyExtension("FillingType", "FillingType")]
        public string FillingType { get; set; }

        [EntityPropertyExtension("EstimateFillingCount", "EstimateFillingCount")]
        public string EstimateFillingCount { get; set; }

        [EntityPropertyExtension("ActualFillingBucket", "ActualFillingBucket")]
        public string ActualFillingBucket { get; set; }

        [EntityPropertyExtension("MoreThanExpected", "MoreThanExpected")]
        public string MoreThanExpected { get; set; }

        [EntityPropertyExtension("FillingWeightSUM", "FillingWeightSUM")]
        public string FillingWeightSUM { get; set; }



        [EntityPropertyExtension("FillingBucketSUM", "FillingBucketSUM")]
        public string FillingBucketSUM { get; set; }

        [EntityPropertyExtension("ProportioningSKU", "ProportioningSKU")]
        public string ProportioningSKU { get; set; }

        [EntityPropertyExtension("ActualLossWeight", "ActualLossWeight")]
        public string ActualLossWeight { get; set; }

        [EntityPropertyExtension("ActualLossRate", "ActualLossRate")]
        public string ActualLossRate { get; set; }

        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }

        [EntityPropertyExtension("str2", "str2")]
        public string str2 { get; set; }

        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

      
        [EntityPropertyExtension("WashLocation", "WashLocation")]
        public string WashLocation { get; set; }

        [EntityPropertyExtension("MoreThanLocation", "MoreThanLocation")]
        public string MoreThanLocation { get; set; }
    }
}
