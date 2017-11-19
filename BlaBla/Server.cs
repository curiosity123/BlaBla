using Common;
using Common.Communication;
using Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BlaBlaServer
{
    public class Server
    {
        Action<TcpClient, Command> PackageReceived;
        List<Session> Sessions = new List<Session>();
        List<User> Users = new List<User>();
        private string Ip;
        private int Port;
        private TcpListener Listener;
        private bool isRunning;
        ISerialization serialization = new XmlSerialization();

        public Server(string ip, int port)
        {
            this.Ip = ip;
            this.Port = port;
            //new TcpClientCommunication(new XmlSerialization(),);
            PackageReceived = CommandProcessor;
            isRunning = true;
        }


        public void Start()
        {
            Listener = new TcpListener(IPAddress.Parse(Ip), Port);
            Listener.Start();
            new Thread(SessionWorker).Start();
        }
        public void Stop()
        {
            isRunning = false;
        }


        private void SessionWorker()
        {
            new Thread(SessionRemoverWorker).Start();
            while (isRunning)
            {
                try
                {
                    Console.WriteLine("Listening for new cleint...");
                    TcpClient client = Listener.AcceptTcpClient();
                    Sessions.Add(new Session() { Client = client, LastActivity = DateTime.UtcNow });
                    Console.WriteLine("Connected with new client " + client.Client.RemoteEndPoint.ToString());
                    CommunicationTools.Receive(serialization, client, PackageReceived);
                }
                catch
                {
                    Console.WriteLine("Server stopped");
                    isRunning = false;
                    break;
                }
            }
        }
        private void SessionRemoverWorker()
        {
            while (isRunning)
            {
                var session = from x in Sessions where x.LastActivity.AddSeconds(300) < DateTime.UtcNow select x;

                foreach (Session cs in session)
                    cs.Client.Close();

                Sessions.RemoveAll(x => x.Client.Connected == false);

                Thread.Sleep(1000);
            }
        }


        private void CommandProcessor(TcpClient Client, Command Cmd)
        {
            if (Cmd.Type == PackageTypeEnum.Alive)
                Alive(Client, Cmd);
            if (Cmd.Type == PackageTypeEnum.Register)
                Register(Client, Cmd);
            if (Cmd.Type == PackageTypeEnum.Login)
                Login(Client, Cmd);
            if (Cmd.Type == PackageTypeEnum.Logout)
                Logout(Client, Cmd);
            if (Cmd.Type == PackageTypeEnum.Users)
                SendUsers(Client, Cmd);
            if (Cmd.Type == PackageTypeEnum.Message)
                Message(Client, Cmd);
        }


        private void Message(TcpClient client, Command cmd)
        {
            Command messageCmd = new Command() { Type = PackageTypeEnum.Message, Content = cmd.Content };
            foreach (User u in (cmd.Content as Message).UserList)
            {
                var cli = (from x in Sessions where u.NickName == x.User.NickName select x).FirstOrDefault();
                if (cli != null)
                    CommunicationTools.Send(serialization,new StreamWriter(cli.Client.GetStream()), messageCmd);
            }
        }

        private void SendUsers(TcpClient client, Command cmd)
        {
            Command usersCmd = new Command() { Type = PackageTypeEnum.Users, Content = Users };
            CommunicationTools.Send(serialization, new StreamWriter(client.GetStream()), usersCmd);
        }

        private void Logout(TcpClient client, Command cmd)
        {
            var session = (from x in Sessions where x.User.Id == (cmd.Content as User).Id select x);
            CommunicationTools.Send(serialization, new StreamWriter(client.GetStream()), new Command() { Type = PackageTypeEnum.Logout, Content = null });
        }

        private void Login(TcpClient client, Command cmd)
        {
            if ((from x in Users where x.NickName == (cmd.Content as User).NickName && x.Password == (cmd.Content as User).Password select x).Count() > 0)
            {
                var usr = (from x in Users where x.NickName == (cmd.Content as User).NickName && x.Password == (cmd.Content as User).Password select x).First();
                if (usr != null)
                {
                    (from x in Sessions where x.Client == client select x).First().User = usr;
                    CommunicationTools.Send(serialization, new StreamWriter(client.GetStream()), new Command() { Type = PackageTypeEnum.Login, Content = usr });
                }
            }
        }

        private void Register(TcpClient client, Command cmd)
        {
            if (cmd.Content is User && (from x in Users where x.NickName == (cmd.Content as User).NickName select x).Count() == 0)
            {
                User usr = new User()
                {
                    NickName = (cmd.Content as User).NickName,
                    Password = (cmd.Content as User).Password,
                    Id = Guid.NewGuid()
                };
                Users.Add(usr);
                CommunicationTools.Send(serialization, new StreamWriter(client.GetStream()), new Command() { Type = PackageTypeEnum.Register, Content = usr });
            }
            else
                CommunicationTools.Send(serialization, new StreamWriter(client.GetStream()), new Command() { Type = PackageTypeEnum.Register, Content = null });
        }

        private void Alive(TcpClient client, Command cmd)
        {
            var session = (from x in Sessions
                           where x.User.Id == (cmd.Content as User).Id
                           select x).FirstOrDefault();

            if (session != null)
                session.LastActivity = DateTime.UtcNow;
        }
    }
}
