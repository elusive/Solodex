using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Core.Models;
using Elusive.Solodex.Logging.Interfaces;
using Elusive.Solodex.Modules.Contacts.Notifications;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace Elusive.Solodex.Modules.Contacts.ViewModels
{
    /// <summary>
    ///     View model for list view of contacts
    /// </summary>
    [Export]
    public class ListContactsViewModel : BindableBase
    {
        private readonly IContactService _contactService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger = LoggerFactory.GetLogger();
        private ObservableCollection<ContactViewModel> _contacts;
        private int _selectedContactIndex;


        /// <summary>
        ///     Initializes a new instance of the <see cref="ListContactsViewModel" /> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="contactService">The contact service.</param>
        [ImportingConstructor]
        public ListContactsViewModel(IEventAggregator eventAggregator, IContactService contactService)
        {
            _eventAggregator = eventAggregator;
            _contactService = contactService;

            Contacts = new ObservableCollection<ContactViewModel>();
            LoadedCommand = new DelegateCommand<object>(e => Load());
            AddContactCommand = new DelegateCommand(ExecuteAddContactCommand);
            EditContactCommand = new DelegateCommand(ExecuteEditContactCommand, CanExecuteEditContact);

            CreateEditContactInteractionRequest = new InteractionRequest<CreateEditContactNotification>();
        }

        /// <summary>
        ///     Gets the add contact interaction request.
        /// </summary>
        public InteractionRequest<CreateEditContactNotification> CreateEditContactInteractionRequest { get; private set;
        }

        /// <summary>
        ///     Gets the edit contact command
        /// </summary>
        public DelegateCommand EditContactCommand { get; private set; }

        /// <summary>
        ///     Gets the add contact command.
        /// </summary>
        public DelegateCommand AddContactCommand { get; private set; }

        /// <summary>
        ///     Command for when the view is loaded
        /// </summary>
        public DelegateCommand<object> LoadedCommand { get; private set; }

        /// <summary>
        ///     Gets or sets the index of the selected Contact.
        /// </summary>
        public int SelectedContactIndex
        {
            get { return _selectedContactIndex; }
            set
            {
                _selectedContactIndex = value;
                if (value > -1)
                {
                    _logger.LogFormat("Contact selected by user: ", value);
                }
                else
                {
                    _logger.Log("Contacts deselected.");
                }

                RaiseContactChanges();
            }
        }

        /// <summary>
        ///     The list of all <see cref="Contact" /> objects.
        /// </summary>
        public ObservableCollection<ContactViewModel> Contacts
        {
            get { return _contacts; }
            private set
            {
                _contacts = value;
                OnPropertyChanged(() => Contacts);
            }
        }

        /// <summary>
        ///     Enable/Disable the EditContactCommand based on the current contact selection.
        /// </summary>
        /// <returns></returns>
        public bool CanExecuteEditContact()
        {
            return SelectedContactIndex > -1;
        }

        private void ExecuteEditContactCommand()
        {
            if (SelectedContactIndex < 0) return;

            CreateEditContactInteractionRequest.Raise(
                new CreateEditContactNotification(_contactService, Contacts[SelectedContactIndex])
                {
                    Title = "Edit Contact"
                },
                (data) =>
                {
                    if (data.Confirmed)
                    {
                        // refresh event
                        Load();
                    }
                });
        }

        private void ExecuteAddContactCommand()
        {
            CreateEditContactInteractionRequest.Raise(
                new CreateEditContactNotification(_contactService)
                {
                    Title = "Add Contact"
                },
                (data) =>
                {
                    if (data.Confirmed)
                    {
                        // refresh event
                    }
                });
        }

        private void Load()
        {
            Contacts.Clear();
            Contacts.AddRange(
                _contactService.GetContacts()
                    .Select(c => new ContactViewModel(c))
                    .ToList());

            RaiseContactChanges();
        }

        private void RaiseContactChanges()
        {
            OnPropertyChanged(() => SelectedContactIndex);
            EditContactCommand.RaiseCanExecuteChanged();
        }
    }
}