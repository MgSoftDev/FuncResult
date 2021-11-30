using System;
using System.Runtime.CompilerServices;
using FuncResult.Exceptions;

namespace FuncResult
{
    public class ErrorInfo
    {
        public const string UnhandledError = "UnhandledError";


        public string                       ErrorMessage { get; set; }
        public string                       ErrorCode    { get; set; }
        public string                       MemberName   { get; set; }
        public string                       FilePath     { get; set; }
        public int                          LineNumber   { get; set; }
        public Exception                    TryException { get; set; }
        public object Parameters   { get; set; } 

        public ErrorInfo()
        {
           
        }

        public ErrorInfo(string errorMessage, Exception tryException = null, string errorCode="", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            ErrorMessage = errorMessage;
            TryException = tryException;
            MemberName   = memberName;
            FilePath     = filePath;
            LineNumber   = lineNumber;
            ErrorCode    = errorCode;
        }
        public ErrorInfo(string errorMessage, object parameters,  Exception tryException = null, string errorCode = "", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            ErrorMessage = errorMessage;
            TryException = tryException;
            MemberName   = memberName;
            FilePath     = filePath;
            LineNumber   = lineNumber;
            Parameters   = parameters;
            ErrorCode    = errorCode;
        }

        public void Throw() { throw new ReturningException( this ); }
    }

}
