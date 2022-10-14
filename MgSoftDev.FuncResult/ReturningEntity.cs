using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.FuncResult.Exceptions;
using MgSoftDev.FuncResult.Helper;
using static MgSoftDev.FuncResult.UnfinishedInfo;

// ReSharper disable ExplicitCallerInfoArgument

namespace MgSoftDev.FuncResult
{
    public class Returning< T > : Returning, IDisposable
    {
        #region Contructor

        public Returning() { Val                     = default; }
        public Returning(T              value) { Val = value; }
        public Returning(UnfinishedInfo unfinishedInfo) { AddUnfinishedItem(unfinishedInfo); }
        public Returning(ErrorInfo      error) { AddError(error); }

        public Returning(Returning returning)
        {
            AddError(returning.Errors);
            AddUnfinishedItem(returning.UnfinishedItems);
        }


        public Returning(ErrorInfo error, IEnumerable<Returning> childResults)
        {
            AddError(error);
            ChildResult = childResults.ToList();
        }

        #endregion


        #region Static

        public new static Returning<T> Success=>new Returning<T>();

        public new static Returning<T> Error(string                  errorMessage,    Exception              tryException = null, string errorCode = "", [CallerMemberName] string memberName = null,
                                             [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber   = 0)
        {
            return new ErrorInfo(errorMessage, tryException, errorCode, memberName, filePath, lineNumber);
        }

        public new static Returning<T> Error(string                    errorMessage,      object parameters,      Exception tryException = null, string errorCode = "",
                                             [CallerMemberName] string memberName = null, [CallerFilePath] string       filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return new ErrorInfo(errorMessage, parameters, tryException, errorCode, memberName, filePath, lineNumber);
        }


        public new static Returning<T> Unfinished(string title, string mensaje, NotifyType notifyType = NotifyType.Information, string errorCode = null)
        {
            return new UnfinishedInfo(title, mensaje, notifyType, errorCode);
        }
        public new static Returning<T> Unfinished(string title, NotifyType notifyType = NotifyType.Information, string errorCode = null)
        {
            return new UnfinishedInfo(title, notifyType, errorCode);
        }

        public new static Returning<T> UnfinishedLocalization(string titleKey, string mensajeKey, object[] titleFormatArgs = null, object[] mensajeFormatArgs = null,
                                                              NotifyType notifyType = NotifyType.Information, string errorCode = null)=>
            UnfinishedInfo.FromLocalization(titleKey, mensajeKey, titleFormatArgs, mensajeFormatArgs, notifyType, errorCode);

        public new static Returning<T> UnfinishedLocalization(string titleKey, object[] titleFormatArgs = null, NotifyType notifyType = NotifyType.Information, string errorCode = null)=>
            UnfinishedInfo.FromLocalization(titleKey, titleFormatArgs, notifyType, errorCode);


        public static Returning<T> Try(Func<T> methodFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [CallerMemberName] string memberName = null,
                                       [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                var val = methodFunc.Invoke();

                return new Returning<T>(val);
            }
            catch ( ReturningUnfinishedException e )
            {
                return new Returning<T>(e.Result);
            }
            catch ( ReturningException e )
            {
                return new Returning<T>(e.Result);
            }
            catch ( Exception e )
            {
                return new Returning<T>(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
            }
        }

        public static Task<Returning<T>> TryTask(Func<Task<T>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, string logName = "",
                                                 [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(async ()=>
            {
                try
                {
                    var invoke = await methodFunc.Invoke();

                    return new Returning<T>(invoke);
                }
                catch ( ReturningUnfinishedException e )
                {
                    return new Returning<T>(e.Result);
                }
                catch ( ReturningException e )
                {
                    return saveLog ? new Returning<T>(e.Result).SaveLog(ReturningEnums.LogLevel.Error, null,logName) : new Returning<T>(e.Result);
                }
                catch ( Exception e )
                {
                    var error = new Returning<T>(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
                    if( saveLog ) error.SaveLog(ReturningEnums.LogLevel.Error,null, logName);

                    return error;
                }
            });
        }

        public static Task<Returning<T>> TryTask(Func<T> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, string logName = "",
                                                 [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(()=>
            {
                try
                {
                    var val = methodFunc.Invoke();

                    return new Returning<T>(val);
                }
                catch ( ReturningUnfinishedException e )
                {
                    return new Returning<T>(e.Result);
                }
                catch ( ReturningException e )
                {
                    return saveLog ? new Returning<T>(e.Result).SaveLog(ReturningEnums.LogLevel.Error,null, logName) : new Returning<T>(e.Result);
                }
                catch ( Exception e )
                {
                    var error = new Returning<T>(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
                    if( saveLog ) error.SaveLog(ReturningEnums.LogLevel.Error,null, logName);

                    return error;
                }
            });
        }


        public static Returning<T> Try(Func<Returning<T>> methodFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [CallerMemberName] string memberName = null,
                                       [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                return methodFunc.Invoke();
            }
            catch ( ReturningUnfinishedException e )
            {
                return new Returning<T>(e.Result);
            }
            catch ( ReturningException e )
            {
                return new Returning<T>(e.Result);
            }
            catch ( Exception e )
            {
                return new Returning<T>(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
            }
        }

        public static Task<Returning<T>> TryTask(Func<Task<Returning<T>>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                                 string logName = "", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(async ()=>
            {
                try
                {
                    return await methodFunc.Invoke();
                }
                catch ( ReturningUnfinishedException e )
                {
                    return new Returning<T>(e.Result);
                }
                catch ( ReturningException e )
                {
                    return saveLog ? new Returning<T>(e.Result).SaveLog(ReturningEnums.LogLevel.Error,null, logName) : new Returning<T>(e.Result);
                }
                catch ( Exception e )
                {
                    var error = new Returning<T>(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
                    if( saveLog ) error.SaveLog(ReturningEnums.LogLevel.Error,null, logName);

                    return error;
                }
            });
        }

        public static Task<Returning<T>> TryTask(Func<Returning<T>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                                 string logName = "", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(()=>
            {
                try
                {
                    return methodFunc.Invoke();
                }
                catch ( ReturningUnfinishedException e )
                {
                    return new Returning<T>(e.Result);
                }
                catch ( ReturningException e )
                {
                    return saveLog ? new Returning<T>(e.Result).SaveLog(ReturningEnums.LogLevel.Error,null, logName) : new Returning<T>(e.Result);
                }
                catch ( Exception e )
                {
                    var error = new Returning<T>(new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber));
                    if( saveLog ) error.SaveLog(ReturningEnums.LogLevel.Error,null, logName);

                    return error;
                }
            });
        }

        #endregion

        #region Operators Overloading

        public static implicit operator T(Returning<T> value)=>value.Val;

        public static implicit operator Returning<T>(T                            value)=>new Returning<T>(value);
        public static implicit operator Returning<T>(ErrorInfo                    value)=>new Returning<T>(value);
        public static implicit operator Returning<T>(UnfinishedInfo               value)=>new Returning<T>(value);
        public static implicit operator Returning<T>(ReturningUnfinishedException value)=>new Returning<T>(value.Result);
        public static implicit operator Returning<T>(ReturningException           value)=>new Returning<T>(value.Result);

        #endregion


        public T Val { get; set; }


        public bool OkNotNull=>Ok && Val != null;

        #region Methods

        /// <exception cref="ReturningException">Condition.</exception>
        public new Returning<T> Throw()
        {
            if( ResultType == TypeResult.Error ) throw new ReturningException(this);

            if( ResultType == TypeResult.Unfinished ) throw new ReturningUnfinishedException(this);

            return this;
        }


        public T ValThrowIfIsNull()
        {
            if( !Throw().OkNotNull ) throw new Exception("El Objeto no puede ser Null, Property Val (ValNull)");

            return Val;
        }


        public new void Dispose()
        {
            Val = default;

            GC.SuppressFinalize(this);
            GC.Collect(2, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Forced);
        }

        #endregion
    }
}
