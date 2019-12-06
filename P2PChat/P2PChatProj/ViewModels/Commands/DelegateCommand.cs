using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChatProj.ViewModels.Commands
{
    /// <summary>
    /// An ICommand using delegates. can be used with or without an canExecuteMethod
    /// </summary>
    public class DelegateCommand : ICommand
    {
        // Private variables
        private Action executeMethod;
        private Func<bool> canExecuteMethod;

        /// <summary>
        /// DelegateCommand constructor
        /// </summary>
        /// <param name="executeMethod">A method that executes the command logic</param>
        /// <param name="canExecuteMethod">A method that controls if the command can be executed</param>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod = null)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Decides if the command can be executed
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (canExecuteMethod != null)
            {
                return canExecuteMethod();
            }

            return true; 
        }

        /// <summary>
        /// Executes the action delegate provided by the creator
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            executeMethod();
        }
    }
}
