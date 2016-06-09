using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Elusive.Solodex.UI.Common.Interactivity
{
    public abstract class InteractionViewModelBase<TNotification> : BindableBase, IInteractionRequestAware
        where TNotification : class, INotification
    {
        protected TNotification NotificationContext;

        protected InteractionViewModelBase()
        {
            // need to know when the IInteractionRequestAware.Notification property is set
            // because it may have dependencies we need to be aware of and that we need to
            // pull from that object.
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Notification" && Notification != null)
            {
                UpdateFromNotificationContext();
            }
        }

        /// <summary>
        /// This method is called when the  <see cref="IInteractionRequestAware.Notification"/> 
        /// property is populated with a not null value. The notification object contains the 
        /// context data and possible dependencies for use by the dialog/interaction viewmodel.
        /// </summary>
        /// <remarks>The Notification property serves as the context payload for an interaction
        /// viewmodel.  Override this method to execute code when Notification property
        /// is populated and you have dependencies to retrieve.</remarks>
        protected virtual void UpdateFromNotificationContext()
        {
        }

        #region IInteractionRequestAware Implementation

        /// <summary>
        /// An <see cref="T:System.Action"/> that can be invoked to finish the interaction.
        /// </summary>
        public Action FinishInteraction { get; set; }

        /// <summary>
        /// The <see cref="T:Microsoft.Practices.Prism.Interactivity.InteractionRequest.INotification" /> 
        /// passed when the interaction request was raised.  Serves as the context payload
        /// for the interaction view.  
        /// </summary>
        public INotification Notification
        {
            get
            {
                return NotificationContext;
            }

            set
            {
                if (value is TNotification)
                {
                    NotificationContext = value as TNotification;
                    OnPropertyChanged(() => Notification);
                }
            }
        }
        #endregion
    }
}
