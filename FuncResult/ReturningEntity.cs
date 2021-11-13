using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FuncResult
{
    public class Returning<T> : Returning, IDisposable
    {

        #region Contructor

        public Returning()
        {
            Val = default;
        }
        public Returning(T value)
        {
            Val = value;
        }
        public Returning(UnfinishedInfo unfinishedInfo)
        {
            AddUnfinishedItem(unfinishedInfo);
        }
        public Returning(ErrorInfo error)
        {
            AddError(error);
        }

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

        public new static     Returning<T>    Success                                            => new Returning<T>();
        public new static Returning<T> FromUnfinishedInfo( UnfinishedInfo unfinishedInfo )=> unfinishedInfo;
        public new static Returning<T> FromErrorInfo( ErrorInfo           errorInfo )     => errorInfo;

       

        public static Returning<T> Try(Func<T> metodoFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                    var val = metodoFunc.Invoke();
                    return new Returning<T>(val);
               
            }
            catch (ReturningUnfinishedException e)
            {
                return new Returning<T>( e.Result);
            }
            catch (ReturningException e)
            {
                return new Returning<T>(e.Result);
            }
            catch (Exception e)
            {
                return new Returning<T>(new ErrorInfo(errorName, e,errorCode, memberName, filePath, lineNumber));
            }
        }

        public static  Task<Returning<T>> TryTask(Func<Task<T>> metodoFunc, string errorName = "Unhandled error", bool saveLog= false, string errorCode = ErrorInfo.UnhandledError, string logName="", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run( async () =>
            {

                try
                {
                    var invoke = await metodoFunc?.Invoke();

                    return  new Returning<T>(  invoke);
                }
                catch( ReturningUnfinishedException e )
                {
                    return new Returning<T>( e.Result );
                }
                catch( ReturningException e )
                {
                    return saveLog ? new Returning<T>( e.Result ).SaveLog( ReturningEnums.LogLevel.Error, logName ) : new Returning<T>( e.Result );
                }
                catch( Exception e )
                {
                    var error = new Returning<T>( new ErrorInfo( errorName, e,errorCode,  memberName, filePath, lineNumber ) );
                    if( saveLog ) error.SaveLog( ReturningEnums.LogLevel.Error, logName );

                    return error;
                }
            } );
        }

        public static Task<Returning<T>> TryTask(Func<T> metodoFunc, string errorName = "Unhandled error", bool saveLog = false, string errorCode = ErrorInfo.UnhandledError, string logName="", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(() =>
            {

                try
                {
                    
                        var val = metodoFunc.Invoke();
                        return new Returning<T>(val);
                    
                }
                catch (ReturningUnfinishedException e)
                {
                    return new Returning<T>(e.Result);
                }
                catch (ReturningException e)
                {
                    return saveLog ? new Returning<T>(e.Result).SaveLog(ReturningEnums.LogLevel.Error, logName) : new Returning<T>(e.Result);
                }
                catch (Exception e)
                {
                    var error= new Returning<T>(new ErrorInfo(errorName, e,errorCode,memberName, filePath, lineNumber));
                    if( saveLog ) error.SaveLog( ReturningEnums.LogLevel.Error, logName );
                    return error;
                }

            });
        }


        public static Returning<T> Try(Func<Returning<T>> metodoFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                return  metodoFunc.Invoke();
                    
                
            }
            catch (ReturningUnfinishedException e)
            {
                return new Returning<T>(e.Result);
            }
            catch (ReturningException e)
            {
                return new Returning<T>(e.Result);
            }
            catch (Exception e)
            {
                return new Returning<T>(new ErrorInfo(errorName, e,errorCode, memberName, filePath, lineNumber));
            }
        }

        public static Task<Returning<T>> TryTask(Func<Task<Returning< T>>> metodoFunc, string errorName = "Unhandled error", bool saveLog = false, string errorCode = ErrorInfo.UnhandledError, string logName = "", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(async () =>
            {

                try
                {
                    return await metodoFunc?.Invoke();
                   
                }
                catch (ReturningUnfinishedException e)
                {
                    return new Returning<T>(e.Result);
                }
                catch (ReturningException e)
                {
                    return saveLog ? new Returning<T>(e.Result).SaveLog(ReturningEnums.LogLevel.Error, logName) : new Returning<T>(e.Result);
                }
                catch (Exception e)
                {
                    var error = new Returning<T>(new ErrorInfo(errorName, e,errorCode, memberName, filePath, lineNumber));
                    if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, logName);

                    return error;
                }
            });
        }

        public static Task<Returning<T>> TryTask(Func<Returning<T>> metodoFunc, string errorName = "Unhandled error", bool saveLog = false, string errorCode = ErrorInfo.UnhandledError, string logName = "", [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            return Task.Run(() =>
            {

                try
                {
                    return metodoFunc.Invoke();
                       
                }
                catch (ReturningUnfinishedException e)
                {
                    return new Returning<T>(e.Result);
                }
                catch (ReturningException e)
                {
                    return saveLog ? new Returning<T>(e.Result).SaveLog(ReturningEnums.LogLevel.Error, logName) : new Returning<T>(e.Result);
                }
                catch (Exception e)
                {
                    var error = new Returning<T>(new ErrorInfo(errorName, e,errorCode, memberName, filePath, lineNumber));
                    if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, logName);
                    return error;
                }

            });
        }



        #endregion

        #region Operators Overloading

        public static implicit operator T(Returning<T> value) => value.Val;

        public static implicit operator Returning<T>(T value) => new Returning<T>(value);
        public static implicit operator Returning<T>(ErrorInfo value) => new Returning<T>(value);
        public static implicit operator Returning<T>(UnfinishedInfo value) => new Returning<T>(value);
        public static implicit operator Returning<T>(ReturningUnfinishedException value) => new Returning<T>(value.Result);
        public static implicit operator Returning<T>(ReturningException value) => new Returning<T>(value.Result);


        #endregion

        private bool _NotCheckThrowIfError = false;

        private T _Val;
        public  T Val { get =>  _Val; set => _Val = value; }
       

        public bool OkNotNull => Ok && Val != null;

        #region Methods

        /// <exception cref="ReturningException">Condition.</exception>
        public new Returning<T> Throw()
        {
            if( ResultType == TypeResult.Error ) throw new ReturningException( this );

            if( ResultType == TypeResult.Unfinished ) throw new ReturningUnfinishedException( this );

            return this;
        }


        public  T ValThrowIfIsNull()
        {
            if (!Throw().OkNotNull)
                throw new Exception("El Objeto no puede ser Null, Property Val (ValNull)");
            return Val;
        }

        public Returning< T > DisableThrow(bool value = true)
        {
            _NotCheckThrowIfError = value;

            return this;
        }
        public void Dispose()
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
