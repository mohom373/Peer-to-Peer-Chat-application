using P2PChatProj.ViewModels;
using P2PChatProj.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace P2PChatProj
{
    /// <summary>
    /// App startup logic
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            Console.WriteLine("STATUS: App starting up");
            Dispatcher.UnhandledException += OnUnhandledException;
            try
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
                MainWindowViewModel mainWindowViewModel = new MainWindowViewModel((MainWindow) MainWindow);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Startup Error: {e.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Shutdown();
            }
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //MessageBox.Show($"Unhandled Error: {e.Exception.Message}\n", "Unhandled Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine("EXCEPTION: " + e.Exception.ToString());
            this.Shutdown();
        }
    }
}
