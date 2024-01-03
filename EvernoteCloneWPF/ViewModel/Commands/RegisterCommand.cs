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
            User user = parameter as User;
            if (user == null)
                return false;
            if (string.IsNullOrEmpty(user.Username))
                return false;
            if (string.IsNullOrEmpty(user.Password))
                return false;
            if (string.IsNullOrEmpty(user.ConfirmPassword))
                return false;
            if(user.Password != user.ConfirmPassword)
                return false;

            return true;    
        }

        public void Execute(object? parameter)
        {
            VM.Register();
        }
    }
}
