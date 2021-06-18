using System;
using System.Reflection;

namespace FuncResult
{
    public static class ReturningLogger
    {
        public static IReturningLoggerService LoggerService;

        public static Returning SaveLog(this Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null)
        {
            if (returning == null || returning.IsLogStored || LoggerService ==  null) return returning;

            
            logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
            if (LoggerService.SaveLog( returning, logLevel, logName ))
                returning.IsLogStored = true;
            
            return returning;
        }

        public static Returning<T> SaveLog<T>(this Returning<T> returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null)
        {
            if (returning == null || returning.IsLogStored || LoggerService == null) return returning;
            logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
            ((Returning)returning).SaveLog(logLevel, logName);

            return returning;
        }

        public static ErrorInfo SaveLog(this ErrorInfo error, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null)
        {
            if (error == null || LoggerService == null) return error;
            logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
            LoggerService.SaveLog(error, logLevel, logName);

            return error;
        }

       

    }
}
