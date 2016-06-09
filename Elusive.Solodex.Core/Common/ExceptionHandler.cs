using System;
using Elusive.Solodex.Logging.Interfaces;

namespace Elusive.Solodex.Core.Common
{
    /// <summary>
    ///     Exception Handler class
    /// </summary>
    public class ExceptionHandler
    {
        /// <summary>
        ///     Get the logger, using the Exception category
        /// </summary>
        private static readonly ILogger Logger = LoggerFactory.GetLogger();

        /// <summary>
        ///     Prevents a default instance of the ExceptionHandler class from being created.
        /// </summary>
        private ExceptionHandler()
        {
        }

        /// <summary>
        ///     Wrap the given exception into the new Type, trace, and re-throw. No known reason to wrap and only trace, if that
        ///     becomes a need then add the Severity to this parameter list.
        /// </summary>
        /// <typeparam name="T">Concrete exception type</typeparam>
        /// <param name="message">the message for the Exception</param>
        /// <param name="debugMessage">Debug message</param>
        /// <param name="innerException">The current exception</param>
        /// <param name="args">Arguments for the new Type</param>
        /// <returns>The instance of the new exception</returns>
        /// <example>
        ///     try
        ///     {
        ///     throw new Exception();
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///     throw ExceptionHandler.Instance.HandleException
        ///     <ConcreteExceptionType>
        ///         (ex, "User friendly error string", "detailed exception information", argsOptional);
        ///         }
        /// </example>
        public static BaseException HandleException<T>(
            string message,
            string debugMessage,
            Exception innerException,
            params object[] args) where T : BaseException
        {
            // This is newExceptionType dependent
            object[] ctorParams;

            // Fill in the expected parameters for the constructor.
            if ((args == null) || (args.Length == 0))
            {
                ctorParams = new object[3];
                ctorParams[0] = message;
                ctorParams[1] = debugMessage;
                ctorParams[2] = innerException;
            }
            else
            {
                ctorParams = new object[4];
                ctorParams[0] = message;
                ctorParams[1] = debugMessage;
                ctorParams[2] = innerException;
                ctorParams[3] = args;
            }

            // Create the new exception, with the code, the debug message and inner exception
            var newException = (BaseException) Activator.CreateInstance(typeof (T), ctorParams);

            HandleException(newException);
            return newException;
        }

        /// <summary>This method will take a given exception and log it</summary>
        /// <param name="ex">The current exception</param>
        /// <example>
        ///     try
        ///     {
        ///     throw new Exception();
        ///     }
        ///     catch (Exception ex)
        ///     {
        ///     ExceptionHandler.Instance.HandleException(ex);
        ///     }
        /// </example>
        public static void HandleException(BaseException ex)
        {
            if (Logger != null)
            {
                // log all exceptions at Error for initial implementation
                Logger.Log(LogLevel.Error, ex.AdditionalDataForLogging(), ex);
            }
        }
    }
}