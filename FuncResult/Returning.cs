using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FuncResult
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

        public Returning( ) { }
        public Returning( UnfinishedInfo unfinishedInfo ) { AddUnfinishedItem( unfinishedInfo ); }
        public Returning( ErrorInfo      error ) { AddError( error ); }

        public Returning( ErrorInfo error, IEnumerable<Returning> childResults )
        {
            AddError( error );
            ChildResult              = childResults.ToList();
        }

        #endregion

        #region Property

        public List<UnfinishedInfo> UnfinishedItems { get; set; } = new List<UnfinishedInfo>();
        public List<ErrorInfo> Errors { get; set; } = new List<ErrorInfo>();
        public List<Returning> ChildResult { get; set; } = new List<Returning>();
        public bool       IsLogStored { get; set; }
        public Exception  LogException { get; set; }
        public bool       Ok=>( Errors == null || Errors.Count == 0 ) && ( UnfinishedItems == null || UnfinishedItems.Count == 0 );
        public TypeResult ResultType=>Ok ? TypeResult.Success : ( ( Errors != null && Errors.Count > 0 ) ? TypeResult.Error : TypeResult.Unfinished );

        #endregion

        #region Methods

        /// <exception cref="ReturningException">Condition.</exception>
        public Returning Throw( )
        {
            if( ResultType == TypeResult.Error ) throw new ReturningException( this );
            if( ResultType == TypeResult.Unfinished ) throw new ReturningUnfinishedException( this );

            return this;
        }

        public Returning AddError( ErrorInfo error )
        {
            Errors.Add( error );

            return this;
        }

        public Returning AddError( IEnumerable<ErrorInfo> errorList )
        {
            Errors.AddRange( errorList?.ToList() ?? new List<ErrorInfo>() );

            return this;
        }

        public Returning AddUnfinishedItem( UnfinishedInfo unfinishedInfo )
        {
            UnfinishedItems.Add( unfinishedInfo );

            return this;
        }

        public Returning AddUnfinishedItem( IEnumerable<UnfinishedInfo> items )
        {
            UnfinishedItems.AddRange( items?.ToList() ?? new List<UnfinishedInfo>() );

            return this;
        }

        #endregion

        #region Static Metodos

        public static Returning Try( Action                  metodoAction,    string errorName = "", [CallerMemberName] string memberName = null,
                                     [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0 )
        {
            try
            {
                metodoAction?.Invoke();

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
                return new ErrorInfo( errorName, e, memberName, filePath, lineNumber );
            }
        }

        public static Task<Returning> TryTask( Func<Task> metodoAction, string errorName = "", bool saveLog = false, string logName = "",
                                               [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null,
                                               [CallerLineNumber] int lineNumber = 0 )
        {
            return Task.Run( async ( )=>
            {
                try
                {
                    await metodoAction?.Invoke();

                    return new Returning();
                }
                catch ( ReturningUnfinishedException e )
                {
                    return e.Result;
                }
                catch ( ReturningException e )
                {
                    return saveLog ? e.Result.SaveLog( ReturningEnums.LogLevel.Error, logName ) : e.Result;
                }
                catch ( Exception e )
                {
                    var error = new Returning( new ErrorInfo( errorName, e, memberName, filePath, lineNumber ) );
                    if( saveLog ) error.SaveLog( ReturningEnums.LogLevel.Error, logName );

                    return error;
                }
            } );
        }

        public static Task<Returning> TryTask( Action metodoAction, string errorName = "", bool saveLog = false, string logName = "",
                                               [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null,
                                               [CallerLineNumber] int lineNumber = 0 )
        {
            return Task.Run( ( )=>
            {
                try
                {
                    metodoAction?.Invoke();

                    return new Returning();
                }
                catch ( ReturningUnfinishedException e )
                {
                    return e.Result;
                }
                catch ( ReturningException e )
                {
                    return saveLog ? e.Result.SaveLog( ReturningEnums.LogLevel.Error, logName ) : e.Result;
                }
                catch ( Exception e )
                {
                    var error = new Returning( new ErrorInfo( errorName, e, memberName, filePath, lineNumber ) );
                    if( saveLog ) error.SaveLog( ReturningEnums.LogLevel.Error, logName );

                    return error;
                }
            } );
        }

        public static Returning FromUnfinishedInfo( UnfinishedInfo unfinishedInfo )=>unfinishedInfo;

        public static Returning FromErrorInfo( ErrorInfo errorInfo )=>errorInfo;

        #endregion

        #region Operators Overloading

        public static implicit operator Returning( ErrorInfo                    value )=>new Returning( value );
        public static implicit operator Returning( UnfinishedInfo               value )=>new Returning( value );
        public static implicit operator Returning( ReturningException           value )=>value.Result;
        public static implicit operator Returning( ReturningUnfinishedException value )=>value.Result;

        #endregion

        #region IDisposable

        public void Dispose( )
        {
            Errors          = null;
            ChildResult     = null;
            UnfinishedItems = null;
            LogException    = null;
        }

        #endregion
    }
}
