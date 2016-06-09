using System.ComponentModel.Composition;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Modules.Contacts.Notifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Elusive.Solodex.Modules.Contacts.ViewModels
{
    /// <summary>
    /// View model for contacts main view.
    /// </summary>
    [Export]
    public class ContactsViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsViewModel"/> class.
        /// </summary>
        /// <param name="contactService">The contact service.</param>
        [ImportingConstructor]
        public ContactsViewModel(IContactService contactService)
        {
        }
    }
}