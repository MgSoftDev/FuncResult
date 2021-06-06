using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FuncResult
{
    public class ErrorInfo
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public string MemberName { get; set; }
        public string FilePath { get; set; }
        public int LineNumber { get; set; }
        public Exception TryException { get; set; }

        public ErrorInfo()
        {
           
        }

        public ErrorInfo(string errorMessage, Exception tryException = null,[CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            ErrorMessage = errorMessage;
            TryException = tryException;
            MemberName = memberName;
            FilePath = filePath;
            LineNumber = lineNumber;
        }

        public void Throw() { throw new ReturningException( this ); }
    }

}
