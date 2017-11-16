using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace BlaBlaClient
{

    class Client
    {
        private string Ip;
        private int Port;

        public Client(string ip, int port)
        {
            this.Ip = ip;
            this.Port = port;
            DataReceived = EventProcessor;
        }

        TcpClient tcpClient;
        NetworkStream networkStream;
        StreamReader clientStreamReader;
        StreamWriter clientStreamWriter;
        private event Action<TcpClient, Command> DataReceived;
        public List<User> ActiveUsers = new List<User>();

        public void Connect()
        {
            tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(Ip, Port);
                NetworkTools.Receiver(tcpClient, DataReceived);
            }
            catch
            {
                Console.WriteLine("Cant connect");
            }
            networkStream = tcpClient.GetStream();
            clientStreamReader = new StreamReader(networkStream);
            clientStreamWriter = new StreamWriter(networkStream);
        }
        public void Disconnect()
        {
            tcpClient.Close();
        }


        public void RegisterNewUser(User user)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Register, Content = user };
            NetworkTools.Send(clientStreamWriter, cmd);
        }

        public void Login(User user)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Login, Content = user };
            NetworkTools.Send(clientStreamWriter, cmd);
        }

        public void Logout(User user)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Logout, Content = user };
            NetworkTools.Send(clientStreamWriter, cmd);
        }

        public void GetUsers()
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Users, Content = null };
            NetworkTools.Send(clientStreamWriter, cmd);
        }

        public void Message(Message msg)
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Message, Content = msg };
            NetworkTools.Send(clientStreamWriter, cmd);
        }

        private void EventProcessor(TcpClient client, Command cmd)
        {

            if (cmd.Type == PackageTypeEnum.Users)
                ActiveUsers = cmd.Content as List<User>;

        }
    }
}
