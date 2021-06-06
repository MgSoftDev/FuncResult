namespace FuncResult
{
    public interface IReturningLoggerService
    {
        bool SaveLog(  Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null );

        
        bool SaveLog(  ErrorInfo error, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null );


    }
}
