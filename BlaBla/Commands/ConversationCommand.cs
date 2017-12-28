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
    public class ConversationCommand : ICommand, ICommandFactory
    {
        public DataPackage Cmd { get; set; }
        public ServerPackageManager Manager { get; set; }
        public PackageTypeEnum Type { get => PackageTypeEnum.Conversation; }
        public TcpClient Client { get; set; }


        public void Execute()
        {
            DataPackage cmd = new DataPackage() { Type = PackageTypeEnum.Conversation, Content = Cmd.Content };

            List<Conversation> conv = (from x in Manager.Conversations
                                       where
                                       x.Sender.Id == ((cmd.Content as Conversation).Sender.Id) &&
                                       x.Sender.Id == ((cmd.Content as Conversation).Receiver.Id) &&
                                       x.Receiver.Id == ((cmd.Content as Conversation).Sender.Id) &&
                                       x.Receiver.Id == ((cmd.Content as Conversation).Receiver.Id)
                                       select x).ToList<Conversation>();
            DataPackage c = new DataPackage() { Type = PackageTypeEnum.Conversation, Content = conv };
            Manager.Communication.Send(Client, c);
        }

        ICommand ICommandFactory.MakeCommand(TcpClient Client, DataPackage Cmd, ServerPackageManager manager )
        {
            return new ConversationCommand() { Cmd = Cmd, Manager = manager ,Client = Client };
        }
    }
}
