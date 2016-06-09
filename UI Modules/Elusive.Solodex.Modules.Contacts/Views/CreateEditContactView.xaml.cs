using System.ComponentModel.Composition;
using System.Windows.Controls;
using Elusive.Solodex.Modules.Contacts.ViewModels;

namespace Elusive.Solodex.Modules.Contacts.Views
{
    /// <summary>
    ///     Interaction logic for CreateEditContactView.xaml
    /// </summary>
    public partial class CreateEditContactView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEditContactView"/> class.
        /// </summary>
        public CreateEditContactView()
        {
            InitializeComponent();
            DataContext = new CreateEditContactViewModel();
        }
    }
}