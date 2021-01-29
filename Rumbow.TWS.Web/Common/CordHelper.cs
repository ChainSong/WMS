using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.MessageContracts;
using System.Threading.Tasks;
using Runbow.TWS.Biz.WMS;

namespace Runbow.TWS.Web.Common
{
    public class CordHelper
    {
        //public List<WMS_Cord_Operation> list = new List<WMS_Cord_Operation>();

        public async static Task<string> AddWMS_Cord_FeedBack(IEnumerable<WMS_Cord_Operation> list)
        {
            string message = await Task.Run(() =>
            {
                CordOperationService service = new CordOperationService();
                var response = service.AddCordOperation(list);

                return response.Result;

            }).ConfigureAwait(false);
            return message;
        }


    }
}