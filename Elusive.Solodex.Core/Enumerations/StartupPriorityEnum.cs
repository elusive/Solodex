using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Enumerations
{
    /// <summary>
    /// Defines the ordering of startup actions
    /// </summary>
    /// <remarks>
    /// When adding a new startup action, insert a new enum value at where desired, noting that the actions will be invoked in order (top to bottom).
    /// </remarks>
    public enum StartupPriorityEnum
    {
        DatabaseManagement,

        InteractionHelper,

        ConfigurationServiceInitialization,

        ApplicationService

    }
}
