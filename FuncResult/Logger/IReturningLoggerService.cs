using System.Threading.Tasks;
using FuncResult.Helper;

namespace FuncResult.Logger
{
    public interface IReturningLoggerService
    {
        bool       SaveLog(Returning      returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null);
        Task<bool> SaveLogAsync(Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null);
    }
}
