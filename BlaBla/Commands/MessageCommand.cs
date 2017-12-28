using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using BlaBlaServer;
using Common;
using Common.ICommandPattern;

namespace BlaBlaServer.Commands
{
    public class MessageCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public ServerPackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Message; }
        public TcpClient Client { get; set; }


        public void Execute()
        {
            DataPackage messageCmd = new DataPackage() { Type = PackageTypeEnum.Message, Content = Cmd.Content };
            foreach (User u in (Cmd.Content as Message).UserList)
            {

                var cli = (from x in Manager.Settings.Sessions where x.User != null && u.NickName == x.User.NickName select x).FirstOrDefault();
                if (cli != null)
                    Manager.Communication.Send(cli.Client, messageCmd);

                User currentUser = (from x in Manager.Settings.Sessions where x.Client == Client select x).First().User;
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
                Manager.Conversations.Add(conv);

            }
        }

        ICommand ICommandFactory.MakeCommand(TcpClient Client, DataPackage Cmd, ServerPackageManager manager )
        {
            return new MessageCommand() { Cmd = Cmd, Manager = manager ,Client = Client };
        }
    }
}
