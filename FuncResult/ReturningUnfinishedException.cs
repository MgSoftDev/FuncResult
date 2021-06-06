using System.Linq;

namespace FuncResult
{
    public class ReturningUnfinishedException : System.Exception
    {
        public UnfinishedInfo Error { get; set; }
        public Returning Result { get; set; }

        public ReturningUnfinishedException( Returning result )
        {
            Result = result;
            Error = Result.UnfinishedItems?.FirstOrDefault();
        }

        public override string ToString()
        {
            return $"Title:{Error?.Title}\nMensaje:{Error?.Mensaje}\nErrorCode:{Error?.ErrorCode}\nType:{Error?.Type}";

        }
    }
}
