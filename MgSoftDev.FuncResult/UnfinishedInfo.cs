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

        public string   ErrorCode         { get; set; }
        public string   Title             { get; set; }
        public string   Mensaje           { get; set; }
        public bool     UseLocalization   { get; set; }
        public object   Parameters        { get; set; }
        public object[] TitleFormatArgs   { get; set; }
        public object[] MensajeFormatArgs { get; set; }

        public NotifyType Type { get; set; }
        public UnfinishedInfo() { }

        public UnfinishedInfo(string title, string mensaje, NotifyType notifyType = NotifyType.Information, string errorCode = null)
        {
            Title             = title;
            Mensaje           = mensaje;
            ErrorCode         = errorCode;
            Type              = notifyType;
            UseLocalization   = false;
            Parameters        = null;
            TitleFormatArgs   = null;
            MensajeFormatArgs = null;
        }
        public UnfinishedInfo(string title,  NotifyType notifyType = NotifyType.Information, string errorCode = null)
        {
            Title             = title;
            Mensaje           = null;
            Type              = notifyType;
            UseLocalization   = false;
            TitleFormatArgs   = null;
            MensajeFormatArgs = null;
            ErrorCode         = errorCode;
            Parameters        = null;
        }

        public static UnfinishedInfo FromLocalization(string titleKey, string mensajeKey, object[] titleFormatArgs = null,object[] mensajeFormatArgs = null, NotifyType notifyType = NotifyType.Information, string errorCode = null)
        {
            return new UnfinishedInfo()
            {
                Title             = titleKey,
                Mensaje           = mensajeKey,
                Type              = notifyType,
                UseLocalization   = true,
                TitleFormatArgs   = titleFormatArgs,
                MensajeFormatArgs = mensajeFormatArgs,
                ErrorCode         = errorCode,
                Parameters        = null,
            };
        }
        public static UnfinishedInfo FromLocalization(string titleKey, object[] titleFormatArgs = null, NotifyType notifyType = NotifyType.Information, string errorCode = null)
        {
            return new UnfinishedInfo()
            {
                Title             = titleKey,
                Mensaje           = null,
                Type              = notifyType,
                UseLocalization   = true,
                TitleFormatArgs   = titleFormatArgs,
                MensajeFormatArgs = null,
                ErrorCode         = errorCode,
                Parameters        = null,
            };
        }

        public void Throw() { throw new ReturningUnfinishedException(this); }
    }
}
