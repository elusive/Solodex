using Elusive.Solodex.Core.Common;
using Prism.Events;

namespace Elusive.Solodex.Core.Events
{
    /// <summary>
    ///     Event fired for all committed Entity changes to the data store.
    /// </summary>
    public class EntityChangedEvent : PubSubEvent<EntityChangeNotification>
    {
    }
}