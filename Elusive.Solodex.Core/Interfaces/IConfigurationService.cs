using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>
    /// Interface defines the application service responsible for
    /// storing and reading application configuration settings
    /// </summary>
    public interface IConfigurationService : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the date and time format.
        /// </summary>
        string DateAndTimeFormat { get; }

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the time format.
        /// </summary>
        string TimeFormat { get; set; }

    }

}
