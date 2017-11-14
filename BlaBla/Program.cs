using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlaBla
{
    class Program
    {
        static Server server;
        static void Main(string[] args)
        {
            Console.WriteLine("server started");
            server = new Server("127.0.0.1", 8000);
            server.Start();
            Console.ReadKey();
        }
    }




    public class Session
    {
        public DateTime LastActivity;
        public TcpClient Client;
        public User User;
    }


    class Server
    {
        Action<TcpClient, Command> PackageReceived;
        List<Command> CommandBus = new List<Command>();
        List<Session> Sessions = new List<Session>();
        List<User> Users = new List<User>();
        private string Ip;
        private int Port;
        private TcpListener Listener;
        private bool isRunning;

        public Server(string v1, int v2)
        {
            this.Ip = v1;
            this.Port = v2;
            PackageReceived = CommandProcessor;
            isRunning = true;
        }


        public void Start()
        {
            Listener = new TcpListener(IPAddress.Parse(Ip), Port);
            Listener.Start();
            new Thread(AcceptingClients).Start();
        }
        public void Stop()
        {
        }


        private void AcceptingClients()
        {
            new Thread(RemovingTerminatedClient).Start();
            while (isRunning)
            {
                try
                {
                    Console.WriteLine("Listening for new cleint...");
                    TcpClient client = Listener.AcceptTcpClient();
                    Sessions.Add(new Session() { Client = client, LastActivity = DateTime.UtcNow });
                    Console.WriteLine("Connected with new client " + client.Client.RemoteEndPoint.ToString());
                    NetTools.Receiver(client, PackageReceived);
                }
                catch
                {
                    Console.WriteLine("Server stopped");
                    isRunning = false;
                    break;
                }
            }
        }
        private void RemovingTerminatedClient()
        {
            return;
            while (isRunning)
            {
                var session = from x in Sessions where x.LastActivity.AddSeconds(100) < DateTime.UtcNow select x;

                foreach (Session cs in session)
                    cs.Client.Close();

                Sessions.RemoveAll(x => x.Client.Connected == false);

                Thread.Sleep(1000);
            }
        }


        private void CommandProcessor(TcpClient Client, Command Cmd)
        {
            if (Cmd.Type == PackageType.Register)
                Register(Cmd);
            if (Cmd.Type == PackageType.Login)
                Login(Client, Cmd);
            if (Cmd.Type == PackageType.Logout)
                Logout(Client, Cmd);
            if (Cmd.Type == PackageType.Users)
                SendUsers(Client, Cmd);
            if (Cmd.Type == PackageType.Message)
                Message(Client, Cmd);

        }


        private void Message(TcpClient client, Command cmd)
        {
            Command messageCmd = new Command() { Type = PackageType.Users, Content = cmd };
            foreach (User u in (cmd.Content as Message).UserList)
            {
                var cli = (from x in Sessions where u.Id == x.User.Id select x).First();
                Tools.Send(new StreamWriter(cli.Client.GetStream()), messageCmd);
            }
        }

        private void SendUsers(TcpClient client, Command cmd)
        {
            Command usersCmd = new Command() { Type = PackageType.Users, Content = Users };
            Tools.Send(new StreamWriter(client.GetStream()), usersCmd);
        }

        private void Logout(TcpClient client, Command cmd)
        {
                var session = (from x in Sessions where x.Client == client select x);
                foreach (Session x in session)
                    x.Client.Close();
        }


    

    private void Login(TcpClient client, Command cmd)
    {
        if ((from x in Users where x.NickName == (cmd.Content as User).NickName && x.Password == (cmd.Content as User).Password select x).Count() > 0)
        {
            var usr = (from x in Users where x.NickName == (cmd.Content as User).NickName && x.Password == (cmd.Content as User).Password select x).First();
            (from x in Sessions where x.Client == client select x).First().User = usr;

                }

    }

    private void Register(Command cmd)
    {
        if ((from x in Users where x.NickName == (cmd.Content as User).NickName select x).Count() == 0)
        {
            User usr = new User()
            {
                NickName = (cmd.Content as User).NickName,
                Password = (cmd.Content as User).Password,
                Id = Guid.NewGuid()
            };
            Users.Add(usr);
        }
    }
}







}
