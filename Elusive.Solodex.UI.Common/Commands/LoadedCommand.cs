using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Elusive.Solodex.Logging.Interfaces;

namespace Elusive.Solodex.UI.Common.Commands
{
    /// <summary>
    ///     Attached property for controls that allows for binding a command to the Control.Loading event
    /// </summary>
    public class LoadedCommand
    {
        private static readonly ILogger _logger = LoggerFactory.GetLogger();

        /// <summary>
        ///     Private attached property for storing the command behavior
        /// </summary>
        private static readonly DependencyProperty LoadedCommandBehaviorProperty =
            DependencyProperty.RegisterAttached(
                "LoadedCommandBehavior", typeof (LoadedCommandBehavior), typeof (LoadedCommand), null);

        /// <summary>
        ///     Attached property for binding a Command for the Window.Loaded event
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", typeof (ICommand), typeof (LoadedCommand), new PropertyMetadata(OnSetCommandCallback));

        /// <summary>
        ///     Set the CommandProperty wrapper
        /// </summary>
        /// <param name="element">Target Control</param>
        /// <param name="command">Command to execute when Window is closing</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Only works for Window")]
        public static void SetCommand(FrameworkElement element, ICommand command)
        {
            element.SetValue(CommandProperty, command);
        }

        /// <summary>
        ///     Get the CommandProperty wrapper
        /// </summary>
        /// <param name="element">Target Window</param>
        /// <returns>Command to execute when Control is loaded</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Only works for Window")]
        public static ICommand GetCommand(FrameworkElement element)
        {
            return element.GetValue(CommandProperty) as ICommand;
        }

        /// <summary>
        ///     Creates the LoadedCommandBehavior for the target Window
        /// </summary>
        /// <param name="dependencyObject">Target Control</param>
        /// <param name="e">event args</param>
        private static void OnSetCommandCallback(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as Control;
            if (control != null)
            {
                GetOrCreateBehavior(control).Command = e.NewValue as ICommand;

                if (control is UserControl & control.IsLoaded)
                {
                    // the re-binding issue of when the datacontext changes
                    _logger.Log(LogLevel.Warning,
                        "View has already been loaded prior to adding a loaded command handler.  Please ensure your data context is set prior to calling InitializeComponent() in your view constructor.");
                }
            }
        }
        
        /// <summary>
        ///     Creates a new LoadedCommandBehavior for the target Window
        /// </summary>
        /// <param name="control">Target Window</param>
        /// <returns>LoadedCommandBehavior instance</returns>
        private static LoadedCommandBehavior GetOrCreateBehavior(Control control)
        {
            var behavior = control.GetValue(LoadedCommandBehaviorProperty) as LoadedCommandBehavior;
            if (behavior == null)
            {
                behavior = new LoadedCommandBehavior(control);
                control.SetValue(LoadedCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
    }
}