using P2PChatProj.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels.Commands
{
    public class UserConnectCommand : ICommand
    {
        private UserConnectViewModel viewModel;

        public UserConnectCommand(UserConnectViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.CanConnect;
        }

        public void Execute(object parameter)
        {
            viewModel.SaveChanges();
        }
    }
}
