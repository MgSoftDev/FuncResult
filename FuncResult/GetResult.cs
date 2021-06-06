using System;
using System.Collections.Generic;
using System.Linq;

namespace FuncResult
{
    public class GetResult
    {
        public List<ErrorInfo> Errors { get; private set; } = new List<ErrorInfo>();
        public List<GetResult> ChildResult { get; set; } = new List<GetResult>();
        public dynamic ExternalProperties { get; set; } = new ExternalProperty();
        public bool Ok => Errors == null || Errors.Count == 0;

        #region Constructores
        public GetResult()
        {
        }
        public GetResult(ErrorInfo error)
        {
            AddError(error);
        }

        public GetResult(ErrorInfo error, IEnumerable<GetResult> childResults)
        {
            AddError(error);
            ChildResult = childResults.ToList();
        }
        #endregion

        /// <exception cref="GetResultException">Condition.</exception>
        public GetResult Throw()
        {
            if(!Ok)
              throw new GetResultException(this);
            return this;
        }


        public GetResult AddError(ErrorInfo error)
        {
            Errors.Add(error);
            return this;
        }
        public GetResult AddError(IEnumerable<ErrorInfo> errorList)
        {
            Errors.AddRange(errorList?.ToList()?? new List<ErrorInfo>());
            return this;
        }


        public static GetResult Error(string errorMessage, Exception tryException = null) => new GetResult(new ErrorInfo(errorMessage, tryException));
        public static GetResult Error(string errorMessage, IEnumerable<GetResult> childResults, Exception tryException = null) => new GetResult(new ErrorInfo(errorMessage, tryException), childResults);
    }
}
