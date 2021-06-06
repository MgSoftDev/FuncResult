using System;
using System.Runtime.CompilerServices;

namespace FuncResult
{
    public class UnfinishedInfo
    {
        public enum NotifyType
        {
            Information,
            Success,
            Warning,
            Error
        }
        public string ErrorCode { get; set; }
        public string Title { get; set; }
        public string Mensaje { get; set; }

        public NotifyType Type { get; set; }
        public UnfinishedInfo()
        {
            
        }
        public UnfinishedInfo( string title, string mensaje = null,NotifyType notifyType = NotifyType.Information,  string errorCode =null )
        {
            Title = title;
            Mensaje = mensaje;
            ErrorCode = errorCode;
            Type = notifyType;
        }

        public void Throw() { throw new ReturningUnfinishedException( this ); }
    }

}
