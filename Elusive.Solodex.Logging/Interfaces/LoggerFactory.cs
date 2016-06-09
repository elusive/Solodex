
namespace Elusive.Solodex.Logging.Interfaces
{
    /// <summary>
    /// The Logger Factory, creates an instance of ILogger
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Get the Logger for the declaring Type
        /// </summary>
        /// <returns>the ILogger instance</returns>
        public static ILogger GetLogger()
        {
            return new Logger();
        }
    }
}
