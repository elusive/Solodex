using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Logging.Interfaces
{
    /// <summary>
    /// The ILogger interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Configure the Logger
        /// </summary>
        void Configure();

        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="message">The message.</param>
        void Log(object message);

        /// <summary>
        /// Log a message at specified level
        /// </summary>
        /// <param name="level">level to log</param>
        /// <param name="message">message to log</param>
        void Log(LogLevel level, object message);

        /// <summary>
        /// Log a message surrounding an exception
        /// </summary>
        /// <param name="debugMessage">Debug message for the exception (should be English)</param>
        /// <param name="exception">exception to log</param>
        void Log(string debugMessage, Exception exception);

        /// <summary>
        /// Log a message surrounding an exception
        /// </summary>
        /// <param name="level">level to log</param>
        /// <param name="debugMessage">Debug message for the exception (should be English)</param>
        /// <param name="exception">exception to log</param>
        void Log(LogLevel level, string debugMessage, Exception exception);

        /// <summary>
        /// Log a message given the format
        /// </summary>
        /// <param name="format">format to use</param>
        /// <param name="args">arguments for the format</param>
        void LogFormat(string format, params object[] args);

        /// <summary>
        /// Log a message given the format
        /// </summary>
        /// <param name="level">level to log</param>
        /// <param name="format">format to use</param>
        /// <param name="args">arguments for the format</param>
        void LogFormat(LogLevel level, string format, params object[] args);
    }
}
