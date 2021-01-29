using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetContractByConditionRequest
    {
        public ContractSearchCondition SearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        //0 全显示 1 显示即将过期 2 显示已过期
        public int? SearchType1 { get; set; }

        //0 全显示 1 显示未过期 2 显示已过期
        public int? SearchType2 { get; set; }
    }
}
