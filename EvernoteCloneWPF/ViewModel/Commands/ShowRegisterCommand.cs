using EvernoteCloneWPF.ViewModel.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteCloneWPF.ViewModel.Commands
{
    public class ShowRegisterCommand : ICommand
    {
        public LoginVM LoginVM { get; set; }

        public ShowRegisterCommand(LoginVM vm)
        {
            LoginVM = vm;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            LoginVM.SwitchViews();
        }
    }
}
