using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Common
{
    /// <summary>
    /// Abstract class for base exception handling. Left it to be partial class to 
    /// be extensible to further evolution, based on domain specifics
    /// </summary>
    public abstract partial class BaseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the BaseException class
        /// </summary>
        /// <param name="message">Message to associate to exception</param>
        /// <param name="debugMessage">Debug message</param>
        protected BaseException(string message, string debugMessage)
            : base(message)
        {
            UniqueId = Guid.NewGuid();
            DebugMessage = debugMessage;
        }

        /// <summary>
        /// Initializes a new instance of the BaseException class
        /// </summary>
        /// <param name="message">Message to associate to exception</param>
        /// <param name="debugMessage">Debug Message</param>
        /// <param name="innerException">Inner exception</param>
        protected BaseException(string message, string debugMessage, Exception innerException)
            : base(message, innerException)
        {
            UniqueId = Guid.NewGuid();
            DebugMessage = debugMessage;
        }

        /// <summary>
        /// Gets the Debug Message associated to this Exception
        /// </summary>
        public string DebugMessage { get; private set; }

        /// <summary>
        /// Gets the Unique Identifier for this Exception
        /// </summary>
        public Guid UniqueId { get; private set; }

        /// <summary>
        /// Method to return additional data for logging
        /// </summary>
        /// <returns>returns a formatted debug string to the exception of the additional properties</returns>
        public virtual string AdditionalDataForLogging()
        {
            return "Exception Instance: " + UniqueId + " Msg: " + DebugMessage;
        }
    }
}
