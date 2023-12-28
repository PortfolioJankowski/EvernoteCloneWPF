using EvernoteCloneWPF.ViewModel.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteCloneWPF.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        //w jakim VM ten command jest?:
        public LoginVM VM { get; set; }

        public event EventHandler? CanExecuteChanged;

        public RegisterCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;    
        }

        public void Execute(object? parameter)
        {
            //TODO: Login functionality
        }
    }
}
