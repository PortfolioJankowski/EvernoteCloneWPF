﻿using EvernoteCloneWPF.Model;
using EvernoteCloneWPF.ViewModel.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteCloneWPF.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        public LoginVM VM { get; set; }
        public LoginCommand(LoginVM vm)
        {
            VM = vm;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            User user = parameter as User;
            if (user == null)
                return false;
            if (string.IsNullOrEmpty(user.Username))
                return false;
            if (string.IsNullOrEmpty (user.Password)) 
                return false;
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.Login();
        }
    }
}
