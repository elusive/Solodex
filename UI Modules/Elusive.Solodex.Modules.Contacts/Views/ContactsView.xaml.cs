using System.ComponentModel.Composition;
using Elusive.Solodex.Modules.Contacts.ViewModels;

namespace Elusive.Solodex.Modules.Contacts.Views
{
    /// <summary>
    ///     Interaction logic for SettingsView.xaml
    /// </summary>
    [Export]
    public partial class ContactsView
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ContactsView" /> class.
        /// </summary>
        [ImportingConstructor]
        public ContactsView(ContactsViewModel viewModel)
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Sets the view model.
        /// </summary>
        [Import]
        public ContactsViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}