
using Prism.Commands;

namespace Elusive.Solodex.UI.Common.Commands
{
    /// <summary>
    /// Class used for defining shared global commands
    /// </summary>
    public class GlobalCommands
    {
        /// <summary>
        /// This is a globally available command for disabling the main window's content by 
        /// graying it out and setting enabled properties to false on the views.
        /// </summary>
        public static readonly CompositeCommand DisableMainContentCommand = new CompositeCommand();

        /// <summary>
        /// This global command is for re-enabling main content by returning its content to normal
        /// opacity levels and enabled properties set to true.
        /// </summary>
        public static readonly CompositeCommand EnableMainContentCommand = new CompositeCommand();
    }
}
