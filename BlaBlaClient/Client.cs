using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
        private User CurrentUser = new User();


        bool IsAlive = true;

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
            IsAlive = false;
            Thread.Sleep(2000);
            tcpClient.Close();
        }

        private void KeepAlive()
        {
            if (IsAlive == false)
                new Thread(() =>
                {
                    while (IsAlive)
                    {
                        Alive();
                        Thread.Sleep(1000);
                    }
                }).Start();
        }

        private void Alive()
        {
            Command cmd = new Command() { Type = PackageTypeEnum.Alive, Content = CurrentUser };
            NetworkTools.Send(clientStreamWriter, cmd);
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
            Command cmd = new Command() { Type = PackageTypeEnum.Users, Content = CurrentUser };
            NetworkTools.Send(clientStreamWriter, cmd);
        }

        public void Message(string text, List<User> users)
        {
            Message msg = new Common.Message() { Sender = CurrentUser, UserList = users, Text = text };
            Command cmd = new Command() { Type = PackageTypeEnum.Message, Content = msg };
            NetworkTools.Send(clientStreamWriter, cmd);
        }


        private void EventProcessor(TcpClient client, Command cmd)
        {
            if (cmd.Type == PackageTypeEnum.Users)
                ActiveUsers = cmd.Content as List<User>;

            if (cmd.Type == PackageTypeEnum.Login && cmd.Content is User)
            {
                CurrentUser = cmd.Content as User;
                IsAlive = true;
                KeepAlive();
            }

            if (cmd.Type == PackageTypeEnum.Logout)
            {
                IsAlive = false;
                CurrentUser = new User();
            }

            if (cmd.Type == PackageTypeEnum.Message)
            {
                Message msg = cmd.Content as Message;
                Console.WriteLine(msg.Sender.NickName + "# Wrote: " + msg.Text);
            }

            if (cmd.Type == PackageTypeEnum.Register)
            {
                User u = cmd.Content as User;
                if (u != null)
                    Console.WriteLine("User " + u.NickName + " was registered succesfully! :)");
                else
                    Console.WriteLine("User " + u.NickName + " was not registered, Try different nickname... :(");
            }
        }

    }
}
