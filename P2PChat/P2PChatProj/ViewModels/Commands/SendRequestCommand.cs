using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels.Commands
{
    class SendRequestCommand : ICommand
    {
        private MenuViewModel menuViewModel;
        public SendRequestCommand(MenuViewModel menuViewModel)
        {
            this.menuViewModel = menuViewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return !menuViewModel.Connecting && !menuViewModel.Waiting 
                && !String.IsNullOrEmpty(menuViewModel.InputIp) 
                && (menuViewModel.InputPort > 1023 && menuViewModel.InputPort < 65000);
        }

        public async void Execute(object parameter)
        {
           await menuViewModel.SendRequest();
        }
    }
}
