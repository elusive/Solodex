using Prism.Events;

namespace Elusive.Solodex.Core.Events
{
    /// <summary>
    /// Event to query for safe application shutdown
    /// </summary>
    /// <remarks>
    /// Any subscribers of this event MUST subscribe on the publishers thread in order for their vote to be taken into consideration.
    /// Do not block this query.
    /// </remarks>
    public class IsApplicationShutdownSafeEvent : PubSubEvent<CanApplicationShutdownQuery>
    {
    }
}