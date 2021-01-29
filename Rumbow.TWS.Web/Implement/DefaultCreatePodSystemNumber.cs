using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Interface;

namespace Runbow.TWS.Web.Implement
{
    public class DefaultCreatePodSystemNumber : ICreatePodSystemNumber
    {
        public string CreatePodSystemNumber()
        {
            return string.Concat("Runbow", DateTime.Now.ToString("yyyyMMddHHmmssff"), "0001");
        }

        public int GetTodaysPodNumber(out string systemNumberPrefix)
        {
            long projectID = (long)(HttpContext.Current.Session["ProjectID"]);
            string projectName = HttpContext.Current.Session["ProjectName"].ToString();
            systemNumberPrefix = string.Concat(projectName, DateTime.Today.ToString("yyyyMMdd"));
            var response = new PodService().GetTodaysPodCount(new GetTodaysPodCountRequest() { ProjectID = projectID, SystemNumberPrefix = systemNumberPrefix });

            if (response.IsSuccess)
            {
                return response.Result;
            }

            throw response.Exception;
        }
    }
}