using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class ContractConfig
    {
        [EntityPropertyExtension("Code", "Code")]
        public string Code { get; set; }  //参数编号

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }  //参数名称
    }
}
