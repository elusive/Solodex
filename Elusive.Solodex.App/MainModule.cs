using System.ComponentModel.Composition;
using System.Windows.Controls;
using Elusive.Solodex.UI.Common;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;

namespace Elusive.Solodex.App
{
    /// <summary>
    ///     Prism module for the main executable of the application
    /// </summary>
    [Module(ModuleName = "MainModule")]
    [ModuleExport(typeof (MainModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class MainModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        /// <summary>
        ///     Constructor for the RunModule class
        /// </summary>
        [ImportingConstructor]
        public MainModule(IRegionViewRegistry registry)
        {
            _regionViewRegistry = registry;
        }

        /// <summary>
        ///     Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            // Create a placeholder for a splash screen or whatever we would want to show
            // while the main window is being composed. Otherwise, the first UI Module discovered
            // by Prism that has a view registered to the Main Region will show.
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.MainRegion, () => new UserControl());
        }
    }
}