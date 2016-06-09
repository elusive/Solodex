using System.ComponentModel.Composition.Hosting;
using Prism.Mef;

namespace Elusive.Solodex.Core.Common
{
    /// <summary>
    /// Provides public access to the Mef container.
    /// </summary>
    public abstract class BootstrapperBase : MefBootstrapper
    {
        /// <summary>
        /// Gets the MEF Composition Container
        /// </summary>
        public new CompositionContainer Container
        {
            get
            {
                return base.Container;
            }
        }
    }
}
