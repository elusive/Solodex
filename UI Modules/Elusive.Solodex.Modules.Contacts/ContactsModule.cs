using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elusive.Solodex.Modules.Contacts.Controls;
using Elusive.Solodex.Modules.Contacts.Views;
using Elusive.Solodex.UI.Common;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;

namespace Elusive.Solodex.Modules.Contacts
{
    /// <summary>
    /// Module class for contacts related UI
    /// </summary>
    [Module(ModuleName = "ContactsModule")]
    [ModuleExport(typeof(ContactsModule), InitializationMode = InitializationMode.WhenAvailable)]
    public class ContactsModule : IModule
    {
        /// <summary>
        /// Prism object used for registering content with a region
        /// </summary>
        private readonly IRegionViewRegistry _regionViewRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsModule"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        [ImportingConstructor]
        public ContactsModule(IRegionViewRegistry registry)
        {
            _regionViewRegistry = registry;
        }

        /// <summary>
        /// Initialize this module
        /// </summary>
        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ContactsView));
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.MenuRegion, typeof(ContactsViewItemControl));

            _regionViewRegistry.RegisterViewWithRegion(RegionNames.ContactsRegion, typeof(ListContactsView));
        }
    }
}