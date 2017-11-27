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
    public class RegisterWindowViewModel : INotifyPropertyChanged
    {

        public RegisterWindowViewModel()
        {

        }





        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
