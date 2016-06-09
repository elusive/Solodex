using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>
    /// Provides the application data repository
    /// </summary>
    public interface IDataService
    {
        /// <summary>Gets a new repository instance.</summary>
        IRepository Repository { get; }
    }
}
