using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace BlaBlaClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000);
            Client client = new Client("127.0.0.1", 8000);
            client.Connect();
        
            User u = new User() { NickName = "lukasz", Password = "123" };
            client.RegisterNewUser(u);

            Console.ReadKey();
            client.Login(u);
            Console.ReadKey();
            client.GetUsers();
                      Console.ReadKey();
            client.Message(new Message() { MyMessage = "test", UserList = new List<User>() { u } });

            Console.ReadKey();
            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }


    class Client
    {
        private string Ip;
        private int Port;

        public Client(string ip, int port)
        {
            this.Ip = ip;
            this.Port = port;
        }

        TcpClient tcpClient;
        NetworkStream networkStream;
        StreamReader clientStreamReader;
        StreamWriter clientStreamWriter;
        private event Action<TcpClient, Command> DataReceived;


        public void Connect()
        {
            tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(Ip, Port);
                NetTools.Receiver(tcpClient, DataReceived);
            }
            catch
            {
                Console.WriteLine("Cant connect");
            }
            networkStream = tcpClient.GetStream();
            clientStreamReader = new StreamReader(networkStream);
            clientStreamWriter = new StreamWriter(networkStream);
            DataReceived = EventProcessor;
        }
        public void Disconnect()
        {
            tcpClient.Close();
        }


        public void RegisterNewUser(User user)
        {
            Command cmd = new Command() { Type = PackageType.Register, Content = user };
            Tools.Send(clientStreamWriter, cmd);
        }

        public void Login(User user)
        {
            Command cmd = new Command() { Type = PackageType.Login, Content = user };
            Tools.Send(clientStreamWriter, cmd);
        }

        public void Logout(User user)
        {
            Command cmd = new Command() { Type = PackageType.Logout, Content = user };
            Tools.Send(clientStreamWriter, cmd);
        }

        public void GetUsers()
        {
            Command cmd = new Command() { Type = PackageType.Users  };
            Tools.Send(clientStreamWriter, cmd);
        }

        public void Message(Message msg)
        {
            Command cmd = new Command() { Type = PackageType.Message, Content = msg };
            Tools.Send(clientStreamWriter, cmd);
        }

        private void EventProcessor(TcpClient client, Command cmd)
        {
            Console.WriteLine(cmd.Type.ToString());
        }



    }

}
