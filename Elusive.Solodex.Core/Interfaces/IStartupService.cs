using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>
    /// Interface for service to handle startup actions when app initializes.
    /// </summary>
    public interface IStartupService
    {
        /// <summary>
        /// Startup / Initialize the application
        /// </summary>
        void Startup();
    }
}
