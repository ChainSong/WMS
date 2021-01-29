using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Logger.LogHelper
{
    public class CustomLayout : log4net.Layout.PatternLayout
    {
        public CustomLayout()
        {
            this.AddConverter("Property", typeof(MyPatternConverter));
            //this.AddConverter("content", typeof(UserPhonePatternConverter));
            //this.AddConverter("user", typeof(IPPatternConverter));
            //this.AddConverter("operation", typeof(ControllerNamePatternConverter));
        }
    }

    //internal sealed class UserPhonePatternConverter : PatternLayoutConverter
    //{
    //    override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
    //    {
    //        LogContent logMessage = loggingEvent.MessageObject as LogContent;

    //        if (logMessage != null)
    //            writer.Write(logMessage.content);
    //    }
    //}

    //internal sealed class IPPatternConverter : PatternLayoutConverter
    //{
    //    override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
    //    {
    //        LogContent logMessage = loggingEvent.MessageObject as LogContent;

    //        if (logMessage != null)
    //            writer.Write(logMessage.user);
    //    }
    //}

    //internal sealed class ControllerNamePatternConverter : PatternLayoutConverter
    //{
    //    override protected void Convert(TextWriter writer, LoggingEvent loggingEvent)
    //    {
    //        LogContent logMessage = loggingEvent.MessageObject as LogContent;

    //        if (logMessage != null)
    //            writer.Write(logMessage.operation);
    //    }
    //}

}
