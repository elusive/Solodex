using System.Windows;
using System.Windows.Controls;
using Prism.Interactivity;

namespace Elusive.Solodex.UI.Common.Commands
{
    /// <summary>
    ///     Command Behavior used for attaching a command to the Loaded event
    /// </summary>
    public class LoadedCommandBehavior : CommandBehaviorBase<Control>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="control">Target Control</param>
        public LoadedCommandBehavior(Control control)
            : base(control)
        {
            control.Loaded += element_Loaded;
        }

        /// <summary>
        ///     Loaded event handler used to execute the command.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">event args</param>
        private void element_Loaded(object sender, RoutedEventArgs e)
        {
            CommandParameter = e;
            ExecuteCommand(null);
        }
    }
}