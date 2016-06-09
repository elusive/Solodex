using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elusive.Solodex.Core.Enumerations;

namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>
    /// Represents a class supporting startup action processing
    /// </summary>
    public interface IStartupAction
    {
        /// <summary>
        /// Gets the priority for the startup action.
        /// </summary>
        StartupPriorityEnum Priority { get; }

        /// <summary>
        /// Processes the startup action.
        /// </summary>
        void ProcessStartupAction();
    }
}
