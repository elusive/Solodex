using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Elusive.Solodex.Core.Extensions;
using Elusive.Solodex.Core.Models;
using Prism.Mvvm;

namespace Elusive.Solodex.Modules.Contacts.ViewModels
{
    /// <summary>
    ///     View model for contact entity.
    /// </summary>
    public class ContactViewModel : BindableBase, IDisposable
    {
        protected readonly Contact Model;
        private readonly ObservableCollection<AddressViewModel> _addresses;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContactViewModel" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ContactViewModel(Contact model)
        {
            model.RequireThat("model").IsNotNull();

            Model = model;
            Model.PropertyChanged += OnModelPropertyChanged;

            _addresses = new ObservableCollection<AddressViewModel>();
            if (Model.Addresses.Any())
            {
                _addresses.AddRange(Model.Addresses.Select(a => new AddressViewModel(a)));
            }
        }

        /// <summary>
        /// Gets the contact.
        /// </summary>
        public Contact Contact
        {
            get { return Model; }
        }

        /// <summary>
        ///     Gets the full name.
        /// </summary>
        public string FullName
        {
            get { return Model.FullName; }
        }

        /// <summary>
        /// Gets or sets the first.
        /// </summary>
        public string First
        {
            get { return Model.First; }
            set { Model.First = value; }
        }

        /// <summary>
        /// Gets or sets the middle.
        /// </summary>
        public string Middle
        {
            get { return Model.Middle; }
            set { Model.Middle = value; }
        }

        /// <summary>
        /// Gets or sets the last.
        /// </summary>
        public string Last
        {
            get { return Model.Last; }
            set { Model.Last = value; }
        }

        /// <summary>
        ///     Gets the mobile phone.
        /// </summary>
        public string MobilePhone
        {
            get { return Model.MobilePhone; }
            set { Model.MobilePhone = value; }
        }

        /// <summary>
        ///     Gets the work phone.
        /// </summary>
        public string WorkPhone
        {
            get { return Model.WorkPhone; }
            set { Model.WorkPhone = value; }
        }

        /// <summary>
        ///     Gets the email.
        /// </summary>
        public string Email
        {
            get { return Model.Email; }
            set { Model.Email = value; }
        }

        /// <summary>
        ///     Gets the notes.
        /// </summary>
        public string Notes
        {
            get { return Model.Notes; }
            set { Model.Notes = value; }
        }

        /// <summary>
        ///     Gets the addresses.
        /// </summary>
        public ObservableCollection<AddressViewModel> Addresses
        {
            get { return _addresses; }
        }

        /// <summary>
        ///     Invoked when this object is being removed from the application
        ///     and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }

        /// <summary>
        ///     Default implementation just bubbles the model property changed to the view on
        ///     behalf of the view model.  This is only appropriate when the view model is simply
        ///     just passing thru model properties.
        /// </summary>
        protected virtual void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        protected void OnDispose()
        {
            Model.PropertyChanged -= OnModelPropertyChanged;
        }
    }
}