using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Elusive.Solodex.UI.Common.Commands;
using Prism.Interactivity;
using Prism.Interactivity.InteractionRequest;

namespace Elusive.Solodex.UI.Common.Interactivity
{
    public class PopupNotificationAction : PopupWindowAction
    {
        public const string WindowName = "DialogWindow";

        /// <summary>
        /// ctor
        /// </summary>
        public PopupNotificationAction()
        {
            IsModal = true;
        }
        
        /// <summary>
        /// Returns the window to display as part of the trigger action.
        /// </summary>
        /// <param name="notification">The notification to be set as a DataContext in the window.</param>
        /// <returns/>
        protected override Window GetWindow(INotification notification)
        {
            Window wrapperWindow;

            if (WindowContent != null)
            {
                wrapperWindow = new Window
                {
                    Name = WindowName,
                    Style = Application.Current.FindResource("InteractionWindowStyle") as Style,
                    DataContext = notification, // If WindowContent has no DataContext, it will inherit this one.
                    Title = notification.Title,
                    Owner = Application.Current.MainWindow,
                };

                PrepareContentForWindow(notification, wrapperWindow);
            }
            else
            {
                wrapperWindow = CreateDefaultWindow(notification);
            }

            if (IsModal)
            {
                wrapperWindow.Loaded += (o, a) => GlobalCommands.DisableMainContentCommand.Execute(o);
                wrapperWindow.Closed += (o, a) => GlobalCommands.EnableMainContentCommand.Execute(o);
            }

            return wrapperWindow;
        }
    }
}
