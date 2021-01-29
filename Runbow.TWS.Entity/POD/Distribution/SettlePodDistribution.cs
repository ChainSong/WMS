using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD.Distribution
{
   public class SettlePodDistribution
    {
      [EntityPropertyExtension("FeePodID", "FeePodID")]
       public long FeePodID { get; set; }
       [EntityPropertyExtension("FeeSystemNumber", "FeeSystemNumber")]
       public string FeeSystemNumber { get; set; }
       [EntityPropertyExtension("FeeCustomOrerderNunber", "FeeCustomOrerderNunber")]
       public string FeeCustomOrerderNunber { get; set; }
       [EntityPropertyExtension("FeeCreator", "FeeCreator")]
       public string FeeCreator { get; set; }
       [EntityPropertyExtension("FeeCreatorTime", "FeeCreatorTime")]
       public DateTime FeeCreatorTime { get; set; }
       //发车批次号
       [EntityPropertyExtension("FeeStr1", "FeeStr1")]
       public string FeeStr1 { get; set; }
       //承运商
       [EntityPropertyExtension("FeeStr2", "FeeStr2")]
       public string FeeStr2 { get; set; }
       //车牌号
       [EntityPropertyExtension("FeeStr3", "FeeStr3")]
       public string FeeStr3 { get; set; }
       //总重量
       [EntityPropertyExtension("FeeStr4", "FeeStr4")]
       public string FeeStr4 { get; set; }
       //车型
       [EntityPropertyExtension("FeeStr5", "FeeStr5")]
       public string FeeStr5 { get; set; }
       //备注
       [EntityPropertyExtension("FeeStr10", "FeeStr10")]
       public string FeeStr10 { get; set; }
       //提货费(从工厂)
       [EntityPropertyExtension("Decimal1", "Decimal1")]
       public decimal? FeeDecimal1 { get; set; }
       //提货费(从嘉托)
       [EntityPropertyExtension("Decimal2", "Decimal2")]
       public decimal? FeeDecimal2 { get; set; }
       //卸车费
       [EntityPropertyExtension("Decimal3", "Decimal3")]
       public decimal? FeeDecimal3 { get; set; }
       //油费
       [EntityPropertyExtension("Decimal4", "Decimal4")]
       public decimal? FeeDecimal4 { get; set; }
       //包车费
       [EntityPropertyExtension("Decimal5", "Decimal5")]
       public decimal? FeeDecimal5 { get; set; }
       //起步费
       [EntityPropertyExtension("Decimal6", "Decimal6")]
       public decimal? FeeDecimal6 { get; set; }
       //点费
       [EntityPropertyExtension("Decimal7", "Decimal7")]
       public decimal? FeeDecimal7 { get; set; }
       //通行证费
       [EntityPropertyExtension("Decimal8", "Decimal8")]
       public decimal? FeeDecimal8 { get; set; }
       //其他费用
       [EntityPropertyExtension("Decimal9", "Decimal9")]
       public decimal? FeeDecimal9 { get; set; }
       //合计
       [EntityPropertyExtension("Decimal10", "Decimal10")]
       public decimal? FeeDecimal10 { get; set; }
       //总计
       [EntityPropertyExtension("Decimal11", "Decimal11")]
       public decimal? FeeDecimal11 { get; set; }
       //点数
       [EntityPropertyExtension("FeeInt1", "FeeInt1")]
       public int? FeeInt1 { get; set; }


    }
}
