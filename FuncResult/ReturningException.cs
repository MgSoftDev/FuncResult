using System.Linq;

namespace FuncResult
{
    public class ReturningException : System.Exception
    {
        public ErrorInfo Error { get; set; }
        public Returning Result { get; set; }

        public ReturningException( Returning result )
        {
            Result = result;
            Error = Result.Errors?.FirstOrDefault();
        }

        public override string ToString()
        {
            return $"ErrorMessage:{Error?.ErrorMessage}\nErrorCode:{Error?.ErrorCode}\nMemberName:{Error?.MemberName}\nLineNumber:{Error?.LineNumber}\nFilePath:{Error?.FilePath}\nTryException:{Error?.TryException}";

        }
    }
}
