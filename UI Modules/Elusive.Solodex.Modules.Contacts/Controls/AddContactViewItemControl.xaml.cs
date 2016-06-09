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
    [Export(typeof (AddContactViewItemControl))]
    [ViewSortHint("200")]
    public partial class AddContactViewItemControl : ViewItemControl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ContactsViewItemControl" /> class.
        /// </summary>
        [ImportingConstructor]
        public AddContactViewItemControl(IRegionManager regionManager)
            : base(regionManager)
        {
            InitializeComponent();
            ViewRegionName = RegionNames.ContactsRegion;
            ViewType = typeof (CreateEditContactView);
        }
    }
}