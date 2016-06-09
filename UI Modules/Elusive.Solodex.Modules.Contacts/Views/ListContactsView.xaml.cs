using System.ComponentModel.Composition;
using System.Windows.Controls;
using Elusive.Solodex.Modules.Contacts.ViewModels;

namespace Elusive.Solodex.Modules.Contacts.Views
{
    /// <summary>
    ///     Interaction logic for ListContactsView.xaml
    /// </summary>
    [Export]
    public partial class ListContactsView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListContactsView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        [ImportingConstructor]
        public ListContactsView(ListContactsViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}