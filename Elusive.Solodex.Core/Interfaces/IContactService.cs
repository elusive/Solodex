using System.Collections.Generic;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Models;

namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>
    /// Interface for service to manage contacts.
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Gets the contacts from the database.
        /// </summary>
        /// <returns>Enumerable collection of <see cref="Contact"/> entities.</returns>
        IEnumerable<Contact> GetContacts();

        /// <summary>
        /// Deletes the contact specified.
        /// </summary>
        /// <param name="contactToDelete">The contact to delete.</param>
        void DeleteContact(Contact contactToDelete);

        /// <summary>
        /// Modifies the contact.
        /// </summary>
        /// <param name="contactToModify">The contact to modify.</param>
        /// <returns>Result enum value.</returns>
        ModifyContactResultEnum ModifyContact(Contact contactToModify);

        /// <summary>
        /// Createcontacts the specified new contact.
        /// </summary>
        /// <param name="newContact">The new contact.</param>
        /// <returns>Result enum value.</returns>
        CreateContactResult Createcontact(Contact newContact);
    }
}