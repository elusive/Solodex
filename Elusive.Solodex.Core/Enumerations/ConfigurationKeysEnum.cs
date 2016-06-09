using System;
using Elusive.Solodex.Core.Common;

namespace Elusive.Solodex.Core.Enumerations
{
    /// <summary>
    ///     Enumeration of available Application Configuration Settings
    /// </summary>
    public enum ConfigurationKeysEnum
    {
        [Default("")] Culture,

        [Default(Constants.DefaultDateFormat)] DateFormat,

        [Default(Constants.DefaultTimeFormat)] TimeFormat,

        [Default("DateTimeOffset.Now", typeof (DateTimeOffset))] ApplicationInstallDate,
    }
}