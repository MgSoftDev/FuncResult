using System;

namespace FuncResult
{
    public static class Extension
    {
        public static void OkNotNullAction<T>(this Returning<T> returning, Action<T> action )
        {
            if (returning.OkNotNull)
                action(returning.Val);
        }

        
    }
}
