using System;
using System.Diagnostics;
using Elusive.Solodex.Logging.Interfaces;

namespace Elusive.Solodex.Data
{
    /// <summary>
    ///     Provides tracing information about all database activity.
    /// </summary>
    public class ActivityTracer : IDisposable
    {
        /// <summary>
        ///     Interface to logging implementation (static reference)
        /// </summary>
        private static readonly ILogger Logger = LoggerFactory.GetLogger();

        /// <summary>
        ///     Database context
        /// </summary>
        private readonly Entities _context;

        /// <summary>
        ///     Name of executed operation
        /// </summary>
        private readonly string _operation;

        /// <summary>
        ///     Times sql operations
        /// </summary>
        private readonly Stopwatch _stopwatch;

        /// <summary>
        ///     True if tracing is enabled
        /// </summary>
        private readonly bool _tracingSql;

        /// <summary>Initializes a new instance of the <see cref="ActivityTracer" /> class.</summary>
        /// <param name="context">The context.</param>
        /// <param name="tracingSql">The tracing sql.</param>
        /// <param name="operation">The operation.</param>
        public ActivityTracer(Entities context, bool tracingSql, string operation)
        {
            

            _context = context;
            _operation = operation;

            _tracingSql = tracingSql;
            if (_tracingSql)
            {
                _context.Database.Log += Log;
            }

            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        /// <summary>
        ///     Stop the timer and log the activity, then clean up
        /// </summary>
        public void Dispose()
        {
            _stopwatch.Stop();
            if (_tracingSql)
            {
                _context.Database.Log -= Log;
            }
            // Log if tracing is on or if a long running query
            if (_tracingSql || _stopwatch.ElapsedMilliseconds > 100)
            {
                Logger.LogFormat(
                    LogLevel.Debug,
                    "Repository {0} operation completed in {1} ms.",
                    _operation,
                    _stopwatch.ElapsedMilliseconds);
            }
        }

        private void Log(string s)
        {
            Logger.Log(LogLevel.Info, s);
        }
    }
}