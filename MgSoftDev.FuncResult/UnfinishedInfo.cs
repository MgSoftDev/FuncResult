using MgSoftDev.FuncResult.Exceptions;

namespace MgSoftDev.FuncResult
{
    public class UnfinishedInfo
    {
        public enum NotifyType
        {
            Information = 1,
            Success     = 2,
            Warning     = 3,
            Error       = 4
        }

        public string ErrorCode       { get; set; }
        public string Title           { get; set; }
        public string Mensaje         { get; set; }
        public bool   UseLocalization { get; set; }
        public object Parameters      { get; set; }

        public NotifyType Type { get; set; }
        public UnfinishedInfo() { }

        public UnfinishedInfo(string title, string mensaje = null, NotifyType notifyType = NotifyType.Information, bool useLocalization = false, string errorCode = null, object parameters = null)
        {
            Title           = title;
            Mensaje         = mensaje;
            ErrorCode       = errorCode;
            Type            = notifyType;
            UseLocalization = useLocalization;
            Parameters      = parameters;
        }

        public void Throw() { throw new ReturningUnfinishedException(this); }
    }
}
