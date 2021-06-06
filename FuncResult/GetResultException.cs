namespace FuncResult
{
    public class GetResultException : System.Exception
    {
        public GetResult Result { get; set; }
        public GetResultException(GetResult result) { Result = result; }
    }
}
