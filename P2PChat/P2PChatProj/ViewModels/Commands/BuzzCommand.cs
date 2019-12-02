using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels.Commands
{
    public class BuzzCommand : ICommand
    {
        private ChatViewModel chatViewModel;

        public BuzzCommand(ChatViewModel chatViewModel)
        {
            this.chatViewModel = chatViewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return chatViewModel.ChatRunning;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("BUZZING...");
        }
    }
}
