using System.ComponentModel.Composition;
using Elusive.Solodex.Modules.Contacts.Views;
using Elusive.Solodex.UI.Common;
using Elusive.Solodex.UI.Common.Controls;
using Prism.Regions;

namespace Elusive.Solodex.Modules.Contacts.Controls
{
    /// <summary>
    ///     Interaction logic for ContactsViewItemControl.xaml
    /// </summary>
    [Export(typeof (ContactsViewItemControl))]
    [ViewSortHint("100")]
    public partial class ContactsViewItemControl : ViewItemControl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ContactsViewItemControl" /> class.
        /// </summary>
        [ImportingConstructor]
        public ContactsViewItemControl(IRegionManager regionManager)
            : base(regionManager)
        {
            InitializeComponent();
            ViewRegionName = RegionNames.MainRegion;
            ViewType = typeof (ContactsView);
        }
    }
}