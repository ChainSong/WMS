using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Runbow.TWS.Logger.LogHelper
{
    public class LogContent
    {
        public string content { get; set; }
        public string user { get; set; }
        public string operation { get; set; }
        public string logLevel { get; set; }

        public LogContent(string Content, string User, string Oeration, LogLevel logLevel)
        {
            this.content = Content;
            this.user = User;
            this.operation = Oeration;
            this.logLevel = logLevel.ToString();
        }

    }
    public class MyPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            if (Option != null)
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            else
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
        }

        //通过反射获取传入的日志对象的某个属性的值
        private object LookupProperty(string property, log4net.Core.LoggingEvent loggingEvent)
        {
            object propertyvalue = string.Empty;
            PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);

            if (propertyInfo != null)
                propertyvalue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
            return propertyvalue;
        }
    }
}