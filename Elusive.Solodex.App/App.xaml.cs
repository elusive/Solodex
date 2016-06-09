using System;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Elusive.Solodex.Core.Common;

namespace Elusive.Solodex.App
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : ApplicationBase
    {
        /// <summary>
        ///     Overrides the OnStartup method to implement custom
        ///     startup logic
        /// </summary>
        /// <param name="e">Arguments for application startup</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                // Setup global exception handling
                DispatcherUnhandledException += AppDispatcherUnhandledException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

                // Create and run the bootstrapper
                var bootstrapper = new Bootstrapper();
                Bootstrapper = bootstrapper;
                bootstrapper.Run();

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        /// <summary>
        ///     Displays the error message to the user
        /// </summary>
        /// <param name="ex">Exception to show</param>
        private static void ShowErrorMessage(Exception ex)
        {
            var message = new StringBuilder();
            message.AppendLine(ex.Message);

            if (ex.InnerException != null)
            {
                message.AppendLine(string.Format("Inner Exception: {0}", ex.InnerException.Message));
            }

            //var loadException = (ex as ReflectionTypeLoadException);
            //if (loadException != null)
            //{
            //    foreach (var le in loadException.LoaderExceptions)
            //    {
            //        message.AppendLine(le.Message);
            //    }
            //}

            MessageBox.Show(
                message.ToString(),
                "Unhandled Exception",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        /// <summary>
        ///     Event handler for Application.DispatcherUnhandledException event
        /// </summary>
        /// <param name="sender">object that raised event</param>
        /// <param name="e">DispatcherUnhandledExceptionEventArgs</param>
        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ShowErrorMessage(e.Exception);
            e.Handled = true;
        }

        /// <summary>
        ///     Event handler for AppDomain.DispatcherUnhandledException event
        /// </summary>
        /// <param name="sender">object that raised event</param>
        /// <param name="e">Unhandled Exception EventArgs</param>
        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                ShowErrorMessage(e.ExceptionObject as Exception);
            }

            if (e.IsTerminating)
            {
                Current.Shutdown();
            }
        }
    }
}