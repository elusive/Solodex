using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elusive.Solodex.Core.Common;

namespace Elusive.Solodex.Data
{
    /// <summary>Defines a data access layer exception</summary>
    public class DataAccessException : BaseException
    {
        /// <summary>Initializes a new instance of the <see cref="DataAccessException"/> class.</summary>
        /// <param name="message">The message.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DataAccessException(string message, string debugMessage, Exception innerException)
            : base(message, debugMessage, innerException)
        {
        }
    }
}
