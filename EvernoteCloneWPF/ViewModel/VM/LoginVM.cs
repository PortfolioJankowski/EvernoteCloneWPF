using EvernoteCloneWPF.Model;
using EvernoteCloneWPF.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EvernoteCloneWPF.ViewModel.VM
{
    public class LoginVM : INotifyPropertyChanged
    {
		private User user;
        private bool isShowingRegister = false;
        public event PropertyChangedEventHandler? PropertyChanged;

        private string username;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                User = new User
                {
                    Username = username,
                    Password = this.Password,
                    Name = this.Name,
                    Lastname = this.LastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged("Username");
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set 
            { 
                password = value;
                User = new User
                {
                    Username = this.Username,
                    Password = password,
                    Name = this.Name,
                    Lastname = this.LastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged("Password");
            }
        }
        private string confirmPassword;

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set 
            { 
                confirmPassword = value;
                User = new User()
                {
                    Username = this.Username,
                    Password = this.Password,
                    Name = this.Name,
                    Lastname = this.LastName,
                    ConfirmPassword = confirmPassword
                };
                OnPropertyChanged("ConfirmPassword");
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                User = new User()
                {
                    Username = this.Username,
                    Password = this.Password,
                    Name = name,
                    Lastname = this.LastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged("Name");
            }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                User = new User()
                {
                    Username = this.Username,
                    Password = this.Password,
                    Name = this.Name,
                    Lastname = lastName,
                    ConfirmPassword = this.ConfirmPassword
                };
                OnPropertyChanged("LastName");
            }
        }


        public User User
        {
            get { return user; }
            set 
            { 
                user = value;
                OnPropertyChanged("User");
            }
        }


        private Visibility loginVis;

        public Visibility LoginVis
        {
            get { return loginVis; }
            set 
            { 
                loginVis = value;
                OnPropertyChanged("LoginVis");
            }
        }

        private Visibility registerVis;

        public Visibility RegisterVis
        {
            get { return registerVis; }
            set 
            { 
                registerVis = value;
                OnPropertyChanged("RegisterVis");
            }
        }

        public ShowRegisterCommand ShowRegisterCommand { get; set; }
        
       
        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }

        public LoginVM()
        {
            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
            loginVis = Visibility.Visible;
            registerVis = Visibility.Collapsed;
            ShowRegisterCommand = new ShowRegisterCommand(this);
            User = new User();
        }

        public void SwitchViews()
        {
            //kliknięcie przycisku wywołuje Command -> odwrotna wartość isShowingRegister
            //właśnie dlatego przypisuję ten sam Command do przycisku zaloguj i zarejestruj
            isShowingRegister = !isShowingRegister;
            if (isShowingRegister)
            {
                RegisterVis = Visibility.Visible;
                LoginVis = Visibility.Collapsed;
            }
            else
            {
                RegisterVis = Visibility.Collapsed;
                LoginVis = Visibility.Visible;
            }
        }

        public void Login()
        {
            //TODO
        }

        public void Register()
        {
            //TODO
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
