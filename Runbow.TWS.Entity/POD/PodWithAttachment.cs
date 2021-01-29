using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class PodWithAttachment : Pod
    {
        [EntityPropertyExtension("AttachmentGroupID", "AttachmentGroupID")]
        public string AttachmentGroupID { get; set; }

        [EntityPropertyExtension("IsUploadPod", "IsUploadPod")]
        public int? IsUploadPod { get; set; }

        [EntityPropertyExtension("ReplyTime", "ReplyTime")]
        public DateTime? ReplyTime { get; set; }

        [EntityPropertyExtension("GroupID", "GroupID")]
        public string GroupID { get; set; }

        [EntityPropertyExtension("Url", "Url")]
        public string Url { get; set; }

        public PodWithAttachment()
        {
        }

        public PodWithAttachment(Pod pod)
        {
            this.ID = pod.ID;
            this.ProjectID = pod.ProjectID;
            this.SystemNumber = pod.SystemNumber;
            this.CustomerOrderNumber = pod.CustomerOrderNumber;
            this.CustomerID = pod.CustomerID;
            this.CustomerName = pod.CustomerName;
            this.ShipperID = pod.ShipperID;
            this.ShipperName = pod.ShipperName;
            this.ActualDeliveryDate = pod.ActualDeliveryDate;
            this.StartCityID = pod.StartCityID;
            this.StartCityName = pod.StartCityName;
            this.EndCityID = pod.EndCityID;
            this.PODTypeID = pod.PODTypeID;
            this.PODTypeName = pod.PODTypeName;
            this.EndCityName = pod.EndCityName;
            this.PODStateID = pod.PODStateID;
            this.PODStateName = pod.PODStateName;
            this.ShipperTypeID = pod.ShipperTypeID;
            this.ShipperTypeName = pod.ShipperTypeName;
            this.TtlOrTplID = pod.TtlOrTplID;
            this.TtlOrTplName = pod.TtlOrTplName;
            this.BoxNumber = pod.BoxNumber;
            this.Weight = pod.Weight;
            this.GoodsNumber = pod.GoodsNumber;
            this.Volume = pod.Volume;
            this.Creator = pod.Creator;
            this.CreateTime = pod.CreateTime;
            this.Str1 = pod.Str1;
            this.Str2 = pod.Str2;
            this.Str3 = pod.Str3;
            this.Str4 = pod.Str4;
            this.Str5 = pod.Str5;
            this.Str6 = pod.Str6;
            this.Str7 = pod.Str7;
            this.Str8 = pod.Str8;
            this.Str9 = pod.Str9;
            this.Str10 = pod.Str10;
            this.Str11 = pod.Str11;
            this.Str12 = pod.Str12;
            this.Str13 = pod.Str13;
            this.Str14 = pod.Str14;
            this.Str15 = pod.Str15;
            this.Str16 = pod.Str16;
            this.Str17 = pod.Str17;
            this.Str18 = pod.Str18;
            this.Str19 = pod.Str19;
            this.Str20 = pod.Str20;
            this.Str21 = pod.Str21;
            this.Str22 = pod.Str22;
            this.Str23 = pod.Str23;
            this.Str24 = pod.Str24;
            this.Str25 = pod.Str25;
            this.Str26 = pod.Str26;
            this.Str27 = pod.Str27;
            this.Str28 = pod.Str28;
            this.Str29 = pod.Str29;
            this.Str30 = pod.Str30;
            this.Str31 = pod.Str31;
            this.Str32 = pod.Str32;
            this.Str33 = pod.Str33;
            this.Str34 = pod.Str34;
            this.Str35 = pod.Str35;
            this.Str36 = pod.Str36;
            this.Str37 = pod.Str37;
            this.Str38 = pod.Str38;
            this.Str39 = pod.Str39;
            this.Str40 = pod.Str40;
            this.Str41 = pod.Str41;
            this.Str42 = pod.Str42;
            this.Str43 = pod.Str43;
            this.Str44 = pod.Str44;
            this.Str45 = pod.Str45;
            this.Str46 = pod.Str46;
            this.Str47 = pod.Str47;
            this.Str48 = pod.Str48;
            this.Str49 = pod.Str49;
            this.Str50 = pod.Str50;
            this.DateTime1 = pod.DateTime1;
            this.DateTime2 = pod.DateTime2;
            this.DateTime3 = pod.DateTime3;
            this.DateTime4 = pod.DateTime4;
            this.DateTime5 = pod.DateTime5;
            this.DateTime6 = pod.DateTime6;
            this.DateTime7 = pod.DateTime7;
            this.DateTime8 = pod.DateTime8;
            this.DateTime9 = pod.DateTime9;
            this.DateTime10 = pod.DateTime10;
            this.DateTime11 = pod.DateTime11;
            this.DateTime12 = pod.DateTime12;
            this.DateTime13 = pod.DateTime13;
            this.DateTime14 = pod.DateTime14;
            this.DateTime15 = pod.DateTime15;
            this.IsSettledForCustomer = pod.IsSettledForCustomer;
            this.IsSettledForShipper = pod.IsSettledForShipper;
            this.Type = pod.Type;
            this.HasShortDial = pod.HasShortDial;
            this.HasDistribution = pod.HasDistribution;
            this.HasExpress = pod.HasExpress;
            this.AttachmentGroupID = string.Empty;
            this.IsUploadPod = 0;
            this.wxStatus = pod.wxStatus;
        }
    }
}