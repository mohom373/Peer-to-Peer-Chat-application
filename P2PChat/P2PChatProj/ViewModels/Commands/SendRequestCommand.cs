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
        private MenuViewModel viewModel;
        public SendRequestCommand(MenuViewModel viewModel)
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
            return !viewModel.Connecting && !viewModel.Waiting 
                && !String.IsNullOrEmpty(viewModel.InputIp) 
                && (viewModel.InputPort > 1023 && viewModel.InputPort < 65000);
        }

        public async void Execute(object parameter)
        {
           await viewModel.SendRequestAsync();
        }
    }
}
