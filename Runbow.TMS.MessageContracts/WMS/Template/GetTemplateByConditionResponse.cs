using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.Template
{
    public class GetTemplateByConditionResponse
    {
        public IEnumerable<TableColumn> TemplateCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
