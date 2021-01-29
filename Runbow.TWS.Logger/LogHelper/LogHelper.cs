using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Logger.LogHelper
{
    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevel
    {
        Error,
        Debug,
        Warning,
        Info,
        Success,
    }
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        SysLog,
        ADONetAppender
    }
    /// <summary>
    /// 日志记录服务
    /// </summary>
    public static class SysLogWriter
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        //private   log4net.ILog m_Log;

        //public void Init(LogType _LogType = LogType.SysLog)
        //{
        //    string s = _LogType.ToString();
        //    m_Log = log4net.LogManager.GetLogger(s);
        //}
        /// <summary>
        /// 输出错误级别日志
        /// </summary>
        /// <param name="message">输出的消息</param>
        public static void Error(object message, LogType _LogType = LogType.SysLog)
        {
            WriteLog(LogLevel.Error, message, _LogType);
        }

        /// <summary>
        /// 输出警告级别日志
        /// </summary>
        /// <param name="message">输出的消息</param>
        public static void Warning(object message, LogType _LogType = LogType.SysLog)
        {
            //log4net.LogManager.GetLogger(s);
            //记录日志
            WriteLog(LogLevel.Warning, message, _LogType);
        }

        /// <summary>
        /// 输出信息级别日志
        /// </summary>
        /// <param name="message">输出的消息</param>
        public static void Info(object message, LogType _LogType = LogType.SysLog)
        {
            //记录日志
            WriteLog(LogLevel.Info, message, _LogType);
        }

        /// <summary>
        /// 输出调试级别日志
        /// </summary>
        /// <param name="message">输出的消息</param>
        public static void Debug(object message, LogType _LogType = LogType.SysLog)
        {
            //记录日志
            WriteLog(LogLevel.Debug, message, _LogType);
        }

        /// <summary>
        /// 记录系统日志
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="message">输出的消息</param>
        private static void WriteLog(LogLevel logLevel, object message, LogType _LogType = LogType.SysLog)
        {
            string s = _LogType.ToString();
            log4net.ILog m_Log = log4net.LogManager.GetLogger(s);
            switch (logLevel)
            {
                case LogLevel.Debug:
                    m_Log.Debug(message);
                    break;
                case LogLevel.Error:
                    m_Log.Error(message);
                    break;
                case LogLevel.Info:
                    m_Log.Info(message);
                    break;
                case LogLevel.Warning:
                    m_Log.Warn(message);
                    break;
                case LogLevel.Success:
                    m_Log.Info(message);
                    break;
            }

        }
        /// <summary>
        /// 记录系统日志
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="message">输出的消息</param>
        //private void WriteLog(LogLevel logLevel, string message)
        //{
        //    switch (logLevel)
        //    {
        //        case LogLevel.Debug:
        //            m_Log.Debug(message);
        //            break;
        //        case LogLevel.Error:
        //            m_Log.Error(message);
        //            break;
        //        case LogLevel.Info:
        //            m_Log.Info(message);
        //            break;
        //        case LogLevel.Warning:
        //            m_Log.Warn(message);
        //            break;
        //    }

        //}

    }
}
