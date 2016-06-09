namespace Elusive.Solodex.Core.Enumerations
{
    /// <summary>
    ///     Alert severity
    /// </summary>
    public enum AlertSeverityEnum : short
    {
        /// <summary>
        ///     The intended definition of Information alerts would be those alerts
        ///     that are in direct correlation with a current user interaction with
        ///     the instrument or application and thus a user not present would not
        ///     need to be notified via an alert notification. The user at the
        ///     instrument can remedy the situation.
        /// </summary>
        Informational,

        /// <summary>
        ///     The intended definition of Warning alerts would be those alerts that
        ///     get sent via Alert notifications. In other words, those alerts that
        ///     a user that is not within close proximity to the instrument may need
        ///     to be notified to become aware of and put measures in place to remedy.
        /// </summary>
        Warning,
    }
}