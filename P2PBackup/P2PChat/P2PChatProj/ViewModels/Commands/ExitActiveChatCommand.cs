using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels.Commands
{
    public class ExitActiveChatCommand : ICommand
    {
        private MenuViewModel menuViewModel;

        public ExitActiveChatCommand(MenuViewModel menuViewModel)
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
            return true;
        }

        public async void Execute(object parameter)
        {
            await menuViewModel.ExitActiveChat();
        }
    }
}
