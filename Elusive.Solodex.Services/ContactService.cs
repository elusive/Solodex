using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Extensions;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Core.Models;
using Elusive.Solodex.Logging.Interfaces;

namespace Elusive.Solodex.Services
{
    /// <summary>
    /// Service for managing contact entities.
    /// </summary>
    [Export(typeof(IContactService))]
    public class ContactService : IContactService
    {
        private readonly ILogger _logger = LoggerFactory.GetLogger();
        private readonly IDataService _dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactService"/> class.
        /// </summary>
        /// <param name="dataService">The data service.</param>
        [ImportingConstructor]
        public ContactService(IDataService dataService)
        {
            _dataService = dataService;
        }


        /// <summary>
        /// Gets the contacts from database.
        /// </summary>
        /// <returns>Collection of contacts</returns>
        public IEnumerable<Contact> GetContacts()
        {
            using (var repo = _dataService.Repository)
            {
                return repo.Get<Contact>("Addresses");
            }
        }

        /// <summary>
        /// Deletes the contact specified.
        /// </summary>
        /// <param name="contactToDelete">The contact to delete.</param>
        public void DeleteContact(Contact contactToDelete)
        {
            using (var repo = _dataService.Repository)
            {
                var contact = repo.Get<Contact>().FirstOrDefault(c => c.Id == contactToDelete.Id);
                if (contact == null)
                {
                    _logger.LogFormat("Attempt to deleted contact that does not exist: ", contactToDelete.Id);
                    return;
                }

                foreach (var address in contact.Addresses)
                {
                    repo.Delete(address);
                    _logger.LogFormat("Deleting address: ", address.Id);
                }
                
                repo.Delete(contact);

                _logger.LogFormat("Deleting contact: ", contact.Id);
            }
        }


        /// <summary>
        /// Modifies the contact.
        /// </summary>
        /// <param name="contactToModify">The contact to modify.</param>
        /// <returns>
        /// Result enum value.
        /// </returns>
        public ModifyContactResultEnum ModifyContact(Contact contactToModify)
        {
            try
            {
                contactToModify.RequireThat("contactToModify").IsNotNull();
                contactToModify.Validate();
                if (!contactToModify.IsValid)
                {
                    return ModifyContactResultEnum.InvalidContact;
                }

                using (var repo = _dataService.Repository)
                {
                    var contact = repo.Get<Contact>().FirstOrDefault(c => c.Id == contactToModify.Id);
                    if (contact == null)
                    {
                        _logger.Log("Attempt to modify contact that does not exist.");
                        return ModifyContactResultEnum.InvalidContact;
                    }

                    contact.First = contactToModify.First;
                    contact.Middle = contactToModify.Middle;
                    contact.Last = contactToModify.Last;
                    contact.MobilePhone = contactToModify.MobilePhone;
                    contact.WorkPhone = contactToModify.WorkPhone;
                    contact.Email = contactToModify.Email;
                    contact.Notes = contactToModify.Notes;

                    repo.Save();

                    return ModifyContactResultEnum.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogFormat("Error saving contact information: {0}", ex.Message);
                return ModifyContactResultEnum.UnknownError;
            }
        }

        public CreateContactResult Createcontact(Contact newContact)
        {
            throw new System.NotImplementedException();
        }
    }
}