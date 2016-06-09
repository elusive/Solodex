namespace Elusive.Solodex.Core.Events
{
    /// <summary>
    ///     Carrier to communication safe shutdown result
    /// </summary>
    public class CanApplicationShutdownQuery
    {
        /// <summary>
        ///     Gets or sets a value indicating whether shutdown is safe.
        /// </summary>
        /// <remarks>
        ///     This query assumes shutdown is safe.  Only assign (false) to prevent a shutdown.  Never assign true.
        /// </remarks>
        public bool IsShutdownSafe { get; set; }
    }
}