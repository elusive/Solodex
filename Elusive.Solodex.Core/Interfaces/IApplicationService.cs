namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>
    ///     ApplicationService interface.
    ///     Publishes the ApplicationStartedEvent, ApplicationShuttingDownEvent, ApplicationCultureChangedEvent, and
    ///     InactivityTimeoutEvent events
    /// </summary>
    /// <remarks>Shows alternative to a separate shutdown and startup service.</remarks>
    public interface IApplicationService
    {
        /// <summary>
        ///     Determines if it is safe to shutdown the application.
        /// </summary>
        bool IsShutdownSafe();
        
        /// <summary>
        ///     Shutdowns this instance.
        /// </summary>
        void Shutdown();
    }
}