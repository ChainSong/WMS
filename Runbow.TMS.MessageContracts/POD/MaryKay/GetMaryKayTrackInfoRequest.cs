using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.POD.MaryKay
{
    public  class GetMaryKayTrackInfoRequest
    {
        public string SqlWhere { get; set; }
        public DataTable TrackInfoTable { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public string ID { get; set; }


        public string UpLoadMKStatus { get; set; }
        public int  PodTrackID { get; set; }
        public string TrackInfo { get; set; }


        public string CustomerOrderNumber { get; set; }
        public int PODStatusID { get; set; }
        public string PODStatusName { get; set; }
        public string TrackComment { get; set; }

        public string ResponsibilityOwner { get; set; }
        public DateTime? TrackTime { get; set; }
        public string SignName { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
