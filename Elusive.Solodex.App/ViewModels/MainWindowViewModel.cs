using System.ComponentModel.Composition;
using System.Diagnostics;
using Prism.Mvvm;

namespace Elusive.Solodex.App.ViewModels
{
    /// <summary>
    ///     View Model for the Main Window's presentation logic
    /// </summary>
    [Export]
    public class MainWindowViewModel : BindableBase
    {
        /// <summary>Constructor</summary>
        private MainWindowViewModel()
        {
        }

        /// <summary>
        ///     Gets the Application Version
        /// </summary>
        public string AppVersion
        {
            get { return FileVersionInfo.GetVersionInfo(GetType().Assembly.Location).FileVersion; }
        }
    }
}