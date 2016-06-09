using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Elusive.Solodex.Core.Events;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Logging.Interfaces;
using Prism.Events;

namespace Elusive.Solodex.Services
{
    [Export(typeof(IStartupService))]
    public class StartupService : IStartupService
    {
        private readonly IDispatchService _dispatchService;

        private readonly IEventAggregator _eventAggregator;

        private readonly IApplicationService _applicationService;

        private readonly ILogger _logger = LoggerFactory.GetLogger();

        private readonly IEnumerable<IStartupAction> _startupActions;

        [ImportingConstructor]
        public StartupService(
            IEventAggregator eventAggregator,
            IDispatchService dispatchService,
            IApplicationService applicationService,
            [ImportMany] IEnumerable<IStartupAction> startupActions)
        {
            _eventAggregator = eventAggregator;
            _dispatchService = dispatchService;
            _applicationService = applicationService;
            _startupActions = startupActions;
        }

        /// <summary>
        /// Startup / Initialize the application
        /// </summary>
        public void Startup()
        {

            try
            {
                _startupActions.OrderBy(x => x.Priority).ToList().ForEach(
                    x =>
                    {

                        try
                        {
                            x.ProcessStartupAction();
                        }
                        catch (Exception e)
                        {
                            _logger.Log(
                                LogLevel.Fatal,
                                string.Format("Exception thrown from startup action {0}", x.Priority.ToString()),
                                e);
                            throw;
                        }
                    });

                _eventAggregator.GetEvent<ApplicationStartedEvent>().Publish(null);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e);
                if (Application.Current != null)
                {
                    // Must be done on the dispatcher thread
                    var shutdownAction = new Action(() => Application.Current.Shutdown(-1));
                    if (_dispatchService.CheckAccess())
                    {
                        shutdownAction.Invoke();
                    }
                    else
                    {
                        _dispatchService.Invoke(shutdownAction);
                    }
                }
                _applicationService.Shutdown();
            }
        }
    }
}