using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Prism.Mvvm;

namespace Elusive.Solodex.Core.Models
{
    /// <summary>Base class for all Model classes in the application
    /// that will require notification support.
    /// This class is abstract.</summary>
    /// <typeparam name="TParameter">The TParameter type</typeparam>
    public abstract class ModelBase<TParameter> : BindableBase, IDisposable
        where TParameter : class
    {
        private readonly Validator<TParameter> _validator =
            ValidationFactory.CreateValidatorFromAttributes<TParameter>();

        private bool _isValid;

        private string _validationError;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ModelBase()
        {
        }

        /// <summary>
        /// Event indicates that the value of IsValid has changed
        /// due to a change in validity of the object.
        /// </summary>
        public event EventHandler IsValidChanged;

        /// <summary>
        /// Gets an error message indicating validation errors with this object.
        /// If no validation errors exist then an empty string is returned.
        /// <seealso cref="IDataErrorInfo"/>
        /// </summary>
        public string Error
        {
            get
            {
                return _validationError;
            }
        }

        /// <summary>
        /// Returns true if this object has no validation errors.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            protected set
            {
                if (_isValid == value) return;
                _isValid = value;
                if (IsValidChanged != null)
                {
                    IsValidChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// If no validation errors exist then an empty string is returned.
        /// <seealso cref="IDataErrorInfo"/>
        /// </summary>
        /// <param name="columnName">The name of the property whose error message to get.</param>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        public string this[string columnName]
        {
            get
            {
                ValidationResults validationResults = OnValidate();
                foreach (var validationResult in validationResults)
                {
                    if (validationResult.Key == columnName)
                    {
                        return validationResult.Message;
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Method for manual validation of object.
        /// </summary>
        public virtual void Validate()
        {
            OnValidate();
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected void OnDispose()
        {
        }

        /// <summary>
        /// Called to validate this object.
        /// This can be overridden for custom validation; 
        /// however, using the SelfValidation attribute is preferred.
        /// </summary>
        /// <seealso cref="Validation"/>
        /// <returns>A collection of validation results</returns>
        protected ValidationResults OnValidate()
        {
            var error = new StringBuilder();
            ValidationResults validationResults = _validator.Validate(this);
            foreach (var validationResult in validationResults)
            {
                error.AppendLine(validationResult.Message);
            }
            IsValid = validationResults.IsValid;
            _validationError = error.ToString();
            return validationResults;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
