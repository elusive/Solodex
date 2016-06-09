using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>
    /// Provides access to an entity's identifier
    /// </summary>
    public interface IEntityIdentifier
    {
        /// <summary>Gets the entity identifier.</summary>
        Guid EntityId { get; }
    }
}
