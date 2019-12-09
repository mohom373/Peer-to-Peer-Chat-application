using P2PChatProj.Models;
using P2PChatProj.ViewModels.Commands;
using P2PChatProj.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace P2PChatProj.ViewModels
{
    /// <summary>
    /// Viewmodel controling the offline state of the app. The User can enter
    /// a username and a port number to go to the online state of the app.  
    /// </summary>
    public class OfflineViewModel : BaseViewModel
    {
        // Private backing fields
        private ValidationError userNameError = new ValidationError();
        private ValidationError portNumberError = new ValidationError();

        #region Properties

        // Parent viewmodel
        public MainWindowViewModel MainWindowViewModel { get; set; }

        // Commands
        public DelegateCommand GoOnlineCommand { get; private set; }

        // User input
        public string InputUserName { get; set; } = "";

        public string InputPortNumber { get; set; } = "";

        // Validation errors
        public ValidationError UserNameError
        {
            get
            {
                return userNameError;
            }
            private set
            {
                userNameError = value;
                RaisePropertyChanged("UserNameError");
            }
        }

        public ValidationError PortNumberError
        {
            get
            {
                return portNumberError;
            }
            private set
            {
                portNumberError = value;
                RaisePropertyChanged("PortNumberError");
            }
        }
        #endregion

        /// <summary>
        /// OfflineViewModel constructor
        /// </summary>
        /// <param name="mainWindowViewModel">Parent ViewModel</param>
        public OfflineViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            GoOnlineCommand = new DelegateCommand(GoOnline);
        }

        /// <summary>
        /// Event handler for when the app is closing
        /// </summary>
        /// <param name="sender">Object sending the event</param>
        /// <param name="e">Arguments associated with the closing event</param>
        internal void ClosingApp(object sender, CancelEventArgs e)
        {
            Console.WriteLine("STATUS: OfflineViewModel closing");
        }

        /// <summary>
        /// Tries to enter the online state of the app
        /// </summary>
        public void GoOnline()
        {
            // Input validation
            Console.WriteLine("STATUS: Validating user input");
            bool hasInputError = ValidateInput();
            
            if (!hasInputError)
            {
                Console.WriteLine("RESULT: Correct input");

                // Creates a user object and changes app state to online
                User user = new User(InputUserName, Convert.ToInt32(InputPortNumber));
                MainWindowViewModel.ChangeToOnlineView(user);
            }
            else
            {
                Console.WriteLine("RESULT: Incorrect input values");
            }
        }

        /// <summary>
        /// Validates user inputs
        /// </summary>
        /// <returns>Returns a boolean depending on if the inputs were correctly validated</returns>
        private bool ValidateInput()
        {
            bool hasError = false;
            
            // Removes previous error
            UserNameError = new ValidationError();

            // Checking if username is empty
            if (String.IsNullOrWhiteSpace(InputUserName))
            {
                UserNameError = new ValidationError()
                {
                    ErrorMessage = "Please enter a username",
                    HasError = Visibility.Visible
                };
                Console.WriteLine("ERROR: Empty username input");
                hasError = true;
            }
            // Checking length of username (10 W's will distort the UI)
            else if (InputUserName.Length > 9)
            {
                UserNameError = new ValidationError()
                {
                    ErrorMessage = "Username can be at most 10 characters",
                    HasError = Visibility.Visible
                };
                Console.WriteLine("ERROR: Username input too long");
                hasError = true;
            }
            
            // Removes previous error
            PortNumberError = new ValidationError();

            // Checking if port number is empty
            if (String.IsNullOrWhiteSpace(InputPortNumber))
            {
                PortNumberError = new ValidationError()
                {
                    ErrorMessage = "Please enter a port number",
                    HasError = Visibility.Visible
                };
                Console.WriteLine("ERROR: Empty port number input");
                hasError = true;
            }
            else
            {
                // Checking if port number is a valid integer
                int portNumberConvert;

                try
                {
                    portNumberConvert = Convert.ToInt32(InputPortNumber);
                }
                catch (FormatException)
                {
                    PortNumberError = new ValidationError()
                    {
                        ErrorMessage = "Port number must be an integer",
                        HasError = Visibility.Visible
                    };
                    Console.WriteLine("ERROR: Port number input can't convert to integer");
                    hasError = true;
                    return hasError;
                }

                // Checking if port number is between 1024 - 65000
                if (portNumberConvert < 1024 || portNumberConvert > 65000)
                {
                    PortNumberError = new ValidationError()
                    {
                        ErrorMessage = "Only ports 1024 - 65000 can be used",
                        HasError = Visibility.Visible
                    };
                    Console.WriteLine("ERROR: Port number not between 1024 - 65000");
                    hasError = true;
                }
            }
            return hasError;
        }
    }
}
