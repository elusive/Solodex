using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Events;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Logging.Interfaces;
using Prism.Events;

namespace Elusive.Solodex.Services
{
    /// <summary>
    /// Application services.
    /// <see cref="IApplicationService"/>
    /// </summary>
    [Export(typeof(IApplicationService))]
    [Export(typeof(IStartupAction))]
    public class ApplicationService : IApplicationService, IStartupAction
    {
        private readonly IConfigurationService _configurationService;

        private readonly IEventAggregator _eventAggregator;
        
        private readonly ILogger _logger = LoggerFactory.GetLogger();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationService" /> class.
        /// </summary>
        [ImportingConstructor]
        public ApplicationService(
            IEventAggregator eventAggregator,
            IConfigurationService configurationService)
        {
            _eventAggregator = eventAggregator;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Gets the priority for the startup action.
        /// </summary>
        StartupPriorityEnum IStartupAction.Priority
        {
            get
            {
                return StartupPriorityEnum.ApplicationService;
            }
        }

        /// <summary>
        /// Determines if it is safe to shutdown the application.
        /// </summary>
        public bool IsShutdownSafe()
        {
            try
            {
                var payload = new CanApplicationShutdownQuery {IsShutdownSafe = true};
                _eventAggregator.GetEvent<IsApplicationShutdownSafeEvent>().Publish(payload);
                return payload.IsShutdownSafe;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Exception querying for safe shutdown", ex);
                return false;
            }
        }

        /// <summary>
        /// Processes the startup action.
        /// </summary>
        public void ProcessStartupAction()
        {            
        }
        
        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Shutdown()
        {
            Application.Current.Shutdown();
        }
    }
}