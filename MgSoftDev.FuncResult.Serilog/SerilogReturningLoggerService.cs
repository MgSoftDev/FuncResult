using System;
using System.Reflection;
using System.Threading.Tasks;
using FuncResult;
using FuncResult.Helper;
using FuncResult.Logger;
using Serilog;
using Serilog.Events;

namespace MgSoftDev.FuncResult.Serilog
{

	public class SerilogReturningLoggerService : IReturningLoggerService
	{
		#region Implementation of IReturningLoggerService

		public bool SaveLog(Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null)
		{
			try
			{
				logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
				logName = logName?.Replace('.', ' ');

				returning.Errors.ForEach(e =>
				{
					var param = new
					{

						Date = DateTime.Now,
						LogLevel = logLevel,
						LogName = logName,
                        e.ErrorMessage,
                        e.Parameters,
                        e.ErrorCode,
                        e.TryException,
                        e.MemberName,
                        e.FilePath,
                        e.LineNumber
					};
					Log.Write(ConvertToLogEventLevel(logLevel), "<Schema>{0}</Schema> <LogData>{@param}</LogData>", "ErrorInfo", param);
				});
				return true;
			}
			catch (Exception ex)
			{
				returning.LogException = ex;

				return false;
			}
		}

		public Task<bool> SaveLogAsync(Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null)
		{
			return Task.Run(() => SaveLog(returning, logLevel, logName));
		}

		public bool SaveLog(UnfinishedInfo unfinished, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null)
		{
			try
			{
				logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
				logName = logName?.Replace('.', ' ');

				var param = new
				{
					Date = DateTime.Now,
					LogLevel = logLevel,
					LogName = logName,
                    unfinished.Title,
                    unfinished.Mensaje,
                    unfinished.UseLocalization,
                    unfinished.Parameters,
					NotifyType = unfinished.Type

				};
				Log.Write(ConvertToLogEventLevel(logLevel), "<Schema>{0}</Schema> <LogData>{@param}</LogData>", "UnfinishedInfo", param);

			}
			catch (Exception)
			{

				return false;
			}

			return true;
		}

		public Task<bool> SaveLogAsync(UnfinishedInfo unfinished, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null)
		{
			return Task.Run(() => SaveLog(unfinished, logLevel, logName));
		}

		public bool SaveLog(string errorMessage, object parameters, Exception tryException = null, string errorCode = "", ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error,
							string logName = null, string memberName = null, string filePath = null, int lineNumber = 0)
		{
			try
			{
				logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
				logName = logName?.Replace('.', ' ');

				var param = new
				{

					Date = DateTime.Now,
					LogLevel = logLevel,
					LogName = logName,
					ErrorMessage = errorMessage,
					Parameters = parameters,
					ErrorCode = errorCode,
					TryException = tryException,
					MemberName = memberName,
					FilePath = filePath,
					LineNumber = lineNumber
				};
				Log.Write(ConvertToLogEventLevel(logLevel), "<Schema>{0}</Schema> <LogData>{@param}</LogData>", "ErrorInfo", param);



			}
			catch (Exception)
			{

				return false;
			}

			return true;
		}

		public Task<bool> SaveLogAsync(string errorMessage, object parameters, Exception tryException = null, string errorCode = "", ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error,
									   string logName = null, string memberName = null, string filePath = null, int lineNumber = 0)
		{
			return Task.Run(() => SaveLog(errorMessage, parameters, tryException, errorCode, logLevel, logName, memberName, filePath, lineNumber));
		}



		private LogEventLevel ConvertToLogEventLevel(ReturningEnums.LogLevel logLevel)
		{
			switch (logLevel)
			{
				case ReturningEnums.LogLevel.Trace: return LogEventLevel.Debug;
				case ReturningEnums.LogLevel.Debug: return LogEventLevel.Debug;
				case ReturningEnums.LogLevel.Info: return LogEventLevel.Information;
				case ReturningEnums.LogLevel.Warn: return LogEventLevel.Warning;
				case ReturningEnums.LogLevel.Error: return LogEventLevel.Error;
				case ReturningEnums.LogLevel.Fatal: return LogEventLevel.Fatal;

				default: return LogEventLevel.Error;
			}
		}

		#endregion
	}

}
