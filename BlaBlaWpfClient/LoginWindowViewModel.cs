using Common;
using Common.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BlaBlaWpfClient
{
    public class LoginWindowViewModel : PropertyChange
    {
        private BlaBlaClient.Client client;

        public BlaBlaClient.Client Client
        {
            get { return client; }
            set
            {
                client = value;
                Client.CommandManager.LoginReceived = LoginResult;
            }
        }

        public Action Result { get; set; }




        public LoginWindowViewModel()
        {

        }

        private void LoginResult(bool obj)
        {
           // if (obj)
                if (Result != null)
                    Result();
                else
                    MessageBox.Show("błąd spróbuj jeszcze raz");

        }

        public ICommand OpenRegistrationCmd { get { return new RelayCommand(CanOpenRegistrationCmd, OpenRegistration); } }
        private void OpenRegistration(object obj)
        {
            RegisterWindow w = new RegisterWindow();
            w.ShowDialog();
        }
        private bool CanOpenRegistrationCmd(object obj)
        {
            return true;
        }


        public ICommand TryLoginCmd { get { return new RelayCommand(CanTryLoginCmd, TryLogin); } }
        private void TryLogin(object obj)
        {
            User user = new User() { NickName = Login, Password = Password };
            Client.CommandManager.Login(user);
        }
        private bool CanTryLoginCmd(object obj)
        {
            return true;
        }



        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

    }


}
