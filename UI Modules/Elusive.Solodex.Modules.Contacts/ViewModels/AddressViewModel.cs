using System;
using System.ComponentModel;
using Elusive.Solodex.Core.Extensions;
using Elusive.Solodex.Core.Models;
using Prism.Mvvm;

namespace Elusive.Solodex.Modules.Contacts.ViewModels
{
    /// <summary>
    ///     View model for address entity.
    /// </summary>
    public class AddressViewModel : BindableBase, IDisposable
    {
        protected readonly Address Model;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddressViewModel" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public AddressViewModel(Address model)
        {
            model.RequireThat().IsNotNull();
            Model = model;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }

        /// <summary>
        ///     Gets the street.
        /// </summary>
        public string Street
        {
            get { return Model.Street; }
            set { Model.Street = value; }
        }

        /// <summary>
        ///     Gets the city.
        /// </summary>
        public string City
        {
            get { return Model.City; }
            set { Model.City = value; }
        }

        /// <summary>
        ///     Gets the state.
        /// </summary>
        public string State
        {
            get { return Model.State; }
            set { Model.State = value; }
        }

        /// <summary>
        ///     Gets the zip.
        /// </summary>
        public string Zip
        {
            get { return Model.Zip; }
            set { Model.Zip = value; }
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