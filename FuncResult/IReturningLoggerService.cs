using System.Threading.Tasks;

namespace FuncResult
{
    public interface IReturningLoggerService
    {
        bool SaveLog(  Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null );

        
        bool       SaveLog(  ErrorInfo error,     ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null );
        Task<bool> SaveLogAsync(Returning   returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null);


        Task<bool> SaveLogAsync(ErrorInfo error, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null);

        bool       SaveLog(UnfinishedInfo error, string logName = null);
        Task<bool> SaveLogAsync(UnfinishedInfo error, string logName = null);
    }
}
