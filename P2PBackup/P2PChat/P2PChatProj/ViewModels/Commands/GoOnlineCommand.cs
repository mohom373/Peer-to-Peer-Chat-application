using P2PChatProj.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels.Commands
{
    public class GoOnlineCommand : ICommand
    {
        private OfflineViewModel userConnectViewModel;
        
        /// <summary>
        /// Initializes a new instance of UserConnectCommand class.
        /// </summary>
        public GoOnlineCommand(OfflineViewModel userConnectViewModel)
        {
            this.userConnectViewModel = userConnectViewModel;
        }
        
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return !String.IsNullOrWhiteSpace(userConnectViewModel.User.UserName) && (userConnectViewModel.User.PortNumber > 1023 && userConnectViewModel.User.PortNumber < 65000);
        }

        public void Execute(object parameter)
        {
            userConnectViewModel.GoOnline();
        }
    }
}
