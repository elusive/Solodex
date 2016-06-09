using System;
using System.Windows.Threading;

namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>
    ///     Service used for invoking methods on the application's Dispatcher
    /// </summary>
    public interface IDispatchService
    {
        /// <summary>
        ///     Invoke an action on a background thread - any thread but the dispatcher.
        /// </summary>
        /// <param name="action">the Method to execute</param>
        void Background(Action action);

        /// <summary>
        ///     Invoke a method on the Dispatcher asynchronously
        /// </summary>
        /// <param name="action">the Method to execute</param>
        void BeginInvoke(Action action);

        /// <summary>
        ///     Invoke a method on the Dispatcher asynchronously
        /// </summary>
        /// <param name="action">the Method to execute</param>
        /// <param name="priority">Invoke Priority</param>
        void BeginInvoke(Action action, DispatcherPriority priority);

        /// <summary>
        ///     Determines if the calling thread is the dispatcher thread.
        /// </summary>
        /// <returns>True if the calling thread is the dispatcher</returns>
        bool CheckAccess();

        /// <summary>
        ///     Invoke a method on the Dispatcher synchronously
        /// </summary>
        /// <param name="action">the Method to execute</param>
        void Invoke(Action action);

        /// <summary>
        ///     Invoke a method on the Dispatcher synchronously
        /// </summary>
        /// <param name="action">the Method to execute</param>
        /// <param name="priority">Invoke Priority</param>
        void Invoke(Action action, DispatcherPriority priority);

        /// <summary>
        ///     Invoke a method on the Dispatcher synchronously
        /// </summary>
        /// <param name="func">the Method to execute</param>
        T Invoke<T>(Func<T> func);

        /// <summary>
        ///     Invoke a method on the Dispatcher synchronously
        /// </summary>
        /// <param name="func">the Method to execute</param>
        /// <param name="priority">Invoke Priority</param>
        T Invoke<T>(Func<T> func, DispatcherPriority priority);
    }
}