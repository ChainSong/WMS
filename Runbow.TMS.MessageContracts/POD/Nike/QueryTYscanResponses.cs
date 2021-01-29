using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class QueryTYscanResponses
    {
        public IEnumerable<TYscan> TYscanCollection { get; set; }
        public IEnumerable<TYscanGroupBy> TYscanGroupByCollection { get; set; }
        public IEnumerable<TYscanDetail> TYscanDetailCollection { get; set; }
        

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
