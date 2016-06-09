using System.ComponentModel.Composition;
using System.Windows;
using Elusive.Solodex.Core.Extensions;

namespace Elusive.Solodex.Core.Common
{
    /// <summary>
    ///     Extends WPF Application with Bootstrapper and SatisfyImports property.
    /// </summary>
    public partial class ApplicationBase : Application
    {
        /// <summary>
        ///     Lazy loaded bootstrapper.
        /// </summary>
        private static BootstrapperBase _bootstrapper = null;

        /// <summary>
        ///     Gets/Sets the Bootstraper from the Application Properties.
        /// </summary>
        public BootstrapperBase Bootstrapper
        {
            get { return (BootstrapperBase) Current.Properties[typeof (BootstrapperBase).Name]; }
            set { Current.Properties[typeof (BootstrapperBase).Name] = value; }
        }

        /// <summary>
        ///     Satisfies the imports for the specified part.
        ///     Used the objects initialized by the XAML initializer that does not automatically resolve Mef attributes.
        /// </summary>
        /// <param name="attributedPart">target part</param>
        public virtual void SatisfyImports(object attributedPart)
        {
            // check parameters
            attributedPart.RequireThat("attributedPart").IsNotNull();

            if (_bootstrapper == null)
            {
                _bootstrapper = Bootstrapper;
            }

            if (_bootstrapper.Container == null)
            {
                return;
            }

            _bootstrapper.Container.SatisfyImportsOnce(attributedPart);
        }
    }
}