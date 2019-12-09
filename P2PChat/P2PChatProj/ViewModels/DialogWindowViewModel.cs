using P2PChatProj.ViewModels.Commands;
using P2PChatProj.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PChatProj.ViewModels
{
    /// <summary>
    /// Controls a custom dialog window
    /// </summary>
    public class DialogWindowViewModel : BaseViewModel
    {
        // Private window view
        private DialogWindow window;
        private string infoMessage = "";

        #region Properties

        public string InfoMessage
        {
            get
            {
                return infoMessage;
            }
            set
            {
                infoMessage = value;
                RaisePropertyChanged("InfoMessage");
            }
        }

        public DelegateCommand OkCommand { get; set; }

        #endregion

        /// <summary>
        /// DialogWindowViewModel constructor
        /// </summary>
        /// <param name="infoMessage">Message to be displayed</param>
        public DialogWindowViewModel()
        {
            OkCommand = new DelegateCommand(CloseWindow);
        }

        /// <summary>
        /// Shows the dialog window  
        /// </summary>
        public void Show(string infoMessage)
        {
            if (window != null && window.IsActive)
            {
                window.Close();
            }

            window = new DialogWindow();
            window.Owner = Application.Current.MainWindow;
            window.DataContext = this;
            InfoMessage = infoMessage;
            window.Show();
        }

        /// <summary>
        /// Closes the dialog window
        /// </summary>
        private void CloseWindow()
        {
            window.Close();
        }
    }
}
