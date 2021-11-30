using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.FuncResult.Exceptions;
using MgSoftDev.FuncResult.Helper;

// ReSharper disable MethodHasAsyncOverload
// ReSharper disable ExplicitCallerInfoArgument

namespace MgSoftDev.FuncResult
{
    public class Returning : IDisposable
    {
        public enum TypeResult
        {
            Success,
            Error,
            Unfinished
        }

        #region Constructores

        public Returning() { }
        public Returning(UnfinishedInfo unfinishedInfo) { AddUnfinishedItem(unfinishedInfo); }
        public Returning(ErrorInfo      error) { AddError(error); }

        public Returning(ErrorInfo error, IEnumerable<Returning> childResults)
        {
            AddError(error);
            ChildResult = childResults.ToList();
        }

        #endregion

        #region Property

        public List<UnfinishedInfo> UnfinishedItems { get; set; } = new List<UnfinishedInfo>();
        public List<ErrorInfo>      Errors          { get; set; } = new List<ErrorInfo>();
        public List<Returning>      ChildResult     { get; set; } = new List<Returning>();
        public bool                 IsLogStored     { get; set; }
        public Exception            LogException    { get; set; }
        public bool                 Ok              =>( Errors == null || Errors.Count == 0 ) && ( UnfinishedItems == null || UnfinishedItems.Count == 0 );
        public TypeResult           ResultType      =>Ok ? TypeResult.Success : ( ( Errors != null && Errors.Count > 0 ) ? TypeResult.Error : TypeResult.Unfinished );

        #endregion

        #region Methods

        /// <exception cref="ReturningException">Condition.</exception>
        public Returning Throw()
        {
            if( ResultType == TypeResult.Error ) throw new ReturningException(this);
            if( ResultType == TypeResult.Unfinished ) throw new ReturningUnfinishedException(this);

            return this;
        }

        public Returning AddError(ErrorInfo error)
        {
            Errors.Add(error);

            return this;
        }

        public Returning AddError(IEnumerable<ErrorInfo> errorList)
        {
            Errors.AddRange(errorList?.ToList() ?? new List<ErrorInfo>());

            return this;
        }

        public Returning AddUnfinishedItem(UnfinishedInfo unfinishedInfo)
        {
            UnfinishedItems.Add(unfinishedInfo);

            return this;
        }

        public Returning AddUnfinishedItem(IEnumerable<UnfinishedInfo> items)
        {
            UnfinishedItems.AddRange(items?.ToList() ?? new List<UnfinishedInfo>());

            return this;
        }

        #endregion

        #region Static Metodos

        public static Returning Success=>new Returning();


        public static Returning Try(Action methodAction, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [CallerMemberName] string memberName = null,
                                    [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                methodAction.Invoke();

                return new Returning();
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result;
            }
            catch ( ReturningException e )
            {
                return e.Result;
            }
            catch ( Exception e )
            {
                return new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
            }
        }

        public static Task<Returning> TryTask(Func<Task> methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", string logName = "",
                                              [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(async ()=>
            {
                try
                {
                    await methodAction.Invoke();

                    return new Returning();
                }
                catch ( ReturningUnfinishedException e )
                {
                    return e.Result;
                }
                catch ( ReturningException e )
                {
                    return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, logName) : e.Result;
                }
                catch ( Exception e )
                {
                    var error = new Returning(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
                    if( saveLog ) error.SaveLog(ReturningEnums.LogLevel.Error, logName);

                    return error;
                }
            });
        }

        public static Task<Returning> TryTask(Action methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", string logName = "",
                                              [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(()=>
            {
                try
                {
                    methodAction?.Invoke();

                    return new Returning();
                }
                catch ( ReturningUnfinishedException e )
                {
                    return e.Result;
                }
                catch ( ReturningException e )
                {
                    return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, logName) : e.Result;
                }
                catch ( Exception e )
                {
                    var error = new Returning(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
                    if( saveLog ) error.SaveLog(ReturningEnums.LogLevel.Error, logName);

                    return error;
                }
            });
        }


        public static Returning Try(Func<Returning> methodAction, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", [CallerMemberName] string memberName = null,
                                    [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                return methodAction.Invoke();
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result;
            }
            catch ( ReturningException e )
            {
                return e.Result;
            }
            catch ( Exception e )
            {
                return new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
            }
        }


        public static Task<Returning> TryTask(Func<Task<Returning>> methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError",
                                              string logName = "", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(async ()=>
            {
                try
                {
                   return await methodAction.Invoke();
                }
                catch ( ReturningUnfinishedException e )
                {
                    return e.Result;
                }
                catch ( ReturningException e )
                {
                    return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, logName) : e.Result;
                }
                catch ( Exception e )
                {
                    var error = new Returning(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
                    if( saveLog ) error.SaveLog(ReturningEnums.LogLevel.Error, logName);

                    return error;
                }
            });
        }

        public static Task<Returning> TryTask(Func<Returning> methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError",
                                              string          logName = "", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(()=>
            {
                try
                {
                    return methodAction.Invoke();
                }
                catch ( ReturningUnfinishedException e )
                {
                    return e.Result;
                }
                catch ( ReturningException e )
                {
                    return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, logName) : e.Result;
                }
                catch ( Exception e )
                {
                    var error = new Returning(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
                    if( saveLog ) error.SaveLog(ReturningEnums.LogLevel.Error, logName);

                    return error;
                }
            });
        }


        public static Returning Error(string errorMessage, Exception tryException = null, string errorCode = "", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null,
                                      [CallerLineNumber] int lineNumber = 0)
        {
            return new ErrorInfo(errorMessage, tryException, errorCode, memberName, filePath, lineNumber);
        }

        public static Returning Error(string errorMessage, object parameters, Exception tryException = null, string errorCode = "", [CallerMemberName] string memberName = null,
                                      [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return new ErrorInfo(errorMessage, parameters, tryException, errorCode, memberName, filePath, lineNumber);
        }


        public static Returning Unfinished(string title,            string mensaje = null, UnfinishedInfo.NotifyType notifyType = UnfinishedInfo.NotifyType.Information, bool useLocalization = false,
                                           string errorCode = null,  object parameters = null)
        {
            return new UnfinishedInfo(title, mensaje, notifyType, useLocalization, errorCode, parameters);
        }

        #endregion

        #region Operators Overloading

        public static implicit operator Returning(ErrorInfo                    value)=>new Returning(value);
        public static implicit operator Returning(UnfinishedInfo               value)=>new Returning(value);
        public static implicit operator Returning(ReturningException           value)=>value.Result;
        public static implicit operator Returning(ReturningUnfinishedException value)=>value.Result;

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Errors          = null;
            ChildResult     = null;
            UnfinishedItems = null;
            LogException    = null;
        }

        #endregion
    }
}
