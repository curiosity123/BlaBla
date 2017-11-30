using BlaBlaClient;
using Common.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlaBlaWpfClient
{
    public class RegisterWindowViewModel : PropertyChange
    {

        public Client client;


        public RegisterWindowViewModel()
        {
        }

        private string login="Enter your nickname";

        public string NickName
        {
            get { return login; }
            set {
                login = value;
                RaisePropertyChangedEvent("NickName");
            }
        }


        private string password="Enter your password";

        public string  Password
        {
            get { return password; }
            set { password = value;
                RaisePropertyChangedEvent("Password");
            }
        }




    }


}
