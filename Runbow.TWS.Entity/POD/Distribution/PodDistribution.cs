using System;
using System.Collections.Generic;

namespace Runbow.TWS.Entity
{
    public class PodDistribution : Pod
    {
        public bool IsPaging { get; set; }
        //是否分配
        public int IsintDistribution { get; set; }

        public  bool IsDistribution;
        
        //发车批次号
        public string FeeStr1 { get; set; }
        //承运商
        public string FeeStr2 { get; set; }
        //车牌号
        public string FeeStr3 { get; set; }
        //整车的重量
        public string FeeStr4 { get; set; }
        //备注
        public string FeeStr10 { get; set; }
        //车型
        public int IntCarModels{get;set;}
        public string FeeStr5 { get; set; }
        //public string StrCarModels { get; set; }

        public DateTime? EndActualDeliveryDate { get; set; }

        public bool IsExport { get; set; }

        public string podID { get; set; }

    }
}