using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.POD.Models.TotalModel
{
    public class MessageHistoryQueryModel
    {
        public string CustomerOrderNumber { get; set; }
        public string Phone { get; set; }
        public DataTable MessageHistoryTable { get; set; }
        public DateTime? BeginSendTime { get; set; }
        public DateTime? EndSendTime { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public bool  IsExprot { get; set; }
    }
}