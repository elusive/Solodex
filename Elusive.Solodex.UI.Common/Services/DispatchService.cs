using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Logging.Interfaces;

namespace Elusive.Solodex.UI.Common.Services
{
    /// <summary>
    /// Service used for invoking methods on the application's Dispatcher
    /// </summary>
    [Export(typeof(IDispatchService))]
    public class DispatchService : IDispatchService
    {
        private readonly ILogger _logger = LoggerFactory.GetLogger();

        private readonly TimeSpan _uiEternity = new TimeSpan(0, 0, 10);

        private Dispatcher _dispatcher;

        /// <summary>
        /// Current application's Dispatcher
        /// </summary>
        private Dispatcher Dispatcher
        {
            get
            {
                if (_dispatcher == null && Application.Current != null)
                {
                    _dispatcher = Application.Current.Dispatcher;
                }

                return _dispatcher;
            }
        }

        /// <summary>
        /// Invoke an action on a background thread - any thread but the dispatcher.
        /// </summary>
        /// <param name="action">the Method to execute</param>
        public void Background(Action action)
        {
            var wrappedAction = new Action(
                () =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        _logger.Log(LogLevel.Error, e);
                    }
                });
            if (CheckAccess())
            {
                Task.Run(wrappedAction);
            }
            else
            {
                wrappedAction();
            }
        }

        /// <summary>
        /// Invoke a method on the Dispatcher asynchronously
        /// </summary>
        /// <param name="action">the Method to execute</param>
        public void BeginInvoke(Action action)
        {
            BeginInvoke(action, DispatcherPriority.Normal);
        }

        /// <summary>
        /// Invoke a method on the Dispatcher asynchronously
        /// </summary>
        /// <param name="action">the Method to execute</param>
        /// <param name="priority">Invoke Priority</param>
        public void BeginInvoke(Action action, DispatcherPriority priority)
        {
            if (Dispatcher == null || Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Dispatcher.BeginInvoke(action, priority);
            }
        }

        /// <summary>
        /// Determines if the calling thread is the dispatcher thread.
        /// </summary>
        /// <returns>True if the calling thread is the dispatcher</returns>
        public bool CheckAccess()
        {
            return (Dispatcher == null || Dispatcher.CheckAccess());
        }

        /// <summary>
        /// Invoke a method on the Dispatcher synchronously
        /// </summary>
        /// <param name="action">the Method to execute</param>
        public void Invoke(Action action)
        {
            Invoke(action, DispatcherPriority.Normal);
        }

        /// <summary>
        /// Invoke a method on the Dispatcher synchronously
        /// </summary>
        /// <param name="action">the Method to execute</param>
        /// <param name="priority">Invoke Priority</param>
        public void Invoke(Action action, DispatcherPriority priority)
        {
            if (Dispatcher == null || Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                var operation = Dispatcher.BeginInvoke(action, priority);
                MonitorDispatcherOperation(operation);
            }
        }

        /// <summary>
        /// Invoke a method on the Dispatcher synchronously
        /// </summary>
        /// <param name="func">the Method to execute</param>
        public T Invoke<T>(Func<T> func)
        {
            return Invoke(func, DispatcherPriority.Normal);
        }

        /// <summary>
        /// Invoke a method on the Dispatcher synchronously
        /// </summary>
        /// <param name="func">the Method to execute</param>
        /// <param name="priority">Invoke Priority</param>
        public T Invoke<T>(Func<T> func, DispatcherPriority priority)
        {
            if (Dispatcher == null || Dispatcher.CheckAccess())
            {
                return func();
            }
            var operation = Dispatcher.BeginInvoke(func, priority);
            MonitorDispatcherOperation(operation);
            return (T)operation.Result;
        }

        private void MonitorDispatcherOperation(DispatcherOperation operation)
        {
            while (operation.Wait(_uiEternity) != DispatcherOperationStatus.Completed)
            {
                // Threads get delayed when debugging, ignore this if it happens in the debugger.
                if (!Debugger.IsAttached)
                {
                    var stack = new StackTrace();
                    _logger.Log(
                        LogLevel.Warning,
                        string.Format(
                            "Dispatcher deadlock suspected.  Operation status: {0} at: {1}",
                            operation.Status,
                            stack));
                }
            }
        }
    }
}