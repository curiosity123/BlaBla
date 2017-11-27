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
    public class ServerCommandManager : IServerCommandManager
    {
        ServerSettings Settings;
        IServerCommunication Communication;
        List<Conversation> Conversations;

        public ServerCommandManager(ServerSettings settings, IServerCommunication communication, List<Conversation> conv)
        {
            Settings = settings;
            Communication = communication;
            Communication.PackageReceived += CommandProcessor;
            Conversations = conv;
        }

        private ServerCommandManager() { }



        public void CommandProcessor(TcpClient Client, Command Cmd)
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
                SendMessage(Client, Cmd);

            if (Cmd.Type == PackageTypeEnum.Conversation)
                SendConversation(Client, Cmd);
        }


        private void SendMessage(TcpClient client, Command cmd)
        {
            Command messageCmd = new Command() { Type = PackageTypeEnum.Message, Content = cmd.Content };
            foreach (User u in (cmd.Content as Message).UserList)
            {

                var cli = (from x in Settings.Sessions where x.User!=null && u.NickName == x.User.NickName select x).FirstOrDefault();
                if (cli != null)             
                    Communication.Send(cli.Client, messageCmd);

                    User currentUser = (from x in Settings.Sessions where x.Client == client select x).First().User;
                    Conversation conv = new Conversation()
                    {
                        Receiver = u,
                        Sender = currentUser,
                        Sentence = new Sentence()
                        {
                            TimeStamp = DateTime.UtcNow,
                            Text = (messageCmd.Content as Message).Text
                        }
                    };
                    Conversations.Add(conv);
                
            }
        }

        private void SendConversation(TcpClient client, Command cmd)
        {
            Command Cmd = new Command() { Type = PackageTypeEnum.Conversation, Content = cmd.Content };

            List<Conversation> conv = (from x in Conversations
                        where
                        x.Sender.Id == ((Cmd.Content as Conversation).Sender.Id) &&
                        x.Sender.Id == ((Cmd.Content as Conversation).Receiver.Id) &&
                        x.Receiver.Id == ((Cmd.Content as Conversation).Sender.Id) &&
                        x.Receiver.Id == ((Cmd.Content as Conversation).Receiver.Id)
                        select x).ToList<Conversation>();
            Command c = new Command() { Type = PackageTypeEnum.Conversation, Content = conv };
            Communication.Send(client, c);

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
                var usr = (from x in Settings.Users where x.NickName == (cmd.Content as User).NickName && x.Password == (cmd.Content as User).Password select x).FirstOrDefault();
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
                           where x.User!=null && 
                           x.User.Id == (cmd.Content as User).Id
                           select x).FirstOrDefault();

            if (session != null)
                session.LastActivity = DateTime.UtcNow;
        }
    }
}
