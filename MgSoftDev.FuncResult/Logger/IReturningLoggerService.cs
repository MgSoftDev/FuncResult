using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.FuncResult.Helper;

namespace MgSoftDev.FuncResult.Logger
{
    public interface IReturningLoggerService
    {
        bool       SaveLog(Returning      returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null);
        Task<bool> SaveLogAsync(Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null);



        bool       SaveLog(UnfinishedInfo      unfinished, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null);
        Task<bool> SaveLogAsync(UnfinishedInfo unfinished, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null);



        bool SaveLog(string errorMessage, object parameters, Exception tryException = null, string errorCode = "", ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        Task<bool> SaveLogAsync(string errorMessage, object parameters, Exception tryException = null, string errorCode = "", ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
    }
}
