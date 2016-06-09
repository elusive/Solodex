using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Elusive.Solodex.Core.Models
{
    /// <summary>
    /// Extending <see cref="Contact"/> entity generated classes with attributes.
    /// </summary>
    public partial class ContactAttributes
    {
        /// <summary>
        ///     Attributes for the FirstName property
        /// </summary>
        [StringLengthValidator(1, 30)]
        public string First { get; set; }

        /// <summary>
        ///     Attributes for the LastName property
        /// </summary>
        [StringLengthValidator(1, 30)]
        public string Last { get; set; }
    }
}