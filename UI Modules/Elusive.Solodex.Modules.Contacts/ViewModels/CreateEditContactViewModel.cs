using System;
using System.ComponentModel.Composition;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Core.Models;
using Elusive.Solodex.Modules.Contacts.Notifications;
using Elusive.Solodex.UI.Common.Interactivity;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace Elusive.Solodex.Modules.Contacts.ViewModels
{
    /// <summary>
    /// View model class for editing contact or creating new contact.
    /// </summary>
    [Export]
    public class CreateEditContactViewModel : InteractionViewModelBase<CreateEditContactNotification>
    {
        private bool _isEditing;
        private Contact _contact;
        private IContactService _contactService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEditContactViewModel"/> class.
        /// </summary>
        public CreateEditContactViewModel()
        {
            SaveCommand = new DelegateCommand(ExecuteSaveCommand);
            CancelCommand = new DelegateCommand(
                () =>
                {
                    NotificationContext.Confirmed = false;
                    FinishInteraction();
                });
        }

        /// <summary>
        ///     Gets the cancel command.
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }

        /// <summary>
        ///     Gets the save command.
        /// </summary>
        public DelegateCommand SaveCommand { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        ///     Gets or sets the contact to edit.
        /// </summary>
        public ContactViewModel ContactToEdit { get; set; }

        private void ExecuteSaveCommand()
        {
            var result = _contactService.ModifyContact(ContactToEdit.Contact);
            if (result != ModifyContactResultEnum.Success)
            {
                Message = result.ToString();
                return;
            }

            NotificationContext.Confirmed = true;
            FinishInteraction();
        }

        /// <summary>
        ///     This method is called when the  <see cref="IInteractionRequestAware.Notification" />
        ///     property is populated with a not null value. The notification object contains the
        ///     context data and possible dependencies for use by the dialog/interaction viewmodel.
        /// </summary>
        /// <remarks>
        ///     Override this method to execute code when Notification property
        ///     is populated and you have dependencies to retrieve.
        /// </remarks>
        protected override void UpdateFromNotificationContext()
        {
            _contactService = NotificationContext.ContactServiceInstance;
            _isEditing = NotificationContext.ContactToEdit != null;
            ContactToEdit = NotificationContext.ContactToEdit ?? new ContactViewModel(new Contact());
            OnPropertyChanged(() => ContactToEdit);
        }
    }
}