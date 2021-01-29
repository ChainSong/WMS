using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Common
{
    public class ProvincesAndCity
    {
        /// <summary>
        /// 省
        /// </summary>
        protected string Provinces;

        // <summary>
        /// 城市
        /// </summary>
        protected string City;


        public ProvincesAndCity(string Provinces, string City)
        {
            this.Provinces = Provinces;
            this.City = City; 
            var pid = ApplicationConfigHelper.GetRegions().Where(q => q.Grade == 2);
        }
        /// <summary>
        /// 用户自定义(自己写的玩儿)
        /// </summary>
        /// <returns></returns>
        public virtual void CustomerCity()
        {

        }
    }
}