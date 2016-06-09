using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elusive.Solodex.Core.Common;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Core.Models;
using Elusive.Solodex.Logging;
using Prism.Events;

namespace Elusive.Solodex.Services
{
    /// <summary>
    /// Provides configuration settings for the application
    /// </summary>
    [Export(typeof(IConfigurationService))]
    [Export(typeof(IStartupAction))]
    public class ConfigurationService : IConfigurationService, IStartupAction
    {
        private readonly Dictionary<ConfigurationKeysEnum, ConfigurationSetting> _configurationSettings =
            new Dictionary<ConfigurationKeysEnum, ConfigurationSetting>();
        
        /// <summary>
        /// The Data Service
        /// </summary>
        private readonly IDataService _dataService;

        /// <summary>
        /// Constructs a new instance of ConfigurationService
        /// </summary>
        [ImportingConstructor]
        public ConfigurationService(
            IDataService dataService,
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;

            foreach (ConfigurationKeysEnum key in Enum.GetValues(typeof(ConfigurationKeysEnum)))
            {
                // loads dictionary of settings with default values
                _configurationSettings.Add(key, new ConfigurationSetting(key));
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Defines the time when the application was first run
        /// </summary>
        public DateTimeOffset ApplicationInstallDate
        {
            get
            {
                return (DateTimeOffset)_configurationSettings[ConfigurationKeysEnum.ApplicationInstallDate].Value;
            }
        }

        /// <summary>
        /// Gets the date and time format.
        /// </summary>
        public string DateAndTimeFormat
        {
            get
            {
                return string.Format("{0} {1}", DateFormat, TimeFormat);
            }
        }

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        public string DateFormat
        {
            get
            {
                return _configurationSettings[ConfigurationKeysEnum.DateFormat].Value as string;
            }
            set
            {
                _configurationSettings[ConfigurationKeysEnum.DateFormat].Value = value;
                ModifySetting(ConfigurationKeysEnum.DateFormat);
            }
        }
        
        /// <summary>
        /// Gets the priority for the startup action.
        /// </summary>
        public StartupPriorityEnum Priority
        {
            get
            {
                return StartupPriorityEnum.ConfigurationServiceInitialization;
            }
        }

        /// <summary>
        /// Current Culture for the Application
        /// </summary>
        public string StartupCulture { get; set; }

        /// <summary>
        /// Gets or sets the time format.
        /// </summary>
        public string TimeFormat
        {
            get
            {
                return _configurationSettings[ConfigurationKeysEnum.TimeFormat].Value as string;
            }
            set
            {
                _configurationSettings[ConfigurationKeysEnum.TimeFormat].Value = value;
                ModifySetting(ConfigurationKeysEnum.TimeFormat);
            }
        }

        /// <summary>
        /// Processes the startup action.
        /// </summary>
        public void ProcessStartupAction()
        {
            LoadApplicationSettings();
        }

        /// <summary>
        /// Loads the application settings.
        /// </summary>
        private void LoadApplicationSettings()
        {
            using (var repo = _dataService.Repository)
            {
                var settings = repo.Get<ApplicationSetting>().ToList();

                settings.ForEach(
                    setting =>
                    {
                        var key = (ConfigurationKeysEnum)Enum.Parse(typeof(ConfigurationKeysEnum), setting.Key);
                        _configurationSettings[key].ConvertFromApplicationSetting(setting.Value);
                    });

                foreach (ConfigurationKeysEnum key in Enum.GetValues(typeof(ConfigurationKeysEnum)))
                {
                    if (settings.Find(setting => setting.Key == key.ToString()) == null)
                    {
                        repo.Add(
                            new ApplicationSetting
                            {
                                Key = key.ToString(),
                                Value =
                                    _configurationSettings[key].Value != null
                                        ? _configurationSettings[key].Value.ToString()
                                        : string.Empty
                            });
                    }
                }

                // Save the settings back to the database
                repo.Save();
            }
        }

        /// <summary>
        /// Save setting to DB 
        /// </summary>
        /// <param name="key">the key </param>
        private void ModifySetting(ConfigurationKeysEnum key)
        {
            using (var repo = _dataService.Repository)
            {
                // The key value must be a string for LINQ to work
                string keyValue = key.ToString();

                var setting = repo.Get<ApplicationSetting>().FirstOrDefault();

                // If the setting isn't found, create a new one
                if (setting == null)
                {
                    setting = new ApplicationSetting { Key = keyValue };
                    repo.Add(setting);
                }

                // Update the value of the setting and save it
                setting.Value = _configurationSettings[key].Value != null
                    ? _configurationSettings[key].ConvertToApplicationSetting()
                    : string.Empty;
                repo.Save();

                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(setting.Key));
                }
            }
        }
    }
}