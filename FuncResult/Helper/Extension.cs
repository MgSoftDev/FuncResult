using System;

namespace FuncResult.Helper
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
        
    }
}
