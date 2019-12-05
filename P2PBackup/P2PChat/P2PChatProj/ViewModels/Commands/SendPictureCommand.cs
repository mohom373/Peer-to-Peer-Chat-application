using P2PChatProj.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels.Commands
{
    public class SendPictureCommand : ICommand
    {
        private ChatViewModel chatViewModel;

        public SendPictureCommand(ChatViewModel chatViewModel)
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
            return chatViewModel.ChatState == ChatState.Online;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("SENDING PICTURE...");
        }
    }
}
