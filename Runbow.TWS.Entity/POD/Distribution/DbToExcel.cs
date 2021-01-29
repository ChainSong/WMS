using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD.Distribution
{
   public  class DbToExcel:Pod
    {
        //PodID, 
        //SystemNumber,
        //CustomerOrderNumber,
        //Creator,
       [EntityPropertyExtension("FeeStr1", "FeeStr1")]
       public  string FeeStr1{get;set;}
       [EntityPropertyExtension("FeeStr2", "FeeStr2")]
       public  string FeeStr2{get;set;}
       [EntityPropertyExtension("FeeStr3", "FeeStr3")]
       public  string FeeStr3{get;set;}
       [EntityPropertyExtension("FeeStr4", "FeeStr4")]
       public  string FeeStr4{get;set;}
       [EntityPropertyExtension("FeeStr5", "FeeStr5")]
       public  string FeeStr5{get;set;}
      [EntityPropertyExtension("FeeStr6", "FeeStr6")]
      public  string FeeStr6{get;set;}
      [EntityPropertyExtension("FeeStr7", "FeeStr7")]
      public  string FeeStr7{get;set;}
      [EntityPropertyExtension("FeeStr8", "FeeStr8")]
      public  string FeeStr8{get;set;}
      [EntityPropertyExtension("FeeStr9", "FeeStr9")]
      public  string FeeStr9{get;set;}
      [EntityPropertyExtension("FeeStr10", "FeeStr10")]
      public  string FeeStr10{get;set;}
      [EntityPropertyExtension("FeeCreateTime", "FeeCreateTime")]
      public DateTime FeeCreateTime{get;set;}
      [EntityPropertyExtension("FeeDecimal1", "FeeDecimal1")]
      public decimal FeeDecimal1{get;set;}
     [EntityPropertyExtension("FeeDecimal2", "FeeDecimal2")]
      public decimal FeeDecimal2{get;set;}
      [EntityPropertyExtension("FeeDecimal3", "FeeDecimal3")]
      public decimal FeeDecimal3{get;set;}
      [EntityPropertyExtension("FeeDecimal4", "FeeDecimal4")]
      public decimal FeeDecimal4{get;set;}
      [EntityPropertyExtension("FeeDecimal5", "FeeDecimal5")]
      public decimal FeeDecimal5{get;set;}
      [EntityPropertyExtension("FeeDecimal6", "FeeDecimal6")]
      public decimal FeeDecimal6{get;set;}
      [EntityPropertyExtension("FeeDecimal7", "FeeDecimal7")]
      public decimal FeeDecimal7{get;set;}
      [EntityPropertyExtension("FeeDecimal8", "FeeDecimal8")]
      public decimal FeeDecimal8{get;set;}
      [EntityPropertyExtension("FeeDecimal9", "FeeDecimal9")]
      public decimal FeeDecimal9{get;set;}
      [EntityPropertyExtension("FeeDecimal10", "FeeDecimal10")]
      public decimal FeeDecimal10{get;set;}
      [EntityPropertyExtension("FeeDecimal11", "FeeDecimal11")]
      public decimal FeeDecimal11{get;set;}
      [EntityPropertyExtension("FeeInt1", "FeeInt1")]
      public int FeeInt1 { get; set; }
    }
}
