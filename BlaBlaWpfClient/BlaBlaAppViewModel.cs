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
    public class BlaBlaAppViewModel : INotifyPropertyChanged
    {
        LoginWindow w;
        public BlaBlaAppViewModel()
        {
            Client client = Client.Create(new XmlSerialization(), "127.0.0.1", 8000);
            client.Run();
            w = new LoginWindow();
            (w.DataContext as LoginWindowViewModel).Client = client;
            w.ShowDialog();


            client.Run();

        }

        private void CloseWindow(bool obj)
        {
            w.Close();
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
