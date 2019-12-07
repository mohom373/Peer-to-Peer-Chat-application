using P2PChatProj.Models;
using P2PChatProj.ViewModels.Commands;
using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PChatProj.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        // Private backing fields
        private MenuState menuState = MenuState.Listening;
        private string chatStatusInfo = "No active chat";
        private Visibility exitButtonVisibility = Visibility.Collapsed;
        private Visibility acceptDeclineButtonVisibility = Visibility.Collapsed;
        private ValidationError ipAddressError = new ValidationError();
        private ValidationError portNumberError = new ValidationError();

        #region Properties

        public User User { get; private set; }

        // Parent viewmodel
        public OnlineViewModel OnlineViewModel { get; private set; }

        // Commands
        public DelegateCommand SendRequestCommand { get; private set; }

        public DelegateCommand ExitButtonCommand { get; private set; }

        public DelegateCommand AcceptButtonCommand { get; private set; }

        public DelegateCommand DeclineButtonCommand { get; private set; }

        // Menu and chat state information
        public MenuState MenuState
        {
            get
            {
                return menuState;
            }
            set
            {
                menuState = value;
                UpdateMenuState();
            }
        }

        public string ChatStatusInfo 
        {
            get 
            {
                return chatStatusInfo;
            }
            set 
            {
                chatStatusInfo = value;
                RaisePropertyChanged("ChatStatusInfo");
            } 
        }

        public Visibility ExitButtonVisibility 
        {
            get
            {
                return exitButtonVisibility;
            } 
            set
            {
                exitButtonVisibility = value;
                RaisePropertyChanged("ExitButtonVisibility");
            }
        }

        public Visibility AcceptDeclineButtonVisibility
        {
            get
            {
                return acceptDeclineButtonVisibility;
            }
            set
            {
                acceptDeclineButtonVisibility = value;
                RaisePropertyChanged("AcceptDeclineButtonVisibility");
            }
        }

        // User input
        public string InputIpAddress { get; set; } = "";

        public string InputPortNumber { get; set; } = "";

        public string InputSearchHistory { get; set; } = "";

        // Validation errors
        public ValidationError IpAddressError
        {
            get
            {
                return ipAddressError;
            }
            set
            {
                ipAddressError = value;
                RaisePropertyChanged("IpAddressError");
            }
        }

        public ValidationError PortNumberError
        {
            get
            {
                return portNumberError;
            }
            set
            {
                portNumberError = value;
                RaisePropertyChanged("PortNumberError");
            }
        }

        // History list
        public ObservableCollection<ChatData> ChatHistoryList { get; set; }

        #endregion

        /// <summary>
        /// MenuViewModel constructor
        /// </summary>
        /// <param name="onlineViewModel">Parent viewmodel</param>
        /// <param name="user">User information</param>
        public MenuViewModel(OnlineViewModel onlineViewModel, User user)
        {
            User = user;
            OnlineViewModel = onlineViewModel;

            ChatHistoryList = new ObservableCollection<ChatData>();

            // Create new command objects!!!!!
        }

        private void UpdateMenuState()
        {
            Console.WriteLine("STATUS: Updating menu state");
        }

        internal void ClosingApp(object sender, CancelEventArgs e)
        {
            Console.WriteLine("STATUS: MenuViewModel closing");
        }


    }
}
