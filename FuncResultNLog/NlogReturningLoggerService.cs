using System;
using System.Reflection;
using System.Threading.Tasks;
using FuncResult;
using NLog;
using NLog.LayoutRenderers;

namespace FuncResultNLog
{
    public  class NlogReturningLoggerService : IReturningLoggerService
    {
        public readonly NlogConfig NlogConfig;

        public NlogReturningLoggerService( string filePath, bool enableConsoleresult = false )
        {
            NlogConfig = new NlogConfig( filePath , enableConsoleresult);

            NlogConfig.AddStackTrace()
                    .AddCallSiteLine( 1 )
                    .AddCallSite(
                            new CallSiteLayoutRenderer() { ClassName = true, CleanNamesOfAnonymousDelegates = true, FileName = true, IncludeSourcePath = true, MethodName = true, SkipFrames = 0 } )
                    ;
        }


        public bool SaveLog( Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null )
        {
            try
            {
                logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
                logName = logName?.Replace( '.', ' ' );

                var level = LogLevel.FromOrdinal( (int) logLevel );


                var logger = LogManager.GetLogger( "Returning" );

                returning.Errors.ForEach( e =>
                {
                    var theEvent = new LogEventInfo( level, logName, null, e.ErrorMessage, null, e.TryException );
                    theEvent.Properties["MemberName"] = e.MemberName;
                    theEvent.Properties["FilePath"]   = e.FilePath;
                    theEvent.Properties["LineNumber"] = e.LineNumber;
                    logger.Log( theEvent );
                } );

            }
            catch( Exception ex )
            {
                returning.LogException= ex;

                return false;
            }

            return true;
        }

        public  bool SaveLog( ErrorInfo error, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null)
        {
            try
            {

                logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
                logName = logName?.Replace( '.', ' ' );

                var level = LogLevel.FromOrdinal( (int) logLevel );
                
                var logger = LogManager.GetLogger( "Returning" );

                var theEvent = new LogEventInfo( level, logName, null, error.ErrorMessage, null, error.TryException );
                theEvent.Properties["AssemblyName"] = logName;
                theEvent.Properties["MemberName"] = error.MemberName;
                theEvent.Properties["FilePath"]   = error.FilePath;
                theEvent.Properties["LineNumber"] = error.LineNumber;
                logger.Log( theEvent );
            }
            catch( Exception ex )
            {
                return false;
            }

            return true;
        }

        public Task<bool> SaveLogAsync( Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null )
        {
            return Task.Run( ( )=>SaveLog( returning, logLevel, logName ) );
        }

        public Task<bool> SaveLogAsync( ErrorInfo error, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, string logName = null )
        {
            return Task.Run(() => SaveLog(error, logLevel, logName));
        }

        public bool SaveLog(UnfinishedInfo error,  string logName = null)
        {
            try
            {

                logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
                logName = logName?.Replace('.', ' ');

                var level = LogLevel.FromOrdinal((int)error.Type);

                var logger = LogManager.GetLogger("Returning");

                var theEvent = new LogEventInfo(level, logName, null, error.Title + "  "+ error.Mensaje, null, null);
                theEvent.Properties["AssemblyName"] = logName;
                logger.Log(theEvent);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public Task<bool> SaveLogAsync(UnfinishedInfo error,  string logName = null)
        {
            return Task.Run(() => SaveLog(error,  logName));
        }
    }
}