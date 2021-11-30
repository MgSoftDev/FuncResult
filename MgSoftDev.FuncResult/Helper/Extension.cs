using System;

namespace MgSoftDev.FuncResult.Helper
{
    public static class Extension
    {
        public static void OkNotNullAction<T>(this Returning<T> returning, Action<T> action )
        {
            if (returning.OkNotNull)
                action(returning.Val);
        }


        public static string Format( this string val, params object[] args )
        {
            if( val == null  ) return "";

            return args == null ? val : string.Format( val, args );
        }

        public static ReturningEnums.LogLevel ToLogLevel(this UnfinishedInfo.NotifyType value)
        {
            switch ( value )
            {
                case UnfinishedInfo.NotifyType.Information: return ReturningEnums.LogLevel.Info;
                case UnfinishedInfo.NotifyType.Warning:     return ReturningEnums.LogLevel.Warn;
                case UnfinishedInfo.NotifyType.Error:       return ReturningEnums.LogLevel.Error;
                case UnfinishedInfo.NotifyType.Success:       return ReturningEnums.LogLevel.Info;
                default:                                    return ReturningEnums.LogLevel.Info;
            }
        }


    }
}
