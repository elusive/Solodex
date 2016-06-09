using System;
using System.ComponentModel.Composition;
using System.Windows;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.UI.Common.Commands;
using Prism.Commands;
using Prism.Regions;

namespace Elusive.Solodex.UI.Common.Interactivity
{
    /// <summary>
    ///     Helper class for interactivity functionality.
    /// </summary>
    [Export]
    [Export(typeof (IStartupAction))]
    public class InteractionHelper : IStartupAction
    {
        private readonly IRegionManager _regionManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InteractionHelper" /> class.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        [ImportingConstructor]
        public InteractionHelper(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        /// <summary>
        ///     Gets the priority for the startup action.
        /// </summary>
        public StartupPriorityEnum Priority
        {
            get { return StartupPriorityEnum.InteractionHelper; }
        }

        /// <summary>
        ///     This startup method will register the helper methods herein with
        ///     the global composite commands for disabling and reenabling the
        ///     main window's content, for use when interaction requests are triggered.
        /// </summary>
        public void ProcessStartupAction()
        {
            GlobalCommands.DisableMainContentCommand.RegisterCommand(new DelegateCommand(DisableMainContent));
            GlobalCommands.EnableMainContentCommand.RegisterCommand(new DelegateCommand(EnableMainContent));
        }

        /// <summary>
        ///     Disables the content of the main window by setting all views
        ///     to disabled and to 50% opacity (grays out all regions).
        /// </summary>
        public void DisableMainContent()
        {
            UpdateViews(
                (element, changeOpacity) =>
                {
                    element.IsEnabled = false;

                    if (changeOpacity)
                    {
                        element.Opacity = 0.5;
                    }
                });
        }

        /// <summary>
        ///     Returns any disabled views to enabled state and to normal
        ///     level opacity.
        /// </summary>
        public void EnableMainContent()
        {
            UpdateViews(
                (element, changeOpacity) =>
                {
                    element.ClearValue(UIElement.IsEnabledProperty);
                    element.ClearValue(UIElement.OpacityProperty);
                });
        }

        private void UpdateViews(Action<UIElement, bool> action)
        {
            foreach (var region in _regionManager.Regions)
            {
                foreach (var view in region.Views)
                {
                    var uiElement = (view as UIElement);
                    if (uiElement != null)
                    {
                        action(uiElement, true);
                    }
                }
            }
        }
    }
}