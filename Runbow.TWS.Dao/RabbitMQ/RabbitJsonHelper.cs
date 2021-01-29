using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Dao.RabbitMQ
{
    public class RabbitJsonHelper
    {
        private static string _typeName;

        public static JsonSerializerSettings GetJsonSerializerSettings(string typeName)
        {
            _typeName = typeName;
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Binder = new AssemblySerializeBinder(_typeName),
                ContractResolver = new CamelCasePropertyNamesContractResolver()
                //TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                //DateFormatHandling = DateFormatHandling.IsoDateFormat,
                //DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }
    }
}
