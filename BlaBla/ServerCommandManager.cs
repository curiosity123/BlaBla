using Common;
using Common.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace BlaBlaServer
{
    public class ServerCommandManager
    {
        ServerSettings Settings;
        TcpListenerCommunication Communication;

        public ServerCommandManager(ServerSettings settings, TcpListenerCommunication communication )
        {
            Settings = settings;
            Communication = communication;
            Communication.PackageReceived += CommandProcessor;
        }

        private ServerCommandManager() { }
            


        internal void CommandProcessor(TcpClient Client, Command Cmd)
        {
            if (Cmd == null)
                return;

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
                var cli = (from x in Settings.Sessions where u.NickName == x.User.NickName select x).FirstOrDefault();
                if (cli != null)
                    Communication.Send(cli.Client, messageCmd);
            }
        }

        private void SendUsers(TcpClient client, Command cmd)
        {
            Command usersCmd = new Command() { Type = PackageTypeEnum.Users, Content = Settings.Users };
            Communication.Send(client, usersCmd);
        }

        private void Logout(TcpClient client, Command cmd)
        {
            var session = (from x in Settings.Sessions where x.User.Id == (cmd.Content as User).Id select x);
            Communication.Send(client, new Command() { Type = PackageTypeEnum.Logout, Content = null });
        }

        private void Login(TcpClient client, Command cmd)
        {
            if ((from x in Settings.Users where x.NickName == (cmd.Content as User).NickName && x.Password == (cmd.Content as User).Password select x).Count() > 0)
            {
                var usr = (from x in Settings.Users where x.NickName == (cmd.Content as User).NickName && x.Password == (cmd.Content as User).Password select x).First();
                if (usr != null)
                {
                    (from x in Settings.Sessions where x.Client == client select x).First().User = usr;
                    Communication.Send(client, new Command() { Type = PackageTypeEnum.Login, Content = usr });
                }
            }
        }

        private void Register(TcpClient client, Command cmd)
        {
            if (cmd.Content is User && (from x in Settings.Users where x.NickName == (cmd.Content as User).NickName select x).Count() == 0)
            {
                User usr = new User()
                {
                    NickName = (cmd.Content as User).NickName,
                    Password = (cmd.Content as User).Password,
                    Id = Guid.NewGuid()
                };
                Settings.Users.Add(usr);
                Communication.Send(client, new Command() { Type = PackageTypeEnum.Register, Content = usr });
            }
            else
                Communication.Send(client, new Command() { Type = PackageTypeEnum.Register, Content = null });
        }

        private void Alive(TcpClient client, Command cmd)
        {
            var session = (from x in Settings.Sessions
                           where x.User.Id == (cmd.Content as User).Id
                           select x).FirstOrDefault();

            if (session != null)
                session.LastActivity = DateTime.UtcNow;
        }
    }
}
