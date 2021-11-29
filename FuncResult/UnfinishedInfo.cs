using FuncResult.Exceptions;

namespace FuncResult
{
    public class UnfinishedInfo
    {
        public enum NotifyType : int
        {
            Information=2,
            Success=2,
            Warning=3,
            Error=4
        }
        public string                       ErrorCode       { get; set; }
        public string                       Title           { get; set; }
        public string                       Mensaje         { get; set; }
        public bool                         UseLocalization { get; set; }
        public (string Key, string Value)[] KeysValues      { get; set; }

        public NotifyType Type { get; set; }
        public UnfinishedInfo()
        {
            
        }
        public UnfinishedInfo( string title, string mensaje = null,NotifyType notifyType = NotifyType.Information,bool useLocalization = false , string errorCode =null , params (string Key, string Value)[] keysValues)
        {
            Title           = title;
            Mensaje         = mensaje;
            ErrorCode       = errorCode;
            Type            = notifyType;
            UseLocalization = useLocalization;
            KeysValues    = keysValues;
        }

        public void Throw() { throw new ReturningUnfinishedException( this ); }
    }

}
