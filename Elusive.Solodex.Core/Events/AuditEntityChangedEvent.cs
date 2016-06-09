using System.Collections.Generic;
using Elusive.Solodex.Core.Common;
using Elusive.Solodex.Core.Interfaces;
using Prism.Events;

namespace Elusive.Solodex.Core.Events
{
    /// <summary>
    ///     Event fired for all pending Entity changes to the data store.
    /// </summary>
    public class AuditEntityChangesEvent : PubSubEvent<AuditData>
    {
    }

    /// <summary>
    ///     Payload for the AuditentityChanges event
    /// </summary>
    public class AuditData
    {
        /// <summary>
        ///     The changes being persisted
        /// </summary>
        public List<EntityChangeNotification> Changes;

        /// <summary>
        ///     The Repository to audit changes in
        /// </summary>
        public IRepository Repository;
    }
}