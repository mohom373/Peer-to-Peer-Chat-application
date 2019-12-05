using P2PChatProj.ViewModels;
using P2PChatProj.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace P2PChatProj
{
    /// <summary>
    /// App startup logic
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            try
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
                MainWindowViewModel mainWindowViewModel = new MainWindowViewModel((MainWindow) MainWindow);
            }
            catch (Exception e)
            {
                Console.WriteLine($"EXCEPTION: {e.ToString()}");
            }
        }
    }
}
