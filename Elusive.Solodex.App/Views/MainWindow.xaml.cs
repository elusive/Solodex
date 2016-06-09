using System.ComponentModel.Composition;
using System.Diagnostics;
using Elusive.Solodex.App.ViewModels;

namespace Elusive.Solodex.App.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow
    {
        /// <summary>
        ///     Constructor for MainWindow
        /// </summary>
        [ImportingConstructor]
        public MainWindow(MainWindowViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();

            if (Debugger.IsAttached)
            {
                // main window adjustments for dev can be made here
            }
        }
    }
}