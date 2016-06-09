using System.Collections.ObjectModel;
using Elusive.Solodex.Core.Extensions;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Modules.Contacts.ViewModels;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;

namespace Elusive.Solodex.Modules.Contacts.Notifications
{
    /// <summary>
    /// Notification derived class for passing context to EditFilterView dialog.
    /// </summary>
    public class CreateEditContactNotification : Confirmation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEditContactNotification"/> class.
        /// </summary>
        public CreateEditContactNotification()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEditContactNotification"/> class.
        /// </summary>
        public CreateEditContactNotification(
            IContactService contactService,
            ContactViewModel contact = null)
            : this()
        {
            contactService.RequireThat("contactService").IsNotNull();

            ContactServiceInstance = contactService;
            ContactToEdit = contact;
        }
        
        /// <summary>
        /// Contact to edit if applicable.
        /// </summary>
        public ContactViewModel ContactToEdit { get; set; }

        /// <summary>
        /// Contact service instance
        /// </summary>
        public IContactService ContactServiceInstance { get; set; }
    }
}