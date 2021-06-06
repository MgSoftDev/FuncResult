using System;
using System.Collections.Generic;
using System.Linq;

namespace FuncResult
{
    public class GetResult<T> : GetResult, IDisposable
    {
        public T Result { get; set; }

        public GetResult(GetResult getResult)
        {
            AddError(getResult.Errors);
        }

        
        public GetResult(T result)
        {
            Result = result;
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

        public static GetResult<T> ErrorEntity(string errorMessage, Exception tryException = null) => new GetResult<T>(new ErrorInfo(errorMessage, tryException));
        public static GetResult<T> ErrorEntity(string errorMessage, IEnumerable<GetResult> childResults, Exception tryException = null) => new GetResult<T>(new ErrorInfo(errorMessage, tryException), childResults);


        public bool OkNotNull => (Errors == null || Errors.Count == 0) && Result != null;

        /// <exception cref="GetResultException">Condition.</exception>
        public T ValueOrThrow()
        {
            if (!Ok)
                throw new GetResultException(this);
            return Result;
        }
        public T ValueNotNullOrThrow()
        {
            if (!OkNotNull)
                throw new GetResultException(this);
            return Result;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect(2, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Forced);            
        }
    }

}
