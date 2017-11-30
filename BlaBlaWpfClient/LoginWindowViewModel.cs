﻿using Common.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlaBlaWpfClient
{
    public class LoginWindowViewModel:PropertyChange
    {
        BlaBlaClient.Client Client;

        public LoginWindowViewModel()
        {
            Client = BlaBlaClient.Client.Create(new XmlSerialization(), "127.0.0.1",8000);   
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
           
        }
        private bool CanTryLoginCmd(object obj)
        {
            return true;
        }

        
    }


}
