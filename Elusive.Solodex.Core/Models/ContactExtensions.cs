using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Models
{
    /// <summary>
    /// Extensions for the Contact entity.
    /// </summary>
    public partial class Contact
    {
        /// <summary>
        ///     Gets the full name including prefix and suffix if available.
        /// </summary>
        public string FullName
        {
            get
            {
                var parts = new List<string>();

                if (!string.IsNullOrEmpty(Prefix))
                    parts.Add(Prefix);
                if (!string.IsNullOrEmpty(First))
                    parts.Add(First);
                if (!string.IsNullOrEmpty(Middle))
                    parts.Add(Middle);
                if (!string.IsNullOrEmpty(Last))
                    parts.Add(Last);
                if (!string.IsNullOrEmpty(Suffix))
                    parts.Add(Suffix);

                return String.Join(" ", parts);
            }
        }
    }
}

