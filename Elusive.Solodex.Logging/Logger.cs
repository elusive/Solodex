using System;
using Elusive.Solodex.Logging.Interfaces;

namespace Elusive.Solodex.Logging
{
    /// <summary>
    /// Implementation of the ILogger interface.  
    /// </summary>
    /// <remarks>
    /// Simplified excluding error handling, categories, and named loggers.
    /// </remarks>
    public class Logger : ILogger
    {
        /// <summary>
        /// synchronization lock
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// has the logger been configured?
        /// </summary>
        private static bool _configured;
        
        /// <summary>
        /// Initializes a new instance of the Logger type
        /// </summary>
        public Logger()
        {
            Configure();
        }

        /// <summary>
        /// Configure the Logger
        /// </summary>
        public void Configure()
        {
            lock (LockObject)
            {
                if (!_configured)
                {
                    SimpleLog.SetLogFile(logDir: ".//Log", prefix: "SolodexLog_", writeText: true);
                    _configured = true;
                }
            }
        }

        public void Log(object message)
        {
            Log(LogLevel.Info, message);
        }

        /// <summary>
        /// Log a message at specified level
        /// </summary>
        /// <param name="level">level to log</param>
        /// <param name="message">message to log</param>
        public void Log(LogLevel level, object message)
        {
            var messageToLog = String.Format("{0}|{1}", level, message);

            switch (level)
            {
                case LogLevel.Debug:
                    SimpleLog.Info(messageToLog);
                    break;
                case LogLevel.Error:
                    SimpleLog.Error(messageToLog);
                    break;
                case LogLevel.Fatal:
                    SimpleLog.Error(messageToLog);
                    break;
                case LogLevel.Info:
                    SimpleLog.Info(messageToLog);
                    break;
                case LogLevel.Warning:
                    SimpleLog.Warning(messageToLog);
                    break;
                default:
                    SimpleLog.Error(messageToLog);
                    break;
            }
        }

        /// <summary>
        /// Log a message surrounding an exception
        /// </summary>
        /// <param name="debugMessage">Debug message for the exception (should be English)</param>
        /// <param name="exception">exception to log</param>
        public void Log(string debugMessage, Exception exception)
        {
            SimpleLog.Error(debugMessage);
            SimpleLog.Log(exception);
        }

        /// <summary>
        /// Log an exception message at specified level
        /// </summary>
        /// <param name="level">level to log</param>
        /// <param name="debugMessage">message to log</param>
        /// <param name="exception">The exception to log</param>
        public void Log(LogLevel level, string debugMessage, Exception exception)
        {
            var messageToLog = String.Format("{0}|{1}", level, debugMessage);
            SimpleLog.Error(messageToLog);
            SimpleLog.Log(exception);
        }

        /// <summary>
        /// Log a message given the format
        /// </summary>
        /// <param name="format">format to use</param>
        /// <param name="args">arguments for the format</param>
        public void LogFormat(string format, params object[] args)
        {
            var message = String.Format(format, args);
            Log(message);
        }

        /// <summary>
        /// Log a formatted message at specified level
        /// </summary>
        /// <param name="level">level to log</param>
        /// <param name="format">format to log message</param>
        /// <param name="args">arguments for the format</param>
        public void LogFormat(LogLevel level, string format, params object[] args)
        {
            var message = String.Format(format, args);
            Log(level, message);
        }
    }
}
