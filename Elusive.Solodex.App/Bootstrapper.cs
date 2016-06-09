using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using Elusive.Solodex.App.Views;
using Elusive.Solodex.App.Views;
using Elusive.Solodex.Core.Common;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.UI.Common;
using Prism.Mef;
using Prism.Modularity;
using Prism.Regions;

namespace Elusive.Solodex.App
{

    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default 
        ///             Prism Library services in the container. This is the default behavior.</param>
        public override async void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);
         
            // Perform startup actions
            //var startupService = Container.GetExportedValue<IStartupService>();
            //await Task.Run(() => startupService.Startup());

            
            //Container.GetExportedValue<IRegionManager>().RequestNavigate(RegionNames.MainRegion, "AuthenticationView");
        }

        /// <summary>
        /// Configures the <see cref="AggregateCatalog"/> used by MEF.
        /// </summary>
        /// <remarks>
        /// The base implementation does nothing.
        /// </remarks>
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            // Add this assembly to export ModuleTracker
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));

            // Module B and Module D are copied to a directory as part of a post-build step.
            // These modules are not referenced in the project and are discovered by inspecting a directory.
            // Both projects have a post-build step to copy themselves into that directory.
            var catalog = new DirectoryCatalog(@"."); //@".\Modules\");
            AggregateCatalog.Catalogs.Add(catalog);
        }

        /// <summary>
        /// Configures the <see cref="CompositionContainer"/>.
        /// May be overwritten in a derived class to add specific type mappings required by the application.
        /// </summary>
        /// <remarks>
        /// The base implementation registers all the types direct instantiated by the bootstrapper with the container.
        /// The base implementation also sets the ServiceLocator provider singleton.
        /// </remarks>
        protected override void ConfigureContainer()
        {
            Container.ComposeExportedValue(Container);
            base.ConfigureContainer();
        }

        /// <summary>
        /// Creates the MEF composition container
        /// </summary>
        /// <returns>the container</returns>
        protected override CompositionContainer CreateContainer()
        {
            var container = new CompositionContainer(AggregateCatalog, false);
            return container;
        }
        
        /// <summary>
        /// Creates the <see cref="IModuleCatalog"/> used by Prism.
        /// </summary>
        /// <remarks>
        /// The base implementation returns a new ModuleCatalog.
        /// </remarks>
        /// <returns>
        /// A ConfigurationModuleCatalog.
        /// </returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            // When using MEF, the existing Prism ModuleCatalog is still the place to configure modules via configuration files.
            return new ConfigurationModuleCatalog();
        }

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        /// <remarks>
        /// If the returned instance is a <see cref="DependencyObject"/>, the
        /// <see cref="MefBootstrapper"/> will attach the default <seealso cref="Microsoft.Practices.Prism.Regions.IRegionManager"/> of
        /// the application in its <see cref="Microsoft.Practices.Prism.Regions.RegionManager.RegionManagerProperty"/> attached property
        /// in order to be able to add regions by using the <seealso cref="Microsoft.Practices.Prism.Regions.RegionManager.RegionNameProperty"/>
        /// attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<MainWindow>();
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        /// <remarks>
        /// The base implemention ensures the shell is composed in the container.  Fred
        /// </remarks>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (MainWindow)Shell;
            Application.Current.MainWindow.Show();
        }
    }
}